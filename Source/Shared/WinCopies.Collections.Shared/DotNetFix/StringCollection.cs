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

using WinCopies.Collections.
#if WinCopies3
    Enumeration.Generic;
    using WinCopies.Collections.DotNetFix.Generic;
    using WinCopies.Collections.Extensions.
#endif
    Generic;

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

#if CS7
        int System.Collections.Generic.IReadOnlyCollection<string
#if CS8
            ?
#endif
            >.Count => _sc.Count;

#if WinCopies3
        int IReadOnlyList<string
#if CS8
            ?
#endif
            >.Count => _sc.Count;

        int ICountableEnumerable<string
#if CS8
            ?
#endif
            , Generic.ICountableEnumerator<string
#if CS8
            ?
#endif
            >>.Count => _sc.Count;
#else
        int ICountableEnumerable.Count => _sc.Count;
#endif
#endif

        public StringCollection(in System.Collections.Specialized.StringCollection sc) => _sc = sc ?? throw WinCopies.
#if WinCopies3
            ThrowHelper
#else
            Util.Util
#endif
        .GetArgumentNullException(nameof(sc));

        private System.Collections.Generic.IEnumerator<string> GetEnumerator() => new StringEnumerator(_sc, EnumerationDirection.FIFO);

        System.Collections.Generic.IEnumerator<string> System.Collections.Generic.IEnumerable<string
#if CS8
            ?
#endif
            >.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

#if WinCopies3
        ICountableEnumerator<string> IReadOnlyList<string
#if CS8
            ?
#endif
            >.GetEnumerator() => GetCountableEnumerator();

        ICountableEnumerator<string> ICountableEnumerable<string
#if CS8
            ?
#endif
            , ICountableEnumerator<string
#if CS8
            ?
#endif
            >>.GetEnumerator() => GetCountableEnumerator();

        private ICountableEnumerator<string> GetCountableEnumerator() => new CountableEnumerator<System.Collections.Generic.IEnumerator<string>, string>(GetEnumerator(), () => _sc.Count);
#elif !CS7
        int ICountableEnumerable.Count => _sc.Count;
#endif

#if !CS8
#if !CS7
#if WinCopies3
        int IReadOnlyList<string>.Count => _sc.Count;

        object IReadOnlyList.this[int index] => _sc[index];

        string IReadOnlyList<string>.this[int index] => _sc[index];
#endif
#endif

#if WinCopies3
        object IIndexableR.this[int index] => this[index];

        int ICountable.Count => _sc.Count;

        ICountableEnumerator<string> Enumeration.DotNetFix.IEnumerable<ICountableEnumerator<string>>.GetEnumerator() => GetCountableEnumerator();

        ICountableEnumerator<string> Generic.IEnumerable<string, ICountableEnumerator<string>>.GetEnumerator() => GetCountableEnumerator();

        ICountableEnumerator Enumeration.DotNetFix.IEnumerable<ICountableEnumerator>.GetEnumerator() => GetCountableEnumerator();
#endif
#endif
    }
}
