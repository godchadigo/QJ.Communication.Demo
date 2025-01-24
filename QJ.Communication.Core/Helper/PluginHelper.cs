using QJ.Communication.Core.Interface;
using QJ.Communication.Core.Model.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QJ.Communication.Core.Helper
{
    public class PluginHelper
    {
        public static PluginHelper Instance { get; set; } = new PluginHelper();

        public Dictionary<string, QJPluginBase> Plugins { get; set; } = new Dictionary<string, QJPluginBase>();
    }
}
