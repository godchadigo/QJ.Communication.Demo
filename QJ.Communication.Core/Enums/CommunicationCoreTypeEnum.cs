using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QJ.Communication.Core.Enums
{
    public enum CommunicationCoreTypeEnum
    {
        [Description("未定義的核心類型")]
        Unknown,
        [Description("Tcp核心")]
        Tcp,
        [Description("Udp核心")]
        Udp,
        [Description("SerialPort核心")]
        SerialPort,
    }
}
