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

#if !WinCopies2

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections;
using System.Collections.Generic;

namespace WinCopies.Collections.Generic.Tests
{
    [TestClass]
    public class ArrayMerger
    {
        private readonly WinCopies.Collections.
#if !WinCopies2
            Generic.
#endif
            ArrayMerger<int> _arrayMerger = new WinCopies.Collections.
#if !WinCopies2
            Generic.
#endif
            ArrayMerger<int>();

        private void ClearAndAssert()
        {
            _arrayMerger.Clear();

            Assert.AreEqual(0u, _arrayMerger.Count);
            Assert.AreEqual(0u, _arrayMerger.RealCount);
        }

        [TestMethod]
        public void TestToArray()
        {
            ClearAndAssert();

            int[][] array = new int[10][];

            int i;
            int j;

            for (i = 0; i < 10; i++)
            {
                array[i] = new int[10];

                for (j = 0; j < 10; j++)

                    array[i][j] = i * 10 + j + 1;
            }

            for (i = 0; i < 10; i++)

                for (j = 0; j < 10; j++)

                    Assert.AreEqual(i * 10 + j + 1, array[i][j]);

            foreach (int[] _array in array)

                _ = _arrayMerger.AddLast(new UIntCountableEnumerableArray<int>(_array));

            Assert.AreEqual(10ul * 10ul, _arrayMerger.RealCount);

            int[] result = _arrayMerger.ToArray();

            i = 0;

            foreach (int _result in result)

                Assert.AreEqual(++i, _result);

            Assert.AreEqual(100, i);

            ClearAndAssert();
        }
    }
}

#endif
