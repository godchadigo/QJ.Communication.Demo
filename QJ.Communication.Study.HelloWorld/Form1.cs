using QJ.Communication.Core;
using QJ.Communication.Core.Cores.Tcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QJ.Communication.Study.HelloWorld
{
    public partial class Form1 : Form
    {
        private QJManager _QJManager;
        private TcpCore _tcpCore;
        private bool _isReady = false;
        private BindingList<KeyValueItem> dataList;
        private CancellationTokenSource _autoRefreshCTS;

        private void Form1_Load(object sender, EventArgs e)
        {
            InitListView();
        }
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

        #region QJCommunication
        private void InitQJCommunication()
        {
            _QJManager = new QJManager();
            _QJManager.Init();
        }

        #endregion

        #region InitComponent
        private void InitListView()
        {
            // 設定 ListView
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
           
            // 新增欄位
            listView1.Columns.Add("地址",200);
            listView1.Columns.Add("數值",200);

            // 初始化 BindingList
            dataList = new BindingList<KeyValueItem>();
            dataList.ListChanged += DataList_ListChanged;

        }
        #endregion

        #region ListView Control
        // 自動更新 ListView
        private void DataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.BeginInvoke(new Action(delegate {
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        var newItem = dataList[e.NewIndex];
                        var lvi = new ListViewItem(newItem.Key);
                        lvi.SubItems.Add(newItem.Value);
                        lvi.Tag = newItem;
                        listView1.Items.Insert(e.NewIndex, lvi);
                        break;

                    case ListChangedType.ItemDeleted:
                        if (e.NewIndex < listView1.Items.Count)
                            listView1.Items.RemoveAt(e.NewIndex);
                        break;

                    case ListChangedType.ItemChanged:
                        if (e.NewIndex < listView1.Items.Count)
                        {
                            var item = dataList[e.NewIndex];
                            listView1.Items[e.NewIndex].Text = item.Key;
                            listView1.Items[e.NewIndex].SubItems[1].Text = item.Value;
                        }
                        break;

                    case ListChangedType.Reset:
                        listView1.Items.Clear();
                        break;
                }
            } ));
            
        }

        // 新增項目
        public void AddItem(string key, string value)
        {
            dataList.Add(new KeyValueItem(key, value));
        }

        // 更新項目 (根據 Key)
        public void UpdateItem(string key, string newValue)
        {
            var item = dataList.FirstOrDefault(x => x.Key == key);
            if (item != null)
            {
                item.Value = newValue; // 自動觸發更新
            }
            else
            {
                               // 如果找不到，則新增
                AddItem(key, newValue);
            }
        }

        // 移除項目
        public void RemoveItem(string key)
        {
            var item = dataList.FirstOrDefault(x => x.Key == key);
            if (item != null)
            {
                dataList.Remove(item);
            }
        }

        // 清空
        public void Clear()
        {
            dataList.Clear();
        }

        #endregion

        #region ConnectionBox
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


        #region 用戶區
        /// <summary>
        /// 讀取4x0 10筆數據
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void read_btn_Click(object sender, EventArgs e)
        {
            if (_tcpCore.IsOnline)
            {
                Task.Run(() => AutoRefreshData(!autoRefrresh_chechbox.Checked));
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
                        for (int i = 0; i < values.Count; i++)
                        {
                            UpdateItem($"4x{i}", values[i].ToString());
                        }
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

        #endregion

        private void autoRefrresh_chechbox_CheckedChanged(object sender, EventArgs e)
        {

            if (!autoRefrresh_chechbox.Checked)
            {
                if (_autoRefreshCTS != null) _autoRefreshCTS.Cancel();
            }
        }
    }
}
