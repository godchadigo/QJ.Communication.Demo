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
        abstract QJResult<List<byte>> Read(string varFunc, ushort address, ushort length);
        abstract QJResult<List<bool>> ReadBool(string varFunc, ushort address, ushort length);
        abstract QJResult<List<ushort>> ReadUInt16(string varFunc, ushort address, ushort length);
        abstract QJResult<List<short>> ReadInt16(string varFunc, ushort address, ushort length);
        abstract QJResult<List<uint>> ReadUInt32(string varFunc, ushort address, ushort length);
        abstract QJResult<List<int>> ReadInt32(string varFunc, ushort address, ushort length);
        abstract QJResult<List<ulong>> ReadUInt64(string varFunc, ushort address, ushort length);
        abstract QJResult<List<long>> ReadInt64(string varFunc, ushort address, ushort length);
        abstract QJResult<List<float>> ReadFloat(string varFunc, ushort address, ushort length);
        abstract QJResult<List<double>> ReadDouble(string varFunc, ushort address, ushort length);
    }
}
