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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
#if CS6
using System.Threading.Tasks;

using WinCopies.Collections;
using WinCopies.Extensions.Util;
#endif
using WinCopies.Util;

using static WinCopies.Delegates;
using static WinCopies.ThrowHelper;
using static WinCopies.UtilHelpers;

namespace WinCopies
{
    public readonly
#if CS10
        record
#endif
        struct Interval
#if WinCopies4
        <T>
#if !CS10
        where T : unmanaged
#endif
#endif
#if CS10
        (
#else
    {
        public readonly
#endif
#if WinCopies4
            T
#else
            ulong
#endif
            Start
#if CS10
        ,
#else
    ;
        public readonly
#endif
#if WinCopies4
            T
#else
            ulong
#endif
            Length
#if CS10
        )
#if WinCopies4
        where T : unmanaged
#endif
        ;
#else
            ;

        public Interval(in
#if WinCopies4
            T
#else
            ulong
#endif
            start, in
#if WinCopies4
            T
#else
            ulong
#endif
            length)
        {
            Start = start;
            Length = length;
        }
    }
#endif
#if CS5
    public readonly struct Version : IEquatable<Version>, IComparable<Version>
    {
        public enum VersionPart : byte
        {
            Major = 1,
            Minor,
            Build,
            Revision
        }

        private readonly ulong _highValue;
        private readonly ulong _lowValue;

        public Version(in ulong highValue, in ulong lowValue)
        {
            _highValue = highValue;
            _lowValue = lowValue;
        }
        public Version(in uint major, in uint minor, in uint build, in uint revision) => this = new
#if !CS9
            Version
#endif
            (major.Concatenate(minor), build.Concatenate(revision));
        public Version(in int major, in int minor, in int build, in int revision) => this = new
#if !CS9
            Version
#endif
            ((uint)major, (uint)minor, (uint)build, (uint)revision);

        public Version(in string version)
        {
            string[] versionParts = version.Split('.');

            byte i = 0;

            uint getVersionPart() => uint.Parse(versionParts[i++]);

            this = new
#if !CS9
                Version
#endif
                (getVersionPart(), getVersionPart(), getVersionPart(), getVersionPart());
        }

        public uint GetPart(VersionPart part)
        {
            uint getValue(in ulong value) => GetValue(value, ((byte)part % 2) == 0, Util.Extensions.GetLowPart, Util.Extensions.GetHighPart);

            return ((byte)part).Between((byte)VersionPart.Major, (byte)VersionPart.Revision, true, true) ? getValue(part < VersionPart.Build ? _highValue : _lowValue) : throw new InvalidEnumArgumentException(nameof(part), part);
        }

        public bool Equals(Version other) => _highValue == other._highValue && _lowValue == other._lowValue;

        public override bool Equals(
#if CS8
            [NotNullWhen(true)]
#endif
            object
#if CS8
            ?
#endif
            obj) => obj is Version other && Equals(other);

        public override int GetHashCode() => _highValue.GetHashCode() ^ _lowValue.GetHashCode();

        public int CompareTo(Version other)
        {
            int result = _highValue.CompareTo(other._highValue);

            return result == 0 ? _lowValue.CompareTo(other._lowValue) : result;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            void append(in Version version, in VersionPart part) => result.Append(version.GetPart(part));

            ActionIn<Version, VersionPart> action = (in Version version, in VersionPart part) =>
            {
                append(version, part);

                action = (in Version _version, in VersionPart _part) =>
                {
                    _ = result.Append('.');

                    append(_version, _part);
                };
            };

            foreach (VersionPart value in GetEnumConstants<VersionPart>())

                action(this, value);

            return result.ToString();
        }

        public static explicit operator Version(System.Version version) => new
#if !CS9
            Version
#endif
            (version.Major, version.Minor, version.Build, version.Revision);

        public static bool operator ==(Version left, Version right) => left.Equals(right);
        public static bool operator !=(Version left, Version right) => !(left == right);
        public static bool operator <(Version left, Version right) => left.CompareTo(right) < 0;
        public static bool operator <=(Version left, Version right) => left.CompareTo(right) <= 0;
        public static bool operator >(Version left, Version right) => left.CompareTo(right) > 0;
        public static bool operator >=(Version left, Version right) => left.CompareTo(right) >= 0;
    }
#endif
    public static class Array
    {
        private static unsafe void Shift(ulong* sourcePtr, ulong* destinationPtr, ulong length)
        {
            bool check()
            {
                if (--length > 0)
                {
                    sourcePtr--;
                    destinationPtr--;

                    return true;
                }

                return false;
            }

            do

                *destinationPtr = *sourcePtr;

            while (check());
        }
        private static unsafe void Shift(byte* sourcePtr, byte* destinationPtr, ulong length)
        {
            bool processExtraBytes()
            {
                void move() => *destinationPtr = *sourcePtr;

                void shift<T>() where T : unmanaged
                {
                    void decrementPtr(ref byte* ptr) => ptr -= sizeof(T) - 1;

                    decrementPtr(ref sourcePtr);
                    decrementPtr(ref destinationPtr);

                    *((T*)destinationPtr) = *((T*)sourcePtr);
                }

                var extraBytes = (byte)(length % sizeof(long));

            SWITCH:
                switch (extraBytes)
                {
                    case 0:

                        return false;

                    case sizeof(int):

                        shift<int>();

                        break;

                    case sizeof(short):

                        shift<short>();

                        break;

                    case (sizeof(byte)):

                        move();

                        break;

                    default:

                        if ((extraBytes / sizeof(int)) == 0)
                        {
                            shift<short>();
                            move();

                            break;
                        }

                        shift<int>();

                        extraBytes %= sizeof(int);

                        goto SWITCH;
                }

                return true;
            }

            bool processedExtraBytes = processExtraBytes();

            if ((length /= sizeof(long)) == 0)

                return;

            if (processedExtraBytes)
            {
                sourcePtr--;
                destinationPtr--;
            }

            Shift((ulong*)sourcePtr, (ulong*)destinationPtr, length);
        }
        public static unsafe void Shift(in void* arrayPtr, ref ulong offset, in ulong length)
        {
            if (length == 0UL || (offset %= length) == 0UL)

                return;

            byte* sourcePtr = (byte*)arrayPtr + length - 1;
            byte* destinationPtr = sourcePtr + offset;

            Shift(sourcePtr, destinationPtr, length);
        }
        public static unsafe void Shift<T>(in T* arrayPtr, ref ulong offset, ref ulong length) where T : unmanaged
        {
            if (length == 0UL || (offset %= length) == 0UL)

                return;

            ulong _offset = offset *= (uint)sizeof(T);

            Shift(arrayPtr, ref _offset, length *= (uint)sizeof(T));
        }
        public static unsafe void Shift<T>(in T[] array, in int start, ref ulong offset, ref ulong length) where T : unmanaged
        {
            int arrayLength = array.Length;

            if (length == 0UL || (offset %= (uint)arrayLength) == 0UL)

                return;

            fixed (T* arrayPtr = start.Between(0, arrayLength, true, false) ? length.Between(0, (uint)(arrayLength -= start)) ? offset < (uint)arrayLength ? array : throw new ArgumentOutOfRangeException(nameof(offset)) : throw new ArgumentOutOfRangeException(nameof(length)) : throw new ArgumentOutOfRangeException(nameof(start)))

                Shift(arrayPtr + start, ref offset, ref length);
        }
#if CS5
        public static unsafe void Unshift(in void* arrayPtr, ref ulong offset, in ulong size, in ulong length) => Buffer.MemoryCopy(arrayPtr, (byte*)arrayPtr + offset, size, length);
        public static unsafe void Unshift<T>(in T* arrayPtr, ref ulong offset, in ulong size, ref ulong length) where T : unmanaged
        {
            ulong _offset = offset *= (uint)sizeof(T);

            Unshift(arrayPtr, ref _offset, (size - offset) * (uint)sizeof(T), length *= (uint)sizeof(T));
        }
        public static unsafe void Unshift<T>(in T[] array, in int start, ref ulong offset, ref ulong length) where T : unmanaged
        {
            fixed (T* arrayPtr = array)

                Unshift(arrayPtr + start, ref offset, (ulong)array.Length, ref length);
        }
#endif
#if !CS8
        public static void Fill<T>(in T[] array, in T value)
        {
            for (int i = 0; i < array.Length; i++)

                array[i] = value;
        }
#if !CS5
        private static class EmptyArray<T>
        {
            internal static readonly T[] Array = new T[0];
        }

        public static T[] Empty<T>() => EmptyArray<T>.Array;
#endif
#endif
        public static T[] Repeat<T>(in T value, in int length)
        {
            var array = new T[length];
#if CS8
            System.Array.
#endif
            Fill(array, value);

            return array;
        }
    }
    public interface ISortableItem<T> : IEquatable<T>, IComparable<T>, Collections.Generic.IComparable<T>
    {
#if CS8
        bool Collections.Generic.IComparable<T>.LessThan(T other) => CompareTo(other) < 0;
        bool Collections.Generic.IComparable<T>.LessThanOrEqualTo(T other) => CompareTo(other) <= 0;
        bool Collections.Generic.IComparable<T>.GreaterThan(T other) => CompareTo(other) > 0;
        bool Collections.Generic.IComparable<T>.GreaterThanOrEqualTo(T other) => CompareTo(other) >= 0;
#endif
    }

    /// <summary>
    /// Provides some static helper methods.
    /// </summary>
    public static class UtilHelpers
    {
        private static unsafe byte[] GetBytes<T>(in T* c, in int offset, int length) where T : unmanaged
        {
            byte[] bytes = new byte[length *= sizeof(T)];

            Marshal.Copy((IntPtr)(c + offset), bytes, 0, length);

            return bytes;
        }
        private static unsafe int GetLength<T>(in byte[] bytes, in string paramName) where T : unmanaged
        {
            int length = bytes.Length;

            return length % sizeof(T) == 0 ? length / sizeof(T) : throw new ArgumentException("The length of the given array must be a multiple of sizeof(T).", paramName);
        }

        private static bool PreValidateGetBytes(in int offset, in int length)
        {
            ValidateOffset(offset);

            return length < 0 ? throw new ArgumentOutOfRangeException(nameof(length)) : length == 0;
        }
        private static void PostValidateGetBytes(in int actualLength, in int offset, in int length)
        {
            ValidateOffset(offset, actualLength);

            if (length > actualLength - offset)

                throw new ArgumentOutOfRangeException(nameof(length));
        }

        private static void ValidateOffset(in int offset)
        {
            if (offset < 0)

                throw new ArgumentOutOfRangeException(nameof(offset));
        }
        private static void ValidateOffset(in int offset, in int length)
        {
            if (offset >= length)

                throw new ArgumentOutOfRangeException(nameof(offset));
        }
        private static void ValidateLength(in int length, in int actualLength)
        {
            if (length > actualLength)

                throw new ArgumentOutOfRangeException(nameof(length));
        }
        private static unsafe void ValidateArray<T>(in byte[] bytes, in int offset, in int length) where T : unmanaged
        {
            ValidateOffset(offset, bytes.Length);
            ValidateLength(length, (bytes.Length - offset) / sizeof(T));
        }

