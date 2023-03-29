using System;
using System.Collections;
using System.Collections.Generic;

using static WinCopies.ThrowHelper;

namespace WinCopies.Extensions.Util
{
    public static class Array
    {
        public static System.Array AsArray<T>(this T[] array) => array;

        public static Action<int, int> GetShiftDelegate(this IList array) => (int @in, int @out) => array[@in] = array[@out];

        #region Validation
        public static void ValidateArray(this ICollection array, in string arrayParamName, in int startIndex, ref int length, in bool allowAutoLength)
        {
            if (array == null ? throw new ArgumentNullException(arrayParamName) : allowAutoLength && length < 0)

                length = array.Count;

            if (startIndex < 0 || (length += startIndex) > array.Count)

                length = -1;
        }

        public static void ThrowIfInvalidArray(this ICollection array, in string arrayParamName, in int startIndex, ref int length, in bool allowAutoLength)
        {
            array.ValidateArray(arrayParamName, startIndex, ref length, allowAutoLength);

            if (length == -1)

                throw new ArgumentOutOfRangeException(arrayParamName);
        }

        private static void ThrowIfInvalidArray(in ICollection array, in int startIndex, ref int length, in bool allowAutoLength) => array.ThrowIfInvalidArray(nameof(array), startIndex, ref length, allowAutoLength);

        public static void ValidateArray<T>(this ICollection<T> array, in string arrayParamName, in int startIndex, ref int length, in bool allowAutoLength)
        {
            if (array == null ? throw new ArgumentNullException(arrayParamName) : allowAutoLength && length < 0)

                length = array.Count;

            if (startIndex < 0 || checked(length += startIndex) > array.Count)

                length = -1;
        }

        public static void ThrowIfInvalidArray<T>(this ICollection<T> array, in string arrayParamName, in int startIndex, ref int length, in bool allowAutoLength)
        {
            array.ValidateArray(arrayParamName, startIndex, ref length, allowAutoLength);

            if (length == -1)

                throw new ArgumentOutOfRangeException(arrayParamName);
        }

        private static void ThrowIfInvalidArray<T>(in ICollection<T> array, in int startIndex, ref int length, in bool allowAutoLength) => array.ThrowIfInvalidArray(nameof(array), startIndex, ref length, allowAutoLength);

        public static void ValidateArray<T>(this
#if CS5
            System.Collections.Generic.IReadOnlyList<T>
#else
            T[]
#endif
            array, in string arrayParamName, in int startIndex, ref int length, in bool allowAutoLength)
        {
            int count = array == null ? throw new ArgumentNullException(arrayParamName) : array.
#if CS5
                    Count
#else
                    Length
#endif
                    ;

            if (allowAutoLength && length < 0)

                length = count;

            if (startIndex < 0 || checked(length += startIndex) > count)

                length = -1;
        }

        public static void ThrowIfInvalidArray<T>(this
#if CS5
            System.Collections.Generic.IReadOnlyList<T>
#else
            T[]
#endif
            array, in string arrayParamName, in int startIndex, ref int length, in bool allowAutoLength)
        {
            array.ValidateArray(arrayParamName, startIndex, ref length, allowAutoLength);

            if (length == -1)

                throw new ArgumentOutOfRangeException(arrayParamName);
        }

        private static void ThrowIfInvalidArray<T>(in
#if CS5
            System.Collections.Generic.IReadOnlyList<T>
#else
            T[]
#endif
            array, in int startIndex, ref int length, in bool allowAutoLength) => array.ThrowIfInvalidArray(nameof(array), startIndex, ref length, allowAutoLength);
        #endregion Validation
        #region Data Processing
        public static void ProcessData<T>(this
#if CS5
            System.Collections.Generic.IReadOnlyList<T>
#else
            T[]
#endif
            array, Action<T> action, in int startIndex = 0, int length = -1) => array.ProcessData2((in int i) => action(array[i]), startIndex, length);

        public static void ProcessData2<T>(this
#if CS5
            System.Collections.Generic.IReadOnlyList<T>
#else
            T[]
#endif
            array, Action<int> action, in int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, action);
        }

        public static void ProcessData<T>(this
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            array, ActionIn<T> action, int startIndex = 0, int length = -1) => array.ProcessData2((in int i) => action(array[i]), startIndex, length);

