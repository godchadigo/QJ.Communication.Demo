using QJ.Communication.Core.Cores.Tcp;
using QJ.Communication.Core.Enums;
using QJ.Communication.Core.Extension;
using QJ.Communication.Core.Model.Plugin;
using QJ.Communication.Core.Model.Result;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;

namespace QJ.Communication.Core.Tool
{
    public class PlcBatchReadTool : IDisposable
    {
        private List<CommJunctionModel> commJunctionList = new List<CommJunctionModel>();
        private ConcurrentDictionary<string, List<JunctionVariable>> junctionVariableGroup = new ConcurrentDictionary<string, List<JunctionVariable>>();
        private EndianType _formate;
        private readonly ILogger _logger;
        private bool isDebug = false;
        public PlcBatchReadTool(ILogger logger, EndianType formate = EndianType.LittleSwap)
        {
            this._formate = formate;
            _logger = logger;
        }

        public List<CommJunctionModel> CreateCommJunctionList(List<VariableReadTag> rawVariableList)
        {

            //原則
            //0.將原始變數解析成中繼變數
            //1.分類
            //2.排大小
            //3.間距截斷
            //4.排出列表

            //0.將原始變數解析成中繼變數
            foreach (var rawVariable in rawVariableList)
            {
                var junctionVariable = new JunctionVariable();
                //junctionVariable.Uuid = rawVariable.Uuid;
                junctionVariable.VariableName = rawVariable.VariableName;
                junctionVariable.TagName = rawVariable.TagName;

                junctionVariable.VariableType = rawVariable.VariableType;
                //junctionVariable.VariableLength = rawVariable.VariableLength;
                junctionVariable.VariableHeader = junctionVariable.VariableHeader;
                junctionVariable.VariableBody = junctionVariable.VariableBody;
                //junctionVariable.VariableLength = 1;
                junctionVariable.VariableLength = rawVariable.Length;
                junctionVariable.Divisor = rawVariable.Divisor;
                //junctionVariable.BindingUuid = rawVariable.BindingUuid;
                //junctionVariable.EqualsString = rawVariable.EqualsString;


                //1.分類
                if (junctionVariableGroup.ContainsKey(junctionVariable.VariableHeader))
                {
                    // 如果鍵已存在，將變量添加到現有的列表中
                    junctionVariableGroup[junctionVariable.VariableHeader].Add(junctionVariable);
                }
                else
                {
                    // 如果鍵不存在，創建新列表並將變量添加到其中，然後添加到字典中
                    var newList = new List<JunctionVariable> { junctionVariable };
                    junctionVariableGroup[junctionVariable.VariableHeader] = newList;
                }
            }

            //2.排大小
            foreach (var junctionVariableList in junctionVariableGroup)
            {
                var sortList = junctionVariableList.Value.OrderBy(x => x.VariableBody).ToList();
                junctionVariableGroup[junctionVariableList.Key] = sortList;

                //3.間距截斷                
                var res = SplitVariableList(sortList);
                commJunctionList.AddRange(res);
            }

            return commJunctionList;
        }

