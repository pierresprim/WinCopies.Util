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

namespace WinCopies.Util.Tests
{
    [TestClass]
    public
#if !WinCopies3
        partial
#endif
        class Util
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
        public void GetAllEnumFlags()
        {
            try
            {
                _ =
#if WinCopies3
WinCopies.UtilHelpers
#else
                    WinCopies.Util.Util
#endif
                    .GetAllEnumFlags<TestEnum>();

                // We shouldn't reach this code. So, if we do, we throw an assertion exception.

                Assert.Fail();
            }
            catch (ArgumentException) { }

            TestEnumFlags result =
#if WinCopies3
WinCopies.UtilHelpers
#else
                    WinCopies.Util.Util
#endif
                    .GetAllEnumFlags<TestEnumFlags>();

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
    }
}
