//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using System;
using System.Windows.Forms;
using Com.Enterprisecoding.WinMQTTHub.App.Properties;
using Com.Enterprisecoding.WinMQTTHub.Core;
using log4net;
using System.Configuration;

namespace Com.Enterprisecoding.WinMQTTHub.App
{
    internal class WinMQTTHubApplicationContext : ApplicationContext
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WinMQTTHubApplicationContext));
        private readonly NotifyIcon trayIcon;
        private readonly Hub hub;

        public WinMQTTHubApplicationContext()
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

            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Exit", Exit)
            }),
                Visible = true
            };
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
            if (ConfigurationManager.AppSettings[key]!=null)
            {
                return ConfigurationManager.AppSettings[key];
            }

            return defaultValue;
        }

        void Exit(object sender, EventArgs e)
        {
            if (hub != null)
            {
                hub.Stop();
            }

            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            log.Debug("Application exited...");

            Application.Exit();
        }
    }
}