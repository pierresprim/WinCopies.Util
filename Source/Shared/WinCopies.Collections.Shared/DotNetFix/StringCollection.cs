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
using WinCopies.Collections.Generic;
#if WinCopies3
using WinCopies.Collections.Enumeration.Generic;
#endif

namespace WinCopies.Collections.DotNetFix
{
    public class StringCollection : WinCopies.Collections.Generic.IReadOnlyList<string>
    {
        private readonly System.Collections.Specialized.StringCollection _sc;

#if CS7
        string System.Collections.Generic.IReadOnlyList<string>.this[int index] => _sc[index];

        int System.Collections.Generic.IReadOnlyCollection<string>.Count => _sc.Count;

#if WinCopies3
        int WinCopies.Collections.Generic.IReadOnlyList<string>.Count => _sc.Count;

        int ICountableEnumerable<string, Generic.ICountableEnumerator<string>>.Count => _sc.Count;
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

        private System.Collections.Generic.IEnumerator<string> GetEnumerator() => new Collections.StringEnumerator(_sc, Collections.DotNetFix.EnumerationDirection.FIFO);

        System.Collections.Generic.IEnumerator<string> System.Collections.Generic.IEnumerable<string>.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

#if WinCopies3
        ICountableEnumerator<string> WinCopies.Collections.Generic.IReadOnlyList<string>.GetEnumerator() => GetCountableEnumerator();

        ICountableEnumerator<string> ICountableEnumerable<string, ICountableEnumerator<string>>.GetEnumerator() => GetCountableEnumerator();

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
        int ICountable.Count => _sc.Count;

        ICountableEnumerator<string> Enumeration.DotNetFix.IEnumerable<ICountableEnumerator<string>>.GetEnumerator() => GetCountableEnumerator();

        ICountableEnumerator<string> Generic.IEnumerable<string, ICountableEnumerator<string>>.GetEnumerator() => GetCountableEnumerator();

        ICountableEnumerator Enumeration.DotNetFix.IEnumerable<ICountableEnumerator>.GetEnumerator() => GetCountableEnumerator();
#endif
#endif
    }
}