        public static void ProcessData2<T>(this
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            array, ActionIn<int> action, int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, action);
        }

        public static int ProcessData<T>(this
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            array, Predicate<T> predicate, int startIndex = 0, int length = -1) => array.ProcessData2((in int i) => predicate(array[i]), startIndex, length);

        public static int ProcessData2<T>(this
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            array, Predicate<int> predicate, int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            return

            UtilHelpers._ProcessData(startIndex, length, predicate);
        }

        public static int ProcessData<T>(this
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            array, PredicateIn<T> predicate, int startIndex = 0, int length = -1) => array.ProcessData2((in int i) => predicate(array[i]), startIndex, length);

        public static int ProcessData2<T>(this
#if CS5
            IReadOnlyList<T>
#else
            T[]
#endif
            array, PredicateIn<int> predicate, int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            return

            UtilHelpers._ProcessData(startIndex, length, predicate);
        }

        public static void ProcessData<T>(this IList<T> array, Action<T> action, in int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, (in int i) => action(array[i]));
        }

        public static void ProcessData<T>(this IList<T> array, ActionIn<T> action, int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, (in int i) => action(array[i]));
        }

        public static int ProcessData<T>(this IList<T> array, Predicate<T> predicate, int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            return UtilHelpers._ProcessData(startIndex, length, (in int i) => predicate(array[i]));
        }

        public static int ProcessData<T>(this IList<T> array, PredicateIn<T> predicate, int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            return UtilHelpers._ProcessData(startIndex, length, (in int i) => predicate(array[i]));
        }

        public static void ProcessData(this IList array, Action<object
#if CS8
            ?
#endif
            > action, in int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, (in int i) => action(array[i]));
        }

        public static void ProcessData(this IList array, ActionIn<object
#if CS8
            ?
#endif
            > action, int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, (in int i) => action(array[i]));
        }

        public static int ProcessData(this IList array, Predicate<object
#if CS8
            ?
#endif
            > predicate, int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            return UtilHelpers._ProcessData(startIndex, length, (in int i) => predicate(array[i]));
        }

        public static int ProcessData(this IList array, PredicateIn<object
#if CS8
            ?
#endif
            > predicate, int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            return UtilHelpers._ProcessData(startIndex, length, (in int i) => predicate(array[i]));
        }
        #endregion Data Processing
        #region Fill
        public static void Fill<T>(this T[] array, Converter<int, T> func, in int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array.AsArray(), startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, (in int i) => array[i] = func(i));
        }

        public static void Fill<T>(this IList<T> array, Converter<int, T> func, in int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, (in int i) => array[i] = func(i));
        }

        public static void Fill(this IList array, Converter<int, object
#if CS8
            ?
#endif
            > func, in int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, (in int i) => array[i] = func(i));
        }

        public static void Fill<T>(this T[] array, Converter<int, T> func) => array.Fill(func, 0, array.Length);

        public static void Fill<T>(this IList<T> array, Converter<int, T> func) => array.Fill(func, 0, array.Count);

        public static void Fill(this IList array, Converter<int, object
#if CS8
            ?
#endif
            > func) => array.Fill(func, 0, array.Count);

        public static void Fill<T>(this T[] array, ConverterIn<int, T> func, in int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array.AsArray(), startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, (in int i) => array[i] = func(i));
        }

        public static void Fill<T>(this IList<T> array, ConverterIn<int, T> func, in int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, (in int i) => array[i] = func(i));
        }

        public static void Fill(this IList array, ConverterIn<int, object
#if CS8
            ?
#endif
            > func, in int startIndex = 0, int length = -1)
        {
            ThrowIfInvalidArray(array, startIndex, ref length, true);

            UtilHelpers._ProcessData(startIndex, length, (in int i) => array[i] = func(i));
        }

        public static void Fill<T>(this T[] array, ConverterIn<int, T> func) => array.Fill(func, 0, array.Length);

        public static void Fill<T>(this IList<T> array, ConverterIn<int, T> func) => array.Fill(func, 0, array.Count);

        public static void Fill(this IList array, ConverterIn<int, object
#if CS8
            ?
#endif
            > func) => array.Fill(func, 0, array.Count);
        #endregion Fill
        #region SetValuesTo
        public static void SetValuesTo(this IList array, int startIndex, int length, object
#if CS8
            ?
#endif
            value) => UtilHelpers.ProcessData((in int i) => array[i] = value, startIndex, length);

        public static void SetValuesTo<T>(this T[] array, in int start, in int length, in T
#if CS9
            ?
#endif
            value = default) => array.AsArray().SetValuesTo(start, length, value);

        public static void SetValuesTo(this IList array, object
#if CS8
            ?
#endif
            value) => array.SetValuesTo(0, array.Count, value);

        public static void SetValuesTo<T>(this T[] array, in T
#if CS9
            ?
#endif
            value = default) => array.SetValuesTo(0, array.Length, value);

        public static void SetValuesTo(this IList array, in int startIndex, in int length, Converter<int, object
#if CS8
            ?
#endif
            > func) => UtilHelpers.ProcessData((in int i) => array[i] = func(i), startIndex, length);

        public static void SetValuesTo<T>(this T[] array, in int start, in int length, in Converter<int, T
#if CS9
            ?
#endif
            > func) => array.AsArray().SetValuesTo(start, length, func);

        public static void SetValuesTo(this IList array, in Converter<int, object
#if CS8
            ?
#endif
            > func) => array.SetValuesTo(0, array.Count, func);

        public static void SetValuesTo<T>(this T[] array, in Converter<int, T
#if CS9
            ?
#endif
            > func) => array.SetValuesTo(0, array.Length, func);
        #endregion SetValuesTo
        #region Shift
        private static bool? ValidateShift(in IList array, int inStart, ref int length, ref int offset, ref int avoidOverflow, in Func<bool> func, out int lOut, out bool negativeLength, out int _length, out bool overflow, out int _avoidOverflow, out FuncRef<int, int> outStart, out int arrayLength)
        {
            arrayLength = array.Count;

            if (arrayLength <= 1 || length == 0 || func())
            {
                lOut = -1;
                _avoidOverflow = _length = offset = 0;
                overflow = negativeLength = false;
                outStart = null;

                return false;
            }

            if (negativeLength = length < 0)

                length = -length;

            offset %= inStart.Between(0, arrayLength, true, false) ? length <= arrayLength ? arrayLength : throw new ArgumentOutOfRangeException(nameof(length)) : throw new ArgumentOutOfRangeException(nameof(inStart));
            _length = length;
            overflow = UtilHelpers.CheckOverflow(arrayLength, ref _length, offset);

            _avoidOverflow = avoidOverflow;
            avoidOverflow = _length;

            lOut = 0;

            if (overflow && _avoidOverflow > 0)
            {
                _avoidOverflow = 0;

                offset = offset < 0 ? _length : _length = -_length;

                int _arrayLength = arrayLength;

                outStart = (ref int __length) => UtilHelpers.GetIndex(inStart, _arrayLength, ref __length);

                return true;
            }

            outStart = null;

            return null;
        }

        private abstract class Shifter
        {
            protected readonly int arrayLength;
            protected readonly int inStart;

            public readonly int lOut;
            public readonly int rOut;

            protected abstract int InStart { get; }
            protected abstract int OutStart { get; }

            protected abstract int Destination { get; }

            protected abstract int AvailableLength { get; }

            protected abstract int SourceAvailableLength { get; }

            protected abstract bool DestinationOverflow { get; }

            protected Shifter(in int arrayLength, in int inStart, ref int offset, in Func<int> outStart)
            {
                this.arrayLength = arrayLength;
                this.inStart = inStart;

                lOut = outStart();
                rOut = UtilHelpers.GetIndex(lOut, arrayLength, ref offset);
            }

            protected abstract int GetSource(in int length);

            public void Shift(int length, ActionIn<int, int, int> action)
            {
                void copy(in int outStart, in int _length) => action(InStart, outStart, _length);
                void _copy() => copy(OutStart, length);

                int tmp;

                void __copy() => copy(OutStart, tmp);
                void ___copy()
                {
                    if (length > (tmp = AvailableLength))
                    {
                        __copy();
                        action(GetSource(tmp), Destination, length - tmp);
                    }

                    else

                        _copy();
                }

                if (DestinationOverflow)

                    ___copy();

                else if ((tmp = SourceAvailableLength) > length)
                {
                    __copy();

                    length -= tmp;

                    if (this is DecrementalShifter)

                        tmp = -tmp;

                    int getIndex(in int index) => UtilHelpers.GetIndex(index, arrayLength, ref tmp);

                    action(getIndex(InStart), getIndex(OutStart), length);
                }

                else

                    _copy();
            }
        }
        private sealed class IncrementalShifter : Shifter
        {
            protected override int InStart => inStart;
            protected override int OutStart => lOut;

            protected override int Destination => 0;

            protected override int AvailableLength => arrayLength - lOut;

            protected override int SourceAvailableLength => arrayLength - inStart;

            protected override bool DestinationOverflow => lOut > inStart;

            public IncrementalShifter(in int arrayLength, in int inStart, ref int offset, in Func<int> outStart) : base(arrayLength, inStart, ref offset, outStart) { /* Left empty. */ }

            protected override int GetSource(in int length) => length + inStart;
        }
        private sealed class DecrementalShifter : Shifter
        {
            private readonly int inEnd;

            protected override int InStart => inEnd;
            protected override int OutStart => rOut;

            protected override int Destination => arrayLength - 1;

            protected override int AvailableLength => rOut + 1;

            protected override int SourceAvailableLength => inEnd + 1;

            protected override bool DestinationOverflow => rOut < inEnd;

            public DecrementalShifter(in int arrayLength, in int inStart, ref int offset, in Func<int> outStart) : base(arrayLength, inStart, ref offset, outStart) => inEnd = UtilHelpers.GetIndex(inStart, arrayLength, ref offset);

            protected override int GetSource(in int length) => inEnd - length;
        }

        private static void Shift(in int arrayLength, in int inStart, in int length, in bool positiveOffset, ref int offset, in Func<int> outStart, in ActionIn<int, int, int> action, out int lOut, out int rOut)
        {
            lOut = outStart();

            Shifter shifter = positiveOffset ? new DecrementalShifter(arrayLength, inStart, ref offset, outStart) :
#if !CS9
                (Shifter)
#endif
                new IncrementalShifter(arrayLength, inStart, ref offset, outStart);

            shifter.Shift(length, action);

            lOut = shifter.lOut;
            rOut = shifter.rOut;
        }

        private static unsafe bool _Shift(this System.Array array, int inStart, int length, ref int offset, ref int avoidOverflow, in Func<int> outStart, in Func<Action<int, int>> action, in Func<bool> func, in ConverterIn<bool, ActionIn<int, int, int>
#if CS8
            ?
#endif
            > _action, out int lOut, out int rOut)
        {
            bool? validationResult = ValidateShift(array.Rank == 1 ? array : throw new ArgumentException($"{nameof(array)} must have only one dimension. The given array has {array.Rank} dimensions.", nameof(array)), inStart, ref length, ref offset, ref avoidOverflow, func, out lOut, out bool negativeLength, out int _length, out bool overflow, out int _avoidOverflow, out FuncRef<int, int> _outStart, out int arrayLength);

            if (validationResult.HasValue)
            {
                if (validationResult.Value)

                    return array._Shift(inStart, length, ref offset, ref _avoidOverflow, () => _outStart(ref _length), action, func, _action, out lOut, out rOut);

                rOut = -1;

                return false;
            }

            ActionIn<int, int, int>
#if CS8
                ?
#endif
                __action;

            bool positiveOffset = offset > 0;

            if (negativeLength || (__action = _action(positiveOffset)) == null)

                return array._Shift(inStart, length, ref offset, ref avoidOverflow, outStart, action(), func, negativeLength, overflow, out lOut, out rOut);

            Shift(arrayLength, inStart, length, positiveOffset, ref offset, outStart, __action, out lOut, out rOut);

            return true;
        }
        private static unsafe bool _Shift(this System.Array array, int inStart, int length, ref int offset, ref int avoidOverflow, in Func<int> outStart, in Func<Action<int, int>> action, in Func<bool> func, out int lOut, out int rOut) => array._Shift(inStart, length, ref offset, ref avoidOverflow, outStart, action, func, (in bool positiveOffset) => positiveOffset ?
#if !CS9
            (ActionIn<int, int, int>
#if CS8
            ?
#endif
            )
#endif
            null : (in int _inStart, in int _outStart, in int _length) => System.Array.Copy(array, _inStart, array, _outStart, _length), out lOut, out rOut);
