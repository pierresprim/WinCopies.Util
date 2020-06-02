using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;

namespace WinCopies.Collections.DotNetFix
{

    public class QueueCollection : IEnumerable, ICollection, ICloneable
    {
        protected Queue InnerQueue { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="QueueCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="QueueCollection"/>.</value>
        public int Count => InnerQueue.Count;

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

    public class QueueCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        protected Queue<T> InnerQueue { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="QueueCollection{T}"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="QueueCollection{T}"/>.</value>
        public int Count => InnerQueue.Count;

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

    public class StackCollection : IEnumerable, ICollection, ICloneable
    {
        protected Stack InnerStack { get; }

        public int Count => InnerStack.Count;

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

    public class StackCollection<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        protected Stack<T> InnerStack { get; }

        public int Count => InnerStack.Count;

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

    public class LinkedCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, IDeserializationCallback, ISerializable
    {
        protected System.Collections.Generic.LinkedList<T> InnerList { get; }

        public LinkedListNode<T> Last => InnerList.Last;

        public LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection<T>.IsReadOnly => false;

        public LinkedCollection() : this(new LinkedList<T>()) { }

        public LinkedCollection(in LinkedList<T> list) => InnerList = list;

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

    public class ObservableQueueCollection<T> : QueueCollection<T>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableQueueCollection() : base() { }

        public ObservableQueueCollection(in Queue<T> queue) : base(queue) { }

        protected void RaisePropertyChangedEvent(in PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));

        protected override void ClearItems()
        {
            base.ClearItems();

            RaiseCountPropertyChangedEvent();
        }

        protected override T DequeueItem()
        {
            T result = base.DequeueItem();

            RaiseCountPropertyChangedEvent();

            return result;
        }

        protected override void EnqueueItem(T item)
        {
            base.EnqueueItem(item);

            RaiseCountPropertyChangedEvent();
        }

#if NETCORE

        protected override bool TryDequeueItem([MaybeNullWhen(false)] out T result)
        {
            bool succeeded = base.TryDequeueItem(out result);

            RaiseCountPropertyChangedEvent();

            return succeeded;
        }

#endif

    }

    public class ObservableStackCollection<T> : StackCollection<T>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableStackCollection() : base() { }

        public ObservableStackCollection(Stack<T> stack) : base(stack) { }

        protected void RaisePropertyChangedEvent(in PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));

        protected override void ClearItems()
        {
            base.ClearItems();

            RaiseCountPropertyChangedEvent();
        }

        protected override T PopItem()
        {
            T result = base.PopItem();

            RaiseCountPropertyChangedEvent();

            return result;
        }

        protected override void PushItem(T item)
        {
            base.PushItem(item);

            RaiseCountPropertyChangedEvent();
        }

#if NETCORE

        protected override bool TryPopItem(out T result)
        {
            bool succeeded = base.TryPopItem(out result);

            RaiseCountPropertyChangedEvent();

            return succeeded;
        }

#endif

    }

    public class ObservableLinkedCollection<T> : LinkedCollection<T>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableLinkedCollection() : base() { }

        public ObservableLinkedCollection(in LinkedList<T> list) : base(list) { }

        protected void RaisePropertyChangedEvent(in PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));

        protected override void AddFirstItem(LinkedListNode<T> node)
        {
            base.AddFirstItem(node);

            RaiseCountPropertyChangedEvent();
        }

        protected override LinkedListNode<T> AddFirstItem(T value)
        {
            LinkedListNode<T> result = base.AddFirstItem(value);

            RaiseCountPropertyChangedEvent();

            return result;
        }

        protected override void AddItem(T item)
        {
            base.AddItem(item);

            RaiseCountPropertyChangedEvent();
        }

        protected override void AddItemAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            base.AddItemAfter(node, newNode);

            RaiseCountPropertyChangedEvent();
        }

        protected override LinkedListNode<T> AddItemAfter(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> result = base.AddItemAfter(node, value);

            RaiseCountPropertyChangedEvent();

            return result;
        }

        protected override void AddItemBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            base.AddItemBefore(node, newNode);

            RaiseCountPropertyChangedEvent();
        }

        protected override LinkedListNode<T> AddItemBefore(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> result = base.AddItemBefore(node, value);

            RaiseCountPropertyChangedEvent();

            return result;
        }

        protected override void AddLastItem(LinkedListNode<T> node)
        {
            base.AddLastItem(node);

            RaiseCountPropertyChangedEvent();
        }

        protected override LinkedListNode<T> AddLastItem(T value)
        {
            LinkedListNode<T> result = base.AddLastItem(value);

            RaiseCountPropertyChangedEvent();

            return result;
        }

        protected override void ClearItems()
        {
            base.ClearItems();

            RaiseCountPropertyChangedEvent();
        }

        protected override void RemoveFirstItem()
        {
            base.RemoveFirstItem();

            RaiseCountPropertyChangedEvent();
        }

        protected override void RemoveItem(LinkedListNode<T> node)
        {
            base.RemoveItem(node);

            RaiseCountPropertyChangedEvent();
        }

        protected override bool RemoveItem(T item)
        {
            bool result = base.RemoveItem(item);

            RaiseCountPropertyChangedEvent();

            return result;
        }

        protected override void RemoveLastItem()
        {
            base.RemoveLastItem();

            RaiseCountPropertyChangedEvent();
        }
    }
}