        public static unsafe byte[] GetBytes(in string s, in int offset, in int length)
        {
            if (PreValidateGetBytes(offset, length))

                return
#if CS5
                    System
#else
                    WinCopies
#endif
                    .Array.Empty<byte>();

            PostValidateGetBytes(s.Length, offset, length);

            fixed (char* c = s)

                return GetBytes(c, offset, length);
        }
        public static unsafe byte[] GetBytes(in string s) => GetBytes(s, 0, s.Length);
        public static unsafe byte[] GetBytes<T>(in T[] atoms, in int offset, in int length) where T : unmanaged
        {
            if (PreValidateGetBytes(offset, length))

                return
#if CS5
                    System
#else
                    WinCopies
#endif
                    .Array.Empty<byte>();

            PostValidateGetBytes(atoms.Length, offset, length);

            fixed (T* c = atoms)

                return GetBytes(c, offset, length);
        }
        public static unsafe byte[] GetBytes<T>(in T[] atoms) where T : unmanaged => GetBytes(atoms, 0, atoms.Length);
        public static unsafe byte[] GetBytes<T>(T c) where T : unmanaged => GetBytes(&c, 0, 1);

        public static unsafe T GetAtom<T>(in byte[] bytes, in int offset = 0) where T : unmanaged
        {
            fixed (byte* b = bytes.Length < sizeof(T) ? throw new ArgumentException("The length of the given array must be greater than or equal to sizeof(T).", nameof(bytes)) : offset.Between(0, bytes.Length, true, false) && (bytes.Length - offset) >= sizeof(T) ? bytes : throw new ArgumentOutOfRangeException(nameof(offset)))

                return *(T*)b;
        }
#if CS5
        public static unsafe void GetAtoms<T>(in byte[] bytes, in T[] atoms, in int bytesOffset, in int atomsOffset, in int length) where T : unmanaged
        {
            if (PreValidateGetBytes(bytesOffset, length))

                return;

            ValidateOffset(atomsOffset);
            ValidateArray<T>(bytes, bytesOffset, length);
            ValidateOffset(atomsOffset, atoms.Length);

            int actualAtomsLength = atoms.Length - atomsOffset;

            ValidateLength(length, actualAtomsLength);

            fixed (byte* pBytes = bytes)
            fixed (T* pAtoms = atoms)

                Buffer.MemoryCopy(pBytes + bytesOffset, pAtoms + atomsOffset, actualAtomsLength * sizeof(T), length);
        }
        public static unsafe void GetAtoms<T>(in byte[] bytes, in T[] atoms) where T : unmanaged => GetAtoms(bytes, atoms, 0, 0, bytes.Length / sizeof(T));
        public static unsafe T[] GetAtoms<T>(in byte[] bytes, in int bytesOffset, in int atomsOffset, in int length) where T : unmanaged
        {
            var atoms = new T[length];

            GetAtoms(bytes, atoms, bytesOffset, atomsOffset, length);

            return atoms;
        }
        public static unsafe T[] GetAtoms<T>(in byte[] bytes) where T : unmanaged => GetAtoms<T>(bytes, 0, 0, GetLength<T>(bytes, nameof(bytes)));
#endif
        public static unsafe string GetString(in byte[] bytes, in int offset, in int length)
        {
            if (PreValidateGetBytes(offset, length))

                return string.Empty;

            ValidateArray<char>(bytes, offset, length);

            fixed (byte* b = bytes)

                return new string((char*)(b + offset), 0, length);
        }
        public static unsafe string GetString(in byte[] bytes) => GetString(bytes, 0, GetLength<char>(bytes, nameof(bytes)));
#if !CS8
        public static bool LessThan<T>(in ISortableItem<T> item, in T other) => item.CompareTo(other) < 0;
        public static bool LessThanOrEqualTo<T>(in ISortableItem<T> item, in T other) => item.CompareTo(other) <= 0;
        public static bool GreaterThan<T>(in ISortableItem<T> item, in T other) => item.CompareTo(other) > 0;
        public static bool GreaterThanOrEqualTo<T>(in ISortableItem<T> item, in T other) => item.CompareTo(other) >= 0;
#endif
        public static bool Overlap(in int x, in int index, in int y) => x < y ? index.Between(x, y) : index.Outside(y, x);

        /// <summary>
        /// Increments <paramref name="index"/> circularly by one step. If <paramref name="index"/> is equal to <paramref name="maxLength"/> - 1, then it is reset to zero.
        /// </summary>
        /// <param name="index">The index to increment.</param>
        /// <param name="maxLength">The total length. If an overflow is about to occur, the given index is reset to zero.</param>
        /// <seealso cref="SetIndex(int, in int, ref int)"/>
        /// <seealso cref="IncrementCircularly2(int, in int)"/>
        /// <seealso cref="DecrementCircularly(ref int, in int)"/>
        public static void IncrementCircularly(ref int index, in int maxLength) => index = (index + 1) % maxLength;
        /// <summary>
        /// Increments <paramref name="index"/> circularly by one step. If <paramref name="index"/> is equal to <paramref name="maxLength"/> - 1, then it is reset to zero.
        /// </summary>
        /// <param name="index">The index to increment.</param>
        /// <param name="maxLength">The total length. If an overflow is about to occur, the given index is reset to zero.</param>
        /// <seealso cref="GetIndex(int, in int, ref int)"/>
        /// <seealso cref="IncrementCircularly(int, in int)"/>
        /// <seealso cref="DecrementCircularly2(ref int, in int)"/>
        public static int IncrementCircularly2(int index, in int maxLength)
        {
            IncrementCircularly(ref index, maxLength);

            return index;
        }

        /// <summary>
        /// Decrements <paramref name="index"/> circularly by one step. If <paramref name="index"/> is equal to zero, then it is set to <paramref name="maxLength"/> - 1.
        /// </summary>
        /// <param name="index">The index to decrement.</param>
        /// <param name="maxLength">The total length. If a negative value is about to be computed, the given index is set to the last possible value regarding the value of the current parameter.</param>
        /// <seealso cref="SetIndex(int, in int, ref int)"/>
        /// <seealso cref="DecrementCircularly2(int, in int)"/>
        /// <seealso cref="IncrementCircularly(ref int, in int)"/>
        public static void DecrementCircularly(ref int index, in int maxLength) => index = (index == 0 ? maxLength : index) - 1;

        /// <summary>
        /// Decrements <paramref name="index"/> circularly by one step. If <paramref name="index"/> is equal to zero, then it is set to <paramref name="maxLength"/> - 1.
        /// </summary>
        /// <param name="index">The index to decrement.</param>
        /// <param name="maxLength">The total length. If a negative value is about to be computed, the given index is set to the last possible value regarding the value of the current parameter.</param>
        /// <seealso cref="DecrementCircularly(int, in int)"/>
        /// <seealso cref="IncrementCircularly2(ref int, in int)"/>
        /// <seealso cref="GetIndex(int, in int, ref int)"/>
        public static int DecrementCircularly2(int index, in int maxLength)
        {
            DecrementCircularly(ref index, maxLength);

            return index;
        }

        public static bool TryGetValue<T>(in FuncOut<T, bool> func, out object
#if CS8
            ?
#endif
            value)
        {
            if (func(out T
#if CS9
                ?
#endif
                _value))
            {
                value = _value;

                return true;
            }

            value = null;

            return false;
        }

        public static bool TryGetValue<TIn, TOut>(in FuncOut<TIn
#if CS9
            ?
#endif
            , bool> func, out TOut
#if CS9
            ?
#endif
            value) where TIn : TOut
        {
            if (func(out TIn
#if CS9
                ?
#endif
                _value))
            {
                value = _value;

                return true;
            }

            value = default;

            return false;
        }

        public static void SetOffset(int inStart, ref int outStart, int length)
        {
            string paramName;

            bool check(in int value, in string _paramName)
            {
                paramName = _paramName;

                return value.Between(0, length, true, false);
            }

            if (check(inStart, nameof(inStart)) && check(outStart, nameof(outStart)) ? (inStart = outStart - inStart) < 0 : throw new ArgumentOutOfRangeException(paramName))

                inStart += length;

            outStart = outStart < 0 ? -inStart : inStart;
        }

        public static int GetOffset(in int inStart, int outStart, in int length)
        {
            SetOffset(inStart, ref outStart, length);

            return outStart;
        }

        public static bool CheckOverflow(in int totalLength, ref int length, int offset)
        {
            if (offset < 0)
            {
                offset = -offset;

                bool result = CheckOverflow(totalLength, ref offset, length);

                length = offset;

                return result;
            }

            if (length.Between(2, totalLength - 1) && (length % totalLength) > (length = totalLength - offset))

                return true;

            length = 0;

            return false;
        }

        internal static void _ProcessData(int startIndex, in int length, in Action<int> action)
        {
            for (; startIndex < length; startIndex++)

                action(startIndex);
        }

        internal static void _ProcessData(int startIndex, in int length, in ActionIn<int> action)
        {
            for (; startIndex < length; startIndex++)

                action(startIndex);
        }

        internal static int _ProcessData(int startIndex, in int length, in Predicate<int> predicate)
        {
            for (; startIndex < length; startIndex++)

                if (predicate(startIndex))

                    return startIndex;

            return -1;
        }

        internal static int _ProcessData(int startIndex, in int length, in PredicateIn<int> predicate)
        {
            for (; startIndex < length; startIndex++)

                if (predicate(startIndex))

                    return startIndex;

            return -1;
        }

        public static void ProcessDataRef(int startIndex, ref int length, in Action<int> action) => _ProcessData(startIndex, length += startIndex, action);

        public static void ProcessDataRef(int startIndex, ref int length, in ActionIn<int> action) => _ProcessData(startIndex, length += startIndex, action);

        public static int ProcessDataRef(int startIndex, ref int length, in Predicate<int> predicate) => _ProcessData(startIndex, length += startIndex, predicate);

        public static int ProcessDataRef(int startIndex, ref int length, in PredicateIn<int> predicate) => _ProcessData(startIndex, length += startIndex, predicate);

        public static void ProcessData(in Action<int> action, int startIndex = 0, int length = -1) => _ProcessData(startIndex, startIndex + length, action);

        public static void ProcessData(in ActionIn<int> action, int startIndex = 0, int length = -1) => _ProcessData(startIndex, startIndex + length, action);

        public static int ProcessData(in Predicate<int> predicate, int startIndex = 0, int length = -1) => _ProcessData(startIndex, startIndex + length, predicate);

        public static int ProcessData(in PredicateIn<int> predicate, int startIndex = 0, int length = -1) => _ProcessData(startIndex, startIndex + length, predicate);