#if CS5
        private delegate void __Shift<T>(in T[] array, in int start, ref ulong offset, ref ulong length);
        private static unsafe bool _Shift<T>(this T[] array, int inStart, int length, ref int offset, ref int avoidOverflow, in Func<int> outStart, in Func<Action<int, int>> action, in Func<bool> func, out int lOut, out int rOut) where T : unmanaged
        {
            ActionIn<int, int, int> getAction(__Shift<T> shift) => (in int _inStart, in int _outStart, in int _length) =>
            {
                ulong __outStart = (ulong)_outStart;
                ulong __length = (ulong)length;

                shift(array, inStart, ref __outStart, ref __length);
            };

            return array._Shift(inStart, length, ref offset, ref avoidOverflow, outStart, action, func, (in bool positiveOffset) => positiveOffset ? getAction(WinCopies.Array.Shift) : getAction(WinCopies.Array.Unshift), out lOut, out rOut);
        }
#endif
        private static bool _Shift(this IList array, int inStart, int length, ref int offset, ref int avoidOverflow, in Func<int> outStart, Action<int, int> action, in Func<bool> func, bool negativeLength, bool overflow, out int lOut, out int rOut)
        {
            int arrayLength = array.Count;

            ActionRef<int> outIndexUpdater = null;

            lOut = outStart();
            int _length = length;

            int __offset = offset;

            void incrementCircularly(ref int index) => UtilHelpers.IncrementCircularly(ref index, arrayLength);
            void setOutIndexUpdater(bool reverse, int _lOut, int _rOut, out int __rOut)
            {
                if (_length == 1)
                {
                    __rOut = _rOut;

                    outIndexUpdater = Delegates.EmptyVoid;
                }

                else
                {
                    void setUpdater(ActionRef<int> _action) => outIndexUpdater = (ref int ___rOut) => outIndexUpdater = _action;

                    bool positiveOffset = __offset > 0;

                    bool check(out int ___rOut)
                    {
                        if (reverse)
                        {
                            ___rOut = positiveOffset ? _lOut : _rOut;

                            return true;
                        }

                        ___rOut = _rOut;

                        return false;
                    }

                    if (positiveOffset ^ check(out __rOut))

                        setUpdater((ref int ___rOut) => UtilHelpers.DecrementCircularly(ref ___rOut, arrayLength));

                    else
                        /*{
                            lOut = -lOut;*/

                        setUpdater(incrementCircularly);
                    //}
                }
            }

            void /*bool*/ shift(ref int start, /*Func<bool> condition,*/ in Action inIndexUpdater, int _lOut, out int _rOut)
            {
                PredicateRef<int> condition;
                int inEnd;
                int __rOut = -1;

                void _setOutIndexUpdater(in bool reverse, in int ___rOut) => setOutIndexUpdater(reverse, _lOut, ___rOut, out __rOut);

                PredicateRef<int> getStopCondition(int __start, int ___lOut, int ___rOut/*, out bool _zeroLength*/)
                {
                    PredicateRef<int> _getPredicate(int __lOut) => (ref int _start) => _start == __lOut;
                    PredicateRef<int> getPredicate(byte _i) => _getPredicate((_lOut == 0 ? arrayLength : _lOut) - _i);
                    bool fullLength() => _length == arrayLength;
                    PredicateRef<int> onFullLength() =>/*_lOut < inStart*/ getPredicate(__offset < 0 && _lOut > 0 ? (byte)0 : (byte)1);
                    PredicateRef<int> onOverflow(Converter<PredicateRef<int>, PredicateRef<int>> _func) => __offset < 0 ? -__offset == arrayLength - 1 ? _func(Bool.TrueRef) :
#if !CS9
                        (PredicateRef<int>)(
#endif
                        (ref int ___start) =>
                        {
                            condition = _func(_getPredicate(_lOut));

                            return false;
                        }
#if !CS9
                    )
#endif
                    : _func(_getPredicate(__start == 0 ? __offset /*arrayLength - (_length - /*1*//*(arrayLength - inStart))*/ : (__start -= arrayLength - __offset) < 0 ? arrayLength + __start : __start));
                    PredicateRef<int> getCommonPredicate()
                    {
                        int i = 0;

                        return (ref int _start) => ++i == _length;
                    }

                    if (negativeLength)
                    {
                        bool overlap(in int index) => UtilHelpers.Overlap(inStart, index, inEnd);

                        int from, to;

                        PredicateRef<int> ____getPredicate(PredicateRef<int> predicate) => (ref int _start) =>
                        {
                            if (predicate(ref _start))
                            {
                                array.ReverseTo(from, to);

                                return true;
                            }

                            return false;
                        };
                        PredicateRef<int> ___getPredicate(in Func<int> index) => overflow ? onOverflow(____getPredicate) : ____getPredicate(fullLength() ? onFullLength() : _getPredicate(index()));

                        PredicateRef<int> _predicate;

                        PredicateRef<int> _____getPredicate()
                        {
                            if (__offset > 0)
                            {
                                if (overlap(___lOut))
                                {
                                    from = ___lOut;
                                    to = ___rOut;

                                    return ___getPredicate(() => UtilHelpers.DecrementCircularly2(inStart, arrayLength));
                                }
                            }

                            else if (overlap(___rOut))
                            {
                                from = ___lOut;
                                to = ___rOut;

                                return ___getPredicate(() => UtilHelpers.IncrementCircularly2(inEnd, arrayLength));
                            }

                            return null;
                        }

                        if ((_predicate = _____getPredicate()) != null)
                            /*{
                                _zeroLength = false;*/

                            return _predicate;
                        //}

                        if (overflow)
                        {
                            //_zeroLength = false;

                            from = ___lOut;
                            to = ___rOut;

                            return onOverflow(____getPredicate);
                        }

                        else

                            _setOutIndexUpdater(true, ___rOut);
                    }

                    /*if (_zeroLength = _length == 0)
                    {
                        _length = arrayLength;

                        return getPredicate(1);
                    }*/

                    return overflow ? onOverflow(Delegates.Self) : fullLength() ? onFullLength() : getCommonPredicate();
                }

                /*if (_lOut == 0)

                    _lOut = arrayLength - 1;

                else

                    _lOut--;*/

                int getInEnd()
                {
                    int __length = _length - 1;

                    return UtilHelpers.GetIndex(inStart, arrayLength, ref __length);
                }

                inEnd = getInEnd();

                int ____rOut;

                if (__offset < 0)
                {
                    __rOut = _rOut = UtilHelpers.GetIndex(inStart, arrayLength, ref __offset);

                    int _offset = _length - 1;
                    UtilHelpers.SetIndex(ref _rOut, arrayLength, ref _offset);

                    ____rOut = negativeLength ? _rOut : __rOut;
                }

                else

                    ____rOut = __rOut = _rOut = /*(*/UtilHelpers.GetIndex(inEnd, arrayLength, ref __offset)/*) == 0 ? (__rOut = arrayLength) : __rOut*/;

                condition = getStopCondition(start, _lOut, ____rOut/*, out bool zeroLength*/);

                if (outIndexUpdater == null)

                    _setOutIndexUpdater(false, __rOut);

                //__rOut += _length % arrayLength;*/
                UtilHelpers.DoForUntil(null, condition, inIndexUpdater, (ref int _start) =>
                {
                    outIndexUpdater(ref __rOut);

                    action(__rOut, _start);
                },
                ref start);

                //return zeroLength;
            }

            if (offset < 0)
                /*{
                    if (length > arrayLength - inStart ? length == arrayLength ? inStart - 1 : UtilHelpers.GetIndex(inStart, arrayLength, ref __length) )

                        inStart--;*/

                shift(ref inStart, /*() => inStart < length,*/ () => incrementCircularly(ref inStart), lOut, out rOut);
            //}

            else
            {
                int __length = length - 1;

                length = length > arrayLength - inStart ? length == arrayLength ? inStart - 1 : UtilHelpers.GetIndex(inStart, arrayLength, ref __length) : inStart + length - 1;

                /*if (*/
                shift(ref length, /*() => length > inStart,*/ () => length = (length == 0 ? arrayLength : length) - 1, lOut, out rOut);/*)

                    lOut = 2;*/
            }

            return true;
        }
        private static bool _Shift(this IList array, int inStart, int length, ref int offset, ref int avoidOverflow, in Func<int> outStart, Action<int, int> action, in Func<bool> func, out int lOut, out int rOut)
        {
            if (array is System.Array _array)

                return _array._Shift(inStart, length, ref offset, ref avoidOverflow, outStart, () => action, func, out lOut, out rOut);

            bool? validationResult = ValidateShift(array ?? throw new ArgumentNullException(nameof(array)), inStart, ref length, ref offset, ref avoidOverflow, func, out lOut, out bool negativeLength, out int _length, out bool overflow, out int _avoidOverflow, out FuncRef<int, int> _outStart, out _);

            if (validationResult.HasValue)
            {
                if (validationResult.Value)

                    return array._Shift(inStart, length, ref offset, ref _avoidOverflow, () => _outStart(ref _length), action, func, out lOut, out rOut);

                rOut = -1;

                return false;
            }

            // There is no overflow or we are not avoiding it.

            return array._Shift(inStart, length, ref offset, ref avoidOverflow, outStart, action, func, negativeLength, overflow, out lOut, out rOut);
        }
        #region Custom
        private static int GetOffset(int inStart, int outStart, in int arrayLength, out Func<int> indexFunc, out Func<bool> func)
        {
            int offset = UtilHelpers.GetOffset(inStart, outStart, arrayLength);

            indexFunc = () => outStart;
            func = () => offset == 0;

            return offset;
        }
        private static void UpdateOutStart(in int arrayLength, in int length, ref int outStart)
        {
            if (length == arrayLength)

                outStart = 2;

            else if (outStart < 0)

                outStart = arrayLength + outStart;
        }

        public static bool ShiftByPosition(this IList array, int inStart, in int length, ref int outStart, ref int avoidOverflow, in Action<int, int> action, out int offset, out int rOut)
        {
            offset = GetOffset(inStart, outStart, array.Count, out Func<int> indexFunc, out Func<bool> func);

            bool result = (array ?? throw new ArgumentNullException(nameof(array)))._Shift(inStart, length, ref offset, ref avoidOverflow, indexFunc, action, func, out _, out rOut);

            UpdateOutStart(array.Count, length, ref outStart);

            return result;
        }
        public static bool ShiftByPosition(this System.Array array, int inStart, in int length, ref int outStart, ref int avoidOverflow, in Func<Action<int, int>> action, out int offset, out int rOut)
        {
            offset = GetOffset(inStart, outStart, array.Length, out Func<int> indexFunc, out Func<bool> func);

            bool result = (array ?? throw new ArgumentNullException(nameof(array)))._Shift(inStart, length, ref offset, ref avoidOverflow, indexFunc, action, func, out _, out rOut);

            UpdateOutStart(array.Length, length, ref outStart);

            return result;
        }
