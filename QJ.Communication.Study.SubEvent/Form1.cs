using QJ.Communication.Core.Cores;
using QJ.Communication.Core.Cores.Tcp;
using QJ.Communication.Core.Extension;
using QJ.Communication.Core.Interface;
using QJ.Communication.Core.Model.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QJ.Communication.Study.SubEvent
{
    public partial class Form1 : Form
    {
        private QJManager _QJManager;
        private TcpCore _tcpCore;
        private bool _isReady = false;
        private CancellationTokenSource _autoRefreshCTS;
        public Form1()
        {
            InitializeComponent();

            // 第一步 初始化核心
            InitQJCommunication();

            // 第二步 使用TcpCore實例化插件
            _tcpCore = new TcpCore("ModbusTcpV2", out bool initResult);
            _isReady = initResult;
            if (!_isReady) MessageBox.Show("插件初始化失敗，請檢察插件是否正確載入!");

            // 綁定事件
            SubEvent(_tcpCore.qJPlugin);

            // 初始化表格
            dataGridView1.GridInit();
        }

        #region QJCommunication
        private void InitQJCommunication()
        {
            _QJManager = new QJManager();
            _QJManager.Init();
        }

        #endregion

        #region 事件訂閱
        private void SubEvent(IQJPlugin pluginBase)
        {
            #region Tcp插件事件綁定
            if (pluginBase is QJTcpPluginBase tcpPlugin)
            {
                tcpPlugin.OnConnected = async (_plugin, trigDate) =>
                {
                    var plugin = _plugin as QJTcpPluginBase;
                    dataGridView1.AddRow("連線事件", $"成功連線到{plugin.IpAddress}:{plugin.Port}", trigDate);
                };

                tcpPlugin.OnClosed = async (_plugin, trigDate) =>
                {
                    var plugin = _plugin as QJTcpPluginBase;
                    dataGridView1.AddRow("斷線事件", $"{plugin.IpAddress}:{plugin.Port} 離線!", trigDate);
                };

                tcpPlugin.OnDataReceived = async (reciver, _plugin, trigDate) =>
                {
                    var plugin = _plugin as QJTcpPluginBase;
                    dataGridView1.AddRow("<<接收事件", $"{reciver.RecivedRawData.ToHexString()}", trigDate);
                };

                tcpPlugin.OnDataSend = async (data, len, _plugin, trigDate) =>
                {
                    var plugin = _plugin as QJTcpPluginBase;
                    dataGridView1.AddRow(">>發送事件", $"{data.ToHexString()}", trigDate);
                };
            }

            #endregion

            #region 串口插件事件綁定
            if (pluginBase is QJSerialPluginBase serialPlugin)
            {
                serialPlugin.OnConnected = async (_plugin, trigDate) =>
                {
                    var plugin = _plugin as QJSerialPluginBase;
                    dataGridView1.AddRow("連線事件", $"成功連線到{plugin.serialProp.PortName}", trigDate);
                };

                serialPlugin.OnClosed = async (_plugin, trigDate) =>
                {
                    var plugin = _plugin as QJSerialPluginBase;
                    dataGridView1.AddRow("斷線事件", $"{plugin.serialProp.PortName} 斷開連線", trigDate);
                };
            }
            #endregion

            
        }
        #endregion

        #region 連線 斷線控制
        private void connect_btn_Click(object sender, EventArgs e)
        {
            if (!_isReady) MessageBox.Show("插件初始化失敗，請檢察插件是否正確載入!");

            // 第三步 配置TcpCore參數            
            if (_isReady)
            {
                var ip = ip_tbox.Text;
                var _port = int.TryParse(port_tbox.Text, out int port);

                // 第四步 使用TcpCore進行建立連線
                var connectTask = Task.Run(() => _tcpCore.ConnectAsync(ip, port, 5000));
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
        #endregion

        private void read_btn_Click(object sender, EventArgs e)
        {
            if (_tcpCore.IsOnline)
            {
                Task.Run(() => AutoRefreshData(true));
            }
            else
            {
                MessageBox.Show($"目前與伺服器離線中!");
            }
        }
        private async Task AutoRefreshData(bool isSingle)
        {
            _autoRefreshCTS = new CancellationTokenSource();
            var token = _autoRefreshCTS.Token;
            while (!token.IsCancellationRequested)
            {
                if (_tcpCore.IsOnline)
                {
                    var plugin = _tcpCore.GetPluginBase();
                    var readResult = plugin.ReadInt16("4x", 0, 10);
                    if (readResult.IsOk)
                    {
                        var values = readResult.Data;
#if false
                        for (int i = 0; i < values.Count; i++)
                        {
                            UpdateItem($"4x{i}", values[i].ToString());
                        }
#endif
                    }
                }
                if (isSingle)
                {
                    _autoRefreshCTS.Cancel();
                    break;
                }
                await Task.Delay(1000, token); // 每1秒刷新一次
            }
        }
    }

    public static class GridExtensions
    {
        public static void GridInit(this DataGridView grid)
        {
            // ** 添加欄位 ** //
            grid.Columns.Add("事件名稱", "事件名稱");
            grid.Columns.Add("訊息", "訊息");
            grid.Columns.Add("發生時間", "發生時間");


            // ** 設定表格屬性 ** //
            grid.AllowUserToDeleteRows = false;                                 //不允許用戶刪除列
            grid.AllowUserToOrderColumns = false;                               //不允許用戶重新排列
            grid.AllowUserToResizeColumns = false;                              //不允許用戶改變列寬
            grid.AllowUserToAddRows = false;                                    //不允許用戶添加列
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;    //自動調整列寬
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;      //自動調整行寬
            grid.ReadOnly = true;                                               //只讀模式
        }
        public static void AddRow(this DataGridView grid, string eventName, string msg, DateTime date, bool invoke = true)
        {
            if (invoke)
            {
                grid.BeginInvoke(new Action(delegate {
                    grid.Rows.Add(eventName, msg, date.ToString("HH:mm:ss"));
                }));
            }
            else
            {
                grid.Rows.Add(eventName, msg, date.ToString("HH:mm:ss"));
            }
        }

    }
}
