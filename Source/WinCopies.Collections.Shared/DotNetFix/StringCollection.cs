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

namespace WinCopies.Collections.DotNetFix
{
    public class StringCollection : System.Collections.Generic.IReadOnlyList<string>
    {
        private System.Collections.Specialized.StringCollection _sc;

        string System.Collections.Generic.IReadOnlyList<string>.this[int index] => _sc[index];

        int System.Collections.Generic.IReadOnlyCollection<string>.Count => _sc.Count;

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
    }
}
