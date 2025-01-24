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
    /// 寫入介面(非同步)
    /// </summary>
    public interface IVariableWriteAsync
    {
        #region bool
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, bool value);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, bool[] values);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<bool> values);
        #endregion

        #region ushort
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, ushort value);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, ushort[] values);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<ushort> values);
        #endregion

        #region short
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, short value);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, short[] values);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<short> values);
        #endregion

        #region uint
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, uint value);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, uint[] values);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<uint> values);
        #endregion

        #region int
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, int value);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, int[] values);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<int> values);
        #endregion

        #region ulong
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, ulong value);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, ulong[] values);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<ulong> values);
        #endregion

        #region long
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, long value);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, long[] values);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<long> values);
        #endregion

        #region float
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, float value);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, float[] values);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<float> values);
        #endregion

        #region double
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, double value);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, double[] values);
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<double> values);
        #endregion

        #region string
        abstract Task<QJResult> WriteAsync(string varFunc, ushort address, string str, EncodingType encode);

        #endregion
    }
}
