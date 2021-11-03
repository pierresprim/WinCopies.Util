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

using static WinCopies
#if !WinCopies3
    .Util.Util;

using static WinCopies.Util.ThrowHelper;
#else
    .ThrowHelper;
#endif


namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface IEnumerableStack<T> : IStack<T>, IEnumerableSimpleLinkedList<T>
    {
        // Left empty.
    }

    [Serializable]
    public class EnumerableStack<T> : EnumerableSimpleLinkedList<T>, IEnumerableStack<T>
    {
        [NonSerialized]
        private readonly Stack<T> _stack;

        public sealed override uint Count => _stack.Count;

        public bool HasItems => _stack.HasItems;

        public EnumerableStack() => _stack = new Stack<T>();

        public sealed override void Clear() => _stack.Clear();

        public void Push(T item)
        {
            _stack.Push(item);

            UpdateEnumerableVersion();
        }

        public sealed override T Peek() => _stack.Peek();

        public sealed override bool TryPeek(out T result) => _stack.TryPeek(out result);

        public T Pop()
        {
            T result = _stack.Pop();

            UpdateEnumerableVersion();

            return result;
        }

        public bool TryPop(out T result) => _stack.TryPop(out result);

        public sealed override
#if WinCopies3
            IUIntCountableEnumerator
#else
            System.Collections.Generic.IEnumerator
#endif
          <T> GetEnumerator()
        {
            var enumerator = new Enumerator(this);

            IncrementEnumeratorCount();

            return enumerator;
        }

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if !WinCopies3
        [Serializable]
#endif
        public sealed class Enumerator :
#if WinCopies3
WinCopies.Collections.Generic.Enumerator<T>, IUIntCountableEnumerator<T>
#else
            System.Collections.Generic.IEnumerator<T>, WinCopies.Util.DotNetFix.IDisposable
#endif
        {
            private EnumerableStack<T> _stack;
            private ISimpleLinkedListNode<T> _currentNode;
            private readonly uint _version;

#if !WinCopies3
            private T _current;

            public T Current => IsDisposed ? throw GetExceptionForDispose(false) : _current;

            object System.Collections.IEnumerator.Current => Current;

            public bool IsDisposed { get; private set; }
#else
            private bool _first = true;

            public override bool? IsResetSupported => true;

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override T CurrentOverride => _currentNode.Value;

            public uint Count => _stack.Count;
#endif

            public Enumerator(in EnumerableStack<T> stack)
            {
                _stack = stack;

                _version = stack.EnumerableVersion;

#if !WinCopies3
                Reset();
#else
                ResetOverride();
#endif
            }

#if !WinCopies3
            public void Reset()
            {
                if (IsDisposed)

                    throw GetExceptionForDispose(false);
#else
            protected override void ResetOverride2()
            {
#endif
                ThrowIfVersionHasChanged(_stack.EnumerableVersion, _version);

#if WinCopies3
                _first = true;
#endif

                _currentNode = _stack._stack.FirstItem;
            }

#if !WinCopies3
            public bool MoveNext()
            {
                if (IsDisposed)

                    throw GetExceptionForDispose(false);
#else
            protected override bool MoveNextOverride()
            {
#endif
                ThrowIfVersionHasChanged(_stack.EnumerableVersion, _version);

#if !WinCopies3
                if (_currentNode == null)

                    return false;

                _current = _currentNode.Value;

                _currentNode = _currentNode.NextNode;

                return true;
#else
                if (_first)
                {
                    _first = false;

                    return _currentNode != null;
                }

                if (_currentNode.Next == null)
                {
                    _currentNode = null;

                    return false;
                }

                _currentNode = _currentNode.Next;

                return true;
#endif
            }

#if !WinCopies3
            private void Dispose(bool disposing)
            {
                if (IsDisposed)

                    return;

                _stack.DecrementEnumeratorCount();

                if (disposing)
                {
                    _current = default;

                    _stack = null;

                    _currentNode = null;
                }

                IsDisposed = true;
            }

            public void Dispose() => Dispose(true);
#else
            protected override void Dispose(bool disposing)
            {
                _stack.DecrementEnumeratorCount();

                base.Dispose(disposing);
            }

            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _stack = null;

                _currentNode = null;
            }
#endif

            ~Enumerator() => Dispose(false);
        }
    }
}
