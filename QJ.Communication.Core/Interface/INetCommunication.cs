using QJ.Communication.Core.Enum;
using QJ.Communication.Core.Model.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace QJ.Communication.Core.Interface
{
    /// <summary>
    /// 網路介面
    /// </summary>
    public interface INetCommunication
    {
        /// <summary>
        /// 連線目標設備IP
        /// </summary>
        string IpAddress {  get; }
        /// <summary>
        /// 連線設備目標Port
        /// </summary>
        int Port { get; }
        /// <summary>
        /// 連接連線目標
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Port</param>
        /// <returns></returns>
        QJResult<TcpClient> Connect(string ip, int port);
        /// <summary>
        /// 連接連線目標，傳入TcpClient進行初始化複寫
        /// </summary>
        /// <param name="tcpClient">Touchsocket TcpClient實體</param>
        /// <returns></returns>
        QJResult<TcpClient> Connect(TcpClient tcpClient);
        /// <summary>
        /// 斷開目標設備連線
        /// </summary>
        void Disconnect();
        /// <summary>
        /// 連接目標設備(非同步)
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        Task ConnectAsync(string ip, int port);
        /// <summary>
        /// 斷開目標設備(非同步)
        /// </summary>
        /// <returns></returns>
        Task DisconnectAsync();        

        /// <summary>
        /// 向目標設備傳送封包
        /// </summary>
        /// <param name="data"></param>
        void Send(byte[] data);
        /// <summary>
        /// 向目標設備傳送封包(非同步)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task SendAsync(byte[] data);
        /// <summary>
        /// 向目標設備傳送封包並且接收(同步)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        QJResult<byte[]> SendThenRecived(byte[] data);
        /// <summary>
        /// 向目標設備傳送封包並且接收(非同步)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<QJResult<byte[]>> SendThenRecivedAsync(byte[] data);
        /// <summary>
        /// 向目標設備傳送封包並且接收RequestInfo(同步)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        QJResult<IRequestInfo> SendThenRecivedRequestInfo(byte[] data);
        /// <summary>
        /// 向目標設備傳送封包並且接收RequestInfo(非同步)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<QJResult<IRequestInfo>> SendThenRecivedRequestInfoAsync(byte[] data);
              
    }
}
