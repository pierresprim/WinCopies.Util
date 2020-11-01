/* Copyright © Pierre Sprimont, 2019
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using WinCopies.Util;
//using static WinCopies.Util.Util;

//namespace WinCopies.Collections
//{

//    // todo: make non-generic

//    /// <summary>
//    /// Represents a tree node.
//    /// </summary>
//    /// <typeparam name="T">The value type.</typeparam>
//    public abstract class ReadOnlyTreeNode<T> : ITreeNode<T>

//    {
//        private T _value;

//        /// <summary>
//        /// Gets a value that indicates whether this <see cref="ReadOnlyTreeNode{T}"/> is read-only. This value is always <see langword="false"/> for this class.
//        /// </summary>
//        public bool IsReadOnly => true;

//        /// <summary>
//        /// Determines whether this object is equal to a given object.
//        /// </summary>
//        /// <param name="obj">Object to compare to the current object.</param>
//        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
//        public bool Equals(IValueObject obj) => new ValueObjectEqualityComparer().Equals(this, obj);

//        /// <summary>
//        /// Determines whether this object is equal to a given object.
//        /// </summary>
//        /// <param name="obj">Object to compare to the current object.</param>
//        /// <returns><see langword="true"/> if this object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
//        public bool Equals(IValueObject<T> obj) => new ValueObjectEqualityComparer<T>().Equals(this, obj);

//        /// <summary>
//        /// Gets the parent of the current node.
//        /// </summary>
//        public virtual ITreeNode Parent { get; protected internal set; }

//        /// <summary>
//        /// Gets the value of the object.
//        /// </summary>
//        public virtual T Value { get => _value; set => throw new NotSupportedException("The current node is read-only."); }

//        protected virtual void SetValue(T newValue) => _value = newValue ; 

//        object IValueObject.Value { get => _value; set => throw new NotSupportedException("The current node is read-only."); }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ReadOnlyTreeNode{T}"/> class.
//        /// </summary>
//        /// <param name="value">The value of the new <see cref="ReadOnlyTreeNode{T}"/>.</param>
//        protected ReadOnlyTreeNode(T value) => _value = value;

//        /// <summary>
//        /// Determines whether this object is equal to a given object.
//        /// </summary>
//        /// <param name="obj">The object to compare.</param>
//        /// <returns><see langword="true"/> if the current object is equal to <paramref name="obj"/>, otherwise <see langword="false"/>.</returns>
//        public override bool Equals(object obj) => obj is IValueObject _obj ? Equals(_obj) : obj is T __obj ? Value?.Equals(__obj) == true : obj is null ? !(Value is object) : false;

//        /// <summary>
//        /// Returns a hash code for the current object.
//        /// </summary>
//        /// <returns>A hash code for the current object.</returns>
//        public override int GetHashCode() => Value is object ? Value.GetHashCode() : base.GetHashCode();

//        public override string ToString() => Value?.ToString() ?? base.ToString();

//        #region IDisposable Support
//        private bool disposedValue = false;

//        /// <summary>
//        /// Removes the unmanaged resources and the managed resources if needed. If you override this method, you should call this implementation of this method in your override implementation to avoid unexpected results when using this object laater.
//        /// </summary>
//        /// <param name="disposing"><see langword="true"/> to dispose managed resources, otherwise <see langword="false"/>.</param>
//        protected virtual void Dispose(bool disposing)
//        {

//            if (disposedValue)

//                return;

//            if (Value is System.IDisposable _value)

//                _value.Dispose();

//            this.Parent = null;

//            disposedValue = true;

//        }

//        ~ReadOnlyTreeNode()
//        {

//            Dispose(false);

//        }

//        public void Dispose()
//        {

//            Dispose(true);

//            GC.SuppressFinalize(this);

//        }
//        #endregion

//    }

//    [Serializable]
//    [DebuggerDisplay("Value = {Value}, Count = {Count}")]
//    public class ReadOnlyTreeNode<TValue, TItems> : ReadOnlyTreeNode<TValue>, IReadOnlyTreeNode<TValue, TItems>, ICollection<ReadOnlyTreeNode<TItems>>, System.Collections.Generic.IList<ReadOnlyTreeNode<TItems>>, ICollection, System.Collections.IList, IReadOnlyCollection<ReadOnlyTreeNode<TItems>>, System.Collections.Generic.IReadOnlyList<ReadOnlyTreeNode<TItems>>, IReadOnlyCollection<TItems>, System.Collections.Generic.IReadOnlyList<TItems>
//    {

//        /// <summary>
//        /// Removes the unmanaged resources and the managed resources if needed. If you override this method, you should call this implementation of this method in your override implementation to avoid unexpected results when using this object laater.
//        /// </summary>
//        /// <param name="disposing"><see langword="true"/> to dispose managed resources, otherwise <see langword="false"/>.</param>
//        protected override void Dispose(bool disposing)
//        {

//            Items. Clear();

//            base.Dispose(disposing);

//        }

//        /// <summary>
//        /// Returns the default comparer for <see cref="ReadOnlyTreeNode{TValue, TItems}"/> objects.
//        /// </summary>
//        /// <returns>The default comparer for <see cref="ReadOnlyTreeNode{TValue, TItems}"/> objects.</returns>
//        protected virtual IEqualityComparer<ReadOnlyTreeNode<TItems>> GetDefaultTreeNodeItemsComparer() => new ValueObjectEqualityComparer<TItems>();

//        /// <summary>
//        /// Gets the inner <see cref="IList{T}"/> of this <see cref="ReadOnlyTreeNode{TValue, TItems}"/>.
//        /// </summary>
//        protected System.Collections.Generic.IList<ReadOnlyTreeNode<TItems>> Items { get; }

//        // protected virtual ITreeCollection<TItems> GetDefaultItemCollection() => new TreeCollection<TItems>(this);

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ReadOnlyTreeNode{TValue, TItems}"/> class using a custom value.
//        /// </summary>
//        /// <param name="value">The value of the new <see cref="ReadOnlyTreeNode{TValue, TItems}"/>.</param>
//        public ReadOnlyTreeNode(TValue value) : this(value, new List<ReadOnlyTreeNode<TItems>>()) { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ReadOnlyTreeNode{TValue, TItems}"/> class using a custom value and inner <see cref="IList{T}"/>.
//        /// </summary>
//        /// <param name="value">The value of the new <see cref="ReadOnlyTreeNode{TValue, TItems}"/>.</param>
//        /// <param name="items">A custom inner <see cref="IList{T}"/>.</param>
//        public ReadOnlyTreeNode(TValue value, System.Collections.Generic.IList<ReadOnlyTreeNode<TItems>> items) : base(value)
//        {
//            ThrowIfNull(items, nameof(items));

//            Items = items;
//        }

//        [NonSerialized]
//        private object _syncRoot;

//        object ICollection.SyncRoot => _syncRoot ?? (_syncRoot = Items is ICollection collection ? collection.SyncRoot : System.Threading.Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null));

//        bool ICollection.IsSynchronized => false;

//        /// <summary>
//        /// Gets the item at the specified index in this <see cref="ReadOnlyTreeNode{TValue, TItems}"/>.
//        /// </summary>
//        /// <param name="index">The index of the item.</param>
//        /// <returns>The item at the given index.</returns>
//        /// <exception cref="IndexOutOfRangeException">The given index is lesser than 0 or greater than <see cref="Count"/>.</exception>
//        public ReadOnlyTreeNode<TItems> this[int index] { get => Items[index]; set => throw new NotSupportedException("The current node is read-only."); }

//        TItems System.Collections.Generic.IReadOnlyList<TItems>.this[int index] => this[index].Value;

//        object System.Collections.IList.this[int index] { get => this[index]; set => throw new NotSupportedException("The current node is read-only."); }

//        /// <summary>
//        /// Gets the number of items that this <see cref="ReadOnlyTreeNode{TValue, TItems}"/> directly contains.
//        /// </summary>
//        public int Count => Items.Count;

//        /// <summary>
//        /// Gets a value that indicates whether this <see cref="ReadOnlyTreeNode{TValue, TItems}"/> is fixed-size.
//        /// </summary>
//        public bool IsFixedSize => Items is IList _items ? _items.IsFixedSize : false /*Items.IsReadOnly*/;

