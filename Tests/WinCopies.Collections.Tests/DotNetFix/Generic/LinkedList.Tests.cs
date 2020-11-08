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

namespace WinCopies.Collections.DotNetFix.Generic.Tests
{
    [TestClass]
    public class LinkedListTests
    {
        public WinCopies.Collections.DotNetFix.Generic.LinkedList<int> LinkedList { get; } = new LinkedList<int>();
        public WinCopies.Collections.DotNetFix.Generic.Queue<ILinkedListNode<int>> NodeQueue { get; } = new Queue<ILinkedListNode<int>>();

        [TestMethod]
        public void TestAddFirstAndLast()
        {
            WinCopies.Collections.DotNetFix.Generic.ILinkedListNode<int> node = null;

            void assertCleared()
            {
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

            for (int i = 1; i < 11; i++)
            {
                node = LinkedList.AddFirst(i);

                Assert.AreEqual(i, node.Value);

                NodeQueue.Enqueue(node);
            }

            Assert.AreEqual(10u, LinkedList.Count, "AddFirst assertion fialed: Count should be 10.");

            Assert.AreEqual(10, LinkedList.First.Value);

            Assert.AreEqual(1, LinkedList.Last.Value);

            int j = 10;

            foreach (int i in LinkedList)

                Assert.AreEqual(j--, i, "Error during enumeration.");

            Assert.AreEqual(0, j, "Enumeration failed.");

            LinkedList.Clear();

            Assert.AreEqual(0u, LinkedList.Count, "LinkedList should be empty.");

            Assert.IsNull(LinkedList.First);

            Assert.IsNull(LinkedList.Last);

            node = null;

            assertCleared();



            node = null;

            for (int i = 1; i < 11; i++)
            {
                node = LinkedList.AddLast(i);

                Assert.AreEqual(i, node.Value);

                NodeQueue.Enqueue(node);
            }

            Assert.AreEqual(10u, LinkedList.Count, "AddLast assertion fialed: Count should be 10.");

            Assert.AreEqual(1, LinkedList.First.Value);

            Assert.AreEqual(10, LinkedList.Last.Value);

            j = 0;

            foreach (int i in LinkedList)

                Assert.AreEqual(++j, i, "Error during enumeration.");

            Assert.AreEqual(10, j, "Enumeration failed.");

            LinkedList.Clear();

            Assert.AreEqual(0u, LinkedList.Count, "LinkedList should be empty.");

            Assert.IsNull(LinkedList.First);

            Assert.IsNull(LinkedList.Last);

            node = null;

            assertCleared();
        }
    }
}
