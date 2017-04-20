//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using log4net;
using Newtonsoft.Json;

namespace Com.Enterprisecoding.WinMQTTHub.Core.Interaction
{
    public abstract class Command
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Command));
        private string message;

        internal void ExecuteCommand(string message)
        {
            this.message = message;

            Execute();
        }

        protected ParameterType GetParameter<ParameterType>()
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                log.Debug("Message is empty, returning default parameter type");

                return default(ParameterType);
            }

            try
            {
                return JsonConvert.DeserializeObject<ParameterType>(message);
            }
            catch (System.Exception ex)
            {
                log.Error(string.Format("Unable to deserialize message : {0}", message), ex);

                return default(ParameterType);
            }
        }

        protected abstract void Execute();
    }
}