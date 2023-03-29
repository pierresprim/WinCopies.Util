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
using System.IO;
using System.Linq;
#if CS5
using System.Numerics;
#endif
using System.Reflection;
using System.Text;
using System.Xml;

using WinCopies.Collections;
using WinCopies.Collections.Generic;
using WinCopies.Extensions;

using static WinCopies.XOrResult;

using static WinCopies.Consts
#if WinCopies4
    .Common;
using WinCopies.Consts
#endif
    ;
using static WinCopies.ThrowHelper;
using static WinCopies.UtilHelpers;

namespace WinCopies.Util // To avoid name conflicts.
{
    /// <summary>
    /// Provides some static extension methods.
    /// </summary>
    public static partial class Extensions
    {
        // Source: https://stackoverflow.com/a/21581418
        public static void AddTo<T>(this T flag, ref T value, bool set) where T : Enum
        {
            Type type = typeof(T);
            Type underlyingType = type.GetCustomAttributes<FlagsAttribute>().Any() ? Enum.GetUnderlyingType(type) : throw new ArgumentException("The given value is not a flags enum.");

            // note: AsInt mean: math integer vs enum (not the c# int type)
            dynamic valueAsInt = System.Convert.ChangeType(value, underlyingType);
            dynamic flagAsInt = System.Convert.ChangeType(flag, underlyingType);

            if (set)

                valueAsInt |= flagAsInt;

            else

                valueAsInt &= ~flagAsInt;

            value = (T)valueAsInt;
        }
        public static T SetFlag<T>(this T value, T flag, bool set) where T : Enum
        {
            flag.AddTo(ref value, set);

            return value;
        }

        public static void Invoke<T>(this ValueEventHandler<T> eventHandler, in object sender, in T value) => eventHandler?.Invoke(sender, new Data.EventArgs<T>(value));

        public static void ThrowIf<T>(this Exception
#if CS8
            ?
#endif
            e) where T : Exception
        {
            if ((e ?? throw new ArgumentNullException(nameof(e))) is T)

                throw e;
        }

        public static void ThrowIfNot<T>(this Exception
#if CS8
            ?
#endif
            e) where T : Exception
        {
            if (!((e ?? throw new ArgumentNullException(nameof(e))) is T))

                throw e;
        }

        public static void TryThrowIf<T>(this Exception
#if CS8
            ?
#endif
            e) where T : Exception
        {
            if (e is T)

                throw e;
        }

        public static void TryThrowIfNot<T>(this Exception
#if CS8
            ?
#endif
            e) where T : Exception
        {
            if (!(e == null || e is T))

                throw e;
        }

        public static byte ToByte(this bool value) => value ? (byte)0b1 : (byte)0;

        public static IEnumerable<XmlNode> Enumerate(this XmlNode node) => EnumerateUntilNull(node.FirstChild, _node => _node.NextSibling);

        #region Bitwise Operations
        public static ulong Concatenate(this uint x, in uint y) => ((ulong)x << TypeSizes.Int32) | y;
        public static uint Concatenate(this ushort x, in ushort y) => ((uint)x << TypeSizes.Int16) | y;
        public static ushort Concatenate(this byte x, in byte y) => (byte)(((ushort)x << TypeSizes.Int8) | y);
        public static byte ConcatenateHalfParts(this byte x, in byte y) => (byte)((x << TypeSizes.Int4) | y);

        public static ulong ToHighPart(this uint value) => (ulong)value << TypeSizes.Int32;
        public static uint ToHighPart(this ushort value) => (uint)value << TypeSizes.Int16;
        public static ushort ToUInt16HighPart(this byte value) => (ushort)((ushort)value << TypeSizes.Int8);
        public static byte ToUInt8HighPart(this byte value) => value > TypeSizes.HalfByteMaxValue ? throw new ArgumentOutOfRangeException(nameof(value)) : (byte)(value << TypeSizes.HalfByteMaxValue);

        public static long ToHighPart(this int value) => unchecked((long)unchecked((uint)value).ToHighPart());
        public static int ToHighPart(this short value) => unchecked((int)unchecked((ushort)value).ToHighPart());
        public static short ToUInt16HighPart(this sbyte value) => unchecked((short)unchecked((byte)value).ToUInt16HighPart());
        public static sbyte ToUInt8HighPart(this sbyte value) => unchecked((sbyte)unchecked((byte)value).ToUInt8HighPart());

        public static ulong AsHighPart(this ulong value) => value << TypeSizes.Int32;
        public static uint AsHighPart(this uint value) => value << TypeSizes.Int16;
        public static ushort AsHighPart(this ushort value) => (ushort)(value << TypeSizes.Int8);
        public static byte AsHighPart(this byte value) => (byte)(value << TypeSizes.HalfByteMaxValue);

        public static long AsHighPart(this long value) => unchecked((long)unchecked((ulong)value).AsHighPart());
        public static int AsHighPart(this int value) => unchecked((int)unchecked((uint)value).AsHighPart());
        public static short AsHighPart(this short value) => unchecked((short)unchecked((ushort)value).AsHighPart());
        public static sbyte AsHighPart(this sbyte value) => unchecked((sbyte)unchecked((byte)value).AsHighPart());

        public static ulong AsLowPart(this ulong value) => value >> TypeSizes.Int32;
        public static uint AsLowPart(this uint value) => value >> TypeSizes.Int16;
        public static ushort AsLowPart(this ushort value) => (ushort)(value >> TypeSizes.Int8);
        public static byte AsLowPart(this byte value) => (byte)(value >> TypeSizes.HalfByteMaxValue);

        public static long AsLowPart(this long value) => unchecked((long)unchecked((ulong)value).AsHighPart());
        public static int AsLowPart(this int value) => unchecked((int)unchecked((uint)value).AsHighPart());
        public static short AsLowPart(this short value) => unchecked((short)unchecked((ushort)value).AsHighPart());
        public static sbyte AsLowPart(this sbyte value) => unchecked((sbyte)unchecked((byte)value).AsHighPart());

        public static ulong GetAsHighPart(this ulong value) => value & TypeSizes.HighPartIntMaxValue;
        public static uint GetAsHighPart(this uint value) => value & TypeSizes.HighPartShortMaxValue;
        public static ushort GetAsHighPart(this ushort value) => (ushort)(value & TypeSizes.HighPartByteMaxValue);
        public static byte GetAsHighPart(this byte value) => (byte)(value & TypeSizes.HighPartHalfByteMaxValue);

        public static long GetAsHighPart(this long value) => unchecked((long)unchecked((ulong)value).GetAsHighPart());
        public static int GetAsHighPart(this int value) => unchecked((int)unchecked((uint)value).GetAsHighPart());
        public static short GetAsHighPart(this short value) => unchecked((short)unchecked((ushort)value).GetAsHighPart());
        public static sbyte GetAsHighPart(this sbyte value) => unchecked((sbyte)unchecked((byte)value).GetAsHighPart());

        private static T ShiftHighPart<T>(in
#if CS11
            T
#else
            dynamic
#endif
            value, in byte offset, in byte size) where T : unmanaged
#if CS11
            , IUnsignedNumber<T>, IShiftOperators<T, int, T>
#endif
            => offset == 0 ? value
#if !CS11
            .InnerValue
#endif
            : offset == size ? default : offset > size ? throw new ArgumentOutOfRangeException(nameof(offset)) : value
#if CS11
            >>
#else
            .Shift(
#endif
            offset
#if !CS11
                )
#endif
                ;
#if CS11
        public
#else
        private
#endif
            static unsafe T ShiftHighPart<T>(
#if CS11
            this T
#else
            in dynamic
#endif
            value, in byte offset) where T : unmanaged
#if CS11
            , IUnsignedNumber<T>, IShiftOperators<T, int, T>
#endif
            => ShiftHighPart(value, offset, (byte)(sizeof(T) << 3));
#if CS11
        public
#else
        private
#endif
            static unsafe T GetHighPart<T>(
#if CS11
            this T
#else
            in dynamic
#endif
            value, in byte length) where T : unmanaged
#if CS11
            , IUnsignedNumber<T>, IShiftOperators<T, int, T>
#endif
        {
            byte size = (byte)(sizeof(T) << 3);

            return ShiftHighPart(value, (byte)(size - length), size);
        }

        private static T ShiftLowPart<T>(in
#if CS11
            T
#else
            dynamic
#endif
            value, in byte offset, in byte size) where T : unmanaged
#if CS11
            , IUnsignedNumber<T>, IShiftOperators<T, int, T>
#endif
            => offset == 0 ? value : offset == size ? default : offset > size ? throw new ArgumentOutOfRangeException(nameof(offset)) : (value << offset) >> offset;
#if CS11
        public
#else
        private
#endif
            static unsafe T ShiftLowPart<T>(
#if CS11
            this T
#else
            in dynamic
#endif
            value, in byte offset) where T : unmanaged
#if CS11
            , IUnsignedNumber<T>, IShiftOperators<T, int, T>
#endif
            => ShiftLowPart(value, offset, (byte)(sizeof(T) << 3));
#if CS11
        public
#else
        private
#endif
            static unsafe T GetLowPart<T>(
#if CS11
            this T
#else
            in dynamic
#endif
            value, in byte length) where T : unmanaged
#if CS11
            , IUnsignedNumber<T>, IShiftOperators<T, int, T>
#endif
        {
            byte size = (byte)(sizeof(T) << 3);

            return ShiftLowPart(value, (byte)(size - length), size);
        }

        #region (U)Int64
        #region UInt64
        public static uint GetHighPart(this ulong value) => (uint)(value >> TypeSizes.Int32);
#if !CS11
        public static ulong ShiftHighPart(this ulong value, in byte offset) => ShiftHighPart<ulong>(value, offset);
        public static ulong GetHighPart(this ulong value, in byte length) => GetHighPart<ulong>(value, length);
#endif
        public static uint GetLowPart(this ulong value) => (uint)(value & uint.MaxValue);
#if !CS11
        public static ulong ShiftLowPart(this ulong value, in byte offset) => ShiftLowPart<ulong>(value, offset);
        public static ulong GetLowPart(this ulong value, in byte length) => GetLowPart<ulong>(value, length);
#endif
        public static ulong SetHighPart(this ulong value, in uint newHighPart) => newHighPart.ToHighPart() | (value & uint.MaxValue);
        public static ulong SetLowPart(this ulong value, in uint newLowPart) => value.GetHighPart().Concatenate(newLowPart);
        #endregion UInt64

        #region Int64
        public static int GetHighPart(this long value) => unchecked((int)unchecked((ulong)value).GetHighPart());
        public static long ShiftHighPart(this long value, in byte offset) => unchecked((long)ShiftHighPart(unchecked((ulong)value), offset));
        public static long GetHighPart(this long value, in byte length) => unchecked((long)GetHighPart(unchecked((ulong)value), length));

        public static int GetLowPart(this long value) => unchecked((int)unchecked((ulong)value).GetLowPart());
        public static long ShiftLowPart(this long value, in byte offset) => unchecked((long)ShiftLowPart(unchecked((ulong)value), offset));
        public static long GetLowPart(this long value, in byte length) => unchecked((long)GetLowPart(unchecked((ulong)value), length));

        public static long SetHighPart(this long value, in int newHighPart) => unchecked((long)unchecked((ulong)value).SetHighPart(unchecked((uint)newHighPart)));
        public static long SetLowPart(this long value, in int newLowPart) => unchecked((long)unchecked((ulong)value).SetLowPart(unchecked((uint)newLowPart)));
        #endregion Int64
        #endregion (U)Int64

