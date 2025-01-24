using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QJ.Communication.Core.Model.Result
{
    public class QJResult
    {
        public bool IsOk { get; set; } = false;
        public dynamic Data { get; set; }
        public string Message { get; set; } = "";
    }

    public class QJResult<T>
    {
        public bool IsOk { get; set; } = false;
        public T Data { get; set; }
        public string Message { get; set; } = "";
    }
}
