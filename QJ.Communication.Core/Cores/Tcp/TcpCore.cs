using QJ.Communication.Core.Helper;
using QJ.Communication.Core.Interface;
using QJ.Communication.Core.Model.Plugin;
using QJ.Communication.Core.Model.Result;
using QJ.Communication.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using QJ.Communication.Core.Enums;
using QJ.Communication.Core.Extension;


#if NETFRAMEWORK
using System.Windows.Forms;
#endif

namespace QJ.Communication.Core.Cores.Tcp
{
    /// <summary>
    /// Tcp核心接口
    /// </summary>
    public class TcpCore: CommunicationCore
    {
        public override string Version => "1.0.0";
        public override CommunicationCoreTypeEnum CoreType => CommunicationCoreTypeEnum.Tcp;
        public string IpAddress {  get; set; }
        public int Port {  get; set; }
        public bool IsOnline => qJPlugin.IsOnline;        


        public TcpCore(string _plugName, out bool _res) 
        {
            PluginName = _plugName;
            try
            {
                //var _qJPlugin = PluginHelper.Instance.Plugins[_plugName];
                if (!PluginHelper.Instance.Plugins.ContainsKey(_plugName))
                {
#if NETFRAMEWORK
                    MessageBox.Show($"找不到插件{_plugName}，請檢察名稱是否正確，或是該插件已經被鎖定!");
#else
                    Console.WriteLine($"找不到插件{_plugName}，請檢察名稱是否正確，或是該插件已經被鎖定!");         
#endif
                   
                    _res = false;
                    return;
                }

                // 更改為直接使用新的實例而不是克隆插件本體
                // 直接獲取插件
                var prototype = PluginHelper.Instance.Plugins[_plugName];
                // 創建全新插件實例
                var newInstance = Activator.CreateInstance(prototype.GetType()) as QJTcpPluginBase;
                
                // 取消使用克隆方案
                //var qJPluginClone = PluginHelper.Instance.Plugins[_plugName].Clone() as QJPluginBase;
                if (newInstance is QJTcpPluginBase tcpBase)
                {
                    this.qJPlugin = tcpBase;
                    _res = true;
                    return;
                }
                _res = false;
            }
            catch (Exception ex)
            {
                _res = false;
            }                        
        }
 
        //public string Version() => qJPlugin.Version;
        public  QJTcpPluginBase GetPluginBase() => base.qJPlugin as QJTcpPluginBase;

        public async Task<QJResult> ConnectAsync(int timeout=1000)
        {
            var res = await ConnectAsync(IpAddress, Port, timeout);
            return QJExtension.QJDataResponse(res.IsOk);
        }
        public async Task<QJResult> ConnectAsync(string ip, int port, int timeout=1000)
        {
            IpAddress = ip;
            Port = port;
            if (qJPlugin is QJTcpPluginBase tcpPlugin)
            {
                var res = await tcpPlugin.ConnectAsync(IpAddress, Port, timeout);
                return QJExtension.QJDataResponse(res.IsOk);                
            }

            return new QJResult() { IsOk = false };
        }

        public Task DisconnectAsync()
        {
            return Task.CompletedTask;
        }
    }
}