//        /// <summary>
//        /// Returns an <see cref="System.Collections.IEnumerator{T}"/> for this <see cref="ReadOnlyTreeNode{TValue, TItems}"/>.
//        /// </summary>
//        /// <returns><see cref="System.Collections.IEnumerator{T}"/> for this <see cref="ReadOnlyTreeNode{TValue, TItems}"/>.</returns>
//        public System.Collections.Generic.IEnumerator<ReadOnlyTreeNode<TItems>> GetEnumerator() => Items.GetEnumerator();

//        System.Collections.Generic.IEnumerator<TItems> IEnumerable<TItems>.GetEnumerator() => new ValueObjectEnumerator<TItems>(GetEnumerator());

//        System.Collections.Generic.IEnumerator<ITreeNode<TItems>> IEnumerable<ITreeNode<TItems>>.GetEnumerator() => GetEnumerator();

//        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//        void ICollection<ReadOnlyTreeNode<TItems>>.Add(ReadOnlyTreeNode<TItems> item) => throw new NotSupportedException("The current node is read-only.");

//        int System.Collections.IList.Add(object value) => throw new NotSupportedException("The current node is read-only.");

//        /// <summary>
//        /// Checks if this <see cref="ReadOnlyTreeNode{TValue, TItems}"/> directly contains a given <see cref="ReadOnlyTreeNode{T}"/>.
//        /// </summary>
//        /// <param name="item">The <see cref="ReadOnlyTreeNode{T}"/> to look for.</param>
//        /// <returns><see langword="true"/> if this <see cref="ReadOnlyTreeNode{TValue, TItems}"/> directly contains the given <see cref="ReadOnlyTreeNode{T}"/>, otherwise <see langword="false"/>.</returns>
//        public bool Contains(ReadOnlyTreeNode<TItems> item)

