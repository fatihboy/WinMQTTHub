//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using log4net;
using System;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace Com.Enterprisecoding.WinMQTTHub.Service
{
    static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            log.Debug("Application started...");

            ServiceBase[] servicesToRun = new ServiceBase[]
            {
                new Service()
            };

            if (Environment.UserInteractive)
            {
                log.Debug("Starting as a console application");
                RunInteractive(servicesToRun);
            }
            else
            {
                log.Debug("Starting as a service");
                ServiceBase.Run(servicesToRun);
            }

            log.Debug("Application exited...");
        }

        /// <summary>
        /// Function to start service in interactive mode
        /// </summary>
        /// <param name="servicesToRun"></param>
        /// <remarks>
        /// All credits to : Anders Abel
        /// https://coding.abel.nu/2012/05/debugging-a-windows-service-project/
        /// </remarks>
        static void RunInteractive(ServiceBase[] servicesToRun)
        {
            log.Debug("Services running in interactive mode.");

            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                log.DebugFormat("Starting {0}...", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                log.Debug("Started");
            }

            //Console.WriteLine("Press any key to stop the services and end the process...");
            //Console.ReadKey();

            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                log.DebugFormat("Stopping {0}...", service.ServiceName);
                onStopMethod.Invoke(service, null);
                log.Debug("Stopped");
            }

            log.Debug("All services stopped.");
            // Keep the console alive for a second to allow the user to see the message.
            Thread.Sleep(1000);
        }
    }
}