#if CS5
        public static bool ShiftByPosition<T>(this T[] array, int inStart, in int length, ref int outStart, ref int avoidOverflow, in Func<Action<int, int>> action, out int offset, out int rOut) where T : unmanaged
        {
            offset = GetOffset(inStart, outStart, array.Length, out Func<int> indexFunc, out Func<bool> func);

            bool result = (array ?? throw new ArgumentNullException(nameof(array)))._Shift(inStart, length, ref offset, ref avoidOverflow, indexFunc, action, func, out _, out rOut);

            UpdateOutStart(array.Length, length, ref outStart);

            return result;
        }
#endif
        private static Func<int> GetIndexFunc(int start, int arrayLength, in int offset, out Func<bool> func)
        {
            int _offset = offset;

            func = () => _offset == 0;

            return () => UtilHelpers.GetIndex(start, /*length,*/ arrayLength, ref _offset);
        }

        public static bool Shift(this IList array, int start, int length, ref int offset, ref int avoidOverflow, in Action<int, int> action, out int lOut, out int rOut) => (array ?? throw new ArgumentNullException(nameof(array)))._Shift(start, length, ref offset, ref avoidOverflow, GetIndexFunc(start, array.Count, offset, out Func<bool> func), action, func, out lOut, out rOut);
        public static bool Shift(this System.Array array, int start, int length, ref int offset, ref int avoidOverflow, in Func<Action<int, int>> action, out int lOut, out int rOut) => (array ?? throw new ArgumentNullException(nameof(array)))._Shift(start, length, ref offset, ref avoidOverflow, GetIndexFunc(start, array.Length, offset, out Func<bool> func), action, func, out lOut, out rOut);
