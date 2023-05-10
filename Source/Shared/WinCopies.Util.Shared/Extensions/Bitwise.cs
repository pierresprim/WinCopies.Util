using System;
#if CS5
using System.Numerics;
#endif

using
#if !WinCopies4
    static
#endif
    WinCopies.Consts;

namespace WinCopies.Util
{
    public static partial class Extensions
    {
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
    }
}