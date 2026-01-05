using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QJ.Communication.Core;
using QJ.Communication.Core.Cores.Tcp;

namespace QJ.Communication.Study.HelloWorld
{
    public partial class Form1 : Form
    {
        private QJManager _QJManager;
        private TcpCore _tcpCore;
        private bool _isReady = false;
        public Form1()
        {
            InitializeComponent();

            // 第一步 初始化QJManager
            InitQJCommunication();

            // 第二步 使用TcpCore實例化插件
            _tcpCore = new TcpCore("ModbusTcpV2" , out bool initResult);
            _isReady = initResult;
            if (!_isReady) MessageBox.Show("插件初始化失敗，請檢察插件是否正確載入!");


        }


        private void InitQJCommunication()
        {
            _QJManager = new QJManager();
            _QJManager.Init();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void connect_btn_Click(object sender, EventArgs e)
        {
            if (!_isReady) MessageBox.Show("插件初始化失敗，請檢察插件是否正確載入!");

            // 第三步 配置TcpCore參數            
            if (_isReady)
            {
                var ip = ip_tbox.Text;
                var _port = int.TryParse(port_tbox.Text, out int port);

                // 第四步 使用TcpCore進行建立連線
                var connectTask = Task.Run(()=>_tcpCore.ConnectAsync(ip,port,5000));
                connectTask.Wait();

                // 輸出通訊結果
                Console.WriteLine(connectTask.Result.Message);

                Console.WriteLine($"檢查通訊是否成功建立: {_tcpCore.IsOnline}");

            }

        }

        private void disconnect_btn_Click(object sender, EventArgs e)
        {
            if (_tcpCore != null) _tcpCore.DisconnectAsync();
        }
    }
}
