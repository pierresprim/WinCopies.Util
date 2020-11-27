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
    public class QueueTests
    {
        public WinCopies.Collections.DotNetFix.Generic.Queue<int> Queue { get; } = new Queue<int>();

        [TestMethod]
        public void TestEnqueue()
        {
            for (int i = 1; i < 11; i++)

                Queue.Enqueue(i);

            Assert.AreEqual(10u, Queue.Count, "Enqueue assertion failed: Count should be 10.");

            int j = 0;

            while (Queue.Count != 0)

                Assert.AreEqual(++j, Queue.Dequeue(), "Error during enumeration.");

            Assert.AreEqual(10, j, "Enumeration failed.");

            Assert.AreEqual(0u, Queue.Count, "Queue should be empty.");
        }

        [TestMethod]
        public void TestClear()
        {
            for (int i = 1; i < 11; i++)

                Queue.Enqueue(i);

            Assert.AreEqual(10u, Queue.Count, "Enqueue assertion failed: Count should be 10.");

            Queue.Clear();

            Assert.AreEqual(0u, Queue.Count);
        }
    }

    [TestClass]
    public class EnumerableQueueTests
    {
        public WinCopies.Collections.DotNetFix.Generic.EnumerableQueue<int> Queue { get; } = new EnumerableQueue<int>();

        [TestMethod]
        public void TestEnqueue()
        {
            for (int i = 1; i < 11; i++)

                Queue.Enqueue(i);

            Assert.AreEqual(10u, Queue.Count, "Enqueue assertion failed: Count should be 10.");

            int j = 0;

            while (Queue.Count != 0)

                Assert.AreEqual(++j, Queue.Dequeue(), "Error during enumeration.");

            Assert.AreEqual(10, j, "Enumeration failed.");

            Assert.AreEqual(0u, Queue.Count, "LinkedList should be empty.");
        }

        [TestMethod]
        public void TestEnumeration()
        {
            int i = 1;

            for (; i < 11; i++)

                Queue.Enqueue(i);

            Assert.AreEqual(10u, Queue.Count, "Enqueue assertion failed: Count should be 10.");

            i = 1;

            foreach (int value in Queue)

                Assert.AreEqual(i++, value);

            Queue.Clear();

            Assert.AreEqual(0u, Queue.Count);
        }
    }
}
