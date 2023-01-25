using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using WinCopies.Util;

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
        private static bool _Shift(this IList array, int inStart, int length, ref int offset, ref int avoidOverflow, in Func<int> outStart, Action<int, int> action, in Func<bool> func, out int lOut, out int rOut)
        {
            int arrayLength = (array ?? throw new ArgumentNullException(nameof(array))).Count;

            if (arrayLength <= 1 || length == 0 || func())
            {
                lOut = rOut = -1;
                offset = 0;

                return false;
            }

            bool negativeLength;

            if (negativeLength = length < 0)

                length = -length;

            rOut = inStart.Between(0, arrayLength, true, false) ? length.Between(0, arrayLength) ? -1 : throw new ArgumentOutOfRangeException(nameof(length)) : throw new ArgumentOutOfRangeException(nameof(inStart));
            offset %= arrayLength;
            int _length = length;
            bool overflow = UtilHelpers.CheckOverflow(arrayLength, ref _length, offset);
            int _avoidOverflow = avoidOverflow;
            avoidOverflow = _length;

            if (overflow && _avoidOverflow > 0)
            {
                _avoidOverflow = 0;

                offset = offset < 0 ? _length : _length = -_length;

                return array._Shift(inStart, length, ref offset, ref _avoidOverflow, () => UtilHelpers.GetIndex(inStart, arrayLength, ref _length), action, func, out lOut, out rOut);
            }

            // There is no overflow or we are not avoiding it. We continue.

            ActionRef<int> outIndexUpdater = null;

            lOut = outStart();
            _length = length;

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
        #region Custom
        public static bool ShiftByPosition(this IList array, int inStart, in int length, ref int outStart, ref int avoidOverflow, in Action<int, int> action, out int offset, out int rOut)
        {
            int _outStart = outStart;
            int _offset = UtilHelpers.GetOffset(inStart, _outStart, array.Count);

            bool result = (array ?? throw new ArgumentNullException(nameof(array)))._Shift(inStart, length, ref _offset, ref avoidOverflow, () => _outStart, action, () => _offset == 0, out _, out rOut);

            if (length == array.Count)

                outStart = 2;

            else if (outStart < 0)

                outStart = array.Count + outStart;

            offset = _offset;

            return result;
        }

        public static bool Shift(this IList array, int start, int length, ref int offset, ref int avoidOverflow, in Action<int, int> action, out int lOut, out int rOut)
        {
            int _offset = offset;

            return array._Shift(start, length, ref offset, ref avoidOverflow, () => UtilHelpers.GetIndex(start, /*length,*/ array.Count, ref _offset), action, () => _offset == 0, out lOut, out rOut);
        }
        #endregion Custom
        #region Lossy
        public static bool LossyShiftByPosition(this IList array, in int inStart, in int length, ref int outStart, ref int avoidOverflow, out int offset, out int rOut) => array.ShiftByPosition(inStart, length, ref outStart, ref avoidOverflow, array.GetShiftDelegate(), out offset, out rOut);

        public static bool LossyShift(this IList array, in int start, in int length, ref int offset, ref int avoidOverflow, out int lOut, out int rOut) => array.Shift(start, length, ref offset, ref avoidOverflow, array.GetShiftDelegate(), out lOut, out rOut);
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

        public static void Swap(this IList array, in int x, in int y)
        {
            object
#if CS8
                ?
#endif
                tmp;

            tmp = array[x];
            array[x] = array[y];
            array[y] = tmp;
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
