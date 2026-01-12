using QJ.Communication.Core.Enum;
using QJ.Communication.Core.Interface;
using QJ.Communication.Core.Interface.Adapter;
using QJ.Communication.Core.Model.Result;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using static QJ.Communication.Core.Enums.EncodingTypeEnum;

namespace QJ.Communication.Core.Model.Plugin
{
    /// <summary>
    /// Tcp插件基類
    /// </summary>
    public abstract class QJTcpPluginBase : QJPluginBase, INetCommunication 
    {

        public override CommunicationEnum communicationType => CommunicationEnum.Tcp;
        public QJTcpPluginBase()
        {
        }

        #region INetCommunication
        /// <summary>
        /// Tcp Client 基底實例
        /// 當連線成功後，會賦予tcpClient實例到此屬性。
        /// 插件若是繼承該QJTcpPluginBase並且調用Connect方法，則會自動賦予tcpClient實例。
        /// </summary>
        public TcpClient _tcpClient;
        /// <summary>
        /// 目標連線的IP地址
        /// </summary>
        public virtual string IpAddress {  get; set; }
        /// <summary>
        /// 目標連線的Port
        /// </summary>
        public virtual int Port { get; set; }
        public async Task<QJResult<TcpClient>> ConnectAsync(TcpClient tcpClient, int timeout = 1000)
        {
            QJResult<TcpClient> qJResult = new QJResult<TcpClient>() { IsOk = false };
            try
            {            
                _tcpClient = tcpClient;
                await tcpClient.ConnectAsync(timeout);//调用连接，当连接不成功时，会抛出异常。

                Console.WriteLine("Tcp Client Connected!");
                qJResult.IsOk = true;
                qJResult.Data = tcpClient;
                return qJResult;
            }
            catch (Exception ex)
            {
                qJResult.IsOk = false;
                qJResult.Message = ex.Message;
                return qJResult;
            }
        }

        /// <summary>
        /// 建立Tcp連線到目標設備
        /// </summary>
        /// <param name="ip">目標IP</param>
        /// <param name="port">目標Port</param>
        /// <returns></returns>
        public virtual async Task<QJResult<TcpClient>> ConnectAsync(string ip, int port, int timeout = 1000) 
        {
            QJResult<TcpClient> qJResult = new QJResult<TcpClient>() { IsOk = false };
            try
            {
                IpAddress = ip;
                Port = port;

                // 訂閱接收事件到TcpPluginBase上
                _tcpClient.Received += async (sender, e) => {

                    // 判斷該通訊插件是否有實作IAdapterBase接口
                    if (e.RequestInfo is IRecivedAdapterBase recivedAdapter)
                    {
                        // 轉發接收到的數據
                        await TriggerRecivedRawData(recivedAdapter, DateTime.Now);
                    }
                };

                _tcpClient.Connected += async (sender, e) => 
                {
                    await TriggerOnConnected(DateTime.Now);
                };
                _tcpClient.Closed += async (sender, e) => {
                    await TriggerOnClosed(DateTime.Now);
                };

                qJResult.IsOk = true;
                qJResult.Data = _tcpClient;
                return qJResult;
            }
            catch (Exception ex) {
                qJResult.IsOk = false;
                qJResult.Message = ex.Message;
                return qJResult;
            }           
        }


        /// <summary>
        /// 非同步斷開連線
        /// </summary>
        /// <returns></returns>        
        public virtual async Task DisconnectAsync() 
        {
            await _tcpClient.CloseAsync();
        }

        public void Send(byte[] data)
        {
            if (_tcpClient != null)
            {
                _tcpClient.SendAsync(data);
            }            
        }

        public async Task SendAsync(byte[] data)
        {
            if (_tcpClient != null)
            {
                await _tcpClient.SendAsync(data);
            }
        }

        public async Task SendAsync(ReadOnlyMemory<byte> memory)
        {
            
            if (_tcpClient != null && (_tcpClient.Online) && _tcpClient.IP != null)
            {
#if false
                var data = memory.ToArray();
                string str = string.Empty;
                for (global::System.Int32 i = 0; i < memory.Length; i++)
                {
                    str += data[i].ToString("X2") + " ";
                }

                
                if (base.IsShowRequestMessage)
                {

#if NET462_OR_GREATER || NET472 || NET472_OR_GREATER || NET48 || NET481
                    Console.WriteLine("request packet -> " + str);
#else
                    Debug.WriteLine("request packet -> " + str);
#endif

                   
                }
#endif

                // 轉發發送的原始數據
                await TriggerSendRawData(memory.ToArray(), memory.Length, DateTime.Now);
                await _tcpClient.SendAsync(memory).ConfigureAwait(false);
                
            }
        }
        public QJResult<byte[]> SendThenRecived(byte[] data)
        {
            throw new NotImplementedException();
        }

