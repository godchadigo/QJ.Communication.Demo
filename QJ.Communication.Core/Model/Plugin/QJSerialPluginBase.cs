using QJ.Communication.Core.Interface;
using QJ.Communication.Core.Model.Result;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.SerialPorts;
using TouchSocket.Sockets;

namespace QJ.Communication.Core.Model.Plugin
{
    /// <summary>
    /// 串列阜插件基類
    /// </summary>
    public abstract class QJSerialPluginBase : QJPluginBase, ISerialCommunication, IVariableRead
    {
        public virtual byte StationID { get; set; } = 1;
        public override CommunicationEnum communicationType => CommunicationEnum.Serial;

        #region ISerialCommunication
        public SerialPortClient serialClient = new SerialPortClient();
        public virtual QJResult<SerialPortClient> Connect(SerialPortOption serialProp)
        {
            QJResult<SerialPortClient> qJResult = new QJResult<SerialPortClient>() { IsOk = false };
            try
            {
                if (string.IsNullOrEmpty(serialProp.PortName))
                {
                    qJResult.Message = "端口名稱為空!";
                    return qJResult;
                }

                // 實現串口通訊Client端
                serialClient.SetupAsync(new TouchSocketConfig()
                     .SetSerialPortOption(serialProp)                          
                     .ConfigurePlugins(a =>
                     { }));

                
                // 連線目標設備
                serialClient.Connect();

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

        public virtual async Task ConnectAsync(SerialPortOption serialProp)
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

        public virtual QJResult<byte[]> SendThenRecived(byte[] data)
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
                qJResult.Data = waitClient.SendThenReturn(data);
                
                //Console.WriteLine("內部耗時:" + SW.ElapsedMilliseconds);
                return qJResult;
            }
            return qJResult;
        }

        public virtual Task<QJResult<byte[]>> SendThenRecivedAsync(byte[] data)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 同步傳送Byte[]數據，並且返回RequestInfo對象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public QJResult<IRequestInfo> SendThenRecivedRequestInfo(byte[] data)
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
                    qJResult.Data = waitClient.SendThenResponse(data, 1000).RequestInfo;
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
                    var response = await waitClient.SendThenResponseAsync(data, 1000);
                    qJResult.IsOk = true;
                    qJResult.Data = response.RequestInfo;
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
        public abstract QJResult<List<byte>> Read(string varFunc, ushort address, ushort length);
        public abstract QJResult<List<bool>> ReadBool(string varFunc, ushort address, ushort length);
        public abstract QJResult<List<ushort>> ReadUInt16(string varFunc, ushort address, ushort length);


        public abstract QJResult<List<short>> ReadInt16(string varFunc, ushort address, ushort length);


        public abstract QJResult<List<uint>> ReadUInt32(string varFunc, ushort address, ushort length);


        public abstract QJResult<List<int>> ReadInt32(string varFunc, ushort address, ushort length);


        public abstract QJResult<List<ulong>> ReadUInt64(string varFunc, ushort address, ushort length);


        public abstract QJResult<List<long>> ReadInt64(string varFunc, ushort address, ushort length);
        public abstract QJResult<List<float>> ReadFloat(string varFunc, ushort address, ushort length);
        public abstract QJResult<List<double>> ReadDouble(string varFunc, ushort address, ushort length);



        #endregion

        #region Write
        public abstract void Write(string variableName, ushort v);
        public abstract void Write(string variableName, short v);
        #endregion



    }
}
