//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using log4net;
using System;
using System.Windows.Forms;

namespace Com.Enterprisecoding.WinMQTTHub.App
{
    static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log.Debug("Application started...");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WinMQTTHubApplicationContext());
        }
    }
}
