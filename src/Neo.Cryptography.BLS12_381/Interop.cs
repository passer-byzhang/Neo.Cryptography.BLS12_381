﻿using System;
using System.Runtime.InteropServices;

namespace Neo.Cryptography.BLS12_381
{
    internal static class Interop
    {
        [DllImport("Neo_Cryptography_BLS12_381_Native", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gt_add(IntPtr gt1, IntPtr gt2);

        [DllImport("Neo_Cryptography_BLS12_381_Native", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gt_mul(IntPtr gt, UInt64 multi);

        [DllImport("Neo_Cryptography_BLS12_381_Native", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gt_neg(IntPtr gt);

        [DllImport("Neo_Cryptography_BLS12_381_Native", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr g1_add(IntPtr g1_1, IntPtr g1_2);

        [DllImport("Neo_Cryptography_BLS12_381_Native", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr g1_mul(IntPtr g1, UInt64 multi);

        [DllImport("Neo_Cryptography_BLS12_381_Native", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr g1_neg(IntPtr g1);

        [DllImport("Neo_Cryptography_BLS12_381_Native", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr g2_add(IntPtr g2_1, IntPtr g2_2);

        [DllImport("Neo_Cryptography_BLS12_381_Native", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr g2_mul(IntPtr g2, UInt64 multi);

        [DllImport("Neo_Cryptography_BLS12_381_Native", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr g2_neg(IntPtr g2);

        [DllImport("Neo_Cryptography_BLS12_381_Native", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr g1_g2_pairing(IntPtr g1, IntPtr g2);

        public static GObject Add(GObject p1, GObject p2)
        {
            if (p1.type != p2.type)
            {
                throw new Exception($"Bls12381 operation fault, type:format, error:type mismatch");
            }
            return p1.type switch
            {
                GType.G1 => new GObject(GType.G1, Interop.g1_add(p1.ptr, p2.ptr)),
                GType.G2 => new GObject(GType.G2, Interop.g2_add(p1.ptr, p2.ptr)),
                GType.Gt => new GObject(GType.Gt, Interop.gt_add(p1.ptr, p2.ptr)),
                _ => throw new Exception($"Bls12381 operation fault,type:format,error:valid point length")
            };
        }

        public static GObject Mul(GObject p, UInt64 x)
        {
            return p.type switch
            {
                GType.G1 => new GObject(GType.G1, Interop.g1_mul(p.ptr, x)),
                GType.G2 => new GObject(GType.G2, Interop.g2_mul(p.ptr, x)),
                GType.Gt => new GObject(GType.Gt, Interop.gt_mul(p.ptr, x)),
                _ => throw new Exception($"Bls12381 operation falut,type:format,error:valid point length")
            };
        }

        public static GObject Pairing(GObject p1, GObject p2)
        {
            if (p1.type != GType.G1 || p2.type != GType.G2)
            {
                throw new Exception($"Bls12381 operation fault, type:format, error:type mismatch");
            }
            GObject gt = new GObject(GType.Gt, Interop.g1_g2_pairing(p1.ptr, p2.ptr));
            return gt;
        }
    

        public static byte[] ToByteArray(this IntPtr data, int length)
        {
            if (data == IntPtr.Zero) return null;
            byte[] buffer = new byte[length];
            Marshal.Copy(data, buffer, 0, length);
            return buffer;
        }
    }
}
