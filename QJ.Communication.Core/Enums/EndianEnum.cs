using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QJ.Communication.Core.Enum
{
    /// <summary>
    /// 端序
    /// </summary>
    public class EndianEnum
    {
        public enum QJEndianType
        {
            [Description("小端模式DCBA")]
            DCBA,
            [Description("大端模式ABCD")]            
            ABCD,
            [Description("反小端模式CDAB")]
            CDAB,
            [Description("反大端模式BADC")]
            BADC,
        }
    }
}
