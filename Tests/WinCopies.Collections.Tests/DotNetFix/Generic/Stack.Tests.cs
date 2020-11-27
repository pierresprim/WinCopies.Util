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
    public class StackTests
    {
        public WinCopies.Collections.DotNetFix.Generic.Stack<int> Stack { get; } = new Stack<int>();

        [TestMethod]
        public void TestPush()
        {
            for (int i = 1; i < 11; i++)

                Stack.Push(i);

            Assert.AreEqual(10u, Stack.Count, "Push assertion failed: Count should be 10.");

            int j = 10;

            while (Stack.Count != 0)

                Assert.AreEqual(j--, Stack.Pop(), "Error during enumeration.");

            Assert.AreEqual(0, j, "Enumeration failed.");

            Assert.AreEqual(0u, Stack.Count, "Stack should be empty.");
        }

        [TestMethod]
        public void TestClear()
        {
            for (int i = 1; i < 11; i++)

                Stack.Push(i);

            Assert.AreEqual(10u, Stack.Count, "Enqueue assertion failed: Count should be 10.");

            Stack.Clear();

            Assert.AreEqual(0u, Stack.Count);
        }
    }


    [TestClass]
    public class EnumerableStackTests
    {
        public WinCopies.Collections.DotNetFix.Generic.EnumerableStack<int> Stack { get; } = new EnumerableStack<int>();

        [TestMethod]
        public void TestPush()
        {
            for (int i = 1; i < 11; i++)

                Stack.Push(i);

            Assert.AreEqual(10u, Stack.Count, "Enqueue assertion failed: Count should be 10.");

            int j = 10;

            while (Stack.Count != 0)

                Assert.AreEqual(j--, Stack.Pop(), "Error during enumeration.");

            Assert.AreEqual(0, j, "Enumeration failed.");

            Assert.AreEqual(0u, Stack.Count, "LinkedList should be empty.");
        }

        [TestMethod]
        public void TestEnumeration()
        {
            int i = 1;

            for (; i < 11; i++)

                Stack.Push(i);

            Assert.AreEqual(10u, Stack.Count, "Enqueue assertion failed: Count should be 10.");

            i = 10;

            foreach (int value in Stack)

                Assert.AreEqual(i--, value);

            Stack.Clear();

            Assert.AreEqual(0u, Stack.Count);
        }
    }
}
