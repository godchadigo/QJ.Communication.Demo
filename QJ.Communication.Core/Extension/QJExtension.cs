using QJ.Communication.Core.Model.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Resources;

namespace QJ.Communication.Core.Extension
{
    public static class QJExtension
    {
        /// <summary>
        /// 資料解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Obsolete]
        public static QJResult<T> QJDataAnalyze<T>(this QJResult<T> obj)
        {
            if (obj.Data == null)
            {
                obj.IsOk = false;
                obj.Message = "資料為空!";
                return obj;
            }
            obj.IsOk = true;
            return obj;
        }

        /// <summary>
        /// 將任意類型資料轉為QJResult結果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <param name="isOk"></param>
        /// <returns></returns>
        public static QJResult<T> QJDataResponse<T>(this T obj,  string message = "", bool isOk = false)
        {
            
            QJResult<T> res = new QJResult<T>();


            // 使用模式匹配检查实例类型
            if (obj is bool boolValue)
            {
                res.IsOk = boolValue;
            }
            else
            {
                res.IsOk = isOk;
            }
            res.Data = obj;            
            res.Message = message;
            //return res.QJDataAnalyze();
            return res;
        }

        /// <summary>
        /// 生成一個QJResult結果
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isOk"></param>
        /// <returns></returns>
        public static QJResult QJDataResponse(string message = "", bool isOk = false)
        {
            QJResult res = new QJResult();
            res.IsOk = isOk;
            res.Message = message;
            return res;
        }
        public static QJResult QJDataResponse( bool isOk = false)
        {
            QJResult res = new QJResult();
            res.IsOk = isOk;
            if (isOk)
            {
                res.Message = "請求成功!";
            }
            else
            {
                res.Message = "請求失敗!";
            }
            return res;
        }
        public static QJResult ToQJResult<T>(this QJResult<T> data, bool isOk = false)
        {
            QJResult res = new QJResult();
            res.IsOk = data.IsOk;
            res.Data = data.Data;
            res.Message = data.Message;
            return res;
        }

        /// <summary>
        /// 將byte[]轉為任意List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="len"></param>
        /// <param name="_EndianType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<T> ToAnyList<T>(this byte[] data, ushort len, EndianType _EndianType) where T : struct
        {
            var valueBlock = new ValueByteBlock(data);
            var typeCode = Type.GetTypeCode(typeof(T));
            TouchSocketBitConverter converter = new TouchSocketBitConverter(_EndianType);
            switch (typeCode)
            {
                case TypeCode.Byte:
                    return data.Cast<T>().ToList();
                case TypeCode.Boolean:
                    return converter.ToValues<bool>(valueBlock.Span).ToArray().Cast<T>().Take(len).ToList();
                case TypeCode.Int32:
                    return converter.ToValues<Int32>(valueBlock.Span).ToArray().Cast<T>().Take(len).ToList();
                case TypeCode.UInt32:
                    return converter.ToValues<UInt32>(valueBlock.Span).ToArray().Cast<T>().Take(len).ToList();
                case TypeCode.Int64:
                    return converter.ToValues<Int64>(valueBlock.Span).ToArray().Cast<T>().Take(len).ToList();
                case TypeCode.UInt64:
                    return converter.ToValues<UInt64>(valueBlock.Span).ToArray().Cast<T>().Take(len).ToList();
                case TypeCode.Int16:
                    return converter.ToValues<Int16>(valueBlock.Span).ToArray().Cast<T>().Take(len).ToList();
                case TypeCode.UInt16:
                    return converter.ToValues<UInt16>(valueBlock.Span).ToArray().Cast<T>().Take(len).ToList();
                case TypeCode.Single:
                    return converter.ToValues<float>(valueBlock.Span).ToArray().Cast<T>().Take(len).ToList();
                case TypeCode.Double:
                    return converter.ToValues<double>(valueBlock.Span).ToArray().Cast<T>().Take(len).ToList();
                default:
                    throw new ArgumentException($"Unsupported type: {typeof(T).Name}");
            }
        }

        /// <summary>
        /// 獲取資資料請求命令數據類型長度
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int GetDataTypeMemoryLength(this Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Byte:
                    return 1;
                case TypeCode.Boolean:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return 2;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Single:
                    return 4;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Double:
                    return 8;
                default:
                    throw new ArgumentException($"Unsupported type: {type.Name}");
            }
        }