        #region (U)Int32
        #region UInt32
        public static ushort GetHighPart(this uint value) => (ushort)(value >> TypeSizes.Int16);
#if !CS11
        public static uint ShiftHighPart(this uint value, in byte offset) => ShiftHighPart<uint>(value, offset);
        public static uint GetHighPart(this uint value, in byte length) => GetHighPart<uint>(value, length);
#endif
        public static ushort GetLowPart(this uint value) => (ushort)(value & ushort.MaxValue);
#if !CS11
        public static uint ShiftLowPart(this uint value, in byte offset) => ShiftLowPart<uint>(value, offset);
        public static uint GetLowPart(this uint value, in byte length) => GetLowPart<uint>(value, length);
#endif
        public static uint SetHighPart(this uint value, in ushort newHighPart) => newHighPart.ToHighPart() | (value & ushort.MaxValue);
        public static uint SetLowPart(this uint value, in ushort newLowPart) => value.GetHighPart().Concatenate(newLowPart);
        #endregion UInt32

        #region Int32
        public static short GetHighPart(this int value) => unchecked((short)unchecked((uint)value).GetHighPart());
        public static int ShiftHighPart(this int value, in byte offset) => unchecked((int)ShiftHighPart(unchecked((uint)value), offset));
        public static int GetHighPart(this int value, in byte length) => unchecked((int)GetHighPart(unchecked((uint)value), length));

        public static short GetLowPart(this int value) => unchecked((short)unchecked((uint)value).GetLowPart());
        public static int ShiftLowPart(this int value, in byte offset) => unchecked((int)ShiftLowPart(unchecked((uint)value), offset));
        public static int GetLowPart(this int value, in byte length) => unchecked((int)GetLowPart(unchecked((uint)value), length));

        public static int SetHighPart(this int value, in short newHighPart) => unchecked((int)unchecked((uint)value).SetHighPart(unchecked((ushort)newHighPart)));
        public static int SetLowPart(this int value, in short newLowPart) => unchecked((int)unchecked((uint)value).SetLowPart(unchecked((ushort)newLowPart)));
        #endregion Int32
        #endregion (U)Int32

        #region (U)Int16
        #region UInt16
        public static byte GetHighPart(this ushort value) => (byte)(value >> TypeSizes.Int8);
#if !CS11
        public static ushort ShiftHighPart(this ushort value, in byte offset) => ShiftHighPart<ushort>(value, offset);
        public static ushort GetHighPart(this ushort value, in byte length) => GetHighPart<ushort>(value, length);
#endif
        public static byte GetLowPart(this ushort value) => (byte)(value & byte.MaxValue);
#if !CS11
        public static ushort ShiftLowPart(this ushort value, in byte offset) => ShiftLowPart<ushort>(value, offset);
        public static ushort GetLowPart(this ushort value, in byte length) => GetLowPart<ushort>(value, length);
#endif
        public static ushort SetHighPart(this ushort value, in byte newHighPart) => (ushort)(newHighPart.ToUInt16HighPart() | ((uint)value & byte.MaxValue));
        public static ushort SetLowPart(this ushort value, in byte newLowPart) => value.GetHighPart().Concatenate(newLowPart);
        #endregion UInt16

        #region Int16
        public static sbyte GetHighPart(this short value) => unchecked((sbyte)unchecked((ushort)value).GetHighPart());
        public static short ShiftHighPart(this short value, in byte offset) => unchecked((short)ShiftHighPart(unchecked((ushort)value), offset));
        public static short GetHighPart(this short value, in byte length) => unchecked((short)GetHighPart(unchecked((ushort)value), length));

        public static sbyte GetLowPart(this short value) => unchecked((sbyte)unchecked((ushort)value).GetLowPart());
        public static short ShiftLowPart(this short value, in byte offset) => unchecked((short)ShiftLowPart(unchecked((ushort)value), offset));
        public static short GetLowPart(this short value, in byte length) => unchecked((short)GetLowPart(unchecked((ushort)value), length));

        public static short SetHighPart(this short value, in sbyte newHighPart) => unchecked((short)unchecked((ushort)value).SetHighPart(unchecked((byte)newHighPart)));
        public static short SetLowPart(this short value, in sbyte newLowPart) => unchecked((short)unchecked((ushort)value).SetLowPart(unchecked((byte)newLowPart)));
        #endregion Int16
        #endregion (U)Int16

        #region (U)Int8
        #region UInt8
        public static byte GetHighPart(this byte value) => (byte)(value >> TypeSizes.Int4);
#if !CS11
        public static byte ShiftHighPart(this byte value, in byte offset) => ShiftHighPart<byte>(value, offset);
        public static byte GetHighPart(this byte value, in byte length) => GetHighPart<byte>(value, length);
#endif
        public static byte GetLowPart(this byte value) => (byte)(value & TypeSizes.HalfByteMaxValue);
#if !CS11
        public static byte ShiftLowPart(this byte value, in byte offset) => ShiftLowPart<byte>(value, offset);
        public static byte GetLowPart(this byte value, in byte length) => GetLowPart<byte>(value, length);
#endif
        public static byte SetHighPart(this byte value, in byte newHighPart) => (byte)(newHighPart.ToUInt8HighPart() | ((uint)value & TypeSizes.HalfByteMaxValue));
        public static byte SetLowPart(this byte value, in byte newLowPart) => value.GetHighPart().ConcatenateHalfParts(newLowPart);
        #endregion UInt8

        #region Int8
        public static sbyte GetHighPart(this sbyte value) => unchecked((sbyte)unchecked((byte)value).GetHighPart());
        public static sbyte ShiftHighPart(this sbyte value, in byte offset) => unchecked((sbyte)ShiftHighPart(unchecked((byte)value), offset));
        public static sbyte GetHighPart(this sbyte value, in byte length) => unchecked((sbyte)GetHighPart(unchecked((byte)value), length));

        public static sbyte GetLowPart(this sbyte value) => unchecked((sbyte)unchecked((byte)value).GetLowPart());
        public static sbyte ShiftLowPart(this sbyte value, in byte offset) => unchecked((sbyte)ShiftLowPart(unchecked((byte)value), offset));
        public static sbyte GetLowPart(this sbyte value, in byte length) => unchecked((sbyte)GetLowPart(unchecked((byte)value), length));

        public static sbyte SetHighPart(this sbyte value, in sbyte newHighPart) => unchecked((sbyte)unchecked((byte)value).SetHighPart(unchecked((byte)newHighPart)));
        public static sbyte SetLowPart(this sbyte value, in sbyte newLowPart) => unchecked((sbyte)unchecked((byte)value).SetLowPart(unchecked((byte)newLowPart)));
        #endregion Int8
        #endregion (U)Int8

        #region (U)IntPtr
        #region UIntPtr
        public static ushort GetHighWord(this UIntPtr i) => unchecked((uint)i).GetHighPart();
        public static ushort GetLowWord(this UIntPtr i) => unchecked((uint)i).GetLowPart();

        public static uint GetHighDWord(this UIntPtr i) => ((ulong)i).GetHighPart();
        public static uint GetLowDWord(this UIntPtr i) => ((ulong)i).GetLowPart();

        public static short GetHighShort(this UIntPtr i) => unchecked((short)i.GetHighWord());
        public static short GetLowShort(this UIntPtr i) => unchecked((short)i.GetLowWord());

        public static int GetHighInt(this UIntPtr i) => unchecked((int)i.GetHighDWord());
        public static int GetLowInt(this UIntPtr i) => unchecked((int)i.GetLowDWord());
        #endregion UIntPtr

        #region IntPtr
        public static ushort GetHighWord(this IntPtr i) => unchecked((uint)i.ToInt64()).GetHighPart();
        public static ushort GetLowWord(this IntPtr i) => unchecked((uint)i.ToInt64()).GetLowPart();

        public static uint GetHighDWord(this IntPtr i) => unchecked((ulong)i.ToInt64()).GetHighPart();
        public static uint GetLowDWord(this IntPtr i) => unchecked((ulong)i.ToInt64()).GetLowPart();

        public static short GetHighShort(this IntPtr i) => unchecked((short)i.GetHighWord());
        public static short GetLowShort(this IntPtr i) => unchecked((short)i.GetLowWord());

        public static int GetHighInt(this IntPtr i) => unchecked((int)i.GetHighDWord());
        public static int GetLowInt(this IntPtr i) => unchecked((int)i.GetLowDWord());
        #endregion IntPtr
        #endregion (U)IntPtr
        #endregion Bitwise Operations

        public static string PadWithZeroes(this string s, in int count) => s.PadLeft(count, '0');
        public static unsafe string PadWithZeroes<T>(this string s) where T : unmanaged => s.PadLeft(sizeof(T), '0');

        private static string ToString<T>(this T b, in byte toBase, in Func<T, int, string> func) where T : unmanaged => func(b, toBase).PadWithZeroes<T>();
        public static string ToString(this byte b, in byte toBase) => b.ToString(toBase, System.Convert.ToString);
        public static string ToString(this sbyte b, in byte toBase) => unchecked((byte)b).ToString(toBase, System.Convert.ToString);
        public static string ToString(this short s, in byte toBase) => s.ToString(toBase, System.Convert.ToString);
        public static string ToString(this ushort s, in byte toBase) => unchecked((short)s).ToString(toBase, System.Convert.ToString);
        public static string ToString(this int i, in byte toBase) => i.ToString(toBase, System.Convert.ToString);
        public static string ToString(this uint i, in byte toBase) => unchecked((int)i).ToString(toBase, System.Convert.ToString);
        public static string ToString(this long l, in byte toBase) => l.ToString(toBase, System.Convert.ToString);
        public static string ToString(this ulong l, in byte toBase) => unchecked((long)l).ToString(toBase, System.Convert.ToString);
        public static string ToString(this char c, in byte toBase) => ((short)c).ToString(toBase, System.Convert.ToString);
#if CS8
        private class HttpStream : System.IO.Stream
        {
            private readonly long? _length;

            private readonly System.IO.Stream _stream;

            public override bool CanRead => _stream.CanRead;

            public override bool CanSeek => _stream.CanSeek;

            public override bool CanWrite => _stream.CanWrite;

            public override long Length => _length.HasValue ? _length.Value : -1;

            public override long Position { get => _stream.Position; set => _stream.Position = value; }

            public HttpStream(in System.IO.Stream stream, in long? length)
            {
                _stream = stream;
                _length = length;
            }

            public override void Flush() => _stream.Flush();
            public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);
            public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);
            public override void SetLength(long value) => _stream.SetLength(value);
            public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);
        }

        public static async System.Threading.Tasks.Task<System.IO.Stream> GetStreamWithLengthAsync(this System.Net.Http.HttpClient httpClient, string url) => new HttpStream(await httpClient.GetStreamAsync(url), GetHttpFileSize(url, httpClient));
#endif
        public static Action GetAction(this Func<Action> func) => () => func()();
        public static Action<T> GetAction<T>(this Func<Action<T>> func) => value => func()(value);
        public static ActionIn<T> GetAction<T>(this Func<ActionIn<T>> func) => (in T value) => func()(value);
        public static Func GetFunc(this Func<Func> func) => () => func()();
        public static Func<TIn, TOut> GetFunc<TIn, TOut>(this Func<Func<TIn, TOut>> func) => value => func()(value);
        public static FuncIn<TIn, TOut> GetFunc<TIn, TOut>(this Func<FuncIn<TIn, TOut>> func) => (in TIn value) => func()(value);
#if CS8
        public static FuncNull GetFuncNull(this Func<FuncNull> func) => () => func()();