        /// <summary>
        /// Increments <paramref name="start"/> by a number of steps provided by the value of <paramref name="offset"/>. The given index will remain between zero and a given length.
        /// </summary>
        /// <param name="start">The index to increment.</param>
        /// <param name="totalLength">The total length supported. An offset that would result to an overflow regarding the given index will be felt back to a corresponding offset that will not result to an overflow.</param>
        /// <param name="offset">The offset that represents the number of steps for the incrementation. This value can be any value supported by the <see cref="int"/> type. Negative values mean that this method will compute a decrementation. Overflow values will be felt back to a value between zero and the given length.</param>
        /// <seealso cref="GetIndex(int, in int, ref int)"/>
        /// <seealso cref="IncrementCircularly(ref int, in int)"/>
        /// <seealso cref="DecrementCircularly(ref int, in int)"/>
        public static void SetIndex(ref long start, in long totalLength, ref long offset)
        {
            if (start.Between(0, totalLength, true, false) ? offset == 0 : throw new ArgumentOutOfRangeException(nameof(start)))

                return;

            offset %= totalLength;

            if (start == 0)
            {
                start = offset;

                return;
            }

            switch (offset)
            {
                case 1:

                    start = (start + 1) % totalLength;

                    break;

                case -1:

                    start = start == 0 ? totalLength - 1 : start - 1;

                    break;

                default:

                    if (offset > 0)
                    {
                        //long tmp = totalLength - start;

                        start = Math.IsAdditionResultInRange((ulong)start, (ulong)offset, long.MaxValue) ? (start + offset) % totalLength : unchecked(start + offset) + 1 /*offset == tmp ? 0 : offset > tmp ? (start - (totalLength - offset)) : start + offset*/;
                    }

                    else

                        start = System.Math.Abs(offset) > start ? totalLength + offset + start : start + offset;

                    break;
            }
        }

        /// <summary>
        /// Increments <paramref name="start"/> by a number of steps provided by the value of <paramref name="offset"/>. The given index will remain between zero and a given length.
        /// </summary>
        /// <param name="start">The index to increment.</param>
        /// <param name="totalLength">The total length supported. An offset that would result to an overflow regarding the given index will be felt back to a corresponding offset that will not result to an overflow.</param>
        /// <param name="offset">The offset that represents the number of steps for the incrementation. This value can be any value supported by the <see cref="int"/> type. Negative values mean that this method will compute a decrementation. Overflow values will be felt back to a value between zero and the given length.</param>
        /// <seealso cref="SetIndex(ref int, in int, ref int)"/>
        /// <seealso cref="IncrementCircularly2(int, in int)"/>
        /// <seealso cref="DecrementCircularly2(int, in int)"/>
        public static long GetIndex(long start, /*in int length,*/ in long totalLength, ref long offset)
        {
            SetIndex(ref start, /*length,*/ totalLength, ref offset);

            return start;
        }

        public static void SetIndex(ref int start, in int totalLength, ref int offset)
        {
            long _start = start;
            long _offset = offset;

            SetIndex(ref _start, totalLength, ref _offset);

            start = (int)_start;
            offset = (int)_offset;
        }
        public static int GetIndex(int start, /*in int length,*/ in int totalLength, ref int offset)
        {
            SetIndex(ref start, totalLength, ref offset);

            return start;
        }

        public static string ToString(in bool value) => ToString(value.ToByte());
        public static string ToString(in byte value) => ((char)value).ToString();
        public static string ToString(in sbyte value) => ToString(unchecked((byte)value));
        public static string ToString(in ushort value) => ((char)value).ToString();
        public static string ToString(in short value) => ToString(unchecked((ushort)value));
        private static unsafe string _ToString<T>(T value) where T : unmanaged => new string((char*)&value, 0, sizeof(T) / sizeof(char));
        public static unsafe string ToString(uint value) => _ToString(value);
        public static unsafe string ToString(ulong value) => _ToString(value);
        public static unsafe string ToString(int value) => ToString(unchecked((uint)value));
        public static unsafe string ToString(long value) => ToString(unchecked((ulong)value));

        public static void Prepend(ref Action action, Action prependWith)
        {
            Action _action = action;

            action = () =>
            {
                prependWith();

                _action();
            };
        }

        public static void RunSurrounded(in Action action, in Action surroundWith)
        {
            surroundWith();
            action();
            surroundWith();
        }
#if CS5
        private static T GetTask<T>(in T task) where T : Task => task ?? throw new ArgumentNullException(nameof(task));
        private static void StartTask<T>(in T task) where T : Task
        {
            GetTask(task).Start();
            task.Wait();
        }

        public static ConfiguredTaskAwaitable AwaitFalse(this Task task) => GetTask(task).ConfigureAwait(false);

        public static ConfiguredTaskAwaitable<T> AwaitFalse<T>(this Task<T> task) => GetTask(task).ConfigureAwait(false);

        public static void Await(this Task task) => StartTask(task);

        public static T Await<T>(this Task<T> task)
        {
            StartTask(task);

            return task.Result;
        }
#endif
        public static Predicate<Type> GetTypePredicate(Type type, in bool typeEquality) => typeEquality ?
#if !CS9
            (Predicate<Type>)
#endif
            (t => t == type) : (t => t.IsAssignableFrom(type));

        public static Predicate<Type> GetTypePredicate(in object obj, in bool typeEquality) => GetTypePredicate(obj.GetType(), typeEquality);

        public static void GetAvailableLength(in long length, in long position, ref long count)
        {
            long realLength = length - position;

            if (count > realLength)

                count = realLength;
        }

        public static void GetAvailableLength(in long length, in long position, ref int count)
        {
            long realLength = length - position;

            if (count > realLength)

                count = (int)realLength;
        }
#if CS5
        public static IEnumerable<T> GetEnumConstants<T>() where T : Enum => typeof(T).GetEnumValues().Cast<T>();
#endif
        private struct EmptyEnumerator<T> : IEnumerator<T
#if CS9
            ?
#endif
    >
        {
            public static IEnumerator<T> Instance { get; }

            public T
#if CS9
                ?
#endif
                Current => default;
            object IEnumerator.Current => Current;

            bool IEnumerator.MoveNext() => false;
            void IEnumerator.Reset() { /* Left empty. */ }
            void System.IDisposable.Dispose() { /* Left empty. */ }
        }

        public static IEnumerator<T> GetEmptyEnumerator<T>() => EmptyEnumerator<T>.Instance;

        public static IEnumerable<T
#if CS9
            ?
#endif
            > EnumerateUntil<T>(T
#if CS9
            ?
#endif
            first, Converter<T
#if CS9
            ?
#endif
            , T
#if CS9
            ?
#endif
            > nextItemProvider, Predicate<T
#if CS9
            ?
#endif
            > stop)
        {
        CONDITION:
            if (stop(first))

                yield break;

            yield return first;

            first = nextItemProvider(first);

            goto CONDITION;
        }

        public static IEnumerable<T> EnumerateUntilNull<T>(T
#if CS9
            ?
#endif
            first, Converter<T, T
#if CS9
            ?
#endif
            > nextItemProvider, Predicate<T>
#if CS8
            ?
#endif
            predicate = null) => EnumerateUntil(first, nextItemProvider, predicate == null ?
#if !CS9
                (Predicate<T>)
#endif
#if WinCopies4
                EqualsNull
#else
                CheckIfEqualsNull
#endif
                : value => value == null || predicate(value));

        public static void While(in Action action, ref bool condition)
        {
            while (condition)

                action();
        }

        public static void While(in Action action, in Func<bool> condition)
        {
            while (condition())

                action();
        }

        public static void WhileRef(in Action action, ref Func<bool> condition)
        {
            while (condition())

                action();
        }

        public static void While(in Func<bool> condition)
        {
            while (condition()) { /* Left empty. */ }
        }

        public static void WhileRef(ref Func<bool> condition)
        {
            while (condition()) { /* Left empty. */ }
        }

        public static void DoWhile(in Action action, ref bool condition)
        {
            action();

            While(action, ref condition);
        }

        public static void DoWhile(in Action action, in Func<bool> condition)
        {
            action();

            While(action, condition);
        }

        public static void DoWhileRef(in Action action, ref Func<bool> condition)
        {
            action();

            WhileRef(action, ref condition);
        }

        public static void Until(in Action action, ref bool condition)
        {
        CONDITION:
            if (condition)

                return;

            action();

            goto CONDITION;
        }

        public static void Until(in Action action, in Func<bool> condition)
        {
        CONDITION:
            if (condition())

                return;

            action();

            goto CONDITION;
        }

        public static void UntilRef<T>(in ActionRef<T> action, in PredicateRef<T> condition, ref T value)
        {
        CONDITION:
            if (condition(ref value))

                return;

            action(ref value);

            goto CONDITION;
        }

        public static void UntilRef(in Action action, ref Func<bool> condition)
        {
        CONDITION:
            if (condition())

                return;

            action();

            goto CONDITION;
        }

        public static void Until(in Func<bool> condition)
        {
        CONDITION:
            if (condition())

                return;

            goto CONDITION;
        }

        public static void UntilRef(ref Func<bool> condition)
        {
        CONDITION:
            if (condition())

                return;

            goto CONDITION;
        }

        public static void DoUntil(in Action action, ref bool condition)
        {
            action();

            Until(action, ref condition);
        }

        public static void DoUntil(in Action action, in Func<bool> condition)
        {
            action();

            Until(action, condition);
        }

        public static void DoUntilRef<T>(in ActionRef<T> action, in PredicateRef<T> condition, ref T value)
        {
        CONDITION:
            action(ref value);

            if (condition(ref value))

                return;

            goto CONDITION;
        }

        public static void DoUntilRef(in Action action, ref Func<bool> condition)
        {
            action();

            UntilRef(action, ref condition);
        }

        public static void WhileRef(ref Action action, ref bool condition)
        {
            while (condition)

                action();
        }

        public static void WhileRef2(ref Action action, in Func<bool> condition)
        {
            while (condition())

                action();
        }

        public static void WhileRef3(ref Action action, ref Func<bool> condition)
        {
            while (condition())

                action();
        }

        public static void DoWhileRef(ref Action action, ref bool condition)
        {
            action();

            While(action, ref condition);
        }

        public static void DoWhileRef2(ref Action action, in Func<bool> condition)
        {
            action();

            While(action, condition);
        }

        public static void DoWhileRef3(ref Action action, ref Func<bool> condition)
        {
            action();

            WhileRef(action, ref condition);
        }

        public static void UntilRef(ref Action action, ref bool condition)
        {
        CONDITION:
            if (condition)

                return;

            action();

            goto CONDITION;
        }

        public static void UntilRef2(ref Action action, in Func<bool> condition)
        {
        CONDITION:
            if (condition())

                return;

            action();

            goto CONDITION;
        }

        public static void UntilRef3(ref Action action, ref Func<bool> condition)
        {
        CONDITION:
            if (condition())

                return;

            action();

            goto CONDITION;
        }

        public static void DoUntilRef(ref Action action, ref bool condition)
        {
            action();

            Until(action, ref condition);
        }

        public static void DoUntilRef2(ref Action action, in Func<bool> condition)
        {
            action();

            Until(action, condition);
        }

        public static void DoUntilRef3(ref Action action, ref Func<bool> condition)
        {
            action();

            UntilRef(action, ref condition);
        }

        public static void ForUntil<T>(in Action
#if CS8
            ?
#endif
            preAction, in PredicateRef<T> func, Action updater, ActionRef<T> action, ref T value)
        {
            preAction?.Invoke();

            UntilRef((ref T _value) => { action(ref _value); updater(); }, func, ref value);
        }

