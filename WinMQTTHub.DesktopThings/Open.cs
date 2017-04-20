//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using Com.Enterprisecoding.WinMQTTHub.Core.Interaction;
using Com.Enterprisecoding.WinMQTTHub.DesktopThings.Parameters;
using log4net;
using System.ComponentModel;
using System.Diagnostics;

namespace Com.Enterprisecoding.WinMQTTHub.DesktopThings
{
    [CommandGroup("desktop")]
    public class Open : Command
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Open));

        protected override void Execute()
        {
            var parameter = GetParameter<OpenParameter>();

            if (parameter == null)
            {
                log.Warn("No open parameter deserialized. Nothing will be open...");
                return;
            }

            if (string.IsNullOrWhiteSpace(parameter.URI))
            {
                log.Warn("URI not specified. Nothing will be open...");
                return;
            }

            log.DebugFormat("Openning URI : {0}", parameter.URI);

            try
            {
                Process.Start(parameter.URI);
            }
            catch (Win32Exception)
            {
                Process.Start("IExplore.exe", parameter.URI);
            }
        }
    }
}