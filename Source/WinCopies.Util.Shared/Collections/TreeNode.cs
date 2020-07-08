/* Copyright © Pierre Sprimont, 2019
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

#if WinCopies2

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using WinCopies.Util;
using static WinCopies.Util.Util;
// using WinCopies.Util.Data;

namespace WinCopies.Collections
{

    // todo: make non-generic

    ///// <summary>
    ///// Represents a tree node.
    ///// </summary>
    ///// <typeparam name="T">The value type.</typeparam>
    //public abstract class TreeNode<T> : ITreeNode<T> { }

    /// <summary>
    /// Represents a tree node.
    /// </summary>
    /// <typeparam name="T">The value and item type.</typeparam>
    [Serializable]
    [DebuggerDisplay("Value = {Value}, Count = {Count}")]
    public class TreeNode<T> : IReadOnlyTreeNode, WinCopies
#if WinCopies2
.Util
#endif
        .IValueObject, ITreeNode<T>, ICollection<TreeNode<T>>, System.Collections.Generic.IList<TreeNode<T>>, ICollection, System.Collections.IList, IReadOnlyCollection<TreeNode<T>>, System.Collections.Generic.IReadOnlyList<TreeNode<T>>, IReadOnlyCollection<T>, System.Collections.Generic.IReadOnlyList<T>
    {
        /// <summary>
        /// Gets a value that indicates whether this <see cref="TreeNode{T}"/> is read-only. This value is always <see langword="false"/> for this class.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        bool IEquatable<IReadOnlyValueObject>.Equals(IReadOnlyValueObject obj) => new ValueObjectEqualityComparer().Equals(this, obj);

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">Object to compare to the current object.</param>
        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        bool IEquatable<IReadOnlyValueObject<T>>.Equals(IReadOnlyValueObject<T> obj) => new ValueObjectEqualityComparer<T>().Equals(this, obj);

        /// <summary>
        /// Gets the parent of the current node.
        /// </summary>
        public IReadOnlyTreeNode Parent { get; internal set; }

        private T _value;

        /// <summary>
        /// Gets or sets the value of the object.
        /// </summary>
        public T Value { get => _value; set => SeT(value); }

        private void SeT(T newValue) => _value = newValue;

        object IValueObject.Value { get => Value; set => Value = (T)value; }

        object IReadOnlyValueObject.Value => Value;

        // protected TreeNode() { }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="TreeNode{T}"/> class.
        ///// </summary>
        ///// <param name="value">The value of the new <see cref="TreeNode{T}"/>.</param>
        //protected TreeNode(T value) => Value = value;

        /// <summary>
        /// Determines whether this object is equal to a given object.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><see langword="true"/> if the current object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
        public override bool Equals(object obj) => obj is IReadOnlyTreeNode<T> treeNode
                ? Equals(treeNode)
                : obj is IReadOnlyValueObject<T> _obj
                ? Equals(_obj)
                : obj is IReadOnlyValueObject __obj
                ? Equals(__obj)
                : obj is T value ? Value?.Equals(value) == true : obj is null ? !(Value is object) : false;

        /// <summary>
        /// Returns a hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => Value is object ? Value.GetHashCode() : base.GetHashCode();

        public override string ToString() => Value?.ToString() ?? base.ToString();

        #region IDisposable Support
        private bool disposedValue = false;

        /// <summary>
        /// Removes the unmanaged resources and the managed resources if needed. If you override this method, you should call this implementation of this method in your override implementation to avoid unexpected results when using this object laater.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to dispose managed resources, otherwise <see langword="false"/>.</param>
        protected virtual void Dispose(bool disposing)
        {

            if (disposedValue)

                return;

            if (Value is System.IDisposable _value)

                _value.Dispose();

            this.Parent = null;

            Clear();

            disposedValue = true;

        }

        ~TreeNode()
        {

            Dispose(false);

        }

        public void Dispose()
        {

            Dispose(true);

            GC.SuppressFinalize(this);

        }
        #endregion

        /// <summary>
        /// Returns the default comparer for <see cref="TreeNode{T}"/> objects.
        /// </summary>
        /// <returns>The default comparer for <see cref="TreeNode{T}"/> objects.</returns>
        protected virtual IEqualityComparer<TreeNode<T>> GetDefaultTreeNodeItemsComparer() => (IEqualityComparer<TreeNode<T>>)new ValueObjectEqualityComparer<TreeNode<T>>();

        /// <summary>
        /// Gets the inner <see cref="System.Collections.Generic.IList{T}"/> of this <see cref="TreeNode{T}"/>.
        /// </summary>
        protected System.Collections.Generic.IList<TreeNode<T>> Items { get; }

        // protected virtual ITreeCollection<T> GetDefaultItemCollection() => new TreeCollection<T>(this);

        public TreeNode() : this(value: default) { }

        public TreeNode(System.Collections.Generic.IList<TreeNode<T>> items) : this(default, items) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}"/> class using a custom value.
        /// </summary>
        /// <param name="value">The value of the new <see cref="TreeNode{T}"/>.</param>
        public TreeNode(T value) : this(value, new List<TreeNode<T>>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}"/> class using a custom value and inner <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="value">The value of the new <see cref="TreeNode{T}"/>.</param>
        /// <param name="items">A custom inner <see cref="IList{T}"/>.</param>
        public TreeNode(T value, System.Collections.Generic.IList<TreeNode<T>> items)
        {
            ThrowIfNull(items, nameof(items));

            Value = value;

            Items = items;
        }

        [NonSerialized]
        private object _syncRoot;

        object ICollection.SyncRoot => _syncRoot ?? (_syncRoot = Items is ICollection collection ? collection.SyncRoot : System.Threading.Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null));

        bool ICollection.IsSynchronized => false;

        /// <summary>
        /// Gets or sets the item at the specified index in this <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>The item at the given index.</returns>
        /// <exception cref="IndexOutOfRangeException">The given index is lesser than 0 or greater than <see cref="Count"/>.</exception>
        /// <seealso cref="SetItem(in TreeNode{T})"/>
        public TreeNode<T> this[int index] { get => Items[index]; set => SetItem(index, value); }

        ITreeNode<T> System.Collections.Generic.IReadOnlyList<ITreeNode<T>>.this[int index] => this[index];

        T System.Collections.Generic.IReadOnlyList<T>.this[int index] => this[index].Value;

        object System.Collections.IList.this[int index] { get => throw new NotImplementedException("Currently not implemented."); set => throw new NotImplementedException("Currently not implemented."); }

        ITreeNode<T> System.Collections.Generic.IList<ITreeNode<T>>.this[int index] { get => this[index]; set => this[index] = GetOrThrowIfNotTypeOrNull<TreeNode<T>>(value, nameof(value)); }

        IReadOnlyTreeNode<T> System.Collections.Generic.IReadOnlyList<IReadOnlyTreeNode<T>>.this[int index] => this[index];

        /// <summary>
        /// Gets the number of items that this <see cref="TreeNode{T}"/> directly contains.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Gets a value that indicates whether this <see cref="TreeNode{T}"/> is fixed-size.
        /// </summary>
        public bool IsFixedSize => Items is IList _items ? _items.IsFixedSize : false /*Items.IsReadOnly*/;

        /// <summary>
        /// Returns an <see cref="IEnumerator{T}"/> for this <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <returns><see cref="IEnumerator{T}"/> for this <see cref="TreeNode{T}"/>.</returns>
        public IEnumerator<TreeNode<T>> GetEnumerator() => Items.GetEnumerator();

        IEnumerator<ITreeNode<T>> IEnumerable<ITreeNode<T>>.GetEnumerator() => GetEnumerator();

        IEnumerator<IReadOnlyTreeNode<T>> IEnumerable<IReadOnlyTreeNode<T>>.GetEnumerator() => GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new ValueObjectEnumerator<T>(GetEnumerator());

        // IEnumerator<ITreeNode<T>> IEnumerable<ITreeNode<T>>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => new ValueObjectEnumerator<T>(GetEnumerator());

        /// <summary>
        /// Adds a new item to the end of this <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(TreeNode<T> item) => InsertItem(Count, item);

        int System.Collections.IList.Add(object value) => throw new InvalidOperationException("TreeNodes do not support adding a common object. Add a TreeNode<T> instead.");

        void ICollection<ITreeNode<T>>.Add(ITreeNode<T> item) => Add(GetOrThrowIfNotTypeOrNull<TreeNode<T>>(item, nameof(item)));

        /// <summary>
        /// Checks if this <see cref="TreeNode{T}"/> directly contains a given <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="item">The <see cref="TreeNode{T}"/> to look for.</param>
        /// <returns><see langword="true"/> if this <see cref="TreeNode{T}"/> directly contains the given <see cref="TreeNode{T}"/>, otherwise <see langword="false"/>.</returns>
        public bool Contains(TreeNode<T> item)
        {
            if (item is null)

                return false;

            IEqualityComparer<TreeNode<T>> comp = GetDefaultTreeNodeItemsComparer();

            foreach (TreeNode<T> _item in this)

                if (comp.Equals(_item, item))

                    return true;

            return false;
        }

        /// <summary>
        /// Checks if this <see cref="TreeNode{T}"/> directly contains a given item.
        /// </summary>
        /// <param name="item">The item to look for.</param>
        /// <returns><see langword="true"/> if this <see cref="TreeNode{T}"/> directly contains the given item, otherwise <see langword="false"/>.</returns>
        public bool Contains(T item)
        {
            if (!(item is object))

                return false;

            EqualityComparer<T> comp = EqualityComparer<T>.Default;

            foreach (TreeNode<T> _item in this)

                if (comp.Equals(_item.Value, item))

                    return true;

            return false;
        }

        bool System.Collections.IList.Contains(object value) => value is T item ? Contains(item) : false;

        bool ICollection<ITreeNode<T>>.Contains(ITreeNode<T> item) => item is TreeNode<T> _item ? Contains(_item) : false;

        /// <summary>
        /// Returns the idnex of a given item in this <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="item">The item for which to find the index.</param>
        /// <returns>The index of <paramref name="item"/> if this <see cref="TreeNode{T}"/> contains <paramref name="item"/>, otherwise -1.</returns>
        public int IndexOf(TreeNode<T> item)
        {
            if (item is null)

                return -1;

            IEqualityComparer<TreeNode<T>> comp = GetDefaultTreeNodeItemsComparer();

            for (int i = 0; i < Count; i++)

                if (comp.Equals(this[i], item))

                    return i;

            return -1;
        }

        int System.Collections.Generic.IList<ITreeNode<T>>.IndexOf(ITreeNode<T> item) => item is TreeNode<T> _item ? IndexOf(_item) : -1;

        /// <summary>
        /// Returns the idnex of a given item in this <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="item">The item for which to find out the index.</param>
        /// <returns>The index of <paramref name="item"/> if this <see cref="TreeNode{T}"/> contains <paramref name="item"/>, otherwise -1.</returns>
        public int IndexOf(T item)
        {
            if (!(item is object))

                return -1;

            EqualityComparer<T> comp = EqualityComparer<T>.Default;

            for (int i = 0; i < Count; i++)

                if (comp.Equals(this[i].Value, item))

                    return i;

            return -1;
        }

        int System.Collections.IList.IndexOf(object value) => value is T item ? IndexOf(item) : -1;

        /// <summary>
        /// Removes the item at a given index.
        /// </summary>
        /// <param name="index">The index from which to remove the item.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is lesser than 0 or greater than <see cref="Count"/>.</exception>
        /// <exception cref="NotSupportedException">This <see cref="TreeNode{T}"/> is fixed-size.</exception>
        /// <seealso cref="RemoveItem(int)"/>
        public void RemoveAt(int index) => RemoveItem(index);

        /// <summary>
        /// Removes a given item from the node. The current node must directly contains the given item. This function removes <paramref name="item"/> and returns <see langword="true"/> if <paramref name="item"/> is found, otherwise <see langword="false"/> is returned.
        /// </summary>
        /// <param name="item">The item to remove from the current node.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is removed, otherwise <see langword="false"/>.</returns>
        /// <seealso cref="RemoveItem(int)"/>
        public bool Remove(TreeNode<T> item)
        {
            int index = IndexOf(item);

            if (index > -1)
            {
                RemoveItem(index);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a given item from the node. The current node must directly contains the given item. This function removes <paramref name="item"/> and returns <see langword="true"/> if <paramref name="item"/> is found, otherwise <see langword="false"/> is returned.
        /// </summary>
        /// <param name="item">The item to remove from the current node.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is removed, otherwise <see langword="false"/>.</returns>
        /// <seealso cref="RemoveItem(int)"/>
        public bool Remove(T item)
        {
            if (!(item is object))

                return false;

            EqualityComparer<T> comp = EqualityComparer<T>.Default;

            for (int i = 0; i < Count; i++)

                if (comp.Equals(this[i].Value, item))
                {
                    RemoveItem(i);

                    return true;
                }

            return false;
        }

        bool ICollection<ITreeNode<T>>.Remove(ITreeNode<T> item) => Remove(item as TreeNode<T> ?? throw new ArgumentException($"The given item is not a {typeof(TreeNode<T>).FullName}."));

        void System.Collections.IList.Remove(object value)
        {
            if (value is T _value)

                _ = Remove(_value);
        }

        /// <summary>
        /// Inserts a given item at a specified index in this <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="index">The index of the new item.</param>
        /// <param name="item">The item to insert in this <see cref="TreeNode{T}"/>.</param>
        public void Insert(int index, TreeNode<T> item) => InsertItem(index, item);

        void System.Collections.IList.Insert(int index, object value) => throw new InvalidOperationException("TreeNodes do not support adding common objects. Add a TreeNode<T> instead.");

        void System.Collections.Generic.IList<ITreeNode<T>>.Insert(int index, ITreeNode<T> item) => Insert(index, GetOrThrowIfNotTypeOrNull<TreeNode<T>>(item, nameof(item)));

        /// <summary>
        /// Performs a shallow copy of the items that the current <see cref="TreeNode{T}"/> directly contains starting at a given index of a given array of <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="array">The array in which to store the shallow copies of the items that <see cref="TreeNode{T}"/> directly contains.</param>
        /// <param name="arrayIndex">The index from which to store the items in <paramref name="array"/>.</param>
        public void CopyTo(TreeNode<T>[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

        void ICollection<ITreeNode<T>>.CopyTo(ITreeNode<T>[] array, int arrayIndex)
        {
            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, Count, nameof(array), nameof(arrayIndex));

            if (array is TreeNode<T>[] _array)
            {
                CopyTo(_array, arrayIndex);

                return;
            }

            //if (array is T[] itemsArray)

            //{

            //    foreach (var item in this)

            //        itemsArray[arrayIndex++] = item.Value;

            //    return;

            //}

            // todo: make better checks

            try
            {
                foreach (TreeNode<T> item in this)

                    array[arrayIndex++] = item;
            }

            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid array type.");
            }

        }

        void ICollection.CopyTo(Array array, int arrayIndex) => throw new NotImplementedException("Currently not implemented.");

        //{

        //    ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, Count, nameof(array), nameof(arrayIndex));

        //    if (array is TreeNode<T>[] _array)

        //    {

        //        CopyTo(_array, arrayIndex);

        //        return;

        //    }

        //    //if (array is T[] itemsArray)

        //    //{

        //    //    foreach (var item in this)

        //    //        itemsArray[arrayIndex++] = item.Value;

        //    //    return;

        //    //}

        //    // todo: make better checks

        //    try
        //    {

        //        foreach (TreeNode<T> item in this)

        //            array.SeT(item, arrayIndex++);

        //    }

        //    catch (ArrayTypeMismatchException)

        //    {

        //        try

        //        {

        //            foreach (TreeNode<T> item in this)

        //                array.SeT(item.Value, arrayIndex++);

        //        }

        //        catch (ArrayTypeMismatchException)

        //        {

        //            throw new ArgumentException("Invalid array type.");

        //        }

        //    }

        //}

        private void ThrowOnInvalidItem(TreeNode<T> item)
        {
            if (item.Parent is object)

                throw new InvalidOperationException("The given item already has a parent node.");

            if (item.IsReadOnly)

                throw new ArgumentException("The given item is read-only.");
        }

        private void InsertItem(int index, TreeNode<T> item)
        {
            ThrowOnInvalidItem(item);

            item.Parent = this;

            if (index == Count)

                Items.Add(item);

            else

                Items.Insert(index, item);
        }

        /// <summary>
        /// Removes all items of this <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <seealso cref="ClearItems"/>
        public void Clear() => ClearItems();

        private void ClearItems()
        {
            foreach (TreeNode<T> item in this)

                item.Parent = null;

            Items.Clear();
        }

        private void RemoveItem(int index)
        {
            this[index].Parent = null;

            Items.RemoveAt(index);
        }

        private void SetItem(int index, TreeNode<T> item)
        {
            ThrowOnInvalidItem(item);

            this[index].Parent = null;

            item.Parent = this;

            Items[index] = item;
        }
    }

    //[Serializable]
    //[DebuggerDisplay("Value = {Value}, Count = {Count}")]
    //public class TreeNodeCollection<T> : ITreeNode<T>, ICollection<TreeNode<T>>, System.Collections.Generic.IList<TreeNode<T>>, ICollection, System.Collections.IList, IReadOnlyCollection<TreeNode<T>>, System.Collections.Generic.IReadOnlyList<TreeNode<T>>, IReadOnlyCollection<T>, System.Collections.Generic.IReadOnlyList<T> where T : T

    //{

    //    protected ITreeNode<T> TreeNode { get; }

    //    public ITreeNode Parent => GetParent();

    //    protected virtual ITreeNode GetParent() => TreeNode.Parent;

    //    protected virtual T GeT() => TreeNode.Value;

    //    protected virtual void SeT(T value) => TreeNode.Value = value;

    //    public T Value { get => GeT(); set => SeT(value); }

    //    public bool IsReadOnly => false;

    //    public int Count => TreeNode.Count;

    //    object IValueObject.Value { get => GeT(); set => SeT(GetOrThrowIfNotType<T>(value, nameof(value))); }

    //    public void Add(ITreeNode<T> item) => throw new NotImplementedException();

    //    protected virtual void InsertItem(int index, ITreeNode<T> item) => TreeNode.Insert(index, item);

    //    public void Clear() => throw new NotImplementedException();

    //    public bool Contains(T item) => throw new NotImplementedException();

    //    public bool Contains(ITreeNode<T> item) => throw new NotImplementedException();

    //    public void CopyTo(ITreeNode<T>[] array, int arrayIndex) => throw new NotImplementedException();

    //    public void Dispose() => throw new NotImplementedException();
    //    public bool Equals(IValueObject other) => throw new NotImplementedException();
    //    public bool Equals(IValueObject<T> other) => throw new NotImplementedException();
    //    public IEnumerator<ITreeNode<T>> GetEnumerator() => throw new NotImplementedException();
    //    public bool Remove(ITreeNode<T> item) => throw new NotImplementedException();
    //    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    //}
}

#endif
