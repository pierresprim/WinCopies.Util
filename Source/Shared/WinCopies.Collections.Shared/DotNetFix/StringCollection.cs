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

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Extensions.Generic;

namespace WinCopies.Collections.DotNetFix
{
    public class StringCollection : IReadOnlyList<string
#if CS8
        ?
#endif
        >
    {
        private readonly System.Collections.Specialized.StringCollection _sc;

        public string
#if CS8
            ?
#endif
            this[int index] => _sc[index];

        public int Count => _sc.Count;

        public StringCollection(in System.Collections.Specialized.StringCollection sc) => _sc = sc ?? throw WinCopies.ThrowHelper.GetArgumentNullException(nameof(sc));

        public ICountableEnumerator<string> GetEnumerator() => new StringEnumerator(_sc, EnumerationDirection.FIFO);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
#if !CS8
        System.Collections.Generic.IEnumerator<string> System.Collections.Generic.IEnumerable<string
#if CS8
            ?
#endif
            >.GetEnumerator() => GetEnumerator();
        ICountableEnumerator Enumeration.IEnumerable<ICountableEnumerator>.GetEnumerator() => GetEnumerator();
#if !CS7
        object IReadOnlyList.this[int index] => _sc[index];
        string IReadOnlyList<string>.this[int index] => _sc[index];
#endif
        object IIndexableR.this[int index] => this[index];
#endif
    }
}
