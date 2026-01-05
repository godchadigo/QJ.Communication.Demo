using QJ.Communication.Core.Enum;
using QJ.Communication.Core.Interface;
using QJ.Communication.Core.Interface.Adapter;
using QJ.Communication.Core.Model.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using static QJ.Communication.Core.Enum.EndianEnum;
using static QJ.Communication.Core.Enums.EncodingTypeEnum;

namespace QJ.Communication.Core.Model.Plugin
{
    /// <summary>
    /// 插件基類
    /// </summary>
    public abstract class QJPluginBase : IQJPlugin, ICloneable,IDisposable, IVariableRead, IVariableWrite, IVariableReadAsync, IVariableWriteAsync
    {
        

        #region 委派事件
        /// <summary>
        /// 採用非同步方式轉發插件中的資料接收事件
        /// </summary>
        public Func<IRecivedAdapterBase, IQJPlugin, Task> OnDataReceived;
        /// <summary>
        /// 採用非同步方式轉發插件中的資料發送事件
        /// </summary>
        public Func<byte[], int, IQJPlugin, Task> OnDataSend;
        #endregion

        #region 類別相關
        private bool _disposed;
        private readonly object _disposeLock = new object();

        /// <summary>
        /// 釋放資源
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// 釋放資源的虛擬方法，允許派生類別覆寫
        /// </summary>
        /// <param name="disposing">是否正在釋放託管資源</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (_disposeLock)
            {
                if (_disposed)
                    return;

                if (disposing)
                {
                    // 釋放託管資源
                    IsEnabled = false;
                    IsInitialized = false;
                    IsOnline = false;
                }

                // 釋放非託管資源（如果有的話）

                _disposed = true;
            }
        }

        /// <summary>
        /// 檢查是否已釋放資源
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
        public QJPluginBase()
        {
            // 初始化事件委派，避免空引用異常
            OnDataReceived = (adapterBase, plugin) => Task.CompletedTask;
            OnDataSend = (data, len, plugin) => Task.CompletedTask;
        }

        /// <summary>
        /// 終結器
        /// </summary>
        ~QJPluginBase()
        {
            Dispose(false);
        }

        #endregion

        #region IQJPlugin
        /// <summary>
        /// 插件名稱
        /// </summary>
        public abstract string Name { get; set; }
        /// <summary>
        /// 插件版本
        /// </summary>
        public virtual string Version => UpdateLog.Last().Key;
        /// <summary>
        /// 插件作者
        /// </summary>

        public virtual string Author => "作者很懶連名子都沒打~";
        /// <summary>
        /// 插件註解
        /// </summary>
        public virtual string Description => "";
        /// <summary>
        /// 版本更新紀錄
        /// </summary>
        public virtual Dictionary<string, string> UpdateLog => new Dictionary<string, string>() { { "NullVersion", "作者很懶沒有定義版本更新紀錄!" } };
        /// <summary>
        /// 設備是否在線
        /// </summary>
        public virtual bool IsOnline { get; set; }
        /// <summary>
        /// 設備站號
        /// </summary>
        public virtual byte Station { get; set; } = 1; // 默認站號為1
        /// <summary>
        /// 是否啟用插件
        /// </summary>
        protected virtual bool IsEnabled { get; set; } = true;
        /// <summary>
        /// 是否已經初始化完成
        /// </summary>
        public virtual bool IsInitialized { get; set; }
        /// <summary>
        /// 是否顯示通訊封包
        /// </summary>
        public virtual bool IsShowRequestMessage { get; set; } = false;
        /// <summary>
        /// 是否啟用Debug
        /// </summary>
        public virtual bool UseDebug { get; set; } = false;
        /// <summary>
        /// 插件通訊類型
        /// </summary>
        public virtual CommunicationEnum communicationType { get; set; } = CommunicationEnum.None;
        