        /// <summary>
        /// 可將大部分PLC元件拆解成頭標籤以及編號
        /// 例如:
        /// D1000 => D 1000
        /// X100 => X 100
        /// SM400 => SM 400
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static (string header, ushort number) SplitPlcTagString(this string input)
        {
            // 正規表達式模式：^(.*?[^\d])(\d+)$
            // ^ 表示字符串的開始
            // (.*?[^\d]) 非貪婪地匹配任意字符，直到最後一個非數字字符，這是第一個捕獲組
            // (\d+) 匹配一個或多個數字，這是第二個捕獲組
            // $ 表示字符串的結束
            string pattern = @"^(.*?[^\d])(\d+)$";

            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                // 如果匹配成功，返回兩個捕獲組
                bool _res = ushort.TryParse(match.Groups[2].Value, out var value);
                return (_res) ? (match.Groups[1].Value.TrimEnd(), value) : (match.Groups[1].Value.TrimEnd(), (ushort)0);
            }
            else
            {
                // 如果沒有匹配，檢查是否整個字符串都是數字
                if (Regex.IsMatch(input, @"^\d+$"))
                {
                    bool _res = ushort.TryParse(input, out ushort value);
                    return (_res) ? ("", value) : ("", (ushort)0);

                }
                // 如果沒有匹配且不全是數字，返回原始字符串和空字符串
                return (input.TrimEnd(), (ushort)0);
            }
        }

        /// <summary>
        /// 將常見的資料型態類型字串，轉換為TypeCode對應類別
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static TypeCode ToTypeCode(this string str)
        {
            if (str == null) return TypeCode.Empty;
            switch (str.ToLower().Trim())
            {
                case "bool":
                case "bit":
                case "boolean":
                    return TypeCode.Boolean;
                case "ushort":
                case "uint16":
                    return TypeCode.UInt16;
                case "short":
                case "int16":
                    return TypeCode.Int16;
                case "uint":
                case "uint32":
                    return TypeCode.UInt32;
                case "int":
                case "int32":
                    return TypeCode.Int32;
                case "ulong":
                case "uint64":
                    return TypeCode.UInt64;
                case "long":
                case "int64":
                    return TypeCode.Int64;
                case "float":
                case "single":
                    return TypeCode.Single;
                case "double":
                    return TypeCode.Double;
                case "string":
                    return TypeCode.String;
                
            }
            return TypeCode.Empty;
        }
        /// <summary>
        /// Awaiter TimeOut超時方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="TimeoutException"></exception>
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout)
        {
            var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                return await task;
            }
            throw new TimeoutException();
        }
        /// <summary>
        /// 將Byte[]中的數據雙雙對調
        /// [1,2,3,4,5,6] => [2,1,4,3,6,5]
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SwapByteArray(this byte[] data)
        {
            // 创建新的数组来存储结果
            byte[] buffer = new byte[data.Length];

            // 每次处理两个字节，交换它们的位置
            for (int i = 0; i < data.Length; i += 2)
            {
                // 确保我们不会超出数组边界
                if (i + 1 < data.Length)
                {
                    // 交换相邻字节
                    buffer[i] = data[i + 1];
                    buffer[i + 1] = data[i];
                }
                else
                {
                    // 处理数组长度为奇数的情况
                    // 如果是奇数长度，最后一个字节不变
                    buffer[i] = data[i];
                }
            }

            return buffer;
        }
        /// <summary>
        /// 針對Touchsocket的WaitDataStatus擴展，封裝成QJResult結果返回
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static QJResult<bool> GetStatus(this WaitDataStatus status)
        {
            switch (status)
            {
                case WaitDataStatus.Success:
                    return true.QJDataResponse("成功!",true);
                case WaitDataStatus.Canceled:
                    return false.QJDataResponse("操作被使用者取消!");
                case WaitDataStatus.Overtime:
                    return false.QJDataResponse("超時!");
                case WaitDataStatus.Disposed:
                case WaitDataStatus.Default:
                default:
                    {                        
                        return false.QJDataResponse($"{(TouchSocketCoreResource.UnknownError)}");
                    }
            }
        }

        /// <summary>
        /// 將只讀byte轉換成字串
        /// </summary>
        /// <param name="memory"></param>
        /// <returns></returns>
        public static string ToHexString(this ReadOnlyMemory<byte> memory)
        {
            string str = string.Empty;
            for (global::System.Int32 i = 0; i < memory.Length; i++)
            {
                str += memory.ToArray()[i].ToString("X2") + " ";
            }

            return str;
        }
        /// <summary>
        /// 將Array類型的數據轉換成字串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memory"></param>
        /// <returns></returns>
        public static string ToHexString<T>(this Array memory)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in memory)
            {
                // 假設 T 可以轉換為數值
                sb.Append(Convert.ToInt32(item).ToString("X2"));
                sb.Append(" ");
            }
            return sb.ToString().TrimEnd();
        }
        /// <summary>
        /// 將IEmnuerable的類型轉換成字串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memory"></param>
        /// <returns></returns>
        public static string ToHexString<T>(this IEnumerable<T> memory,char fillChar = ' ')
        {
            int cnt = 0;
            int len = memory.Count();
            StringBuilder sb = new StringBuilder();
            foreach (var item in memory)
            {
                // 假設 T 可以轉換為數值
                sb.Append(Convert.ToInt32(item).ToString("X2"));

                // 判斷最後一個字符不用打印
                if (cnt < len-1)
                {
                    sb.Append(fillChar);
                }                
                cnt++;
            }
            return sb.ToString().TrimEnd();
        }

        public static string ToSpaceString<T>(this IEnumerable<T> data)
        {
            string str = string.Empty;
            foreach (T item in data) 
            {
                str += item.ToString() + " ";
            }
            return str;
        }
    }
}
