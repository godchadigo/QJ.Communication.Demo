using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QJ.Communication.Core.Enums;
using QJ.Communication.Core.Interface;
using QJ.Communication.Core.Model.Plugin;
using QJ.Communication.Core.Model.Result;
using TouchSocket.Core;

namespace QJ.Communication.Core.Cores
{
    public abstract class CommunicationCore : ICommunicationCore
    {
        public virtual string Version { get; set; } = "未知的核心版本";
        public virtual CommunicationCoreTypeEnum CoreType { get; set; } = CommunicationCoreTypeEnum.Unknown;
        public string PluginName { get; set; }
        public QJPluginBase qJPlugin { get; set; }        
        public bool IsOnline => qJPlugin.IsOnline;
        public virtual bool UseDebug { get; set; } = false;
    }
}
