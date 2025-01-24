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
        abstract Task<QJResult<List<byte>>> ReadAsync(string varFunc, ushort address, ushort length);
        abstract Task<QJResult<List<bool>>> ReadBoolAsync(string varFunc, ushort address, ushort length);
        abstract Task<QJResult<List<ushort>>> ReadUInt16Async(string varFunc, ushort address, ushort length);
        abstract Task<QJResult<List<short>>> ReadInt16Async(string varFunc, ushort address, ushort length);
        abstract Task<QJResult<List<uint>>> ReadUInt32Async(string varFunc, ushort address, ushort length);
        abstract Task<QJResult<List<int>>> ReadInt32Async(string varFunc, ushort address, ushort length);
        abstract Task<QJResult<List<ulong>>> ReadUInt64Async(string varFunc, ushort address, ushort length);
        abstract Task<QJResult<List<long>>> ReadInt64Async(string varFunc, ushort address, ushort length);
        abstract Task<QJResult<List<float>>> ReadFloatAsync(string varFunc, ushort address, ushort length);
        abstract Task<QJResult<List<double>>> ReadDoubleAsync(string varFunc, ushort address, ushort length);
    }
}
