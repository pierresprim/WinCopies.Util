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
using WinCopies.Collections;

namespace WinCopies.Util.Tests
{
    [TestClass]
    public class Util
    {
        [TestMethod]
        public void GetIf()
        {
            char[] expectedResult = { 'a', 'b', 'c' };

            char[] _result = new char[3];

            for (int i = -1; i <= 1; i++)

                _result[i + 1] = WinCopies.Util.Extensions.UtilHelpers.GetIf(i, 0, new Comparison((x, y) => ((int)x).CompareTo((int)y)), () => 'a', () => 'b', () => 'c');

            for (int i = 0; i <= 2; i++)

                Assert.AreEqual(expectedResult[i], _result[i]);

            for (int i = -1; i <= 1; i++)

                _result[i + 1] = (char)WinCopies.Util.Extensions.UtilHelpers.GetIf(i, 0, System.Collections.Generic.Comparer<int>.Default, () => 'a', () => 'b', () => 'c');

            for (int i = 0; i <= 2; i++)

                Assert.AreEqual(expectedResult[i], _result[i]);
        }
    }
}