#if CS5
        public static bool Shift<T>(this T[] array, int start, int length, ref int offset, ref int avoidOverflow, in Func<Action<int, int>> action, out int lOut, out int rOut) where T : unmanaged => (array ?? throw new ArgumentNullException(nameof(array)))._Shift(start, length, ref offset, ref avoidOverflow, GetIndexFunc(start, array.Length, offset, out Func<bool> func), action, func, out lOut, out rOut);
#endif
        #endregion Custom
        #region Lossy
        public static bool LossyShiftByPosition(this IList array, in int inStart, in int length, ref int outStart, ref int avoidOverflow, out int offset, out int rOut) => array.ShiftByPosition(inStart, length, ref outStart, ref avoidOverflow, array.GetShiftDelegate(), out offset, out rOut);
        public static bool LossyShift(this IList array, in int start, in int length, ref int offset, ref int avoidOverflow, out int lOut, out int rOut) => array.Shift(start, length, ref offset, ref avoidOverflow, array.GetShiftDelegate(), out lOut, out rOut);

        public static bool LossyShiftByPosition(this System.Array array, in int inStart, in int length, ref int outStart, ref int avoidOverflow, out int offset, out int rOut) => array.ShiftByPosition(inStart, length, ref outStart, ref avoidOverflow, array.GetShiftDelegate, out offset, out rOut);
        public static bool LossyShift(this System.Array array, in int start, in int length, ref int offset, ref int avoidOverflow, out int lOut, out int rOut) => array.Shift(start, length, ref offset, ref avoidOverflow, array.GetShiftDelegate, out lOut, out rOut);