//        {

//            if (item is null)

//                return false;

//            IEqualityComparer<ReadOnlyTreeNode<TItems>> comp = GetDefaultTreeNodeItemsComparer();

//            foreach (ReadOnlyTreeNode<TItems> _item in this)

//                if (comp.Equals(_item, item))

//                    return true;

//            return false;

//        }

//        /// <summary>
//        /// Checks if this <see cref="ReadOnlyTreeNode{TValue, TItems}"/> directly contains a given item.
//        /// </summary>
//        /// <param name="item">The item to look for.</param>
//        /// <returns><see langword="true"/> if this <see cref="ReadOnlyTreeNode{TValue, TItems}"/> directly contains the given item, otherwise <see langword="false"/>.</returns>
//        public bool Contains(TItems item)

//        {

//            if (!(item is object))

//                return false;

//            EqualityComparer<TItems> comp = EqualityComparer<TItems>.Default;

//            foreach (ReadOnlyTreeNode<TItems> _item in this)

//                if (comp.Equals(_item.Value, item))

//                    return true;

//            return false;

//        }

//        bool System.Collections.IList.Contains(object value) => value is TItems item ? Contains(item) : value is ReadOnlyTreeNode<TItems> node ? Contains(node) : false;

//        /// <summary>
//        /// Returns the idnex of a given item in this <see cref="ReadOnlyTreeNode{TValue, TItems}"/>.
//        /// </summary>
//        /// <param name="item">The item for which to find the index.</param>
//        /// <returns>The index of <paramref name="item"/> if this <see cref="ReadOnlyTreeNode{TValue, TItems}"/> contains <paramref name="item"/>, otherwise -1.</returns>
//        public int IndexOf(ReadOnlyTreeNode<TItems> item)

