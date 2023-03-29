/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

#if CS11
using System.Numerics;
#endif

namespace WinCopies
{
    public static class Math
    {
        public static decimal GetBufferCount(in decimal length, in decimal bufferLength) => System.Math.Ceiling(length / bufferLength);

        public static uint GetLength(ulong n) => (uint)System.Math.Floor(System.Math.Log10(n) + 1);

        public static ulong Pow(in byte b, in byte power)
        {
            switch (power)
            {
                case 0:
                    return 1UL;

                case 1:
                    return b;
            }

            ulong result = (ulong)(b * b);

            for (byte i = 3; i <= power; i++)

                result *= b;

            return result;
        }

        public static long Pow(in sbyte b, in sbyte power)
        {
            switch (power)
            {
                case 0:
                    return 1L;

                case 1:
                    return b;

                case -1:
                    return 1 / b;
            }

            long result = b * b;

            if (power < -1)
            {
                for (sbyte i = -3; i >= power; i--)

                    result *= b;

                result = 1 / result;

                return result;
            }

            for (sbyte i = 3; i <= power; i++)

                result *= b;

            return result;
        }

        public static ulong Pow(in ushort s, in ushort power)
        {
            switch (power)
            {
                case 0:
                    return 1UL;

                case 1:
                    return s;
            }

            ulong result = (ulong)(s * s);

            for (ushort i = 3; i <= power; i++)

                result *= s;

            return result;
        }

        public static long Pow(in short s, in short power)
        {
            switch (power)
            {
                case 0:
                    return 1L;

                case 1:
                    return s;

                case -1:
                    return 1 / s;
            }

            long result = s * s;

            if (power < -1)
            {
                for (short i = -3; i >= power; i--)

                    result *= s;

                result = 1 / result;

                return result;
            }

            for (short i = 3; i <= power; i++)

                result *= s;

            return result;
        }

        public static ulong Pow(in uint i, in uint power)
        {
            switch (power)
            {
                case 0:
                    return 1UL;

                case 1:
                    return i;
            }

            ulong result = i * i;

            for (uint _i = 3u; _i <= power; _i++)

                result *= i;

            return result;
        }

        public static long Pow(in int i, in int power)
        {
            switch (power)
            {
                case 0:
                    return 1L;

                case 1:
                    return i;

                case -1:
                    return 1 / i;
            }

            long result = i * i;

            if (power < -1)
            {
                for (int _i = -3; _i >= power; _i--)

                    result *= i;

                result = 1 / result;

                return result;
            }

            for (int _i = 3; _i <= power; _i++)

                result *= i;

            return result;
        }

        public static ulong Pow(in ulong value, in ulong power)
        {
            switch (power)
            {
                case 0:
                    return 1UL;

                case 1:
                    return value;
            }

            ulong result = value * value;

            for (ulong i = 3; i <= power; i++)

                result *= value;

            return result;
        }

        public static long Pow(in long value, in long power)
        {
            switch (power)
            {
                case 0:
                    return 1L;

                case 1:
                    return value;

                case -1:
                    return 1 / value;
            }

            long result = value * value;

            if (power < -1)
            {
                for (long i = -3; i >= power; i--)

                    result *= value;

                result = 1 / result;

                return result;
            }

            for (long i = 3; i <= power; i++)

                result *= value;

            return result;
        }

        public static float Pow(in float value, in float power)
        {
            switch (power)
            {
                case 0:
                    return 1f;

                case 1:
                    return value;

                case -1:
                    return 1 / value;
            }

            float result = (float)(value * value);

            if (power < -1)
            {
                for (float i = -3; i >= power; i--)

                    result *= value;

                result = 1 / result;

                return result;
            }

            for (float i = 3; i <= power; i++)

                result *= value;

            return result;
        }

        public static decimal Pow(in decimal value, in decimal power)
        {
            switch (power)
            {
                case 0:
                    return 1m;

                case 1:
                    return value;

                case -1:
                    return 1 / value;
            }

            decimal result = value * value;

            if (power < -1)
            {
                for (decimal i = -3; i >= power; i--)

                    result *= value;

                result = 1 / result;

                return result;
            }

            for (decimal i = 3; i <= power; i++)

                result *= value;

            return result;
        }

        public static bool IsAdditionResultInRange(in ulong left, in ulong right, in ulong maxValue) => left <= maxValue && maxValue - left >= right;

        public static ulong? TryAdd(in ulong left, in ulong right, in ulong maxValue) => IsAdditionResultInRange(left, right, maxValue) ? left + right :
#if !CS9
            (ulong?)
#endif
            null;

        public static bool IsMultiplicationResultInRange(in ulong left, in ulong right, in ulong maxValue) => left == 0 || maxValue / left >= right;

        public static ulong? TryMultiply(in ulong left, in ulong right, in ulong maxValue) => IsMultiplicationResultInRange(left, right, maxValue) ? left * right :
#if !CS9
            (ulong?)
#endif
            null;

        public static bool IsAdditionResultInRange(in uint left, in uint right, in uint maxValue) => left <= maxValue && maxValue - left >= right;

        public static uint? TryAdd(in uint left, in uint right, in uint maxValue) => IsAdditionResultInRange(left, right, maxValue) ? left + right :
#if !CS9
            (uint?)
#endif
            null;

        public static bool IsMultiplicationResultInRange(in uint left, in uint right, in uint maxValue) => left == 0 || maxValue / left >= right;

