using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Util;

using static WinCopies.Extensions.Util.Tests.Array.CommonTests;

namespace WinCopies.Extensions.Util.Tests.Array
{
    [TestClass]
    public class CommonTests
    {
        private const int StartIndex = 2;
        private const int Length = 6;

        internal static char[] values = new char[10];
        internal static char[] tmp = new char[10];
        internal static sbyte j;
        internal static byte l;

        internal static char GetChar(in int i) => (char)('A' + i);
        private static ConverterIn<int, char> GetGetChar(sbyte offset) => (in int i) => (char)('A' + ((offset + i) % values.Length));
        internal static void Fill(in char[] array, in sbyte offset = 0) => array.Fill(offset == 0 ? GetChar : GetGetChar(offset));

        [TestMethod]
        public void TestFill()
        {
            int[] values;

            void init() => values = new int[10];

            init();

            values.Fill((in int i) => i);

            void check(in int startIndex, in int length)
            {
                for (int i = startIndex; i < length; i++)

                    Assert.AreEqual(i, values[i]);
            }

            void _check() => check(0, values.Length);

            _check();

            init();

            values.Fill((in int i) => i);

            _check();

            init();

            values.Fill((in int i) => i, StartIndex, Length);

            check(StartIndex, Length);
        }

        [TestMethod]
        public void TestSetValuesTo()
        {
            int[] values;

            void init() => (values = new int[10]).Fill((in int i) => 1);

            init();

            values.SetValuesTo(10);

            void check(in int start, int length)
            {
                void areEqual(in int i) => Assert.AreEqual(1, values[i]);

                for (int i = 0; i < start; i++)

                    areEqual(i);

                WinCopies.UtilHelpers.ProcessDataRef(start, ref length, (in int i) => Assert.AreEqual(10, values[i]));

                for (int i = length; i < values.Length; i++)

                    areEqual(i);
            }

            check(0, values.Length);

            init();

            values.SetValuesTo(StartIndex, Length, 10);

            check(StartIndex, Length);

            init();

            values.AsFromType<IList>().SetValuesTo(10);

            check(0, values.Length);

            init();

            values.AsFromType<IList>().SetValuesTo(StartIndex, Length, 10);

            check(StartIndex, Length);
        }

        [TestMethod]
        public void ReverseTest()
        {
            IReadOnlyList<char> expected;
            IReadOnlyList<char> actual;
            sbyte m;

            void assertValues(in byte n, in byte o)
            {
                for (l = 0; l < n; l++)

                    assert();

                m = (sbyte)o;

                for (; l <= o; l++)

                    _assert(l, m--);

                for (; l < values.Length; l++)

                    assert();
            }
            void _assert(in int x, in int y) => Assert.AreEqual(expected[x], actual[y]);
            void assert() => _assert(l, l);

            for (byte i = 1; i <= 10; i++)
            {
                values = new char[i];
                tmp = new char[i];

                for (j = 0; j < values.Length; j++)

                    for (byte k = 0; k < values.Length; k++)
                    {
                        Fill(values);

                        values.CopyTo(tmp, 0);

                        values.ReverseTo(j, k);

                        if (k < j)
                        {
                            IReadOnlyList<char> getArray(in char[] array) => new CircularArray<char>(array, j);

                            expected = getArray(tmp);
                            actual = getArray(values);

                            assertValues(0, (byte)(values.Length - j + k));
                        }

                        else
                        {
                            expected = tmp;
                            actual = values;

                            assertValues((byte)j, k);
                        }
                    }
            }
        }

        [TestMethod]
        public void RemoveTest()
        {
            void assert(in System.Array expected, System.Array actual)
            {
                Assert.AreEqual(expected.Length, actual.Length);

                for (int i = 0; i < expected.Length; i++)

                    Assert.AreEqual(expected.GetValue(i), actual.GetValue(i));
            }

            for (int i = 1; i <= 10; i++)
            {
                values = new char[i];

                Fill(values);

                for (j = 0; j < values.Length; j++)
                {
                    for (byte k = 0; k <= values.Length - j; k++)
                    {
                        tmp = values.Remove(j, k);

                        Assert.AreEqual(values.Length - k, tmp.Length);

                        for (l = 0; l < j; l++)

                            Assert.AreEqual(values[l], tmp[l]);

                        for (; l < tmp.Length; l++)

                            Assert.AreEqual(values[l + k], tmp[l]);
                    }

                    for (byte k = 0; k <= j + 1; k++)

                        assert(values.Remove(j, -k), values.Remove(values.Length - j - 1, k));
                }
            }
        }
    }

