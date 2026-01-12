using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QJ.Communication.Core.Enums
{
    public enum QJDataTypeEnum
    {
        
        Boolean = 0,
        UINT16 = 1,
        INT16 = 2,
        UINT32 = 3,
        INT32 = 4,
        UINT64 = 5,
        INT64 = 6,
        FLOAT = 7,
        DOUBLE = 8,
        STRING = 9,

        BooleanArray = 10,
        UINT16Array = 11,
        INT16Array = 12,
        UINT32Array = 13,
        INT32Array = 14,
        UINT64Array = 15,
        INT64Array = 16,
        FLOATArray = 17,
        DOUBLEArray = 18,
        STRINGArray = 19,

        OTHER = 99,
        Empty = 999,
    }
}
