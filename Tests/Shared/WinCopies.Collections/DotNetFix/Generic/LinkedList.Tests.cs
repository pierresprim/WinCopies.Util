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
using WinCopies.Util;

namespace WinCopies.Collections.DotNetFix.Generic.Tests
{
    [TestClass]
    public class LinkedListTests
    {
        public LinkedList<int> LinkedList { get; } = new LinkedList<int>();
        public IEnumerable<int> Enumerable => LinkedList;

        public Queue<ILinkedListNode<int>> NodeQueue { get; } = new Queue<ILinkedListNode<int>>();

        static void AssertCleared(ILinkedList<int> list, Queue<ILinkedListNode<int>> queue, bool assertNodeQueueCleared = true)
        {
            Assert.AreEqual(0u, list.AsFromType<IUIntCountable>().Count, "list should be empty.");

            Assert.IsNull(list.First, $"{nameof(list)}.{nameof(list.First)} should be null.");

            Assert.IsNull(list.Last, $"{nameof(list)}.{nameof(list.Last)} should be null.");

            ILinkedListNode<int> node;
            uint i = 0;

            while (queue.Count != 0)
            {
                i++;

                node = queue.Dequeue();

                Assert.IsNull(node.AsLinkedListNode().Previous);
                Assert.IsNull(node.AsLinkedListNode().Next);

                Assert.IsNull(node.List);
            }

            if (assertNodeQueueCleared)

                Assert.AreEqual(10u, i);
        }

        static void AddNode(Func<ILinkedListNode<int>> action, int expected, Queue<ILinkedListNode<int>> queue)
        {
            ILinkedListNode<int> node = action();

            Assert.AreEqual(expected, node.Value);

            queue.Enqueue(node);
        }

        [TestMethod]
        public void TestAddFirstAndLast() => TestAddFirstAndLastStatic(LinkedList, NodeQueue);

        public static void TestAddFirstAndLastStatic(ILinkedList<int> list, Queue<ILinkedListNode<int>> queue)
        {
            for (int i = 1; i < 11; i++)

                AddNode(() => list.AddFirst(i), i, queue);

            Assert.AreEqual(10u, list.AsFromType<IUIntCountable>().Count, "AddFirst assertion fialed: Count should be 10.");

            Assert.AreEqual(10, list.First.Value);

            Assert.AreEqual(1, list.Last.Value);

            int j = 10;

            foreach (int i in list.AsFromType<System.Collections.Generic.IEnumerable<int>>())

                Assert.AreEqual(j--, i, "Error during enumeration.");

            Assert.AreEqual(0, j, "Enumeration failed.");

            list.AsFromType<IClearable>().Clear();

            AssertCleared(list, queue);



            for (int i = 1; i < 11; i++)

                AddNode(() => list.AddLast(i), i, queue);

            Assert.AreEqual(10u, list.AsFromType<IUIntCountable>().Count, "AddLast assertion failed: Count should be 10.");

            Assert.AreEqual(1, list.First.Value);

            Assert.AreEqual(10, list.Last.Value);

            j = 0;

            foreach (int i in list.AsFromType<System.Collections.Generic.IEnumerable<int>>())

                Assert.AreEqual(++j, i, "Error during enumeration.");

            Assert.AreEqual(10, j, "Enumeration failed.");

            list.AsFromType<IClearable>().Clear();

            AssertCleared(list, queue);
        }

        [TestMethod]
        public void TestEnumeration()
        {
#if CS8
            static
#endif
                void assertValues(System.Collections.Generic.IEnumerable<int> enumerable, int init, Func<int, int> newValue)
            {
                int i = init;

                foreach (int value in enumerable)

                    Assert.AreEqual((i = newValue(i)), value);
            }

            for (int i = 1; i < 11; i++)

                AddNode(() => LinkedList.AddLast(i), i, NodeQueue);

            assertValues(LinkedList, 0, i => i + 1);

            assertValues(new Enumerable<int>(() => LinkedList.GetEnumerator()), 0, i => i + 1);

            assertValues(new Enumerable<int>(() => LinkedList.GetEnumerator(EnumerationDirection.FIFO)), 0, i => i + 1);

            assertValues(new Enumerable<int>(() => LinkedList.GetReversedEnumerator()), 11, i => i - 1);

            assertValues(new Enumerable<int>(() => LinkedList.GetEnumerator(EnumerationDirection.LIFO)), 11, i => i - 1);

            assertValues(new Enumerable<ILinkedListNode<int>>(() => LinkedList.GetNodeEnumerator(EnumerationDirection.FIFO)).Select(node => node.Value), 0, i => i + 1);

            assertValues(new Enumerable<ILinkedListNode<int>>(() => LinkedList.GetNodeEnumerator(EnumerationDirection.LIFO)).Select(node => node.Value), 11, i => i - 1);

            LinkedList.Clear();

            AssertCleared(LinkedList, NodeQueue);
        }

