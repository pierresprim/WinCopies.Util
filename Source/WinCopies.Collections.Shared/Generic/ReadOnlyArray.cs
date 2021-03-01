/* Copyright © Pierre Sprimont, 2021
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

using System.Collections;
using WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections.Generic
{
    public class ReadOnlyArray<T> : ReadOnlyEnumerable<T[], T>, ICountableEnumerableInfo<T>, System.Collections.Generic.IReadOnlyList<T>
    {
        public int Count => InnerEnumerable.Length;

        public bool SupportsReversedEnumeration => true;

        public T this[int index] => InnerEnumerable[index];

        public ReadOnlyArray(T[] array) : base(array) { /* Left empty. */ }

        public ICountableEnumeratorInfo<T> GetEnumerator() => new ArrayEnumerator<T>(InnerEnumerable);

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => InnerEnumerable.GetEnumerator();

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

        ICountableEnumerator<T> ICountableEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumeratorInfo2<T> Collections.DotNetFix.Generic.IEnumerable<T, IEnumeratorInfo2<T>>.GetEnumerator() => GetEnumerator();

        public ICountableEnumeratorInfo<T> GetReversedEnumerator() => new ArrayEnumerator<T>(InnerEnumerable, true);

        IEnumeratorInfo2<T> Collections.Generic.IEnumerable<T, IEnumeratorInfo2<T>>.GetReversedEnumerator() => GetReversedEnumerator();

        System.Collections.Generic.IEnumerator<T> Collections.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();
    }
}