        private List<CommJunctionModel> SplitVariableList(List<JunctionVariable> sortList, int startIndex = 0)
        {
            List<CommJunctionModel> commJunctionModels = new List<CommJunctionModel>();
            short splitSpace = 128;
            JunctionVariable tempStartVariable = sortList[startIndex];

            for (int i = startIndex; i < sortList.Count; i++)
            {
                var junctionVariable = sortList[i];
                var commJunctionModel = new CommJunctionModel();

                // 列表只有一個元素時直接新增
                if (sortList.Count == 1)
                {
                    //var 當前讀取長度 = (ushort)(junctionVariable.VariableBody + GetReadLength(junctionVariable.VariableType, junctionVariable.VariableLength));
                    var 當前讀取長度 = (ushort)(GetReadLength(junctionVariable.VariableType, junctionVariable.VariableLength));
                    commJunctionModel.Uuid = Guid.NewGuid().ToString();
                    commJunctionModel.StartAddress = junctionVariable.TagName;
                    commJunctionModel.ReadLength = 當前讀取長度;
                    commJunctionModel.VariableType = junctionVariable.VariableType;
                    commJunctionModel.RawJunctionVariables = new List<JunctionVariable> { junctionVariable };
                    commJunctionModels.Add(commJunctionModel);
                    return commJunctionModels;
                }

                // 保護判斷，確保不會越界
                if (i + 1 < sortList.Count)
                {
                    var nextJunctionVariable = sortList[i + 1];
                    var 和後面的變數相差長度 = (ushort)(nextJunctionVariable.VariableBody + GetReadLength(nextJunctionVariable.VariableType, nextJunctionVariable.VariableLength) - junctionVariable.VariableBody);
                    var 當前讀取長度 = (ushort)(junctionVariable.VariableBody - tempStartVariable.VariableBody + GetReadLength(junctionVariable.VariableType, junctionVariable.VariableLength));

                    // 如果加上下一个变量会超出分割长度，则创建当前分割
                    if (和後面的變數相差長度 >= splitSpace || 當前讀取長度 >= splitSpace)
                    {
                        commJunctionModel.Uuid = Guid.NewGuid().ToString();
                        commJunctionModel.StartAddress = tempStartVariable.TagName;
                        commJunctionModel.ReadLength = 當前讀取長度;
                        commJunctionModel.VariableType = tempStartVariable.VariableType;
                        commJunctionModel.RawJunctionVariables = sortList.Skip(startIndex).Take(i - startIndex + 1).ToList();
                        commJunctionModels.Add(commJunctionModel);

                        // 处理剩下的部分
                        commJunctionModels.AddRange(SplitVariableList(sortList, i + 1));
                        return commJunctionModels;  // 确保递归结果正确返回
                    }
                }

                // 如果到最后一个元素，直接创建分割
                if (i == sortList.Count - 1)
                {
                    var 當前讀取長度 = (ushort)(junctionVariable.VariableBody - tempStartVariable.VariableBody + GetReadLength(junctionVariable.VariableType, junctionVariable.VariableLength));
                    commJunctionModel.Uuid = Guid.NewGuid().ToString();
                    commJunctionModel.StartAddress = tempStartVariable.TagName;
                    commJunctionModel.ReadLength = 當前讀取長度;
                    commJunctionModel.VariableType = tempStartVariable.VariableType;
                    commJunctionModel.RawJunctionVariables = sortList.Skip(startIndex).Take(i - startIndex + 1).ToList();
                    commJunctionModels.Add(commJunctionModel);
                }
            }

            return commJunctionModels;
        }
        
