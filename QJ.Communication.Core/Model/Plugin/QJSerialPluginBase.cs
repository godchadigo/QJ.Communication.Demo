using QJ.Communication.Core.Enums;
using QJ.Communication.Core.Extension;
using QJ.Communication.Core.Interface;
using QJ.Communication.Core.Model.Result;
using QJ.Communication.Core.Model.SerialPorts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.SerialPorts;
using TouchSocket.Sockets;
using static QJ.Communication.Core.Enums.EncodingTypeEnum;

namespace QJ.Communication.Core.Model.Plugin
{
    /// <summary>
    /// 串列阜插件基類
    /// </summary>
    public abstract class QJSerialPluginBase : QJPluginBase, ISerialCommunication
    {
        public virtual byte StationID { get; set; } = 1;
        public override CommunicationEnum communicationType => CommunicationEnum.Serial;

        #region ISerialCommunication
        public virtual QJSerialPortProp serialProp { get; set; }
        public QJSerialPortClient serialClient = new QJSerialPortClient();
        public virtual QJResult<QJSerialPortClient> Connect(QJSerialPortProp serialProp)
        {
            QJResult<QJSerialPortClient> qJResult = new QJResult<QJSerialPortClient>() { IsOk = false };
            try
            {
                if (string.IsNullOrEmpty(serialProp.PortName))
                {
                    qJResult.Message = "端口名稱為空!";
                    return qJResult;
                }

                
                // 實現串口通訊Client端
                serialClient.SetupAsync(new TouchSocketConfig()
                     .SetSerialPortOption(options =>
                     {
                         options.BaudRate = serialProp.BaudRate;//波特率
                         options.DataBits = serialProp.DataBits;//数据位
                         options.Parity = serialProp.Parity;//校验位
                         options.PortName = serialProp.PortName;//COM
                         options.StopBits = serialProp.StopBits;//停止位
                         options.RtsEnable = serialProp.RtsEnable;//RTS
                         options.DtrEnable = serialProp.DtrEnable;//DTR
                     })                          
                     .ConfigurePlugins(a =>
                     { }));

                
                // 連線目標設備
                serialClient.ConnectAsync();

                Log.Information("Serial Client Connected!");
                Console.WriteLine("Serial Client Connected!");
                qJResult.IsOk = true;
                qJResult.Data = serialClient;
                return qJResult;
            }
            catch(Exception ex)
            {
                qJResult.Message = ex.Message;
                return qJResult;
            }            
        }

        public virtual QJResult<QJSerialPortClient> Connect(QJSerialPortClient client)
        {
            try
            {
                Task.Run(async () =>
                {
                    await client.ConnectAsync(5000);
                }).Wait();

                return client.QJDataResponse(isOk: true);
            }
            catch (Exception ex)
            {
                return new QJResult<QJSerialPortClient>() { IsOk = false, Message = ex.Message };

            }
        }
        public virtual async Task ConnectAsync(QJSerialPortProp serialProp)
        {
            throw new NotImplementedException();
        }

        public virtual void Disconnect()
        {
            serialClient.CloseAsync();
        }

        public virtual async Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public virtual void Send(byte[] data)
        {
            throw new NotImplementedException();
        }

        public virtual async Task SendAsync(byte[] data)
        {
            throw new NotImplementedException();
        }
        public async Task SendAsync(ReadOnlyMemory<byte> memory)
        {

            if (serialClient != null && (serialClient.Online))
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

                await serialClient.SendAsync(memory).ConfigureAwait(false);

            }
        }
        public virtual QJResult<byte[]> SendThenRecived(byte[] data)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<QJResult<byte[]>> SendThenRecivedAsync(byte[] data)
        {
            QJResult<byte[]> qJResult = new QJResult<byte[]>();
            if (serialClient != null)
            {
                if (!serialClient.Online)
                {
                    qJResult.IsOk = false;
                    qJResult.Message = "連線階段斷開";
                    return qJResult;
                }
                //调用CreateWaitingClient获取到IWaitingClient的对象。
                var waitClient = serialClient.CreateWaitingClient(new WaitingOptions()
                {
                    FilterFunc = response => //设置用于筛选的fun委托，当返回为true时，才会响应返回
                    {
                        return true;
                    }
                });

                qJResult.IsOk = true;
                var res = await waitClient.SendThenResponseAsync(data);
                qJResult.Data = res.Memory.ToArray();

                //Console.WriteLine("內部耗時:" + SW.ElapsedMilliseconds);
                return qJResult;
            }
            return qJResult;
            
        }
        /// <summary>
        /// 同步傳送Byte[]數據，並且返回RequestInfo對象
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
                if (serialClient != null)
                {
                    if (!serialClient.Online)
                    {
                        qJResult.IsOk = false;
                        qJResult.Message = "連線階段斷開";
                        return qJResult;
                    }
                    //调用CreateWaitingClient获取到IWaitingClient的对象。
                    var waitClient = serialClient.CreateWaitingClient(new WaitingOptions()
                    {
                        /*
                        FilterFunc = response => //设置用于筛选的fun委托，当返回为true时，才会响应返回
                        {
                            return true;
                        }
                        */
                        FilterFunc = default
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

    }
}