        public static void DoForUntil<T>(in Action
#if CS8
            ?
#endif
            preAction, in PredicateRef<T> func, Action updater, ActionRef<T> action, ref T value)
        {
            preAction?.Invoke();

            DoUntilRef((ref T _value) => { action(ref _value); updater(); }, func, ref value);
        }
#if !WinCopies4
        [Obsolete("Use methods from WinCopies.Collections package instead.")]
        public static IEnumerable<T> EnumerateRecursively<T>(T item, Converter<T, IEnumerable<T>> func)
        {
            IEnumerable<T> enumerate(T _item)
            {
                yield return _item;

                foreach (T __item in func(_item))

                    foreach (T ___item in enumerate(__item))

                        yield return ___item;
            }

            foreach (T _item in enumerate(item))

                yield return _item;
        }
#endif
#if CS8
        public static long? GetHttpFileSize(in string url, in System.Net.Http.HttpClient client)
        {
            using System.Net.Http.HttpResponseMessage resp = client.
#if CS9
                Send
#else
                SendAsync
#endif
                (new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Head, url))
#if !CS9
                .GetAwaiter().GetResult()
#endif
                ;

            return resp.Content.Headers.ContentLength;
        }
#elif !CS5
        public static TAttribute GetCustomAttribute<TType, TAttribute>(in bool inherit) => (TAttribute)typeof(TType).GetCustomAttributes(typeof(TAttribute), inherit).TryGet(0);
#endif
        public static T CastIfNotNull<T>(in object value, out bool succeeded) where T : struct
        {
            if (value == null)
            {
                succeeded = false;

                return default;
            }

            succeeded = true;

            return (T)value;
        }

