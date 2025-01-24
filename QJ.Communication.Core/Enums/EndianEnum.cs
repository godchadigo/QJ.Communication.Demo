using System;
using System.Collections.Generic;
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
        public enum EndianType
        {
            ABCD = 0,
            CDAB = 1,
            BADC = 2,
            DCBA = 3,
        }
    }
}