        public async Task<QJResult<List<QJResult<VariableReadTag>>>> ExecuteReadAsync(List<CommJunctionModel> junctionList, QJPluginBase commPlugin)
        {
            List<QJResult<VariableReadTag>> variables = new List<QJResult<VariableReadTag>>();
            if (commPlugin == null) return variables.QJDataResponse(isOk: false);

            foreach (var junctionVariable in junctionList)
            {
                QJResult<List<byte>> _ = new QJResult<List<byte>>();
                if (junctionVariable.VariableType == QJDataTypeEnum.Boolean)
                {
                    List<byte> _boolData = new List<byte>();

                    QJResult<List<bool>> boolRes = new QJResult<List<bool>>();
                    if (commPlugin is QJTcpPluginBase tcpPlugin)
                    {
                        boolRes = await tcpPlugin.ReadBoolAsync(junctionVariable.StartAddress.SplitPlcTagString().header, junctionVariable.StartAddress.SplitPlcTagString().number, junctionVariable.ReadLength);
                        if (isDebug) _logger.Information($"[junctionVariable] ReadBit {tcpPlugin.IpAddress}:{tcpPlugin.Port} 讀取變數：{junctionVariable.StartAddress}，長度：{junctionVariable.ReadLength}，結果：{boolRes.IsOk}，消息：{_.Message}");
                    }

                    if (commPlugin is QJTcpPluginBase serialPlugin)
                    {
                        boolRes = await serialPlugin.ReadBoolAsync(junctionVariable.StartAddress.SplitPlcTagString().header, junctionVariable.StartAddress.SplitPlcTagString().number, junctionVariable.ReadLength);
                    }


                    if (boolRes.IsOk)
                    {
                        foreach (var bits in boolRes.Data)
                        {
                            _boolData.Add((byte)(bits ? 1 : 0));
                        }
                        _ = _boolData.QJDataResponse(isOk: true);
                    }
                    else
                    {
                        _.IsOk = boolRes.IsOk;
                        _.Message = boolRes.Message;
                    }

                }
                else
                {
                    if (commPlugin is QJTcpPluginBase tcpPlugin)
                    {
                        _ = await tcpPlugin.ReadAsync(junctionVariable.StartAddress.SplitPlcTagString().header, junctionVariable.StartAddress.SplitPlcTagString().number, junctionVariable.ReadLength);
                        if (isDebug) _logger.Information($"[junctionVariable] ReadWord {tcpPlugin.IpAddress}:{tcpPlugin.Port} 讀取變數：{junctionVariable.StartAddress}，長度：{junctionVariable.ReadLength}，結果：{_.IsOk}，消息：{_.Message}");
                    }
                    if (commPlugin is QJSerialPluginBase serialPlugin)
                    {
                        _ = await serialPlugin.ReadAsync(junctionVariable.StartAddress.SplitPlcTagString().header, junctionVariable.StartAddress.SplitPlcTagString().number, junctionVariable.ReadLength);
                    }
                }



                if (_.IsOk)
                {
                    var data = _.Data;
                    var byteBlock = new ValueByteBlock(data.ToArray());
                   
                    var _pointer = 0;
                    foreach (var rawJunctionVariable in junctionVariable.RawJunctionVariables.OrderBy(x => x.VariableBody))
                    {
                        var header = rawJunctionVariable.VariableHeader;
                        var number = rawJunctionVariable.VariableBody;
                        var len = rawJunctionVariable.VariableLength;
                        var type = rawJunctionVariable.VariableType;

                        var baseAdr = junctionVariable.StartAddress.SplitPlcTagString().number;

                        string _value = string.Empty;

                        var realAdr = 0;
                        var _len = 0;

                        if (type == QJDataTypeEnum.Boolean)
                        {
                            realAdr = (number - baseAdr) * 1;
                            _len = len;
                        }
                        else{
                            realAdr = (number - baseAdr) * 2;
                            _len = len;
                        }
                        
                        // 真實索引，Byte數據起點

                        _pointer = realAdr;

                                               
                        for (int i = 0; i < _len; i++)
                        {
                            // 移動指針到數據起始片段
                            var byteBlockMemory = byteBlock.Span.Slice(_pointer);

                            var bk = (_len == i + 1) ? "" : ",";
                            // 判斷類型
                            switch (type)
                            {
                                case QJDataTypeEnum.Boolean:
                                    var _cnt = 0;
                                    _value += byteBlockMemory.ReadValue<bool>().ToString().ToLower() + bk;
                                    _pointer += 1;

                                    break;
                                case QJDataTypeEnum.UINT16:                                    
                                    _value += byteBlockMemory.ReadValue<UInt16>(_formate).ToString() + bk;
                                    _pointer += 2;
                                    break;
                                case QJDataTypeEnum.INT16:
                                    _value += byteBlockMemory.ReadValue<Int16>(_formate).ToString() + bk;
                                    _pointer += 2;
                                    break;
                                case QJDataTypeEnum.UINT32:
                                    _value += byteBlockMemory.ReadValue<UInt32>(_formate).ToString() + bk;
                                    _pointer += 4;
                                    break;
                                case QJDataTypeEnum.INT32:
                                    _value += byteBlockMemory.ReadValue<int>(_formate).ToString() + bk;
                                    _pointer += 4;
                                    break;
                                case QJDataTypeEnum.UINT64:
                                    _value += byteBlockMemory.ReadValue<UInt64>(_formate).ToString() + bk;
                                    _pointer += 8;
                                    break;
                                case QJDataTypeEnum.INT64:
                                    _value += byteBlockMemory.ReadValue<Int64>(_formate).ToString() + bk;
                                    _pointer += 8;
                                    break;
                                case QJDataTypeEnum.FLOAT:
                                    _value += byteBlockMemory.ReadValue<float>(_formate).ToString() + bk;
                                    _pointer += 4;
                                    break;
                                case QJDataTypeEnum.DOUBLE:
                                    _value += byteBlockMemory.ReadValue<double>(_formate).ToString() + bk;
                                    _pointer += 8;
                                    break;
                            }
                            
                        }

                        // 添加數據到緩衝區
                        if (rawJunctionVariable is VariableReadTag variable)
                        {
                            variable.VariableName = rawJunctionVariable.VariableName;
                            variable.TagName = rawJunctionVariable.TagName;
                            variable.VariableType = rawJunctionVariable.VariableType;
                            variable.Divisor = rawJunctionVariable.Divisor;
                            //variable.BindingUuid = rawJunctionVariable.BindingUuid;
                            //variable.EqualsString = rawJunctionVariable.EqualsString;
                            variable.TagValue = _value;
                            variables.Add(variable.QJDataResponse(isOk: true, message: "讀取成功!"));
                        }
                    }
                }
                else
                {
                    foreach (var rawJunctionVariable in junctionVariable.RawJunctionVariables)
                    {
                        VariableReadTag variable = new VariableReadTag();
                        //base
                        //variable.Uuid = rawJunctionVariable.Uuid;
                        //variable.DeviceUuid = rawJunctionVariable.DeviceUuid;
                        variable.VariableName = rawJunctionVariable.VariableName;
                        variable.TagName = rawJunctionVariable.TagName;
                        //variable.Description = rawJunctionVariable.Description;
                        variable.VariableType = rawJunctionVariable.VariableType;
                        variable.TagValue = "fail!";
                        //variable.isOnline = _.IsSuccess;
                        //variables.Add(variable);
                        variables.Add(variable.QJDataResponse(isOk: false, message: "讀取錯誤!"));
                    }
                    return variables.QJDataResponse(isOk: false);
                }

            }
            //寫入成功封包，判定通訊成功!

            return variables.QJDataResponse(isOk: true);
        }

