using QJ.Communication.Core.Model.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QJ.Communication.Core.Interface
{
    /// <summary>
    /// 讀取介面(非同步)
    /// </summary>
    public interface IVariableReadAsync
    {
        /// <summary>
        /// 以非同步方式讀取原始位元組資料。
        /// </summary>
        /// <param name="varFunc">變數功能碼</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>包含位元組資料的結果</returns>
        abstract Task<QJResult<List<byte>>> ReadAsync(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以非同步方式讀取布林值資料。
        /// </summary>
        /// <param name="varFunc">變數功能碼</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>包含布林值資料的結果</returns>
        abstract Task<QJResult<List<bool>>> ReadBoolAsync(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以非同步方式讀取 UInt16 資料。
        /// </summary>
        /// <param name="varFunc">變數功能碼</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>包含 UInt16 資料的結果</returns>
        abstract Task<QJResult<List<ushort>>> ReadUInt16Async(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以非同步方式讀取 Int16 資料。
        /// </summary>
        /// <param name="varFunc">變數功能碼</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>包含 Int16 資料的結果</returns>
        abstract Task<QJResult<List<short>>> ReadInt16Async(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以非同步方式讀取 UInt32 資料。
        /// </summary>
        /// <param name="varFunc">變數功能碼</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>包含 UInt32 資料的結果</returns>
        abstract Task<QJResult<List<uint>>> ReadUInt32Async(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以非同步方式讀取 Int32 資料。
        /// </summary>
        /// <param name="varFunc">變數功能碼</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>包含 Int32 資料的結果</returns>
        abstract Task<QJResult<List<int>>> ReadInt32Async(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以非同步方式讀取 UInt64 資料。
        /// </summary>
        /// <param name="varFunc">變數功能碼</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>包含 UInt64 資料的結果</returns>
        abstract Task<QJResult<List<ulong>>> ReadUInt64Async(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以非同步方式讀取 Int64 資料。
        /// </summary>
        /// <param name="varFunc">變數功能碼</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>包含 Int64 資料的結果</returns>
        abstract Task<QJResult<List<long>>> ReadInt64Async(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以非同步方式讀取單精度浮點數 (float) 資料。
        /// </summary>
        /// <param name="varFunc">變數功能碼</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>包含 float 資料的結果</returns>
        abstract Task<QJResult<List<float>>> ReadFloatAsync(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以非同步方式讀取雙精度浮點數 (double) 資料。
        /// </summary>
        /// <param name="varFunc">變數功能碼</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>包含 double 資料的結果</returns>
        abstract Task<QJResult<List<double>>> ReadDoubleAsync(string varFunc, ushort address, ushort length);
    }
}
