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

using System;
using System.Collections;

#if WinCopies2
using static WinCopies.Util.Util;
#endif

using static WinCopies.
#if WinCopies2
    Util.
#endif
    ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{

    public interface IEnumerableStack : IStack, IEnumerableSimpleLinkedList
    {
        // Left empty.
    }

    [Serializable]
    public class EnumerableStack : EnumerableSimpleLinkedList, IEnumerableStack
    {
        [NonSerialized]
        private readonly Stack _stack;

#if WinCopies2
        public new bool IsReadOnly => base.IsReadOnly;
#endif

        public sealed override uint Count => _stack.Count;

        public EnumerableStack() => _stack = new Stack();

        public sealed override void Clear()
        {
            _stack.Clear();

            UpdateEnumerableVersion();
        }

        public void Push(object item)
        {
            _stack.Push(item);

            UpdateEnumerableVersion();
        }

        public sealed override object Peek() => _stack.Peek();

        public sealed override bool TryPeek(out object result) => _stack.TryPeek(out result);

        public object Pop()
        {
            object result = _stack.Pop();

            UpdateEnumerableVersion();

            return result;
        }

        public bool TryPop(out object result)
        {
            if (_stack.TryPop(out result))
            {
                UpdateEnumerableVersion();

                return true;
            }

            return false;
        }

        public sealed override System.Collections.IEnumerator GetEnumerator()
        {
            var enumerator = new Enumerator(this);

            IncrementEnumeratorCount();

            return enumerator;
        }

#if WinCopies2
        [Serializable]
#endif
        public sealed class Enumerator :
#if WinCopies2
IEnumerator, Util.DotNetFix.IDisposable
#else
WinCopies.Collections.Enumerator
#endif
        {
            private EnumerableStack _stack;
            private ISimpleLinkedListNode _currentNode;
            private readonly uint _version;
            private bool _first = true;

#if WinCopies2
            public object Current => IsDisposed ? throw GetExceptionForDispose(false) : _currentNode.Value;

            public bool IsDisposed { get; private set; }
#else
            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override object CurrentOverride => _currentNode.Value;

            public override bool? IsResetSupported => true;
#endif

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> class.
            /// </summary>
            /// <param name="stack">The <see cref="EnumerableStack"/> to enumerate.</param>
            public Enumerator(in EnumerableStack stack)
            {
                _stack = stack;

                _version = stack.EnumerableVersion;

#if WinCopies2
                Reset();
#else
                ResetOverride();
#endif
            }

#if WinCopies2
            public void Reset()
            {
                if (IsDisposed)

                    throw GetExceptionForDispose(false);
#else
            protected override void ResetOverride()
            {
                base.ResetOverride();

                ThrowIfVersionHasChanged(_stack.EnumerableVersion, _version);
#endif

                _first = true;

                _currentNode = _stack._stack.FirstItem;
            }

#if WinCopies2
            public bool MoveNext()
            {
                if (IsDisposed)

                    throw GetExceptionForDispose(false);

#else
            protected override bool MoveNextOverride()
            {
#endif
                ThrowIfVersionHasChanged(_stack.EnumerableVersion, _version);

                if (_first)
                {
                    _first = false;

                    return _currentNode != null;
                }

                if (_currentNode.
#if WinCopies2
                    NextNode
#else
                    Next
#endif
                    == null)
                {
                    _currentNode = null;

                    return false;
                }

                _currentNode = _currentNode.
#if WinCopies2
                    NextNode
#else
                    Next
#endif
                    ;

                return true;
            }

#if WinCopies2
            private void Dispose(bool disposing)
            {
                if (IsDisposed)

                    return;

                _stack.DecrementEnumeratorCount();

                if (disposing)
                {
                    _stack = null;

                    _currentNode = null;
                }

                IsDisposed = true;
            }

            public void Dispose() => Dispose(true);
#else
            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _stack = null;

                _currentNode = null;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                _stack.DecrementEnumeratorCount();
            }
#endif

            ~Enumerator() => Dispose(false);
        }
    }
}
