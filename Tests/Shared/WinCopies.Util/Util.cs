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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace WinCopies.Util.Tests
{
    [TestClass]
    public
#if !WinCopies3
        partial
#endif
        class UtilTests
    {
        public enum TestEnum : ushort
        {
            None = 0,

            Value1 = 1,

            Value2 = 2,

            Value3 = 3
        }

        [Flags]
        public enum TestEnumFlags
        {
            None = 0,

            Value1 = 1,

            Value2 = 2,

            Value3 = 4,

            Value4 = 8
        }

        [TestMethod]
        public void GetIndex()
        {
            const int length = 10;
            int offset;
            string msg;

            void __check(in int _offset, in ConverterIn<int, int> _converter)
            {
                offset = _offset;

                for (int i = 0; i < length; i++)
                {
                    msg = $"o: {offset}; i: {i}";

                    Assert.AreEqual(_converter(i), UtilHelpers.GetIndex(i, length, ref offset), msg);

                    Assert.AreEqual(_offset % length, offset);
                }
            }

            void _check(in int _offset, in Func<ConverterIn<int, int>> indexConverterProvider, in ConverterIn<int, int> offsetConverter)
            {
                __check(_offset, indexConverterProvider());
                __check(offsetConverter(_offset), indexConverterProvider());
            }

            int getNewNegativeOffset(in int _offset) => _offset - 10;

            void checkN(in int _offset, in Func<ConverterIn<int, int>> converterProvider) => _check(-_offset, converterProvider, getNewNegativeOffset);
            void checkP(in int _offset, in Func<ConverterIn<int, int>> converterProvider) => _check(_offset, converterProvider, (in int __offset) => __offset + 10);

            void check(in ActionIn<int, Func<ConverterIn<int, int>>> action, in Converter<int, Func<ConverterIn<int, int>>> converterProvider)
            {
                for (int i = 1; i < length; i++)

                    action(i, converterProvider(i));
            }

            ConverterIn<int, int> converter = null;

            check(checkN, i => () =>
            {
                int __i = i;

                converter = (in int _i) => __i == 0 ? (converter = (in int ___i) => ___i - i)(_i) : length - __i--;

                return (in int _i) => converter(_i);
            });

            _check(0, () => Delegates.SelfIn, getNewNegativeOffset);
            checkP(0, () => Delegates.SelfIn);

            check(checkP, i => () => (in int _i) => (i + _i) % length);
        }

        [TestMethod]
        public void GetAllEnumFlags()
        {
            try
            {
                _ = UtilHelpers.GetAllEnumFlags<TestEnum>();

                // We shouldn't reach this code. So, if we do, we throw an assertion exception.

                Assert.Fail();
            }
            catch (ArgumentException) { }

            TestEnumFlags result = UtilHelpers.GetAllEnumFlags<TestEnumFlags>();

            foreach (object _enum in typeof(TestEnumFlags).GetEnumValues())

                Assert.IsTrue(result.HasFlag((TestEnumFlags)_enum));
        }

        [TestMethod]
        public void GetNumValue() => Assert.IsInstanceOfType(TestEnum.None.GetNumValue(), typeof(ushort));

        [TestMethod]
        public void HasMultipleFlags()
        {
            var testEnum = TestEnum.None;

            Assert.IsFalse(testEnum.HasMultipleFlags());

            testEnum = TestEnum.Value1;

            Assert.IsFalse(testEnum.HasMultipleFlags());

            var testEnumFlags = TestEnumFlags.None;

            Assert.IsFalse(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value1;

            Assert.IsFalse(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value2;

            Assert.IsFalse(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value3;

            Assert.IsFalse(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value4;

            Assert.IsFalse(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value1 | TestEnumFlags.Value2;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value3 | TestEnumFlags.Value4;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value1 | TestEnumFlags.Value3;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value1 | TestEnumFlags.Value4;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value2 | TestEnumFlags.Value3;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value2 | TestEnumFlags.Value4;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value1 | TestEnumFlags.Value2 | TestEnumFlags.Value3;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value1 | TestEnumFlags.Value2 | TestEnumFlags.Value4;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value1 | TestEnumFlags.Value2 | TestEnumFlags.Value3 | TestEnumFlags.Value4;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());
        }

        [TestMethod]
        public async Task TryWaitWhile()
        {
            bool b = true;

            int i = 0;

            var t = new Thread(() =>
            {
                while (i < 10)

                    Thread.Sleep(100);

                b = false;
            });

            t.Start();

            await UtilHelpers.TryWaitWhile((ref bool cancel) =>
                {
                    i++;
                    return b;
                }).ConfigureAwait(false);

            Assert.IsFalse(b);
        }
    }
}