    [TestClass]
    public class ShiftTests
    {
        private static bool _Shift(char[] array, int offset, in int inStart, int outStart, in sbyte length, ref int overflow, in ConverterIn<int, int> expectedROut, out int lOut, out int rOut)
        {
            sbyte __offset = (sbyte)(offset % values.Length);
#if CS8
            static
#endif
            void assert(in int __rOut) => Assert.AreEqual(-1, __rOut);

            int _overflow = 0;

            if (array.LossyShift(inStart, length, ref offset, ref _overflow, out lOut, out rOut))
            {
                array.CopyTo(tmp, 0);

                Fill(array);

                Assert.AreEqual(__offset, (sbyte)offset);
            }

            else
            {
                assert(rOut);

                Assert.AreEqual((sbyte)0, (sbyte)offset);
            }

            int _rOut;

            if (offset != 0)
            {
                int __overflow = offset < 0 ? 1 : 0;

                if (array.LossyShiftByPosition(inStart, length, ref outStart, ref __overflow, out int _offset, out _rOut))
                {
                    Assert.AreEqual(offset, offset < 0 && __overflow == 0 ? -(values.Length - _offset) : _offset);

                    if (offset < 0 && System.Math.Abs(length) == values.Length)

                        return true;

                    if (offset > 0 || _overflow == __overflow)
                    {
                        array.ProcessData2((in int i) => Assert.AreEqual(array[i], tmp[i]));

                        if (offset < 0)
                        {
                            _overflow = length;

                            _ = WinCopies.UtilHelpers.CheckOverflow(values.Length, ref _overflow, values.Length + offset);
                        }

                        else

                            Assert.AreEqual(overflow, _overflow);

                        Assert.AreEqual(_overflow, __overflow);
                    }

                    else

                        values = tmp;

                    if (offset < 0 && length == values.Length)

                        values = tmp;

                    return true;
                }

                Assert.AreEqual(0, _offset);
            }

            overflow = _overflow;
            _rOut = -1;

            assert(_rOut);

            return false;
        }
        private static bool Shift(in char[] array, int offset, in int inStart, in int outStart, in sbyte length, ref int overflow, in ConverterIn<int, int> converter, out int lOut, out int rOut)
        {
            Fill(array);

            return _Shift(array, offset, inStart, outStart, length, ref overflow, converter, out lOut, out rOut);
        }

        private static int GetOutStart(in sbyte offset)
        {
            if (offset < 0)
            {
                int _offset = offset;

                return WinCopies.UtilHelpers.GetIndex(j, values.Length, ref _offset);
            }

            return (offset + j) % values.Length;
        }

        private static ConverterIn<int, int> GetLOutToROutConverter(sbyte length, in sbyte offset)
        {
            if (length == 10)
            {
                int i = offset - 1;

                return (in int lOut) => i++ % values.Length;
            }

            length--;

            return (in int lOut) => (lOut + length) % values.Length;
        }

        private static byte n;
        private bool increment;

