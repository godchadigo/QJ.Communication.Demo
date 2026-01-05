using QJ.Communication.Core.Enums;
using QJ.Communication.Core.Model.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QJ.Communication.Core.Cores
{
    public interface ICommunicationCore
    {
        /// <summary>
        /// 核心版本
        /// </summary>
        abstract string Version { get; set; }
        /// <summary>
        /// 核心類型
        /// </summary>
        CommunicationCoreTypeEnum CoreType { get; }
        /// <summary>
        /// 綁定的插件名稱
        /// </summary>
        string PluginName { get; set; }
        /// <summary>
        /// 插件實例
        /// </summary>
        QJPluginBase qJPlugin { get; set; }
        /// <summary>
        /// 插件通訊的設備是否在線
        /// </summary>
        bool IsOnline { get; }      
        bool UseDebug { get; set; }
    }
}
