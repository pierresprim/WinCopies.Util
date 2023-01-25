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
using System;
using System.Collections.Generic;
using System.Text;

using WinCopies.Collections.Generic;

using static WinCopies.ThrowHelper;
#endif
namespace WinCopies.Collections.DotNetFix
{
    public enum EnumerationDirection
    {
        FIFO = 1,
        LIFO = 2
    }
#if CS7
    namespace Generic
    {
        public class LinkedListEnumerator<T> : Enumerator<ILinkedListNode<T>>
        {
            private ILinkedList<T> _list;
            private Action _action;
            private Func<bool> _moveNext;
            private ILinkedListNode<T>
#if CS8
                ?
#endif
                _currentNode;
            private Action _reset;

            protected ILinkedList<T> List => GetOrThrowIfDisposed(_list);

            public EnumerationDirection EnumerationDirection { get; }

            public ILinkedListNode<T> Start { get; }
            public ILinkedListNode<T> End { get; }

            public override bool? IsResetSupported => true;

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override ILinkedListNode<T> CurrentOverride => _currentNode;
            protected DotNetFix.IReadOnlyLinkedListNode<ILinkedListNode<T>> CurrentNode => _currentNode;

            public LinkedListEnumerator(in ILinkedList<T> list, in EnumerationDirection enumerationDirection, in ILinkedListNode<T>
#if CS8
                ?
#endif
                start = default, in ILinkedListNode<T>
#if CS8
                ?
#endif
                end = default)
            {
                _list = list ?? throw GetArgumentNullException(nameof(list));

                // _version = list._enumerableVersion;

                EnumerationDirection = enumerationDirection;

                Start = start;

                End = end;

                switch (enumerationDirection)
                {
                    case EnumerationDirection.FIFO:

                        _action = () => _currentNode = CurrentNode.Next;

                        _reset = () => _currentNode = Start ??

                        _list.First;

                        break;

                    case EnumerationDirection.LIFO:

                        _action = () => _currentNode = CurrentNode.Previous;

                        _reset = () => _currentNode = Start ??

                        _list.Last;

                        break;
                }

                ResetMoveNext();
            }

            protected void ResetMoveNext() => _moveNext = () =>
            {
                if (_list.First == null)

                    return false;

                _reset();

                if (End == null)

                    _moveNext = () =>
                    {
                        _action();

                        return _MoveNext();
                    };

                _moveNext = () =>
                {
                    if (_currentNode == End)

                        return false;

                    _action();

                    return _MoveNext();
                };

                return true;
            };

            private bool _MoveNext()
            {
                if (_currentNode == null)
                {
                    ResetCurrent();

                    return false;
                }

                // The new node has already been updated in the _action delegate.

                return true;
            }

            protected override void ResetOverride2()
            {
                _reset();

                ResetMoveNext();
            }

            protected override bool MoveNextOverride() => _moveNext();

            protected override void ResetCurrent() => _currentNode = null;

            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _action = null;

                ResetCurrent();

                _reset = null;
            }

            protected override void DisposeUnmanaged()
            {
                _list = null;

                base.DisposeUnmanaged();
            }
        }
    }
#endif
}
