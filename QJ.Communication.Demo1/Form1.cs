using QJ.Communication.Core.Cores.Tcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QJ.Communication.Demo1
{
    public partial class Form1 : Form
    {
        private Core.Core _core;
        private Dictionary<string, TcpCore> _TcpDevices = new Dictionary<string, TcpCore>();
        public Form1()
        {
            
            InitializeComponent();
      
            var t1 = Task.Run(() => Init());
            t1.Wait();
            var t2 = Task.Run(() => ConnectAllDevice());
            Task.WhenAll(t1, t2);
        }
        private void Init()
        {
            // 實例化
            _core = new Core.Core();
            _core.Init();

            // Tcp通訊核心
            var device = new TcpCore("ModbusTcpV2", out bool _Init);
            if (_Init)
            {
                // 設備參數
                device.IpAddress = "127.0.0.1";
                device.Port = 502;
                // 顯示請求封包
                device.GetPluginBase().IsShowRequestMessage = true;
            }

            // 添加設備到列表中
            _TcpDevices.Add("設備1號", device);
        }

        // 連線
        private void ConnectAllDevice()
        {
            foreach (var pair in _TcpDevices)
            {
                pair.Value.Connect();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 獲取設備通訊插件本體
            var plugin = _TcpDevices["設備1號"].GetPluginBase();

            var res = plugin.ReadUInt16 ("4x" , 0 , 1);
            if (res.IsOk)
            {
                this.BeginInvoke(new Action(delegate {
                    label1.Text = res.Data[0].ToString();
                }));
            }
            else
            {
                Console.WriteLine(res.Message);
                MessageBox.Show(res.Message);
            }

            //var res = plugin.Write("0x", 0, new bool[2] { true, true });
            //if (!res.IsOk) Console.WriteLine(res.Message);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("妳好");
        }
    }
}
