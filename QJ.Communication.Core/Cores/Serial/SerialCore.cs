using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QJ.Communication.Core.Model.Plugin;
using QJ.Communication.Core.Helper;
using QJ.Communication.Core.Model.SerialPorts;
using QJ.Communication.Core.Extension;

namespace QJ.Communication.Core.Cores.Serial
{
    public class SerialCore
    {
        public QJSerialPluginBase qJPlugin;
        public string PluginName { get; }

        public QJSerialPortProp serialProp { get; set; }

        public SerialCore(string _plugName, out bool _res)
        {
            PluginName = _plugName;
            try
            {
                var _qJPlugin = PluginHelper.Instance.Plugins[_plugName];
                if (_qJPlugin is QJSerialPluginBase serialBase)
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

        public string Version() => qJPlugin.Version;
        public QJSerialPluginBase GetPluginBase() => qJPlugin;

        public void Connect(QJSerialPortProp prop)
        {
            qJPlugin.Connect(prop.ToTouchsocketSerialPortProp());
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

        public byte[] Send(byte[] packet)
        {

            return new byte[0];
        }
    }
}
