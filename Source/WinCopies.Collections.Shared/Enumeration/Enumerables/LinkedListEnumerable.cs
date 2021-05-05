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

namespace WinCopies.Collections.Generic
{
    public interface ILinkedListNodeEnumerable
    {
        IReadOnlyLinkedListNode Node { get; }

        ILinkedListEnumerable List { get; }

        bool CanMovePreviousFromCurrent { get; }

        bool CanMoveNextFromCurrent { get; }
    }

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

    public interface ILinkedListEnumerable : System.Collections.IEnumerable
    {
        ILinkedListNodeEnumerable First { get; }

        ILinkedListNodeEnumerable Current { get; }

        ILinkedListNodeEnumerable Last { get; }

        EnumerationDirection EnumerationDirection { get; }

        bool MovePrevious();

        bool MoveNext();

        void UpdateCurrent(IReadOnlyLinkedListNode node);
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

    public interface IObservableLinkedListEnumerable<T> : ILinkedListEnumerable<T>, INotifyLinkedCollectionChanged<T>
    {

    }

    public class LinkedListEnumerable<TItems, TList> : ILinkedListEnumerable<TItems> where TList : ILinkedList<TItems>, INotifyLinkedCollectionChanged<TItems>
    {
        private ILinkedListNodeEnumerable<TItems> _first;
        private ILinkedListNodeEnumerable<TItems> _current;
        private ILinkedListNodeEnumerable<TItems> _last;

        protected TList InnerList { get; }

        public ILinkedListNodeEnumerable<TItems> First { get => _first; set => _first = InnerList.Equals(value.Node.List) ? value : throw GetInvalidNodeException(nameof(value)); }

        public ILinkedListNodeEnumerable<TItems> Current => _current
#if CS8
            ??=
#else
            ?? (_current =
#endif
            GetLinkedListNodeEnumerable(InnerList.First)
#if !CS8
            )
#endif
            ;

        public ILinkedListNodeEnumerable<TItems> Last { get => _last; set => _last = InnerList.Equals(value.Node.List) ? value : throw GetInvalidNodeException(nameof(value)); }

        public EnumerationDirection EnumerationDirection { get; }

        private void InnerList_CollectionChanged(object sender, LinkedCollectionChangedEventArgs<TItems> e)
        {
            if (e.Action == LinkedCollectionChangedAction.Remove)

                if (_first.Node == e.Node)

                    _first = null;

            if (_last.Node == e.Node)

                _last = null;
        }

        public LinkedListEnumerable(in TList list, in ILinkedListNode<TItems> first, in ILinkedListNode<TItems> current, in ILinkedListNode<TItems> last, in EnumerationDirection enumerationDirection)
        {
            InnerList = list == null ? throw GetArgumentNullException(nameof(list)) : list;

            list.CollectionChanged += InnerList_CollectionChanged;

            _first = GetLinkedListNodeEnumerable(first);

            _last = GetLinkedListNodeEnumerable(last);

            _current = GetLinkedListNodeEnumerable(current ?? first ?? list.First);

            EnumerationDirection = enumerationDirection;
        }

        public LinkedListEnumerable(in TList list, in EnumerationDirection enumerationDirection) : this(list, null, null, null, enumerationDirection) { /* Left empty. */ }

        public virtual ILinkedListNodeEnumerable<TItems> GetLinkedListNodeEnumerable(ILinkedListNode<TItems> node) => new LinkedListNodeEnumerable<TItems>(node, this);

        private bool Move(in ILinkedListNode<TItems> node)
        {
            if (node == null)

                return false;

            _current = GetLinkedListNodeEnumerable(node);

            return true;
        }

        public bool MovePrevious() => Move(EnumerationDirection == EnumerationDirection.FIFO ? Current?.Node.Previous : Current?.Node.Next);

        public bool MoveNext() => Move(EnumerationDirection == EnumerationDirection.FIFO ? Current?.Node.Next : Current?.Node.Previous);

        public ILinkedListNode<TItems> Add(TItems value) => EnumerationDirection == EnumerationDirection.FIFO ? InnerList.AddLast(value) : InnerList.AddFirst(value);

        public void UpdateCurrent(ILinkedListNode<TItems> node) => _current = GetLinkedListNodeEnumerable(node ?? throw GetArgumentNullException(nameof(node)));

        public static ILinkedListNode<TItems> TryGetNode(in IReadOnlyLinkedListNode node) => node as ILinkedListNode<TItems>;

        public static ArgumentException GetInvalidNodeException(in string argumentName) => throw new ArgumentException($"The given node does not implement the {nameof(ILinkedListNode<TItems>)} interface.", argumentName);

        public static ILinkedListNode<TItems> GetNode(in IReadOnlyLinkedListNode node, in string argumentName) => TryGetNode(node) ?? throw GetInvalidNodeException(argumentName);

        void ILinkedListEnumerable.UpdateCurrent(IReadOnlyLinkedListNode node) => UpdateCurrent(GetNode(node, nameof(node)));

        public System.Collections.Generic.IEnumerator<ILinkedListNode<TItems>> GetEnumeratorToCurrent(bool keepCurrent) => Util.GetNodeEnumerator(InnerList, EnumerationDirection.FIFO, InnerList.First, keepCurrent ? Current.Node : Current.Node.Previous);

        public System.Collections.Generic.IEnumerator<ILinkedListNode<TItems>> GetEnumeratorFromCurrent(bool keepCurrent) => Util.GetNodeEnumerator(InnerList, EnumerationDirection.FIFO, keepCurrent ? Current.Node : Current.Node.Next, InnerList.Last);

