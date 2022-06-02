using System;
using System.Runtime.CompilerServices;

namespace Standart.Hash.xxHash
{
    public static class Utils
    {
        public static Guid ToGuid(this uint128 value)
        {
            // allocation
            return new Guid(value.ToBytes());
        }

        public static byte[] ToBytes(this uint128 value)
        {
            // allocation
            byte[] bytes = new byte[sizeof(ulong) * 2];
            Unsafe.As<byte, ulong>(ref bytes[0]) = value.high64;
            Unsafe.As<byte, ulong>(ref bytes[8]) = value.low64;
            return bytes;
        }
    }
}