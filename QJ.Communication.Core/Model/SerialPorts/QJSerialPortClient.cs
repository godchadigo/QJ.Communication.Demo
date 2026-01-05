using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.SerialPorts;

namespace QJ.Communication.Core.Model.SerialPorts
{
    public class QJSerialPortClient : SerialPortClient
    {
        public DataHandlingAdapter ReadOnlyDataHandlingAdapter => ProtectedDataHandlingAdapter;
    }
}
