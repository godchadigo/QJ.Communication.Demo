using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QJ.Communication.Core.Helper
{
    public class ByteHelper
    {
        public static bool[] BytesToBools(byte byte1)
        {
            bool[] result = new bool[8];
            // 低位元8bit
            for (int i = 0; i < 8; i++)
            {
                result[i + 0] = (byte1 & (1 << i)) != 0;
            }
            
            return result;
        }
        public static bool[] BytesToBools(byte byte1, byte byte2)
        {
            bool [] result = new bool[16];
            // 低位元8bit
            for (int i = 0; i < 8; i++)
            {
                result[i + 0] = (byte1 & (1 << i)) != 0;
            }
            
            // 高位元8bit
            for (int i = 0; i < 8; i++)
            {
                result[i + 8] = (byte2 & (1 << i)) != 0;
            }
            
            return result;
        }


        public static ByteHelper Instance = new ByteHelper();
        #region Bytes To DataType
        public static short BytesToShort(byte byte1, byte byte2)
        {
            short value = (short)((byte2 << 8) | byte1);
            return value;
        }
        public static ushort BytesToUShort(byte byte1, byte byte2)
        {
            ushort value = (ushort)((byte2 << 8) | byte1);
            return value;
        }
        public static int BytesToInt(byte byte1, byte byte2, byte byte3, byte byte4)
        {
            int value = (byte4 << 24) | (byte3 << 16) | (byte2 << 8) | byte1;
            return value;
        }

        public static uint BytesToUInt(byte byte1, byte byte2, byte byte3, byte byte4)
        {
            uint value = (uint)((byte4 << 24) | (byte3 << 16) | (byte2 << 8) | byte1);
            return value;
        }
        public static long BytesToLong(byte byte1, byte byte2, byte byte3, byte byte4, byte byte5, byte byte6, byte byte7, byte byte8)
        {
            long value = ((long)byte8 << 56) | ((long)byte7 << 48) | ((long)byte6 << 40) | ((long)byte5 << 32) | ((long)byte4 << 24) | ((long)byte3 << 16) | ((long)byte2 << 8) | byte1;
            return value;
        }
        public static ulong BytesToULong(byte byte1, byte byte2, byte byte3, byte byte4, byte byte5, byte byte6, byte byte7, byte byte8)
        {
            ulong value = ((ulong)byte8 << 56) | ((ulong)byte7 << 48) | ((ulong)byte6 << 40) | ((ulong)byte5 << 32) | ((ulong)byte4 << 24) | ((ulong)byte3 << 16) | ((ulong)byte2 << 8) | byte1;
            return value;
        }
        public static float BytesToFloat(byte byte4, byte byte3, byte byte2, byte byte1)
        {
            byte[] byteArray = { byte4, byte3, byte2, byte1 };
            float value = BitConverter.ToSingle(byteArray, 0);
            return value;
        }
        public static double BytesToDouble(byte byte8, byte byte7, byte byte6, byte byte5, byte byte4, byte byte3, byte byte2, byte byte1)
        {
            byte[] byteArray = { byte8, byte7, byte6, byte5, byte4, byte3, byte2, byte1 };
            double value = BitConverter.ToDouble(byteArray, 0);
            return value;
        }
        public static string BytesToUTF8String(byte[] bytes)
        {
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }
        public static string BytesToAsciiString(byte[] bytes , StringEncoding encode)
        {
            string result = string.Empty;
            switch (encode)
            {
                case StringEncoding.Ascii:
                    result = Encoding.ASCII.GetString(bytes);
                    break;
                case StringEncoding.UTF8:
                    result = Encoding.ASCII.GetString(bytes);
                    break;

            }            
            return result;
        }
        #endregion

        #region DataType To Bytes
        public static byte[] BoolArrayToByteArray(bool[] boolArray)
        {
            int length = boolArray.Length;
            
            int byteCount = (int)Math.Ceiling((double)length / 8);
            if ((byteCount % 2) == 1) byteCount = byteCount + 1;
            byte[] byteArray = new byte[byteCount];
            byteArray[byteCount - 1] = 0;
            for (int i = 0; i < byteCount; i++)
            {
                int startIndex = i * 8; ;
                int endIndex = Math.Min(startIndex + 8, length);
                bool[] subArray = boolArray.Skip(startIndex).Take(endIndex - startIndex).ToArray();
                byteArray[i] = BoolArrayToByte(subArray);
            }
            
            return byteArray;
        }

        private static byte BoolArrayToByte(bool[] boolArray)
        {

            Array.Resize(ref boolArray, 8);
            byte result = 0;

            for (int i = 0; i < 8; i++)
            {
                if (boolArray[i])
                {
                    result |= (byte)(1 << i);
                }
            }

            return result;
        }
        public static byte[] UShortToBytes(ushort value)
        {
            byte byte1 = (byte)(value & 0xFF);
            byte byte2 = (byte)((value >> 8) & 0xFF);

            return new byte[] { byte1, byte2 };
        }
        public static byte[] UShortToBytes(ushort[] values)
        {
            var data = new byte[values.Length * 2];
            for(int i=0;i<values.Length; i++)
            {
                byte byte1 = (byte)(values[i] & 0xFF);                
                byte byte2 = (byte)((values[i] >> 8) & 0xFF);
                data[(i * 2)] = byte2;
                data[(i * 2)+1] = byte1;
            }
            return data;
        }

        public static byte[] ShortToBytes(short value)
        {
            byte byte1 = (byte)(value & 0xFF);
            byte byte2 = (byte)((value >> 8) & 0xFF);

            return new byte[] { byte1, byte2 };
        }
        public static byte[] ShortToBytes(short[] values)
        {
            var data = new byte[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                byte byte1 = (byte)(values[i] & 0xFF);
                byte byte2 = (byte)((values[i] >> 8) & 0xFF);
                data[(i * 2)] = byte2;
                data[(i * 2) + 1] = byte1;
            }
            return data;
        }

        public static byte[] UIntToBytes(uint value)
        {
            byte byte1 = (byte)(value & 0xFF);
            byte byte2 = (byte)((value >> 8) & 0xFF);
            byte byte3 = (byte)((value >> 16) & 0xFF);
            byte byte4 = (byte)((value >> 24) & 0xFF);

            return new byte[] { byte4, byte3, byte2, byte1 };
        }
        public static byte[] UIntToBytes(uint[] value)
        {
            var data = new byte[value.Length * 4];
            for (int i = 0; i < value.Length; i++)
            {
                byte byte1 = (byte)(value[i] & 0xFF);
                byte byte2 = (byte)((value[i] >> 8) & 0xFF);
                byte byte3 = (byte)((value[i] >> 16) & 0xFF);
                byte byte4 = (byte)((value[i] >> 24) & 0xFF);
                data[(i * 4) + 0] = byte4;
                data[(i * 4) + 1] = byte3;
                data[(i * 4) + 2] = byte2;
                data[(i * 4) + 3] = byte1;
            }
            return data;
        }
        public static byte[] IntToBytes(int value)
        {
            byte byte1 = (byte)(value & 0xFF);
            byte byte2 = (byte)((value >> 8) & 0xFF);
            byte byte3 = (byte)((value >> 16) & 0xFF);
            byte byte4 = (byte)((value >> 24) & 0xFF);

            return new byte[] { byte4, byte3, byte2, byte1 };
        }
        public static byte[] IntToBytes(int[] value)
        {
            var data = new byte[value.Length * 4];
            for (int i = 0; i < value.Length; i++)
            {
                byte byte1 = (byte)(value[i] & 0xFF);
                byte byte2 = (byte)((value[i] >> 8) & 0xFF);
                byte byte3 = (byte)((value[i] >> 16) & 0xFF);
                byte byte4 = (byte)((value[i] >> 24) & 0xFF);
                data[(i * 4) + 0] = byte4;
                data[(i * 4) + 1] = byte3;
                data[(i * 4) + 2] = byte2;
                data[(i * 4) + 3] = byte1;
            }
            return data;
        }

        public static byte[] ULongToBytes(ulong value)
        {
            byte[] data = new byte[8];
            data[7] = (byte)(value & 0xFF);
            data[6] = (byte)((value >> 8) & 0xFF);
            data[5] = (byte)((value >> 16) & 0xFF);
            data[4] = (byte)((value >> 24) & 0xFF);
            data[3] = (byte)((value >> 32) & 0xFF);
            data[2] = (byte)((value >> 40) & 0xFF);
            data[1] = (byte)((value >> 48) & 0xFF);
            data[0] = (byte)((value >> 56) & 0xFF);

            return data;
        }
        public static byte[] ULongToBytes(ulong[] value)
        {
            byte[] data = new byte[value.Length * 8];
            for (int i = 0; i < value.Length; i++)
            {
                data[(i * 8) + 7] = (byte)(value[i] & 0xFF);
                data[(i * 8) + 6] = (byte)((value[i] >> 8) & 0xFF);
                data[(i * 8) + 5] = (byte)((value[i] >> 16) & 0xFF);
                data[(i * 8) + 4] = (byte)((value[i] >> 24) & 0xFF);
                data[(i * 8) + 3] = (byte)((value[i] >> 32) & 0xFF);
                data[(i * 8) + 2] = (byte)((value[i] >> 40) & 0xFF);
                data[(i * 8) + 1] = (byte)((value[i] >> 48) & 0xFF);
                data[(i * 8) + 0] = (byte)((value[i] >> 56) & 0xFF);
            }            
            return data;
        }
        public static byte[] LongToBytes(long value)
        {
            byte[] data = new byte[8];
            data[7] = (byte)(value & 0xFF);
            data[6] = (byte)((value >> 8) & 0xFF);
            data[5] = (byte)((value >> 16) & 0xFF);
            data[4] = (byte)((value >> 24) & 0xFF);
            data[3] = (byte)((value >> 32) & 0xFF);
            data[2] = (byte)((value >> 40) & 0xFF);
            data[1] = (byte)((value >> 48) & 0xFF);
            data[0] = (byte)((value >> 56) & 0xFF);

            return data;
        }
        public static byte[] LongToBytes(long[] value)
        {
            byte[] data = new byte[value.Length * 8];
            for (int i = 0; i < value.Length; i++)
            {
                data[(i * 8) + 7] = (byte)(value[i] & 0xFF);
                data[(i * 8) + 6] = (byte)((value[i] >> 8) & 0xFF);
                data[(i * 8) + 5] = (byte)((value[i] >> 16) & 0xFF);
                data[(i * 8) + 4] = (byte)((value[i] >> 24) & 0xFF);
                data[(i * 8) + 3] = (byte)((value[i] >> 32) & 0xFF);
                data[(i * 8) + 2] = (byte)((value[i] >> 40) & 0xFF);
                data[(i * 8) + 1] = (byte)((value[i] >> 48) & 0xFF);
                data[(i * 8) + 0] = (byte)((value[i] >> 56) & 0xFF);
            }
            return data;
        }
        public static byte[] FloatToBytes(float value)
        {
            byte[] byteArray = BitConverter.GetBytes(value);
            Array.Reverse(byteArray);
            return byteArray;
        }
        public static byte[] FloatToBytes(float[] value)
        {
            byte[] data = new byte[value.Length * 4];
            for (int i = 0; i < value.Length; i++)
            {
                byte[] byteArray = BitConverter.GetBytes(value[i]);
                
                data[(i * 4) + 0] = byteArray[3];
                data[(i * 4) + 1] = byteArray[2];
                data[(i * 4) + 2] = byteArray[1];
                data[(i * 4) + 3] = byteArray[0];
            }
                        
            return data;
        }
        public static byte[] DoubleToBytes(double value)
        {
            byte[] byteArray = BitConverter.GetBytes(value);
            Array.Reverse(byteArray);
            return byteArray;
        }


        public static byte[] DoubleToBytes(double[] value)
        {
            byte[] data = new byte[value.Length * 8];
            for (int i = 0; i < value.Length; i++)
            {
                byte[] byteArray = BitConverter.GetBytes(value[i]);

                data[(i * 8) + 0] = byteArray[7];
                data[(i * 8) + 1] = byteArray[6];
                data[(i * 8) + 2] = byteArray[5];
                data[(i * 8) + 3] = byteArray[4];
                data[(i * 8) + 4] = byteArray[3];
                data[(i * 8) + 5] = byteArray[2];
                data[(i * 8) + 6] = byteArray[1];
                data[(i * 8) + 7] = byteArray[0];
            }

            return data;
        }
        public static byte[] StringToBytes(string str, Encoding encoding)
        {            
            var data = encoding.GetBytes(str);
            Array.Reverse(data);
            return data;
        }


        #endregion
        
    }
    public enum StringEncoding
    {
        Ascii = 0,
        UTF8 = 1,
    }
}
