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

#if CS7 && WinCopies3

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Linq;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
#if !WinCopies3
    namespace Generic
    {
#endif
    public interface ILinkedListNodeEnumerable
    {
        IReadOnlyLinkedListNode Node { get; }

        ILinkedListEnumerable List { get; }

        bool CanMovePreviousFromCurrent { get; }

        bool CanMoveNextFromCurrent { get; }
    }

    public interface ILinkedListEnumerable : System.Collections.IEnumerable
#if WinCopies3
        , IUIntCountable, WinCopies.DotNetFix.IDisposable
#endif
    {
        ILinkedListNodeEnumerable First { get; }

        ILinkedListNodeEnumerable Current { get; }

        ILinkedListNodeEnumerable Last { get; }

        EnumerationDirection EnumerationDirection { get; }

        bool MovePrevious();

        bool MoveNext();

        void UpdateCurrent(IReadOnlyLinkedListNode node);
    }

#if WinCopies3
    namespace Generic
    {
#endif
        public interface ILinkedListNodeEnumerable<T> : ILinkedListNodeEnumerable
        {
            ILinkedListNode<T> Node { get; }

            ILinkedListEnumerable<T> List { get; }

#if CS8
            IReadOnlyLinkedListNode ILinkedListNodeEnumerable.Node => Node;

            ILinkedListEnumerable ILinkedListNodeEnumerable.List => List;
#endif
        }

        public class LinkedListNodeEnumerable<T> : ILinkedListNodeEnumerable<T>
        {
            public ILinkedListNode<T> Node { get; }

            public ILinkedListEnumerable<T> List { get; }

            public bool CanMovePreviousFromCurrent => (List.EnumerationDirection == EnumerationDirection.FIFO ? Node.Previous : Node.Next) != null;

            public bool CanMoveNextFromCurrent => (List.EnumerationDirection == EnumerationDirection.FIFO ? Node.Next : Node.Previous) != null;

            public LinkedListNodeEnumerable(in ILinkedListNode<T> node, in ILinkedListEnumerable<T> list)
            {
                Node = node;

                List = list;
            }

#if !CS8
        IReadOnlyLinkedListNode ILinkedListNodeEnumerable.Node => Node;

        ILinkedListEnumerable ILinkedListNodeEnumerable.List => List;

        bool ILinkedListNodeEnumerable.CanMovePreviousFromCurrent => Node.Previous != null;

        bool ILinkedListNodeEnumerable.CanMoveNextFromCurrent => Node.Next != null;
#endif
        }

        public interface ILinkedListEnumerable<T> : ILinkedListEnumerable, System.Collections.Generic.IEnumerable<T>
        {
            ILinkedListNodeEnumerable<T> First { get; }

            ILinkedListNodeEnumerable<T> Current { get; }

            ILinkedListNodeEnumerable<T> Last { get; }

            ILinkedListNode<T> Add(T value);

            ILinkedListNodeEnumerable<T> GetLinkedListNodeEnumerable(ILinkedListNode<T> node);

            void UpdateCurrent(ILinkedListNode<T> node);

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetEnumeratorToCurrent(bool keepCurrent);

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetEnumeratorFromCurrent(bool keepCurrent);

#if CS8
            ILinkedListNodeEnumerable ILinkedListEnumerable.First => First;

            ILinkedListNodeEnumerable ILinkedListEnumerable.Current => Current;

            ILinkedListNodeEnumerable ILinkedListEnumerable.Last => Last;
#endif
        }

        public interface IObservableLinkedListEnumerable<T> : ILinkedListEnumerable<T>, INotifyLinkedCollectionChanged<T>, INotifyPropertyChanged
        {

        }

        public class LinkedListEnumerable<TItems, TList> : ILinkedListEnumerable<TItems> where TList : ILinkedList<TItems>, INotifyLinkedCollectionChanged<TItems>
        {
            private ILinkedListNodeEnumerable<TItems> _first;
            private ILinkedListNodeEnumerable<TItems> _current;
            private ILinkedListNodeEnumerable<TItems> _last;
            private TList _innerList;

            protected TList InnerList => GetValueIfNotDisposed(_innerList);

            protected T GetValueIfNotDisposed<T>(in T value) => GetOrThrowIfDisposed(this, value);

            public uint Count => InnerList.Count;

            public ILinkedListNodeEnumerable<TItems> First { get => GetValueIfNotDisposed(_first); set => _first = InnerList.Equals(value.Node.List) ? value : throw GetInvalidNodeException(nameof(value)); }

            public ILinkedListNodeEnumerable<TItems> Current
            {
                get
                {
                    ThrowIfDisposed(this);

                    return _current
#if CS8
            ??=
#else
            ?? (_current =
#endif
            GetLinkedListNodeEnumerable(_innerList.First)
#if !CS8
            )
#endif
            ;
                }
            }

            public ILinkedListNodeEnumerable<TItems> Last { get => GetValueIfNotDisposed(_last); set => _last = InnerList.Equals(value.Node.List) ? value : throw GetInvalidNodeException(nameof(value)); }

            public EnumerationDirection EnumerationDirection { get; }

            public bool IsDisposed => InnerList == null;

            public LinkedListEnumerable(in TList list, in ILinkedListNode<TItems> first, in ILinkedListNode<TItems> current, in ILinkedListNode<TItems> last, in EnumerationDirection enumerationDirection)
            {
                _innerList = list == null ? throw GetArgumentNullException(nameof(list)) : list;

                list.CollectionChanged += (object sender, LinkedCollectionChangedEventArgs<TItems> e) => 
            {
                    switch (e.Action)
                    {
                        case LinkedCollectionChangedAction.Remove:

                            if (_first.Node == e.Node)

                                _first = null;

                            if (_last.Node == e.Node)

                                _last = null;

                            break;

                        case LinkedCollectionChangedAction.Reset:

                            _first = null;

                            _last = null;

                            break;
                    }
                };

                _first = GetLinkedListNodeEnumerable(first);

                _last = GetLinkedListNodeEnumerable(last);

                _current = GetLinkedListNodeEnumerable(current ?? first ?? list.First);

                EnumerationDirection = enumerationDirection;
            }

            public LinkedListEnumerable(in TList list, in EnumerationDirection enumerationDirection) : this(list, null, null, null, enumerationDirection) { /* Left empty. */ }

            public virtual ILinkedListNodeEnumerable<TItems> GetLinkedListNodeEnumerable(ILinkedListNode<TItems> node) => new LinkedListNodeEnumerable<TItems>(node, this);

            private bool Move(in ILinkedListNode<TItems> node)
            {
                ThrowIfDisposed(this);

                if (node == null)

                    return false;

                _current = GetLinkedListNodeEnumerable(node);

                return true;
            }

            public bool MovePrevious() => Move(EnumerationDirection == EnumerationDirection.FIFO ? Current?.Node.Previous : Current?.Node.Next);

            public bool MoveNext() => Move(EnumerationDirection == EnumerationDirection.FIFO ? Current?.Node.Next : Current?.Node.Previous);

            public ILinkedListNode<TItems> Add(TItems value) => GetValueIfNotDisposed(EnumerationDirection) == EnumerationDirection.FIFO ? InnerList.AddLast(value) : InnerList.AddFirst(value);

            public void UpdateCurrent(ILinkedListNode<TItems> node) => _current = GetLinkedListNodeEnumerable(GetValueIfNotDisposed(node ?? throw GetArgumentNullException(nameof(node))));

            public static ILinkedListNode<TItems> TryGetNode(in IReadOnlyLinkedListNode node) => node as ILinkedListNode<TItems>;

            public static ArgumentException GetInvalidNodeException(in string argumentName) => throw new ArgumentException($"The given node does not implement the {nameof(ILinkedListNode<TItems>)} interface.", argumentName);

            public static ILinkedListNode<TItems> GetNode(in IReadOnlyLinkedListNode node, in string argumentName) => TryGetNode(node) ?? throw GetInvalidNodeException(argumentName);

            void ILinkedListEnumerable.UpdateCurrent(IReadOnlyLinkedListNode node) => UpdateCurrent(GetNode(node, nameof(node)));

            public System.Collections.Generic.IEnumerator<ILinkedListNode<TItems>> GetEnumeratorToCurrent(bool keepCurrent) => Util.GetNodeEnumerator(InnerList, EnumerationDirection.FIFO, _innerList.First, keepCurrent ? Current.Node : Current.Node.Previous);

            public System.Collections.Generic.IEnumerator<ILinkedListNode<TItems>> GetEnumeratorFromCurrent(bool keepCurrent) => Util.GetNodeEnumerator(InnerList, EnumerationDirection.FIFO, keepCurrent ? Current.Node : Current.Node.Next, _innerList.Last);

            public System.Collections.Generic.IEnumerator<TItems> GetEnumerator() => new Enumerable<ILinkedListNode<TItems>>(() => Util.GetNodeEnumerator(InnerList, EnumerationDirection.FIFO, _innerList.First, _innerList.Last)).Select(node => node.Value).GetEnumerator();

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => InnerList.GetNodeEnumerator().SelectConverter(node => GetLinkedListNodeEnumerable(node));

            protected virtual void Dispose(bool disposing)
            {
                _first = null;
                _last = null;
                _current = null;

                _innerList = default;
            }

            public void Dispose()
            {
                if (IsDisposed)

                    return;

                Dispose(true);

                GC.SuppressFinalize(this);
            }

            ~LinkedListEnumerable() => Dispose(false);

#if !CS8
        ILinkedListNodeEnumerable ILinkedListEnumerable.First => First;

        ILinkedListNodeEnumerable ILinkedListEnumerable.Current => Current;

        ILinkedListNodeEnumerable ILinkedListEnumerable.Last => Last;
#endif
        }

        public class LinkedListEnumerable<T> : LinkedListEnumerable<T, ObservableLinkedCollection<T>>
        {
            public LinkedListEnumerable(in ObservableLinkedCollection<T> list, in ILinkedListNode<T> first, in ILinkedListNode<T> current, in ILinkedListNode<T> last, in EnumerationDirection enumerationDirection) : base(list, first, current, last, enumerationDirection) { /* Left empty. */ }

            public LinkedListEnumerable(in ObservableLinkedCollection<T> list, in EnumerationDirection enumerationDirection) : base(list, null, null, null, enumerationDirection) { /* Left empty. */ }

            public LinkedListEnumerable(in EnumerationDirection enumerationDirection) : this(new ObservableLinkedCollection<T>(), enumerationDirection) { /* Left empty. */ }
        }

        public class ObservableLinkedListEnumerable<TItems, TList> : LinkedListEnumerable<TItems, TList>, IObservableLinkedListEnumerable<TItems> where TList : ILinkedList<TItems>, INotifyLinkedCollectionChanged<TItems>, INotifyPropertyChanged
        {
            event LinkedCollectionChangedEventHandler<TItems> INotifyLinkedCollectionChanged<TItems>.CollectionChanged
            {
                add => ((TList)InnerList).CollectionChanged += value;

                remove => ((TList)InnerList).CollectionChanged -= value;
            }

            event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
            {
                add => ((TList)InnerList).PropertyChanged += value;

                remove => ((TList)InnerList).PropertyChanged -= value;
            }

            public ObservableLinkedListEnumerable(in TList list, in EnumerationDirection enumerationDirection) : base(list, null, null, null, enumerationDirection) { /* Left empty. */ }
        }

        public class LinkedCollectionEnumerable<T> : ILinkedListEnumerable<T>
        {
            private ILinkedListEnumerable<T> _innerList;

            public uint Count => GetValueIfNotDisposed(InnerList.Count);

            public bool IsDisposed => _innerList == null;

            protected TValue GetValueIfNotDisposed<TValue>(in TValue value) => IsDisposed ? throw GetExceptionForDispose(false) : value;

            protected void ThrowIfDisposed()
            {
                if (IsDisposed)

                    throw GetExceptionForDispose(false);
            }

            protected ILinkedListEnumerable<T> InnerList => GetValueIfNotDisposed(_innerList);

            public ILinkedListNodeEnumerable<T> First => InnerList.First;

            public ILinkedListNodeEnumerable<T> Current => InnerList.Current;

            public ILinkedListNodeEnumerable<T> Last => InnerList.Last;

            public EnumerationDirection EnumerationDirection => InnerList.EnumerationDirection;

            public LinkedCollectionEnumerable(in ILinkedListEnumerable<T> list) => _innerList = list ?? throw GetArgumentNullException(nameof(list));

            public LinkedCollectionEnumerable(in EnumerationDirection enumerationDirection) : this(new LinkedListEnumerable<T>(enumerationDirection)) { /* Left empty. */ }

            public ILinkedListNode<T> Add(T value) => InnerList.Add(value);

            public ILinkedListNodeEnumerable<T> GetLinkedListNodeEnumerable(ILinkedListNode<T> node) => InnerList.GetLinkedListNodeEnumerable(node);

            protected virtual void OnCurrentUpdated(ILinkedListNode<T> node) { /* Left empty. */ }

            protected virtual void OnUpdateCurrent(ILinkedListNode<T> node)
            {
                InnerList.UpdateCurrent(node);

                OnCurrentUpdated(node);
            }

            public void UpdateCurrent(ILinkedListNode<T> node)
            {
                ThrowIfDisposed();

                OnUpdateCurrent(node);
            }

            void ILinkedListEnumerable.UpdateCurrent(IReadOnlyLinkedListNode node) => OnUpdateCurrent(LinkedListEnumerable<T>.GetNode(node, nameof(node)));

            protected virtual bool OnMovePrevious()
            {
                if (InnerList.MovePrevious())
                {
                    OnCurrentUpdated(Current.Node);

                    return true;
                }

                return false;
            }

            public bool MovePrevious()
            {
                ThrowIfDisposed();

                return OnMovePrevious();
            }

            protected virtual bool OnMoveNext()
            {
                if (InnerList.MoveNext())
                {
                    OnCurrentUpdated(Current.Node);

                    return true;
                }

                return false;
            }

            public bool MoveNext()
            {
                ThrowIfDisposed();

                return OnMoveNext();
            }

            public System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetEnumeratorToCurrent(bool keepCurrent) => InnerList.GetEnumeratorToCurrent(keepCurrent);

            public System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetEnumeratorFromCurrent(bool keepCurrent) => InnerList.GetEnumeratorFromCurrent(keepCurrent);

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

            protected virtual void Dispose(bool disposing) => _innerList = null;

            public void Dispose()
            {
                if (IsDisposed)

                    return;

                Dispose(true);

                GC.SuppressFinalize(this);
            }

            ~LinkedCollectionEnumerable() => Dispose(false);

#if !CS8
        ILinkedListNodeEnumerable ILinkedListEnumerable.First => First;

        ILinkedListNodeEnumerable ILinkedListEnumerable.Current => Current;

        ILinkedListNodeEnumerable ILinkedListEnumerable.Last => Last;
#endif
        }

        public class ObservableLinkedCollectionEnumerable<TItems, TList> : LinkedCollectionEnumerable<TItems>, INotifyPropertyChanged, INotifyLinkedCollectionChanged<TItems>, INotifyCollectionChanged where TList : ILinkedList<TItems>, INotifyLinkedCollectionChanged<TItems>, INotifyPropertyChanged
        {
            private NotifyCollectionChangedEventHandler _collectionChanged;
            private PropertyChangedEventHandler _propertyChanged;

            event LinkedCollectionChangedEventHandler<TItems> INotifyLinkedCollectionChanged<TItems>.CollectionChanged
            {
                add => ((INotifyLinkedCollectionChanged<TItems>)InnerList).CollectionChanged += value;

                remove => ((INotifyLinkedCollectionChanged<TItems>)InnerList).CollectionChanged -= value;
            }

            event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
            {
                add => _collectionChanged += value;

                remove => _collectionChanged -= value;
            }

            event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
            {
                add => _propertyChanged += value;

                remove => _propertyChanged -= value;
            }

            public ObservableLinkedCollectionEnumerable(in TList list, in EnumerationDirection enumerationDirection) : base(new ObservableLinkedListEnumerable<TItems, TList>(list, enumerationDirection))
            {
                ((IObservableLinkedListEnumerable<TItems>)InnerList).CollectionChanged += (object sender, LinkedCollectionChangedEventArgs<TItems> e) => OnCollectionChanged(e);

                ((IObservableLinkedListEnumerable<TItems>)InnerList).PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
            }

            protected virtual bool OnCollectionChanged(LinkedCollectionChangedEventArgs<TItems> e)
            {
                System.Collections.Specialized.NotifyCollectionChangedEventArgs _e = TryGetNotifyCollectionChangedEventArgs(InnerList, e);

                if (_e != null)
                {
                    OnCollectionChanged(_e);

                    return true;
                }

                return false;
            }

            protected virtual void OnCollectionChanged(in System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => _collectionChanged?.Invoke(this, e);

            public static System.Collections.Specialized.NotifyCollectionChangedEventArgs TryGetNotifyCollectionChangedEventArgs(in ILinkedListEnumerable<TItems> list, in LinkedCollectionChangedEventArgs<TItems> e)
            {
                switch (e.Action)
                {
                    case LinkedCollectionChangedAction.AddFirst:

                        return new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list.GetLinkedListNodeEnumerable(e.Node), 0);

                    case LinkedCollectionChangedAction.AddLast:

                        return new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list.GetLinkedListNodeEnumerable(e.Node), (int)list.Count - 1);

                    default:

                        return null;
                }
            }

            protected virtual void OnPropertyChanged(in PropertyChangedEventArgs e) => _propertyChanged?.Invoke(this, e);

            protected virtual void OnPropertyChanged(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            protected override void OnCurrentUpdated(ILinkedListNode<TItems> node)
            {
                base.OnCurrentUpdated(node);

                OnPropertyChanged(nameof(Current));
            }

            protected override void Dispose(bool disposing)
            {
                _collectionChanged = null;
                _propertyChanged = null;

                base.Dispose(disposing);
            }
        }

        public class ObservableLinkedCollectionEnumerable<T> : ObservableLinkedCollectionEnumerable<T, ObservableLinkedCollection<T>>
        {
            public ObservableLinkedCollectionEnumerable(in ObservableLinkedCollection<T> list, in EnumerationDirection enumerationDirection) : base(list, enumerationDirection) { /* Left empty. */ }

            public ObservableLinkedCollectionEnumerable(in EnumerationDirection enumerationDirection) : this(new ObservableLinkedCollection<T>(), enumerationDirection) { /* Left empty. */ }
        }
    }
}

#endif
