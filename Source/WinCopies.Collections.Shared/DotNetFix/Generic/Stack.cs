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

using static WinCopies
#if WinCopies2
    .Util.Util;
using static WinCopies.Util.ThrowHelper;
#else
    .ThrowHelper;
#endif

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface IStack<T> : ISimpleLinkedList<T>
    {
        void Push(T item);

        T Pop();

#if !WinCopies2
        bool TryPop(out T result);
#endif
    }

    public class Stack<T> : SimpleLinkedList<T>, IStack<T>
    {
#if WinCopies2
        public new uint Count => base.Count;

        public new T Peek() => base.Peek();
#endif

        public void Push(T item) => Add(item);

        protected sealed override SimpleLinkedListNode<T> AddItem(T item, out bool actionAfter)
        {
            actionAfter = false;

            return new SimpleLinkedListNode<T>(item) { Next = FirstItem };
        }

        protected sealed override void OnItemAdded()
        {
            // Left empty.
        }

        public T Pop() => Remove();

        public bool TryPop(out T result) => TryRemove(out result);

        protected sealed override SimpleLinkedListNode<T> RemoveItem() => FirstItem.Next;
    }

    public class ReadOnlyStack<T> : ReadOnlySimpleLinkedList<T>, IStack<T>
    {
        private readonly IStack<T> _stack;

        public sealed override uint Count => _stack.Count;

        public ReadOnlyStack(IStack<T> stack) => _stack = stack;

        public sealed override T Peek() => _stack.Peek();

        void IStack<T>.Push(T item) => throw GetReadOnlyListOrCollectionException();

        T IStack<T>.Pop() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
        public sealed override bool TryPeek(out T result) => _stack.TryPeek(out result);

        bool IStack<T>.TryPop(out T result)
        {
            result = default;

            return false;
        }
#endif
    }

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

        public sealed override System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            var enumerator = new Enumerator(this);

            IncrementEnumeratorCount();

            return enumerator;
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
            private EnumerableStack<T> _stack;
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

            public Enumerator(in EnumerableStack<T> stack)
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

#endif
                ThrowIfVersionHasChanged(_stack.EnumerableVersion, _version);

#if !WinCopies2
                _first = true;
#endif

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
                base.Dispose(disposing);

                _stack.DecrementEnumeratorCount();
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

namespace WinCopies.Collections.DotNetFix
{
#if !WinCopies2
    namespace Generic
    {
#endif

    [Serializable]
    public class StackCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        protected internal
#if WinCopies2
            System.Collections.Generic.Stack
#else
IEnumerableStack
#endif
            <T> InnerStack
        { get; }

        public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerStack.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

        public bool IsReadOnly => false;

        bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

        public StackCollection() : this(new
#if WinCopies2
            System.Collections.Generic.Stack
#else
            EnumerableStack
#endif
            <T>())
        { }

        public StackCollection(in
#if WinCopies2
            System.Collections.Generic.Stack
#else
            IEnumerableStack
#endif
            <T> stack) => InnerStack = stack;

        protected virtual void ClearItems() => InnerStack.Clear();

        public void Clear() => ClearItems();

        public void Contains(T item) => InnerStack.Contains(item);

        public T Peek() => InnerStack.Peek();

        protected virtual T PopItem() => InnerStack.Pop();

        public T Pop() => PopItem();

        protected virtual void PushItem(T item) => InnerStack.Push(item);

        public void Push(in T item) => PushItem(item);

        public T[] ToArray() => InnerStack.ToArray();

#if WinCopies2
        public void TrimExcess() => InnerStack.TrimExcess();
#endif

#if NETCORE
        public bool TryPeek(out T result) => InnerStack.TryPeek(out result);

        protected virtual bool TryPopItem(out T result) => InnerStack.TryPop(out result);

        public bool TryPop(out T result) => TryPopItem(out result);
#endif

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStack).CopyTo(array, index);

        public void CopyTo(in T[] array, in int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);

        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerStack.GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();
    }

    [Serializable]
    public class ObservableStackCollection<T> : StackCollection<T>, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ObservableStackCollection() : base() { }

        public ObservableStackCollection(in
#if WinCopies2
            System.Collections.Generic.Stack
#else
            IEnumerableStack
#endif
            <T> stack) : base(stack) { }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

        protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, T item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(action, item));

        protected override void ClearItems()
        {
            base.ClearItems();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Reset, default);
        }

        protected override T PopItem()
        {
            T result = base.PopItem();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);

            return result;
        }

        protected override void PushItem(T item)
        {
            base.PushItem(item);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, item);
        }

#if NETCORE

        protected override bool TryPopItem(out T result)
        {
            bool succeeded = base.TryPopItem(out result);

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
    public class ReadOnlyStackCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        protected
#if WinCopies2
            System.Collections.Generic.Stack
#else
IEnumerableStack
#endif
            <T> InnerStack
        { get; }

        public
#if WinCopies2
            int
#else
uint
#endif
            Count => InnerStack.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

        public ReadOnlyStackCollection(in
#if WinCopies2
            System.Collections.Generic.Stack
#else
            IEnumerableStack
#endif
            <T> stack) => InnerStack = stack;

        public ReadOnlyStackCollection(in StackCollection<T> stackCollection) : this(stackCollection.InnerStack) { }

        public void Contains(T item) => InnerStack.Contains(item);

        public T Peek() => InnerStack.Peek();

        public T[] ToArray() => InnerStack.ToArray();

#if NETCORE

        public bool TryPeek(out T result) => InnerStack.TryPeek(out result);

#endif

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStack).CopyTo(array, index);

        public void CopyTo(in T[] array, in int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);

        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerStack.GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();
    }

    [Serializable]
    public class ReadOnlyObservableStackCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
    {
        protected ObservableStackCollection<T> InnerStackCollection { get; }

        public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerStackCollection.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerStackCollection).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerStackCollection).SyncRoot;

        public event PropertyChangedEventHandler PropertyChanged;

        public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ReadOnlyObservableStackCollection(in ObservableStackCollection<T> stackCollection)
        {
            InnerStackCollection = stackCollection ?? throw GetArgumentNullException(nameof(stackCollection));

            InnerStackCollection.CollectionChanged += (object sender, SimpleLinkedCollectionChangedEventArgs<T> e) => OnCollectionChanged(e);

            InnerStackCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

        protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, T item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(action, item));

        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerStackCollection.GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStackCollection).GetEnumerator();

        public void CopyTo(T[] array, int arrayIndex) => InnerStackCollection.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStackCollection).CopyTo(array, index);
    }
#if !WinCopies2
    }
#endif
}
