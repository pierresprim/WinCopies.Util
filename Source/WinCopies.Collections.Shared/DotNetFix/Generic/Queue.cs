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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using static WinCopies
#if WinCopies2
    .Util.Util;

using System.Runtime.Serialization;

using WinCopies.Util;
using static WinCopies.Util.ThrowHelper;
#else
    .ThrowHelper;
#endif

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface IQueue<T> : ISimpleLinkedList<T>
    {
        void Enqueue(T item);

        T Dequeue();

#if !WinCopies2
        bool TryDequeue(out T result);
#endif
    }

    public class Queue<T> : SimpleLinkedList<T>, IQueue<T>
    {
        private SimpleLinkedListNode<T> _lastItem;

#if WinCopies2
        public new uint Count => base.Count;

        public new T Peek() => base.Peek();
#endif

        public void Enqueue(T item) => Add(item);

        protected sealed override SimpleLinkedListNode<T> AddItem(T item, out bool actionAfter)
        {
            if (FirstItem == null)
            {
                actionAfter = true;

                return new SimpleLinkedListNode<T>(item);
            }

            else
            {
                actionAfter = false;

                var newNode = new SimpleLinkedListNode<T>(item);

                _lastItem.Next = newNode;

                _lastItem = newNode;

                return FirstItem;
            }
        }

        protected sealed override void OnItemAdded() => _lastItem = FirstItem;

        public T Dequeue() => Remove();

        public bool TryDequeue(out T result) => TryRemove(out result);

        protected sealed override SimpleLinkedListNode<T> RemoveItem() => FirstItem.Next;
    }

    public class ReadOnlyQueue<T> : ReadOnlySimpleLinkedList<T>, IQueue<T>
    {
        private readonly IQueue<T> _queue;

        public sealed override uint Count => _queue.Count;

        public ReadOnlyQueue(IQueue<T> queue) => _queue = queue;

        public sealed override T Peek() => _queue.Peek();

        void IQueue<T>.Enqueue(T item) => GetReadOnlyListOrCollectionException();

        T IQueue<T>.Dequeue() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
        public sealed override bool TryPeek(out T result) => _queue.TryPeek(out result);

        bool IQueue<T>.TryDequeue(out T result)
        {
            result = default;

            return false;
        }
#endif
    }

    public interface IEnumerableQueue<T> : IQueue<T>, IEnumerableSimpleLinkedList<T>
    {
        // Left empty.
    }

    [Serializable]
    public class EnumerableQueue<T> : EnumerableSimpleLinkedList<T>, IEnumerableQueue<T>
    {
        [NonSerialized]
        private readonly Queue<T> _queue;

        public sealed override uint Count => _queue.Count;

        public EnumerableQueue() => _queue = new Queue<T>();

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);

            UpdateEnumerableVersion();
        }

        public sealed override T Peek() => _queue.Peek();

        public sealed override bool TryPeek(out T result) => _queue.TryPeek(out result);

        public T Dequeue()
        {
            T result = _queue.Dequeue();

            UpdateEnumerableVersion();

            return result;
        }

        public bool TryDequeue(out T result)
        {
            if (_queue.TryDequeue(out result))
            {
                UpdateEnumerableVersion();

                return true;
            }

            return false;
        }

        public sealed override System.Collections.Generic.IEnumerator<T> GetEnumerator()
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

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if WinCopies2
        [Serializable]
#endif
        public sealed class Enumerator :
#if WinCopies2
            System.Collections.Generic.IEnumerator<T>, WinCopies.Util.DotNetFix.IDisposable
