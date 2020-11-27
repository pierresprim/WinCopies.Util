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
using System.Collections.Generic;
using System.Linq;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections.DotNetFix.Generic.Tests
{
    [TestClass]
    public class LinkedListTests
    {
        public WinCopies.Collections.DotNetFix.Generic.LinkedList<int> LinkedList { get; } = new LinkedList<int>();
        public WinCopies.Collections.DotNetFix.Generic.Queue<ILinkedListNode<int>> NodeQueue { get; } = new Queue<ILinkedListNode<int>>();

        void AssertCleared()
        {
            Assert.AreEqual(0u, LinkedList.Count, "LinkedList should be empty.");

            Assert.IsNull(LinkedList.First, $"{nameof(LinkedList)}.{nameof(LinkedList.First)} should be null.");

            Assert.IsNull(LinkedList.Last, $"{nameof(LinkedList)}.{nameof(LinkedList.Last)} should be null.");

            ILinkedListNode<int> node;
            uint i = 0;

            while (NodeQueue.Count != 0)
            {
                i++;

                node = NodeQueue.Dequeue();

                Assert.IsNull(node.Previous);

                Assert.IsNull(node.Next);

                Assert.IsNull(node.List);
            }

            Assert.AreEqual(10u, i);
        }

        void AddNode(Func<ILinkedListNode<int>> action, int expected)
        {
            ILinkedListNode<int> node = action();

            Assert.AreEqual(expected, node.Value);

            NodeQueue.Enqueue(node);
        }

        [TestMethod]
        public void TestAddFirstAndLast()
        {
            WinCopies.Collections.DotNetFix.Generic.ILinkedListNode<int> node;

            for (int i = 1; i < 11; i++)

                AddNode(() => LinkedList.AddFirst(i), i);

            Assert.AreEqual(10u, LinkedList.Count, "AddFirst assertion fialed: Count should be 10.");

            Assert.AreEqual(10, LinkedList.First.Value);

            Assert.AreEqual(1, LinkedList.Last.Value);

            int j = 10;

            foreach (int i in LinkedList)

                Assert.AreEqual(j--, i, "Error during enumeration.");

            Assert.AreEqual(0, j, "Enumeration failed.");

            LinkedList.Clear();

            node = null;

            AssertCleared();



            for (int i = 1; i < 11; i++)

                AddNode(() => LinkedList.AddLast(i), i);

            Assert.AreEqual(10u, LinkedList.Count, "AddLast assertion failed: Count should be 10.");

            Assert.AreEqual(1, LinkedList.First.Value);

            Assert.AreEqual(10, LinkedList.Last.Value);

            j = 0;

            foreach (int i in LinkedList)

                Assert.AreEqual(++j, i, "Error during enumeration.");

            Assert.AreEqual(10, j, "Enumeration failed.");

            LinkedList.Clear();

            node = null;

            AssertCleared();
        }

        [TestMethod]
        public void TestEnumeration()
        {
            static void assertValues(System.Collections.Generic.IEnumerable<int> enumerable, int init, Func<int, int> newValue)
            {
                int i = init;

                foreach (int value in enumerable)

                    Assert.AreEqual((i = newValue(i)), value);
            }

            for (int i = 1; i < 11; i++)

                AddNode(() => LinkedList.AddLast(i), i);

            assertValues(LinkedList, 0, i => i + 1);

            assertValues(new Enumerable<int>(() => LinkedList.GetEnumerator()), 0, i => i + 1);

            assertValues(new Enumerable<int>(() => LinkedList.GetEnumerator(EnumerationDirection.FIFO)), 0, i => i + 1);

            assertValues(new Enumerable<int>(() => LinkedList.GetReversedEnumerator()), 11, i => i - 1);

            assertValues(new Enumerable<int>(() => LinkedList.GetEnumerator(EnumerationDirection.LIFO)), 11, i => i - 1);

            assertValues(new Enumerable<ILinkedListNode<int>>(() => LinkedList.GetNodeEnumerator(EnumerationDirection.FIFO)).Select(node => node.Value), 0, i => i + 1);

            assertValues(new Enumerable<ILinkedListNode<int>>(() => LinkedList.GetNodeEnumerator(EnumerationDirection.LIFO)).Select(node => node.Value), 11, i => i - 1);

            LinkedList.Clear();

            AssertCleared();
        }

        [TestMethod]
        public void TestRemoveFirst()
        {
            for (int i = 1; i < 11; i++)

                AddNode(() => LinkedList.AddLast(i), i);

            ILinkedListNode<int> node = LinkedList.First;

            void removeAndAssert()
            {
                node = node.Next;

                LinkedList.RemoveFirst();

                Assert.AreEqual(node, LinkedList.First);
            }

            while (LinkedList.Count != 2)

                removeAndAssert();

            Assert.AreEqual(LinkedList.First.Next, LinkedList.Last);
            Assert.AreEqual(LinkedList.Last.Previous, LinkedList.First);

            removeAndAssert();

            Assert.AreEqual(LinkedList.First, LinkedList.Last);

            removeAndAssert();

            Assert.AreEqual(null, LinkedList.First);
            Assert.AreEqual(null, LinkedList.Last);

            Assert.AreEqual(null, node);

            AssertCleared();
        }

        [TestMethod]
        public void TestRemoveLast()
        {
            for (int i = 1; i < 11; i++)

                AddNode(() => LinkedList.AddLast(i), i);

            ILinkedListNode<int> node = LinkedList.Last;

            void removeAndAssert()
            {
                node = node.Previous;

                LinkedList.RemoveLast();

                Assert.AreEqual(node, LinkedList.Last);
            }

            while (LinkedList.Count != 2)

                removeAndAssert();

            Assert.AreEqual(LinkedList.First.Next, LinkedList.Last);
            Assert.AreEqual(LinkedList.Last.Previous, LinkedList.First);

            removeAndAssert();

            Assert.AreEqual(LinkedList.First, LinkedList.Last);

            removeAndAssert();

            Assert.AreEqual(null, LinkedList.First);
            Assert.AreEqual(null, LinkedList.Last);

            Assert.AreEqual(null, node);

            AssertCleared();
        }
    }
}
