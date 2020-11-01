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

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections.Tests
{
    [TestClass]
    public class EmptyCheckEnumeratorTests
    {
        [TestMethod]
        public void NonGenericTests()
        {
            int[] ints = System.Array.Empty<int>();

            EmptyCheckEnumerator emptyCheckEnumerator;

            void updateEnumerator() => emptyCheckEnumerator = new EmptyCheckEnumerator(ints.GetEnumerator());

            IEnumerable getEnumerable() => new Enumerable(() => emptyCheckEnumerator);

            updateEnumerator();

            Assert.IsFalse(emptyCheckEnumerator.HasItems);

            uint count = 0;

            foreach (object obj in getEnumerable())

                count++;

            Assert.AreEqual(0u, count);

            ints = new int[10];

            for (int i = 0; i < 10; i++)

                ints[i] = i + 1;

            emptyCheckEnumerator.Dispose();

            emptyCheckEnumerator.Dispose(); // We make sure that EmptyCheckEnumerator does support multiple disposing.

            updateEnumerator();

            Assert.IsTrue(emptyCheckEnumerator.HasItems);

            count = 0;

            bool isInt;

            foreach (object obj in getEnumerable()) // We use a foreach in order to test the EmptyCheckEnumerator class.
            {
                count++;

                isInt = false;

                if (obj is int value)
                {
                    isInt = true;

                    Assert.AreEqual(value, (int)count);
                }

                Assert.IsTrue(isInt);
            }

            emptyCheckEnumerator.Dispose(); // We make sure that EmptyCheckEnumerator does support multiple disposing. We only need to manually call the Dispose method once because this method was already called automatically in the foreach statement.

            Assert.AreEqual(10u, count);
        }

        [TestMethod]
        public void GenericTests()
        {
            int[] ints = System.Array.Empty<int>();

            EmptyCheckEnumerator<int> emptyCheckEnumerator;

            void updateEnumerator() => emptyCheckEnumerator = new EmptyCheckEnumerator<int>(new ArrayEnumerator<int>(ints));

            System.Collections.Generic.IEnumerable<int> getEnumerable() => new Enumerable<int>(() => emptyCheckEnumerator);

            updateEnumerator();

            Assert.IsFalse(emptyCheckEnumerator.HasItems);

            uint count = 0;

            foreach (int i in getEnumerable())

                count++;

            Assert.AreEqual(0u, count);

            ints = new int[10];

            for (int i = 0; i < 10; i++)

                ints[i] = i + 1;

            emptyCheckEnumerator.Dispose();

            emptyCheckEnumerator.Dispose(); // We make sure that EmptyCheckEnumerator does support multiple disposing.

            updateEnumerator();

            Assert.IsTrue(emptyCheckEnumerator.HasItems);

            count = 0;

            foreach (int i in getEnumerable()) // We use a foreach in order to test the EmptyCheckEnumerator class.
            {
                count++;

                Assert.AreEqual(i, (int)count);
            }

            emptyCheckEnumerator.Dispose(); // We make sure that EmptyCheckEnumerator does support multiple disposing. We only need to manually call the Dispose method once because this method was already called automatically in the foreach statement.

            Assert.AreEqual(10u, count);
        }
    }
}