        public static string GetAssemblyName() => (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetAssemblyName();

        public static void ToggleBitFromLeft(ref byte b, byte pos) => b = b.ToggleBitFromLeft(pos);

        public static void SetBitFromLeft(ref byte b, byte pos) => b = b.SetBitFromLeft(pos);

        public static void UnsetBitFromLeft(ref byte b, byte pos) => b = b.UnsetBitFromLeft(pos);

        public static void SetBitFromLeft(ref byte b, in byte pos, in bool value) => b = b.SetBitFromLeft(pos, value);

        public static void ToggleBit(ref byte b, byte pos) => b = b.ToggleBit(pos);

        public static void SetBit(ref byte b, byte pos) => b = b.SetBit(pos);

        public static void UnsetBit(ref byte b, byte pos) => b = b.UnsetBit(pos);

        public static void SetBit(ref byte b, in byte pos, in bool value) => b = b.SetBit(pos, value);

        public static void ToggleBitFromLeft(ref ushort b, byte pos) => b = b.ToggleBitFromLeft(pos);

        public static void SetBitFromLeft(ref ushort b, byte pos) => b = b.SetBitFromLeft(pos);

        public static void UnsetBitFromLeft(ref ushort b, byte pos) => b = b.UnsetBitFromLeft(pos);

        public static void SetBitFromLeft(ref ushort b, in byte pos, in bool value) => b = b.SetBitFromLeft(pos, value);

        public static void ToggleBit(ref ushort b, byte pos) => b = b.ToggleBit(pos);

        public static void SetBit(ref ushort b, byte pos) => b = b.SetBit(pos);

        public static void UnsetBit(ref ushort b, byte pos) => b = b.UnsetBit(pos);

        public static void SetBit(ref ushort b, in byte pos, in bool value) => b = b.SetBit(pos, value);

        public static T Update<T>(ref T value, in T defaultValue)
        {
#if !CS8
            if (value == null)
#endif
            value
#if CS8
??=
#else
                                                =
#endif
            defaultValue;

            return value;
        }

        /// <summary>
        /// Creates a new instance of a given type if <paramref name="value"/> is null and update <paramref name="value"/> with that value; then returns the value of <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type from which to create a new value, if applicable, and of the value to return.</typeparam>
        /// <param name="value">The reference of the variable to read from and to write to the new value, if applicable.</param>
        /// <param name="func">The <see cref="Func{T}"/> that provides a new value to assign to <paramref name="value"/>.</param>
        /// <returns>A new value from <typeparamref name="T"/> if <paramref name="value"/> is null; otherwise the value of <paramref name="value"/>.</returns>
        public static T InitializeStruct<T>(ref T? value, in Func<T> func) where T : struct
        {
#if !CS8
            if (value == null)
#endif
            value
#if CS8
??=
#else
                                                                 =
#endif
            func();

            return value.Value;
        }

        public static void SetOrThrowIfNull<T>(ref T value, in T newValue, in string paramName) => value = newValue
#if CS8
        ??
#else
        == null ?
#endif
        throw GetArgumentNullException(paramName)
#if !CS8
            : newValue
#endif
            ;

        public static IEnumerable<U> GetArrayEnumerable<T, U>(IEnumerable<U> arrays, IEnumerable<U> right) where U : IEnumerable<T>
        {
            foreach (U array in arrays)

                yield return array;

            foreach (U array in right)

                yield return array;
        }

        public static IEnumerable<U> GetArrayEnumerable<T, U>(in IEnumerable<U> arrays, params U[] right) where U : IEnumerable<T> => GetArrayEnumerable<T, U>(arrays, right.AsEnumerable());

        public static IEnumerable<U> GetArrayEnumerable<T, U>(U left, IEnumerable<U> arrays, IEnumerable<U> right) where U : IEnumerable<T>
        {
            yield return left;

            foreach (U array in GetArrayEnumerable<T, U>(arrays, right))

                yield return array;
        }

        public static IEnumerable<U> GetArrayEnumerable<T, U>(in U left, in IEnumerable<U> arrays, params U[] right) where U : IEnumerable<T> => GetArrayEnumerable<T, U>(left, arrays, right.AsEnumerable());

        public static IEnumerable<U> GetArrayEnumerable<T, U>(IEnumerable<U> left, IEnumerable<U> arrays, IEnumerable<U> right) where U : IEnumerable<T>
        {
            foreach (U array in left)

                yield return array;

            foreach (U array in GetArrayEnumerable<T, U>(arrays, right))

                yield return array;
        }

        public static IEnumerable<U> GetArrayEnumerable<T, U>(T left, IEnumerable<U> arrays, T right, Converter<T, U> converter) where U : IEnumerable<T>
        {
            yield return converter(left);

            foreach (U array in arrays)

                yield return array;

            yield return converter(right);
        }

        public static IEnumerable<U> GetArrayEnumerable<T, U>(in T left, in T right, Converter<T, U> converter, params U[] arrays) where U : IEnumerable<T> => GetArrayEnumerable<T, U>(left, arrays, right, converter);

        public static string Format(string format, params object[] args) => string.Format(CultureInfo.CurrentCulture, format, args);

        public static ulong GetLength<T>(in Converter<T, ulong> converter,
#if CS5
            IEnumerable<
#else
            params
#endif
            T
#if CS5
            >
#else
            []
#endif
            arrays)
        {
            ulong length = 0;

            foreach (T array in arrays)

                length += converter(array);

            return length;
        }

        public static ulong GetLength<T>(Converter<T, int> converter,
#if CS5
            IEnumerable<
#else
            params
#endif
            T
#if CS5
            >
#else
            []
#endif
            arrays) => GetLength(value => (ulong)converter(value), arrays);
#if CS5
        public static ulong GetLength<T>(in Converter<T, ulong> converter, params T[] arrays) => GetLength(converter, arrays.AsEnumerable());

        public static ulong GetLength<T>(in Converter<T, int> converter, params T[] arrays) => GetLength(converter, arrays.AsEnumerable());

        public static ulong GetLength<T>(params IReadOnlyList<T>[] arrays) => GetLength(arrays.AsEnumerable());
#endif
        public static ulong GetLength<T>(
#if CS5
            IEnumerable<
#else
            params
#endif
#if CS5
            IReadOnlyList<
#endif
            T
#if CS5
            >>
#else
            [][]
#endif
            arrays) => GetLength(GetULongLength, arrays);

        public static T GetValue<T>(in T value, in Predicate<T> predicate, in T defaultValue) => predicate(value) ? value : defaultValue;

        public static T GetValue<T>(in Func<T> func, in Predicate<T> predicate, in T defaultValue) => GetValue(func(), predicate, defaultValue);

        public static T GetValueF<T>(in T value, in Predicate<T> predicate, in Func<T> defaultValue) => predicate(value) ? value : defaultValue();

        public static T GetValueF<T>(in Func<T> func, in Predicate<T> predicate, in Func<T> defaultValue) => GetValueF(func(), predicate, defaultValue);

        public static T GetValue<T>(in T value, in Predicate<T> predicate, Func<Exception> func) => GetValue(value, predicate, () => throw func());

        public static T GetValue<T>(in Func<T> func, in Predicate<T> predicate, Func<Exception> exceptionFunc) => GetValue(func, predicate, () => throw exceptionFunc());

        public static T
#if CS9
            ?
#endif
            GetValue<T>(in T value, in Predicate<T> predicate) => predicate(value) ? value : default;

        public static T
#if CS9
            ?
#endif
            GetValue<T>(in Func<T> func, in Predicate<T> predicate) => GetValue(func(), predicate);

        public static Converter<TIn, TOut> GetConverter<TIn, TOut>(in bool condition, in Converter<TIn, TOut> ifTrue, in Converter<TIn, TOut> ifFalse) => condition ? ifTrue : ifFalse;

        public static TOut GetValue<TIn, TOut>(in TIn value, in bool condition, in Converter<TIn, TOut> ifTrue, in Converter<TIn, TOut> ifFalse) => GetConverter(condition, ifTrue, ifFalse)(value);

        public static T GetValue<T>(in bool condition, in Func<T> ifTrue, in Func<T> ifFalse) => (condition ? ifTrue : ifFalse)();

        public static T GetValue<T>(in Func<bool> condition, in Func<T> ifTrue, in Func<T> ifFalse) => GetValue(condition(), ifTrue, ifFalse);

        public static TOut GetValue<TIn, TOut>(in Func<TIn> func, in Predicate<TIn> predicate, in Converter<TIn, TOut> ifTrue, in Converter<TIn, TOut> ifFalse)
        {
            TIn value = func();

            return (predicate(value) ? ifTrue : ifFalse)(value);
        }

        public static T GetValue<T>(in Func<T?> func, T defaultValue) where T : struct => GetValue(func, HasValue, Delegates.GetValue, value => defaultValue);

        public static T GetValueF<T>(in Func<T?> func, Func<T> defaultValue) where T : struct => GetValue(func, HasValue, Delegates.GetValue, value => defaultValue());

        public static T GetValue<T>(in Func<T?> func) where T : struct => GetValue(func, default);

        public static TOut GetValue<TIn, TOut>(in Func<TIn> func, in Predicate<TIn> predicate, in Converter<TIn, TOut> converter, in TOut defaultValue)
        {
            TIn value = func();

            return predicate(value) ? converter(value) : defaultValue;
        }

        public static TOut GetValueF<TIn, TOut>(in Func<TIn> func, in Predicate<TIn> predicate, in Converter<TIn, TOut> converter, in Func<TOut> defaultValue)
        {
            TIn value = func();

            return predicate(value) ? converter(value) : defaultValue();
        }

        // TODO: should also exists as a foreach version.

        public static int While<T>(in
#if CS5
            IReadOnlyList<
#endif
            Func<T>
#if CS5
            >
#else
            []
#endif
            funcs, in Predicate<T> predicate, out T value)
        {
            int i = 0;

            for (; predicate(value = funcs[i]()) && i < funcs.
#if CS5
                Count
#else
                Length
#endif
                ; i++)
            {
                // Left empty.
            }

            return i;
        }

        public static TOut While<TIn, TOut>(
#if CS5
            IReadOnlyList<
#endif
            Func<TIn>
#if CS5
            >
#else
            []
#endif
            funcs, Predicate<TIn> predicate, Func<TIn, TOut> onError, out int result, out TIn value, out bool error)
        {
            TIn _value = default;

            TOut _result = PerformAction((out TIn __value) => While(funcs, predicate, out __value), __value => __value < funcs.
#if CS5
                   Count
#else
                Length
#endif
                   , __value => onError(_value = __value), out result, out error);

            value = _value;

            return _result;
        }

        public static int While<T>(
#if CS5
            IReadOnlyList<
#endif
            Func<T>
#if CS5
            >
#else
            []
#endif
            funcs, Predicate<T> predicate, Action<T> onError, out T value, out bool error)
        {
            _ = While<T, object
#if CS8
                ?
#endif
                >(funcs, predicate, _value =>
            {
                onError(_value);

                return null;
            },
            out int result, out value, out error);

            return result;
        }

        public static T While<T>(
#if CS5
            IReadOnlyList<
#endif
            Func<T>
#if CS5
            >
#else
            []
#endif
            funcs, Predicate<T> predicate, Func<T> onError, out int value, out bool error) => While(funcs, predicate, _value => onError(), out value, out _, out error);

        public static int While<T>(
#if CS5
            IReadOnlyList<
#endif
            Func<T>
#if CS5
            >
#else
            []
#endif
            funcs, Predicate<T> predicate, in Action onError, out T value, out bool error) => While(funcs, predicate, onError.ToParameterizedAction<T>(), out value, out error);

        public static int While<T>(in
#if CS5
            IReadOnlyList<
#endif
            Func<T>
#if CS5
            >
#else
            []
#endif
            funcs, in Predicate<T> predicate) => While(funcs, predicate, out _);

        public static TOut While<TIn, TOut>(
#if CS5
            IReadOnlyList<
#endif
            Func<TIn>
#if CS5
            >
#else
            []
#endif
            funcs, Predicate<TIn> predicate, Func<TIn, TOut> onError) => While(funcs, predicate, onError, out _, out _, out _);

        public static int While<T>(
#if CS5
            IReadOnlyList<
#endif
            Func<T>
#if CS5
            >
#else
            []
#endif
            funcs, Predicate<T> predicate, Action<T> onError) => While(funcs, predicate, onError, out _, out _);

        public static T While<T>(
#if CS5
            IReadOnlyList<
#endif
            Func<T>
#if CS5
            >
#else
            []
#endif
            funcs, Predicate<T> predicate, Func<T> onError) => While(funcs, predicate, onError, out _, out _);

        public static int While<T>(
#if CS5
            IReadOnlyList<
#endif
            Func<T>
#if CS5
            >
#else
            []
#endif
            funcs, Predicate<T> predicate, in Action onError) => While(funcs, predicate, onError, out _, out _);
#if CS5
        public static int While<T>(in Predicate<T> predicate, out T value, params Func<T>[] funcs) => While(funcs, predicate, out value);

        public static TOut While<TIn, TOut>(in Predicate<TIn> predicate, in Func<TIn, TOut> onError, out int result, out TIn value, out bool error, params Func<TIn>[] funcs) => While(funcs, predicate, onError, out result, out value, out error);

        public static int While<T>(in Predicate<T> predicate, in Action<T> onError, out T value, out bool error, params Func<T>[] funcs) => While(funcs, predicate, onError, out value, out error);

        public static int While<T>(in Predicate<T> predicate, in Action onError, out T value, out bool error, params Func<T>[] funcs) => While(funcs, predicate, onError, out value, out error);

        public static int While<T>(in Predicate<T> predicate, params Func<T>[] funcs) => While(funcs, predicate);

        public static TOut While<TIn, TOut>(in Predicate<TIn> predicate, in Func<TIn, TOut> onError, params Func<TIn>[] funcs) => While(funcs, predicate, onError);

        public static int While<T>(in Predicate<T> predicate, in Action<T> onError, params Func<T>[] funcs) => While(funcs, predicate, onError);

        public static int While<T>(in Predicate<T> predicate, in Action onError, params Func<T>[] funcs) => While(funcs, predicate, onError);

        public static T While<T>(in Predicate<T> predicate, in Func<T> onError, out int value, out bool error, params Func<T>[] funcs) => While(funcs, predicate, onError, out value, out error);

        public static T While<T>(in Predicate<T> predicate, in Func<T> onError, params Func<T>[] funcs) => While(funcs, predicate, onError);
#endif
        public static int While(in
#if CS5
    IReadOnlyList<
#endif
    Func<bool>
#if CS5
    >
#else
            []
#endif
    funcs) => While(funcs, Self);

#if CS5
        public static int While(params Func<bool>[] funcs) => While(funcs.AsFromType<IReadOnlyList<Func<bool>>>());
#endif

        public static List<T> GetList<T>(params
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            []
            arrays)
        {
            int length = 0;

            foreach (
#if CS5
                IReadOnlyList<T>
#else
                T[]
#endif
                array in arrays)

                length += array.
#if CS5
                    Count
#else
                    Length
#endif
                    ;

            var items = new List<T>(length);

            foreach (
#if CS5
                IReadOnlyList<T>
#else
                T[]
#endif
                array in arrays)

                foreach (T item in array)

                    items.Add(item);

            return items;
        }

        public static void Using<T>(in Func<T> func, in Action<T> action) where T : System.IDisposable
        {
            ThrowIfNull(func, nameof(func));
            ThrowIfNull(action, nameof(action));

            using
#if !CS8
                (
#endif
                T obj = func()
#if CS8
                ;
#else
                )
#endif
            action(obj);
        }

        public static void UsingIn<T>(in Func<T> func, in ActionIn<T> action) where T : System.IDisposable
        {
            ThrowIfNull(func, nameof(func));
            ThrowIfNull(action, nameof(action));

            using
#if !CS8
                (
#endif
                T obj = func()
#if CS8
                ;
#else
                )
#endif
            action(obj);
        }

        public static TOut Using2<TIn, TOut>(in Func<TIn> func, Func<TIn, TOut> action) where TIn : System.IDisposable
        {
            TOut result = default;

            Using(func, (obj) => result = action(obj));

            return result;
        }

        public static TOut UsingIn2<TIn, TOut>(in Func<TIn> func, FuncIn<TIn, TOut> action) where TIn : System.IDisposable
        {
            TOut result = default;

            UsingIn(func, (in TIn obj) => result = action(obj));

            return result;
        }
#if CS8
        public static ReadOnlySpan<T> GetReadOnlySpan<T>(T[]? array, int start) => array == null ? new
#if !CS9
            ReadOnlySpan<T>
#endif
            () : new
#if !CS9
            ReadOnlySpan<T>
#endif
            (array, start, array.Length - start);

        public static ReadOnlySpan<T> GetReadOnlySpanL<T>(T[]? array, int length) => array == null ? new
#if !CS9
            ReadOnlySpan<T>
#endif
            () : new
#if !CS9
            ReadOnlySpan<T>
#endif
            (array, length, length > array.Length ? array.Length : length);
#endif
        private static void _Dispose<T>(ref T obj) where T : System.IDisposable
        {
            obj.Dispose();
            obj = default;
        }

        public static void Dispose<T>(ref T obj) where T : System.IDisposable
        {
            ThrowIfNull(obj, nameof(obj));

            _Dispose(ref obj);
        }

        public static bool TryDisposeNullable<T>(ref T? obj) where T : struct, System.IDisposable
        {
            ThrowIfNull(obj, nameof(obj));

            if (obj.HasValue)
            {
                obj.Value.Dispose();
                obj = null;

                return true;
            }

            return false;
        }

        public static void DisposeNullable<T>(ref T? obj) where T : struct, System.IDisposable
        {
            if (!TryDisposeNullable(ref obj))

                throw GetArgumentNullException(nameof(obj));
        }

        private static bool TryDispose<T>(ref T
#if CS9
            ?
#endif
            obj, in PredicateIn<T
#if CS9
            ?
#endif
            > func) where T : System.IDisposable
        {
            if (func(obj))

                return false;

            _Dispose(ref obj);

            return true;
        }

        public static bool TryDispose<T>(ref T
#if CS9
            ?
#endif
            obj) where T : System.IDisposable => TryDispose(ref obj,
#if WinCopies4
            EqualsNullIn
#else
            CheckIfEqualsNullIn
#endif
                );

        public static bool TryDispose2<T>(ref T
#if CS9
            ?
#endif
            obj) where T : DotNetFix.IDisposable => TryDispose(ref obj, (in T
#if CS9
                ?
#endif
                _obj) => _obj == null || _obj.IsDisposed);
#if !CS9
        public static Type GetUnderlyingType<T>() where T : Enum => Enum.GetUnderlyingType(typeof(T));

        public static string[] GetNames<T>() where T : Enum => Enum.GetNames(typeof(T));
#endif
        public static Predicate GetPredicate(object obj) => _obj => Delegates.CompareEquality(obj, _obj);

        public static Predicate<object> GetObjectPredicate(object obj) => _obj => Delegates.CompareEquality(obj, _obj);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public static ConstructorInfo
#if CS8
                ?
#endif
                TryGetConstructor<T>(params Type[] types) => typeof(T).TryGetConstructor(types);

        public static ConstructorInfo
#if CS8
                ?
#endif
                GetConstructor<T>(params Type[] types) => typeof(T).AssertGetConstructor(types);

        /*public static System.Collections.Generic.IEnumerable<PropertyInfo> GetAllProperties<T, U>(in bool include = true) where T : U => typeof(T)._GetAllProperties(typeof(U), include);

        public static System.Collections.Generic.IEnumerable<PropertyInfo> GetAllProperties<T>(in Type u, in bool include = true) => typeof(T).GetAllProperties(u, nameof(T), nameof(u), include);*/

        public static bool IsSigned(object value) => (value ?? throw GetArgumentNullException(nameof(value))).Is(true, typeof(int), typeof(short), typeof(long), typeof(sbyte))
                || (value.Is(true, typeof(uint), typeof(ushort), typeof(ulong), typeof(byte))
                ? false
                : throw new ArgumentException("The given value is neither from a signed nor a unsigned type."));

        public static T[] GetArray<T>(params T[] items) => items;
#if CS5
        public static IReadOnlyList<T> AsReadOnlyList<T>(params T[] values) => values;
#endif
        public static IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(
#if CS5
            in IReadOnlyList<
#else
            params
#endif
                KeyValuePair<TKey, TValue>
#if CS5
                >
#else
                []
#endif
            items)
        {
            int count = items.
#if CS5
                Count
#else
                Length
#endif
                ;

            IDictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>(count);

            KeyValuePair<TKey, TValue> item;

            for (int i = 0; i < count; i++)

                dic.Add((item = items[i]).Key, item.Value);

            return dic;
        }
#if CS5
        public static IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(params KeyValuePair<TKey, TValue>[] items) => GetDictionary(items.AsReadOnlyList());
#endif
        public static IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(in TKey key, in TValue value) => new Dictionary<TKey, TValue>(1) { { key, value } };

        public static bool PerformAction<T>(T? value, Action<T> action) where T : struct => PerformActionIf(value.HasValue, () => action(value.Value));

        public static bool PerformActionIn<T>(T? value, ActionIn<T> action) where T : struct => PerformActionIf(value.HasValue, () => action(value.Value));

        public static TOut
#if CS9
            ?
#endif
            PerformAction<TIn, TOut>(TIn? value, Func<TIn, TOut> func, out bool result) where TIn : struct => PerformActionIf<TIn, TOut>(value.HasValue, () => func(value.Value), out result);

        public static TOut
#if CS9
            ?
#endif
            PerformActionIn<TIn, TOut>(TIn? value, FuncIn<TIn, TOut> func, out bool result) where TIn : struct => PerformActionIf<TIn, TOut>(value.HasValue, () => func(value.Value), out result);

        public static T PerformAction<T>(in Func<T> func, in Action<T> action)
        {
            T result = func();

            action(result);

            return result;
        }

        public static T1 PerformAction<T1, T2>(FuncOut<T1, T2> func)
        {
            _ = func(out T1 value);

            return value;
        }

        public static T2 PerformAction<T1, T2>(FuncOut<T1, T2> func, Action<T1, T2> action)
        {
            T1
#if CS9
                ?
#endif
                value = default;

            return PerformAction(() => func(out value), result => action(value, result));
        }

        public static T PerformAction<T>(in Func<T> func, in Predicate<T> predicate, in Action<T> action) => PerformAction(func, action.ToConditionalAction(predicate));

        public static T2 PerformAction<T1, T2>(in FuncOut<T1, T2> func, in Predicate<T2> predicate, in Action<T1> action) => PerformAction(func, action.ToConditionalAction2(predicate));

        public static T2 PerformAction<T1, T2, TResult>(in FuncOut<T1, TResult> func, Predicate<TResult> predicate, Func<T1, T2> action, out TResult result, out bool predicateResult)
        {
            bool _predicateResult = false;
            T2 _result = default;

            result = PerformAction(func, value => _predicateResult = predicate(value), value => _result = action(value));

            return (predicateResult = _predicateResult) ? _result : default;
        }

        public static void PerformAction<TIn, TOut>(in TOut parameter, in string paramName, in Action<TIn> action) => action(parameter is TIn _parameter ? _parameter : throw GetArgumentException(paramName));

        public static bool PerformActionIf(in bool condition, in Action action)
        {
            if (condition)
            {
                action();

                return true;
            }

            return false;
        }

        public static TOut
#if CS9
            ?
#endif
            PerformActionIf<TIn, TOut>(in bool condition, in Func<TOut> func, out bool result) => (result = condition) ? func() : default;

        public static bool PerformActionIfNull(in object
#if CS8
                    ?
#endif
                    param, in Action action) => PerformActionIf(param == null, action);

        public static bool PerformActionIfIsNull(in object
#if CS8
                    ?
#endif
                    param, in Action action) => PerformActionIf(param is null, action);

        public static bool PerformActionIfNotNull(in object
#if CS8
                    ?
#endif
                    param, in Action action) => PerformActionIf(param != null, action);

        public static bool PerformActionIfIsNotNull(in object
#if CS8
                    ?
#endif
                    param, in Action action) => PerformActionIf(
#if !CS9
            !(
#endif
            param is
#if CS9
            not
#endif
            null
#if !CS9
            )
#endif
            , action);

        public static bool PerformActionIf(in Func<bool> func, in Action action) => PerformActionIf(func(), action);

        public static bool PerformActionIfNull(in Func<object
#if CS8
                    ?
#endif
                    > func, in Action action) => PerformActionIf(func() == null, action);

        public static bool PerformActionIfIsNull(in Func<object
#if CS8
                    ?
#endif
                    > func, in Action action) => PerformActionIf(func() is null, action);

        public static bool PerformActionIfNotNull(in Func<object
#if CS8
                    ?
#endif
                    > func, in Action action) => PerformActionIf(func() != null, action);

        public static bool PerformActionIfIsNotNull(in Func<object
#if CS8
                    ?
#endif
                    > func, in Action action) => PerformActionIf(
#if !CS9
            !(
#endif
            func() is
#if CS9
            not
#endif
            null
#if !CS9
            )
#endif
            , action);

        public static bool PerformActionIf<T>(in T param, in bool condition, Action<T> action) => PerformActionIf(condition, GetAction(param, action));

        public static bool PerformActionIf<T>(in T param, Func<bool> func, in Action<T> action) => PerformActionIf(param, func(), action);

        public static bool PerformActionIf<T>(T param, Predicate<T> predicate, Action action) => PerformActionIf(() => predicate(param), action);

        public static bool PerformActionIf<T>(T param, Predicate<T> predicate, Action<T> action) => PerformActionIf(param, predicate, GetAction(param, action));

        public static bool PerformActionIfNull<T>(in T
#if CS9
                    ?
#endif
                    param, in Action<T> action) => PerformActionIf(param, param == null, action);

        public static bool PerformActionIfNull<T>(in Func<T
#if CS9
                    ?
#endif
                    > func, in Action<T> action) => PerformActionIf(func(), param => param == null, action);

        public static bool PerformActionIfNotNull<T>(in T
#if CS9
                    ?
#endif
                    param, in Action<T> action) => PerformActionIf(param, param != null, action);

        public static bool PerformActionIfNotNull<T>(in Func<T
#if CS9
                    ?
#endif
                    > func, in Action<T> action) => PerformActionIf(func(), param => param != null, action);
#if CS8
        public static bool PerformActionIfIsNull<T>(in T param, in Action<T> action) => PerformActionIf(param, param is null, action);

        public static bool PerformActionIfIsNull<T>(in Func<T> func, in Action<T> action) => PerformActionIf(func(), param => param is null, action);

        public static bool PerformActionIfIsNotNull<T>(in T param, in Action<T> action) => PerformActionIf(param,
#if !CS9
            !(
#endif
            param is
#if CS9
            not
#endif
            null
#if !CS9
            )
#endif
            , action);

        public static bool PerformActionIfIsNotNull<T>(in Func<T> func, in Action<T> action) => PerformActionIf(func(), param =>
#if !CS9
            !(
#endif
            param is
#if CS9
            not
#endif
            null
#if !CS9
            )
#endif
            , action);
#endif
        public static bool PerformActionIfNotValidated<TIn, TOut>(TIn
#if CS9
            ?
#endif
    param, Predicate<TIn
#if CS9
            ?
#endif
    > predicate, Converter<TIn
#if CS9
            ?
#endif
    , TOut> converter, out TOut
#if CS9
            ?
#endif
    result)
        {
            if (predicate(param))
            {
                result = default;

                return false;
            }

            result = converter(param);

            return true;
        }

        public static TOut
#if CS9
            ?
#endif
            PerformActionIfNotValidated<TIn, TOut>(TIn param, Predicate<TIn
#if CS9
            ?
#endif
            > predicate, Converter<TIn
#if CS9
            ?
#endif
            , TOut> converter) => PerformActionIfNotValidated(param, predicate, converter, out TOut
#if CS9
                ?
#endif
                result) ? result : default;

        public static bool PerformActionIfNotNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> converter, out TOut
#if CS9
            ?
#endif
            result) => PerformActionIfNotValidated(param,
#if WinCopies4
            EqualsNull
#else
            CheckIfEqualsNull
#endif
            , converter, out result);

