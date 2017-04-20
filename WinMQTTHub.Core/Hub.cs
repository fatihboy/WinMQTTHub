//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using Com.Enterprisecoding.WinMQTTHub.Core.Interaction;
using log4net;
using Ninject;
using System;
using System.Linq;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Ninject.Extensions.Conventions;
using System.Threading;
using System.Text;

namespace Com.Enterprisecoding.WinMQTTHub.Core
{
    public class Hub
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Hub));

        private readonly IKernel kernel;
        private readonly HubConfiguration hubConfiguration;
        private IEnumerable<Event> events;
        private readonly HubType hubType;
        private MqttClient client;

        public Hub(HubConfiguration hubConfiguration)
        {
            this.hubConfiguration = hubConfiguration;

            log.Debug("Initalizing Ninject kernel");
            kernel = new StandardKernel();

            log.DebugFormat("Binding commands. Assembly search pattern : {0}", hubConfiguration.ModuleAssemblyPattern);
            kernel.Bind(binder => binder.FromAssembliesMatching(hubConfiguration.ModuleAssemblyPattern)
                                        .SelectAllClasses().InheritedFrom<Command>().WithAttribute<CommandGroupAttribute>()
                                        .BindSelection((type, baseTypes) => new List<Type> { typeof(Command) })
                                        .Configure((binding, component) =>
                                        {
                                            var commandGroup = component.GetCustomAttributes(typeof(CommandGroupAttribute), false)[0] as CommandGroupAttribute;
                                            var name = string.Format("/{0}/{1}/commands/{2}",
                                                    hubConfiguration.TopicPrefix,
                                                    commandGroup.GroupName.ToLowerInvariant(),
                                                    component.Name.ToLowerInvariant());

                                            binding.Named(name);
                                        }));

            kernel.Bind(binder => binder.FromAssembliesMatching(hubConfiguration.ModuleAssemblyPattern)
                                        .SelectAllClasses().InheritedFrom<Event>()
                                        .BindSelection((type, baseTypes) => new List<Type> { typeof(Event) }));

            hubType = Environment.UserInteractive ? HubType.Interactive : HubType.NonInteractive;
            log.InfoFormat("Hub started in {0} mode", hubType);
        }

        public void Start()
        {
            try
            {
                log.Debug("Starting hub...");

                log.Debug("Retrieving events");
                events = kernel.GetAll<Event>();

                log.InfoFormat("{0} events to initialize", events.Count());
                foreach (var _event in events)
                {
                    log.DebugFormat("Initializing event : {0}", _event.GetType().Name);
                    _event.Initialize();
                }


                client = new MqttClient(hubConfiguration.BrokerUrl);
                client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

                while (!Connec2Server(client))
                {
                    log.DebugFormat("Waiting for {0} seconds to reconnect", hubConfiguration.ReconnectInterval);
                    Thread.Sleep(hubConfiguration.ReconnectInterval * 1000);
                }

                log.Debug("Extracting commands...");
                var commandBindings = kernel.GetBindings(typeof(Command));

                log.InfoFormat("{0} command binding found", commandBindings.Count());

                foreach (var commandBinding in commandBindings)
                {
                    try
                    {
                        log.DebugFormat("Subscribing to topic : {0}", commandBinding.Metadata.Name);
                        client.Subscribe(new String[] { commandBinding.Metadata.Name }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("Unable to subscribe to topic : {0}", commandBinding.Metadata.Name), ex);
                    }
                }

                var statusTopic = string.Format("/{0}/system/online", hubConfiguration.TopicPrefix);
                log.DebugFormat("Publishing online message to topic '{0}'", statusTopic);
                client.Publish(statusTopic, Encoding.UTF8.GetBytes("{ state:\"true\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            }
            catch (Exception ex)
            {
                log.Error("An error occured while starting the hub", ex);
            }
        }

        public void Stop()
        {
            try
            {
                log.Debug("Stopping hub...");

                var statusTopic = string.Format("/{0}/system/online", hubConfiguration.TopicPrefix);
                log.DebugFormat("Publishing offline message to topic '{0}'", statusTopic);
                client.Publish(statusTopic, Encoding.UTF8.GetBytes("{ state:\"false\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);


                if (client != null && client.IsConnected)
                {
                    log.Debug("MQTT client is connected. Stopping...");
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                log.Error("An error occured while stopping the hub", ex);
            }
        }

        private bool Connec2Server(MqttClient client)
        {
            log.DebugFormat("Connecting to broker '{0}' with client id '{1}'", hubConfiguration.BrokerUrl, hubConfiguration.ClientId);

            try
            {
                if (!string.IsNullOrWhiteSpace(hubConfiguration.BrokerUsername) && !string.IsNullOrWhiteSpace(hubConfiguration.BrokerPassword))
                {
                    client.Connect(hubConfiguration.ClientId, hubConfiguration.BrokerUsername, hubConfiguration.BrokerPassword);
                }
                else
                {
                    client.Connect(hubConfiguration.ClientId);
                }

                log.Debug("Connected to the MQTT server");

                return true;
            }
            catch (Exception ex)
            {
                log.Fatal("Unable to connect to MQTT server", ex);
                return false;
            }
        }

        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                var message = System.Text.Encoding.Default.GetString(e.Message);

                log.InfoFormat("MQTT message recieved from topic '{0}' : {1}", e.Topic, message);

                var command = kernel.Get<Command>(e.Topic);

                command.ExecuteCommand(message);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("un able to process message from topic '{0}'", e.Topic), ex);
            }
        }
    }
}