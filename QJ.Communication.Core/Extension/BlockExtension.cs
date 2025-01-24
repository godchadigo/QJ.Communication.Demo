﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;

namespace QJ.Communication.Core.Extension
{
    public static class BlockExtension
    {
        public static short ToInt16(this ValueByteBlock block)
        {            
            return block.ReadInt16();
        }
        public static int ToInt32(this ValueByteBlock block)
        {
            return block.ReadInt32();
        }
        public static long ToInt64(this ValueByteBlock block)
        {
            return block.ReadInt64();
        }
        public static ushort ToUInt16(this ValueByteBlock block)
        {
            return block.ReadUInt16();
        }
        public static uint ToUInt32(this ValueByteBlock block)
        {
            return block.ReadUInt32();
        }
        public static ulong ToUInt64(this ValueByteBlock block)
        {
            return block.ReadUInt64();
        }
        
        public static List<short> ToInt16Array(this ValueByteBlock block)
        {
            List<short> result = new List<short>();
            for (var i = 0; i < block.Length; i++)
            {
                result.Add(block.ToInt16());
            }
            return result;
        }
        public static List<ushort> ToUInt16Array(this ValueByteBlock block)
        {
            List<ushort> result = new List<ushort>();
            for (var i = 0; i < block.Length; i++)
            {
                result.Add(block.ToUInt16());
            }
            return result;
        }


        public static List<int> ToInt32Array(this ValueByteBlock block)
        {
            List<int> result = new List<int>();
            for (var i = 0; i < block.Length; i++)
            {
                result.Add(block.ToInt32());
            }
            return result;
        }
        public static List<uint> ToUInt32Array(this ValueByteBlock block)
        {
            List<uint> result = new List<uint>();
            for (var i = 0; i < block.Length; i++)
            {
                result.Add(block.ToUInt32());
            }
            return result;
        }

        public static List<long> ToInt64Array(this ValueByteBlock block)
        {
            List<long> result = new List<long>();
            for (var i = 0; i < block.Length; i++)
            {
                result.Add(block.ToInt64());
            }
            return result;
        }
        public static List<ulong> ToUInt64Array(this ValueByteBlock block)
        {
            List<ulong> result = new List<ulong>();
            for (var i = 0; i < block.Length; i++)
            {
                result.Add(block.ToUInt64());
            }
            return result;
        }
    }
}