        public static TOut PerformActionIfNotNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> converter) => PerformActionIfNotNull(param, converter, out TOut result) ? result : default;

        public static bool PerformActionIf<TIn, TOut>(TIn param, Predicate<TIn> predicate, Converter<TIn, TOut> converter, out TOut
#if CS9
            ?
#endif
            result)
        {
            if (predicate(param))
            {
                result = converter(param);

                return true;
            }

            result = default;

            return false;
        }

        public static TOut
#if CS9
            ?
#endif
            PerformActionIf<TIn, TOut>(TIn param, Predicate<TIn> predicate, Converter<TIn, TOut> converter) => PerformActionIf(param, predicate, converter, out TOut result) ? result : default;

        public static bool PerformActionIfNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> converter, out TOut result) => PerformActionIf(param,
#if WinCopies4
            EqualsNull
#else
            CheckIfEqualsNull
#endif
            , converter, out result);

        public static TOut
#if CS9
            ?
#endif
            PerformActionIfNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> converter) => PerformActionIfNull(param, converter, out TOut result) ? result : default;

        public static bool PerformActionIf<TIn, TOut>(TIn param, Predicate<TIn> predicate, Converter<TIn, TOut> ifTrue, Converter<TIn, TOut> ifFalse, out TOut result)
        {
            if (predicate(param))
            {
                result = ifTrue(param);

                return true;
            }

            result = ifFalse(param);

            return false;
        }

        public static TOut
#if CS9
            ?
#endif
            PerformActionIf<TIn, TOut>(TIn param, Predicate<TIn> predicate, Converter<TIn, TOut> ifTrue, Converter<TIn, TOut> ifFalse) => PerformActionIf(param, predicate, ifTrue, ifFalse, out TOut result) ? result : default;

        public static bool PerformActionIfNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> ifTrue, Converter<TIn, TOut> ifFalse, out TOut result) => PerformActionIf(param,
#if WinCopies4
            EqualsNull
#else
            CheckIfEqualsNull
#endif
            , ifTrue, ifFalse, out result);

        public static TOut
#if CS9
            ?
#endif
            PerformActionIfNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> ifTrue, Converter<TIn, TOut> ifFalse) => PerformActionIfNull(param, ifTrue, ifFalse, out TOut result) ? result : default;

        public static bool PerformActionIfNotNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> ifTrue, Converter<TIn, TOut> ifFalse, out TOut result) => PerformActionIf(param,
#if WinCopies4
            EqualsNull
#else
            CheckIfEqualsNull
#endif
            , ifFalse, ifTrue, out result);

        public static TOut
#if CS9
            ?
#endif
            PerformActionIfNotNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> ifTrue, Converter<TIn, TOut> ifFalse) => PerformActionIfNotNull(param, ifTrue, ifFalse, out TOut result) ? result : default;

        public static bool PerformActionIf<TIn, TOut>(TIn param, Predicate<TIn> predicate, TOut ifTrue, Converter<TIn, TOut> ifFalse, out TOut result)
        {
            if (predicate(param))
            {
                result = ifTrue;

                return true;
            }

            result = ifFalse(param);

            return false;
        }

        public static bool PerformActionIfNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, TOut ifTrue, Converter<TIn, TOut> ifFalse, out TOut result) => PerformActionIf(param,
#if WinCopies4
            EqualsNull
#else
            CheckIfEqualsNull
#endif
            , ifTrue, ifFalse, out result);

        public static TOut PerformActionIfNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, TOut ifTrue, Converter<TIn, TOut> ifFalse) => PerformActionIfNull(param, ifTrue, ifFalse, out TOut result) ? result : default;

        public static bool PerformActionIfNotNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, TOut ifTrue, Converter<TIn, TOut> ifFalse, out TOut result) => PerformActionIf(param,
#if WinCopies4
            EqualsNull
#else
            CheckIfEqualsNull
#endif
            , ifFalse, ifTrue, out result);

        public static TOut
#if CS9
            ?
