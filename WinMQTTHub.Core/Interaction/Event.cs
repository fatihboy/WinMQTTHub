//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using log4net;

namespace Com.Enterprisecoding.WinMQTTHub.Core.Interaction
{
    public abstract class Event
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Event));

        internal protected abstract void Initialize();

        protected void PublishMessage(object message) {
            var typeName = GetType().Name;
            log.DebugFormat("Message received for event : {0}", typeName);
        }
    }
}
