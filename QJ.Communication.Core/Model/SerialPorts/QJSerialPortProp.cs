using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QJ.Communication.Core.Model.SerialPorts
{
    public class QJSerialPortProp
    {
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; } = 9600;

        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits { get; set; } = 8;

        /// <summary>
        /// 校验位
        /// </summary>
        public Parity Parity { get; set; } = Parity.None;

        /// <summary>
        /// COM
        /// </summary>
        public string PortName { get; set; } = "COM1";

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits { get; set; } = StopBits.One;

        ///<inheritdoc cref = "SerialPort.Handshake" />
        public Handshake Handshake { get; set; }

        ///<inheritdoc cref = "SerialPort.DtrEnable" />
        public bool DtrEnable { get; set; }

        ///<inheritdoc cref = "SerialPort.RtsEnable" />
        public bool RtsEnable { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.PortName}[{this.BaudRate},{this.DataBits},{this.StopBits},{this.Parity}]";
        }
    }
}