        private static void Check(sbyte offset, sbyte length, int overflow, ConverterIn<int, int> _converter)
        {
            var array = new char[values.Length];
            IReadOnlyList<char> chars;
            byte o;
            sbyte k;
            int absLength = System.Math.Abs(length);
            int overflowLength = absLength;
            bool _overflow = WinCopies.UtilHelpers.CheckOverflow(values.Length, ref overflowLength, offset);
            int i;
            int outStart = 0;
            bool increment = false;

            void update()
            {
                array[o] = array[k];

                void __update(ref byte b) => b = (byte)((b + 1) % values.Length);

                byte _k = (byte)k;

                __update(ref _k);

                k = (sbyte)_k;

                __update(ref o);
            }

            void _update() => array[o == 0 ? (o = (byte)(values.Length - 1)) : --o] = array[k == 0 ? (k = (sbyte)(values.Length - 1)) : --k];

            Action action = offset < 0 ? _overflow ? () =>
            {
                if (System.Math.Abs(offset) < absLength && n == 0)
                {
                    k = 0;
                    o = (byte)(values.Length + offset);

                    for (i = 0; i < -offset; i++)

                        update();

                    k = (sbyte)-offset;
                    o = 0;
                    int _overflowLength = values.Length /*- overflowLength*/ + offset /*+ overflowLength*/;

                    for (i = -offset; i < _overflowLength; i++)

                        update();
                }

                else
                {
                    for (i = absLength - 1 - n; i > 0; i--)

                        array[i + absLength - 2 - n] = array[i - 1];

                    increment = true;
                }
            }
            :
#if !CS9
            (Action)(
#endif
            () =>
            {
                k = 0;
                o = (byte)(values.Length + offset);
                int l = absLength;

                for (i = 0; i < l; i++)

                    update();
            }
#if !CS9
            )
#endif
            : _overflow ? () =>
            {
                o = (byte)values.Length;
                k = (sbyte)(o - offset);

                //for (i = 1; i < values.Length - (values.Length - overflowLength); i++)

                byte l = (byte)((absLength + offset) % values.Length);

                bool check() => k > l;

                if (check())
                {
                    do

                        _update();

                    while (check());

                    char c;

                    for (i = 0; i < l; i++)

                        array[i] = (char)((c = array[i == 0 ? values.Length - 1 : i - 1]) == 'A' + values.Length - 1 ? 'A' : c + 1);
                }

                else
                {
                    byte m = (byte)((l -= n) * 2);

                    for (i = l; i < m; i++)

                        array[i - l + n] = array[i + n];

                    increment = true;
                }
            }
            :
#if !CS9
            (Action)(
#endif
            () =>
            {
                k = (sbyte)absLength;
                o = (byte)((k + offset) % values.Length);

                while (k > 0)

                    _update();
            }
#if !CS9
            )
#endif
            ;

            for (j = 0; j < values.Length; j++)
            {
                values = new char[10];

                Assert.IsTrue(Shift(values, offset, j, outStart = GetOutStart(offset), length, ref overflow, _converter, out int lOut, out int rOut));

                Fill(array, j);

                action();

                chars = new Collections.Generic.CircularArray<char>(array, -j);

                if (length < 0)

                    values.ReverseTo(lOut, rOut);

                for (k = 0; k < values.Length; k++)

                    Assert.AreEqual(chars[k], values[k], $"o: {offset} ; i: {k} ; j: {j}");
            }

            if (increment && length < 0)

                n++;
        }
        private static void Check(in sbyte offset, in sbyte length, in ConverterIn<int, int> converter)
        {
            int _length = System.Math.Abs(length);

            _ = WinCopies.UtilHelpers.CheckOverflow(values.Length, ref _length, offset);

            Check(offset, length, _length, converter);
        }

        private static class TestMethods
        {
            private static byte _offset = 0;

            public static void Check1(sbyte offset)
            {
                void check(in sbyte length) => Check(offset, length, Delegates.SelfIn);

                check(1);
                check(-1);
            }

            public static void Check2To9(sbyte offset)
            {
                void check(in sbyte b) => Check(offset, b, GetLOutToROutConverter(b, offset));

                for (sbyte b = 2; b < 10; b++)
                {
                    check(b);
                    check((sbyte)-b);
                }
            }

            public static void Check10(sbyte offset)
            {
                int overflow = 0;
                ConverterIn<int, int> _converter;
                IReadOnlyList<char> array = null;

                void check(in sbyte length)
                {
                    _converter = GetLOutToROutConverter(length, offset);

                    Assert.IsTrue(Shift(values, offset, j, GetOutStart(offset), length, ref overflow, _converter, out int lOut, out int rOut));

                    void assert(in ConverterIn<int, char> x, in ConverterIn<int, char> y)
                    {
                        for (int k = 0; k < values.Length; k++) // TODO: should execute for each value of j.

                            Assert.AreEqual(x(k), y(k), $"o: {offset} ; i: {k} ; j: {j}");
                    }

                    if (/*j == 0 &&*/ offset > 0)
                    {
                        if (length < 0)
                        {
                            values.ReverseTo(lOut, rOut);

                            assert(Delegates.GetIndexerIn<char>(tmp), Delegates.GetIndexerIn<char>(values));
                        }

                        else
                        {
                            values.CopyTo(tmp, 0);

                            array = new CircularReadOnlyList<char>(values, offset);

                            _offset = _offset.GetLowPart();

                            ConverterIn<int, char> converter = (in int _i) =>
                            {
                                if (offset > values.Length / 2 && offset == values.Length - _i)
                                {
                                    _offset += 2;

                                    return (converter = (in int __i) =>
                                    {
                                        if (_offset.GetHighPart() == _offset.GetLowPart())

                                            return (converter = GetChar)(__i);

                                        char result = GetChar(_offset.GetHighPart());

                                        _offset = ((byte)(_offset.GetHighPart() + 1)).ConcatenateHalfParts(_offset.GetLowPart());

                                        return result;
                                    })(_i);
                                }

                                return _i == offset ? (converter = GetChar)(_i) : GetChar(offset + _i);
                            };

                            assert(converter, Delegates.GetIndexerIn(array));
                        }
                    }
                }

                /*for (j = 0; j < values.Length; j++)
                {*/
                j = 0;

                check(10);
                check(-10);
                //}
            }
        }

