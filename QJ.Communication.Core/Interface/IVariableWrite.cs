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
        /// <summary>
        /// 寫入單一 bool 值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, bool value);
        /// <summary>
        /// 寫入多個 bool 陣列值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, bool[] values);
        /// <summary>
        /// 寫入多個 bool 集合值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<bool> values);
        #endregion

        #region ushort
        /// <summary>
        /// 寫入單一 ushort 值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, ushort value);
        /// <summary>
        /// 寫入多個 ushort 陣列值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, ushort[] values);
        /// <summary>
        /// 寫入多個 ushort 集合值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<ushort> values);
        #endregion

        #region short
        /// <summary>
        /// 寫入單一 short 值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, short value);
        /// <summary>
        /// 寫入多個 short 陣列值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, short[] values);
        /// <summary>
        /// 寫入多個 short 集合值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<short> values);
        #endregion

        #region uint
        /// <summary>
        /// 寫入單一 uint 值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, uint value);
        /// <summary>
        /// 寫入多個 uint 陣列值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, uint[] values);
        /// <summary>
        /// 寫入多個 uint 集合值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<uint> values);
        #endregion

        #region int
        /// <summary>
        /// 寫入單一 int 值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, int value);
        /// <summary>
        /// 寫入多個 int 陣列值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, int[] values);
        /// <summary>
        /// 寫入多個 int 集合值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<int> values);
        #endregion

        #region ulong
        /// <summary>
        /// 寫入單一 ulong 值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, ulong value);
        /// <summary>
        /// 寫入多個 ulong 陣列值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, ulong[] values);
        /// <summary>
        /// 寫入多個 ulong 集合值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<ulong> values);
        #endregion

        #region long
        /// <summary>
        /// 寫入單一 long 值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, long value);
        /// <summary>
        /// 寫入多個 long 陣列值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, long[] values);
        /// <summary>
        /// 寫入多個 long 集合值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<long> values);
        #endregion

        #region float
        /// <summary>
        /// 寫入單一 float 值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, float value);
        /// <summary>
        /// 寫入多個 float 陣列值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, float[] values);
        /// <summary>
        /// 寫入多個 float 集合值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<float> values);
        #endregion

        #region double
        /// <summary>
        /// 寫入單一 double 值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, double value);
        /// <summary>
        /// 寫入多個 double 陣列值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, double[] values);
        /// <summary>
        /// 寫入多個 double 集合值到指定地址。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, IEnumerable<double> values);
        #endregion

        #region string
        /// <summary>
        /// 寫入字串到指定地址，並指定編碼方式。
        /// </summary>
        abstract QJResult Write(string varFunc, ushort address, string str, EncodingType encode);
        #endregion

    }
}