        /// <summary>
        /// 獲取讀取長度，底層解析由Word為基底。(16bit)
        /// </summary>
        /// <returns></returns>
        private short GetReadLength(QJDataTypeEnum type, short rawLength)
        {

            return ((short)(GetTypeLength(type) * rawLength));
        }
        /// <summary>
        /// 獲取數據類型占用長度
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private short GetTypeLength(QJDataTypeEnum type)
        {
            short length = 0;
            switch (type)
            {
                case QJDataTypeEnum.INT16:
                case QJDataTypeEnum.UINT16:
                case QJDataTypeEnum.STRING:
                case QJDataTypeEnum.Boolean:
                    length = 1;
                    break;
                case QJDataTypeEnum.INT32:
                case QJDataTypeEnum.UINT32:
                case QJDataTypeEnum.FLOAT:
                    length = 2;
                    break;
                case QJDataTypeEnum.INT64:
                case QJDataTypeEnum.UINT64:
                case QJDataTypeEnum.DOUBLE:
                    length = 4;
                    break;
            }
            return length;
        }

        public void Dispose()
        {
            commJunctionList.Clear();
            junctionVariableGroup.Clear();
        }
    }

    public class VariableReadTag
    {
        private string _tagName;
        private QJDataTypeEnum _type = QJDataTypeEnum.Empty;
        public string VariableName { get; set; }
        /// <summary>
        /// 完整位址，例如 "DM100"、"W0"、"R500"
        /// 設定時會自動解析 RegionHeader 和 AddressNumber
        /// </summary>
        public string TagName
        {
            get => _tagName;
            set
            {
                _tagName = value;
                if (!string.IsNullOrEmpty(value))
                {
                    var parsed = value.SplitPlcTagString();
                    VariableHeader = parsed.header;
                    VariableBody = parsed.number;
                }
            }
        }
        /// <summary>
        /// 變數頭
        /// 舉例DM100 => DM代表頭
        /// </summary>
        public string VariableHeader { get; set; }
        /// <summary>
        /// 變數身體
        /// 舉例DM100 => 100代表身體
        /// </summary>
        public ushort VariableBody { get; set; }
        public short Length { get; set; }
        public string TagValue { get; set; }
        /// <summary>
        /// 除數
        /// </summary>
        public float Divisor { get; set; }
        public QJDataTypeEnum VariableType
        {
            get => _type;
            set
            {
                if (value == QJDataTypeEnum.Empty ||
                    value == QJDataTypeEnum.BooleanArray ||
                    value == QJDataTypeEnum.INT16Array ||
                    value == QJDataTypeEnum.INT32Array ||
                    value == QJDataTypeEnum.INT64Array ||
                    value == QJDataTypeEnum.STRINGArray ||
                    value == QJDataTypeEnum.DOUBLEArray)
                    throw new ArgumentException($"QJCommunication 暫不支持這個類型{value}");
                _type = value;
            }
        }

        public override string ToString()
        {
            return $"變數名稱:{VariableName} 標籤:{TagName} 長度:{Length} 數值:{TagValue}";
        }
    }
    public class JunctionVariable : VariableReadTag
    {
        public string Uuid { get; set; }
        
        public string Description { get; set; }        
        public short VariableLength { get; set; }

    }

    public class CommJunctionModel
    {
        public string Uuid { get; set; }
        public string StartAddress { get; set; }
        public ushort ReadLength { get; set; }
        public QJDataTypeEnum VariableType { get; set; }
        public List<JunctionVariable> RawJunctionVariables { get; set; }
    }


}
