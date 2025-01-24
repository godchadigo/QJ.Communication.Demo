# QJ.Communication.Core

### 這是甚麼?
- 專為工業通訊而生的框架
- 支援Net Framework 4.7.2 4.8 | Core 6.0 8.0
- 目前僅支持常見的讀寫類型設計，PLC專用的功能暫不規劃。

- 如果你是使用者則:
```text
你可以很快速地使用Core來達成與設備的通訊。
```
- 如果你是開發者則:
```text
- 你可以透過Core提供的介面，並且複寫方法重寫通訊指令，來達成快速架構一組通訊協議。
- 你可以撰寫自定義的授權模式來鎖定通訊插件
- 未來會提供一組範例，供開發者參考
```
---

### 目前支持的方法
- 以下的方法均使用abstract抽象類型，而Tcp核心類別會繼承以下介面，並且virtual完以下方法，這樣的好處是插件開發者忘記開發對應功能，不會crash而是會拋出NotImplementedException異常，讓使用者知道插件未開發這個功能。
- 下列的方法均能在插件中overrider重寫功能。

## 讀取(同步)
```c#
  abstract QJResult<List<byte>> Read(string varFunc, ushort address, ushort length);
  abstract QJResult<List<bool>> ReadBool(string varFunc, ushort address, ushort length);
  abstract QJResult<List<ushort>> ReadUInt16(string varFunc, ushort address, ushort length);
  abstract QJResult<List<short>> ReadInt16(string varFunc, ushort address, ushort length);
  abstract QJResult<List<uint>> ReadUInt32(string varFunc, ushort address, ushort length);
  abstract QJResult<List<int>> ReadInt32(string varFunc, ushort address, ushort length);
  abstract QJResult<List<ulong>> ReadUInt64(string varFunc, ushort address, ushort length);
  abstract QJResult<List<long>> ReadInt64(string varFunc, ushort address, ushort length);
  abstract QJResult<List<float>> ReadFloat(string varFunc, ushort address, ushort length);
  abstract QJResult<List<double>> ReadDouble(string varFunc, ushort address, ushort length);
```

## 讀取(非同步)
```c#
 abstract Task<QJResult<List<byte>>> ReadAsync(string varFunc, ushort address, ushort length);
 abstract Task<QJResult<List<bool>>> ReadBoolAsync(string varFunc, ushort address, ushort length);
 abstract Task<QJResult<List<ushort>>> ReadUInt16Async(string varFunc, ushort address, ushort length);
 abstract Task<QJResult<List<short>>> ReadInt16Async(string varFunc, ushort address, ushort length);
 abstract Task<QJResult<List<uint>>> ReadUInt32Async(string varFunc, ushort address, ushort length);
 abstract Task<QJResult<List<int>>> ReadInt32Async(string varFunc, ushort address, ushort length);
 abstract Task<QJResult<List<ulong>>> ReadUInt64Async(string varFunc, ushort address, ushort length);
 abstract Task<QJResult<List<long>>> ReadInt64Async(string varFunc, ushort address, ushort length);
 abstract Task<QJResult<List<float>>> ReadFloatAsync(string varFunc, ushort address, ushort length);
 abstract Task<QJResult<List<double>>> ReadDoubleAsync(string varFunc, ushort address, ushort length);
```
## 寫入(同步)
```c#
    #region bool
    abstract QJResult Write(string varFunc, ushort address, bool value);
    abstract QJResult Write(string varFunc, ushort address, bool[] values);
    abstract QJResult Write(string varFunc, ushort address, IEnumerable<bool> values);
    #endregion

    #region ushort
    abstract QJResult Write(string varFunc, ushort address, ushort value);
    abstract QJResult Write(string varFunc, ushort address, ushort[] values);
    abstract QJResult Write(string varFunc, ushort address, IEnumerable<ushort> values);
    #endregion

    #region short
    abstract QJResult Write(string varFunc, ushort address, short value);
    abstract QJResult Write(string varFunc, ushort address, short[] values);
    abstract QJResult Write(string varFunc, ushort address, IEnumerable<short> values);
    #endregion

    #region uint
    abstract QJResult Write(string varFunc, ushort address, uint value);
    abstract QJResult Write(string varFunc, ushort address, uint[] values);
    abstract QJResult Write(string varFunc, ushort address, IEnumerable<uint> values);
    #endregion

    #region int
    abstract QJResult Write(string varFunc, ushort address, int value);
    abstract QJResult Write(string varFunc, ushort address, int[] values);
    abstract QJResult Write(string varFunc, ushort address, IEnumerable<int> values);
    #endregion

    #region ulong
    abstract QJResult Write(string varFunc, ushort address, ulong value);
    abstract QJResult Write(string varFunc, ushort address, ulong[] values);
    abstract QJResult Write(string varFunc, ushort address, IEnumerable<ulong> values);
    #endregion

    #region long
    abstract QJResult Write(string varFunc, ushort address, long value);
    abstract QJResult Write(string varFunc, ushort address, long[] values);
    abstract QJResult Write(string varFunc, ushort address, IEnumerable<long> values);
    #endregion

    #region float
    abstract QJResult Write(string varFunc, ushort address, float value);
    abstract QJResult Write(string varFunc, ushort address, float[] values);
    abstract QJResult Write(string varFunc, ushort address, IEnumerable<float> values);
    #endregion

    #region double
    abstract QJResult Write(string varFunc, ushort address, double value);
    abstract QJResult Write(string varFunc, ushort address, double[] values);
    abstract QJResult Write(string varFunc, ushort address, IEnumerable<double> values);
    #endregion

    #region string
    abstract QJResult Write(string varFunc, ushort address, string str, EncodingType encode);

    #endregion
```
## 寫入(非同步)
```c#
   #region bool
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, bool value);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, bool[] values);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<bool> values);
   #endregion

   #region ushort
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, ushort value);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, ushort[] values);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<ushort> values);
   #endregion

   #region short
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, short value);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, short[] values);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<short> values);
   #endregion

   #region uint
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, uint value);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, uint[] values);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<uint> values);
   #endregion

   #region int
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, int value);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, int[] values);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<int> values);
   #endregion

   #region ulong
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, ulong value);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, ulong[] values);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<ulong> values);
   #endregion

   #region long
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, long value);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, long[] values);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<long> values);
   #endregion

   #region float
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, float value);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, float[] values);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<float> values);
   #endregion

   #region double
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, double value);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, double[] values);
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, IEnumerable<double> values);
   #endregion

   #region string
   abstract Task<QJResult> WriteAsync(string varFunc, ushort address, string str, EncodingType encode);

   #endregion
```

