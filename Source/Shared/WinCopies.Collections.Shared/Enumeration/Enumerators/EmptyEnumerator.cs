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

using System.Collections;
using System.Collections.Generic;

namespace WinCopies.Collections
{
    public sealed class EmptyEnumerator : System.Collections.IEnumerator
    {
        private readonly object _current = default;

        object System.Collections.IEnumerator.Current => _current;

        bool System.Collections.IEnumerator.MoveNext() => false;

        void System.Collections.IEnumerator.Reset() { }
    }

#if WinCopies3
namespace Generic
{
#endif
    public sealed class EmptyEnumerator<T> : System.Collections.Generic.IEnumerator<T>
    {
        private readonly T _current = default;

        T System.Collections.Generic.IEnumerator<T>.Current => _current;

        object System.Collections.IEnumerator.Current => _current;

        bool System.Collections.IEnumerator.MoveNext() => false;

        void System.Collections.IEnumerator.Reset() { }

        void System.IDisposable.Dispose() { }
    }
#if WinCopies3
}
#endif
}