#endif
        public static Func<T> GetFunc<T>(this Func<Func<T>> func) => () => func()();

        private static bool TryGet<T>(in int index, in Func<int> func, Func<T> _func, out T
#if CS9
                ?
#endif
            result)
        {
            if (index.Between(0, func(), true, false))
            {
                result = _func();

                return true;
            }

            result = default;

            return false;
        }

        public static bool TryGet(this System.Array array, int index, out object result) => TryGet(index, () => array.Length, () => array.GetValue(index), out result);
        public static object TryGet(this System.Array array, int index) => array.TryGet(index, out object result) ? result : null;

        public static bool TryGet<T>(this
#if CS5
        IReadOnlyList<T>
#else
                    T[]
#endif
            array, int index, out T result) => TryGet(index, () => array.
#if CS5
        Count
#else
                    Length
#endif
            , () => array[index], out result);
        public static T TryGet<T>(this
#if CS5
        IReadOnlyList<T>
#else
                    T[]
#endif
            array, int index) => array.TryGet(index, out T result) ? result : default;

        public static bool TryGetFromLast(this System.Array array, int index, out object result) => TryGet(index, () => array.Length, () => array.GetFromLast(index), out result);
        public static object TryGetFromLast(this System.Array array, int index) => array.TryGet(index, out object result) ? result : null;

        public static bool TryGetFromLast<T>(this
#if CS5
        IReadOnlyList<T>
#else
                    T[]
#endif
            array, int index, out T result) => TryGet(index, () => array.
#if CS5
        Count
#else
                    Length
#endif
            , () => array.GetFromLast(index), out result);
        public static T TryGetFromLast<T>(this
#if CS5
        IReadOnlyList<T>
#else
                    T[]
#endif
            array, int index) => array.TryGet(index, out T result) ? result : default;

#if !CS5
        public static T GetCustomAttribute<T>(this Type type, in bool inherit) => (T)type.GetCustomAttributes(typeof(T), inherit).TryGet(0);
#endif

        public static IEnumerable<KeyValuePair<string, System.IO.Stream>> EnumerateEmbeddedResources(this Assembly assembly)
        {
            foreach (string resourceName in assembly.GetManifestResourceNames())

                yield return new KeyValuePair<string, System.IO.Stream>(resourceName, assembly.GetManifestResourceStream(resourceName));
        }

        public static T
#if CS9
                ?
#endif
            TryCast<T>(this object
#if CS8
                    ?
#endif
            value, out bool succeeded)
        {
            if (value is T _value)
            {
                succeeded = true;

                return _value;
            }

            succeeded = false;

            return default;
        }

        public static T
#if CS9
                ?
#endif
            TryCast<T>(this object
#if CS8
                    ?
#endif
            value) => value.TryCast<T>(out _);

        public static char ToChar(this LoggingLevel level)
#if CS8
                =>
#else
        {
            switch (
#endif
            level
#if CS8
                switch
#else
            )
#endif
            {
#if !CS8
                case
#endif
                LoggingLevel.Information
#if CS8
                    =>
#else
            :
                    return
#endif
                               'I'
#if CS8
                        ,
#else
                           ;
                case
#endif
                LoggingLevel.Success
#if CS8
            =>
#else
            :
                    return
#endif
                       'S'
#if CS8
                        ,
#else
                           ;
                case
#endif
                LoggingLevel.Warning
#if CS8
                    =>
#else
            :
                    return
#endif
                               'W'
#if CS8
                        ,
#else
                           ;
                case
#endif
                LoggingLevel.Error
#if CS8
                    =>
#else
            :
                    return
#endif
                               'E'
#if CS8
                        ,
                _ =>
#else
                           ;
                default:
                    return
#endif
    'N'
#if CS8
            };
#else
                ;
            }
        }
#endif

        public static string GetAssemblyName(this Assembly assembly)
        {
            AssemblyName assemblyName = assembly.GetName();
            string
#if CS8
                    ?
#endif
                name = assemblyName.Name;

            return
#if CS5
            IsNullEmptyOrWhiteSpace
#else
                        string.IsNullOrEmpty
#endif
                (name) ? assemblyName.FullName : name;
        }

        #region Bitwise Action
        private static void BitwiseAction(in byte pos, in byte limit, in Action action) => (pos >= 0 && pos <= limit ? action : throw new IndexOutOfRangeException())();

        #region UI8
        private static bool GetBit(in byte pos, byte mask, Func<int> func)
        {
            bool b = false;

            BitwiseAction(pos, 7, () => b = (func() & mask) != 0);

            return b;
        }

        public static bool GetBitFromLeft(this byte b, byte pos) => GetBit(pos, MSBMask, () => b << pos);
        public static bool GetBit(this byte b, byte pos) => GetBit(pos, LSBMask, () => b >> pos);

        private static byte SetBit(in byte pos, Func<int> func)
        {
            byte b = 0;

            BitwiseAction(pos, 7, () => b = (byte)func());

            return b;
        }

        public static byte ToggleBitFromLeft(this byte b, byte pos) => SetBit(pos, () => b ^ (MSBMask >> pos));

        public static byte SetBitFromLeft(this byte b, byte pos) => SetBit(pos, () => b | (MSBMask >> pos));

        public static byte UnsetBitFromLeft(this byte b, byte pos) => SetBit(pos, () => b & ~(MSBMask >> pos));

        public static byte SetBitFromLeft(this byte b, in byte pos, in bool value) => value ? b.SetBitFromLeft(pos) : b.UnsetBitFromLeft(pos);

        public static byte ToggleBit(this byte b, byte pos) => SetBit(pos, () => b ^ (LSBMask << pos));

        public static byte SetBit(this byte b, byte pos) => SetBit(pos, () => b | (LSBMask << pos));

        public static byte UnsetBit(this byte b, byte pos) => SetBit(pos, () => b & ~(LSBMask << pos));

        public static byte SetBit(this byte b, in byte pos, in bool value) => value ? b.SetBit(pos) : b.UnsetBit(pos);
        #endregion UI8

        #region UI16
        private static bool GetBit16(in byte pos, ushort mask, Func<int> func)
        {
            bool b = false;

            BitwiseAction(pos, 15, () => b = (func() & mask) != 0);

            return b;
        }

        public static bool GetBitFromLeft(this ushort b, byte pos) => GetBit16(pos, MSBMask16, () => b << pos);
        public static bool GetBit(this ushort b, byte pos) => GetBit16(pos, LSBMask, () => b >> pos);

        private static ushort SetBit16(in byte pos, Func<int> func)
        {
            ushort b = 0;

            BitwiseAction(pos, 15, () => b = (ushort)func());

            return b;
        }

        public static ushort ToggleBitFromLeft(this ushort b, byte pos) => SetBit16(pos, () => b ^ (MSBMask16 >> pos));

        public static ushort SetBitFromLeft(this ushort b, byte pos) => SetBit16(pos, () => b | (MSBMask16 >> pos));

        public static ushort UnsetBitFromLeft(this ushort b, byte pos) => SetBit16(pos, () => b & ~(MSBMask16 >> pos));

        public static ushort SetBitFromLeft(this ushort b, in byte pos, in bool value) => value ? b.SetBitFromLeft(pos) : b.UnsetBitFromLeft(pos);

        public static ushort ToggleBit(this ushort b, byte pos) => SetBit16(pos, () => b ^ (LSBMask << pos));

        public static ushort SetBit(this ushort b, byte pos) => SetBit16(pos, () => b | (LSBMask << pos));

        public static ushort UnsetBit(this ushort b, byte pos) => SetBit16(pos, () => b & ~(LSBMask << pos));

        public static ushort SetBit(this ushort b, in byte pos, in bool value) => value ? b.SetBit(pos) : b.UnsetBit(pos);
        #endregion UI16
        #endregion Bitwise Action

#if CS5
        private static void _For<T>(in IReadOnlyList<T> values, in Action<T> action)
        {
            for (int i = 0; i < values.Count; i++)

                action(values[i]);
        }

        private static void _For<T>(in IReadOnlyList<T> values, in ActionIn<T> action)
        {
            for (int i = 0; i < values.Count; i++)

                action(values[i]);
        }

        private static void For<TItems, TAction>(in IReadOnlyList<TItems> values, in TAction action, in ActionIn<IReadOnlyList<TItems>, TAction> _action) => _action(values ?? throw GetArgumentNullException(nameof(values)), action
#if CS8
                    ??
#else
        == null ?
#endif
            throw GetArgumentNullException(nameof(action))
#if !CS8
        : action
#endif
            );

        public static void For<T>(this IReadOnlyList<T> values, in Action<T> action) => For(values, action, _For);

        public static void For<T>(this IReadOnlyList<T> values, in ActionIn<T> action) => For(values, action, _For);

        public delegate void ForAction<T>(ref T
#if CS9
                        ?
#endif
                value, T current);
        public delegate void ForActionIn<T>(ref T
#if CS9
                        ?
#endif
                value, in T current);

        public static T
#if CS9
                        ?
#endif
                For<T>(in IReadOnlyList<T> values, ForAction<T> action)
        {
            ThrowIfNull(values, nameof(values));
            ThrowIfNull(action, nameof(action));

            T
#if CS9
                        ?
#endif
                result = default;

            _For(values, (in T value) => action(ref result, value));

            return result;
        }

        public static T
#if CS9
                        ?
#endif
                For<T>(this IReadOnlyList<T> values, ForActionIn<T> action)
        {
            ThrowIfNull(values, nameof(values));
            ThrowIfNull(action, nameof(action));

            T
#if CS9
                        ?
#endif
                result = default;

            _For(values, (in T value) => action(ref result, value));

            return result;
        }
#endif

#if CS11
        private static void And<T>(ref T result, in T b) where T : struct, IBitwiseOperators<T, T, T> => result &= b;
        private static void Or<T>(ref T result, in T b) where T : struct, IBitwiseOperators<T, T, T> => result |= b;
        private static void XOr<T>(ref T result, in T b) where T : struct, IBitwiseOperators<T, T, T> => result ^= b;

        public static T And<T>(this IEnumerable<T> bytes) where T : struct, IBitwiseOperators<T, T, T> => bytes.ForEach(And);
        public static T Or<T>(this IEnumerable<T> bytes) where T : struct, IBitwiseOperators<T, T, T> => bytes.ForEach(Or);
        public static T XOr<T>(this IEnumerable<T> bytes) where T : struct, IBitwiseOperators<T, T, T> => bytes.ForEach(XOr);

        public static T And<T>(this IReadOnlyList<T> bytes) where T : struct, IBitwiseOperators<T, T, T> => bytes.For(And);
        public static T Or<T>(this IReadOnlyList<T> bytes) where T : struct, IBitwiseOperators<T, T, T> => bytes.For(Or);
        public static T XOr<T>(this IReadOnlyList<T> bytes) where T : struct, IBitwiseOperators<T, T, T> => bytes.For(XOr);