        [TestMethod]
        public void LossyTest()
        {
            Fill(values);

            byte i;

            for (i = 0; i < values.Length; i++)

                Assert.AreEqual(values[i], GetChar(i));
#if CS8
            static
#endif
            int getExpectedROut(in int _lOut) => 0;

            int lOut, rOut, overflow = 0;

            for (i = 0; i <= values.Length; i++)

                for (j = 0; j < values.Length; j++)
                {
                    Assert.IsFalse(_Shift(values, i, j, i, 0, ref overflow, getExpectedROut, out lOut, out rOut));
                    Assert.IsFalse(_Shift(values, -i, j, (values.Length - i) % values.Length, 0, ref overflow, getExpectedROut, out lOut, out rOut));
                }

            for (i = 0; i < values.Length; i++)

                for (j = 0; j <= values.Length; j++)

                    Assert.IsFalse(_Shift(values, 0, i, 0, j, ref overflow, getExpectedROut, out lOut, out rOut));

            for (i = 0; i < values.Length; i++)

                for (j = 0; j <= values.Length; j++)

                    Assert.IsFalse(_Shift(values, 0, i, i, j, ref overflow, getExpectedROut, out lOut, out rOut));

            var args = new object[1];
            void update(in bool positive) => args[0] = positive ? (sbyte)i : (sbyte)-i;

            foreach (MethodInfo method in typeof(TestMethods).GetMethods(BindingFlags.Public | BindingFlags.Static))

                for (i = 1; i < values.Length; i++)
                {
                    values = new char[10];
                    n = 0;

                    update(true);
                    _ = method.Invoke(null, args);

                    values = new char[10];
                    n = 0;

                    update(false);
                    _ = method.Invoke(null, args);
                }
        }
#if WinCopies4
        [TestMethod]
        public void ResetTest()
        {
            sbyte i;
            int avoidOverflow = 0;
            int _length;
            IQueueBase<char> chars = EnumerableHelper<char>.GetQueue();

            void assert(int offset, byte length, byte start)
            {
                Fill(values);

                _length = length + start;

                for (i = (sbyte)start; i < _length; i++)

                    chars.Enqueue(values[i % values.Length]);

                Assert.IsTrue(values.ShiftAndReset(start, length, ref offset, ref avoidOverflow, out int lOut, out int rOut, _i => (char)_i));

                void _assert() => Assert.AreEqual((sbyte)(i % values.Length), (sbyte)values[i % values.Length]);
                void dequeueAndAssert() => Assert.AreEqual(chars.Dequeue(), values[i % values.Length]);

                if (offset > 0)
                {
                    offset += start;
                    offset %= values.Length;

                    if (offset + length <= values.Length || length == 1)
                    {
                        for (i = 0; i < offset; i++)

                            _assert();

                        for (i = (sbyte)(offset + length); i < values.Length; i++)

                            _assert();
                    }

                    else

                        for (i = (sbyte)(rOut + 1); i < lOut; i++)

                            _assert();

                    for (i = (sbyte)offset; i < offset + length; i++)

                        dequeueAndAssert();
                }

                else
                {
                    bool check() => i < lOut;

                    i = (sbyte)((rOut + 1) % values.Length);

                    if (check())

                        do
                        {
                            _assert();

                            i++;
                        }

                        while (check());

                    else

                        for (; i < lOut + values.Length; i++)

                            _assert();

                    _length = lOut + length;

                    for (i = (sbyte)lOut; i < _length; i++)

                        dequeueAndAssert();
                }

                Assert.IsFalse(chars.HasItems);
            }

            for (byte j = 1; j < values.Length - 1; j++)

                for (sbyte k = 1; k < 10; k++)

                    for (byte l = 0; l < 10; l++)

                        if (j + k <= values.Length || j == 1) // TODO: should execute also when false.
                        {
                            void _assert(sbyte _k) => assert(_k, j, l);

                            _assert(k);
                            _assert((sbyte)-k);
                        }
        }
#endif
        [TestMethod]
        public void LosslessTest()
        {
            char[] chars;
            byte start = 0, length = 0, j;
            sbyte k;
            int avoidOverflow;
            byte m = 0, n = 0;

            void assert(int offset, ISimpleLinkedListCommon<char> items)
            {
                Fill(values);

                void process()
                {
                    chars = new char[values.Length];

                    char c;

                    System.Array.Copy(values, chars, values.Length);

                    for (sbyte i = (sbyte)(length - 1); i >= 0; i--)
                    {
                        k = (k = (sbyte)(((j = (byte)((i + start) % values.Length)) + offset) % values.Length)) < 0 ? (sbyte)(values.Length + k) : k;

                        c = chars[k];

                        chars[k] = values[j % values.Length];

                        items.Add(c);
                    }
                }

                process();

                int _offset = System.Math.Abs(offset);
                j = 0;
                int __offset;
                Func<bool> check;

                Action postWork = null;

                if (increment = length > _offset)
                {
                    __offset = l;
                    check = () =>
                    {
                        if (j++ < l)

                            return true;

                        items.Clear();

                        return false;
                    };
                }

                else
                {
                    __offset = length;
                    check = () => items.HasItems;
                }

                /*int __offset = -_offset + 1;

                if (length > _offset)

                    __offset += (k - start - 1);

                k = (byte)WinCopies.UtilHelpers.GetIndex(k, values.Length, ref __offset);*/

                k = (sbyte)WinCopies.UtilHelpers.GetIndex(start, values.Length, ref __offset);

                void writeChar(in int index) => chars[index] = items.Remove();

                ActionIn<int> writeCharAction;

                if (offset < -1 && length > 1)
                {
                    void _writeChar(in int index)
                    {
                        writeChar(index);

                        n = (byte)index;
                    }

                    writeCharAction = (in int index) =>
                    {
                        _writeChar(index);

                        m = (byte)index;

                        writeCharAction = _writeChar;
                    };

                    postWork = () => chars.ReverseTo(n, m);
                }

                else

                    writeCharAction = writeChar;

                bool _check()
                {
                    if (check())

                        return true;

                    postWork?.Invoke();

                    return false;
                }

                while (_check())
                {
                    k = (sbyte)((k == 0 ? values.Length : k) - 1);

                    writeCharAction(offset < 0 && length > _offset ? (sbyte)((k + (length - _offset)) % values.Length) : k);
                }

                Assert.IsFalse(items.HasItems);

                avoidOverflow = 0;

                Assert.IsTrue(values.LosslessShift(start, length, ref offset, ref avoidOverflow, out _, out _));

                for (k = 0; k < values.Length; k++)

                    Assert.AreEqual(chars[k], values[k]);
            }

            void _assert(in ISimpleLinkedListCommon<char> _list, in ConverterIn<int, int> converter)
            {
                int ___offset = 0;

                for (start = 0; start < values.Length; start++)

                    for (length = 1; length <= values.Length; length++)
                    {
                        increment = false;
                        l = 1;

                        for (___offset = 1; ___offset < values.Length; ___offset++)

                            if (___offset + length <= values.Length) // TODO: should execute for all values.
                            {
                                if (increment)
                                {
                                    increment = false;
                                    l++;
                                }

                                assert(converter(___offset), _list);
                            }
                    }
            }

            EnumerableHelper<char>.ILinkedList list = EnumerableHelper<char>.GetLinkedList();

            _assert(list.AsQueue(), Delegates.SelfIn);

            list.Clear();

            _assert(list.AsStack(), (in int ___offset) => -___offset);
        }
    }
}
