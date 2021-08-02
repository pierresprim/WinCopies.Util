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

using System;

namespace WinCopies
#if !WinCopies3
.Util
#endif
{
    public static class Math
    {
        public static ulong Pow(in byte b, in byte power)
        {
            if (power == 0) return 1UL;

            if (power == 1) return b;

            ulong result = (ulong)(b * b);

            for (byte i = 3; i <= power; i++)

                result *= b;

            return result;
        }

        public static long Pow(in sbyte b, in sbyte power)
        {
            if (power == 0) return 1L;

            if (power == 1) return b;

            if (power == -1)

                return (long)(1 / b);

            long result;

            if (power < 0)
            {
                if (power < -1)
                {
                    result = (long)(b * b);

                    for (sbyte i = -3; i >= power; i--)

                        result *= b;

                    result = 1 / result;

                    return result;
                }
            }

            result = b * b;

            for (sbyte i = 3; i <= power; i++)

                result *= b;

            return result;
        }

        public static ulong Pow(in ushort s, in ushort power)
        {
            if (power == 0) return 1UL;

            if (power == 1) return s;

            ulong result = (ulong)(s * s);

            for (ushort i = 3; i <= power; i++)

                result *= s;

            return result;
        }

        public static long Pow(in short s, in short power)
        {
            if (power == 0) return 1L;

            if (power == 1) return s;

            if (power == -1)

                return (long)(1 / s);

            long result;

            if (power < 0)
            {
                if (power < -1)
                {
                    result = (long)(s * s);

                    for (short i = -3; i >= power; i--)

                        result *= s;

                    result = 1 / result;

                    return result;
                }
            }

            result = s * s;

            for (short i = 3; i <= power; i++)

                result *= s;

            return result;
        }

        public static ulong Pow(in uint i, in uint power)
        {
            if (power == 0u) return 1UL;

            if (power == 1u) return i;

            ulong result = (ulong)(i * i);

            for (uint _i = 3u; _i <= power; _i++)

                result *= i;

            return result;
        }

        public static long Pow(in int i, in int power)
        {
            if (power == 0) return 1L;

            if (power == 1) return i;

            if (power == -1)

                return (long)(1 / i);

            long result;

            if (power < 0)
            {
                if (power < -1)
                {
                    result = (long)(i * i);

                    for (int _i = -3; _i >= power; _i--)

                        result *= i;

                    result = 1 / result;

                    return result;
                }
            }

            result = i * i;

            for (int _i = 3; _i <= power; _i++)

                result *= i;

            return result;
        }

        public static ulong Pow(in ulong value, in ulong power)
        {
            if (power == 0) return 1UL;

            if (power == 1) return value;

            ulong result = value * value;

            for (ulong i = 3; i <= power; i++)

                result *= value;

            return result;
        }

        public static long Pow(in long value, in long power)
        {
            if (power == 0) return 1L;

            if (power == 1) return value;

            if (power == -1)

                return 1 / value;

            long result;

            if (power < 0)
            {
                if (power < -1)
                {
                    result = value * value;

                    for (long i = -3; i >= power; i--)

                        result *= value;

                    result = 1 / result;

                    return result;
                }
            }

            result = value * value;

            for (long i = 3; i <= power; i++)

                result *= value;

            return result;
        }

        public static float Pow(in float value, in float power)
        {
            if (power == 0) return 1f;

            if (power == 1) return value;

            if (power == -1)

                return 1 / value;

            float result;

            if (power < 0)
            {
                if (power < -1)
                {
                    result = (float)(value * value);

                    for (float i = -3; i >= power; i--)

                        result *= value;

                    result = 1 / result;

                    return result;
                }
            }

            result = (float)(value * value);

            for (float i = 3; i <= power; i++)

                result *= value;

            return result;
        }

        public static decimal Pow(in decimal value, in decimal power)
        {
            if (power == 0) return 1m;

            if (power == 1) return value;

            if (power == -1)

                return 1 / value;

            decimal result;

            if (power < 0)
            {
                if (power < -1)
                {
                    result = (decimal)(value * value);

                    for (decimal i = -3; i >= power; i--)

                        result *= value;

                    result = 1 / result;

                    return result;
                }
            }

            result = (decimal)(value * value);

            for (decimal i = 3; i <= power; i++)

                result *= value;

            return result;
        }

        public static bool IsAdditionResultInRange(in ulong left, in ulong right, in ulong maxValue) => left <= maxValue && maxValue - left >= right;

        public static ulong? TryAdd(in ulong left, in ulong right, in ulong maxValue)
        {
            if (IsAdditionResultInRange(left, right, maxValue))

                return left + right;

            return null;
        }

        public static bool IsMultiplicationResultInRange(in ulong left, in ulong right, in ulong maxValue) => left == 0 || maxValue / left >= right;

        public static ulong? TryMultiply(in ulong left, in ulong right, in ulong maxValue)
        {
            if (IsMultiplicationResultInRange(left, right, maxValue))

                return left * right;

            return null;
        }

        public static bool IsAdditionResultInRange(in uint left, in uint right, in uint maxValue) => left <= maxValue && maxValue - left >= right;

        public static uint? TryAdd(in uint left, in uint right, in uint maxValue)
        {
            if (IsAdditionResultInRange(left, right, maxValue))

                return left + right;

            return null;
        }

        public static bool IsMultiplicationResultInRange(in uint left, in uint right, in uint maxValue) => left == 0 || maxValue / left >= right;

        public static uint? TryMultiply(in uint left, in uint right, in uint maxValue)
        {
            if (IsMultiplicationResultInRange(left, right, maxValue))

                return left * right;

            return null;
        }

        public static bool IsAdditionResultInRange(in ushort left, in ushort right, in ushort maxValue) => left <= maxValue && maxValue - left >= right;

        public static ushort? TryAdd(in ushort left, in ushort right, in ushort maxValue)
        {
            if (IsAdditionResultInRange(left, right, maxValue))

                return (ushort)(left + right);

            return null;
        }

        public static bool IsMultiplicationResultInRange(in ushort left, in ushort right, in ushort maxValue) => left == 0 || maxValue / left >= right;

        public static ushort? TryMultiply(in ushort left, in ushort right, in ushort maxValue)
        {
            if (IsMultiplicationResultInRange(left, right, maxValue))

                return (ushort)(left * right);

            return null;
        }

        public static bool IsAdditionResultInRange(in byte left, in byte right, in byte maxValue) => left <= maxValue && maxValue - left >= right;

        public static byte? TryAdd(in byte left, in byte right, in byte maxValue)
        {
            if (IsAdditionResultInRange(left, right, maxValue))

                return (byte)(left + right);

            return null;
        }

        public static bool IsMultiplicationResultInRange(in byte left, in byte right, in byte maxValue) => left == 0 || maxValue / left >= right;

        public static byte? TryMultiply(in byte left, in byte right, in byte maxValue)
        {
            if (IsMultiplicationResultInRange(left, right, maxValue))

                return (byte)(left * right);

            return null;
        }
    }

    public static class MathExtensions
    {
        public static bool Between(this byte i, byte x, byte y, in bool bx, bool by)
        {
            bool between(in Func<bool> func) => func() && by ? i <= y : i < y;

            return between(bx ?
#if !CS9
                (Func<bool>)(
#endif
                () => x <= i
#if !CS9
                )
#endif
                : () => x < i);
        }

        public static bool Between(this sbyte i, sbyte x, sbyte y, in bool bx, bool by)
        {
            bool between(in Func<bool> func) => func() && by ? i <= y : i < y;

            return between(bx ?
#if !CS9
                (Func<bool>)(
#endif
                () => x <= i
#if !CS9
                )
#endif
                : () => x < i);
        }

        public static bool Between(this short i, short x, short y, in bool bx, bool by)
        {
            bool between(in Func<bool> func) => func() && by ? i <= y : i < y;

            return between(bx ?
#if !CS9
                (Func<bool>)(
#endif
                () => x <= i
#if !CS9
                )
#endif
                : () => x < i);
        }

        public static bool Between(this ushort i, ushort x, ushort y, in bool bx, bool by)
        {
            bool between(in Func<bool> func) => func() && by ? i <= y : i < y;

            return between(bx ?
#if !CS9
                (Func<bool>)(
#endif
                () => x <= i
#if !CS9
                )
#endif
                : () => x < i);
        }

        public static bool Between(this int i, int x, int y, in bool bx, bool by)
        {
            bool between(in Func<bool> func) => func() && by ? i <= y : i < y;

            return between(bx ?
#if !CS9
                (Func<bool>)(
#endif
                () => x <= i
#if !CS9
                )
#endif
                : () => x < i);
        }

        public static bool Between(this uint i, uint x, uint y, in bool bx, bool by)
        {
            bool between(in Func<bool> func) => func() && by ? i <= y : i < y;

            return between(bx ?
#if !CS9
                (Func<bool>)(
#endif
                () => x <= i
#if !CS9
                )
#endif
                : () => x < i);
        }

        public static bool Between(this long i, long x, long y, in bool bx, bool by)
        {
            bool between(in Func<bool> func) => func() && by ? i <= y : i < y;

            return between(bx ?
#if !CS9
                (Func<bool>)(
#endif
                () => x <= i
#if !CS9
                )
#endif
                : () => x < i);
        }

        public static bool Between(this ulong i, ulong x, ulong y, in bool bx, bool by)
        {
            bool between(in Func<bool> func) => func() && by ? i <= y : i < y;

            return between(bx ?
#if !CS9
                (Func<bool>)(
#endif
                () => x <= i
#if !CS9
                )
#endif
                : () => x < i);
        }

        public static bool Between(this float i, float x, float y, in bool bx, bool by)
        {
            bool between(in Func<bool> func) => func() && by ? i <= y : i < y;

            return between(bx ?
#if !CS9
                (Func<bool>)(
#endif
                () => x <= i
#if !CS9
                )
#endif
                : () => x < i);
        }

        public static bool Between(this double i, double x, double y, in bool bx, bool by)
        {
            bool between(in Func<bool> func) => func() && by ? i <= y : i < y;

            return between(bx ?
#if !CS9
                (Func<bool>)(
#endif
                () => x <= i
#if !CS9
                )
#endif
                : () => x < i);
        }

        public static bool Between(this decimal i, decimal x, decimal y, in bool bx, bool by)
        {
            bool between(in Func<bool> func) => func() && by ? i <= y : i < y;

            return between(bx ?
#if !CS9
                (Func<bool>)(
#endif
                () => x <= i
#if !CS9
                )
#endif
                : () => x < i);
        }
    }
}