#endif

        public static Func<object, bool> Reverse(this Func<object, bool> func) => obj => !func(obj);
        public static Predicate Reverse(this Predicate predicate) => obj => !predicate(obj);

        public static Func<T, bool> Reverse<T>(this Func<T, bool> func) => obj => !func(obj);
        public static Predicate<T> Reverse<T>(this Predicate<T> predicate) => obj => !predicate(obj);

        public static void PerformAllActions(this IEnumerable<Action> actions)
        {
            foreach (Action action in actions)

                action();
        }

        public static string ToString<TKey, TValue>(this KeyValuePair<TKey, TValue> keyValuePair, in object middle, in bool replaceNull = false)
        {
            Converter<object
#if CS8
                    ?
#endif
                , string
#if CS8
                    ?
#endif
                > converter = replaceNull ? value => value == null ? "<Null>" : value.ToString() :
#if !CS9
                (Converter<object
#if CS8
                    ?
#endif
                , string
#if CS8
                    ?
#endif
                >)(
#endif
                value => value?.ToString()
#if !CS9
                )
#endif
                ;

            return $"{converter(keyValuePair.Key)}{converter(middle)}{converter(keyValuePair.Value)}";
        }

        public static Action<T> ToParameterizedAction<T>(this Action action) => value => action();

        public static Action ToConditionalAction(this Action action, bool condition) => () =>
        {
            if (condition)

                action();
        };

        public static Action ToConditionalAction(this Action action, Func<bool> func) => action.ToConditionalAction(func());

        public static Action<T> ToConditionalAction<T>(this Action<T> action, bool condition) => value =>
        {
            if (condition)

                action(value);
        };

        public static Action<T> ToConditionalAction<T>(this Action<T> action, Predicate<T> predicate) => value =>
        {
            if (predicate(value))

                action(value);
        };

        public static Action<T1, T2> ToConditionalAction2<T1, T2>(this Action<T1> action, Predicate<T2> predicate) => (T1 value1, T2 value2) =>
        {
            if (predicate(value2))

                action(value1);
        };

        public static Func<bool> ToBoolFunc(this Action action) => () =>
        {
            action();

            return true;
        };

#if !CS9
        public static string GetName<T>(this T value) where T : Enum => Enum.GetName(typeof(T), value);

        public static string Format<T>(this T value, in string format) where T : Enum => Enum.Format(typeof(T), value, format);

        public static bool IsDefined<T>(this T value) where T : Enum => Enum.IsDefined(typeof(T), value);

        public static T Parse<T>(this string value) where T : Enum => (T)Enum.Parse(typeof(T), value);

        public static T Parse<T>(this string value, in bool ignoreCase) where T : Enum => (T)Enum.Parse(typeof(T), value, ignoreCase);

        public static T ToObject<T>(this object value) where T : Enum => (T)Enum.ToObject(typeof(T), value);

        public static T ToObject<T>(this byte value) where T : Enum => (T)Enum.ToObject(typeof(T), value);

        public static T ToObject<T>(this sbyte value) where T : Enum => (T)Enum.ToObject(typeof(T), value);

        public static T ToObject<T>(this short value) where T : Enum => (T)Enum.ToObject(typeof(T), value);

        public static T ToObject<T>(this ushort value) where T : Enum => (T)Enum.ToObject(typeof(T), value);

        public static T ToObject<T>(this int value) where T : Enum => (T)Enum.ToObject(typeof(T), value);

        public static T ToObject<T>(this uint value) where T : Enum => (T)Enum.ToObject(typeof(T), value);

        public static T ToObject<T>(this long value) where T : Enum => (T)Enum.ToObject(typeof(T), value);

        public static T ToObject<T>(this ulong value) where T : Enum => (T)Enum.ToObject(typeof(T), value);
#endif

        public static Predicate<object> AsObjectPredicate(this Predicate predicate) => GetOrThrowIfNull<Predicate, Predicate<object>>(predicate, nameof(predicate), () => obj => predicate(obj));

        public static Func<object> AsObjectFunc(this Func func) => GetOrThrowIfNull<Func, Func<object>>(func, nameof(func), () => () => func());

        public static EqualityComparison<object> AsObjectEqualityComparison(this EqualityComparison comparison) => GetOrThrowIfNull<EqualityComparison, EqualityComparison<object>>(comparison, nameof(comparison), () => (object x, object y) => comparison(x, y));

        public static Comparison<object> AsObjectComparison(this Comparison comparison) => GetOrThrowIfNull<Comparison, Comparison<object>>(comparison, nameof(comparison), () => (object x, object y) => comparison(x, y));

        public static Predicate GetPredicate(this EqualityComparison comparison, object obj) => GetOrThrowIfNull(comparison, nameof(comparison), () => GetOrThrowIfNull<object, Predicate>(obj, nameof(obj), () => _obj => comparison(obj, _obj)));

        public static Predicate<T> GetPredicate<T>(this EqualityComparison<T> comparison, T obj) => GetOrThrowIfNull(comparison, nameof(comparison), () => GetOrThrowIfNull<object, Predicate<T>>(obj, nameof(obj), () => _obj => comparison(obj, _obj)));

        public static bool IsConstant(this FieldInfo f) => f.IsStatic && f.IsLiteral && !f.IsInitOnly;

        public static IEnumerable<FieldInfo> GetConstants(this Type type) => type.GetFields().Where(IsConstant);
        public static IEnumerable<FieldInfo> GetActualFields(this Type type) => type.GetFields().Where(f => !f.IsConstant());
#if !CS4
        [Obsolete("Replaced with GetActualFields method.")]
        public static IEnumerable<FieldInfo> GetRealFields(this Type type) => type.GetActualFields();
#endif
        public static string GetRealName(this Type type) => type.GetRealGenericTypeParameterLength() == 0 ? type.Name : type.Name.Remove(type.Name.IndexOf('`'));

        public static byte GetRealGenericTypeParameterLength(this Type type)
        {
            if (type.GetGenericArguments().Length == 0)

                return new byte();

            int i = type.Name.IndexOf('`');

            return i > -1 ? byte.Parse(type.Name.
#if CS8
                AsSpan
#else
                Substring
#endif
                (i + 1)) : new byte();
        }

        public static bool ContainsRealGenericParameters(this Type type) => type.GetRealGenericTypeParameterLength() > 0;

        public static bool IsStruct(this Type type) => type.IsValueType && !type.IsEnum;
        public static bool IsReferenceType(this Type type) => type.IsClass || type.IsInterface;
        public static bool IsPublicType(this Type type) => type.IsPublic || type.IsNestedPublic;
        public static bool IsTypeNestedFamily(this Type type) => type.IsNestedFamily || type.IsNestedFamORAssem;

        public static ConstructorInfo GetTypeConstructor(this Type type, params Type[] types) => type.GetConstructor(types);
        public static ConstructorInfo
#if CS8
                ?
#endif
            TryGetConstructor(this Type t, params Type[] types) => t.GetConstructor(types);

        public static ConstructorInfo AssertGetConstructor(this Type t, params Type[] types) => t.TryGetConstructor(types) ?? throw new InvalidOperationException("There is no such constructor for this type.");

        public static object
#if CS8
                ?
#endif
            AsObject(this object
#if CS8
                ?
#endif
            obj) => obj;

        public static U AsFromType<T, U>(this T obj) where T : U => obj;
        public static T AsFromType<T>(this T obj) => obj;
#if CS5
        public static IReadOnlyList<T> AsReadOnlyList<T>(this T[] values) => UtilHelpers.AsReadOnlyList(values);
#endif

#if CS5
        public static T First<T>(this IReadOnlyList<T> list) => list[0];

        public static T Last<T>(this IReadOnlyList<T> list) => list[list.Count - 1];
#endif
#if CS11
        public static bool HasFlag<T>(this T value, in T flag) where T : IBitwiseOperators<T, T, T>, IEquatable<T> => (value & flag).Equals(flag);
#else
        public static bool HasFlag(this byte value, in byte flag) => (value & flag) == flag;

        public static bool HasFlag(this sbyte value, in sbyte flag) => (value & flag) == flag;

        public static bool HasFlag(this short value, in short flag) => (value & flag) == flag;

        public static bool HasFlag(this ushort value, in ushort flag) => (value & flag) == flag;

        public static bool HasFlag(this int value, in int flag) => (value & flag) == flag;

        public static bool HasFlag(this uint value, in uint flag) => (value & flag) == flag;

        public static bool HasFlag(this long value, in long flag) => (value & flag) == flag;

        public static bool HasFlag(this ulong value, in ulong flag) => (value & flag) == flag;
#endif
        public static void ForEach<T>(this IEnumerable<T> enumerable, in Action<T> action)
        {
            foreach (T item in enumerable)

                action(item);
        }

#if CS5
        public static T
#if CS9
                    ?
#endif
            ForEach<T>(this IEnumerable<T> enumerable, in ForAction<T> action)
        {
            T
#if CS9
                    ?
#endif
            result = default;

            foreach (T item in enumerable)

                action(ref result, item);

            return result;
        }
#endif
        public static void ForEach<T>(this IEnumerable<T> enumerable, in Func<T, bool> predicate, in Action<T> action)
        {
            foreach (T item in enumerable)

                if (predicate(item))

                    action(item);
        }

        public static void ForEachPredicate<T>(this IEnumerable<T> enumerable, in Predicate<T> predicate, in Action<T> action)
        {
            foreach (T item in enumerable)

                if (predicate(item))

                    action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, in ActionIn<T> action)
        {
            foreach (T item in enumerable)

                action(item);
        }

#if CS5
        public static T
#if CS9
                    ?
#endif
            ForEach<T>(this IEnumerable<T> enumerable, in ForActionIn<T> action)
        {
            T
#if CS9
                    ?
#endif
            result = default;

            foreach (T item in enumerable)

                action(ref result, item);

            return result;
        }
#endif
        public static void ForEach<T>(this IEnumerable<T> enumerable, in FuncIn<T, bool> predicate, in ActionIn<T> action)
        {
            foreach (T item in enumerable)

                if (predicate(item))

                    action(item);
        }

        public static void ForEachPredicate<T>(this IEnumerable<T> enumerable, in PredicateIn<T> predicate, in ActionIn<T> action)
        {
            foreach (T item in enumerable)

                if (predicate(item))

                    action(item);
        }
#if !CS5
        internal static Type _GetEnumUnderlyingType(this Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return fields.Length > 0 ? fields[0].FieldType : null;
        }

        public static Type GetEnumUnderlyingType(this Type type) => type.IsEnum ? type._GetEnumUnderlyingType() : null;

        public static Type GetEnumUnderlyingType<T>(this T value) where T : Enum => UtilHelpers.GetEnumUnderlyingType<T>();

        public static bool HasFlag<T>(this T value, in T flag) where T : Enum
        {
            if (value == null ? throw GetArgumentNullException(nameof(value)) : flag == null ? throw GetArgumentNullException(nameof(flag)) : typeof(T).GetCustomAttributes(false).FirstOrDefault(a => a is FlagsAttribute) == null)

                return false;

            object _value = value.GetNumValue();

            return _value is int @int
                ? @int.HasFlag((int)flag.GetNumValue())
                : _value is uint @uint
                ? @uint.HasFlag((uint)flag.GetNumValue())
                : _value is short @short
                ? @short.HasFlag((short)flag.GetNumValue())
                : _value is ushort @ushort
                ? @ushort.HasFlag((ushort)flag.GetNumValue())
                : _value is long @long
                ? @long.HasFlag((long)flag.GetNumValue())
                : _value is ulong @ulong
                ? @ulong.HasFlag((ulong)flag.GetNumValue())
                : _value is byte @byte
                ? @byte.HasFlag((byte)flag.GetNumValue())
                : ((sbyte)_value).HasFlag((sbyte)flag.GetNumValue());
        }