#if CS5
        public static bool LossyShiftByPosition<T>(this T[] array, in int inStart, in int length, ref int outStart, ref int avoidOverflow, out int offset, out int rOut) where T : unmanaged => array.ShiftByPosition(inStart, length, ref outStart, ref avoidOverflow, array.GetShiftDelegate, out offset, out rOut);
        public static bool LossyShift<T>(this T[] array, in int start, in int length, ref int offset, ref int avoidOverflow, out int lOut, out int rOut) where T : unmanaged => array.Shift(start, length, ref offset, ref avoidOverflow, array.GetShiftDelegate, out lOut, out rOut);
#endif
        #endregion Lossy
#if WinCopies4
        #region Reset
        private static bool ShiftAndReset(this IList array, out int rOut, in FuncOut<Interval<int>, bool> func, in ActionIn<int, int> action)
        {
            if (func(out Interval<int> interval))
            {
                int _rOut;

                if (interval.Start > (rOut = interval.Length))

                    action(_rOut = interval.Length + 1, interval.Start - _rOut);

                else
                {
                    action(0, interval.Start);

                    _rOut = rOut + 1;

                    action(_rOut, array.Count - _rOut);
                }

                return true;
            }

            rOut = -1;

            return false;
        }

        private static ActionIn<int, int> GetAction(IList array, object
#if CS8
            ?
#endif
            value) => (in int start, in int length) => array.SetValuesTo(start, length, value);

        private static ActionIn<int, int> GetAction(IList array, Converter<int, object
#if CS8
            ?
#endif
            > func) => (in int start, in int length) => array.SetValuesTo(start, length, func);

        private static bool ShiftAndResetByPosition(this IList array, int inStart, int length, int outStart, ref int avoidOverflow, out int offset, out int rOut, in ActionIn<int, int> action)
        {
            int _avoidOverflow = avoidOverflow;
            int _offset = 0;

            bool result = array.ShiftAndReset(out rOut, (out Interval<int> interval) =>
            {
                if (array.LossyShiftByPosition(inStart, length, ref outStart, ref _avoidOverflow, out _offset, out int _rOut))
                {
                    interval = new Interval<int>(outStart, _rOut);

                    return true;
                }

                interval = default;

                return false;
            }, action);

            avoidOverflow = _avoidOverflow;
            offset = _offset;

            return result;
        }

        private static bool ShiftAndReset(this IList array, int start, int length, ref int offset, ref int avoidOverflow, out int lOut, out int rOut, ActionIn<int, int> action)
        {
            int _offset = offset;
            int _lOut = -1;

            int _avoidOverflow = avoidOverflow;

            bool result = array.ShiftAndReset(out rOut, (out Interval<int> interval) =>
            {
                if (array.LossyShift(start, length, ref _offset, ref _avoidOverflow, out _lOut, out int _rOut))
                {
                    interval = new Interval<int>(_lOut, _rOut);

                    return true;
                }

                interval = default;

                return false;
            }, action);

            avoidOverflow = _avoidOverflow;
            lOut = _lOut;
            offset = _offset;

            return result;
        }

        public static bool ShiftAndResetByPosition(this IList array, int inStart, int length, int outStart, ref int avoidOverflow, out int offset, out int rOut, in object
#if CS8
            ?
#endif
            value) => array.ShiftAndResetByPosition(inStart, length, outStart, ref avoidOverflow, out offset, out rOut, GetAction(array, value));

        public static bool ShiftAndReset(this IList array, int start, int length, ref int offset, ref int avoidOverflow, out int lOut, out int rOut, object
#if CS8
            ?
#endif
            value) => array.ShiftAndReset(start, length, ref offset, ref avoidOverflow, out lOut, out rOut, GetAction(array, value));

        public static bool ShiftAndResetByPosition<T>(this T[] array, in int inStart, in int length, in int outStart, ref int avoidOverflow, out int offset, out int rOut, in T
#if CS9
            ?
#endif
            value = default) => array.AsArray().ShiftAndResetByPosition(inStart, length, outStart, ref avoidOverflow, out offset, out rOut, value);

        public static bool ShiftAndReset<T>(this T[] array, in int start, in int length, ref int offset, ref int avoidOverflow, out int lOut, out int rOut, in T
#if CS9
            ?
#endif
            value = default) => array.AsArray().ShiftAndReset(start, length, ref offset, ref avoidOverflow, out lOut, out rOut, value);

        public static bool ShiftAndResetByPosition(this IList array, int inStart, int length, int outStart, ref int avoidOverflow, out int offset, out int rOut, in Converter<int, object
#if CS8
            ?
#endif
            > func) => array.ShiftAndResetByPosition(inStart, length, outStart, ref avoidOverflow, out offset, out rOut, GetAction(array, func));

        public static bool ShiftAndReset(this IList array, int start, int length, ref int offset, ref int avoidOverflow, out int lOut, out int rOut, in Converter<int, object
#if CS8
            ?
#endif
            > func) => array.ShiftAndReset(start, length, ref offset, ref avoidOverflow, out lOut, out rOut, GetAction(array, func));

        public static bool ShiftAndResetByPosition<T>(this T[] array, in int inStart, in int length, in int outStart, ref int avoidOverflow, out int offset, out int rOut, Converter<int, T
#if CS9
            ?
#endif
            > func) => array.AsArray().ShiftAndResetByPosition(inStart, length, outStart, ref avoidOverflow, out offset, out rOut, i => func(i));

        public static bool ShiftAndReset<T>(this T[] array, in int start, in int length, ref int offset, ref int avoidOverflow, out int lOut, out int rOut, Converter<int, T
#if CS9
            ?
#endif
            > func) => array.AsArray().ShiftAndReset(start, length, ref offset, ref avoidOverflow, out lOut, out rOut, i => func(i));
        #endregion Reset
