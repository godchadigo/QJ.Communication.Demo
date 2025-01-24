using QJ.Communication.Core.Enum;
using QJ.Communication.Core.Interface;
using QJ.Communication.Core.Model.Result;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    public abstract class QJTcpPluginBase : QJPluginBase, INetCommunication , IVariableRead, IVariableWrite, IVariableReadAsync , IVariableWriteAsync
    {

        public override CommunicationEnum communicationType => CommunicationEnum.Tcp;


        #region INetCommunication
        public TcpClient tcpClient;


        public virtual string IpAddress {  get; set; }

        public virtual int Port { get; set; }

        
        
        
        public virtual QJResult<TcpClient> Connect(string ip, int port) 
        {
            QJResult<TcpClient> qJResult = new QJResult<TcpClient>() { IsOk = false };
            try
            {
                IpAddress = ip;
                Port = port;

                tcpClient.Connecting = (client, e) => { return EasyTask.CompletedTask; };//即将连接到服务器，此时已经创建socket，但是还未建立tcp
                tcpClient.Connected = (client, e) => {
                    Console.WriteLine($"{client.IP} 連接到伺服器成功!");
                    return EasyTask.CompletedTask;
                };//成功连接到服务器
                tcpClient.Closing = (client, e) => { return EasyTask.CompletedTask; };//即将从服务器断开连接。此处仅主动断开才有效。
                tcpClient.Closed = (client, e) => {
                    Console.WriteLine($"{client.IP} 與伺服器斷開!");
                    return EasyTask.CompletedTask; 
                };//从服务器断开连接，当连接不成功时不会触发。
                tcpClient.Received = (client, e) =>
                {
                    //从服务器收到信息。但是一般byteBlock和requestInfo会根据适配器呈现不同的值。.
                    /*
                    var mes = e.ByteBlock.Span.ToString(Encoding.UTF8);
                    tcpClient.Logger.Info($"客户端接收到信息：{mes}");
                    */
                    return EasyTask.CompletedTask;
                };

                //载入配置
                tcpClient.Setup(new TouchSocketConfig()
                      .SetRemoteIPHost($"{IpAddress}:{Port}")
                      .ConfigureContainer(a =>
                      {
                          a.AddConsoleLogger();//添加一个日志注入
                      })
                      .ConfigurePlugins(a =>
                      {
                          a.UseTcpReconnection()
                          .UsePolling(TimeSpan.FromSeconds(1));
                      }));

                tcpClient.Connect();//调用连接，当连接不成功时，会抛出异常。

                //Log.Information("Tcp Client Connected!");
                Console.WriteLine("Tcp Client Connected!");
                qJResult.IsOk = true;
                qJResult.Data = tcpClient;
                return qJResult;
            }
            catch (Exception ex) {
                qJResult.Message = ex.Message;
                return qJResult;
            }           
        }

        public virtual QJResult<TcpClient> Connect(TcpClient _tcpClient)
        {
            QJResult<TcpClient> qJResult = new QJResult<TcpClient>() { IsOk = false };
            try
            {

                tcpClient = _tcpClient;

                qJResult.IsOk = true;
                qJResult.Data = _tcpClient;
               
                _tcpClient.Connect();//调用连接，当连接不成功时，会抛出异常。

                //Log.Information("Tcp Client Connected!");
                //Console.WriteLine("Tcp Client Connected!");
              
                // 賦予連線後的客戶端到全局變數
                
                return qJResult;
            }
            catch (Exception ex)
            {
                qJResult.Message = ex.Message;
                return qJResult;
            }
        }
        public virtual void Disconnect() {
            if (tcpClient != null) tcpClient.Close();
        }

        public virtual Task ConnectAsync(string ip, int port)
        {
            IpAddress = ip;
            Port = port;
            return Task.CompletedTask;
        }

        public virtual Task DisconnectAsync() { return Task.CompletedTask; }
        public virtual void CreateBasePacket() { }

        public void Send(byte[] data)
        {
            if (tcpClient != null)
            {
                tcpClient.Send(data);
            }            
        }

        public async Task SendAsync(byte[] data)
        {
            if (tcpClient != null)
            {
                await tcpClient.SendAsync(data);
            }
        }

        public async Task SendAsync(ReadOnlyMemory<byte> memory)
        {
            if (tcpClient != null && tcpClient.Online)
            {
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

                await tcpClient.SendAsync(memory);
            }
        }
        public QJResult<byte[]> SendThenRecived(byte[] data)
        {
            QJResult<byte[]> qJResult = new QJResult<byte[]>();
            if (tcpClient != null)
            {
                if (!tcpClient.Online)
                {
                    qJResult.IsOk = false;
                    qJResult.Message = "連線階段斷開";
                    return qJResult;
                }
                //调用CreateWaitingClient获取到IWaitingClient的对象。
                var waitClient = tcpClient.CreateWaitingClient(new WaitingOptions()
                {
                    FilterFunc = response => //设置用于筛选的fun委托，当返回为true时，才会响应返回
                    {
                        return true;
                    }
                });

                qJResult.IsOk = true;
                qJResult.Data = waitClient.SendThenReturn(data);
                return qJResult;
            }
            return qJResult;
        }



        public async Task<QJResult<byte[]>> SendThenRecivedAsync(byte[] data)
        {
            QJResult<byte[]> qJResult = new QJResult<byte[]>();
            if (tcpClient != null)
            {
                //调用CreateWaitingClient获取到IWaitingClient的对象。
                var waitClient = tcpClient.CreateWaitingClient(new WaitingOptions()
                {
                    FilterFunc = response => //设置用于筛选的fun委托，当返回为true时，才会响应返回
                    {
                        return true;
                    }
                });

                qJResult.IsOk = true;
                qJResult.Data = await waitClient.SendThenReturnAsync(data);
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
            QJResult<IRequestInfo> qJResult = new QJResult<IRequestInfo>();
            try
            {
                if (tcpClient != null)
                {
                    if (!tcpClient.Online)
                    {
                        qJResult.IsOk = false;
                        qJResult.Message = "連線階段斷開";
                        return qJResult;
                    }
                    //调用CreateWaitingClient获取到IWaitingClient的对象。
                    var waitClient = tcpClient.CreateWaitingClient(new WaitingOptions()
                    {

                        FilterFunc = response => //设置用于筛选的fun委托，当返回为true时，才会响应返回
                        {
                            return true;
                        }
                    });

                    qJResult.IsOk = true;
                    qJResult.Data = waitClient.SendThenResponse(data,1000).RequestInfo;
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
                if (tcpClient != null)
                {
                    if (!tcpClient.Online)
                    {
                        qJResult.IsOk = false;
                        qJResult.Message = "連線階段斷開";
                        return qJResult;
                    }
                    //调用CreateWaitingClient获取到IWaitingClient的对象。
                    var waitClient = tcpClient.CreateWaitingClient(new WaitingOptions()
                    {

                        FilterFunc = response => //设置用于筛选的fun委托，当返回为true时，才会响应返回
                        {
                            return true;
                        }
                    });

                    var _response = await waitClient.SendThenResponseAsync(data, 1000);
                    qJResult.IsOk = true;
                    qJResult.Data = _response.RequestInfo;
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

        #region Read
        public virtual QJResult<List<byte>> Read(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        public virtual QJResult<List<bool>> ReadBool(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        public virtual QJResult<List<ushort>> ReadUInt16(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult<List<short>> ReadInt16(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }


        public virtual QJResult<List<uint>> ReadUInt32(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }


        public virtual QJResult<List<int>> ReadInt32(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }


        public virtual QJResult<List<ulong>> ReadUInt64(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }


        public virtual QJResult<List<long>> ReadInt64(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        public virtual QJResult<List<float>> ReadFloat(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        public virtual QJResult<List<double>> ReadDouble(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Write
        public virtual QJResult Write(string varFunc, ushort address, bool value)
        {
            throw new NotImplementedException();
        }



        public virtual QJResult Write(string varFunc, ushort address, bool[] values)
        {
            throw new NotImplementedException();
        }



        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<bool> values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, ushort value)
        {
            throw new NotImplementedException();
        }
        public virtual QJResult Write(string varFunc, ushort address, ushort[] values)
        {
            throw new NotImplementedException();
        }
        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<ushort> values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, short value)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, short[] values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<short> values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, uint value)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, uint[] values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<uint> values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, int value)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, int[] values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<int> values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, ulong value)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, ulong[] values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<ulong> values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, long value)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, long[] values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<long> values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, float value)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, float[] values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<float> values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, double value)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, double[] values)
        {
            throw new NotImplementedException();
        }

        public virtual QJResult Write(string varFunc, ushort address, IEnumerable<double> values)
        {
            throw new NotImplementedException();
        }
        public virtual QJResult Write(string varFunc, ushort address, string str, EncodingType encode)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region ReadAsync
        public virtual Task<QJResult<List<byte>>> ReadAsync(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }
        public virtual Task<QJResult<List<bool>>> ReadBoolAsync(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult<List<ushort>>> ReadUInt16Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult<List<short>>> ReadInt16Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult<List<uint>>> ReadUInt32Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult<List<int>>> ReadInt32Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult<List<ulong>>> ReadUInt64Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult<List<long>>> ReadInt64Async(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult<List<float>>> ReadFloatAsync(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult<List<double>>> ReadDoubleAsync(string varFunc, ushort address, ushort length)
        {
            throw new NotImplementedException();
        }



        #endregion

        #region WriteAsync

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, bool value)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, bool[] values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<bool> values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, ushort value)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, ushort[] values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<ushort> values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, short value)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, short[] values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<short> values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, uint value)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, uint[] values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<uint> values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, int value)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, int[] values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<int> values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, ulong value)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, ulong[] values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<ulong> values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, long value)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, long[] values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<long> values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, float value)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, float[] values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<float> values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, double value)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, double[] values)
        {
            throw new NotImplementedException();
        }

        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<double> values)
        {
            throw new NotImplementedException();
        }
        public virtual Task<QJResult> WriteAsync(string varFunc, ushort address, string str, EncodingType encode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
