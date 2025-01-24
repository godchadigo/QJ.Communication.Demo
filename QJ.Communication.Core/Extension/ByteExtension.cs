using Newtonsoft.Json;
using QJ.Communication.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace QJ.Communication.Core.Extension
{
    public static class ByteExtension
    {
        public static byte[] ToHexBytes(this string str)
        {
            // 產生byte[] 封包
            byte[] packet = new byte[str.Count() / 2];
            for (int j = 0; j < packet.Count(); j++)
            {
                packet[j] = Convert.ToByte(str[j + j].ToString() + str[j + j + 1], 16);
            }
            return packet;
        }

        public static string ToHexString(this byte[] data)
        {
            var str = string.Empty;
            foreach (byte b in data)
            {
                str += b.ToString("X2") + " ";
            }
            return str;
        }

        // 方法1：使用 BitConverter
        public static string ToHexString<T>(this T[] data)
        {
            if (data == null) return "封包為空!";
            var str = new StringBuilder();  // 使用 StringBuilder 效能更好

            foreach (T item in data)
            {
                byte[] bytes;
                if (item is byte b)  // 如果已經是 byte 就直接使用
                {
                    str.AppendFormat("{0:X2} ", b);
                }
                else  // 其他類型則轉換為 bytes
                {
                    bytes = BitConverter.GetBytes(Convert.ToInt64(item));
                    foreach (byte bb in bytes)
                    {
                        str.AppendFormat("{0:X2} ", bb);
                    }
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 使用 Newtonsoft.Json 序列化成byte[]
        /// </summary>
        public static byte[] ToByteArray<T>(this T data)
        {
            if (data == null)
                return null;

            // 序列化為 JSON 字串
            string jsonString = JsonConvert.SerializeObject(data);
            // 轉換為 byte 陣列
            return Encoding.UTF8.GetBytes(jsonString);
        }

        /// <summary>
        /// 使用 Newtonsoft.Json 序列化成byte[]，並支援不同端序
        /// </summary>
        /// <typeparam name="T">資料類型</typeparam>
        /// <param name="data">要序列化的資料</param>
        /// <param name="endian">端序類型，預設為 ABCD</param>
        /// <returns>序列化後的 byte 陣列</returns>
        public static byte[] ToByteArray<T>(this T[] data, EndianEnum.EndianType endian = EndianEnum.EndianType.ABCD)
        {
            if (data == null)
                return null;

            // 序列化為 JSON 字串
            string jsonString = JsonConvert.SerializeObject(data);
            // 轉換為 byte 陣列
            byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

            // 根據指定的端序調整 byte 順序
            return ConvertEndian(bytes, endian);
        }

        /// <summary>
        /// 調整 byte 陣列的端序
        /// </summary>
        private static byte[] ConvertEndian(byte[] source, EndianEnum.EndianType endian)
        {
            if (source == null || source.Length < 2)
                return source;

            byte[] result = new byte[source.Length];
            source.CopyTo(result, 0);

            // 每4個bytes為一組進行轉換
            for (int i = 0; i < result.Length - 3; i += 4)
            {
                switch (endian)
                {
                    case EndianEnum.EndianType.ABCD: // 不需轉換
                        break;

                    case EndianEnum.EndianType.CDAB: // 3412
                        SwapBytes(result, i + 0, i + 2);
                        SwapBytes(result, i + 1, i + 3);
                        break;

                    case EndianEnum.EndianType.BADC: // 2143
                        SwapBytes(result, i + 0, i + 1);
                        SwapBytes(result, i + 2, i + 3);
                        break;

                    case EndianEnum.EndianType.DCBA: // 4321
                        SwapBytes(result, i + 0, i + 3);
                        SwapBytes(result, i + 1, i + 2);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// 交換兩個位置的 byte
        /// </summary>
        private static void SwapBytes(byte[] array, int pos1, int pos2)
        {
            byte temp = array[pos1];
            array[pos1] = array[pos2];
            array[pos2] = temp;
        }

        /// <summary>
        /// 將 byte[] 反序列化回陣列，支援不同端序
        /// </summary>
        public static T[] FromByteArray<T>(this byte[] bytes, EndianEnum.EndianType endian = EndianEnum.EndianType.ABCD)
        {
            if (bytes == null || bytes.Length == 0)
                return default;

            // 先將 byte 陣列轉換回原始端序
            byte[] originalBytes = ConvertEndian(bytes, GetReverseEndian(endian));

            // 轉換回 JSON 字串
            string jsonString = Encoding.UTF8.GetString(originalBytes);
            // 反序列化為陣列
            return JsonConvert.DeserializeObject<T[]>(jsonString);
        }

        /// <summary>
        /// 獲取相反的端序（用於反序列化）
        /// </summary>
        private static EndianEnum.EndianType GetReverseEndian(EndianEnum.EndianType endian)
        {
            switch (endian)
            {
                case EndianEnum.EndianType.CDAB:
                    return EndianEnum.EndianType.CDAB;  // CDAB 是自反的
                case EndianEnum.EndianType.BADC:
                    return EndianEnum.EndianType.BADC;  // BADC 是自反的
                case EndianEnum.EndianType.DCBA:
                    return EndianEnum.EndianType.DCBA;  // DCBA 是自反的
                default:
                    return EndianEnum.EndianType.ABCD;
            }
        }
    }
}