#else
WinCopies.Collections.Generic.Enumerator<T>
#endif
        {
            private EnumerableQueue<T> _queue;
            private ISimpleLinkedListNode<T> _currentNode;
            private readonly uint _version;

#if WinCopies2
            private T _current;

            public T Current => IsDisposed ? throw GetExceptionForDispose(false) : _current;

            object System.Collections.IEnumerator.Current => Current;

            public bool IsDisposed { get; private set; }
#else
            private bool _first = true;

            public override bool? IsResetSupported => true;

            protected override T CurrentOverride => _currentNode.Value;
#endif

            public Enumerator(in EnumerableQueue<T> queue)
            {
                _queue = queue;

                _version = queue.EnumerableVersion;

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

                ThrowIfVersionHasChanged(_queue.EnumerableVersion, _version);

                _first = true;
#endif

                _currentNode = _queue._queue.FirstItem;
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
                ThrowIfVersionHasChanged(_queue.EnumerableVersion, _version);

#if WinCopies2
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

#if WinCopies2
            private void Dispose(bool disposing)
            {
                if (IsDisposed)

                    return;

                _queue.DecrementEnumeratorCount();

                if (disposing)
                {
                    _current = default;

                    _queue = null;

                    _currentNode = null;
                }

                IsDisposed = true;
            }

            public void Dispose() => Dispose(true);
#else
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                _queue.DecrementEnumeratorCount();
            }

            protected override void DisposeManaged()
            {
                base.DisposeManaged();

                _queue = null;

                _currentNode = null;
            }
#endif

            ~Enumerator() => Dispose(false);
        }
    }
}

    namespace WinCopies.Collections.DotNetFix
    {
#if !WinCopies2
        namespace Generic
        {
#endif

            [Serializable]
            public class QueueCollection<T> : System.Collections.Generic.IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
            {
                protected internal
#if WinCopies2
            System.Collections.Generic.Queue<T>
#else
IEnumerableQueue<T>
#endif
            InnerQueue
                { get; }

                /// <summary>
                /// Gets the number of elements contained in the <see cref="QueueCollection{T}"/>.
                /// </summary>
                /// <value>The number of elements contained in the <see cref="QueueCollection{T}"/>.</value>
                public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerQueue.Count;

#if !WinCopies2
                int ICollection.Count => (int)Count;

                int IReadOnlyCollection<T>.Count => (int)Count;
#endif

                public bool IsReadOnly => false;

                bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

                object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

                /// <summary>
                /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class.
                /// </summary>
                public QueueCollection() : this(new
#if WinCopies2
            System.Collections.Generic.Queue
#else
            EnumerableQueue
#endif
            <T>())
                { }

                /// <summary>
                /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom queue.
                /// </summary>
                /// <param name="queue">The inner queue for this <see cref="QueueCollection{T}"/>.</param>
                public QueueCollection(in
#if WinCopies2
            System.Collections.Generic.Queue<T>
#else
            IEnumerableQueue<T>
#endif
             queue) => InnerQueue = queue;

                /// <summary>
                /// Removes all objects from the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
                /// </summary>
                protected virtual void ClearItems() => InnerQueue.Clear();

                /// <summary>
                /// Removes all objects from the <see cref="QueueCollection{T}"/>. Override the <see cref="ClearItems"/> method to provide a custom implementation.
                /// </summary>
                public void Clear() => ClearItems();

                /// <summary>
                /// Determines whether an element is in the <see cref="QueueCollection{T}"/>.
                /// </summary>
                /// <param name="item">The object to locate in the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
                /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="System.Collections.Generic.Queue{T}"/>; otherwise, <see langword="false"/>.</returns>
                public bool Contains(T item) => InnerQueue.Contains(item);

                void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueue).CopyTo(array, index);

                /// <summary>
                /// Copies the <see cref="QueueCollection{T}"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
                /// </summary>
                /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection{T}"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
                /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
                /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
                /// <exception cref="ArgumentException">The number of elements in the source <see cref="QueueCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
                public void CopyTo(in T[] array, in int arrayIndex) => InnerQueue.CopyTo(array, arrayIndex);

                /// <summary>
                /// Removes and returns the object at the beginning of the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
                /// </summary>
                /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection{T}"/>.</returns>
                /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
                protected virtual T DequeueItem() => InnerQueue.Dequeue();

                /// <summary>
                /// Removes and returns the object at the beginning of the <see cref="QueueCollection{T}"/>. Override the <see cref="DequeueItem"/> to provide a custom implementation.
                /// </summary>
                /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection{T}"/>.</returns>
                /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
                /// <seealso cref="TryDequeue(out T)"/>
                public T Dequeue() => DequeueItem();

                /// <summary>
                /// Adds an object to the end of the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
                /// </summary>
                /// <param name="item">The object to add to the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
                protected virtual void EnqueueItem(T item) => InnerQueue.Enqueue(item);

                /// <summary>
                /// Adds an object to the end of the <see cref="QueueCollection{T}"/>. Override the <see cref="EnqueueItem(T)"/> method to provide a custom implementation.
                /// </summary>
                /// <param name="item">The object to add to the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
                public void Enqueue(in T item) => EnqueueItem(item);

                /// <summary>
                /// Returns the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
                /// </summary>
                /// <returns>The object at the beginning of the <see cref="QueueCollection{T}"/>.</returns>
                /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
                /// <seealso cref="TryPeek(out T)"/>
                public T Peek() => InnerQueue.Peek();

                /// <summary>
                /// Copies the <see cref="QueueCollection{T}"/> elements to a new array.
                /// </summary>
                /// <returns>A new array containing elements copied from the <see cref="QueueCollection{T}"/>.</returns>
                public T[] ToArray() => InnerQueue.ToArray();

#if WinCopies2
        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see cref="QueueCollection{T}"/>, if that number is less than 90 percent of current capacity.
        /// </summary>
        public void TrimExcess() => InnerQueue.TrimExcess();
#endif

#if NETCORE

                /// <summary>
                /// Tries to remove and return the object at the beginning of the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
                /// </summary>
                /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
                /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been removed and retrieved.</returns>
                protected virtual bool TryDequeueItem([MaybeNullWhen(false)] out T result) => InnerQueue.TryDequeue(out result);

                /// <summary>
                /// Tries to remove and return the object at the beginning of the <see cref="QueueCollection{T}"/>. Override the <see cref="TryDequeueItem(out T)"/> method to provide a custom implementation.
                /// </summary>
                /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
                /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been removed and retrieved.</returns>
                /// <seealso cref="Dequeue"/>
                public bool TryDequeue([MaybeNullWhen(false)] out T result) => TryDequeueItem(out result);

                /// <summary>
                /// Tries to peek the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
                /// </summary>
                /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
                /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been retrieved.</returns>
                /// <seealso cref="Peek"/>
                public bool TryPeek([MaybeNullWhen(false)] out T result) => InnerQueue.TryPeek(out result);

#endif

                /// <summary>
                /// Returns an enumerator that iterates through the <see cref="QueueCollection{T}"/>.
                /// </summary>
                /// <returns>An <see cref="System.Collections.IEnumerator{T}"/> for the <see cref="QueueCollection{T}"/>.</returns>
                public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerQueue.GetEnumerator();

                System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueue).GetEnumerator();
            }

            [Serializable]
            public class ObservableQueueCollection<T> : QueueCollection<T>, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
            {
                public event PropertyChangedEventHandler PropertyChanged;

                public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

                public ObservableQueueCollection() : base() { }

                public ObservableQueueCollection(in
#if WinCopies2
            System.Collections.Generic.Queue
#else
            IEnumerableQueue
#endif
            <T> queue) : base(queue) { }

                protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

                protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

                protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

                protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

                protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, T item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(action, item));

                protected override void ClearItems()
                {
                    base.ClearItems();

                    RaiseCountPropertyChangedEvent();

                    OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Reset, default));
                }

                protected override T DequeueItem()
                {
                    T result = base.DequeueItem();

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);

                    return result;
                }

                protected override void EnqueueItem(T item)
                {
                    base.EnqueueItem(item);

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, item);
                }