## 核心原理 
- 使用者對TcpCore或SerialCore下達讀或寫命令。Core會去呼叫介面對應插件的方法，並使用插件內的方法做執行(前提是插件有override該方法)，插件內部會將請求整理好，並反過來向Core請求Tcp Send，當請求成功時設備會回覆封包，core也會Recived該封包並解析，最後將解析完的結果歸還給使用者。
- ![image](https://github.com/user-attachments/assets/55780c12-bc30-402e-97ff-c21b277d77c4)


## 使用者必看
### 使用說明
1. 創建專案
2. 加入參考 QJ.Communication.Core.dll
3. 打開專案nuget搜尋Touchsocket，安裝Touchsocket以及Touchsocket.Core
4. 在專案輸出位置，通常在(bin/Debug或bin/Release)，新增一個plugins資料夾，將想要使用的通訊插件放入。

- 小技巧，當請求完Send或是Write時，都會返回QJResult<T>結果，QJResult的封裝有興趣可以去看原始碼，這邊要解釋的是可以透過IsOk來判斷是否請求成功。
```c#
 // 請求讀取有符號16位元4x100的1筆數據
 res = plugin.ReadInt16("4x", 100, 1);
 if (res.IsOk) Console.WriteLine(res.Data[0]); //判斷請求成功打印結果
 Console.WriteLine(res.Message);
```

### 開發者必看
- 開啟專案 "開發板模" 加入QJ.Communication.Core專案 參考。
1. 新增新專案，選擇 新增類別庫 ，注意 注意 注意 專案名稱請一定要以"QJ.Communication.Tcp" 或"QJ.Communication.Serial"為開頭，為了後期快速測試建議開發者遵循該命名規範，版本選擇netstandard2.0，這樣就可以下一步完成了。
2. 複製插件板模的5個資料夾以及1個主程式檔案(這樣就是一個完整的插件專案) :
- Adapter    //適配器
- Common     //通用方法
- Enums      //Enum列表
- Exceptions //異常處理
- Interface  //介面
- AnyName.cs //主程式

## 實例分享
### Modbus批量寫入
![image](https://github.com/user-attachments/assets/d900253b-c0ca-4e7c-b8cf-47fb33a7054b)

## 聲明
- 由於該專案屬於實驗性功能專案，請不要再實際機台做測試以及商業使用。
- 插件開發者的部分暫時不開放，等資料完善後再作規劃。
- 提供的插件均經過加密。使用者無法看到底層實現原理。但能夠正常使用方法
- 目前專案插件會加上24hr時效檢測，未來可能會商業化，所以暫時閉源僅提供使用方法。
- 本框架底層使用[Touchsocket](https://github.com/RRQM/TouchSocket)網路工具開源專案。

## 第三方套件聲明
- 本套件使用Touchsocket已經向Touchsocket購買完整授權。
