using QJ.Communication.Core.Model.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QJ.Communication.Core.Enums.EncodingTypeEnum;

namespace QJ.Communication.Core.Interface
{
    /// <summary>
    /// 寫入介面(同步)
    /// </summary>
    public interface IVariableWrite
    {
        #region bool
        abstract QJResult Write(string varFunc, ushort address, bool value);
        abstract QJResult Write(string varFunc, ushort address, bool[] values);
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<bool> values);
        #endregion

        #region ushort
        abstract QJResult Write(string varFunc, ushort address, ushort value);
        abstract QJResult Write(string varFunc, ushort address, ushort[] values);
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<ushort> values);
        #endregion

        #region short
        abstract QJResult Write(string varFunc, ushort address, short value);
        abstract QJResult Write(string varFunc, ushort address, short[] values);
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<short> values);
        #endregion

        #region uint
        abstract QJResult Write(string varFunc, ushort address, uint value);
        abstract QJResult Write(string varFunc, ushort address, uint[] values);
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<uint> values);
        #endregion

        #region int
        abstract QJResult Write(string varFunc, ushort address, int value);
        abstract QJResult Write(string varFunc, ushort address, int[] values);
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<int> values);
        #endregion

        #region ulong
        abstract QJResult Write(string varFunc, ushort address, ulong value);
        abstract QJResult Write(string varFunc, ushort address, ulong[] values);
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<ulong> values);
        #endregion

        #region long
        abstract QJResult Write(string varFunc, ushort address, long value);
        abstract QJResult Write(string varFunc, ushort address, long[] values);
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<long> values);
        #endregion

        #region float
        abstract QJResult Write(string varFunc, ushort address, float value);
        abstract QJResult Write(string varFunc, ushort address, float[] values);
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<float> values);
        #endregion

        #region double
        abstract QJResult Write(string varFunc, ushort address, double value);
        abstract QJResult Write(string varFunc, ushort address, double[] values);
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<double> values);
        #endregion

        #region string
        abstract QJResult Write(string varFunc, ushort address, string str, EncodingType encode);

        #endregion

    }
}
