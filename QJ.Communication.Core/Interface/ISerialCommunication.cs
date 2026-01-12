using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QJ.Communication.Core.Model.Result;
using QJ.Communication.Core.Model.SerialPorts;
using TouchSocket.Core;
using TouchSocket.SerialPorts;

namespace QJ.Communication.Core.Interface
{
    public interface ISerialCommunication 
    {
        QJResult<QJSerialPortClient> Connect(QJSerialPortProp serialProp);
        QJResult<QJSerialPortClient> Connect(QJSerialPortClient client);
        void Disconnect();
        Task<QJResult<QJSerialPortClient>> ConnectAsync(QJSerialPortProp serialProp,int timeout);
        Task DisconnectAsync();
        void Send(byte[] data);
        Task SendAsync(byte[] data);
        QJResult<byte[]> SendThenRecived(byte[] data);      
        Task<QJResult<byte[]>> SendThenRecivedAsync(byte[] data);
        QJResult<IRequestInfo> SendThenRecivedRequestInfo(byte[] data);
        Task<QJResult<IRequestInfo>> SendThenRecivedRequestInfoAsync(byte[] data);
    }
}
