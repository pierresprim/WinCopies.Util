using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WinCopies.Util;

namespace WinCopies.Tests.Collections
{
    [TestClass]
    public class ArrayBuilder
    {
        private readonly WinCopies.Collections.ArrayBuilder<int> _arrayBuilder = new WinCopies.Collections.ArrayBuilder<int>();

        private const int Count = 4;

        private void FillArrayBuilder()
        {
            for (int i = 0; i < Count; i++)

                _ = _arrayBuilder.AddLast(i);
        }

        private void AssertArray(in int[] array, in int times)
        {
            int count = Count * times;

            Assert.AreEqual(count, array.Length);

            int _count = Count * (times - 1);

            int i, j;

            for (i = 0; i < _count; i++)

                Assert.AreEqual(0, array[i]);

            for (i = _count, j = 0; i < count; i++, j++)

                Assert.AreEqual(j, array[i]);
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

            Assert.AreEqual(0, _arrayBuilder.Count);

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

            Assert.AreEqual(0, _arrayBuilder.Count);

            AssertArray(array, 2);
        }

        private void AssertArrayList(in ArrayList arrayList, in int times)
        {
            int count = Count * times;

            Assert.AreEqual(count, arrayList.Count);

            int _count = Count * (times - 1);

            int i, j;

            for (i = 0; i < _count; i++)

                Assert.AreEqual(0, arrayList[i]);

            for (i = _count, j = 0; i < count; i++, j++)

                Assert.AreEqual(j, arrayList[i]);
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

            Assert.AreEqual(0, _arrayBuilder.Count);

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

                _=arrayList.Add(0);

            _arrayBuilder.ToArrayList(arrayList, true, 4);

            Assert.AreEqual(0, _arrayBuilder.Count);

            AssertArrayList(arrayList, 2);
        }

        private void AssertList(in IList<int> list, in int times)
        {
            int count = Count * times;

            Assert.AreEqual(count, list.Count);

            int _count = Count * (times - 1);

            int i, j;

            for (i = 0; i < _count; i++)

                Assert.AreEqual(0, list[i]);

            for (i = _count, j = 0; i < count; i++, j++)

                Assert.AreEqual(j, list[i]);
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

            Assert.AreEqual(0, _arrayBuilder.Count);

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

            Assert.AreEqual(0, _arrayBuilder.Count);

            AssertList(list, 2);
        }
    }
}