        [TestMethod]
        public void TestRemoveFirst()
        {
            for (int i = 1; i < 11; i++)

                AddNode(() => LinkedList.AddLast(i), i, NodeQueue);

            ILinkedListNode<int> node = LinkedList.First;

            void removeAndAssert()
            {
                node = node.AsLinkedListNode().Next;

                _ = LinkedList.RemoveFirst();

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

            AssertCleared(LinkedList, NodeQueue);
        }

        [TestMethod]
        public void TestRemoveLast()
        {
            for (int i = 1; i < 11; i++)

                AddNode(() => LinkedList.AddLast(i), i, NodeQueue);

            ILinkedListNode<int> node = LinkedList.Last;

            void removeAndAssert()
            {
                node = node.AsLinkedListNode().Previous;

                _ = LinkedList.RemoveLast();

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

            AssertCleared(LinkedList, NodeQueue);
        }

        public void Fill()
        {
            int i;

            for (i = 0; i < 10; i++)

                _ = LinkedList.AddLast(i);

            Assert.AreEqual(10u, LinkedList.Count);
        }

        [TestMethod]
        public void TestMoveAfter()
        {
            Fill();

            Assert.IsFalse(LinkedList.MoveAfter(LinkedList.First.Next, LinkedList.First));

            Assert.IsTrue(LinkedList.MoveAfter(LinkedList.First, LinkedList.First.Next));

            LinkedList<int>.LinkedListNode node = LinkedList.First;

            const int value = 2;

            int i;

            for (i = 0; i < 2; i++)

                node = node.Next;

            Assert.AreEqual(value, node.Value);

            LinkedList<int>.LinkedListNode _node = LinkedList.Last;

            const int _value = 7;

            for (i = 0; i < 2; i++)

                _node = _node.Previous;

            Assert.AreEqual(_value, _node.Value);

            Assert.IsTrue(LinkedList.MoveAfter(node, _node));

            int[] expected = { 1, 0, 3, 4, 5, 6, 7, 2, 8, 9 };

            i = 0;

            foreach (int actual in Enumerable)

                Assert.AreEqual(expected[i++], actual);

            LinkedList.Clear();

            AssertCleared(LinkedList, NodeQueue, false);
        }

        [TestMethod]
        public void TestMoveBefore()
        {
            Fill();

            Assert.IsFalse(LinkedList.MoveBefore(LinkedList.First, LinkedList.First.Next));

            Assert.IsTrue(LinkedList.MoveBefore(LinkedList.First.Next, LinkedList.First));

            LinkedList<int>.LinkedListNode node = LinkedList.First;

            const int value = 2;

            int i;

            for (i = 0; i < 2; i++)

                node = node.Next;

            Assert.AreEqual(value, node.Value);

            LinkedList<int>.LinkedListNode _node = LinkedList.Last;

            const int _value = 7;

            for (i = 0; i < 2; i++)

                _node = _node.Previous;

            Assert.AreEqual(_value, _node.Value);

            Assert.IsTrue(LinkedList.MoveBefore(node, _node));

            int[] expected = { 1, 0, 3, 4, 5, 6, 2, 7, 8, 9 };

            i = 0;

            foreach (int actual in Enumerable)

                Assert.AreEqual(expected[i++], actual);

            LinkedList.Clear();

            AssertCleared(LinkedList, NodeQueue, false);
        }

        [TestMethod]
        public void TestSwap()
        {
            void assertSwapResult(in int[] array, in LinkedList<int>.LinkedListNode start/*, bool ignoreLastIndex = false*/)
            {
                Assert.AreEqual(3, array.Length);

                LinkedList<int>.LinkedListNode node = start;

                Assert.AreEqual(array[0], node.Value);

                int _i;

                for (_i = 1; _i < 3 /*(ignoreLastIndex ? 2 : 3)*/; _i++)

                    Assert.AreEqual(array[_i], (node = node.Next).Value);

                for (_i = 1; _i >= 0; _i--)

                    Assert.AreEqual(array[_i], (node = node.Previous).Value);
            }

            void assertSwap(int[] expected)
            {
                Assert.AreEqual(10, expected.Length);

                int _i = 0;

                foreach (int actual in Enumerable)

                    Assert.AreEqual(expected[_i++], actual, $"i: {_i - 1}");
            }

            Fill();



            LinkedList.Swap(LinkedList.First, LinkedList.First.Next);

            assertSwapResult(new int[] { 1, 0, 2 }, LinkedList.First);

            assertSwapResult(new int[] { 0, 2, 3 }, LinkedList.First.Next);

            assertSwap(new int[] { 1, 0, 2, 3, 4, 5, 6, 7, 8, 9 });

            LinkedList.Swap(LinkedList.First.Next.Next, LinkedList.First.Next);

            assertSwapResult(new int[] { 1, 2, 0 }, LinkedList.First);

            assertSwapResult(new int[] { 2, 0, 3 }, LinkedList.First.Next);

            assertSwap(new int[] { 1, 2, 0, 3, 4, 5, 6, 7, 8, 9 });

            LinkedList.Swap(LinkedList.First.Next, LinkedList.First);

            assertSwapResult(new int[] { 2, 1, 0 }, LinkedList.First);

            assertSwapResult(new int[] { 1, 0, 3 }, LinkedList.First.Next);

            assertSwap(new int[] { 2, 1, 0, 3, 4, 5, 6, 7, 8, 9 });



            LinkedList.Swap(LinkedList.Last, LinkedList.Last.Previous); // 2, 1, 0, 3, 4, 5, 6, 7, 9, 8

            assertSwapResult(new int[] { 7, 9, 8 }, LinkedList.Last.Previous.Previous);

            LinkedList.Swap(LinkedList.Last.Previous.Previous, LinkedList.Last.Previous); // 2, 1, 0, 3, 4, 5, 6, 9, 7, 8

            assertSwapResult(new int[] { 9, 7, 8 }, LinkedList.Last.Previous.Previous); // We compare LinkedList.Last.Previous.Previous, LinkedList.Last.Previous, LinkedList.Last, LinkedList.Last.Previous, LinkedList.Last.Previous.Previous.

            LinkedList.Swap(LinkedList.Last.Previous, LinkedList.Last); // 2, 1, 0, 3, 4, 5, 6, 9, 8, 7

            assertSwapResult(new int[] { 9, 8, 7 }, LinkedList.Last.Previous.Previous);

            assertSwapResult(new int[] { 6, 9, 8 }, LinkedList.Last.Previous.Previous.Previous);

            LinkedList.Swap(LinkedList.First, LinkedList.Last.Previous); // 8, 1, 0, 3, 4, 5, 6, 9, 2, 7

            assertSwapResult(new int[] { 8, 1, 0 }, LinkedList.First);

            assertSwapResult(new int[] { 9, 2, 7 }, LinkedList.Last.Previous.Previous);

            LinkedList.Swap(LinkedList.Last, LinkedList.First.Next); // 8, 7, 0, 3, 4, 5, 6, 9, 2, 1

            assertSwapResult(new int[] { 8, 7, 0 }, LinkedList.First);

            assertSwapResult(new int[] { 9, 2, 1 }, LinkedList.Last.Previous.Previous);

            LinkedList.Swap(LinkedList.First, LinkedList.Last); // 1, 7, 0, 3, 4, 5, 6, 9, 2, 8

            int[] _expected = { 1, 7, 0, 3, 4, 5, 6, 9, 2, 8 };

            assertSwap(_expected);

            LinkedList.Swap(LinkedList.Last, LinkedList.First); // 2, 1, 0, 3, 4, 5, 6, 7, 9, 8

            _expected.Swap(9, 0);

            LinkedList.Swap(LinkedList.First.Next.Next, LinkedList.Last.Previous.Previous); // 2, 1, 7, 3, 4, 5, 6, 0, 9, 8

            _expected.Swap(2, 7);

            LinkedList.Swap(LinkedList.First.Next.Next.Next, LinkedList.Last.Previous.Previous.Previous); // 2, 1, 7, 6, 4, 5, 3, 0, 9, 8

            _expected.Swap(3, 6);

            LinkedList.Swap(LinkedList.First.Next.Next.Next.Next, LinkedList.Last.Previous.Previous.Previous.Previous); // 2, 1, 7, 6, 5, 4, 3, 0, 9, 8

            _expected.Swap(4, 5);

            assertSwap(_expected);

            LinkedList.Swap(LinkedList.Last.Previous.Previous, LinkedList.First.Next.Next); // 2, 1, 0, 3, 4, 5, 6, 7, 9, 8

            _expected.Swap(7, 2);

            LinkedList.Swap(LinkedList.Last.Previous.Previous.Previous, LinkedList.First.Next.Next.Next); // 2, 1, 0, 6, 4, 5, 3, 7, 9, 8

            _expected.Swap(6, 3);

            LinkedList.Swap(LinkedList.First.Next.Next.Next.Next, LinkedList.Last.Previous.Previous.Previous.Previous); // 2, 1, 0, 6, 4, 5, 3, 7, 9, 8

            _expected.Swap(5, 4);

            assertSwap(_expected);

            LinkedList.Clear();

            AssertCleared(LinkedList, NodeQueue, false);
        }

        [TestMethod]
        public void TestSort()
        {
            _ = LinkedList.AddLast(4);
            _ = LinkedList.AddLast(8);
            _ = LinkedList.AddLast(9);
            _ = LinkedList.AddLast(0);
            _ = LinkedList.AddLast(6);
            _ = LinkedList.AddLast(7);
            _ = LinkedList.AddLast(3);
            _ = LinkedList.AddLast(1);
            _ = LinkedList.AddLast(2);
            _ = LinkedList.AddLast(5);

            Assert.AreEqual(10u, LinkedList.Count);

            int i;

            for (i = 0; i < 10; i++)

                Assert.IsTrue(LinkedList.Contains(i), $"i: {i}");

            LinkedList.Sort();

            i = 0;

            foreach (int value in Enumerable)

                Assert.AreEqual(i++, value);

            Assert.AreEqual(10u, (uint)i);

            LinkedList.Clear();

            AssertCleared(LinkedList, NodeQueue, false);
        }
    }
}
