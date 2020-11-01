using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies;

namespace WinCopies.Util.Tests
{
    [TestClass]
    public class Util
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
    }
}
