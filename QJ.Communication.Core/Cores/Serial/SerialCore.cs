using QJ.Communication.Core.Enums;
using QJ.Communication.Core.Extension;
using QJ.Communication.Core.Helper;
using QJ.Communication.Core.Model.Plugin;
using QJ.Communication.Core.Model.Result;
using QJ.Communication.Core.Model.SerialPorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.SerialPorts;
using System.Net;
using System.Threading;


#if NETFRAMEWORK
using System.Windows.Forms;
#endif

namespace QJ.Communication.Core.Cores.Serial
{
    public class SerialCore: CommunicationCore
    {
        public override string Version => "1.0.0";
        public override CommunicationCoreTypeEnum CoreType => CommunicationCoreTypeEnum.SerialPort;
        public QJSerialPortProp serialProp { get; set; }
        public bool IsOnline => qJPlugin.IsOnline;

        public SerialCore(string _plugName, out bool _res)
        {
            base.PluginName = _plugName;
            try
            {
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
                var newInstance = Activator.CreateInstance(prototype.GetType()) as QJSerialPluginBase;

                //取消克隆方案
                //var _qJPlugin = PluginHelper.Instance.Plugins[_plugName];
                if (newInstance is QJSerialPluginBase serialBase)
                {
                    qJPlugin = serialBase;
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
        public QJSerialPluginBase GetPluginBase() => base.qJPlugin as QJSerialPluginBase;

        public QJResult<QJSerialPortClient> Connect()
        {
            if (qJPlugin is QJSerialPluginBase serialPlugin)
            {
                if (serialProp == null)
                {
                    return QJExtension.QJDataResponse<QJSerialPortClient>(null, "串口屬性未設置!", false);
                }
                return serialPlugin.Connect(serialProp);
            }
            return QJExtension.QJDataResponse<QJSerialPortClient>(null, "串口連接失敗!", false);
        }
        public QJResult<QJSerialPortClient> Connect(QJSerialPortProp prop)
        {
            if (qJPlugin is QJSerialPluginBase serialPlugin)
            {
                return serialPlugin.Connect(prop);
            }
            return QJExtension.QJDataResponse<QJSerialPortClient>(null,"串口連接失敗!",false);
        }

        public void Disconnect()
        {
            if (qJPlugin is QJSerialPluginBase serialPlugin)
            {
                serialPlugin.Disconnect();
            }
        }

        public async Task<QJResult> ConnectAsync(int timeout)
        {
            var res = await ConnectAsync(serialProp, timeout);
            return QJExtension.QJDataResponse(res.IsOk);
        }
        public async Task<QJResult> ConnectAsync(QJSerialPortProp props, int timeout)
        {
            
            if (qJPlugin is QJSerialPluginBase serialPlugin)
            {
                serialProp = props;
                var res = await serialPlugin.ConnectAsync(props, timeout);
                return QJExtension.QJDataResponse(res.IsOk);
            }

            return new QJResult() { IsOk = false };
        }

        public Task DisconnectAsync()
        {
            return Task.CompletedTask;
        }

        public byte[] Send(byte[] packet)
        {

            return new byte[0];
        }
    }
}