#endif
        public static string Truncate(this string s, in int length) => s.Substring(0, length);

        // TODO: add custom index checks

        public static string Truncate(this string s, in char c) => s.Truncate(s.IndexOf(c));

        public static string Truncate(this string s, in string str) => s.Truncate(s.IndexOf(str));

        public static string TryTruncate(this string s, in int index) => index > -1 ? s.Truncate(index) : null;

        public static string TryTruncate(this string s, in char c) => s.TryTruncate(s.IndexOf(c));

        public static string TryTruncate(this string s, in string str) => s.TryTruncate(s.IndexOf(str));

        public static string TryTruncateOrOriginal(this string s, in int index) => index > -1 ? s.Truncate(index) : s;

        public static string TryTruncateOrOriginal(this string s, in char c) => s.TryTruncateOrOriginal(s.IndexOf(c));

        public static string TryTruncateOrOriginal(this string s, in string str) => s.TryTruncateOrOriginal(s.IndexOf(str));

        public static string TruncateL(this string s, in char c) => s.Truncate(s.LastIndexOf(c));

        public static string TruncateL(this string s, in string str) => s.Truncate(s.LastIndexOf(str));

        public static string TryTruncateL(this string s, in char c) => s.TryTruncate(s.LastIndexOf(c));

        public static string TryTruncateL(this string s, in string str) => s.TryTruncate(s.LastIndexOf(str));

        public static string TryTruncateOrOriginalL(this string s, in char c) => s.TryTruncateOrOriginal(s.LastIndexOf(c));

        public static string TryTruncateOrOriginalL(this string s, in string str) => s.TryTruncateOrOriginal(s.LastIndexOf(str));

        public static string Truncate(this string s, in int index, in string replace) => s == null ? throw GetArgumentNullException(nameof(s)) : index.Between(0, s.Length, true, false) ? $"{s.Remove(index)}{replace}" : throw new IndexOutOfRangeException(nameof(index));

        public static string Truncate2(this string s, in int index, in string replace) => s.Truncate(index - Etcetera.Length, replace);

        public static string Truncate2(this string s, in int index) => s.Truncate2(index, Etcetera);
#if CS6
        public static List<T> ToList<T>(this IReadOnlyList<T> list)
        {
            var _list = new List<T>(list.Count);

            for (int i = 0; i < list.Count; i++)

                _list.Add(list[i]);

            return _list;
        }
#endif
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<T> enumerable)
        {
            foreach (T item in enumerable)

                yield return item;
        }

        public static IEnumerable<TDestination> ToEnumerable<TSource, TDestination>(this IEnumerable<TSource> enumerable) where TSource : TDestination
        {
            foreach (TSource item in enumerable)

                yield return item;
        }
        public static bool ForEachANDALSO<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            foreach (T item in enumerable)

                if (!predicate(item))

                    return false;

            return true;
        }

        public static bool ForEachAND<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            bool result = true;

            foreach (T item in enumerable)

                result &= predicate(item);

            return result;
        }

        public static bool ForEachORELSE<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            foreach (T item in enumerable)

                if (predicate(item))

                    return true;

            return false;
        }

        public static bool ForEachOR<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            bool result = false;

            foreach (T item in enumerable)

                result |= predicate(item);

            return result;
        }

        public static bool ForEachXORELSE<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            bool result = false;

            enumerable.ForEach(item => result = predicate(item), item =>
            {
                if (predicate(item))

                    if (result)

                        return (result = false);

                    else

                        result = true;

                return true;
            });

            return result;
        }

        public static bool ForEachXOR<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            bool result = false;

            Action<T> action = item =>
            {
                if (predicate(item))

                    if (result)
                    {
                        result = false;

                        action = _item => predicate(_item);
                    }

                    else

                        result = true;
            };

            enumerable.ForEach(item => result = predicate(item), action);

            return result;
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, in Predicate<T> firstAction, Predicate<T> otherAction)
        {
            IEnumerator<T> enumerator = enumerable.GetEnumerator();

            try
            {
                if (enumerator.MoveNext())

                    if (firstAction(enumerator.Current))

                        while (enumerator.MoveNext() && otherAction(enumerator.Current))
                        {
                            // Left empty.
                        }
            }

            finally
            {
                enumerator.Dispose();
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, in Action<T> firstAction, Predicate<T> otherAction)
        {
            IEnumerator<T> enumerator = enumerable.GetEnumerator();

            try
            {
                if (enumerator.MoveNext())
                {
                    firstAction(enumerator.Current);

                    while (enumerator.MoveNext() && otherAction(enumerator.Current))
                    {
                        // Left empty.
                    }
                }
            }

            finally
            {
                enumerator.Dispose();
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, in Action<T> firstAction, Action<T> otherAction)
        {
            IEnumerator<T> enumerator = enumerable.GetEnumerator();

            try
            {
                if (enumerator.MoveNext())
                {
                    firstAction(enumerator.Current);

                    while (enumerator.MoveNext())

                        otherAction(enumerator.Current);
                }
            }

            finally
            {
                enumerator.Dispose();
            }
        }

        public static T GetIfNotDisposed<T>(this DotNetFix.IDisposable obj, in T value) => obj.IsDisposed ? throw GetExceptionForDispose(false) : value;

        public static T GetIfNotDisposedOrDisposing<T>(this IDisposable obj, in T value) => obj.IsDisposed ? throw GetExceptionForDispose(false) : obj.IsDisposing ? throw GetExceptionForDispose(true) : value;

        private static void ActionFromLast<T>(this T array, in int indexation, in Func<int> func, in ActionIn<int> action)
        {
            ThrowIfNull(array, nameof(array));

            int count = func();

            (count-- > indexation ? action : throw new ArgumentOutOfRangeException(nameof(indexation), indexation, $"{nameof(indexation)} must be less than or equal to {nameof(array)}'s length."))(count - indexation);
        }

        private static TItems
#if CS9
                ?
#endif
            GetFromLast<TArray, TItems>(this TArray array, in int indexation, in Func<int> func, Func<Converter<int, TItems>> _func)
        {
            TItems
#if CS9
                ?
#endif
            value = default;

            array.ActionFromLast(indexation, func, (in int i) => value = _func()(i));

            return value;
        }

        public static object GetFromLast(this System.Array array, int indexation) => array.GetFromLast<System.Array, object>(indexation, () => array.Length, () => array.GetValue);
        public static void SetFromLast(this System.Array array, object value, int indexation) => array.ActionFromLast(indexation, () => array.Length, (in int i) => array.SetValue(value, i));

        public static object GetLast(this System.Array array) => array.GetFromLast(0);
        public static void SetLast(this System.Array array, object value) => array.SetFromLast(value, 0);

        public static T GetFromLast<T>(this T[] array, int indexation)
#if CS5
        => array.AsFromType<IReadOnlyList<T>>().GetFromLast(indexation);

        public static T GetFromLast<T>(this IReadOnlyList<T> array, int indexation)
#endif
                => array.GetFromLast<
#if CS5
                IReadOnlyList<T>
#else
                        T[]
#endif
                    , T>(indexation, () => array.
#if CS5
                Count
#else
                        Length
#endif
                    , () => i => array[i]);

        public static void SetFromLast<T>(this T[] array, T value, int indexation) => array.ActionFromLast(indexation, () => array.Length, (in int i) => array[i] = value);
        public static void SetFromLast<T>(this IList<T> values, T value, int indexation) => values.ActionFromLast(indexation, () => values.Count, (in int i) => values[i] = value);

        public static T GetLast<T>(this T[] array) => array.GetFromLast(0);
#if CS5
        public static T GetLast<T>(this IReadOnlyList<T> array) => array.GetFromLast(0);
#endif

        public static void SetLast<T>(this T[] array, T value) => array.SetFromLast(value, 0);
        public static void SetLast<T>(this IList<T> values, T value) => values.SetFromLast(value, 0);

        public static Result ToResultEnum(this bool? value) => value.HasValue ? value.Value.ToResultEnum() : Result.None;

        public static Result ToResultEnum(this bool value) => value ? Result.True : Result.False;

        public static XOrResult ToXOrResult(this bool value) => value ? XOrResult.OneTrueResult : XOrResult.NoTrueResult;

        public static bool ToBool(this bool? value) => value.HasValue && value.Value;

        public static bool ToBoolIgnoreNull(this bool? value) => value ?? true;



        public static bool AndAlso(this bool? value, bool other) => ToBool(value) && other;

        public static bool AndAlso(this bool? value, Func<bool> other) => ToBool(value) && other();

        public static bool AndAlso(this bool? value, bool? other) => ToBool(value) && ToBool(other);

        public static bool AndAlso(this bool? value, Func<bool?> other) => ToBool(value) && ToBool(other());

        public static bool AndAlsoIgnoreNull(this bool? value, bool other) => value.HasValue ? value.Value && other : other;

        public static bool AndAlsoIgnoreNull(this bool? value, Func<bool> other) => value.HasValue ? value.Value && other() : other();

        public static bool AndAlsoIgnoreNull(this bool? value, bool? other) => value.HasValue
            ? other.HasValue
                ? value.AndAlsoIgnoreNull(other.Value)
                : value.Value
            : other ?? false;

        public static bool AndAlsoIgnoreNull(this bool? value, Func<bool?> other)
        {
            bool? _other = other();

            return value.HasValue
                ? _other.HasValue
                    ? value.AndAlsoIgnoreNull(_other.Value)
                    : value.Value
                : _other ?? false;
        }



        public static bool OrElse(this bool? value, bool other) => value.HasValue && (value.Value || other);

        public static bool OrElse(this bool? value, Func<bool> other) => value.HasValue && (value.Value || other());

        public static bool OrElse(this bool? value, bool? other) => value.HasValue && other.HasValue && (value.Value || other.Value);

        public static bool OrElse(this bool? value, Func<bool?> other)
        {
            bool? _other = other();

            return value.HasValue && _other.HasValue && (value.Value || _other.Value);
        }

        public static bool OrElseIgnoreNull(this bool? value, bool other) => value.HasValue ? value.Value || other : other;

        public static bool OrElseIgnoreNull(this bool? value, Func<bool> other) => value.HasValue ? value.Value || other() : other();

        public static bool OrElseIgnoreValue(this bool? value, bool? other) => value.HasValue
            ? other.HasValue
                ? value.Value || other.Value
                : value.Value
            : other ?? false;

        public static bool OrElseIgnoreValue(this bool? value, Func<bool?> other)
        {
            bool? _other = other();

            return value.HasValue
                ? _other.HasValue
                    ? value.Value || _other.Value
                    : value.Value
                : _other ?? false;
        }



        public static bool XOr(this bool? value, bool other) => value.HasValue && (value.Value ^ other);

        public static bool XOr(this bool? value, Func<bool> other) => value.HasValue && (value.Value ^ other());

        public static bool XOr(this bool? value, bool? other) => value.HasValue && other.HasValue && (value.Value ^ other.Value);

        public static bool XOr(this bool? value, Func<bool?> other)
        {
            bool? _other = other();

            return value.HasValue && _other.HasValue && (value.Value ^ _other.Value);
        }

        public static bool XOrIgnoreNull(this bool? value, bool other) => value.HasValue ? value.Value ^ other : other;

        public static bool XOrIgnoreNull(this bool? value, Func<bool> other) => value.HasValue ? value.Value ^ other() : other();

        public static bool XOrIgnoreValue(this bool? value, bool? other) => value.HasValue
            ? other.HasValue
                ? value.Value ^ other.Value
                : value.Value
            : other ?? false;

        public static bool XOrIgnoreValue(this bool? value, Func<bool?> other)
        {
            bool? _other = other();

            return value.HasValue
             ? _other.HasValue
                 ? value.Value ^ _other.Value
                 : value.Value
             : _other ?? false;
        }



        public static XOrResult GetXOrResult(this bool? value, bool other) => ToBool(value)
            ? other
                ? MoreThanOneTrueResult
                : OneTrueResult
            : other
                ? OneTrueResult
            : NoTrueResult;

        public static XOrResult GetXOrResult(this bool? value, Func<bool> other) => ToBool(value)
            ? other()
                ? MoreThanOneTrueResult
                : OneTrueResult
            : other()
                ? OneTrueResult
            : NoTrueResult;

        public static XOrResult GetXOrResult(this bool? value, bool? other) => ToBool(value)
            ? ToBool(other)
                ? MoreThanOneTrueResult
                : OneTrueResult
            : ToBool(other)
                ? OneTrueResult
                : NoTrueResult;

        public static XOrResult GetXOrResult(this bool? value, Func<bool?> other) => ToBool(value)
            ? ToBool(other())
                ? MoreThanOneTrueResult
                : OneTrueResult
            : ToBool(other())
                ? OneTrueResult
                : NoTrueResult;

        public static XOrResult GetXOrResultIgnoreNull(this bool? value, bool other) => ToBool(value)
            ? other
                ? MoreThanOneTrueResult
                : OneTrueResult
            : other
                ? OneTrueResult
                : NoTrueResult;

        public static XOrResult GetXOrResultIgnoreNull(this bool? value, Func<bool> other) => ToBool(value)
            ? other()
                ? MoreThanOneTrueResult
                : OneTrueResult
            : other()
                ? OneTrueResult
                : NoTrueResult;

        public static XOrResult GetXOrResultIgnoreValue(this bool? value, bool? other) => ToBool(value)
            ? ToBool(other)
                ? MoreThanOneTrueResult
                : OneTrueResult
            : ToBool(other)
                ? OneTrueResult
                : NoTrueResult;

        public static XOrResult GetXOrResultIgnoreValue(this bool? value, Func<bool?> other) => ToBool(value)
            ? ToBool(other())
                ? MoreThanOneTrueResult
                : OneTrueResult
            : ToBool(other())
                ? OneTrueResult
                : NoTrueResult;

        public static void CopyTo(this BitArray source, in BitArray array, in int startIndex)
        {
            ThrowIfNull(source, nameof(source));
            ThrowIfNull(array, nameof(array));

            for (int i = array.Length > source.Length
                    ? throw new ArgumentOutOfRangeException(nameof(array))
                    : startIndex < 0 || startIndex + array.Length > source.Length
                    ? throw new IndexOutOfRangeException($"{nameof(startIndex)} is out of range.")
                    : 0; i < source.Length; i++)

                array[startIndex + i] = source[i];
        }

        public static void SetMultipleBits(this BitArray array, in byte[] bytes, in int startIndex)
        {
            long length = bytes.Length * 8;

            new BitArray(length > array.Length
                ? throw new ArgumentOutOfRangeException(nameof(bytes))
                : startIndex < 0 || startIndex + length > array.Length
                ? throw new IndexOutOfRangeException($"{nameof(startIndex)} is out of range.")
                : bytes).CopyTo(array, startIndex);
        }

        public static void SetMultipleBits(this BitArray array, in BitArray bits, in int startIndex) => new BitArray(bits.Length > array.Length
            ? throw new ArgumentOutOfRangeException(nameof(bits))
            : startIndex < 0 || startIndex + bits.Length > array.Length
            ? throw new IndexOutOfRangeException($"{nameof(startIndex)} is out of range.")
            : bits).CopyTo(array, startIndex);

        public static bool And<T>(this IEnumerable<T> enumerable, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T item in enumerable)

                if (!predicate(item))

                    return false;

            return true;
        }

        public static bool Or<T>(this IEnumerable<T> enumerable, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T item in enumerable)

                if (predicate(item))

                    return true;

            return false;
        }

        public static bool XOr<T>(this IEnumerable<T> enumerable, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            bool found = false;

            foreach (T item in enumerable)

                if (predicate(item))
                {
                    if (found)

                        return false;

                    found = true;
                }

            return found;
        }

        public static XOrResult XOrAsXOrResult<T>(this IEnumerable<T> enumerable, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            bool found = false;

            foreach (T item in enumerable)

                if (predicate(item))
                {
                    if (found)

                        return MoreThanOneTrueResult;

                    found = true;
                }

            return found.ToXOrResult();
        }

        public static bool Is(this object
#if CS8
                ?
#endif
            obj, in bool typeEquality, in IEnumerable<Type> types) => obj != null && (types ?? throw GetArgumentNullException(nameof(types))).Or(GetTypePredicate(obj, typeEquality));

        /// <summary>
        /// Checks if the current object is assignable from at least one type of a given <see cref="Type"/> array.
        /// </summary>
        /// <param name="obj">The object from which check the type</param>
        /// <param name="typeEquality"><see langword="true"/> to preserve type equality, regardless of the type inheritance, otherwise <see langword="false"/></param>
        /// <param name="types">The types to compare</param>
        /// <returns><see langword="true"/> if the current object is assignable from at least one of the given types, otherwise <see langword="false"/>.</returns>
        public static bool Is(this object
#if CS8
                ?
#endif
            obj, in bool typeEquality, params Type[] types) => obj.Is(typeEquality, types.AsEnumerable());

        public static bool IsAND(this object
#if CS8
                ?
#endif
            obj, in IEnumerable<Type> types) => obj != null && (types ?? throw GetArgumentNullException(nameof(types))).And(GetTypePredicate(obj, false));

        public static bool IsAND(this object
#if CS8
                ?
#endif
            obj, params Type[] types) => obj.IsAND(types.AsEnumerable());

        public static bool IsXOR(this object
#if CS8
                ?
#endif
            obj, in IEnumerable<Type> types) => obj != null && (types ?? throw GetArgumentNullException(nameof(types))).XOr(GetTypePredicate(obj, false));

        public static bool IsXOR(this object
#if CS8
                ?
#endif
            obj, params Type[] types) => obj.IsXOR(types.AsEnumerable());

        public static bool IsType(this Type
#if CS8
                ?
#endif
            t, in bool typeEquality, in IEnumerable<Type> types) => t != null && (types ?? throw GetArgumentNullException(nameof(types))).Or(GetTypePredicate(t, typeEquality));

        public static bool IsType(this Type
#if CS8
                ?
#endif
            t, in bool typeEquality, params Type[] types) => IsType(t, typeEquality, types.AsEnumerable());

        public static bool IsAssignableFrom<T>(this Type t) => (t ?? throw GetArgumentNullException(nameof(t))).IsAssignableFrom(typeof(T));
        /*{
            ThrowIfNull(t, nameof(t));

            Type from = typeof(T);

            if (from == t)

                return true;

            if (t.IsInterface)

                return t.IsType(from.GetInterfaces());

            if (from.IsInterface)

                return false;

            from = from.BaseType;

            while (from != null)
            {
                if (from == t)

                    return true;

                from = from.BaseType;
            }

            return false;
        }*/

