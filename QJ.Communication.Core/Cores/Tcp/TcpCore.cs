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

#if NETFRAMEWORK
using System.Windows.Forms;
#endif

namespace QJ.Communication.Core.Cores.Tcp
{
    /// <summary>
    /// Tcp核心接口
    /// </summary>
    public class TcpCore
    {
        public QJTcpPluginBase qJPlugin;
        public string PluginName {  get; }

        public string IpAddress {  get; set; }

        public int Port {  get; set; }

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

                var _qJPlugin = PluginHelper.Instance.Plugins[_plugName].Clone() as QJPluginBase;
                if (_qJPlugin is QJTcpPluginBase tcpBase)
                {
                    qJPlugin = tcpBase;
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
 
        public string Version() => qJPlugin.Version;
        public QJTcpPluginBase GetPluginBase() => qJPlugin;

        public void Connect()
        {
            Connect(IpAddress, Port);
        }
        public void Connect(string ip, int port)
        {
            IpAddress = ip;
            Port = port;
            var qjRes = qJPlugin.Connect(ip,port);
        }

        public void Disconnect()
        {
            qJPlugin.Disconnect();
        }
     

        public Task ConnectAsync(string ip, int port)
        {
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            return Task.CompletedTask;
        }
    }
}
