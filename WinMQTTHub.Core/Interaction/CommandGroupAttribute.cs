//  
// Copyright (c) Fatih Boy. All rights reserved.  
// Licensed under the GNU LESSER GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.  
//

using System;

namespace Com.Enterprisecoding.WinMQTTHub.Core.Interaction
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CommandGroupAttribute : Attribute
    {
        internal string GroupName { get; set; }

        public CommandGroupAttribute(string groupName)
        {
            this.GroupName = groupName;
        }
    }
}