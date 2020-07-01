﻿/* Copyright © Pierre Sprimont, 2020
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
using System.Runtime.Serialization;
using static WinCopies.
#if WinCopies2
    Util.Util;
using WinCopies.Util;
#else
    ThrowHelper;
using WinCopies;
#endif

namespace WinCopies.Collections.DotNetFix
{
    namespace Extensions
    {
        [Serializable]
        internal sealed class LinkedListNodeEnumerator<T> : IEnumerator<LinkedListNode<T>>, IEnumerable<LinkedListNode<T>>
        {
            private ILinkedList<T> _list;

            public LinkedListNode<T> Current { get; private set; }

            object IEnumerator.Current => Current;

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

            public IEnumerator<LinkedListNode<T>> GetEnumerator() => this;

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Serializable]
        public class LinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
        {
            protected ILinkedList<T> InnerList { get; }

            public LinkedListNode<T> Last => InnerList.Last;

            public LinkedListNode<T> First => InnerList.First;

            public int Count => InnerList.Count;

            bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

            bool ICollection<T>.IsReadOnly => false;

            public LinkedCollection() : this(new LinkedList<T>()) { }

            public LinkedCollection(in ILinkedList<T> list) => InnerList = list;

            protected virtual void AddItem(T item) => ((ICollection<T>)InnerList).Add(item);

            void ICollection<T>.Add(T item) => AddItem(item);

            protected virtual void AddItemAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

            /// <summary>
            /// Adds the specified new node after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(LinkedListNode{T}, LinkedListNode{T})"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert <paramref name="newNode"/>.</param>
            /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
            /// <seealso cref="AddAfter(LinkedListNode{T}, T)"/>
            public void AddAfter(in LinkedListNode<T> node, in LinkedListNode<T> newNode) => AddItemAfter(node, newNode);

            protected virtual LinkedListNode<T> AddItemAfter(LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

            /// <summary>
            /// Adds a new node containing the specified value after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(LinkedListNode{T}, T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
            /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
            /// <seealso cref="AddAfter(in LinkedListNode{T}, in LinkedListNode{T})"/>
            public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) => AddItemAfter(node, value);

            protected virtual void AddItemBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

            /// <summary>
            /// Adds the specified new node before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(LinkedListNode{T}, LinkedListNode{T})"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert <paramref name="newNode"/>.</param>
            /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
            /// <seealso cref="AddBefore(in LinkedListNode{T}, in T)"/>
            public void AddBefore(in LinkedListNode<T> node, in LinkedListNode<T> newNode) => AddItemBefore(node, newNode);

            protected virtual LinkedListNode<T> AddItemBefore(LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);

            /// <summary>
            /// Adds a new node containing the specified value before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(LinkedListNode{T}, T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
            /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
            /// <seealso cref="AddBefore(in LinkedListNode{T}, in LinkedListNode{T})"/>
            public LinkedListNode<T> AddBefore(in LinkedListNode<T> node, in T value) => AddItemBefore(node, value);

            protected virtual void AddFirstItem(LinkedListNode<T> node) => InnerList.AddFirst(node);

            /// <summary>
            /// Adds the specified new node at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(LinkedListNode{T})"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
            /// <seealso cref="AddFirst(in T)"/>
            public void AddFirst(in LinkedListNode<T> node) => AddFirstItem(node);

            protected virtual LinkedListNode<T> AddFirstItem(T value) => InnerList.AddFirst(value);

            /// <summary>
            /// Adds a new node containing the specified value at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="value">The value to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <seealso cref="AddFirst(in T)"/>
            public LinkedListNode<T> AddFirst(in T value) => AddFirstItem(value);

            protected virtual void AddLastItem(LinkedListNode<T> node) => InnerList.AddLast(node);

            /// <summary>
            /// Adds the specified new node at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(LinkedListNode{T})"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
            /// <seealso cref="AddLast(in T)"/>
            public void AddLast(in LinkedListNode<T> node) => AddLastItem(node);

            protected virtual LinkedListNode<T> AddLastItem(T value) => InnerList.AddLast(value);

            /// <summary>
            /// Adds a new node containing the specified value at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="value">The value to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <seealso cref="AddLast(in LinkedListNode{T})"/>
            public LinkedListNode<T> AddLast(in T value) => AddLastItem(value);

            public LinkedListNode<T> Find(T value) => InnerList.Find(value);

            public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

            protected virtual void ClearItems() => InnerList.Clear();

            public void Clear() => ClearItems();

            public bool Contains(T item) => InnerList.Contains(item);

            public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

            public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

            public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

            public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

            protected virtual bool RemoveItem(T item) => InnerList.Remove(item);

            public bool Remove(T item) => RemoveItem(item);

            protected virtual void RemoveItem(LinkedListNode<T> node) => InnerList.Remove(node);

            public void Remove(LinkedListNode<T> node) => RemoveItem(node);

            protected virtual void RemoveFirstItem() => InnerList.RemoveFirst();

            public void RemoveFirst() => RemoveFirstItem();

            protected virtual void RemoveLastItem() => InnerList.RemoveLast();

            public void RemoveLast() => RemoveLastItem();

            public IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();
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

        //    public LinkedListNode<T> AddedBeforeOrAfter { get; }

        //    public LinkedListNode<T> Node { get; }

        //    public LinkedCollectionChangedEventArgs(LinkedCollectionChangeAction action, LinkedListNode<T> addedBeforeOrAfter, LinkedListNode<T> node)
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

            protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in LinkedListNode<T> addedBefore, in LinkedListNode<T> addedAfter, in LinkedListNode<T> node) => OnCollectionChanged(new LinkedCollectionChangedEventArgs<T>(action, addedBefore, addedAfter, node));

            protected override void AddFirstItem(LinkedListNode<T> node)
            {
                base.AddFirstItem(node);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddFirst, null, null, node);
            }

            protected override LinkedListNode<T> AddFirstItem(T value)
            {
                LinkedListNode<T> result = base.AddFirstItem(value);

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

            protected override void AddItemAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
            {
                base.AddItemAfter(node, newNode);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, newNode);
            }

            protected override LinkedListNode<T> AddItemAfter(LinkedListNode<T> node, T value)
            {
                LinkedListNode<T> result = base.AddItemAfter(node, value);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, result);

                return result;
            }

            protected override void AddItemBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
            {
                base.AddItemBefore(node, newNode);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, newNode);
            }

            protected override LinkedListNode<T> AddItemBefore(LinkedListNode<T> node, T value)
            {
                LinkedListNode<T> result = base.AddItemBefore(node, value);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, result);

                return result;
            }

            protected override void AddLastItem(LinkedListNode<T> node)
            {
                base.AddLastItem(node);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, node);
            }

            protected override LinkedListNode<T> AddLastItem(T value)
            {
                LinkedListNode<T> result = base.AddLastItem(value);

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
                LinkedListNode<T> node = InnerList.First;

                base.RemoveFirstItem();

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
            }

            protected override void RemoveItem(LinkedListNode<T> node)
            {
                base.RemoveItem(node);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
            }

            protected override bool RemoveItem(T item)
            {
                foreach (LinkedListNode<T> node in new LinkedListNodeEnumerator<T>(InnerList))

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
                LinkedListNode<T> node = InnerList.Last;

                base.RemoveLastItem();

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
            }
        }
    }

    [Serializable]
    internal sealed class LinkedListNodeEnumerator<T> : IEnumerator<LinkedListNode<T>>, IEnumerable<LinkedListNode<T>>
    {
        private System.Collections.Generic.LinkedList<T> _list;

        public LinkedListNode<T> Current { get; private set; }

        object IEnumerator.Current => Current;

        public LinkedListNodeEnumerator(System.Collections.Generic.LinkedList<T> list) => _list = list;

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

        public IEnumerator<LinkedListNode<T>> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Serializable]
    public class QueueCollection : IEnumerable, ICollection, ICloneable
    {
        protected internal Queue InnerQueue { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="QueueCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="QueueCollection"/>.</value>
        public int Count => InnerQueue.Count;

        public bool IsReadOnly => false;

        bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class.
        /// </summary>
        public QueueCollection() : this(new Queue()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class with a custom <see cref="Queue"/>.
        /// </summary>
        /// <param name="queue">The inner <see cref="Queue"/> for this <see cref="QueueCollection"/>.</param>
        public QueueCollection(in Queue queue) => InnerQueue = queue;

        /// <summary>
        /// Creates a shallow copy of the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>A shallow copy of the <see cref="QueueCollection"/>.</returns>
        public object Clone() => InnerQueue.Clone();

        /// <summary>
        /// Removes all objects from the <see cref="QueueCollection"/>. Override this method to provide a custom implementation.
        /// </summary>
        protected virtual void ClearItems() => InnerQueue.Clear();

        /// <summary>
        /// Removes all objects from the <see cref="QueueCollection"/>. Override the <see cref="ClearItems"/> method to provide a custom implementation.
        /// </summary>
        public void Clear() => ClearItems();

        /// <summary>
        /// Determines whether an element is in the <see cref="QueueCollection"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="QueueCollection"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="System.Collections.Queue"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(object item) => InnerQueue.Contains(item);

        /// <summary>
        /// Copies the <see cref="QueueCollection"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> is multidimensional. -or- The number of elements in the source <see cref="QueueCollection"/> is greater than the available space from index to the end of the destination array.</exception>
        /// <exception cref="ArrayTypeMismatchException">The type of the source <see cref="QueueCollection"/> cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(Array array, int index) => InnerQueue.CopyTo(array, index);

        /// <summary>
        /// Removes and returns the object at the beginning of the <see cref="QueueCollection"/>. Override this method to provide a custom implementation.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection"/> is empty.</exception>
        protected virtual object DequeueItem() => InnerQueue.Dequeue();

        /// <summary>
        /// Removes and returns the object at the beginning of the <see cref="QueueCollection"/>. Override the <see cref="DequeueItem"/> to provide a custom implementation.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection"/> is empty.</exception>
        public object Dequeue() => DequeueItem();

        /// <summary>
        /// Adds an object to the end of the <see cref="QueueCollection"/>. Override this method to provide a custom implementation.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="QueueCollection"/>. The value can be <see langword="null"/> for reference types.</param>
        protected virtual void EnqueueItem(in object item) => InnerQueue.Enqueue(item);

        /// <summary>
        /// Adds an object to the end of the <see cref="QueueCollection"/>. Override the <see cref="EnqueueItem(object)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="QueueCollection"/>. The value can be <see langword="null"/> for reference types.</param>
        public void Enqueue(in object item) => EnqueueItem(item);

        /// <summary>
        /// Returns the object at the beginning of the <see cref="QueueCollection"/> without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the <see cref="QueueCollection"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection"/> is empty.</exception>
        public object Peek() => InnerQueue.Peek();

        /// <summary>
        /// Copies the <see cref="QueueCollection"/> elements to a new array.
        /// </summary>
        /// <returns>A new array containing elements copied from the <see cref="QueueCollection"/>.</returns>
        public object[] ToArray() => InnerQueue.ToArray();

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> for the <see cref="QueueCollection"/>.</returns>
        public IEnumerator GetEnumerator() => InnerQueue.GetEnumerator();
    }

    [Serializable]
    public class QueueCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        protected internal Queue<T> InnerQueue { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="QueueCollection{T}"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="QueueCollection{T}"/>.</value>
        public int Count => InnerQueue.Count;

        public bool IsReadOnly => false;

        bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class.
        /// </summary>
        public QueueCollection() : this(new Queue<T>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom <see cref="Queue{T}"/>.
        /// </summary>
        /// <param name="queue">The inner <see cref="Queue{T}"/> for this <see cref="QueueCollection{T}"/>.</param>
        public QueueCollection(in Queue<T> queue) => InnerQueue = queue;

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

        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see cref="QueueCollection{T}"/>, if that number is less than 90 percent of current capacity.
        /// </summary>
        public void TrimExcess() => InnerQueue.TrimExcess();

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
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="QueueCollection{T}"/>.</returns>
        public IEnumerator<T> GetEnumerator() => InnerQueue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueue).GetEnumerator();
    }

    [Serializable]
    public class StackCollection : IEnumerable, ICollection, ICloneable
    {
        protected internal Stack InnerStack { get; }

        public int Count => InnerStack.Count;

        public bool IsReadOnly => false;

        public bool IsSynchronized => InnerStack.IsSynchronized;

        public object SyncRoot => InnerStack.SyncRoot;

        public StackCollection() : this(new Stack()) { }

        public StackCollection(in Stack stack) => InnerStack = stack;

        public object Clone() => InnerStack.Clone();

        protected virtual void ClearItems() => InnerStack.Clear();

        public void Clear() => ClearItems();

        public void Contains(object item) => InnerStack.Contains(item);

        public object Peek() => InnerStack.Peek();

        protected virtual object PopItem() => InnerStack.Pop();

        public object Pop() => PopItem();

        protected virtual void PushItem(object item) => InnerStack.Push(item);

        public void Push(in object item) => PushItem(item);

        public object[] ToArray() => InnerStack.ToArray();

        public void CopyTo(Array array, int index) => InnerStack.CopyTo(array, index);

        public IEnumerator GetEnumerator() => InnerStack.GetEnumerator();
    }

    [Serializable]
    public class StackCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        protected internal Stack<T> InnerStack { get; }

        public int Count => InnerStack.Count;

        public bool IsReadOnly => false;

        bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

        public StackCollection() : this(new Stack<T>()) { }

        public StackCollection(in Stack<T> stack) => InnerStack = stack;

        protected virtual void ClearItems() => InnerStack.Clear();

        public void Clear() => ClearItems();

        public void Contains(T item) => InnerStack.Contains(item);

        public T Peek() => InnerStack.Peek();

        protected virtual T PopItem() => InnerStack.Pop();

        public T Pop() => PopItem();

        protected virtual void PushItem(T item) => InnerStack.Push(item);

        public void Push(in T item) => PushItem(item);

        public T[] ToArray() => InnerStack.ToArray();

        public void TrimExcess() => InnerStack.TrimExcess();

#if NETCORE

        public bool TryPeek(out T result) => InnerStack.TryPeek(out result);

        protected virtual bool TryPopItem(out T result) => InnerStack.TryPop(out result);

        public bool TryPop(out T result) => TryPopItem(out result);

#endif

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStack).CopyTo(array, index);

        public void CopyTo(in T[] array, in int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => InnerStack.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();
    }

    [Serializable]
    public class LinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
    {
        protected internal System.Collections.Generic.LinkedList<T> InnerList { get; }

        public LinkedListNode<T> Last => InnerList.Last;

        public LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection<T>.IsReadOnly => false;

        public LinkedCollection() : this(new LinkedList<T>()) { }

        public LinkedCollection(in System.Collections.Generic.LinkedList<T> list) => InnerList = list;

        protected virtual void AddItem(T item) => ((ICollection<T>)InnerList).Add(item);

        void ICollection<T>.Add(T item) => AddItem(item);

        protected virtual void AddItemAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

        /// <summary>
        /// Adds the specified new node after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(LinkedListNode{T}, LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddAfter(LinkedListNode{T}, T)"/>
        public void AddAfter(in LinkedListNode<T> node, in LinkedListNode<T> newNode) => AddItemAfter(node, newNode);

        protected virtual LinkedListNode<T> AddItemAfter(LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

        /// <summary>
        /// Adds a new node containing the specified value after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(LinkedListNode{T}, T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
        /// <seealso cref="AddAfter(in LinkedListNode{T}, in LinkedListNode{T})"/>
        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) => AddItemAfter(node, value);

        protected virtual void AddItemBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

        /// <summary>
        /// Adds the specified new node before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(LinkedListNode{T}, LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddBefore(in LinkedListNode{T}, in T)"/>
        public void AddBefore(in LinkedListNode<T> node, in LinkedListNode<T> newNode) => AddItemBefore(node, newNode);

        protected virtual LinkedListNode<T> AddItemBefore(LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);

        /// <summary>
        /// Adds a new node containing the specified value before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(LinkedListNode{T}, T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
        /// <seealso cref="AddBefore(in LinkedListNode{T}, in LinkedListNode{T})"/>
        public LinkedListNode<T> AddBefore(in LinkedListNode<T> node, in T value) => AddItemBefore(node, value);

        protected virtual void AddFirstItem(LinkedListNode<T> node) => InnerList.AddFirst(node);

        /// <summary>
        /// Adds the specified new node at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddFirst(in T)"/>
        public void AddFirst(in LinkedListNode<T> node) => AddFirstItem(node);

        protected virtual LinkedListNode<T> AddFirstItem(T value) => InnerList.AddFirst(value);

        /// <summary>
        /// Adds a new node containing the specified value at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="value">The value to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <seealso cref="AddFirst(in T)"/>
        public LinkedListNode<T> AddFirst(in T value) => AddFirstItem(value);

        protected virtual void AddLastItem(LinkedListNode<T> node) => InnerList.AddLast(node);

        /// <summary>
        /// Adds the specified new node at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddLast(in T)"/>
        public void AddLast(in LinkedListNode<T> node) => AddLastItem(node);

        protected virtual LinkedListNode<T> AddLastItem(T value) => InnerList.AddLast(value);

        /// <summary>
        /// Adds a new node containing the specified value at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="value">The value to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
        /// <seealso cref="AddLast(in LinkedListNode{T})"/>
        public LinkedListNode<T> AddLast(in T value) => AddLastItem(value);

        public LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        protected virtual void ClearItems() => InnerList.Clear();

        public void Clear() => ClearItems();

        public bool Contains(T item) => InnerList.Contains(item);

        public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

        public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

        public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        protected virtual bool RemoveItem(T item) => InnerList.Remove(item);

        public bool Remove(T item) => RemoveItem(item);

        protected virtual void RemoveItem(LinkedListNode<T> node) => InnerList.Remove(node);

        public void Remove(LinkedListNode<T> node) => RemoveItem(node);

        protected virtual void RemoveFirstItem() => InnerList.RemoveFirst();

        public void RemoveFirst() => RemoveFirstItem();

        protected virtual void RemoveLastItem() => InnerList.RemoveLast();

        public void RemoveLast() => RemoveLastItem();

        public IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();
    }

    public class SimpleLinkedCollectionChangedEventArgs<T>
    {
        public NotifyCollectionChangedAction Action { get; }

        public T Item { get; }

        public SimpleLinkedCollectionChangedEventArgs(NotifyCollectionChangedAction action, T item)
        {
            Action = action;

            Item = item;
        }
    }

    public delegate void SimpleLinkedCollectionChangedEventHandler<T>(object sender, SimpleLinkedCollectionChangedEventArgs<T> e);

    public interface INotifySimpleLinkedCollectionChanged<T>
    {
        event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;
    }

    public enum LinkedCollectionChangedAction : byte
    {
        AddFirst = 0,

        AddLast = 1,

        AddBefore = 2,

        AddAfter = 3,

        Remove = 4,

        //Replace = 5,

        Move = 6,

        Reset = 7,
    }

    public class LinkedCollectionChangedEventArgs<T>
    {
        public LinkedCollectionChangedAction Action { get; }

        public LinkedListNode<T> AddedBefore { get; }

        public LinkedListNode<T> AddedAfter { get; }

        public LinkedListNode<T> Node { get; }

        public LinkedCollectionChangedEventArgs(LinkedCollectionChangedAction action, LinkedListNode<T> addedBefore, LinkedListNode<T> addedAfter, LinkedListNode<T> node)
        {
            bool check(LinkedCollectionChangedAction _action, LinkedListNode<T> parameter) => (_action == action && parameter == null) || !(_action == action || parameter == null);

            if ((action == LinkedCollectionChangedAction.Reset && (node != null || addedBefore != null || addedAfter != null))
                || (action != LinkedCollectionChangedAction.Reset && node == null)
                || (action.IsValidEnumValue(true, LinkedCollectionChangedAction.AddFirst, LinkedCollectionChangedAction.AddLast) && (addedBefore != null || addedAfter != null))
                || (action == LinkedCollectionChangedAction.Move && addedBefore == null && addedAfter == null)
                || check(LinkedCollectionChangedAction.AddBefore, addedBefore)
                || check(LinkedCollectionChangedAction.AddAfter, addedAfter)
                || (action.IsValidEnumValue(true, LinkedCollectionChangedAction.Remove, LinkedCollectionChangedAction.Reset) && !(addedBefore == null && addedAfter == null)))

                throw new ArgumentException($"Invalid combination of {nameof(action)} and {nameof(node)}, {nameof(addedBefore)} or {nameof(addedAfter)}.");

            Action = action;

            AddedBefore = addedBefore;

            AddedAfter = addedAfter;

            Node = node;
        }
    }

    public delegate void LinkedCollectionChangedEventHandler<T>(object sender, LinkedCollectionChangedEventArgs<T> e);

    public interface INotifyLinkedCollectionChanged<T>
    {
        event LinkedCollectionChangedEventHandler<T> CollectionChanged;
    }

    [Serializable]
    public class ObservableQueueCollection<T> : QueueCollection<T>, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ObservableQueueCollection() : base() { }

        public ObservableQueueCollection(in Queue<T> queue) : base(queue) { }

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
    public class ObservableStackCollection<T> : StackCollection<T>, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ObservableStackCollection() : base() { }

        public ObservableStackCollection(in Stack<T> stack) : base(stack) { }

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
    public class ObservableLinkedCollection<T> : LinkedCollection<T>, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ObservableLinkedCollection() : base() { }

        public ObservableLinkedCollection(in LinkedList<T> list) : base(list) { }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

        protected virtual void OnCollectionChanged(LinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in LinkedListNode<T> addedBefore, in LinkedListNode<T> addedAfter, in LinkedListNode<T> node) => OnCollectionChanged(new LinkedCollectionChangedEventArgs<T>(action, addedBefore, addedAfter, node));

        protected override void AddFirstItem(LinkedListNode<T> node)
        {
            base.AddFirstItem(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddFirst, null, null, node);
        }

        protected override LinkedListNode<T> AddFirstItem(T value)
        {
            LinkedListNode<T> result = base.AddFirstItem(value);

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

        protected override void AddItemAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            base.AddItemAfter(node, newNode);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, newNode);
        }

        protected override LinkedListNode<T> AddItemAfter(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> result = base.AddItemAfter(node, value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, result);

            return result;
        }

        protected override void AddItemBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            base.AddItemBefore(node, newNode);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, newNode);
        }

        protected override LinkedListNode<T> AddItemBefore(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> result = base.AddItemBefore(node, value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, result);

            return result;
        }

        protected override void AddLastItem(LinkedListNode<T> node)
        {
            base.AddLastItem(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, node);
        }

        protected override LinkedListNode<T> AddLastItem(T value)
        {
            LinkedListNode<T> result = base.AddLastItem(value);

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
            LinkedListNode<T> node = InnerList.First;

            base.RemoveFirstItem();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
        }

        protected override void RemoveItem(LinkedListNode<T> node)
        {
            base.RemoveItem(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
        }

        protected override bool RemoveItem(T item)
        {
            foreach (LinkedListNode<T> node in new LinkedListNodeEnumerator<T>(InnerList))

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
            LinkedListNode<T> node = InnerList.Last;

            base.RemoveLastItem();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
        }
    }



    [Serializable]
    public class ReadOnlyQueueCollection : IEnumerable, ICollection, ICloneable
    {
        protected Queue InnerQueue { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="QueueCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="QueueCollection"/>.</value>
        public int Count => InnerQueue.Count;

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class with a custom <see cref="Queue"/>.
        /// </summary>
        /// <param name="queue">The inner <see cref="Queue"/> for this <see cref="QueueCollection"/>.</param>
        public ReadOnlyQueueCollection(in Queue queue) => InnerQueue = queue;

        public ReadOnlyQueueCollection(in QueueCollection queueCollection) : this(queueCollection.InnerQueue) { }

        /// <summary>
        /// Creates a shallow copy of the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>A shallow copy of the <see cref="QueueCollection"/>.</returns>
        public object Clone() => InnerQueue.Clone();

        /// <summary>
        /// Determines whether an element is in the <see cref="QueueCollection"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="QueueCollection"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="System.Collections.Queue"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(object item) => InnerQueue.Contains(item);

        /// <summary>
        /// Copies the <see cref="QueueCollection"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> is multidimensional. -or- The number of elements in the source <see cref="QueueCollection"/> is greater than the available space from index to the end of the destination array.</exception>
        /// <exception cref="ArrayTypeMismatchException">The type of the source <see cref="QueueCollection"/> cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(Array array, int index) => InnerQueue.CopyTo(array, index);

        /// <summary>
        /// Returns the object at the beginning of the <see cref="QueueCollection"/> without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the <see cref="QueueCollection"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection"/> is empty.</exception>
        public object Peek() => InnerQueue.Peek();

        /// <summary>
        /// Copies the <see cref="QueueCollection"/> elements to a new array.
        /// </summary>
        /// <returns>A new array containing elements copied from the <see cref="QueueCollection"/>.</returns>
        public object[] ToArray() => InnerQueue.ToArray();

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> for the <see cref="QueueCollection"/>.</returns>
        public IEnumerator GetEnumerator() => InnerQueue.GetEnumerator();
    }

    [Serializable]
    public class ReadOnlyQueueCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        protected Queue<T> InnerQueue { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="QueueCollection{T}"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="QueueCollection{T}"/>.</value>
        public int Count => InnerQueue.Count;

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom <see cref="Queue{T}"/>.
        /// </summary>
        /// <param name="queue">The inner <see cref="Queue{T}"/> for this <see cref="QueueCollection{T}"/>.</param>
        public ReadOnlyQueueCollection(in Queue<T> queue) => InnerQueue = queue;

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
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="QueueCollection{T}"/>.</returns>
        public IEnumerator<T> GetEnumerator() => InnerQueue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueue).GetEnumerator();
    }

    [Serializable]
    public class ReadOnlyStackCollection : IEnumerable, ICollection, ICloneable
    {
        protected Stack InnerStack { get; }

        public int Count => InnerStack.Count;

        public bool IsReadOnly => true;

        public bool IsSynchronized => InnerStack.IsSynchronized;

        public object SyncRoot => InnerStack.SyncRoot;

        public ReadOnlyStackCollection(in Stack stack) => InnerStack = stack;

        public ReadOnlyStackCollection(in StackCollection stackCollection) : this(stackCollection.InnerStack) { }

        public object Clone() => InnerStack.Clone();

        public void Contains(object item) => InnerStack.Contains(item);

        public object Peek() => InnerStack.Peek();

        public object[] ToArray() => InnerStack.ToArray();

        public void CopyTo(Array array, int index) => InnerStack.CopyTo(array, index);

        public IEnumerator GetEnumerator() => InnerStack.GetEnumerator();
    }

    [Serializable]
    public class ReadOnlyStackCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        protected Stack<T> InnerStack { get; }

        public int Count => InnerStack.Count;

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

        public ReadOnlyStackCollection(in Stack<T> stack) => InnerStack = stack;

        public ReadOnlyStackCollection(in StackCollection<T> stackCollection) : this(stackCollection.InnerStack) { }

        public void Contains(T item) => InnerStack.Contains(item);

        public T Peek() => InnerStack.Peek();

        public T[] ToArray() => InnerStack.ToArray();

#if NETCORE

        public bool TryPeek(out T result) => InnerStack.TryPeek(out result);

#endif

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStack).CopyTo(array, index);

        public void CopyTo(in T[] array, in int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => InnerStack.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();
    }

    [Serializable]
    public class ReadOnlyLinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
    {
        protected System.Collections.Generic.LinkedList<T> InnerList { get; }

        public LinkedListNode<T> Last => InnerList.Last;

        public LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection<T>.IsReadOnly => true;

        public ReadOnlyLinkedCollection(in System.Collections.Generic.LinkedList<T> list) => InnerList = list;

        public ReadOnlyLinkedCollection(in LinkedCollection<T> listCollection) : this(listCollection.InnerList) { }

        public LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        public bool Contains(T item) => InnerList.Contains(item);

        public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

        public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

        public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        public IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();

        void ICollection<T>.Add(T item) => throw new InvalidOperationException("The collection is read-only.");

        void ICollection<T>.Clear() => throw new InvalidOperationException("The collection is read-only.");

        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException("The collection is read-only.");
    }

    [Serializable]
    public class ReadOnlyObservableQueueCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
    {
        protected ObservableQueueCollection<T> InnerQueueCollection { get; }

        public int Count => InnerQueueCollection.Count;

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

        public IEnumerator<T> GetEnumerator() => InnerQueueCollection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueueCollection).GetEnumerator();

        public void CopyTo(T[] array, int arrayIndex) => InnerQueueCollection.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueueCollection).CopyTo(array, index);
    }

    [Serializable]
    public class ReadOnlyObservableStackCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
    {
        protected ObservableStackCollection<T> InnerStackCollection { get; }

        public int Count => InnerStackCollection.Count;

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

        public IEnumerator<T> GetEnumerator() => InnerStackCollection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStackCollection).GetEnumerator();

        public void CopyTo(T[] array, int arrayIndex) => InnerStackCollection.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStackCollection).CopyTo(array, index);
    }

    [Serializable]
    public class ReadOnlyObservableLinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>
    {
        protected ObservableLinkedCollection<T> InnerLinkedCollection { get; }

        public int Count => InnerLinkedCollection.Count;

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerLinkedCollection).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerLinkedCollection).SyncRoot;

        public event PropertyChangedEventHandler PropertyChanged;

        public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ReadOnlyObservableLinkedCollection(in ObservableLinkedCollection<T> linkedCollection)
        {
            InnerLinkedCollection = linkedCollection ?? throw GetArgumentNullException(nameof(linkedCollection));

            InnerLinkedCollection.CollectionChanged += (object sender, LinkedCollectionChangedEventArgs<T> e) => OnCollectionChanged(e);

            InnerLinkedCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

        protected virtual void OnCollectionChanged(LinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in LinkedListNode<T> addedBefore, in LinkedListNode<T> addedAfter, in LinkedListNode<T> node) => OnCollectionChanged(new LinkedCollectionChangedEventArgs<T>(action, addedBefore, addedAfter, node));

        public IEnumerator<T> GetEnumerator() => InnerLinkedCollection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerLinkedCollection).GetEnumerator();

        public void CopyTo(T[] array, int arrayIndex) => InnerLinkedCollection.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerLinkedCollection).CopyTo(array, index);

        void ICollection<T>.Add(T item) => ((ICollection<T>)InnerLinkedCollection).Add(item);

        void ICollection<T>.Clear() => InnerLinkedCollection.Clear();

        public bool Contains(T item) => InnerLinkedCollection.Contains(item);

        bool ICollection<T>.Remove(T item) => InnerLinkedCollection.Remove(item);
    }
}
