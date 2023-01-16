/* Copyright © Pierre Sprimont, 2022
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
using System.Diagnostics.CodeAnalysis;
#if CS5
using System.Numerics;
#endif

namespace WinCopies.Util
{
    public static partial class Extensions
    {
        public readonly struct Enum<T> : ISortableItem<T>, ISortableItem<Enum<T>>
#if CS11
            , IComparisonOperators<Enum<T>, T, bool>, IComparisonOperators<Enum<T>, Enum<T>, bool>
#endif
            where T : Enum
        {
            private readonly T _value;

            public Enum(in T value) => _value = value;

            public bool Equals(T
#if CS9
                ?
#endif
                other) => _value.Equals(other);
            public int CompareTo(T
#if CS9
                ?
#endif
                other) => _value.CompareTo(other);

            public bool Equals(Enum<T> other) => Equals(other._value);
            public int CompareTo(Enum<T> other) => CompareTo(other._value);

            public static bool operator ==(Enum<T> left, T
#if CS9
                ?
#endif
                right) => left.Equals(right);
            public static bool operator !=(Enum<T> left, T
#if CS9
                ?
#endif
                right) => !(left == right);
            public static bool operator <(Enum<T> left, T right) => left.CompareTo(right) < 0;
            public static bool operator >(Enum<T> left, T right) => left.CompareTo(right) > 0;
            public static bool operator <=(Enum<T> left, T right) => left.CompareTo(right) <= 0;
            public static bool operator >=(Enum<T> left, T right) => left.CompareTo(right) >= 0;

            public static bool operator ==(Enum<T> left, Enum<T> right) => left == right._value;
            public static bool operator !=(Enum<T> left, Enum<T> right) => !(left == right);
            public static bool operator <(Enum<T> left, Enum<T> right) => left < right._value;
            public static bool operator >(Enum<T> left, Enum<T> right) => left > right._value;
            public static bool operator <=(Enum<T> left, Enum<T> right) => left <= right._value;
            public static bool operator >=(Enum<T> left, Enum<T> right) => left >= right._value;

            public override bool Equals(
#if CS8
                [NotNullWhen(true)]
#endif
                object
#if CS8
                ?
#endif
                obj) => (obj is T item && Equals(item)) || (obj is Enum<T> value && Equals(value));

            public override int GetHashCode() => _value.GetHashCode();
#if !CS8
            public bool LessThan(T other) => UtilHelpers.LessThan(this, other);
            public bool LessThanOrEqualTo(T other) => UtilHelpers.LessThanOrEqualTo(this, other);
            public bool GreaterThan(T other) => UtilHelpers.GreaterThan(this, other);
            public bool GreaterThanOrEqualTo(T other) => UtilHelpers.GreaterThanOrEqualTo(this, other);

            public bool LessThan(Enum<T> other) => UtilHelpers.LessThan(this, other);
            public bool LessThanOrEqualTo(Enum<T> other) => UtilHelpers.LessThanOrEqualTo(this, other);
            public bool GreaterThan(Enum<T> other) => UtilHelpers.GreaterThan(this, other);
            public bool GreaterThanOrEqualTo(Enum<T> other) => UtilHelpers.GreaterThanOrEqualTo(this, other);
#endif
        }
#if !CS11
        private readonly struct Number<T> : ISortableItem<T>
        {
            private readonly Predicate<T> _predicate;
            private readonly Converter<T, int> _converter;

            public Number(in Predicate<T> predicate, in Converter<T, int> converter)
            {
                _predicate = predicate;
                _converter = converter;
            }

            public bool Equals(T other) => _predicate(other);
            public int CompareTo(T other) => _converter(other);
#if !CS8
            public bool LessThan(T other) => UtilHelpers.LessThan(this, other);
            public bool LessThanOrEqualTo(T other) => UtilHelpers.LessThanOrEqualTo(this, other);
            public bool GreaterThan(T other) => UtilHelpers.GreaterThan(this, other);
            public bool GreaterThanOrEqualTo(T other) => UtilHelpers.GreaterThanOrEqualTo(this, other);
#endif
        }

        internal
#endif
        static class NumberHelper
        {
#if !CS11
            public static ISortableItem<byte> GetNumber(byte value) => new Number<byte>(value.Equals, value.CompareTo);
            public static ISortableItem<sbyte> GetNumber(sbyte value) => new Number<sbyte>(value.Equals, value.CompareTo);
            public static ISortableItem<short> GetNumber(short value) => new Number<short>(value.Equals, value.CompareTo);
            public static ISortableItem<ushort> GetNumber(ushort value) => new Number<ushort>(value.Equals, value.CompareTo);
            public static ISortableItem<int> GetNumber(int value) => new Number<int>(value.Equals, value.CompareTo);
            public static ISortableItem<uint> GetNumber(uint value) => new Number<uint>(value.Equals, value.CompareTo);
            public static ISortableItem<long> GetNumber(long value) => new Number<long>(value.Equals, value.CompareTo);
            public static ISortableItem<ulong> GetNumber(ulong value) => new Number<ulong>(value.Equals, value.CompareTo);
            public static ISortableItem<float> GetNumber(float value) => new Number<float>(value.Equals, value.CompareTo);
            public static ISortableItem<double> GetNumber(double value) => new Number<double>(value.Equals, value.CompareTo);
            public static ISortableItem<decimal> GetNumber(decimal value) => new Number<decimal>(value.Equals, value.CompareTo);
#endif
            public static
#if CS11
                Enum<T>
#else
                ISortableItem<T>
#endif
                GetNumber<T>(T value) where T : Enum => new Enum<T>(value);
        }
    }
}
