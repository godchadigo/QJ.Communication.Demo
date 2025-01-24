using QJ.Communication.Core.Model.SerialPorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.SerialPorts;

namespace QJ.Communication.Core.Extension
{
    public static class PropExtension
    {
        public static QJSerialPortProp ToQJSerialPortProp(this SerialPortOption config)
        {
            QJSerialPortProp prop = new QJSerialPortProp() { 
                PortName = config.PortName,
                BaudRate = config.BaudRate,
                DataBits = config.DataBits,
                Parity = config.Parity,
                StopBits = config.StopBits,
                DtrEnable = config.DtrEnable,
                RtsEnable = config.RtsEnable,
                Handshake = config.Handshake,
            };
            return prop;
        }

        
        public static SerialPortOption ToTouchsocketSerialPortProp(this QJSerialPortProp config)
        {
            SerialPortOption prop = new SerialPortOption()
            {
                PortName = config.PortName,
                BaudRate = config.BaudRate,
                DataBits = config.DataBits,
                Parity = config.Parity,
                StopBits = config.StopBits,
                DtrEnable = config.DtrEnable,
                RtsEnable = config.RtsEnable,
                Handshake = config.Handshake,
            };
            return prop;
        }
    }
}
