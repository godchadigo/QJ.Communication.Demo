using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;

namespace QJ.Communication.Core.Interface
{
    /// <summary>
    /// 基本插件介面
    /// </summary>
    public interface IQJPlugin
    {
        /// <summary>
        /// 插件名稱
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 插件版本
        /// </summary>
        string Version {  get; }
        /// <summary>
        /// 插件作者
        /// </summary>
        string Author {  get; }
        /// <summary>
        /// 是否啟用該插件
        /// </summary>
        //bool IsEnabled { get; set; }
        /// <summary>
        /// 插件是否初始化完成
        /// </summary>
        bool IsInitialized { get; }
        /// <summary>
        /// 插件通訊類型
        /// </summary>
        CommunicationEnum communicationType { get; }
        /// <summary>
        /// 插件註解
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 插件初始化方法(同步)
        /// </summary>
        void Initialize();
        /// <summary>
        /// 插件初始化方法(非同步)
        /// </summary>
        /// <returns></returns>
        Task InitializeAsync();
        /// <summary>
        /// 插件通訊端序
        /// </summary>
        EndianType EndianType { get; set; }
    }
    /// <summary>
    /// 通訊類型
    /// </summary>
    public enum CommunicationEnum
    {
        [Description("未定義")]
        None,
        [Description("Tcp網路")]
        Tcp,
        [Description("Udp網路")]
        Udp,
        [Description("串列阜")]
        Serial
    }
}