        public System.Collections.Generic.IEnumerator<TItems> GetEnumerator() => new Enumerable<ILinkedListNode<TItems>>(() => Util.GetNodeEnumerator(InnerList, EnumerationDirection.FIFO, InnerList.First, InnerList.Last)).Select(node => node.Value).GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => InnerList.GetNodeEnumerator().SelectConverter(node => GetLinkedListNodeEnumerable(node));

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

    public class ObservableLinkedListEnumerable<TItems, TList> : LinkedListEnumerable<TItems, TList>, IObservableLinkedListEnumerable<TItems> where TList : ILinkedList<TItems>, INotifyLinkedCollectionChanged<TItems>
    {
        event LinkedCollectionChangedEventHandler<TItems> INotifyLinkedCollectionChanged<TItems>.CollectionChanged
        {
            add => ((TList)InnerList).CollectionChanged += value;

            remove => ((TList)InnerList).CollectionChanged -= value;
        }

        public ObservableLinkedListEnumerable(in TList list, in EnumerationDirection enumerationDirection) : base(list, null, null, null, enumerationDirection) { /* Left empty. */ }
    }

    public class LinkedCollectionEnumerable<T> : ILinkedListEnumerable<T>
    {
        protected ILinkedListEnumerable<T> InnerList { get; }

        public ILinkedListNodeEnumerable<T> First => InnerList.First;

        public ILinkedListNodeEnumerable<T> Current => InnerList.Current;

        public ILinkedListNodeEnumerable<T> Last => InnerList.Last;

        public EnumerationDirection EnumerationDirection => InnerList.EnumerationDirection;

        public LinkedCollectionEnumerable(in ILinkedListEnumerable<T> list) => InnerList = list;

        public LinkedCollectionEnumerable(in EnumerationDirection enumerationDirection) : this(new LinkedListEnumerable<T>(enumerationDirection)) { /* Left empty. */ }

        public ILinkedListNode<T> Add(T value) => InnerList.Add(value);

        public ILinkedListNodeEnumerable<T> GetLinkedListNodeEnumerable(ILinkedListNode<T> node) => InnerList.GetLinkedListNodeEnumerable(node);

        protected virtual void OnCurrentUpdated(ILinkedListNode<T> node) { /* Left empty. */ }

        protected virtual void OnUpdateCurrent(ILinkedListNode<T> node)
        {
            InnerList.UpdateCurrent(node);

            OnCurrentUpdated(node);
        }

        public void UpdateCurrent(ILinkedListNode<T> node) => OnUpdateCurrent(node);

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

        public bool MovePrevious() => OnMovePrevious();

        protected virtual bool OnMoveNext()
        {
            if (InnerList.MoveNext())
            {
                OnCurrentUpdated(Current.Node);

                return true;
            }

            return false;
        }

        public bool MoveNext() => OnMoveNext();

        public System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetEnumeratorToCurrent(bool keepCurrent) => InnerList.GetEnumeratorToCurrent(keepCurrent);

        public System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetEnumeratorFromCurrent(bool keepCurrent) => InnerList.GetEnumeratorFromCurrent(keepCurrent);

        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

#if !CS8
        ILinkedListNodeEnumerable ILinkedListEnumerable.First => First;

        ILinkedListNodeEnumerable ILinkedListEnumerable.Current => Current;

        ILinkedListNodeEnumerable ILinkedListEnumerable.Last => Last;
#endif
    }

    public class ObservableLinkedCollectionEnumerable<TItems, TList> : LinkedCollectionEnumerable<TItems>, INotifyPropertyChanged, INotifyLinkedCollectionChanged<TItems>, INotifyCollectionChanged where TList : ILinkedList<TItems>, INotifyLinkedCollectionChanged<TItems>
    {
        private NotifyCollectionChangedEventHandler _collectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableLinkedCollectionEnumerable(in TList list, in EnumerationDirection enumerationDirection) : base(new ObservableLinkedListEnumerable<TItems, TList>(list, enumerationDirection)) => ((IObservableLinkedListEnumerable<TItems>)InnerList).CollectionChanged += ObservableLinkedCollectionEnumerable_CollectionChanged;

        private void ObservableLinkedCollectionEnumerable_CollectionChanged(object sender, LinkedCollectionChangedEventArgs<TItems> e)
        {
            System.Collections.Specialized.NotifyCollectionChangedEventArgs _e = TryGetNotifyCollectionChangedEventArgs(InnerList, e);

            if (_e != null)

                _collectionChanged?.Invoke(this, _e);
        }

        public static System.Collections.Specialized.NotifyCollectionChangedEventArgs TryGetNotifyCollectionChangedEventArgs(in ILinkedListEnumerable<TItems> list, in LinkedCollectionChangedEventArgs<TItems> e)
        {
            switch (e.Action)
            {
                case LinkedCollectionChangedAction.AddFirst:

                    return new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list.GetLinkedListNodeEnumerable(e.Node), 0);

                case LinkedCollectionChangedAction.AddLast:

                    return new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list.GetLinkedListNodeEnumerable(e.Node));

                default:

                    return null;
            }
        }

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

        protected void OnPropertyChanged(in string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected override void OnCurrentUpdated(ILinkedListNode<TItems> node)
        {
            base.OnCurrentUpdated(node);

            OnPropertyChanged(nameof(Current));
        }
    }

    public class ObservableLinkedCollectionEnumerable<T> : ObservableLinkedCollectionEnumerable<T, ObservableLinkedCollection<T>>
    {
        public ObservableLinkedCollectionEnumerable(in ObservableLinkedCollection<T> list, in EnumerationDirection enumerationDirection) : base(list, enumerationDirection) { /* Left empty. */ }

        public ObservableLinkedCollectionEnumerable(in EnumerationDirection enumerationDirection) : this(new ObservableLinkedCollection<T>(), enumerationDirection) { /* Left empty. */ }
    }
}

#endif
