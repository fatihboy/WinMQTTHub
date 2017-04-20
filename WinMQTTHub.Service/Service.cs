//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using Com.Enterprisecoding.WinMQTTHub.Core;
using System.Configuration;
using System.ServiceProcess;

namespace Com.Enterprisecoding.WinMQTTHub.Service
{
    public partial class Service : ServiceBase
    {
        private Hub hub;

        public Service()
        {
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {
            var hubConfiguration = new HubConfiguration();

            hubConfiguration.BrokerPassword = GetConfiguration("BrokerPassword", hubConfiguration.BrokerPassword);
            hubConfiguration.BrokerUrl = GetConfiguration("BrokerUrl", hubConfiguration.BrokerUrl);
            hubConfiguration.BrokerUsername = GetConfiguration("BrokerUsername", hubConfiguration.BrokerUsername);
            hubConfiguration.ClientId = GetConfiguration("ClientId", hubConfiguration.ClientId);
            hubConfiguration.ModuleAssemblyPattern = GetConfiguration("ModuleAssemblyPattern", hubConfiguration.ModuleAssemblyPattern);
            hubConfiguration.ReconnectInterval = GetConfigurationAsShort("ReconnectInterval", hubConfiguration.ReconnectInterval);
            hubConfiguration.TopicPrefix = GetConfiguration("TopicPrefix", hubConfiguration.TopicPrefix);

            hub = new Hub(hubConfiguration);
            hub.Start();
        }

        protected override void OnStop()
        {
            if (hub != null)
            {
                hub.Stop();
            }
        }

        private short GetConfigurationAsShort(string key, short defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return short.Parse(ConfigurationManager.AppSettings[key]);
            }

            return defaultValue;
        }

        private string GetConfiguration(string key, string defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigurationManager.AppSettings[key];
            }

            return defaultValue;
        }
    }
}
