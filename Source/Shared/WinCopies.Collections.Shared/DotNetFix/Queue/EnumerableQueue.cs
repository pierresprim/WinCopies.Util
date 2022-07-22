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

#if !WinCopies3
using static WinCopies.Util.Util;
#endif

using static WinCopies.
#if !WinCopies3
    Util.
#endif
    ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{

    public interface IEnumerableQueue : IQueue, IEnumerableSimpleLinkedList
    {
        // Left empty.
    }

    [Serializable]
    public class EnumerableQueue : EnumerableSimpleLinkedList, IEnumerableQueue
    {
        [NonSerialized]
        private readonly Queue _queue;

#if !WinCopies3
        public new bool IsReadOnly => base.IsReadOnly;
#endif

        public sealed override uint Count => _queue.Count;

        public EnumerableQueue() => _queue = new Queue();

        public void Enqueue(object item)
        {
            _queue.Enqueue(item);

            UpdateEnumerableVersion();
        }

        public sealed override object Peek() => _queue.Peek();

        public sealed override bool TryPeek(out object result) => _queue.TryPeek(out result);

        public bool TryDequeue(out object result)
        {
            if (_queue.TryDequeue(out result))
            {
                UpdateEnumerableVersion();

                return true;
            }

            return false;
        }

        public object Dequeue()
        {
            object result = _queue.Dequeue();

            UpdateEnumerableVersion();

            return result;
        }

        /// <summary>
        /// Returns an <see cref="System.Collections.IEnumerator"/> for this <see cref="EnumerableQueue"/>.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator"/> for this <see cref="EnumerableQueue"/>.</returns>
        public sealed override System.Collections.IEnumerator GetEnumerator()
        {
            var enumerator = new Enumerator(this);

            IncrementEnumeratorCount();

            return enumerator;
        }

        public sealed override void Clear()
        {
            _queue.Clear();

            UpdateEnumerableVersion();
        }

#if !WinCopies3
        [Serializable]
#endif
        public sealed class Enumerator :
#if WinCopies3
            WinCopies.Collections.Enumerator
#else
            IEnumerator, WinCopies.Util.DotNetFix.IDisposable
#endif
        {
            private EnumerableQueue _queue;
            private ISimpleLinkedListNode _currentNode;
            private readonly uint _version;

#if WinCopies3
            private bool _first = true;

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override object CurrentOverride => _currentNode.Value;

            public override bool? IsResetSupported => true;
#else
            public object Current => IsDisposed ? throw GetExceptionForDispose(false) : _currentNode.Value;

            public bool IsDisposed { get; private set; }
#endif

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> class.
            /// </summary>
            /// <param name="queue">The <see cref="EnumerableQueue"/> to enumerate.</param>
            public Enumerator(in EnumerableQueue queue)
            {
                _queue = queue;

                _version = queue.EnumerableVersion;

#if WinCopies3
                ResetOverride();
#else
                Reset();
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
                ThrowIfVersionHasChanged(_queue.EnumerableVersion, _version);

                _first = true;
#endif

                _currentNode = _queue._queue.FirstItem;
            }

#if !WinCopies3
            public bool MoveNext()
            {
                if (IsDisposed)

                    throw GetExceptionForDispose(false);

                if (_queue.EnumerableVersion != _version)
#else
            protected override bool MoveNextOverride()
            {
#endif
                    ThrowIfVersionHasChanged(_queue.EnumerableVersion, _version);

#if !WinCopies3
                if (_currentNode == null)

                    return false;

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

#if WinCopies3
            protected override void DisposeUnmanaged()
            {
                _queue.DecrementEnumeratorCount();
                _queue = null;

                base.DisposeUnmanaged();
            }

            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _currentNode = null;
            }
#else
            private void Dispose(bool disposing)
            {
                if (IsDisposed)

                    return;

                _queue.DecrementEnumeratorCount();

                if (disposing)
                {
                    _queue = null;

                    _currentNode = null;
                }

                IsDisposed = true;
            }

            public void Dispose() => Dispose(true);

            ~Enumerator() => Dispose(false);
#endif
        }
    }
}