#endif
            PerformActionIfNotNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, TOut ifTrue, Converter<TIn, TOut> ifFalse) => PerformActionIfNotNull(param, ifTrue, ifFalse, out TOut result) ? result : default;

        public static bool PerformActionIf<TIn, TOut>(TIn param, Predicate<TIn> predicate, Converter<TIn, TOut> ifTrue, TOut ifFalse, out TOut result)
        {
            if (predicate(param))
            {
                result = ifTrue(param);

                return true;
            }

            result = ifFalse;

            return false;
        }

        public static bool PerformActionIfNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> ifTrue, TOut ifFalse, out TOut result) => PerformActionIf(param,
#if WinCopies4
            EqualsNull
#else
            CheckIfEqualsNull
#endif
            , ifTrue, ifFalse, out result);

        public static TOut
#if CS9
            ?
#endif
            PerformActionIfNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> ifTrue, TOut ifFalse) => PerformActionIfNull(param, ifTrue, ifFalse, out TOut result) ? result : default;

        public static bool PerformActionIfNotNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> ifTrue, TOut ifFalse, out TOut result) => PerformActionIf(param,
#if WinCopies4
            EqualsNull
#else
            CheckIfEqualsNull
#endif
            , ifFalse, ifTrue, out result);

        public static TOut
#if CS9
            ?
#endif
            PerformActionIfNotNull<TIn, TOut>(TIn
#if CS9
            ?
#endif
            param, Converter<TIn, TOut> ifTrue, TOut ifFalse) => PerformActionIfNotNull(param, ifTrue, ifFalse, out TOut result) ? result : default;

#if CS6
        public const bool LOOP = true;
        public const bool CANCEL = !LOOP;

        public static async Task<bool> TryWaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var t = new System.Threading.CancellationToken();

            var waitTask = Task.Run(async () =>
            {
                while (condition()) await Task.Delay(frequency).ConfigureAwait(false);
            }, t);

            if (timeout >= 0)
            {
                if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)).ConfigureAwait(false))
                {
                    t.ThrowIfCancellationRequested();

                    return false;
                }

                return true;
            }

            else
            {
                await waitTask.ConfigureAwait(false);

                return true;
            }
        }

        public static async Task<bool> TryWaitWhile(TaskAwaiterPredicate condition, int frequency = 25, int timeout = -1)
        {
            bool loop = LOOP;
            bool cancel = CANCEL;

            void updateLoop() => cancel = !(loop = false);

            var waitTask = Task.Run(async () =>
            {
                while (loop && condition(ref cancel)) await Task.Delay(frequency).ConfigureAwait(false);
            });

            if (timeout >= 0)
            {
                if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)).ConfigureAwait(false))
                {
                    updateLoop();

                    return false;
                }

                return true;
            }

            else
            {
                await waitTask.ConfigureAwait(false);

                return true;
            }
        }

        public static async Task WaitWhile(TaskAwaiterPredicate condition, int frequency = 25, int timeout = -1)
        {
            if (!await TryWaitWhile(condition, frequency, timeout).ConfigureAwait(false))

                throw new TimeoutException();
        }
#endif

        public static string GetName<T>() => typeof(T).Name;

        public static void Lambda<T>(Action<Action> action, ActionRef<T> actionRef, ref T value)
        {
            T _value = value;

            action(() => actionRef(ref _value));

            value = _value;
        }

        public static void Lambda<T>(Action<Action, T> action, ActionRef<T> actionRef, ref T value)
        {
            T _value = value;

            action(() => actionRef(ref _value), value);

            value = _value;
        }

        public static void Lambda<T>(ActionIn<Action> action, ActionRef<T> actionRef, ref T value)
        {
            T _value = value;

            action(() => actionRef(ref _value));

            value = _value;
        }

        public static void Lambda<T>(ActionIn<Action, T> action, ActionRef<T> actionRef, ref T value)
        {
            T _value = value;

            action(() => actionRef(ref _value), value);

            value = _value;
        }

        public static void Lambda<T1, T2>(Action<Action, T1> action, ActionRef<T1, T2> actionRef, ref T1 value1, ref T2 value2)
        {
            T1 _value1 = value1;
            T2 _value2 = value2;

            action(() => actionRef(ref _value1, ref _value2), value1);

            value2 = _value2;
            value1 = _value1;
        }

        public static void Lambda<T1, T2>(Action<Action, T1, T2> action, ActionRef<T1, T2> actionRef, ref T1 value1, ref T2 value2)
        {
            T1 _value1 = value1;
            T2 _value2 = value2;

            action(() => actionRef(ref _value1, ref _value2), value1, value2);

            value2 = _value2;
            value1 = _value1;
        }

        public static void Lambda<T1, T2>(ActionIn<Action, T1> action, ActionRef<T1, T2> actionRef, ref T1 value1, ref T2 value2)
        {
            T1 _value1 = value1;
            T2 _value2 = value2;

            action(() => actionRef(ref _value1, ref _value2), value1);

            value2 = _value2;
            value1 = _value1;
        }

        public static void Lambda<T1, T2>(ActionIn<Action, T1, T2> action, ActionRef<T1, T2> actionRef, ref T1 value1, ref T2 value2)
        {
            T1 _value1 = value1;
            T2 _value2 = value2;

            action(() => actionRef(ref _value1, ref _value2), value1, value2);

            value2 = _value2;
            value1 = _value1;
        }

        public static T
#if CS9
            ?
#endif
            GetValue<T>(Func<T> func) => func == null ? default : func();
        public static TResult
#if CS9
            ?
#endif
            GetValue<TParam, TResult>(in Func<TParam, TResult> func, in TParam param) => func == null ? default : func(param);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, TResult>(in Func<T1, T2, TResult> func, in T1 param1, in T2 param2) => func == null ? default : func(param1, param2);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, TResult>(in Func<T1, T2, T3, TResult> func, in T1 param1, in T2 param2, in T3 param3) => func == null ? default : func(param1, param2, param3);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, TResult>(in Func<T1, T2, T3, T4, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4) => func == null ? default : func(param1, param2, param3, param4);
#if CS5
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, TResult>(in Func<T1, T2, T3, T4, T5, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5) => func == null ? default : func(param1, param2, param3, param4, param5);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, T6, TResult>(in Func<T1, T2, T3, T4, T5, T6, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6) => func == null ? default : func(param1, param2, param3, param4, param5, param6);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, T6, T7, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11, in T12 param12) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11, in T12 param12, in T13 param13) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11, in T12 param12, in T13 param13, in T14 param14) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11, in T12 param12, in T13 param13, in T14 param14, in T15 param15) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15);
        public static TResult
#if CS9
            ?
#endif
            GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11, in T12 param12, in T13 param13, in T14 param14, in T15 param15, in T16 param16) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15, param16);
#endif
        public static void Reverse<T>(ref T x, ref T y)
#if CS7
            => (y, x) = (x, y);
#else
        {
            T z = x;

            x = y;

            y = z;
        }
#endif
        public static void Reverse<T>(in T x, in T y, in Action<T> setX, in Action<T> setY)
        {
            ThrowIfNull(setX, nameof(setX));
            ThrowIfNull(setY, nameof(setY));

            setX(y);

            setY(x);
        }

        public static bool UpdateValue<T>(ref T value, in T newValue, in bool condition)
        {
            if (condition)
            {
                value = newValue;

                return true;
            }

            return false;
        }

        public static bool UpdateValue(ref bool value) => UpdateValue(ref value, false, value);

        public static bool UpdateValue<T>(ref T value, in T whenTrue, in T whenFalse, in bool condition)
        {
            if (condition)
            {
                value = whenTrue;

                return true;
            }

            value = whenFalse;

            return false;
        }

        public static bool UpdateValue2(ref bool value) => UpdateValue(ref value, false, true, value);

        public static bool UpdateValue<T>(ref T value, in T newValue)
        {
            if (Equals(value, newValue))

                return false;

            value = newValue;

            return true;
        }

        public static bool UpdateValue<T>(ref T value, T newValue, Action<T, T> action)
        {
            T _value = value;

            return UpdateValue(ref value, newValue, () => action(_value, newValue));
        }

        public const string Etcetera = "...";

        private static bool _TruncateIfLonger(ref string s, in int index, in FuncIn2<string, int, string> func)
        {
            if ((s ?? throw GetArgumentNullException(nameof(s))).Length > index)
            {
                s = func(s, index);

                return true;
            }

            return false;
        }

        private static bool _TruncateIfLonger(ref string s, int index, string replace, FuncIn2<string, int, string, string> func) => _TruncateIfLonger(ref s, index, (string _s, in int _index) => func(_s, _index, replace));

        public static bool TruncateIfLonger(ref string s, int index, string replace) => _TruncateIfLonger(ref s, index, replace, Util.Extensions.Truncate);

        public static bool TruncateIfLonger2(ref string s, in int index, in string replace) => _TruncateIfLonger(ref s, index, replace, Util.Extensions.Truncate2);

        public static bool TruncateIfLonger(ref string s, in int index) => _TruncateIfLonger(ref s, index, Util.Extensions.Truncate);
#if !CS5
        public static Type GetEnumUnderlyingType<T>() where T : Enum => typeof(T)._GetEnumUnderlyingType();
#endif
        public static bool For(in Func<bool> loopCondition, in Func<bool> action, in Action postIterationAction)
        {
            while (loopCondition())
            {
                if (action())

                    return true;

                postIterationAction();
            }

            return false;
        }

        public static bool For(in Func<bool> loopCondition, in Action action, in Action postIterationAction) => For(loopCondition, action.ToBoolFunc(), postIterationAction);

        public static bool TryFor(in Func<bool> loopCondition, Func<bool> action, in Action postIterationAction, out bool succeeded)
        {
            bool _succeeded = true;

            bool result = For(loopCondition, () =>
            {
                try
                {
                    return action();
                }

                catch { return _succeeded = false; }
            },
            postIterationAction);

            succeeded = _succeeded;

            return result;
        }

        public static bool TryFor(in Func<bool> loopCondition, Action action, in Action postIterationAction, out bool succeeded) => TryFor(loopCondition, action.ToBoolFunc(), postIterationAction, out succeeded);

        public static bool PredicateRef<T>(object value, Predicate<T
#if CS9
            ?
#endif
            > predicate) where T : class =>
#if CS9
            predicate(
#endif
            value is T _value ?
#if !CS9
            predicate(
#endif
                _value
#if !CS9
                )
#endif
            : value == null ?
#if !CS9
            predicate(
#endif
                null
#if !CS9
                )
#endif
            : throw GetInvalidTypeArgumentException(nameof(value))
#if CS9
        )
#endif
        ;
        public static bool PredicateVal<T>(object value, Predicate<T> predicate) where T : struct => predicate(value is T _value ? _value : throw GetInvalidTypeArgumentException(nameof(value)));

        public static bool UpdateValue<T>(ref T value, in T newValue, in Action action)
        {
            if (UpdateValue(ref value, newValue))
            {
                action();

                return true;
            }

            return false;
        }

        public static bool IsNullOrEmpty(in System.Array array) => array == null || array.Length == 0;

        public static TValue GetValue<TKey, TValue>(KeyValuePair<TKey, TValue> keyValuePair) => keyValuePair.Value;

        /// <summary>
        /// Provides a <see cref="Predicate"/> implementation that always returns <see langword="true"/>.
        /// </summary>
        /// <returns>Returns the <see langword="true"/> value.</returns>
        public static Predicate GetCommonPredicate() => (object value) => true;

        /// <summary>
        /// Provides a <see cref="Predicate{T}"/> implementation that always returns <see langword="true"/>.
        /// </summary>
        /// <returns>Returns the <see langword="true"/> value.</returns>
        public static Predicate<T> GetCommonPredicate<T>() => (T value) => true;
