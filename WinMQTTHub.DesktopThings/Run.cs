//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using Com.Enterprisecoding.WinMQTTHub.Core.Interaction;
using Com.Enterprisecoding.WinMQTTHub.DesktopThings.Parameters;
using log4net;
using System.Diagnostics;

namespace Com.Enterprisecoding.WinMQTTHub.DesktopThings
{
    [CommandGroup("desktop")]
    public class Run : Command
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Run));

        protected override void Execute()
        {
            var parameter = GetParameter<RunParameter>();

            if (parameter == null)
            {
                log.Warn("No run parameter deserialized. Nothing will be run...");
                return;
            }

            if (string.IsNullOrWhiteSpace(parameter.Command))
            {
                log.Warn("Command not specified. Nothing will be run...");
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(parameter.Command)
            {
                UseShellExecute = true
            };

            if (!string.IsNullOrWhiteSpace(parameter.Argument))
            {
                startInfo.Arguments = parameter.Argument;
            }

            if (!string.IsNullOrWhiteSpace(parameter.WorkingDirectory))
            {
                startInfo.WorkingDirectory = parameter.WorkingDirectory;
            }

            log.DebugFormat("Running '{0}' with parameter '{1}' on directory '{2}'", parameter.Command, parameter.Argument, parameter.WorkingDirectory);

            Process.Start(startInfo);
        }
    }
}
