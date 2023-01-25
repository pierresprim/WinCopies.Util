/* Copyright © Pierre Sprimont, 2021
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

namespace WinCopies.Util.Tests
{
    interface _IA
    { }

    interface _IB : _IA
    { }

    interface _IC : _IB
    { }

    class _A : _IA
    { }

    class _B : _A, _IB
    { }

    class _C : _B, _IC
    { }

    [TestClass]
    public partial class ExtensionsTests
    {
        [TestMethod]
        public void TestIsAssignableFrom()
        {
            Assert.IsTrue(typeof(_IA).IsAssignableFrom<_IA>());
            Assert.IsTrue(typeof(_IA).IsAssignableFrom<_IB>());
            Assert.IsTrue(typeof(_IA).IsAssignableFrom<_IC>());

            Assert.IsFalse(typeof(_IB).IsAssignableFrom<_IA>());
            Assert.IsTrue(typeof(_IB).IsAssignableFrom<_IB>());
            Assert.IsTrue(typeof(_IB).IsAssignableFrom<_IC>());

            Assert.IsFalse(typeof(_IC).IsAssignableFrom<_IA>());
            Assert.IsFalse(typeof(_IC).IsAssignableFrom<_IB>());
            Assert.IsTrue(typeof(_IC).IsAssignableFrom<_IC>());



            Assert.IsFalse(typeof(_A).IsAssignableFrom<_IA>());
            Assert.IsFalse(typeof(_A).IsAssignableFrom<_IB>());
            Assert.IsFalse(typeof(_A).IsAssignableFrom<_IC>());

            Assert.IsFalse(typeof(_B).IsAssignableFrom<_IA>());
            Assert.IsFalse(typeof(_B).IsAssignableFrom<_IB>());
            Assert.IsFalse(typeof(_B).IsAssignableFrom<_IC>());

            Assert.IsFalse(typeof(_C).IsAssignableFrom<_IA>());
            Assert.IsFalse(typeof(_C).IsAssignableFrom<_IB>());
            Assert.IsFalse(typeof(_C).IsAssignableFrom<_IC>());



            Assert.IsTrue(typeof(_IA).IsAssignableFrom<_A>());
            Assert.IsTrue(typeof(_IA).IsAssignableFrom<_B>());
            Assert.IsTrue(typeof(_IA).IsAssignableFrom<_C>());

            Assert.IsFalse(typeof(_IB).IsAssignableFrom<_A>());
            Assert.IsTrue(typeof(_IB).IsAssignableFrom<_B>());
            Assert.IsTrue(typeof(_IB).IsAssignableFrom<_C>());

            Assert.IsFalse(typeof(_IC).IsAssignableFrom<_A>());
            Assert.IsFalse(typeof(_IC).IsAssignableFrom<_B>());
            Assert.IsTrue(typeof(_IC).IsAssignableFrom<_C>());



            Assert.IsTrue(typeof(_A).IsAssignableFrom<_A>());
            Assert.IsTrue(typeof(_A).IsAssignableFrom<_B>());
            Assert.IsTrue(typeof(_A).IsAssignableFrom<_C>());

            Assert.IsFalse(typeof(_B).IsAssignableFrom<_A>());
            Assert.IsTrue(typeof(_B).IsAssignableFrom<_B>());
            Assert.IsTrue(typeof(_B).IsAssignableFrom<_C>());

            Assert.IsFalse(typeof(_C).IsAssignableFrom<_A>());
            Assert.IsFalse(typeof(_C).IsAssignableFrom<_B>());
            Assert.IsTrue(typeof(_C).IsAssignableFrom<_C>());
        }
    }
}
