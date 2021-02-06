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

#if !WinCopies3 && CS7

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using System.Runtime.Serialization;

namespace WinCopies.Collections.DotNetFix.Extensions
{
    [Serializable]
    internal sealed class LinkedListNodeEnumerator<T> : System.Collections.Generic.IEnumerator<System.Collections.Generic.LinkedListNode<T>>, IEnumerable<System.Collections.Generic.LinkedListNode<T>>
    {
        private ILinkedList<T> _list;

        public System.Collections.Generic.LinkedListNode<T> Current { get; private set; }

        object System.Collections.IEnumerator.Current => Current;

        public LinkedListNodeEnumerator(ILinkedList<T> list) => _list = list;

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

        public System.Collections.Generic.IEnumerator<System.Collections.Generic.LinkedListNode<T>> GetEnumerator() => this;

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Obsolete("This type has been replaced by WinCopies.Collections.DotNetFix.Generic.LinkedCollection<T>.")]
    [Serializable]
    public class LinkedCollection<T> : ICollection<T>, System.Collections.Generic.IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
    {
        protected ILinkedList<T> InnerList { get; }

        public System.Collections.Generic.LinkedListNode<T> Last => InnerList.Last;

        public System.Collections.Generic.LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection<T>.IsReadOnly => false;

        public LinkedCollection() : this(new LinkedList<T>()) { }

        public LinkedCollection(in ILinkedList<T> list) => InnerList = list;

        protected virtual void AddItem(T item) => ((ICollection<T>)InnerList).Add(item);

        void ICollection<T>.Add(T item) => AddItem(item);

        protected virtual void AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

        /// <summary>
        /// Adds the specified new node after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(System.Collections.Generic.LinkedListNode{T}, System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddAfter(System.Collections.Generic.LinkedListNode{T}, T)"/>
        public void AddAfter(in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.LinkedListNode<T> newNode) => AddItemAfter(node, newNode);

        protected virtual System.Collections.Generic.LinkedListNode<T> AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

        /// <summary>
        /// Adds a new node containing the specified value after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(System.Collections.Generic.LinkedListNode{T}, T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
        /// <seealso cref="AddAfter(in System.Collections.Generic.LinkedListNode{T}, in System.Collections.Generic.LinkedListNode{T})"/>
        public System.Collections.Generic.LinkedListNode<T> AddAfter(System.Collections.Generic.LinkedListNode<T> node, T value) => AddItemAfter(node, value);

        protected virtual void AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

        /// <summary>
        /// Adds the specified new node before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(System.Collections.Generic.LinkedListNode{T}, System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddBefore(in System.Collections.Generic.LinkedListNode{T}, in T)"/>
        public void AddBefore(in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.LinkedListNode<T> newNode) => AddItemBefore(node, newNode);

        protected virtual System.Collections.Generic.LinkedListNode<T> AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);

        /// <summary>
        /// Adds a new node containing the specified value before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(System.Collections.Generic.LinkedListNode{T}, T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
        /// <seealso cref="AddBefore(in System.Collections.Generic.LinkedListNode{T}, in System.Collections.Generic.LinkedListNode{T})"/>
        public System.Collections.Generic.LinkedListNode<T> AddBefore(in System.Collections.Generic.LinkedListNode<T> node, in T value) => AddItemBefore(node, value);

        protected virtual void AddFirstItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddFirst(node);

        /// <summary>
        /// Adds the specified new node at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddFirst(in T)"/>
        public void AddFirst(in System.Collections.Generic.LinkedListNode<T> node) => AddFirstItem(node);

        protected virtual System.Collections.Generic.LinkedListNode<T> AddFirstItem(T value) => InnerList.AddFirst(value);

        /// <summary>
        /// Adds a new node containing the specified value at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="value">The value to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <seealso cref="AddFirst(in T)"/>
        public System.Collections.Generic.LinkedListNode<T> AddFirst(in T value) => AddFirstItem(value);

        protected virtual void AddLastItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddLast(node);

        /// <summary>
        /// Adds the specified new node at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddLast(in T)"/>
        public void AddLast(in System.Collections.Generic.LinkedListNode<T> node) => AddLastItem(node);

        protected virtual System.Collections.Generic.LinkedListNode<T> AddLastItem(T value) => InnerList.AddLast(value);

        /// <summary>
        /// Adds a new node containing the specified value at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="value">The value to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <seealso cref="AddLast(in System.Collections.Generic.LinkedListNode{T})"/>
        public System.Collections.Generic.LinkedListNode<T> AddLast(in T value) => AddLastItem(value);

        public System.Collections.Generic.LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public System.Collections.Generic.LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        protected virtual void ClearItems() => InnerList.Clear();

        public void Clear() => ClearItems();

        public bool Contains(T item) => InnerList.Contains(item);

        public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

        public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

        public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        protected virtual bool RemoveItem(T item) => InnerList.Remove(item);

        public bool Remove(T item) => RemoveItem(item);

        protected virtual void RemoveItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.Remove(node);

        public void Remove(System.Collections.Generic.LinkedListNode<T> node) => RemoveItem(node);

        protected virtual void RemoveFirstItem() => InnerList.RemoveFirst();

        public void RemoveFirst() => RemoveFirstItem();

        protected virtual void RemoveLastItem() => InnerList.RemoveLast();