//        {

//            if (item is null)

//                return -1;

//            IEqualityComparer<ReadOnlyTreeNode<TItems>> comp = GetDefaultTreeNodeItemsComparer();

//            for (int i = 0; i < Count; i++)

//                if (comp.Equals(this[i], item))

//                    return i;

//            return -1;

//        }

//        /// <summary>
//        /// Returns the idnex of a given item in this <see cref="ReadOnlyTreeNode{TValue, TItems}"/>.
//        /// </summary>
//        /// <param name="item">The item for which to find out the index.</param>
//        /// <returns>The index of <paramref name="item"/> if this <see cref="ReadOnlyTreeNode{TValue, TItems}"/> contains <paramref name="item"/>, otherwise -1.</returns>
//        public int IndexOf(TItems item)

//        {

//            if (!(item is object))

//                return -1;

//            EqualityComparer<TItems> comp = EqualityComparer<TItems>.Default;

//            for (int i = 0; i < Count; i++)

//                if (comp.Equals(this[i].Value, item))

//                    return i;

//            return -1;

//        }

//        int System.Collections.IList.IndexOf(object value) => value is TItems item ? IndexOf(item) : value is ReadOnlyTreeNode<TItems> node ? IndexOf(node) : -1;

//        void System.Collections.IList.RemoveAt(int index) => RemoveItem(index);

//        void System.Collections.Generic.IList<ReadOnlyTreeNode<TItems>>.RemoveAt(int index) => RemoveItem(index);

//        bool ICollection<ReadOnlyTreeNode<TItems>>.Remove(ReadOnlyTreeNode<TItems> item) => throw new NotSupportedException("The current node is read-only.");

//        void System.Collections.IList.Remove(object value) => throw new NotSupportedException("The current node is read-only.");

//        void System.Collections.Generic.IList<ReadOnlyTreeNode<TItems>>.Insert(int index, ReadOnlyTreeNode<TItems> item) => throw new NotSupportedException("The current node is read-only.");

//        void System.Collections.IList.Insert(int index, object value) => throw new NotSupportedException("The current node is read-only.");

//        /// <summary>
//        /// Performs a shallow copy of the items that the current <see cref="ReadOnlyTreeNode{TValue, TItems}"/> directly contains starting at a given index of a given array of <see cref="ReadOnlyTreeNode{T}"/>.
//        /// </summary>
//        /// <param name="array">The array in which to store the shallow copies of the items that <see cref="ReadOnlyTreeNode{TValue, TItems}"/> directly contains.</param>
//        /// <param name="arrayIndex">The index from which to store the items in <paramref name="array"/>.</param>
//        public void CopyTo(ReadOnlyTreeNode<TItems>[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

//        void ICollection.CopyTo(Array array, int arrayIndex)
//        {

//            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, Count, nameof(array), nameof(arrayIndex));

//            if (array is ReadOnlyTreeNode<TItems>[] _array)

//            {

//                CopyTo(_array, arrayIndex);

//                return;

//            }

//            //if (array is TItems[] itemsArray)

//            //{

//            //    foreach (var item in this)

//            //        itemsArray[arrayIndex++] = item.Value;

//            //    return;

//            //}

//            // todo: make better checks

//            try
//            {

//                foreach (ReadOnlyTreeNode<TItems> item in this)

//                    array.SetValue(item, arrayIndex++);

//            }

//            catch (ArrayTypeMismatchException)

//            {

//                try

//                {

//                    foreach (ReadOnlyTreeNode<TItems> item in this)

//                        array.SetValue(item.Value, arrayIndex++);

//                }

//                catch (ArrayTypeMismatchException)

