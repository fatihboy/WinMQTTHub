//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

namespace Com.Enterprisecoding.WinMQTTHub.Core
{
    public class HubConfiguration
    {
        private string moduleAssemblyPattern = "*.*Things.dll";
        private string brokerUrl = "tcp://localhost:1883";
        private string clientId = "WinMQTTHub";
        private string topicPrefix = "winmqtthub";
        private short reconnectInterval = 5;
        private string brokerUsername;
        private string brokerPassword;

        public string ModuleAssemblyPattern
        {
            get { return moduleAssemblyPattern; }
            set { moduleAssemblyPattern = value; }
        }

        public string BrokerUrl
        {
            get { return brokerUrl; }
            set { brokerUrl = value; }
        }

        public string ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }

        public string TopicPrefix
        {
            get { return topicPrefix; }
            set { topicPrefix = value; }
        }

        public short ReconnectInterval
        {
            get { return reconnectInterval; }
            set { reconnectInterval = value; }
        }

        public string BrokerUsername
        {
            get { return brokerUsername; }
            set { brokerUsername = value; }
        }

        public string BrokerPassword
        {
            get { return brokerPassword; }
            set { brokerPassword = value; }
        }
    }
}
