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

#if CS7

#if !WinCopies3
using System;
#endif
using System.Collections;
using System.Collections.Generic;

namespace WinCopies.Collections.DotNetFix.Generic
{
#if !WinCopies3
    [Serializable]
#endif
    internal sealed class LinkedListNodeEnumerator<T> : System.Collections.Generic.IEnumerator<
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T>>, IEnumerable<
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T>>
    {
        private
#if !WinCopies3
                System.Collections.Generic.LinkedList
#else
                ILinkedList
#endif
                <T> _list;

        public
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> Current
        { get; private set; }

        object System.Collections.IEnumerator.Current => Current;

        public LinkedListNodeEnumerator(
#if !WinCopies3
                System.Collections.Generic.LinkedList
#else
                ILinkedList
#endif
                <T> list) => _list = list; // todo: make inner list version check

        public void Dispose()
        {
            Current = null;

            _list = null;
        }

        private bool _first = true;

        public bool MoveNext()
        {
            if (_list.Count == 0)

                return false;

            if (_first)
            {
                _first = false;

                Current = _list.First;

                return true;
            }

            if (Current.Next == null)
            {
                Current = null;

                return false;
            }

            Current = Current.Next;

            return true;
        }

        public void Reset() { }

        public System.Collections.Generic.IEnumerator<
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T>> GetEnumerator() => this;

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

#endif
