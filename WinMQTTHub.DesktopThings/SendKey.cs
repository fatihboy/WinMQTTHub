//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using Com.Enterprisecoding.WinMQTTHub.Core.Interaction;
using Com.Enterprisecoding.WinMQTTHub.DesktopThings.Parameters;
using log4net;

namespace Com.Enterprisecoding.WinMQTTHub.DesktopThings
{
    [CommandGroup("keyboard")]
    public class SendKey : Command
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Run));

        protected override void Execute()
        {
            var parameter = GetParameter<SendKeyParameter>();

            if (parameter == null)
            {
                log.Warn("No send key parameter deserialized. No key will be send...");
                return;
            }

            if (string.IsNullOrWhiteSpace(parameter.Key))
            {
                log.Warn("Key not specified. No key will be send...");
                return;
            }

            System.Windows.Forms.SendKeys.Send(parameter.Key);
        }
    }
}