        public async Task<QJResult<byte[]>> SendThenRecivedAsync(byte[] data)
        {
            QJResult<byte[]> qJResult = new QJResult<byte[]>();
            if (_tcpClient != null)
            {
                if (!_tcpClient.Online)
                {
                    qJResult.IsOk = false;
                    qJResult.Message = "連線階段斷開";
                    return qJResult;
                }
                //调用CreateWaitingClient获取到IWaitingClient的对象。
                var waitClient = _tcpClient.CreateWaitingClient(new WaitingOptions()
                {
                    FilterFunc = response => //设置用于筛选的fun委托，当返回为true时，才会响应返回
                    {
                        return true;
                    }
                });

                qJResult.IsOk = true;
                var res = await waitClient.SendThenResponseAsync(data, 1000);
                qJResult.Data = res.Memory.ToArray();
                return qJResult;
            }
            return qJResult;
        }
        /// <summary>
        /// 傳送Byte[]數據，並且返回RequestInfo對象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public QJResult<IRequestInfo> SendThenRecivedRequestInfo(byte[] data)
        {
           throw new NotImplementedException();
        }

        /// <summary>
        /// 非同步傳送Byte[]數據，並且返回RequestInfo對象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<QJResult<IRequestInfo>> SendThenRecivedRequestInfoAsync(byte[] data)
        {
            QJResult<IRequestInfo> qJResult = new QJResult<IRequestInfo>();
            try
            {
                if (_tcpClient != null)
                {
                    if (!_tcpClient.Online)
                    {
                        qJResult.IsOk = false;
                        qJResult.Message = "連線階段斷開";
                        return qJResult;
                    }
                    //调用CreateWaitingClient获取到IWaitingClient的对象。
                    var waitClient = _tcpClient.CreateWaitingClient(new WaitingOptions()
                    {

                        FilterFunc = response => //设置用于筛选的fun委托，当返回为true时，才会响应返回
                        {
                            return true;
                        }
                    });

                    qJResult.IsOk = true;
                    var res = await waitClient.SendThenResponseAsync(data, 1000);
                    qJResult.Data = res.RequestInfo;
                    return qJResult;
                }
                return qJResult;
            }
            catch (Exception ex)
            {
                qJResult.Message = ex.Message;
                return qJResult;
            }
        }

#endregion

        #region 轉發事件功能插件
        public virtual async Task TriggerSendRawData(byte[] data, int len, DateTime trigDate)
        {
            await OnDataSend(data, len, this, trigDate).ConfigureAwait(EasyTask.ContinueOnCapturedContext);
        }

        public virtual async Task TriggerRecivedRawData(IRecivedAdapterBase recivedAdapter, DateTime trigDate)
        {
            await OnDataReceived(recivedAdapter, this, trigDate).ConfigureAwait(EasyTask.ContinueOnCapturedContext);
        }
        public virtual async Task TriggerOnConnected(DateTime trigDate)
        {
            await OnConnected(this, trigDate).ConfigureAwait(EasyTask.ContinueOnCapturedContext);
        }
        public virtual async Task TriggerOnClosed(DateTime trigDate)
        {
            await OnClosed(this, trigDate).ConfigureAwait(EasyTask.ContinueOnCapturedContext);
        }
        public QJResult<TcpClient> Connect(string ip, int port)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region 棄用方法


        /// <summary>
        /// 建立Tcp連線到目標設備，傳入TcpClient實例，請注意若是使用該方法連線，Recevied事件將會QJTcpPluginBase內部處理，如果有需要Recived事件，可以直接使用this.Received事件來處理接收的數據。
        /// </summary>
        /// <param name="_tcpClient">TcpClient實例</param>
        /// <returns></returns>
        [Obsolete("該接口因為TS套件全面改為Async防止死鎖所以直接棄用該方法")]
        public virtual async Task<QJResult<TcpClient>> Connect(TcpClient _tcpClient)
        {
            QJResult<TcpClient> qJResult = new QJResult<TcpClient>() { IsOk = false };
            try
            {
                // 連線前確保連線沒有連接

                if (this._tcpClient != null)
                {
                    this._tcpClient.CloseAsync();
                }

                // 訂閱接收事件到TcpPluginBase上
                _tcpClient.Received += async (sender, e) => {

                    // 判斷該通訊插件是否有實作IAdapterBase接口
                    if (e.RequestInfo is IRecivedAdapterBase recivedAdapter)
                    {
                        // 轉發接收到的數據
                        await TriggerRecivedRawData(recivedAdapter, DateTime.Now);
                    }
                };

                var res = await _tcpClient.TryConnectAsync(1000);//调用连接，当连接不成功时，会抛出异常。

                qJResult.IsOk = res.IsSuccess;
                qJResult.Data = _tcpClient;
                this._tcpClient = _tcpClient;

                if (res.IsSuccess)
                {

                }
                else
                {
                    //qJResult.Data = _tcpClient;
                    _tcpClient.SafeDispose();
                }
                // 賦予連線後的客戶端到全局變數

                return qJResult;
            }
            catch (Exception ex)
            {
                //throw new Exception("連線到目標異常!");
                //Task.Run(() => HandleDisconnectAsync()) ;
                /*
                Thread t = new Thread(() => HandleDisconnectAsync());
                t.IsBackground = true;
                t.Start();
                */
                qJResult.IsOk = false;
                qJResult.Message = ex.Message;
                return qJResult;
            }
        }
        [Obsolete("該接口因為TS套件全面改為Async防止死鎖所以直接棄用該方法")]
        QJResult<TcpClient> INetCommunication.Connect(TcpClient tcpClient)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 斷開連線
        /// </summary>
        [Obsolete("該接口因為TS套件全面改為Async防止死鎖所以直接棄用該方法")]
        public virtual void Disconnect()
        {
            if (_tcpClient != null) _tcpClient.CloseAsync();
        }
        #endregion

    }
}
