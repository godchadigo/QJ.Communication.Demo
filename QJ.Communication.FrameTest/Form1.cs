using QJ.Communication.Core.Cores.Tcp;
using QJ.Communication.Core.Model.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QJ.Communication.FrameTest
{
    public partial class Form1 : Form
    {
        private Core.Core _core;
        private Dictionary<string, TcpCore> _TcpcDevices = new Dictionary<string, TcpCore>();
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
            // 通訊核心初始化
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
            _TcpcDevices.Add("設備1號", device);
        }

        private void ConnectAllDevice()
        {
            foreach (var pair in _TcpcDevices)
            {
                pair.Value.Connect();
            }
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // 獲取設備通訊插件本體
            var plugin = _TcpcDevices["設備1號"].GetPluginBase();
            var res = await plugin.WriteAsync("0x", 0, new bool[2] { true, true });
            if (!res.IsOk) Console.WriteLine(res.Message);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var plugin = _TcpcDevices["設備1號"].GetPluginBase();
            var res = new QJResult();
            var addrStr = "4x";
            var addrStr2 = "0x";
#if true

            res = plugin.Write(addrStr2, 100, true);
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr2, 0, new bool[16] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true });
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 5, (ushort)123);
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 6, new ushort[2] { 123, 456 });
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 10, (short)123);
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 11, new short[2] { 123, 456 });
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 20, (uint)123);
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 22, new uint[2] { 123, 456 });
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 30, (int)123);
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 32, new int[2] { 123, 456 });
            Console.WriteLine(res.Message);

            res = plugin.Write(addrStr, 100, (ulong)123);
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 104, new ulong[2] { 123, 456 });
            Console.WriteLine(res.Message);

            res = plugin.Write(addrStr, 120, (long)123);
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 124, new long[2] { 123, 456 });
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 140, (float)123);
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 144, new float[2] { 123.123f, 456.456f });
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 160, (double)123.123);
            Console.WriteLine(res.Message);
            res = plugin.Write(addrStr, 164, new double[2] { 123.123, 456.456 });
            Console.WriteLine(res.Message);
#else 
            res = await plugin.WriteAsync("0x", 17, true);
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync("0x", 20, new bool[12] { true ,true , false , true, false, true, false, true, false, true, false, true });
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 5, (ushort)123);
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 6, new ushort[2] { 123, 456 });
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 10, (short)123);
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 11, new short[2] { 123, 456 });
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 20, (uint)123);
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 22, new uint[2] { 123, 456 });
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 30, (int)123);
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 32, new int[2] { 123, 456 });
            Console.WriteLine(res.Message);

            res = await plugin.WriteAsync(addrStr, 100, (ulong)123);
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 104, new ulong[2] { 123, 456 });
            Console.WriteLine(res.Message);

            res = await plugin.WriteAsync(addrStr, 120, (long)123);
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 124, new long[2] { 123, 456 });
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 140, (float)123);
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 144, new float[2] { 123.123f, 456.456f });
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 160, (double)123.123);
            Console.WriteLine(res.Message);
            res = await plugin.WriteAsync(addrStr, 164, new double[2] { 123.123, 456.456 });
            Console.WriteLine(res.Message);
#endif
        }
    }
}
