using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using QJ.Communication.Core.Model.Result;

namespace QJ.Communication.Core.Interface
{
    /// <summary>
    /// 讀取介面(同步)
    /// </summary>
    public interface IVariableRead
    {
        /// <summary>
        /// 以位元組方式讀取指定變數區段的資料
        /// </summary>
        /// <param name="varFunc">變數區段名稱</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>讀取結果，包含位元組清單</returns>
        abstract QJResult<List<byte>> Read(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以布林值方式讀取指定變數區段的資料
        /// </summary>
        /// <param name="varFunc">變數區段名稱</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>讀取結果，包含布林值清單</returns>
        abstract QJResult<List<bool>> ReadBool(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以無號16位元整數方式讀取指定變數區段的資料
        /// </summary>
        /// <param name="varFunc">變數區段名稱</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>讀取結果，包含無號16位元整數清單</returns>
        abstract QJResult<List<ushort>> ReadUInt16(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以有號16位元整數方式讀取指定變數區段的資料
        /// </summary>
        /// <param name="varFunc">變數區段名稱</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>讀取結果，包含有號16位元整數清單</returns>
        abstract QJResult<List<short>> ReadInt16(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以無號32位元整數方式讀取指定變數區段的資料
        /// </summary>
        /// <param name="varFunc">變數區段名稱</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>讀取結果，包含無號32位元整數清單</returns>
        abstract QJResult<List<uint>> ReadUInt32(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以有號32位元整數方式讀取指定變數區段的資料
        /// </summary>
        /// <param name="varFunc">變數區段名稱</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>讀取結果，包含有號32位元整數清單</returns>
        abstract QJResult<List<int>> ReadInt32(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以無號64位元整數方式讀取指定變數區段的資料
        /// </summary>
        /// <param name="varFunc">變數區段名稱</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>讀取結果，包含無號64位元整數清單</returns>
        abstract QJResult<List<ulong>> ReadUInt64(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以有號64位元整數方式讀取指定變數區段的資料
        /// </summary>
        /// <param name="varFunc">變數區段名稱</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>讀取結果，包含有號64位元整數清單</returns>
        abstract QJResult<List<long>> ReadInt64(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以單精度浮點數方式讀取指定變數區段的資料
        /// </summary>
        /// <param name="varFunc">變數區段名稱</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>讀取結果，包含單精度浮點數清單</returns>
        abstract QJResult<List<float>> ReadFloat(string varFunc, ushort address, ushort length);

        /// <summary>
        /// 以雙精度浮點數方式讀取指定變數區段的資料
        /// </summary>
        /// <param name="varFunc">變數區段名稱</param>
        /// <param name="address">起始位址</param>
        /// <param name="length">讀取長度</param>
        /// <returns>讀取結果，包含雙精度浮點數清單</returns>
        abstract QJResult<List<double>> ReadDouble(string varFunc, ushort address, ushort length);
    }
}