//                {

//                    throw new ArgumentException("Invalid array type.");

//                }

//            }

//        }

//        void System.Collections.IList.Clear() => throw new NotSupportedException("The current tree node is read-only.");

//        void ICollection<ReadOnlyTreeNode<TItems>>.Clear() => throw new NotSupportedException("The current tree node is read-only.");

//        private void ThrowOnInvalidItem(ReadOnlyTreeNode<TItems> item)

//        {

//            if (item.Parent is object)

//                throw new InvalidOperationException("The given item already has a parent node.");

//            if (!item.IsReadOnly)

//                throw new ArgumentException("The given item is not read-only.");

//        }

//        /// <summary>
//        /// Inserts a given item at a specified index in this <see cref="ReadOnlyTreeNode{TValue, TItems}"/>. Because it is impossible to have a read-only wrapper for <see cref="ITreeNode"/>, like collections read-only wrappers, this class is the same as the <see cref="ReadOnlyTreeNode{TValue, TItems}"/> class, but without the mutable methods. So, if you want to insert new items in <see cref="Items"/>, you should use this method in order to insert new items properly.
//        /// </summary>
//        /// <param name="index">The index of the new item.</param>
//        /// <param name="item">The item to insert in this <see cref="ReadOnlyTreeNode{TValue, TItems}"/>.</param>
//        protected virtual void InsertItem(int index, ReadOnlyTreeNode<TItems> item)
//        {

//            ThrowOnInvalidItem(item);

//            item.Parent = this;

//            if (index == Count)

//                Items.Add(item);

//            else

//                Items.Insert(index, item);

//        }

//        /// <summary>
//        /// Removes all items of this <see cref="ReadOnlyTreeNode{TValue, TItems}"/>. Because it is impossible to have read-only wrappers for <see cref="ITreeNode"/>, like collections read-only wrappers, this class is the same as the <see cref="ReadOnlyTreeNode{TValue, TItems}"/> class, but without the mutable methods. So, if you want to clear all items in <see cref="Items"/>, you should use this method in order to remove the items properly.
//        /// </summary>
//        protected virtual void ClearItems()
//        {
//            foreach (ReadOnlyTreeNode<TItems> item in this)

//                item.Parent = null;

//            Items.Clear();
//        }

//        /// <summary>
//        /// Removes the item at a given index. Because it is impossible to have read-only wrappers for <see cref="ITreeNode"/>, like collections read-only wrappers, this class is the same as the <see cref="ReadOnlyTreeNode{TValue, TItems}"/> class, but without the mutable methods. So, if you want to remove items from <see cref="Items"/>, you should use this method in order to remove the items properly.
//        /// </summary>
//        /// <param name="index">The index from which to remove the item.</param>
//        protected virtual void RemoveItem(int index)
//        {
//            this[index].Parent = null;

//            Items.RemoveAt(index);
//        }

//        /// <summary>
//        /// Sets a given item at a specified index of this <see cref="ReadOnlyTreeNode{TValue, TItems}"/>. This method sets <paramref name="item"/> directly in the current <see cref="ReadOnlyTreeNode{TValue, TItems}"/>. Because it is impossible to have read-only wrappers for <see cref="ITreeNode"/>, like collections read-only wrappers, this class is the same as the <see cref="ReadOnlyTreeNode{TValue, TItems}"/> class, but without the mutable methods. So, if you want to alter existing items in <see cref="Items"/>, you should use this method in order to alter the items properly.
//        /// </summary>
//        /// <param name="index">The index at which to set <paramref name="item"/>.</param>
//        /// <param name="item">The item to update with.</param>
//        protected virtual void SetItem(int index, ReadOnlyTreeNode<TItems> item)
//        {
//            ThrowOnInvalidItem(item);

//            this[index].Parent = null;

//            item.Parent = this;

//            Items[index] = item;
//        }

//    }
//}
