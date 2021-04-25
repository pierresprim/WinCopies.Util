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

using System;

namespace WinCopies.Collections.Generic
{
    public class CustomEnumeratorEnumerable<TItems, TEnumerator> : System.Collections.Generic.IEnumerable<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
    {
        private readonly TEnumerator _enumerator;

        public CustomEnumeratorEnumerable(TEnumerator enumerator) => _enumerator = enumerator;

        public TEnumerator GetEnumerator() => _enumerator;

        System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => _enumerator;

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _enumerator;
    }

    public class CustomEnumeratorProvider<TItems, TEnumerator> : System.Collections.Generic.IEnumerable<TItems> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
    {
        private readonly Func<TEnumerator> _func;

        public CustomEnumeratorProvider(Func<TEnumerator> func) => _func = func;

        public TEnumerator GetEnumerator() => _func();

        System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => _func();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _func();
    }
}