#if NETCORE

                protected override bool TryDequeueItem([MaybeNullWhen(false)] out T result)
                {
                    bool succeeded = base.TryDequeueItem(out result);

                    if (succeeded)
                    {
                        RaiseCountPropertyChangedEvent();

                        RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);
                    }

                    return succeeded;
                }

#endif
            }

            [Serializable]
            public class ReadOnlyQueueCollection<T> : System.Collections.Generic.IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
            {
                protected
#if WinCopies2
            System.Collections.Generic.Queue
#else
IEnumerableQueue
#endif
            <T> InnerQueue
                { get; }

                /// <summary>
                /// Gets the number of elements contained in the <see cref="QueueCollection{T}"/>.
                /// </summary>
                /// <value>The number of elements contained in the <see cref="QueueCollection{T}"/>.</value>
                public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerQueue.Count;

#if !WinCopies2
                int ICollection.Count => (int)Count;
                int IReadOnlyCollection<T>.Count => (int)Count;
#endif

                public bool IsReadOnly => true;

                bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

                object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

                /// <summary>
                /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom <see cref="System.Collections.Generic.Queue{T}"/>.
                /// </summary>
                /// <param name="queue">The inner <see cref="System.Collections.Generic.Queue{T}"/> for this <see cref="QueueCollection{T}"/>.</param>
                public ReadOnlyQueueCollection(in
#if WinCopies2
                System.Collections.Generic.Queue<T>
#else
                IEnumerableQueue<T>
#endif
                queue) => InnerQueue = queue;

                public ReadOnlyQueueCollection(in QueueCollection<T> queueCollection) : this(queueCollection.InnerQueue) { }

                /// <summary>
                /// Determines whether an element is in the <see cref="QueueCollection{T}"/>.
                /// </summary>
                /// <param name="item">The object to locate in the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
                /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="System.Collections.Generic.Queue{T}"/>; otherwise, <see langword="false"/>.</returns>
                public bool Contains(T item) => InnerQueue.Contains(item);

                void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueue).CopyTo(array, index);

                /// <summary>
                /// Copies the <see cref="QueueCollection{T}"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
                /// </summary>
                /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection{T}"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
                /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
                /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
                /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
                /// <exception cref="ArgumentException">The number of elements in the source <see cref="QueueCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
                public void CopyTo(in T[] array, in int arrayIndex) => InnerQueue.CopyTo(array, arrayIndex);

                /// <summary>
                /// Returns the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
                /// </summary>
                /// <returns>The object at the beginning of the <see cref="QueueCollection{T}"/>.</returns>
                /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
                /// <seealso cref="TryPeek(out T)"/>
                public T Peek() => InnerQueue.Peek();

