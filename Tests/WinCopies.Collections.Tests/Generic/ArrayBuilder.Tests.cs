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

using System.Collections;
using System.Collections.Generic;

namespace WinCopies.Collections.Generic.Tests
{
    [TestClass]
    public class ArrayBuilder
    {
        private readonly WinCopies.Collections.Generic.ArrayBuilder<int> _arrayBuilder = new WinCopies.Collections.Generic.ArrayBuilder<int>();

        private const uint Count = 4;

        private void FillArrayBuilder()
        {
            for (int i = 0; i < Count; i++)

                _ = _arrayBuilder.AddLast(i);
        }

        private void AssertArray(in int[] array, in uint times)
        {
            uint count = Count * times;

            Assert.AreEqual((int)count, array.Length);

            uint _count = Count * (times - 1);

            uint i, j;

            for (i = 0; i < _count; i++)

                Assert.AreEqual(0, array[i]);

            for (i = _count, j = 0; i < count; i++, j++)

                Assert.AreEqual((int)j, array[i]);
        }

        [TestMethod]
        public void ToArrayTest1()
        {
            FillArrayBuilder();

            int[] array = _arrayBuilder.ToArray();

            _arrayBuilder.Clear();

            AssertArray(array, 1);
        }

        [TestMethod]
        public void ToArrayTest2()
        {
            FillArrayBuilder();

            int[] array = _arrayBuilder.ToArray(true);

            Assert.AreEqual(0u, _arrayBuilder.Count);

            AssertArray(array, 1);
        }

        [TestMethod]
        public void ToArrayTest3()
        {
            FillArrayBuilder();

            int[] array = new int[4];

            _arrayBuilder.ToArray(array);

            _arrayBuilder.Clear();

            AssertArray(array, 1);
        }

        [TestMethod]
        public void ToArrayTest4()
        {
            FillArrayBuilder();

            int[] array = new int[8];

            _arrayBuilder.ToArray(array, true, 4);

            Assert.AreEqual(0u, _arrayBuilder.Count);

            AssertArray(array, 2);
        }

        private void AssertArrayList(in ArrayList arrayList, in uint times)
        {
            uint count = Count * times;

            Assert.AreEqual((int)count, arrayList.Count);

            uint _count = Count * (times - 1);

            uint i, j;

            for (i = 0; i < _count; i++)

                Assert.AreEqual(0, arrayList[(int)i]);

            for (i = _count, j = 0; i < count; i++, j++)

                Assert.AreEqual((int)j, arrayList[(int)i]);
        }

        [TestMethod]
        public void ToArrayListTest1()
        {
            FillArrayBuilder();

            var arrayList = _arrayBuilder.ToArrayList();

            _arrayBuilder.Clear();

            AssertArrayList(arrayList, 1);
        }

        [TestMethod]
        public void ToArrayListTest2()
        {
            FillArrayBuilder();

            var arrayList = _arrayBuilder.ToArrayList(true);

            Assert.AreEqual(0u, _arrayBuilder.Count);

            AssertArrayList(arrayList, 1);
        }

        [TestMethod]
        public void ToArrayListTest3()
        {
            FillArrayBuilder();

            var arrayList = new ArrayList(4);

            _arrayBuilder.ToArrayList(arrayList);

            _arrayBuilder.Clear();

            AssertArrayList(arrayList, 1);
        }

        [TestMethod]
        public void ToArrayListTest4()
        {
            FillArrayBuilder();

            var arrayList = new ArrayList(8);

            for (int i = 0; i < 4; i++)

                _ = arrayList.Add(0);

            _arrayBuilder.ToArrayList(arrayList, true, 4);

            Assert.AreEqual(0u, _arrayBuilder.Count);

            AssertArrayList(arrayList, 2);
        }

        private void AssertList(in IList<int> list, in uint times)
        {
            uint count = Count * times;

            Assert.AreEqual((int)count, list.Count);

            uint _count = Count * (times - 1);

            uint i, j;

            for (i = 0; i < _count; i++)

                Assert.AreEqual(0, list[(int)i]);

            for (i = _count, j = 0; i < count; i++, j++)

                Assert.AreEqual((int)j, list[(int)i]);
        }

        [TestMethod]
        public void ToListTest1()
        {
            FillArrayBuilder();

            IList<int> list = _arrayBuilder.ToList();

            _arrayBuilder.Clear();

            AssertList(list, 1);
        }

        [TestMethod]
        public void ToListTest2()
        {
            FillArrayBuilder();

            IList<int> list = _arrayBuilder.ToList(true);

            Assert.AreEqual(0u, _arrayBuilder.Count);

            AssertList(list, 1);
        }

        [TestMethod]
        public void ToListTest3()
        {
            FillArrayBuilder();

            IList<int> list = new List<int>(4);

            _arrayBuilder.ToList(list);

            _arrayBuilder.Clear();

            AssertList(list, 1);
        }

        [TestMethod]
        public void ToListTest4()
        {
            FillArrayBuilder();

            IList<int> list = new List<int>(8);

            for (int i = 0; i < 4; i++)

                list.Add(0);

            _arrayBuilder.ToList(list, true, 4);

            Assert.AreEqual(0u, _arrayBuilder.Count);

            AssertList(list, 2);
        }
    }
}
