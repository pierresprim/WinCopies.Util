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
#if CS11
using System.Numerics;
#endif

using static WinCopies.UtilHelpers;

namespace WinCopies
{
    public static class Math
    {
        public static uint GetLength(ulong n) => (uint)System.Math.Floor(System.Math.Log10(n) + 1);

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
                    result = value * value;

                    for (decimal i = -3; i >= power; i--)

                        result *= value;

                    result = 1 / result;

                    return result;
                }
            }

            result = value * value;

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
        private static bool Between<T>(this
#if !CS11
            ISortableItem<
#endif
            T
#if !CS11
            >
#endif
            value, in T x, in T y, in bool bx, in bool by) where T :
#if CS11
            IComparisonOperators<T, T, bool>
#else
            unmanaged
#endif
            => Compare(x, y, bx ?
#if CS11
            _value => value >= _value : _value => value > _value
#else
            value.GreaterThanOrEqualTo :
#if !CS9
                (Predicate<T>)
#endif
                value.GreaterThan
#endif
            , by ?
#if CS11
            _value => value <= _value : _value => value < _value
#else
            value.LessThanOrEqualTo :
#if !CS9
                (Predicate<T>)
#endif
                value.LessThan
#endif
                , Bool.AndIn);

        private static bool Outside<T>(this
#if !CS11
            ISortableItem<
#endif
            T
#if !CS11
            >
#endif
            value, in T x, in T y, in bool bx, in bool by) where T :
#if CS11
            IComparisonOperators<T, T, bool>
#else
            unmanaged
#endif
            => Compare(x, y, bx ?
#if CS11
            _value => value <= _value : _value => value < _value
#else
            value.LessThanOrEqualTo :
#if !CS9
                (Predicate<T>)
#endif
                value.LessThan
#endif
            , by ?
#if CS11
            _value => value >= _value : _value => value > _value
#else
            value.GreaterThanOrEqualTo :
#if !CS9
                (Predicate<T>)
#endif
                value.GreaterThan
#endif
                , Bool.OrIn);

        public static bool Between(this byte value, byte x, byte y, in bool bx = true, bool by = true) => Between
#if CS11
            <byte>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Between(this sbyte value, sbyte x, sbyte y, in bool bx = true, bool by = true) => Between
#if CS11
            <sbyte>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Between(this short value, short x, short y, in bool bx = true, bool by = true) => Between
#if CS11
            <short>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Between(this ushort value, ushort x, ushort y, in bool bx = true, bool by = true) => Between
#if CS11
            <ushort>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Between(this int value, int x, int y, in bool bx = true, bool by = true) => Between
#if CS11
            <int>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Between(this uint value, uint x, uint y, in bool bx = true, bool by = true) => Between
#if CS11
            <uint>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Between(this long value, long x, long y, in bool bx = true, bool by = true) => Between
#if CS11
            <long>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Between(this ulong value, ulong x, ulong y, in bool bx = true, bool by = true) => Between
#if CS11
            <ulong>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Between(this float value, float x, float y, in bool bx = true, bool by = true) => Between
#if CS11
            <float>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Between(this double value, double x, double y, in bool bx = true, bool by = true) => Between
#if CS11
            <double>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Between(this decimal value, decimal x, decimal y, in bool bx = true, bool by = true) => Between
#if CS11
            <decimal>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);

        public static bool Outside(this byte value, byte x, byte y, in bool bx = true, bool by = true) => Outside
#if CS11
            <byte>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Outside(this sbyte value, sbyte x, sbyte y, in bool bx = true, bool by = true) => Outside
#if CS11
            <sbyte>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Outside(this short value, short x, short y, in bool bx = true, bool by = true) => Outside
#if CS11
            <short>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Outside(this ushort value, ushort x, ushort y, in bool bx = true, bool by = true) => Outside
#if CS11
            <ushort>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Outside(this int value, int x, int y, in bool bx = true, bool by = true) => Outside
#if CS11
            <int>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Outside(this uint value, uint x, uint y, in bool bx = true, bool by = true) => Outside
#if CS11
            <uint>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Outside(this long value, long x, long y, in bool bx = true, bool by = true) => Outside
#if CS11
            <long>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Outside(this ulong value, ulong x, ulong y, in bool bx = true, bool by = true) => Outside
#if CS11
            <ulong>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Outside(this float value, float x, float y, in bool bx = true, bool by = true) => Outside
#if CS11
            <float>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Outside(this double value, double x, double y, in bool bx = true, bool by = true) => Outside
#if CS11
            <double>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
        public static bool Outside(this decimal value, decimal x, decimal y, in bool bx = true, bool by = true) => Outside
#if CS11
            <decimal>
#endif
            (
#if !CS11
            Util.Extensions.NumberHelper.GetNumber(
#endif
            value
#if !CS11
            )
#endif
            , x, y, bx, by);
    }
}
