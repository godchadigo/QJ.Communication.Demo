using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QJ.Communication.Core.Enum;
using QJ.Communication.Core.Interface;
using QJ.Communication.Core.Model.Result;
using TouchSocket.Core;

namespace QJ.Communication.Core.Model.Plugin
{
    /// <summary>
    /// 插件基類
    /// </summary>
    public abstract class QJPluginBase : IQJPlugin, ICloneable
    {
        #region IQJPlugin
        /// <summary>
        /// 插件名稱
        /// </summary>
        public abstract string Name { get; set; }
        /// <summary>
        /// 插件版本
        /// </summary>
        public virtual string Version => "作者很懶連版本都沒打~";
        /// <summary>
        /// 插件作者
        /// </summary>

        public virtual string Author => "作者很懶連名子都沒打~";
        /// <summary>
        /// 插件註解
        /// </summary>
        public virtual string Description => "";
        /// <summary>
        /// 是否啟用插件
        /// </summary>
        protected virtual bool IsEnabled { get; set; } = true;
        /// <summary>
        /// 是否已經初始化完成
        /// </summary>
        public virtual bool IsInitialized { get; set; }
        /// <summary>
        /// 是否顯示通訊封包
        /// </summary>
        public virtual bool IsShowRequestMessage { get; set; } = false;
        /// <summary>
        /// 插件通訊類型
        /// </summary>
        public virtual CommunicationEnum communicationType { get; set; } = CommunicationEnum.None;
        
        /// <summary>
        /// 端序，預設小端序
        /// </summary>
        public virtual EndianType EndianType { get; set; } = EndianType.Big;
        public bool GetEnable()
        {
            return IsEnabled;
        }
        /// <summary>
        /// 設定通訊端序
        /// </summary>
        /// <param name="type"></param>
        public void SetEndianType(EndianEnum.EndianType type)
        {
            switch (type)
            {
                case EndianEnum.EndianType.ABCD:
                    EndianType = EndianType.Big;
                    break;
                case EndianEnum.EndianType.BADC:
                    EndianType = EndianType.BigSwap;
                    break;
                case EndianEnum.EndianType.DCBA:
                    EndianType = EndianType.Little;
                    break;
                case EndianEnum.EndianType.CDAB:
                    EndianType = EndianType.LittleSwap;
                    break;
            }
        }

        public abstract void Initialize();

        public abstract Task InitializeAsync();

        /// <summary>
        /// 拷貝插件本體
        /// 這是必要的否則使用者產生的實例會不斷被新的覆蓋
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            // 創建深拷貝
            var clone = this.MemberwiseClone() as QJPluginBase;
            // 處理需要深拷貝的成員
            //clone.Initialize();
            //clone.InitializeAsync();
            return clone;
        }

        #endregion
    }
}