#if !CS9
        public static bool IsAssignableTo(this Type t, in Type type) => type.IsAssignableFrom(t);
#endif

        public static bool IsAssignableTo<T>(this Type t) => (t ?? throw GetArgumentNullException(nameof(t))).IsAssignableTo(typeof(T));

        public static bool IsAssignableFrom(this Type t, in IEnumerable<Type> enumerable) => (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).Or((t ?? throw GetArgumentNullException(nameof(t))).IsAssignableFrom);

        public static bool IsAssignableFromAND(this Type t, in IEnumerable<Type> enumerable)
        {
            ThrowIfNull(t, nameof(t));

            return (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).And((t ?? throw GetArgumentNullException(nameof(t))).IsAssignableFrom);
        }

        public static bool IsAssignableFromXOr(this Type t, in IEnumerable<Type> enumerable)
        {
            ThrowIfNull(t, nameof(t));

            return (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).XOr((t ?? throw GetArgumentNullException(nameof(t))).IsAssignableFrom);
        }

        public static IEnumerable<TKey> GetKeys<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)
        {
            ThrowIfNull(array, nameof(array));

            foreach (KeyValuePair<TKey, TValue> value in array)

                yield return value.Key;
        }

        public static IEnumerable<TValue> GetValues<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)
        {
            ThrowIfNull(array, nameof(array));

            foreach (KeyValuePair<TKey, TValue> value in array)

                yield return value.Value;
        }

        public static bool CheckIntegrity<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)
        {
#if CS8
            static
#endif

            bool predicateByVal(TKey keyA, TKey keyB) => Equals(keyA, keyB);

#if CS8
            static
#endif

            bool predicateByRef(TKey keyA, TKey keyB) => ReferenceEquals(keyA, keyB);

            Func<TKey, TKey, bool> predicate = typeof(TKey).IsValueType ? predicateByVal :
#if !CS9
                (Func<TKey, TKey, bool>)
#endif
                predicateByRef;

            IEnumerable<TKey> keys = array.GetKeys();

            IEnumerable<TKey> _keys = array.GetKeys();

            bool foundOneOccurrence = false;

            foreach (TKey key in keys)
            {
                if (key == null)

                    throw GetOneOrMoreKeyIsNullException();

                foreach (TKey _key in _keys)
                {
                    if (predicate(key, _key))

                        if (foundOneOccurrence)

                            return false;

                        else

                            foundOneOccurrence = true;
                }

                foundOneOccurrence = false;
            }

            return true;
        }

        public static bool CheckPropertySetIntegrity(Type propertyObjectType, in string propertyName, out string methodName, in int skipFrames, in BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet)
        {
            ThrowIfNull(propertyObjectType, nameof(propertyObjectType));

            PropertyInfo property = propertyObjectType.GetProperty(propertyName, bindingFlags) ?? throw GetFieldOrPropertyNotFoundException(propertyName, propertyObjectType);

            MethodBase
#if CS8
                    ?
#endif
                method = new StackFrame(skipFrames).GetMethod();

            methodName = method?.Name;

            //#if DEBUG 

            //            Debug.WriteLine("Property: " + property.Name + ", " + property.DeclaringType);

            //            Debug.WriteLine("Method: " + method.Name + ", " + method.DeclaringType);

            //#endif 

            // todo: tuple and check DeclaringTypeNotCorrespond throws

            return (property.CanWrite && property.GetSetMethod() != null) || (method == null ? false : property.DeclaringType == method.DeclaringType);
        }

        internal static FieldInfo GetField(in string fieldName, in Type objectType, in BindingFlags bindingFlags) => objectType.GetField(fieldName, bindingFlags) ?? throw GetFieldOrPropertyNotFoundException(fieldName, objectType);

        internal static PropertyInfo GetProperty(in string propertyName, in Type objectType, in BindingFlags bindingFlags) => objectType.GetProperty(propertyName, bindingFlags) ?? throw GetFieldOrPropertyNotFoundException(propertyName, objectType);

        // todo: use attributes