        public static uint? TryMultiply(in uint left, in uint right, in uint maxValue) => IsMultiplicationResultInRange(left, right, maxValue) ? left * right :
#if !CS9
            (uint?)
#endif
            null;

        public static bool IsAdditionResultInRange(in ushort left, in ushort right, in ushort maxValue) => left <= maxValue && maxValue - left >= right;

        public static ushort? TryAdd(in ushort left, in ushort right, in ushort maxValue) => IsAdditionResultInRange(left, right, maxValue) ? (ushort)(left + right) :
#if !CS9
            (ushort?)
#endif
            null;

        public static bool IsMultiplicationResultInRange(in ushort left, in ushort right, in ushort maxValue) => left == 0 || maxValue / left >= right;

        public static ushort? TryMultiply(in ushort left, in ushort right, in ushort maxValue) => IsMultiplicationResultInRange(left, right, maxValue) ? (ushort)(left * right) :
#if !CS9
            (ushort?)
#endif
            null;

        public static bool IsAdditionResultInRange(in byte left, in byte right, in byte maxValue) => left <= maxValue && maxValue - left >= right;

        public static byte? TryAdd(in byte left, in byte right, in byte maxValue) => IsAdditionResultInRange(left, right, maxValue) ? (byte)(left + right) :
#if !CS9
            (byte?)
#endif
            null;

        public static bool IsMultiplicationResultInRange(in byte left, in byte right, in byte maxValue) => left == 0 || maxValue / left >= right;

        public static byte? TryMultiply(in byte left, in byte right, in byte maxValue) => IsMultiplicationResultInRange(left, right, maxValue) ? (byte)(left * right) :
#if !CS9
            (byte?)
#endif
            null;
    }

    public static class MathExtensions
    {
        private static bool Between<T>(in
#if CS11
            T
#else
            dynamic
#endif
            value, in T x, in T y, in bool bx, in bool by) where T :
#if CS11
            IComparisonOperators<T, T, bool>
#else
            unmanaged
#endif
            => (bx ? (value >= x) : (value > x)) && (by ? (value <= y) : (value < y));

        private static bool Outside<T>(in
#if CS11
            T
#else
            dynamic
#endif
            value, in T x, in T y, in bool bx, in bool by) where T :
#if CS11
            IComparisonOperators<T, T, bool>
#else
            unmanaged
#endif
            => (bx ? (value <= x) : (value < x)) && (by ? (value >= y) : (value > y));

        public static bool Between(this byte value, byte x, byte y, in bool bx = true, bool by = true) => Between<byte>(value, x, y, bx, by);
        public static bool Between(this sbyte value, sbyte x, sbyte y, in bool bx = true, bool by = true) => Between<sbyte>(value, x, y, bx, by);
        public static bool Between(this short value, short x, short y, in bool bx = true, bool by = true) => Between<short>(value, x, y, bx, by);
        public static bool Between(this ushort value, ushort x, ushort y, in bool bx = true, bool by = true) => Between<ushort>(value, x, y, bx, by);
        public static bool Between(this int value, int x, int y, in bool bx = true, bool by = true) => Between<int>(value, x, y, bx, by);
        public static bool Between(this uint value, uint x, uint y, in bool bx = true, bool by = true) => Between<uint>(value, x, y, bx, by);
        public static bool Between(this long value, long x, long y, in bool bx = true, bool by = true) => Between<long>(value, x, y, bx, by);
        public static bool Between(this ulong value, ulong x, ulong y, in bool bx = true, bool by = true) => Between<ulong>(value, x, y, bx, by);
        public static bool Between(this float value, float x, float y, in bool bx = true, bool by = true) => Between<float>(value, x, y, bx, by);
        public static bool Between(this double value, double x, double y, in bool bx = true, bool by = true) => Between<double>(value, x, y, bx, by);
        public static bool Between(this decimal value, decimal x, decimal y, in bool bx = true, bool by = true) => Between<decimal>(value, x, y, bx, by);

        public static bool Outside(this byte value, byte x, byte y, in bool bx = true, bool by = true) => Outside<byte>(value, x, y, bx, by);
        public static bool Outside(this sbyte value, sbyte x, sbyte y, in bool bx = true, bool by = true) => Outside<sbyte>(value, x, y, bx, by);
        public static bool Outside(this short value, short x, short y, in bool bx = true, bool by = true) => Outside<short>(value, x, y, bx, by);
        public static bool Outside(this ushort value, ushort x, ushort y, in bool bx = true, bool by = true) => Outside<ushort>(value, x, y, bx, by);
        public static bool Outside(this int value, int x, int y, in bool bx = true, bool by = true) => Outside<int>(value, x, y, bx, by);
        public static bool Outside(this uint value, uint x, uint y, in bool bx = true, bool by = true) => Outside<uint>(value, x, y, bx, by);
        public static bool Outside(this long value, long x, long y, in bool bx = true, bool by = true) => Outside<long>(value, x, y, bx, by);
        public static bool Outside(this ulong value, ulong x, ulong y, in bool bx = true, bool by = true) => Outside<ulong>(value, x, y, bx, by);
        public static bool Outside(this float value, float x, float y, in bool bx = true, bool by = true) => Outside<float>(value, x, y, bx, by);
        public static bool Outside(this double value, double x, double y, in bool bx = true, bool by = true) => Outside<double>(value, x, y, bx, by);
        public static bool Outside(this decimal value, decimal x, decimal y, in bool bx = true, bool by = true) => Outside<decimal>(value, x, y, bx, by);
    }
}
