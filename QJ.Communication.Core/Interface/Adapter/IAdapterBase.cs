using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;

namespace QJ.Communication.Core.Interface.Adapter
{
    /// <summary>
    /// 用於讓各式通訊適配器實現發送以及接收原始數據的接口
    /// 原始傳送數據不需要，因為在發送時，就已經觸發事件轉發了
    /// 由於接收數據有可能是非同步的，並且適配器也可能在其他線程，所以獨立開發一個介面來處理接收到的原始數據
    /// </summary>
    public interface IRecivedAdapterBase
    {
        /// <summary>
        /// 接收原始數據
        /// </summary>        
        List<byte> RecivedRawData { get; set; }
    }

}