#if CS7
        private static (bool fieldChanged, object oldValue) SetField(this object obj, in FieldInfo field, in object previousValue, in object newValue, in string paramName, in bool setOnlyIfNotNull, in bool throwIfNull, in bool disposeOldValue, in FieldValidateValueCallback validateValueCallback, in bool throwIfValidationFails, in FieldValueChangedCallback valueChangedCallback)
        {
            if (newValue is null)

                if (throwIfNull)

                    throw GetArgumentNullException(paramName);

                else if (setOnlyIfNotNull)

                    return (false, previousValue);

            (bool validationResult, Exception validationException) = validateValueCallback?.Invoke(obj, newValue, field, paramName) ?? (true, null);

            if (validationResult)

                if ((newValue == null && previousValue != null) || (newValue != null && !newValue.Equals(previousValue)))
                {
                    if (disposeOldValue)

                        ((IDisposable)previousValue).Dispose();

                    field.SetValue(obj, newValue);

                    valueChangedCallback?.Invoke(obj, newValue, field, paramName);

                    return (true, previousValue);

                    //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                    //             BindingFlags.Static | BindingFlags.Instance |
                    //             BindingFlags.DeclaredOnly;
                    //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);
                }

                else

                    return (false, previousValue);

            else

                return throwIfValidationFails ? throw (validationException ?? new ArgumentException("Validation error.", paramName)) : (false, previousValue);
        }

        public static (bool fieldChanged, object oldValue) SetField(this object obj, in string fieldName, in object newValue, in Type declaringType, in BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, in string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, in FieldValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, in FieldValueChangedCallback valueChangedCallback = null)
        {
            ThrowIfNull(declaringType, nameof(declaringType));

            FieldInfo field = GetField(fieldName, declaringType, bindingFlags);

            return obj.SetField(field, field.GetValue(obj), newValue, paramName, setOnlyIfNotNull, throwIfNull, false, validateValueCallback, throwIfValidationFails, valueChangedCallback);
        }

        public static (bool fieldChanged, IDisposable oldValue) DisposeAndSetField(this object obj, in string fieldName, in IDisposable newValue, in Type declaringType, in BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, in string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, in FieldValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, in FieldValueChangedCallback valueChangedCallback = null)
        {
            ThrowIfNull(declaringType, nameof(declaringType));

            FieldInfo field = GetField(fieldName, declaringType, bindingFlags);

            return ((bool, IDisposable))obj.SetField(field, field.GetValue(obj), newValue, paramName, setOnlyIfNotNull, throwIfNull, true, validateValueCallback, throwIfValidationFails, valueChangedCallback);
        }

        // todo: update code (in, throw if null)

        /// <summary>
        /// Sets a value to a property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="fieldName">The field related to the property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The declaring types of the given property and field name doesn't correspond. OR The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged, object oldValue) SetProperty(this object obj, string propertyName, string fieldName, object newValue, Type declaringType, in bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null)
        {
            //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
            //             BindingFlags.Static | BindingFlags.Instance |
            //             BindingFlags.DeclaredOnly;
            //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName, previousValue, newValue)); 

            // if (declaringType == null) 

            // {

            //while (objectType != declaringType && objectType != typeof(object))

            //    objectType = objectType.BaseType;

            //if (objectType != declaringType)

            //    throw new ArgumentException(string.Format((string)ResourcesHelper.GetResource("DeclaringTypeIsNotInObjectInheritanceHierarchyException"), declaringType, objectType));

            // }

            //#if DEBUG

            //            var fields = objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            //            foreach (var _field in fields)

            //                Debug.WriteLine("Object type: " + objectType + " " + _field.Name);

            //#endif

            // var objectType = obj.GetType();

            FieldInfo field = GetField(fieldName, declaringType, bindingFlags);

            object previousValue = field.GetValue(obj);

            if (!CheckPropertySetIntegrity(declaringType, propertyName, out string methodName, 3, bindingFlags))

                throw GetDeclaringTypesNotCorrespondException(propertyName, methodName);

            PropertyInfo property = GetProperty(propertyName, declaringType, bindingFlags);

            return !property.CanWrite || property.SetMethod == null
                ? throwIfReadOnly ? throw new InvalidOperationException(string.Format("This property is read-only. Property name: {0}, declaring type: {1}.", propertyName, declaringType)) : (false, previousValue)
                : obj.SetField(field, previousValue, newValue, paramName, setOnlyIfNotNull, throwIfNull, false, validateValueCallback, throwIfValidationFails, valueChangedCallback);
        }

        /// <summary>
        /// Sets a value to a property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged, object oldValue) SetProperty(this object obj, string propertyName, object newValue, Type declaringType, in bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null)
        {
            PropertyInfo property = GetProperty(propertyName, declaringType, bindingFlags);

            object previousValue = property.GetValue(obj);

            if (!property.CanWrite || property.SetMethod == null)

                return throwIfReadOnly ? throw new InvalidOperationException(string.Format("This property is read-only. Property name: {0}, declaring type: {1}.", propertyName, declaringType)) : (false, previousValue);

            if (newValue is null)

                if (throwIfNull)

                    throw GetArgumentNullException(paramName);

                else if (setOnlyIfNotNull)

                    return (false, previousValue);

            (bool validationResult, Exception validationException) = validateValueCallback?.Invoke(obj, newValue, property, paramName) ?? (true, null);

            if (validationResult)

                if ((newValue == null && previousValue != null) || (newValue != null && !newValue.Equals(previousValue)))
                {
                    property.SetValue(obj, newValue);

                    valueChangedCallback?.Invoke(obj, newValue, property, paramName);

                    return (true, previousValue);

                    //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                    //             BindingFlags.Static | BindingFlags.Instance |
                    //             BindingFlags.DeclaredOnly;
                    //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);
                }

                else

                    return (false, previousValue);

            else

                return throwIfValidationFails ? throw (validationException ?? new ArgumentException("Validation error.", paramName)) : (false, previousValue);
        }

        /// <summary>
        /// Disposes an old value of a property then sets a new value to the given property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="fieldName">The field related to the property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The declaring types of the given property and field name doesn't correspond. OR The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged, System.IDisposable oldValue) DisposeAndSetProperty(this object obj, string propertyName, string fieldName, System.IDisposable newValue, Type declaringType, in bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null)
        {
            //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
            //             BindingFlags.Static | BindingFlags.Instance |
            //             BindingFlags.DeclaredOnly;
            //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName, previousValue, newValue)); 

            // if (declaringType == null) 

            // {

            //while (objectType != declaringType && objectType != typeof(object))

            //    objectType = objectType.BaseType;

            //if (objectType != declaringType)

            //    throw new ArgumentException(string.Format((string)ResourcesHelper.GetResource("DeclaringTypeIsNotInObjectInheritanceHierarchyException"), declaringType, objectType));

            // }

            //#if DEBUG

            //            var fields = objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            //            foreach (var _field in fields)

            //                Debug.WriteLine("Object type: " + objectType + " " + _field.Name);

            //#endif

            // var objectType = obj.GetType();

            FieldInfo field = GetField(fieldName, declaringType, bindingFlags);

            var previousValue = (System.IDisposable)field.GetValue(obj);

            if (!CheckPropertySetIntegrity(declaringType, propertyName, out string methodName, 3, bindingFlags))

                throw GetDeclaringTypesNotCorrespondException(propertyName, methodName);

            PropertyInfo property = GetProperty(propertyName, declaringType, bindingFlags);

            return !property.CanWrite || property.SetMethod == null
                ? throwIfReadOnly ? throw new InvalidOperationException(string.Format("This property is read-only. Property name: {0}, declaring type: {1}.", propertyName, declaringType)) : (false, previousValue)
                : ((bool, System.IDisposable))obj.SetField(field, previousValue, newValue, paramName, setOnlyIfNotNull, throwIfNull, true, validateValueCallback, throwIfValidationFails, valueChangedCallback);
        }

        /// <summary>
        /// Disposes an old value of a property then sets a new value to the given property if the new value is different.
        /// </summary>
        /// <param name="obj">The object in which to set the property.</param>
        /// <param name="propertyName">The name of the given property.</param>
        /// <param name="newValue">The value to set.</param>
        /// <param name="declaringType">The actual declaring type of the property.</param>
        /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
        /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
        /// <param name="paramName">The parameter from which the value was passed to this method.</param>
        /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
        /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
        /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
        /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
        /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
        /// <exception cref="InvalidOperationException">The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
        public static (bool propertyChanged,System. IDisposable oldValue) DisposeAndSetProperty(this object obj, string propertyName, System.IDisposable newValue, Type declaringType, in bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null)
        {
            PropertyInfo property = GetProperty(propertyName, declaringType, bindingFlags);

            var previousValue = (System.IDisposable)property.GetValue(obj);

            if (!property.CanWrite || property.SetMethod == null)

                return throwIfReadOnly ? throw new InvalidOperationException(string.Format("This property is read-only. Property name: {0}, declaring type: {1}.", propertyName, declaringType)) : (false, previousValue);

            if (newValue is null)

                if (throwIfNull)

                    throw GetArgumentNullException(paramName);

                else if (setOnlyIfNotNull)

                    return (false, previousValue);

            (bool validationResult, Exception validationException) = validateValueCallback?.Invoke(obj, newValue, property, paramName) ?? (true, null);

            if (validationResult)

                if ((newValue == null && previousValue != null) || (newValue != null && !newValue.Equals(previousValue)))
                {
                    previousValue.Dispose();

                    property.SetValue(obj, newValue);

                    valueChangedCallback?.Invoke(obj, newValue, property, paramName);

                    return (true, previousValue);

                    //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                    //             BindingFlags.Static | BindingFlags.Instance |
                    //             BindingFlags.DeclaredOnly;
                    //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);
                }

                else

                    return (false, previousValue);

            else

                return throwIfValidationFails ? throw (validationException ?? new ArgumentException("Validation error.", paramName)) : (false, previousValue);
        }
#endif

        // todo: IFormatProvider

        /// <summary>
        /// Gets the numeric value for an enum.
        /// </summary>
        /// <param name="enum">The enum for which get the corresponding numeric value.</param>
        /// <returns>The numeric value corresponding to this enum, in the given enum type underlying type.</returns>
        public static object GetNumValue(this Enum @enum) => System.Convert.ChangeType(@enum, Enum.GetUnderlyingType(@enum.GetType()));

        /// <summary>
        /// Gets the numeric value for an enum.
        /// </summary>
        /// <param name="enum">The enum for which get the corresponding numeric value.</param>
        /// <returns>The numeric value corresponding to this enum, in the given enum type underlying type.</returns>
        public static object GetNumValue<T>(this T @enum) where T : Enum => System.Convert.ChangeType(@enum, Enum.GetUnderlyingType(typeof(T)));

        // public static object GetNumValue(this Enum @enum) => GetNumValue(@enum, @enum.ToString());

        // todo : test if Math.Log(Convert.ToInt64(flagsEnum), 2) == 'SomeInt64'; (no float, double ...) would be faster.

#if CS6
        ///// <summary>
        ///// Determines whether the current enum value is within the enum values range delimited by the first and the last fields; see the Remarks section for more information.
        ///// </summary>
        ///// <param name="enum">The enum value to check.</param>
        ///// <returns><see langword="true"/> if the given value is in the enum values range, otherwise <see langword="false"/>.</returns>
        ///// <remarks>This method doesn't read all the enum fields, but only takes care of the first and last numeric enum fields, so if the value is 1, and the enum has only defined fields for 0 and 2, this method still returns <see langword="true"/>. For a method that actually reads all the enum fields, see the <see cref="Type.IsEnumDefined(object)"/> method.</remarks>
        ///// <seealso cref="ThrowIfNotValidEnumValue(Enum)"/>
        ///// <seealso cref="ThrowIfNotDefinedEnumValue(Enum)"/>
        ///// <seealso cref="ThrowIfNotValidEnumValue(Enum, in string)"/>
        ///// <seealso cref="ThrowIfNotDefinedEnumValue(Enum,in string)"/>
        public static bool IsValidEnumValue(this Enum @enum)
        {
            var values = new ArrayList(@enum.GetType().GetEnumValues());

            values.Sort();

            // object _value = Convert.ChangeType(value, value.GetType().GetEnumUnderlyingType());

            return @enum.CompareTo(values[0]) >= 0 && @enum.CompareTo(values[
#if CS8
                    ^1
#else
        values.Count - 1
#endif
            ]) <= 0;
        }
