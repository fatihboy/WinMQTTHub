//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using Com.Enterprisecoding.WinMQTTHub.Core.Interaction;
using log4net;
using System.Management;

namespace Com.Enterprisecoding.WinMQTTHub.SystemThings
{
    [CommandGroup("system")]
    public class Shutdown : Command
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Shutdown));

        protected override void Execute() {
            log.Info("Shutting down the system");

            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();

            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams = mcWin32.GetMethodParameters("Win32Shutdown");
            
            mboShutdownParams["Flags"] = "1"; //Shutdown
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown", mboShutdownParams, null);
            }
        }
    }
}