#if NETCORE

                /// <summary>
                /// Tries to peek the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
                /// </summary>
                /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
                /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been retrieved.</returns>
                /// <seealso cref="Peek"/>
                public bool TryPeek([MaybeNullWhen(false)] out T result) => InnerQueue.TryPeek(out result);

#endif

                /// <summary>
                /// Returns an enumerator that iterates through the <see cref="QueueCollection{T}"/>.
                /// </summary>
                /// <returns>An <see cref="System.Collections.IEnumerator{T}"/> for the <see cref="QueueCollection{T}"/>.</returns>
                public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerQueue.GetEnumerator();

                System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueue).GetEnumerator();
            }

            [Serializable]
            public class ReadOnlyObservableQueueCollection<T> : System.Collections.Generic.IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
            {
                protected ObservableQueueCollection<T> InnerQueueCollection { get; }

                public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerQueueCollection.Count;

#if !WinCopies2
                int ICollection.Count => (int)Count;

                int IReadOnlyCollection<T>.Count => (int)Count;
#endif

                public bool IsReadOnly => true;

                bool ICollection.IsSynchronized => ((ICollection)InnerQueueCollection).IsSynchronized;

                object ICollection.SyncRoot => ((ICollection)InnerQueueCollection).SyncRoot;

                public event PropertyChangedEventHandler PropertyChanged;

                public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

                public ReadOnlyObservableQueueCollection(in ObservableQueueCollection<T> queueCollection)
                {
                    InnerQueueCollection = queueCollection ?? throw GetArgumentNullException(nameof(queueCollection));

                    InnerQueueCollection.CollectionChanged += (object sender, SimpleLinkedCollectionChangedEventArgs<T> e) => OnCollectionChanged(e);

                    InnerQueueCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
                }

                protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

                protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

                protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

                protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, T item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(action, item));

                public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerQueueCollection.GetEnumerator();

                System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueueCollection).GetEnumerator();

                public void CopyTo(T[] array, int arrayIndex) => InnerQueueCollection.CopyTo(array, arrayIndex);

                void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueueCollection).CopyTo(array, index);
            }
#if !WinCopies2
        }
#endif
    }
