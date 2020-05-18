using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace WinCopies.Util
{
    public struct CheckedUInt64 : IComparable<CheckedUInt64>/*, IComparable<CheckedUInt32>, IComparable<CheckedUInt16>, IComparable<CheckedByte>*/
    {
        private readonly ulong? _value;

        public bool IsNaN => _value.HasValue;

        public ulong Value => _value.HasValue ? throw new InvalidOperationException("The current value is not a number.") : _value.Value;

        public ulong MaxValue { get; }

        public CheckedUInt64(ulong value, ulong? maxValue = null)
        {
            MaxValue = maxValue.HasValue ? value > maxValue.Value ? throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(value)} is greater than {nameof(maxValue)}.") : maxValue.Value : ulong.MaxValue;

            _value = value;
        }

        public CheckedUInt64(ulong? value, ulong? maxValue = null)
        {
            if (value.HasValue)

                this = new CheckedUInt64(value.Value, maxValue);

            else
            {

                _value = null;

                MaxValue = 0;

            }
        }

        public CheckedUInt64(bool isNaN)
        {
            if (isNaN)
            {
                _value = null;

                MaxValue = 0;
            }

            else

                this = new CheckedUInt64();
        }

        public int CompareTo(
#if NETCORE
            [AllowNull]
#endif
        CheckedUInt64 other) =>
#if NETCORE
            other == null ||
#endif
            this < other ? -1 : this == other ? 0 : 1;

        public int CompareTo([AllowNull] long other) => other < 0 ? 1 : this < other ? -1 : this == other ? 0 : 1;

        public int CompareTo([AllowNull] int other) => other < 0 ? 1 : this < other ? -1 : this == other ? 0 : 1;

        public int CompareTo([AllowNull] short other) => other < 0 ? 1 : this < other ? -1 : this == other ? 0 : 1;

        public int CompareTo([AllowNull] sbyte other) => other < 0 ? 1 : this < other ? -1 : this == other ? 0 : 1;

        public int CompareTo([AllowNull] ulong other) => this < other ? -1 : this == other ? 0 : 1;

        public int CompareTo([AllowNull] uint other) => this < other ? -1 : this == other ? 0 : 1;

        public int CompareTo([AllowNull] ushort other) => this < other ? -1 : this == other ? 0 : 1;

        public int CompareTo([AllowNull] byte other) => this < other ? -1 : this == other ? 0 : 1;



        private bool IsLessThan(in long other) => other <= 0 || IsNaN ? false : _value < (ulong)other;

        private bool IsGreaterThan(in long other) => IsNaN ? false : other < 0 || (other == 0 && _value != 0) ? true : _value > (ulong)other;

        private bool IsEqualTo(in long other) => IsNaN || other < 0 || (other == 0 && _value != 0) ? false : _value == (ulong)other;

        private bool IsLessThan(in ulong other) => IsNaN ? false : _value < other;

        private bool IsGreaterThan(in ulong other) => IsNaN ? false : other == 0 && _value != 0 ? true : _value > other;

        private bool IsEqualTo(in ulong other) => IsNaN || (other == 0 && _value != 0) ? false : _value == other;

        private bool IsLessThan(in float other) => IsNaN ? false : (float)_value < other;

        private bool IsGreaterThan(in float other) => IsNaN ? false : (float)_value > other;

        private bool IsEqualTo(in float other) => IsNaN ? false : (float)_value == other;

        private bool IsLessThan(in double other) => IsNaN ? false : (double)_value < other;

        private bool IsGreaterThan(in double other) => IsNaN ? false : (double)_value > other;

        private bool IsEqualTo(in double other) => IsNaN ? false : (double)_value == other;

        private bool IsLessThan(in decimal other) => IsNaN ? false : (decimal)_value < other;

        private bool IsGreaterThan(in decimal other) => IsNaN ? false : (decimal)_value > other;

        private bool IsEqualTo(in decimal other) => IsNaN ? false : (decimal)_value == other;

        #region CheckedUInt64 operators

        #region Equality operators 

        #region CheckedUInt64 operators

        public static bool operator <(in CheckedUInt64 s1, in CheckedUInt64 s2) => s1.IsNaN || s2.IsNaN ? false : s1._value < s2._value;

        public static bool operator >(in CheckedUInt64 s1, in CheckedUInt64 s2) => s1.IsNaN || s2.IsNaN ? false : s1._value > s2._value;

        public static bool operator <=(in CheckedUInt64 s1, in CheckedUInt64 s2) => !(s1 > s2);

        public static bool operator >=(in CheckedUInt64 s1, in CheckedUInt64 s2) => !(s1 < s2);

        public static bool operator ==(in CheckedUInt64 s1, in CheckedUInt64 s2) => s1.IsNaN && s2.IsNaN ? true : s1.IsNaN || s2.IsNaN ? false : s1._value == s2._value;

        public static bool operator !=(in CheckedUInt64 s1, in CheckedUInt64 s2) => !(s1 == s2);

        #endregion



        #region CheckedUInt64, numeric operators

        #region sbyte operators

        public static bool operator <(in CheckedUInt64 s, in sbyte l) => s.IsLessThan((long)l);

        public static bool operator >(in CheckedUInt64 s, in sbyte l) => s.IsGreaterThan((long)l);

        public static bool operator <=(in CheckedUInt64 s, in sbyte l) => !s.IsGreaterThan((long)l);

        public static bool operator >=(in CheckedUInt64 s, in sbyte l) => !s.IsLessThan((long)l);

        public static bool operator ==(in CheckedUInt64 s, in sbyte l) => s.IsEqualTo((long)l);

        public static bool operator !=(in CheckedUInt64 s, in sbyte l) => !s.IsEqualTo((long)l);

        #endregion



        #region byte operators

        public static bool operator <(in CheckedUInt64 s, in byte l) => s.IsLessThan((ulong)l);

        public static bool operator >(in CheckedUInt64 s, in byte l) => s.IsGreaterThan((ulong)l);

        public static bool operator <=(in CheckedUInt64 s, in byte l) => !s.IsGreaterThan((ulong)l);

        public static bool operator >=(in CheckedUInt64 s, in byte l) => !s.IsLessThan((ulong)l);

        public static bool operator ==(in CheckedUInt64 s, in byte l) => s.IsEqualTo((ulong)l);

        public static bool operator !=(in CheckedUInt64 s, in byte l) => !s.IsEqualTo((ulong)l);

        #endregion



        #region short operators

        public static bool operator <(in CheckedUInt64 s, in short @short) => s.IsLessThan((long)@short);

        public static bool operator >(in CheckedUInt64 s, in short @short) => s.IsGreaterThan((long)@short);

        public static bool operator <=(in CheckedUInt64 s, in short @short) => !s.IsGreaterThan((long)@short);

        public static bool operator >=(in CheckedUInt64 s, in short @short) => !s.IsLessThan((long)@short);

        public static bool operator ==(in CheckedUInt64 s, in short @short) => s.IsEqualTo((long)@short);

        public static bool operator !=(in CheckedUInt64 s, in short @short) => !s.IsEqualTo((long)@short);

        #endregion



        #region ushort operators

        public static bool operator <(in CheckedUInt64 s, in ushort @short) => s.IsLessThan((ulong)@short);

        public static bool operator >(in CheckedUInt64 s, in ushort @short) => s.IsGreaterThan((ulong)@short);

        public static bool operator <=(in CheckedUInt64 s, in ushort @short) => !s.IsGreaterThan((ulong)@short);

        public static bool operator >=(in CheckedUInt64 s, in ushort @short) => !s.IsLessThan((ulong)@short);

        public static bool operator ==(in CheckedUInt64 s, in ushort @short) => s.IsEqualTo((ulong)@short);

        public static bool operator !=(in CheckedUInt64 s, in ushort @short) => !s.IsEqualTo((ulong)@short);

        #endregion



        #region int operators

        public static bool operator <(in CheckedUInt64 s, in int i) => s.IsLessThan((long)i);

        public static bool operator >(in CheckedUInt64 s, in int i) => s.IsGreaterThan((long)i);

        public static bool operator <=(in CheckedUInt64 s, in int i) => !s.IsGreaterThan((long)i);

        public static bool operator >=(in CheckedUInt64 s, in int i) => !s.IsLessThan((long)i);

        public static bool operator ==(in CheckedUInt64 s, in int i) => s.IsEqualTo((long)i);

        public static bool operator !=(in CheckedUInt64 s, in int i) => !s.IsEqualTo((long)i);

        #endregion



        #region uint operators

        public static bool operator <(in CheckedUInt64 s, in uint i) => s.IsLessThan((ulong)i);

        public static bool operator >(in CheckedUInt64 s, in uint i) => s.IsGreaterThan((ulong)i);

        public static bool operator <=(in CheckedUInt64 s, in uint i) => !s.IsGreaterThan((ulong)i);

        public static bool operator >=(in CheckedUInt64 s, in uint i) => !s.IsLessThan((ulong)i);

        public static bool operator ==(in CheckedUInt64 s, in uint i) => s.IsEqualTo((ulong)i);

        public static bool operator !=(in CheckedUInt64 s, in uint i) => !s.IsEqualTo((ulong)i);

        #endregion



        #region long operators

        public static bool operator <(in CheckedUInt64 s, in long l) => s.IsLessThan(l);

        public static bool operator >(in CheckedUInt64 s, in long l) => s.IsGreaterThan(l);

        public static bool operator <=(in CheckedUInt64 s, in long l) => !s.IsGreaterThan(l);

        public static bool operator >=(in CheckedUInt64 s, in long l) => !s.IsLessThan(l);

        public static bool operator ==(in CheckedUInt64 s, in long l) => s.IsEqualTo(l);

        public static bool operator !=(in CheckedUInt64 s, in long l) => !s.IsEqualTo(l);

        #endregion



        #region ulong operators

        public static bool operator <(in CheckedUInt64 s, in ulong l) => s.IsLessThan(l);

        public static bool operator >(in CheckedUInt64 s, in ulong l) => s.IsGreaterThan(l);

        public static bool operator <=(in CheckedUInt64 s, in ulong l) => !s.IsGreaterThan(l);

        public static bool operator >=(in CheckedUInt64 s, in ulong l) => !s.IsLessThan(l);

        public static bool operator ==(in CheckedUInt64 s, in ulong l) => s.IsEqualTo(l);

        public static bool operator !=(in CheckedUInt64 s, in ulong l) => !s.IsEqualTo(l);

        #endregion



        #region float operators

        public static bool operator <(in CheckedUInt64 s, in float f) => s.IsLessThan(f);

        public static bool operator >(in CheckedUInt64 s, in float f) => s.IsGreaterThan(f);

        public static bool operator <=(in CheckedUInt64 s, in float f) => !s.IsGreaterThan(f);

        public static bool operator >=(in CheckedUInt64 s, in float f) => !s.IsLessThan(f);

        public static bool operator ==(in CheckedUInt64 s, in float f) => s.IsEqualTo(f);

        public static bool operator !=(in CheckedUInt64 s, in float f) => !s.IsEqualTo(f);

        #endregion



        #region double operators

        public static bool operator <(in CheckedUInt64 s, in double d) => s.IsLessThan(d);

        public static bool operator >(in CheckedUInt64 s, in double d) => s.IsGreaterThan(d);

        public static bool operator <=(in CheckedUInt64 s, in double d) => !s.IsGreaterThan(d);

        public static bool operator >=(in CheckedUInt64 s, in double d) => !s.IsLessThan(d);

        public static bool operator ==(in CheckedUInt64 s, in double d) => s.IsEqualTo(d);

        public static bool operator !=(in CheckedUInt64 s, in double d) => !s.IsEqualTo(d);

        #endregion



        #region decimal operators

        public static bool operator <(in CheckedUInt64 s, in decimal d) => s.IsLessThan(d);

        public static bool operator >(in CheckedUInt64 s, in decimal d) => s.IsGreaterThan(d);

        public static bool operator <=(in CheckedUInt64 s, in decimal d) => !s.IsGreaterThan(d);

        public static bool operator >=(in CheckedUInt64 s, in decimal d) => !s.IsLessThan(d);

        public static bool operator ==(in CheckedUInt64 s, in decimal d) => s.IsEqualTo(d);

        public static bool operator !=(in CheckedUInt64 s, in decimal d) => !s.IsEqualTo(d);

        #endregion

        #endregion



        #region Numeric, CheckedUInt64 operators

        #region sbyte operators

        public static bool operator <(in sbyte l, in CheckedUInt64 s) => s.IsGreaterThan((long)l);

        public static bool operator >(in sbyte l, in CheckedUInt64 s) => s.IsLessThan((long)l);

        public static bool operator <=(in sbyte l, in CheckedUInt64 s) => !s.IsLessThan((long)l);

        public static bool operator >=(in sbyte l, in CheckedUInt64 s) => !s.IsGreaterThan((long)l);

        public static bool operator ==(in sbyte l, in CheckedUInt64 s) => s.IsEqualTo((long)l);

        public static bool operator !=(in sbyte l, in CheckedUInt64 s) => !s.IsEqualTo((long)l);

        #endregion



        #region byte operators

        public static bool operator <(in byte l, in CheckedUInt64 s) => s.IsGreaterThan((ulong)l);

        public static bool operator >(in byte l, in CheckedUInt64 s) => s.IsLessThan((ulong)l);

        public static bool operator <=(in byte l, in CheckedUInt64 s) => !s.IsLessThan((ulong)l);

        public static bool operator >=(in byte l, in CheckedUInt64 s) => !s.IsGreaterThan((ulong)l);

        public static bool operator ==(in byte l, in CheckedUInt64 s) => s.IsEqualTo((ulong)l);

        public static bool operator !=(in byte l, in CheckedUInt64 s) => !s.IsEqualTo((ulong)l);

        #endregion



        #region short operators

        public static bool operator <(in short l, in CheckedUInt64 s) => s.IsGreaterThan((long)l);

        public static bool operator >(in short l, in CheckedUInt64 s) => s.IsLessThan((long)l);

        public static bool operator <=(in short l, in CheckedUInt64 s) => !s.IsLessThan((long)l);

        public static bool operator >=(in short l, in CheckedUInt64 s) => !s.IsGreaterThan((long)l);

        public static bool operator ==(in short l, in CheckedUInt64 s) => s.IsEqualTo((long)l);

        public static bool operator !=(in short l, in CheckedUInt64 s) => !s.IsEqualTo((long)l);

        #endregion



        #region ushort operators

        public static bool operator <(in ushort l, in CheckedUInt64 s) => s.IsGreaterThan((ulong)l);

        public static bool operator >(in ushort l, in CheckedUInt64 s) => s.IsLessThan((ulong)l);

        public static bool operator <=(in ushort l, in CheckedUInt64 s) => !s.IsLessThan((ulong)l);

        public static bool operator >=(in ushort l, in CheckedUInt64 s) => !s.IsGreaterThan((ulong)l);

        public static bool operator ==(in ushort l, in CheckedUInt64 s) => s.IsEqualTo((ulong)l);

        public static bool operator !=(in ushort l, in CheckedUInt64 s) => !s.IsEqualTo((ulong)l);

        #endregion



        #region int operators

        public static bool operator <(in int l, in CheckedUInt64 s) => s.IsGreaterThan((long)l);

        public static bool operator >(in int l, in CheckedUInt64 s) => s.IsLessThan((long)l);

        public static bool operator <=(in int l, in CheckedUInt64 s) => !s.IsLessThan((long)l);

        public static bool operator >=(in int l, in CheckedUInt64 s) => !s.IsGreaterThan((long)l);

        public static bool operator ==(in int l, in CheckedUInt64 s) => s.IsEqualTo((long)l);

        public static bool operator !=(in int l, in CheckedUInt64 s) => !s.IsEqualTo((long)l);

        #endregion



        #region uint operators

        public static bool operator <(in uint l, in CheckedUInt64 s) => s.IsGreaterThan((ulong)l);

        public static bool operator >(in uint l, in CheckedUInt64 s) => s.IsLessThan((ulong)l);

        public static bool operator <=(in uint l, in CheckedUInt64 s) => !s.IsLessThan((ulong)l);

        public static bool operator >=(in uint l, in CheckedUInt64 s) => !s.IsGreaterThan((ulong)l);

        public static bool operator ==(in uint l, in CheckedUInt64 s) => s.IsEqualTo((ulong)l);

        public static bool operator !=(in uint l, in CheckedUInt64 s) => !s.IsEqualTo((ulong)l);

        #endregion



        #region long operators

        public static bool operator <(in long l, in CheckedUInt64 s) => s.IsGreaterThan(l);

        public static bool operator >(in long l, in CheckedUInt64 s) => s.IsLessThan(l);

        public static bool operator <=(in long l, in CheckedUInt64 s) => !s.IsLessThan(l);

        public static bool operator >=(in long l, in CheckedUInt64 s) => !s.IsGreaterThan(l);

        public static bool operator ==(in long l, in CheckedUInt64 s) => s.IsEqualTo(l);

        public static bool operator !=(in long l, in CheckedUInt64 s) => !s.IsEqualTo(l);

        #endregion



        #region ulong operators

        public static bool operator <(in ulong l, in CheckedUInt64 s) => s.IsGreaterThan(l);

        public static bool operator >(in ulong l, in CheckedUInt64 s) => s.IsLessThan(l);

        public static bool operator <=(in ulong l, in CheckedUInt64 s) => !s.IsLessThan(l);

        public static bool operator >=(in ulong l, in CheckedUInt64 s) => !s.IsGreaterThan(l);

        public static bool operator ==(in ulong l, in CheckedUInt64 s) => s.IsEqualTo(l);

        public static bool operator !=(in ulong l, in CheckedUInt64 s) => !s.IsEqualTo(l);

        #endregion



        #region float operators

        public static bool operator <(in float l, in CheckedUInt64 s) => s.IsGreaterThan(l);

        public static bool operator >(in float l, in CheckedUInt64 s) => s.IsLessThan(l);

        public static bool operator <=(in float l, in CheckedUInt64 s) => !s.IsLessThan(l);

        public static bool operator >=(in float l, in CheckedUInt64 s) => !s.IsGreaterThan(l);

        public static bool operator ==(in float l, in CheckedUInt64 s) => s.IsEqualTo(l);

        public static bool operator !=(in float l, in CheckedUInt64 s) => !s.IsEqualTo(l);

        #endregion



        #region double operators

        public static bool operator <(in double l, in CheckedUInt64 s) => s.IsGreaterThan(l);

        public static bool operator >(in double l, in CheckedUInt64 s) => s.IsLessThan(l);

        public static bool operator <=(in double l, in CheckedUInt64 s) => !s.IsLessThan(l);

        public static bool operator >=(in double l, in CheckedUInt64 s) => !s.IsGreaterThan(l);

        public static bool operator ==(in double l, in CheckedUInt64 s) => s.IsEqualTo(l);

        public static bool operator !=(in double l, in CheckedUInt64 s) => !s.IsEqualTo(l);

        #endregion



        #region decimal operators

        public static bool operator <(in decimal l, in CheckedUInt64 s) => s.IsGreaterThan(l);

        public static bool operator >(in decimal l, in CheckedUInt64 s) => s.IsLessThan(l);

        public static bool operator <=(in decimal l, in CheckedUInt64 s) => !s.IsLessThan(l);

        public static bool operator >=(in decimal l, in CheckedUInt64 s) => !s.IsGreaterThan(l);

        public static bool operator ==(in decimal l, in CheckedUInt64 s) => s.IsEqualTo(l);

        public static bool operator !=(in decimal l, in CheckedUInt64 s) => !s.IsEqualTo(l);

        #endregion

        #endregion

        #endregion



        #region Arithmetic operators

        #region CheckedUInt64 operators

        public static CheckedUInt64 operator +(in CheckedUInt64 s1, in CheckedUInt64 s2)
        {
            if (s1.IsNaN || s2.IsNaN)

                throw new InvalidOperationException("Cannot add 'not a number' values.");

            else
            {
                ulong maxValue = System.Math.Min(s1.MaxValue, s2.MaxValue);

                ulong? result = Math.TryAdd(s1._value.Value, s2._value.Value, maxValue);

                return result.HasValue ? new CheckedUInt64(result.Value, maxValue) : new CheckedUInt64(true);
            }
        }

        //public static CheckedUInt64 operator -(in CheckedUInt64 s1, in CheckedUInt64 s2) => new CheckedUInt64(s1.ValueInBytes - s2.ValueInBytes);

        public static CheckedUInt64 operator *(in CheckedUInt64 s1, in CheckedUInt64 s2)
        {
            if (s1.IsNaN || s2.IsNaN)

                throw new InvalidOperationException("Cannot add 'not a number' values.");

            else
            {
                ulong maxValue = System.Math.Min(s1.MaxValue, s2.MaxValue);

                ulong? result = Math.TryMultiply(s1._value.Value, s2._value.Value, maxValue);

                return result.HasValue ? new CheckedUInt64(result.Value, maxValue) : new CheckedUInt64(true);
            }
        }

        //public static CheckedUInt64 operator /(in CheckedUInt64 s1, in CheckedUInt64 s2) => new CheckedUInt64(s1.ValueInBytes / s2.ValueInBytes);

        //public static CheckedUInt64 operator %(in CheckedUInt64 s1, in CheckedUInt64 s2) => new CheckedUInt64(s1.ValueInBytes % s2.ValueInBytes);

        #endregion



        #region CheckedUInt64, numeric operators

        #region sbyte operators

        public static CheckedUInt64 operator +(in CheckedUInt64 s, in sbyte l) => s + (long)l;

        public static CheckedUInt64 operator *(in CheckedUInt64 s, in sbyte l) => s * (long)l;

        #endregion



        #region byte operators

        public static CheckedUInt64 operator +(in CheckedUInt64 s, in byte l) => s + (ulong)l;

        public static CheckedUInt64 operator *(in CheckedUInt64 s, in byte l) => s * (ulong)l;

        #endregion



        #region short operators

        public static CheckedUInt64 operator +(in CheckedUInt64 s, in short @short) => s + (long)@short;

        public static CheckedUInt64 operator *(in CheckedUInt64 s, in short @short) => s * (long)@short;

        #endregion



        #region ushort operators

        public static CheckedUInt64 operator +(in CheckedUInt64 s, in ushort @short) => s + (ulong)@short;

        public static CheckedUInt64 operator *(in CheckedUInt64 s, in ushort @short) => s * (ulong)@short;

        #endregion



        #region int operators

        public static CheckedUInt64 operator +(in CheckedUInt64 s, in int i) => s + (long)i;

        public static CheckedUInt64 operator *(in CheckedUInt64 s, in int i) => s * (long)i;

        #endregion



        #region uint operators

        public static CheckedUInt64 operator +(in CheckedUInt64 s, in uint i) => s + (ulong)i;

        public static CheckedUInt64 operator *(in CheckedUInt64 s, in uint i) => s * (ulong)i;

        #endregion



        #region long operators

        public static CheckedUInt64 operator +(in CheckedUInt64 s, in long l)
        {
            if (s.IsNaN)

                throw new InvalidOperationException("Cannot add 'not a number' values.");

            else
            {
                if (l < 0)

                    throw new ArgumentOutOfRangeException(nameof(l), l, $"{nameof(l)} cannot be less than zero.");

                ulong? result = Math.TryAdd(s._value.Value, (ulong)l, s.MaxValue);

                return result.HasValue ? new CheckedUInt64(result.Value, s.MaxValue) : new CheckedUInt64(true);
            }
        }

        public static CheckedUInt64 operator *(in CheckedUInt64 s, in long l)
        {
            if (s.IsNaN)

                throw new InvalidOperationException("Cannot add 'not a number' values.");

            else
            {
                if (l < 0)

                    throw new ArgumentOutOfRangeException(nameof(l), l, $"{nameof(l)} cannot be less than zero.");

                ulong? result = Math.TryMultiply(s._value.Value, (ulong)l, s.MaxValue);

                return result.HasValue ? new CheckedUInt64(result.Value, s.MaxValue) : new CheckedUInt64(true);
            }
        }

        #endregion



        #region ulong operators

        public static CheckedUInt64 operator +(in CheckedUInt64 s, in ulong l)
        {
            if (s.IsNaN)

                throw new InvalidOperationException("Cannot add 'not a number' values.");

            else
            {
                ulong? result = Math.TryAdd(s._value.Value, l, s.MaxValue);

                return result.HasValue ? new CheckedUInt64(result.Value, s.MaxValue) : new CheckedUInt64(true);
            }
        }

        public static CheckedUInt64 operator *(in CheckedUInt64 s, in ulong l)
        {
            if (s.IsNaN)

                throw new InvalidOperationException("Cannot add 'not a number' values.");

            else
            {
                ulong? result = Math.TryMultiply(s._value.Value, l, s.MaxValue);

                return result.HasValue ? new CheckedUInt64(result.Value, s.MaxValue) : new CheckedUInt64(true);
            }
        }

        #endregion

        #endregion



        #region Numeric, CheckedUInt64 operators

        #region sbyte operators

        public static CheckedUInt64 operator +(in sbyte l, in CheckedUInt64 s) => s + l;

        public static CheckedUInt64 operator *(in sbyte l, in CheckedUInt64 s) => s * l;

        #endregion



        #region byte operators

        public static CheckedUInt64 operator +(in byte l, in CheckedUInt64 s) => s + l;

        public static CheckedUInt64 operator *(in byte l, in CheckedUInt64 s) => s * l;

        #endregion



        #region short operators

        public static CheckedUInt64 operator +(in short @short, in CheckedUInt64 s) => s + @short;

        public static CheckedUInt64 operator *(in short @short, in CheckedUInt64 s) => s * @short;

        #endregion



        #region ushort operators

        public static CheckedUInt64 operator +(in ushort @short, in CheckedUInt64 s) => s+@short;

        public static CheckedUInt64 operator *(in ushort @short, in CheckedUInt64 s) => s*@short;

        #endregion



        #region int operators

        public static CheckedUInt64 operator +(in int i, in CheckedUInt64 s) => s + i;

        public static CheckedUInt64 operator *(in int i, in CheckedUInt64 s) => s * i;

        #endregion



        #region uint operators

        public static CheckedUInt64 operator +(in uint i, in CheckedUInt64 s) => s+i;

        public static CheckedUInt64 operator *(in uint i, in CheckedUInt64 s) => s*i;

        #endregion



        #region long operators

        public static CheckedUInt64 operator +(in long l, in CheckedUInt64 s) => s+l;

        public static CheckedUInt64 operator *(in long l, in CheckedUInt64 s) => s*l;

        #endregion



        #region ulong operators

        public static CheckedUInt64 operator +(in ulong l, in CheckedUInt64 s) => s+l;

        public static CheckedUInt64 operator *(in ulong l, in CheckedUInt64 s) => s*l;

        #endregion

        #endregion

        #endregion



        //#region CheckedUInt64 operators

        ///// <summary>
        ///// Returns the size.
        ///// </summary>
        ///// <param name="s">CheckedUInt64 to return</param>
        ///// <returns>The size value</returns> 
        //public static CheckedUInt64 operator +(CheckedUInt64 s) => new CheckedUInt64(+s.ValueInBytes, s.Unit);

        ///// <summary>
        ///// Returns the size opposite.
        ///// </summary>
        ///// <param name="s">CheckedUInt64 for which one return the opposite</param>
        ///// <returns>The opposite of the size value</returns> 
        //public static CheckedUInt64 operator -(CheckedUInt64 s) => new CheckedUInt64(-s.ValueInBytes, s.Unit);

        //public static CheckedUInt64 operator ~(CheckedUInt64 s) => new CheckedUInt64(~s.GetValueInUnit(s.Unit), s.Unit); 

        //#endregion



        //public static CheckedUInt64 operator <<(CheckedUInt64 s, int i) => Create(s.GetValueInUnit(Unit.Byte) << i);

        //public static CheckedUInt64 operator >>(CheckedUInt64 s, int i) => Create(s.GetValueInUnit(Unit.Byte) >> i);



        #region Cast operators

        #region Numeric value to CheckedUInt64

        public static explicit operator CheckedUInt64(sbyte l) => l < 0 ? throw new ArgumentOutOfRangeException(nameof(l), l, $"{nameof(l)} cannot be less than zero.") : new CheckedUInt64((ulong)l);

        public static explicit operator CheckedUInt64(byte l) => new CheckedUInt64((ulong)l);

        public static explicit operator CheckedUInt64(short l) => l < 0 ? throw new ArgumentOutOfRangeException(nameof(l), l, $"{nameof(l)} cannot be less than zero.") : new CheckedUInt64((ulong)l);

        public static explicit operator CheckedUInt64(ushort l) => new CheckedUInt64((ulong)l);

        public static explicit operator CheckedUInt64(int l) => l < 0 ? throw new ArgumentOutOfRangeException(nameof(l), l, $"{nameof(l)} cannot be less than zero.") : new CheckedUInt64((ulong)l);

        public static explicit operator CheckedUInt64(uint l) => new CheckedUInt64((ulong)l);

        public static explicit operator CheckedUInt64(long l) => l < 0 ? throw new ArgumentOutOfRangeException(nameof(l), l, $"{nameof(l)} cannot be less than zero.") : new CheckedUInt64((ulong)l);

        public static explicit operator CheckedUInt64(ulong l) => new CheckedUInt64(l);

        #endregion

        #region CheckedUInt64 to numeric value

        public static explicit operator sbyte(CheckedUInt64 s) => s.IsNaN ? throw new InvalidOperationException($"{nameof(s)} is not a number.") :    (sbyte) s._value.Value ; 

        public static explicit operator byte(CheckedUInt64 s) => s.IsNaN ? throw new InvalidOperationException($"{nameof(s)} is not a number.") : (byte)s._value.Value;

        public static explicit operator short(CheckedUInt64 s) => s.IsNaN ? throw new InvalidOperationException($"{nameof(s)} is not a number.") : (short)s._value.Value;

        public static explicit operator ushort(CheckedUInt64 s) => s.IsNaN ? throw new InvalidOperationException($"{nameof(s)} is not a number.") : (ushort)s._value.Value;

        public static explicit operator int(CheckedUInt64 s) => s.IsNaN ? throw new InvalidOperationException($"{nameof(s)} is not a number.") : (int)s._value.Value;

        public static explicit operator uint(CheckedUInt64 s) => s.IsNaN ? throw new InvalidOperationException($"{nameof(s)} is not a number.") : (uint)s._value.Value;

        public static explicit operator long(CheckedUInt64 s) => s.IsNaN ? throw new InvalidOperationException($"{nameof(s)} is not a number.") : (long)s._value.Value;

        public static explicit operator ulong(CheckedUInt64 s) => s.IsNaN ? throw new InvalidOperationException($"{nameof(s)} is not a number.") : (ulong)s._value.Value;

        public static explicit operator float(CheckedUInt64 s) => s.IsNaN ? throw new InvalidOperationException($"{nameof(s)} is not a number.") : (float)s._value.Value;

        public static explicit operator double(CheckedUInt64 s) => s.IsNaN ? throw new InvalidOperationException($"{nameof(s)} is not a number.") : (double)s._value.Value;

        public static explicit operator decimal(CheckedUInt64 s) => s.IsNaN ? throw new InvalidOperationException($"{nameof(s)} is not a number.") : (decimal)s._value.Value;

        #endregion

        #endregion

        #endregion
    }
}
