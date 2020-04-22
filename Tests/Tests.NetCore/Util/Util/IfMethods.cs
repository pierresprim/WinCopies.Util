using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinCopies.Util;
using static WinCopies.Util.Util;
using IfType = WinCopies.Util.Util.ComparisonType;
using IfMode = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;

namespace WinCopies.Tests.Util.Util
{
    [TestClass]
    public class IfMethods
    {
        [TestMethod]
        public void TestMethod()
        {
            // True
            for (int i = 0; i <= 1; i++)
            {
                Assert.IsTrue(If(IfType.And, (IfMode)i, IfComp.Equal, 0, 0, 0));
                Assert.IsTrue(If(IfType.And, (IfMode)i, IfComp.NotEqual, 0, 1, 1));
                Assert.IsTrue(If(IfType.Or, (IfMode)i, IfComp.Equal, 0, 0, 0));
                Assert.IsTrue(If(IfType.Or, (IfMode)i, IfComp.Equal, 0, 0, 1));
                Assert.IsTrue(If(IfType.Or, (IfMode)i, IfComp.Equal, 0, 1, 0));
                Assert.IsTrue(If(IfType.Or, (IfMode)i, IfComp.NotEqual, 0, 1, 1));
                Assert.IsTrue(If(IfType.Xor, (IfMode)i, IfComp.Equal, 0, 0, 1));
                Assert.IsTrue(If(IfType.Xor, (IfMode)i, IfComp.Equal, 0, 1, 0));
                Assert.IsTrue(If(IfType.Xor, (IfMode)i, IfComp.NotEqual, 0, 0, 1));
                Assert.IsTrue(If(IfType.Xor, (IfMode)i, IfComp.NotEqual, 0, 1, 0));
            }
            // False
            for (int i = 0; i <= 1; i++)
            {
                Assert.IsFalse(If(IfType.And, (IfMode)i, IfComp.NotEqual, 0, 0, 0));
                Assert.IsFalse(If(IfType.And, (IfMode)i, IfComp.Equal, 0, 1, 0));
                Assert.IsFalse(If(IfType.And, (IfMode)i, IfComp.Equal, 0, 0, 1));
                Assert.IsFalse(If(IfType.And, (IfMode)i, IfComp.Equal, 0, 1, 1));
                Assert.IsFalse(If(IfType.Or, (IfMode)i, IfComp.NotEqual, 0, 0, 0));
                Assert.IsFalse(If(IfType.Or, (IfMode)i, IfComp.Equal, 0, 1, 1));
                Assert.IsFalse(If(IfType.Xor, (IfMode)i, IfComp.Equal, 0, 0, 0));
                Assert.IsFalse(If(IfType.Xor, (IfMode)i, IfComp.NotEqual, 0, 0, 0));
                Assert.IsFalse(If(IfType.Xor, (IfMode)i, IfComp.NotEqual, 0, 1, 1, 0));
                Assert.IsFalse(If(IfType.Xor, (IfMode)i, IfComp.Equal, 0, 1, 1));
            }
        }
    }
}