        /// <summary>
        /// 端序，預設大端序
        /// </summary>
        protected virtual EndianType EndianType { get; set; } = EndianType.Big;
        public bool GetEnable()
        {
            return IsEnabled;
        }
        /// <summary>
        /// 設定通訊端序
        /// </summary>
        /// <param name="type"></param>
        public void SetEndianType(EndianEnum.QJEndianType type)
        {                        
            switch (type)
            {                
                case EndianEnum.QJEndianType.ABCD:
                    EndianType = EndianType.Big;
                    break;
                case EndianEnum.QJEndianType.BADC:
                    EndianType = EndianType.BigSwap;
                    break;
                case EndianEnum.QJEndianType.DCBA:
                    EndianType = EndianType.Little;
                    break;
                case EndianEnum.QJEndianType.CDAB:
                    EndianType = EndianType.LittleSwap;
                    break;
            }                        
        }
        /// <summary>
        /// 獲取當前端序
        /// </summary>
        /// <returns></returns>
        public QJEndianType GetEndianType()
        {
            return (QJEndianType)EndianType;
        }
        public abstract void Initialize();

        public abstract Task InitializeAsync();


        /// <summary>
        /// 拷貝插件本體
        /// 這是必要的否則使用者產生的實例會不斷被新的覆蓋
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            ThrowIfDisposed();
            // 創建深拷貝
            var clone = this.MemberwiseClone() as QJPluginBase;
            // 處理需要深拷貝的成員
            //clone.Initialize();
            //clone.InitializeAsync();
            return clone;
        }
        /// <summary>
        /// 打印當前插件資訊
        /// </summary>
        public virtual void PrintInformation()
        {
            Console.WriteLine($"                                            ");
            Console.WriteLine($"                                            ");
            Console.WriteLine($"========== {Name} ==========");
            Console.WriteLine($"作者:{Author} 版本:{Version} 簡介:{Description}");
            Console.WriteLine($"========== 歷史版本更新紀錄  ==========");
            foreach (var log in UpdateLog)
            {
                Console.WriteLine($"版本:{log.Key} 說明:{log.Value}");
            }
            Console.WriteLine($"                                            ");
            Console.WriteLine($"                                            ");
        }

        #endregion

        #region Read
        /// <inheritdoc />
        public virtual QJResult<List<byte>> Read(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult<List<bool>> ReadBool(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult<List<ushort>> ReadUInt16(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult<List<short>> ReadInt16(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult<List<uint>> ReadUInt32(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult<List<int>> ReadInt32(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult<List<ulong>> ReadUInt64(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult<List<long>> ReadInt64(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult<List<float>> ReadFloat(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult<List<double>> ReadDouble(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Write
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, bool value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, bool[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<bool> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, ushort value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, ushort[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<ushort> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, short value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, short[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<short> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, uint value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, uint[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<uint> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, int value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, int[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<int> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, ulong value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, ulong[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<ulong> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, long value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, long[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<long> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, float value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, float[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<float> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, double value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, double[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<double> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual QJResult Write(string varFunc, ushort address, string str, EncodingType encode)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ReadAsync
        /// <inheritdoc />
        public virtual Task<QJResult<List<byte>>> ReadAsync(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult<List<bool>>> ReadBoolAsync(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult<List<ushort>>> ReadUInt16Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult<List<short>>> ReadInt16Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult<List<uint>>> ReadUInt32Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult<List<int>>> ReadInt32Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult<List<ulong>>> ReadUInt64Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult<List<long>>> ReadInt64Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult<List<float>>> ReadFloatAsync(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult<List<double>>> ReadDoubleAsync(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region WriteAsync
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, bool value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, bool[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<bool> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, ushort value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, ushort[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<ushort> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, short value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, short[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<short> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, uint value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, uint[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<uint> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, int value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, int[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<int> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, ulong value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, ulong[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<ulong> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, long value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, long[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<long> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, float value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, float[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<float> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, double value)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, double[] values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<double> values)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, string str, EncodingType encode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