#endif
        #endregion Shift
        #region Remove
        private static TArray Remove<TItems, TArray>(this TArray _array, System.Array array, in int startIndex, in int length, in ConverterIn<int, TItems[]> destination, int outStart, in ConverterIn<TItems[], TArray> converter)
        {
            if (_array == null ? throw new ArgumentNullException(nameof(array)) : length == 0)

                return _array;

            if (length < 0)

                return _array.Remove(array, array.Length - startIndex - 1, System.Math.Abs(length), destination, outStart, converter);

            int _length = startIndex + length;

            if (startIndex < 0 || _length > array.Length ? throw new ArgumentOutOfRangeException(nameof(startIndex)) : length == array.Length)

                return converter(
#if CS5
                    System
#else
                    WinCopies
#endif
                    .Array.Empty<TItems>());

            int destinationLength = array.Length - length;

            TItems[] destinationArray = destination(destinationLength);

            void copy(in int sourceStart, in int lengthToCopy) => System.Array.Copy(array, sourceStart, destinationArray, outStart, lengthToCopy);

            if (startIndex > 0)
            {
                copy(0, startIndex);

                outStart += startIndex;
            }

            if (_length < array.Length)

                copy(_length, array.Length - _length);

            return converter(destinationArray);
        }

        public static System.Array Remove(this System.Array array, in int startIndex, in int length, object[] destination, int outStart) => array.Remove<object, System.Array>(array, startIndex, length, Delegates.GetArrayConverterIn<object>(destination, outStart, nameof(outStart)), outStart, Delegates.SelfIn);
        public static System.Array Remove(this System.Array array, in int startIndex, in int length) => array.Remove<object, System.Array>(array, startIndex, length, Delegates.GetArrayConverterIn<object>(), 0, Delegates.SelfIn);

        public static T[] Remove<T>(this T[] array, in int startIndex, in int length, T[] destination, int outStart) => array.Remove<T, T[]>(array, startIndex, length, Delegates.GetArrayConverterIn(destination, outStart, nameof(outStart)), outStart, Delegates.SelfIn);
        public static T[] Remove<T>(this T[] array, in int startIndex, in int length) => array.Remove<T, T[]>(array, startIndex, length, Delegates.GetArrayConverterIn<T>(), 0, Delegates.SelfIn);
        #endregion Remove

        public static void Swap(this IList array, int x, int y)
        {
            ThrowOnInvalidArrayMoveOperation(array, nameof(array), x, nameof(x), y, nameof(y));

            if (x == y)

                return;
#if CS5
            (array[y], array[x]) = (array[x], array[y]);
#else
            object tmp = array[x];

            array[x] = array[y];
            array[y] = tmp;
#endif
        }

        public static void Swap<T>(this T[] array, int x, int y)
        {
            ThrowOnInvalidArrayMoveOperation(array, nameof(array), x, nameof(x), y, nameof(y));

            if (x == y)

                return;
#if CS5
            (array[y], array[x]) = (array[x], array[y]);
#else
            T temp = array[x];

            array[x] = array[y];
            array[y] = temp;
#endif
        }

        public static void Reverse(this IList array, int startIndex, int length)
        {
            int arrayLength = (array ?? throw new ArgumentNullException(nameof(array))).Count;

            if (startIndex.Between(0, arrayLength, true, false))

                if (length.Between(0, arrayLength))
                {
                    if (length < 2)

                        return;

                    int _length = startIndex;

                    _length = UtilHelpers.GetIndex(length - 1, arrayLength, ref _length);

                    length /= 2;

                    void swap() => array.Swap(startIndex, _length);

                    Action action = () =>
                    {
                        swap();

                        action = () =>
                        {
                            startIndex = ++startIndex % arrayLength;

                            UtilHelpers.DecrementCircularly(ref _length, arrayLength);

                            swap();
                        };
                    };

                    for (int i = 0; i < length; i++)

                        action();
                }

                else

                    throw new ArgumentOutOfRangeException(nameof(length));

            else

                throw new ArgumentOutOfRangeException(nameof(startIndex));
        }
        public static void ReverseTo(this IList array, int startIndex, int endIndex) => array.Reverse(startIndex, UtilHelpers.GetOffset(startIndex, endIndex, array.Count) + 1);
        public static void Reverse(this IList list) => (list ?? throw new ArgumentNullException(nameof(list))).Reverse(0, list.Count);
    }
}
