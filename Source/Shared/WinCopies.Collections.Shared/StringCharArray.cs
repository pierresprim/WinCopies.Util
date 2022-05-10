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
    public class StringCharArray : ICountableEnumerableInfo<char>, IReadOnlyList<char>
    {
        private readonly string _s;

        public int Count => _s.Length;

        public char this[int index] => _s[index];

#if !CS8
        object IIndexableR.this[int index] => this[index];
#endif

        public bool SupportsReversedEnumeration => true;

#if !(WinCopies3 && CS7)
        object IReadOnlyList.this[int index] => this[index];
#endif

        public StringCharArray(in string s) => _s = s;

        public ICountableEnumeratorInfo<char> GetEnumerator() => new CountableEnumeratorInfo<char>(new EnumeratorInfo<char>(_s), () => Count);

        System.Collections.Generic.IEnumerator<char> System.Collections.Generic.IEnumerable<char>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumeratorInfo2<char> GetReversedEnumerator() => new ArrayEnumerator<char>(this, true);

        System.Collections.Generic.IEnumerator<char> Generic.IEnumerable<char>.GetReversedEnumerator() => GetReversedEnumerator();

        ICountableEnumerator<char> IReadOnlyList<char>.GetEnumerator() => GetEnumerator();

        private CountableEnumeratorInfo<char> _GetReversedEnumerator() => new
#if !CS9
CountableEnumeratorInfo<char>
#endif
            (GetReversedEnumerator(), () => Count);

        ICountableEnumerator<char> ICountableEnumerable<char, ICountableEnumerator<char>>.GetEnumerator() => _GetReversedEnumerator();

        ICountableEnumeratorInfo<char> Generic.IEnumerable<char, ICountableEnumeratorInfo<char>>.GetReversedEnumerator() => _GetReversedEnumerator();

#if !CS8
        ICountableEnumerator<char> Enumeration.DotNetFix.IEnumerable<ICountableEnumerator<char>>.GetEnumerator() => GetEnumerator();

        ICountableEnumerator<char> DotNetFix.Generic.IEnumerable<char, ICountableEnumerator<char>>.GetEnumerator() => GetEnumerator();

        DotNetFix.ICountableEnumerator Enumeration.DotNetFix.IEnumerable<DotNetFix.ICountableEnumerator>.GetEnumerator() => GetEnumerator();

        IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
    }
}
#endif