#if CS7
        public static bool IsFlagsEnum<T>() where T : Enum =>
#if !CS9
            !(
#endif
            typeof(T).GetCustomAttribute<FlagsAttribute>() is
#if CS9
            not
#endif
            null
#if !CS9
            )
#endif
            ;
#endif
        public static KeyValuePair<TKey, TValue> GetKeyValuePair<TKey, TValue>(in TKey key, in TValue value) => new
#if !CS9
    KeyValuePair<TKey, TValue>
#endif
    (key, value);

        public static KeyValuePair<TKeyOut, TValueOut> GetKeyValuePair<TKeyIn, TKeyOut, TValueIn, TValueOut>(KeyValuePair<TKeyIn, TValueIn> item) where TKeyIn : TKeyOut where TValueIn : TValueOut => GetKeyValuePair<TKeyOut, TValueOut>(item.Key, item.Value);

        public static KeyValuePair<TKey, Func<bool>> GetKeyValuePairPredicate<TKey>(in TKey key, in Func<bool> predicate) =>
#if CS9
            new
#else
            GetKeyValuePair
#endif
            (key, predicate);
#if CS5
        public static bool IsNullEmptyOrWhiteSpace(in string
#if CS8
            ?
#endif
            value) => string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
#endif
        public static BitArray GetBitArray(long[] values)
        {
            var array = new BitArray(values.Length * 64);

            for (int i = 0; i < values.Length; i++)

                new BitArray(BitConverter.GetBytes(values[i])).CopyTo(array, i * 64);

            return array;
        }

        /// <summary>
        /// Concatenates multiple arrays from a same item type. Arrays must have only one dimension.
        /// </summary>
        /// <param name="arrays">The different arrays to concatenate.</param>
        /// <returns>An array with a copy of all values of the given arrays.</returns>
        public static T[] Concatenate<T>(params T[][] arrays)
        {
            // /// <param name="elementType">The type of the items inside the tables.</param>

            int totalArraysLength = 0;

            int totalArraysIndex = 0;

            foreach (T[] array in arrays)

                totalArraysLength += array.Length;

            var newArray = new T[totalArraysLength];

            T[] _array;

            for (int i = 0; i < arrays.Length - 1; i++)
            {
                (_array = arrays[i]).CopyTo(newArray, totalArraysIndex);

                totalArraysIndex += _array.Length;
            }

            arrays[
#if CS8
                ^
#else
                arrays.Length -
#endif
                1].CopyTo(newArray, totalArraysIndex);

            return newArray;
        }

        /// <summary>
        /// Checks if a object is a numeric value (an instance of a numeric value type).
        /// </summary>
        /// <remarks>This function makes a check for the object type. For a string-parsing-checking for numerical value, look at the <see cref="IsNumeric(in string, out decimal)"/> function.</remarks>
        /// <param name="value">The object to check</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the object given is from a numerical type.</returns>
        public static bool IsNumber(in object value)
        {
#if CS9
            return
#else
            if (
#endif
            value is null
#if CS9
            ? false : value switch
#else
            )

                return false;

            switch (value)
#endif
            {
#if !CS9
                case
#endif
                byte _
#if CS9
                or
#else
            :
            case 
#endif
                sbyte _
#if CS9
                or
#else
            :
            case 
#endif
                short _
#if CS9
                or
#else
            :
            case 
#endif
                ushort _
#if CS9
                or
#else
            :
            case 
#endif
                int _
#if CS9
                or
#else
            :
            case 
#endif
                uint _
#if CS9
                or
#else
            :
            case 
#endif
                long _
#if CS9
                or
#else
            :
            case 
#endif
                ulong _
#if CS9
                or
#else
            :
            case 
#endif
                Int128 _
#if CS9
                or
#else
            :
            case 
#endif
                UInt128 _
#if CS9
                or
#else
            :
            case 
#endif
                float _
#if CS9
                or
#else
            :
            case 
#endif
                double _
#if CS9
                or
#else
            :
            case 
#endif
                decimal _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<byte> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<sbyte> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<short> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<ushort> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<int> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<uint> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<long> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<ulong> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<Int128> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<UInt128> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<float> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<double> _
#if CS9
                or
#else
            :
            case 
#endif
                Nullable<decimal> _
#if CS9
                =>
#else
            :
            return
#endif
                    true,
                _ => false
#if CS9
            };
#else
            ;
        }

            return false;
#endif
        }

        /// <summary>
        /// Checks if a <see cref="string"/> is a numerical value.
        /// </summary>
        /// <remarks>This function tries to parse a <see cref="string"/> value to a <see cref="decimal"/> value. Given that <see cref="decimal"/> type is the greatest numerical type in the .NET framework, all the numbers can be supported in the .NET framework can be set in a <see cref="decimal"/> object.</remarks>
        /// <param name="s">The <see cref="string"/> to check</param>
        /// <param name="d">The <see cref="decimal"/> in which one set the <see cref="decimal"/> value</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the <see cref="string"/> given is a <see cref="decimal"/>.</returns>
        public static bool IsNumeric(in string s, out decimal d) => decimal.TryParse(s, out d);
#if CS7
        /// <summary>
        /// Get all the flags in a flags enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <returns>All the flags in the given enum type.</returns>
        public static T GetAllEnumFlags<T>() where T : Enum
        {
            Type enumType = typeof(T);

            System.Array array = enumType.GetCustomAttribute<FlagsAttribute>() == null ? throw new ArgumentException("Enum is not a 'flags' enum.") : Enum.GetValues(enumType);

            return enumType.GetEnumUnderlyingType().IsType(true, typeof(sbyte), typeof(short), typeof(int), typeof(long)) ? (T)Enum.ToObject(enumType, NumericArrayToLongValue(array)) : (T)Enum.ToObject(enumType, NumericArrayToULongValue(array));
        }
#endif
        public static long NumericArrayToLongValue(System.Array array)
        {
            ThrowIfNull(array, nameof(array));

            long values = 0;

            foreach (object value in array)

                values |= (long)System.Convert.ChangeType(value, TypeCode.Int64, CultureInfo.CurrentCulture);

            return values;
        }

        public static ulong NumericArrayToULongValue(System.Array array)
        {
            ThrowIfNull(array, nameof(array));

            ulong values = 0;

            foreach (object value in array)

                values |= (ulong)System.Convert.ChangeType(value, TypeCode.UInt64, CultureInfo.CurrentCulture);

            return values;
        }

        /// <summary>
        /// Gets the numeric value for a field in an enum.
        /// </summary>
        /// <param name="enumType">The enum type in which to look for the specified enum field value.</param>
        /// <param name="fieldName">The enum field to look for.</param>
        /// <returns>The numeric value corresponding to this enum, in the given enum type underlying type.</returns>
        public static object GetNumValue<T>(in string fieldName) where T : Enum
        {
            Type enumType = typeof(T);

            return enumType.IsEnum ? System.Convert.ChangeType(enumType.GetField(fieldName).GetValue(null), Enum.GetUnderlyingType(enumType)) : throw new ArgumentException("'enumType' is not an enum type.");
        }
#if CS8
        // https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/

        public static Process StartProcessNetCore(in string url) =>

             // hack because of this: https://github.com/dotnet/corefx/issues/10361
             // if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))

             Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}") { CreateNoWindow = true });

        //else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        //{
        //    Process.Start("xdg-open", url);
        //}
        //else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        //{
        //    Process.Start("open", url);
        //}
        //else
        //{
        //    throw;
        //}

        public static void StartProcessNetCore(in Uri url)
        {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))

                _ = Process.Start(new ProcessStartInfo("cmd", $"/c start {url.ToString().Replace("&", "^&")}") { CreateNoWindow = true });

            //else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            //{
            //    Process.Start("xdg-open", url);
            //}
            //else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            //{
            //    Process.Start("open", url);
            //}
            //else
            //{
            //    throw;
            //}
        }
#endif
    }
#if WinCopies4
    namespace
#else
    public static class
#endif
        Consts
    {
        public static class TypeSizes
        {
            public const byte Int1 = sizeof(byte);
            public const byte Int2 = Int1 << 1;
            public const byte Int4 = Int2 << 1;
            public const byte Int8 = Int4 << 1;
            public const byte Int16 = Int8 << 1;
            public const byte Int32 = Int16 << 1;
            public const byte Int64 = Int32 << 1;

            public const byte HalfByteMaxValue = byte.MaxValue >> Int4;

            public const byte HighPartHalfByteMaxValue = HalfByteMaxValue << Int4;
            public const ushort HighPartByteMaxValue = byte.MaxValue << Int8;
            public const uint HighPartShortMaxValue = (uint)ushort.MaxValue << Int16;
            public const ulong HighPartIntMaxValue = uint.MaxValue << Int32;
        }
#if WinCopies4
        namespace
#else
        public static class
#endif
            Symbols
        {
            public static class GeneralPunctuation
            {
                public const char EmDash = '\u2014';
                public const char EnDash = '\u2013';
                public const char Bullet = '\u2022';
            }

            public static class MathematicalOperators
            {
                public const char PlusMinus = '\u00B1';
                public const char PerMille = '\u2030';
                public const char PartialDifferential = '\u2202';
                public const char Delta = '\u2206';
                public const char Pi = '\u220F';
                public const char Sigma = '\u2211';
                public const char Minus = '\u2212';
                public const char BulletMultiplication = '\u2219';
                public const char SquareRoot = '\u221A';
                public const char Division = '\u2215';
                public const char Infinity = '\u221E';
                public const char Integral = '\u222B';
                public const char AlmostEqualTo = '\u2248';
                public const char NotEqualTo = '\u2260';
                public const char IdenticalTo = '\u2261';
                public const char NotIdenticalTo = '\u2262';
                public const char LessThanOrEqualTo = '\u2264';
                public const char GreaterThanOrEqualTo = '\u2265';
            }

            public static class GeometricShapes
            {
                public const char TriangleUp = '\u25B2';
                public const char TriangleRight = '\u25BA';
                public const char TriangleDown = '\u25BC';
                public const char TriangleLeft = '\u25C4';
            }
        }
#if WinCopies4
        public static class Common
        {
#endif
            public const int DefaultBufferLength = 4096;

            public const int NotSetIndex = -1;
            public const int DefaultStartIndex = 0;

            public const string NotApplicable = "N/A";

            public const char PathFilterChar = '*';
            public const char LikeStatementChar = '%';

            public const byte LSBMask = 1;
            public const byte MSBMask = LSBMask << 7;
            public const byte SetToFalseMask = byte.MaxValue;

            public const ushort MSBMask16 = LSBMask << 15;
            public const ushort SetToFalseMask16 = ushort.MaxValue;

            public const BindingFlags DefaultBindingFlagsForPropertySet = BindingFlags.Public | BindingFlags.NonPublic |
                             BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
#if WinCopies4
        }
#endif
    }
}