        public void RemoveLast() => RemoveLastItem();

        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();
    }

    //public interface INotifySimpleLinkedCollectionChanged<T>
    //{
    //    event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;
    //}

    //public enum LinkedCollectionChangeAction : byte
    //{
    //    AddedFirst = 0,

    //    AddedLast = 1,

    //    AddedBefore = 2,

    //    AddedAfter = 3,

    //    Removed = 4,

    //    Cleared = 5,
    //}

    //public class LinkedCollectionChangedEventArgs<T>
    //{
    //    public LinkedCollectionChangeAction Action { get; }

    //    public System.Collections.Generic.LinkedListNode<T> AddedBeforeOrAfter { get; }

    //    public System.Collections.Generic.LinkedListNode<T> Node { get; }

    //    public LinkedCollectionChangedEventArgs(LinkedCollectionChangeAction action, System.Collections.Generic.LinkedListNode<T> addedBeforeOrAfter, System.Collections.Generic.LinkedListNode<T> node)
    //    {
    //        if (((action == LinkedCollectionChangeAction.AddedBefore || action == LinkedCollectionChangeAction.AddedAfter) && addedBeforeOrAfter == null) || ((action != LinkedCollectionChangeAction.AddedBefore && action != LinkedCollectionChangeAction.AddedAfter) && addedBeforeOrAfter != null) || (action == LinkedCollectionChangeAction.Removed && (node == null || addedBeforeOrAfter != null)) || (action == LinkedCollectionChangeAction.Cleared && (node != null || addedBeforeOrAfter != null)))

    //            throw new ArgumentException($"Invalid combination of {nameof(action)} and {nameof(node)} or {nameof(addedBeforeOrAfter)}.");

    //        Action = action;

    //        AddedBeforeOrAfter = addedBeforeOrAfter;

    //        Node = node;
    //    }
    //}

    //public delegate void LinkedCollectionChangedEventHandler<T>(object sender, LinkedCollectionChangedEventArgs<T> e);

    //public interface INotifyLinkedCollectionChanged<T>
    //{
    //    event LinkedCollectionChangedEventHandler<T> CollectionChanged;
    //}

    [Obsolete("This type has been replaced by WinCopies.Collections.DotNetFix.Generic.ObservableLinkedCollection<T>.")]
    [Serializable]
    public class ObservableLinkedCollection<T> : LinkedCollection<T>, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ObservableLinkedCollection() : base() { }

        public ObservableLinkedCollection(in ILinkedList<T> list) : base(list) { }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

        protected virtual void OnCollectionChanged(LinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in System.Collections.Generic.LinkedListNode<T> addedBefore, in System.Collections.Generic.LinkedListNode<T> addedAfter, in System.Collections.Generic.LinkedListNode<T> node) => OnCollectionChanged(new LinkedCollectionChangedEventArgs<T>(action, addedBefore, addedAfter, node));

        protected override void AddFirstItem(System.Collections.Generic.LinkedListNode<T> node)
        {
            base.AddFirstItem(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddFirst, null, null, node);
        }

        protected override System.Collections.Generic.LinkedListNode<T> AddFirstItem(T value)
        {
            System.Collections.Generic.LinkedListNode<T> result = base.AddFirstItem(value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddFirst, null, null, result);

            return result;
        }

        protected override void AddItem(T item)
        {
            base.AddItem(item);

            RaiseCountPropertyChangedEvent();

            // Assumming that items are added to the end of the list.

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, InnerList.Last);
        }

        protected override void AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode)
        {
            base.AddItemAfter(node, newNode);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, newNode);
        }

        protected override System.Collections.Generic.LinkedListNode<T> AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, T value)
        {
            System.Collections.Generic.LinkedListNode<T> result = base.AddItemAfter(node, value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, result);

            return result;
        }

        protected override void AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode)
        {
            base.AddItemBefore(node, newNode);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, newNode);
        }

        protected override System.Collections.Generic.LinkedListNode<T> AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, T value)
        {
            System.Collections.Generic.LinkedListNode<T> result = base.AddItemBefore(node, value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, result);

            return result;
        }

        protected override void AddLastItem(System.Collections.Generic.LinkedListNode<T> node)
        {
            base.AddLastItem(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, node);
        }

        protected override System.Collections.Generic.LinkedListNode<T> AddLastItem(T value)
        {
            System.Collections.Generic.LinkedListNode<T> result = base.AddLastItem(value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, result);

            return result;
        }

        protected override void ClearItems()
        {
            base.ClearItems();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Reset, null, null, null);
        }

        protected override void RemoveFirstItem()
        {
            System.Collections.Generic.LinkedListNode<T> node = InnerList.First;

            base.RemoveFirstItem();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
        }

        protected override void RemoveItem(System.Collections.Generic.LinkedListNode<T> node)
        {
            base.RemoveItem(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
        }

        protected override bool RemoveItem(T item)
        {
            foreach (System.Collections.Generic.LinkedListNode<T> node in new LinkedListNodeEnumerator<T>(InnerList))

                if (node.Value.Equals(item))
                {
                    base.RemoveItem(node); // This is a custom internal enumerator designed to do not throw when its underlying collection change.

                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);

                    return true;
                }

            return false;
        }

        protected override void RemoveLastItem()
        {
            System.Collections.Generic.LinkedListNode<T> node = InnerList.Last;

            base.RemoveLastItem();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
        }
    }
}
#endif