#if CS7
        /// <summary>
        /// Determines whether an enum has multiple flags.
        /// </summary>
        /// <param name="flagsEnum">The enum to check.</param>
        /// <returns><see langword="true"/> if <paramref name="flagsEnum"/> type has the <see cref="FlagsAttribute"/> and has multiple flags; otherwise, <see langword="false"/>.</returns>
        /// <remarks><paramref name="flagsEnum"/> type must have the <see cref="FlagsAttribute"/>.</remarks>
        public static bool HasMultipleFlags(this Enum flagsEnum)
        {
            Type type = flagsEnum.GetType();

            if (type.GetCustomAttributes(typeof(FlagsAttribute)).Any())
            {
                bool alreadyFoundAFlag = false;

                Enum enumValue;

                // FieldInfo field = null;

                foreach (string s in type.GetEnumNames())
                {
                    enumValue = (Enum)Enum.Parse(type, s);

                    if (enumValue.GetNumValue().Equals(0)) continue;

                    if (flagsEnum.HasFlag(enumValue))

                        if (alreadyFoundAFlag) return true;

                        else alreadyFoundAFlag = true;
                }
            }

            return false;
        }
#endif
#endif
        public static bool HasAtLeastOneFlag<T>(this T flagsEnum, params T[] values) where T : Enum
        {
            foreach (T value in values)

                if (flagsEnum.HasFlag(value))

                    return true;

            return false;
        }

        public static bool HasAllFlags<T>(this T flagsEnum, params T[] values) where T : Enum
        {
            int i = 0;
            bool result = false;

            DoUntil(() => i++, () => (result = i == values.Length) && flagsEnum.HasFlag(values[i]));

            return result;
        }
#if CS7
        public static bool IsFlagsEnum(this Enum @enum) => (@enum ?? throw GetArgumentNullException(nameof(@enum))).GetType().GetCustomAttribute<FlagsAttribute>() is
#if CS9
            not null
#else
            object
#endif
            ;
#endif
        public static bool IsValidEnumValue<T>(this T value, in bool valuesAreExpected, params T[] values) where T : Enum
        {
            ThrowIfNull(values, nameof(values));

            foreach (T _value in values)

                if (value.Equals(_value))

                    return valuesAreExpected;

            return !valuesAreExpected;
        }
#if CS11
        public static bool Between<T>(this T
#else
        private static bool _Between<T>(in dynamic
#endif
            value, in T x, in T y)
#if CS11
            where T : IComparisonOperators<T, T, bool>
#endif
            => value >= x && value <= y;
#if CS11
        public static bool Outside<T>(this T
#else
        private static bool _Outside<T>(in dynamic
#endif
            value, in T x, in T y)
#if CS11
            where T : IComparisonOperators<T, T, bool>
#endif
            => value <= x && value >= y;
#if !CS11
        /// <summary>
        /// Checks if a number is between two given numbers.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="x">The left operand.</param>
        /// <param name="y">The right operand.</param>
        /// <returns><see langword="true"/> if <paramref name="b"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
        public static bool Between(this sbyte value, in sbyte x, in sbyte y) => _Between(value, x, y);

        /// <summary>
        /// Checks if a number is between two given numbers.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="x">The left operand.</param>
        /// <param name="y">The right operand.</param>
        /// <returns><see langword="true"/> if <paramref name="b"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
        public static bool Between(this byte value, in byte x, in byte y) => _Between(value, x, y);

        /// <summary>
        /// Checks if a number is between two given numbers.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="x">The left operand.</param>
        /// <param name="y">The right operand.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
        public static bool Between(this short value, in short x, in short y) => _Between(value, x, y);

        /// <summary>
        /// Checks if a number is between two given numbers.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="x">The left operand.</param>
        /// <param name="y">The right operand.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
        public static bool Between(this ushort value, in ushort x, in ushort y) => _Between(value, x, y);

        /// <summary>
        /// Checks if a number is between two given numbers.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="x">The left operand.</param>
        /// <param name="y">The right operand.</param>
        /// <returns><see langword="true"/> if <paramref name="i"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
        public static bool Between(this int value, in int x, in int y) => _Between(value, x, y);

        /// <summary>
        /// Checks if a number is between two given numbers.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="x">The left operand.</param>
        /// <param name="y">The right operand.</param>
        /// <returns><see langword="true"/> if <paramref name="i"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
        public static bool Between(this uint value, in uint x, in uint y) => _Between(value, x, y);

        /// <summary>
        /// Checks if a number is between two given numbers.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="x">The left operand.</param>
        /// <param name="y">The right operand.</param>
        /// <returns><see langword="true"/> if <paramref name="l"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
        public static bool Between(this long value, in long x, in long y) => _Between(value, x, y);

        /// <summary>
        /// Checks if a number is between two given numbers.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="x">The left operand.</param>
        /// <param name="y">The right operand.</param>
        /// <returns><see langword="true"/> if <paramref name="l"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
        public static bool Between(this ulong value, in ulong x, in ulong y) => _Between(value, x, y);

        /// <summary>
        /// Checks if a number is between two given numbers.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="x">The left operand.</param>
        /// <param name="y">The right operand.</param>
        /// <returns><see langword="true"/> if <paramref name="f"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
        public static bool Between(this float value, in float x, in float y) => _Between(value, x, y);

        /// <summary>
        /// Checks if a number is between two given numbers.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="x">The left operand.</param>
        /// <param name="y">The right operand.</param>
        /// <returns><see langword="true"/> if <paramref name="d"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
        public static bool Between(this double value, in double x, in double y) => _Between(value, x, y);

        /// <summary>
        /// Checks if a number is between two given numbers.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <param name="x">The left operand.</param>
        /// <param name="y">The right operand.</param>
        /// <returns><see langword="true"/> if <paramref name="d"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
        public static bool Between(this decimal value, in decimal x, in decimal y) => _Between(value, x, y);
#endif
        public static bool EnumBetween<T>(this T value, in T x, in T y) where T : Enum
#if CS11
        {
            dynamic _value = value;

            return _value >= (dynamic)x && _value <= (dynamic)y;
        }
#else
            => _Between(value, x, y);

        public static bool Outside(this sbyte value, in sbyte x, in sbyte y) => _Outside(value, x, y);
        public static bool Outside(this byte value, in byte x, in byte y) => _Outside(value, x, y);
        public static bool Outside(this short value, in short x, in short y) => _Outside(value, x, y);
        public static bool Outside(this ushort value, in ushort x, in ushort y) => _Outside(value, x, y);
        public static bool Outside(this int value, in int x, in int y) => _Outside(value, x, y);
        public static bool Outside(this uint value, in uint x, in uint y) => _Outside(value, x, y);
        public static bool Outside(this long value, in long x, in long y) => _Outside(value, x, y);
        public static bool Outside(this ulong value, in ulong x, in ulong y) => _Outside(value, x, y);
        public static bool Outside(this float value, in float x, in float y) => _Outside(value, x, y);
        public static bool Outside(this double value, in double x, in double y) => _Outside(value, x, y);
        public static bool Outside(this decimal value, in decimal x, in decimal y) => _Outside(value, x, y);
#endif
        public static bool EnumOutside<T>(this T value, in T x, in T y) where T : Enum
#if CS11
        {
            dynamic _value = value;

            return _value <= (dynamic)x || _value >= (dynamic)y;
        }
#else
            => _Outside(value, x, y);
#endif
        public static void SplitValues<T, U, TContainer>(this IEnumerable<T> enumerable, in bool skipEmptyEnumerables, IValueSplitFactory<T, U, TContainer> splitFactory, params T[] separators) where T : struct where U : IEnumerable<T>
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(splitFactory, nameof(splitFactory));
            // ThrowIfNull(enumerableNullableValueEntryCallback, nameof(enumerableNullableValueEntryCallback));
            ThrowIfNull(separators, nameof(separators));

            Predicate<T> predicate = separators.Length == 0
                ? throw new ArgumentException($"{nameof(separators)} does not contain values.")
                : separators.Length == 1
                ? (value => /*value != null && */ value.Equals(separators[0]))
                :
#if !CS9
                (Predicate<T>)
#endif
                (value => separators.Contains(value));
            IEnumerator<T> enumerator = enumerable.GetEnumerator();

            enumerable = null;

            void subAddAndAdd(in T value)
            {
                splitFactory.SubAdd(value);

                splitFactory.Add(splitFactory.GetEnumerable());
            }

            if (skipEmptyEnumerables)
            {
                if (enumerator.MoveNext())
                {
                    T? value = enumerator.Current;

                    if (enumerator.MoveNext()) // There are more than one value.
                    {
                        value = null;

                        void tryAdd()
                        {
                            if (predicate(enumerator.Current) && splitFactory.SubCount > 0)
                            {
                                splitFactory.Add(splitFactory.GetEnumerable());

                                splitFactory.SubClear();
                            }

                            else

                                splitFactory.SubAdd(enumerator.Current);
                        }

                        tryAdd();

                        while (enumerator.MoveNext())

                            tryAdd();
                    }

                    else if (predicate(value.Value)) // There is one value.

                        return;

                    else

                        subAddAndAdd(enumerator.Current);
                }

                else // There is no value.

                    return;
            }

            else if (enumerator.MoveNext())
            {
                T? value = enumerator.Current;

                if (enumerator.MoveNext()) // There are more than one value.
                {
                    value = null;

                    void tryAdd()
                    {
                        if (predicate(enumerator.Current))
                        {
                            splitFactory.Add(splitFactory.GetEnumerable());

                            if (splitFactory.SubCount > 0)

                                splitFactory.SubClear();
                        }

                        else

                            splitFactory.SubAdd(enumerator.Current);
                    }

                    tryAdd();

                    while (enumerator.MoveNext())

                        tryAdd();
                }

                else if (predicate(value.Value)) // There is one value.

                    splitFactory.Add(splitFactory.GetEnumerable());

                else

                    subAddAndAdd(enumerator.Current);
            }

            else // There is no value.

                splitFactory.Add(splitFactory.GetEnumerable());
        }

        public static string Repeat(this char c, in int length) => c.Repeat(null, null, length);

        public static string Repeat(this char c, in char left, in char right, in int length) => c.Repeat(left, right, length);

        private static string Repeat(this char c, in char? left, in char? right, in int length)
        {
            var sb = new StringBuilder();

            Action action;

            if (left.HasValue)
            {
                char _left = left.Value;

                if (right.HasValue)
                {
                    char _right = right.Value;

                    action = () => { _ = sb.Append(_left); _ = sb.Append(c); _ = sb.Append(_right); };
                }

                else

                    action = () => { _ = sb.Append(_left); _ = sb.Append(c); };
            }

            else if (right.HasValue)
            {
                char _right = right.Value;

                action = () => { _ = sb.Append(c); _ = sb.Append(_right); };
            }

            else

                action = () => sb.Append(c);

            for (int i = 0; i < length; i++)

                action();

            return sb.ToString();
        }

        public static string Repeat(this string s, in int length) => s.Repeat(null, null, length);

        public static string Repeat(this string s, string left, string right, in int length)
        {
            var sb = new StringBuilder();

            Action action = left != null
                ? right == null
                    ? (() => { _ = sb.Append(left); _ = sb.Append(s); })
                    :
#if !CS9
            (Action)
#endif
            (() => { _ = sb.Append(left); _ = sb.Append(s); _ = sb.Append(right); })
                : right != null ? (() => { _ = sb.Append(s); _ = sb.Append(right); }) :
#if !CS9
            (Action)
#endif
            (() => sb.Append(s));

            for (int i = 0; i < length; i++)

                action();

            return sb.ToString();
        }

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
    }
}
