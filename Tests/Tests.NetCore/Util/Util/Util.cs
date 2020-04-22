using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Collections;
using WinCopies.Util;

namespace WinCopies.Tests.Util.Util
{
    [TestClass]
    public class Util
    {
        public enum TestEnum:ushort
        {
            None=0,

            Value1=1,

            Value2=2,

            Value3=3
        }

        [Flags]
        public enum TestEnumFlags
        {
            None = 0,

            Value1 = 1,

            Value2 = 2,

            Value3 = 4,

            Value4=8
        }

        [TestMethod]
        public void GetAllEnumFlags()
        {

            try
            {

                _ = WinCopies.Util.Util.GetAllEnumFlags<TestEnum>();

                // We shouldn't reach this code. So, if we do, we throw an assertion exception.

                Assert.Fail();

            }
            catch (ArgumentException) { }

            TestEnumFlags result = WinCopies.Util.Util.GetAllEnumFlags<TestEnumFlags>();

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

            testEnumFlags = TestEnumFlags.Value1| TestEnumFlags.Value2;

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

            testEnumFlags = TestEnumFlags.Value1 | TestEnumFlags.Value2| TestEnumFlags.Value3;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value1 | TestEnumFlags.Value2 | TestEnumFlags.Value4;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());

            testEnumFlags = TestEnumFlags.Value1 | TestEnumFlags.Value2 | TestEnumFlags.Value3| TestEnumFlags.Value4;

            Assert.IsTrue(testEnumFlags.HasMultipleFlags());

        }

        [TestMethod]
        public void GetIf()
        {

            char[] expectedResult = { 'a', 'b', 'c' };

            char[] _result = new char[3];

            for (int i = -1; i <= 1; i++)

                _result[i + 1] = WinCopies.Util.Util.GetIf(i, 0, new Comparison((x, y) => ((int)x).CompareTo((int)y)), () => 'a', () => 'b', () => 'c');

            for (int i = 0; i <= 2; i++)

                Assert.AreEqual(expectedResult[i], _result[i]);

            for (int i = -1; i <= 1; i++)

                _result[i + 1] = (char)WinCopies.Util.Util.GetIf(i, 0, System.Collections.Generic.Comparer<int>.Default, () => 'a', () => 'b', () => 'c');

            for (int i = 0; i <= 2; i++)

                Assert.AreEqual(expectedResult[i], _result[i]);

        }
    }
}
