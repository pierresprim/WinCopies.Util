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

#if WinCopies3

using System.Collections;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections
{
    public class StringCharArray :
        ICountableEnumerableInfo<char>, IReadOnlyList<char>
    {
        private readonly string _s;

        public int Count => _s.Length;

        public char this[int index] => _s[index];

        public bool SupportsReversedEnumeration => true;

        object IReadOnlyList.this[int index] => this[index];

        public StringCharArray(in string s) => _s = s;

        public ICountableEnumeratorInfo<char> GetEnumerator() => new CountableEnumeratorInfo<char>(new EnumeratorInfo<char>(_s), () => Count);

        System.Collections.Generic.IEnumerator<char> System.Collections.Generic.IEnumerable<char>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        ICountableEnumerator<char> ICountableEnumerable<char>.GetEnumerator() => GetEnumerator();

        public IEnumeratorInfo2<char> GetReversedEnumerator() => new ArrayEnumerator<char>(this, true);

        IEnumeratorInfo2<char> DotNetFix.Generic.IEnumerable<char, IEnumeratorInfo2<char>>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<char> Generic.IEnumerable<char>.GetReversedEnumerator() => GetReversedEnumerator();
    }
}

#endif
