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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using static WinCopies.Util.Util;

namespace WinCopies.Collections
{
    namespace DotNetFix
    {
        public class NotifyCollectionChangedEventArgs : System.Collections.Specialized.NotifyCollectionChangedEventArgs
        {
            public bool IsChangingEvent { get; } = false;

            public IList ResetItems { get; }

            public NotifyCollectionChangedEventArgs(IList resetItems) : base(NotifyCollectionChangedAction.Reset)

            {
                IsChangingEvent = true;

                ResetItems = resetItems;
            }

            public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem) : base(action, changedItem) => IsChangingEvent = isChangingEvent;

            public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems) : base(action, changedItems) => IsChangingEvent = isChangingEvent;

            public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem, int index) : base(action, changedItem, index) => IsChangingEvent = isChangingEvent;

            public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems, int startingIndex) : base(action, changedItems, startingIndex) => IsChangingEvent = isChangingEvent;

            public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object newItem, object oldItem) : base(action, newItem, oldItem) => IsChangingEvent = isChangingEvent;

            public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList newItems, IList oldItems) : base(action, newItems, oldItems) => IsChangingEvent = isChangingEvent;

            public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object newItem, object oldItem, int index) : base(action, newItem, oldItem, index) => IsChangingEvent = isChangingEvent;

            public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex) : base(action, newItems, oldItems, startingIndex) => IsChangingEvent = isChangingEvent;

            public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex) : base(action, changedItem, index, oldIndex) => IsChangingEvent = isChangingEvent;

            public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex) : base(action, changedItems, index, oldIndex) => IsChangingEvent = isChangingEvent;
        }

        /// <summary>
        /// Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <remarks>
        /// <para>In many cases the data that you work with is a collection of objects. For example, a common scenario in data binding is to use an <see cref="ItemsControl"/> such as a <see cref="ListBox"/>, <see cref="ListView"/>, or <see cref="TreeView"/> to display a collection of records.</para>
        /// <para>You can enumerate over any collection that implements the <see cref="IEnumerable"/> interface. However, to set up dynamic bindings so that insertions or deletions in the collection update the UI automatically, the collection must implement the <see cref="INotifyCollectionChanged"/> interface. This interface exposes the <see cref="System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged"/> event, an event that should be raised whenever the underlying collection changes.</para>
        /// <para>WPF provides the <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> class, which is a built-in implementation of a data collection that implements the <see cref="INotifyCollectionChanged"/> interface.</para>
        /// <para>Before implementing your own collection, consider using <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> or one of the existing collection classes, such as <see cref="List{T}"/>, <see cref="Collection{T}"/>, and <see cref="BindingList{T}"/>, among many others. If you have an advanced scenario and want to implement your own collection, consider using <see cref="IList"/>, which provides a non-generic collection of objects that can be individually accessed by index. Implementing <see cref="IList"/> provides the best performance with the data binding engine.</para>
        /// <para>Notes: To fully support transferring data values from binding source objects to binding targets, each object in your collection that supports bindable properties must implement an appropriate property changed notification mechanism such as the <see cref="INotifyPropertyChanged"/> interface.</para>
        /// <para>For more information, see "Binding to Collections" in the Data Binding Overview article at: https://docs.microsoft.com/en-us/dotnet/framework/wpf/data/data-binding-overview?view=netframework-4.8</para>
        /// <para>For notes on XAML usage, see the following article: https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8</para>
        /// </remarks>
        [Serializable]
        public class ObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>, IObservableCollection<T>, INotifyCollectionChanging
        {
            public virtual event NotifyCollectionChangingEventHandler CollectionChanging;

            /// <summary>
            /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class.
            /// </summary>
            public ObservableCollection() : base() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class that contains elements copied from the specified list.
            /// </summary>
            /// <param name="list">The list from which the elements are copied.</param>
            /// <remarks>The elements are copied onto the <see cref="ObservableCollection{T}"/> in the same order they are read by the enumerator of the list.</remarks>
            /// <exception cref="ArgumentNullException">The <paramref name="list"/> parameter cannot be <see langword="null"/>.</exception>
            public ObservableCollection(List<T> list) : base(list) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class that contains elements copied from the specified collection.
            /// </summary>
            /// <param name="collection">The collection from which the elements are copied.</param>
            /// <remarks>The elements are copied onto the <see cref="ObservableCollection{T}"/> in the same order they are read by the enumerator of the collection.</remarks>
            /// <exception cref="ArgumentNullException">The <paramref name="collection"/> parameter cannot be <see langword="null"/>.</exception>
            public ObservableCollection(IEnumerable<T> collection) : base(collection) { }

            /// <summary>
            /// Inserts an item into the collection at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
            /// <param name="item">The object to insert.</param>
            /// <remarks><para>The base class calls this method when an item is added to the collection. This implementation raises the <see cref="INotifyCollectionChanging.CollectionChanging"/> and the <see cref="System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged"/> events.</para>
            /// <para>For more information, see the <see cref="Collection{T}.InsertItem(int, T)"/> method of the <see cref="Collection{T}"/> base class.</para></remarks>
            protected override void InsertItem(int index, T item)
            {
                CheckReentrancy();

                OnCollectionChanging(new NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Add, item, index));

                base.InsertItem(index, item);
            }

            protected override void MoveItem(int oldIndex, int newIndex)
            {
                CheckReentrancy();

                OnCollectionChanging(new NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Move, this[oldIndex], oldIndex, newIndex));

                base.MoveItem(oldIndex, newIndex);
            }

            protected override void SetItem(int index, T item)
            {
                CheckReentrancy();

                OnCollectionChanging(new NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Replace, this[index], item));

                base.SetItem(index, item);
            }

            protected override void RemoveItem(int index)
            {
                CheckReentrancy();

                OnCollectionChanging(new NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Remove, this[index], index));

                base.RemoveItem(index);
            }

            /// <summary>
            /// Removes all items from the collection.
            /// </summary>
            /// <remarks><para>The base class calls this method when the list is being cleared. This implementation raises the <see cref="INotifyCollectionChanging.CollectionChanging"/> and the <see cref="System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged"/> events.</para>
            /// <para>For more information, see the <see cref="Collection{T}.ClearItems"/> method of the <see cref="Collection{T}"/> base class.</para></remarks>
            protected override void ClearItems()
            {
                CheckReentrancy();

                OnCollectionChanging(new NotifyCollectionChangedEventArgs(new ReadOnlyCollection<T>(this.ToList())));

                base.ClearItems();
            }

            protected virtual void OnCollectionChanging(DotNetFix.NotifyCollectionChangedEventArgs e)
            {
                if (!e.IsChangingEvent) throw new ArgumentException($"'{nameof(e)}' must have the IsChangingProperty set to true.");

                if (CollectionChanging != null)

                    using (BlockReentrancy())

                        CollectionChanging(this, e);
            }
        }
    }

    [Obsolete("This class has been moved to the WinCopies.Collections.DotNetFix namespace. This implementation is still here temporarily only.")]
    public class NotifyCollectionChangedEventArgs : System.Collections.Specialized.NotifyCollectionChangedEventArgs
    {
        public bool IsChangingEvent { get; } = false;

        public IList ResetItems { get; }

        public NotifyCollectionChangedEventArgs(IList resetItems) : base(NotifyCollectionChangedAction.Reset)
        {
            IsChangingEvent = true;

            ResetItems = resetItems;
        }

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem) : base(action, changedItem) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems) : base(action, changedItems) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem, int index) : base(action, changedItem, index) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems, int startingIndex) : base(action, changedItems, startingIndex) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object newItem, object oldItem) : base(action, newItem, oldItem) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList newItems, IList oldItems) : base(action, newItems, oldItems) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object newItem, object oldItem, int index) : base(action, newItem, oldItem, index) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex) : base(action, newItems, oldItems, startingIndex) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex) : base(action, changedItem, index, oldIndex) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex) : base(action, changedItems, index, oldIndex) => IsChangingEvent = isChangingEvent;
    }

    [Obsolete("This class has been moved to the WinCopies.Collections.DotNetFix namespace. This implementation is still here temporarily only.")]
    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <remarks>
    /// <para>In many cases the data that you work with is a collection of objects. For example, a common scenario in data binding is to use an <see cref="ItemsControl"/> such as a <see cref="ListBox"/>, <see cref="ListView"/>, or <see cref="TreeView"/> to display a collection of records.</para>
    /// <para>You can enumerate over any collection that implements the <see cref="IEnumerable"/> interface. However, to set up dynamic bindings so that insertions or deletions in the collection update the UI automatically, the collection must implement the <see cref="INotifyCollectionChanged"/> interface. This interface exposes the <see cref="System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged"/> event, an event that should be raised whenever the underlying collection changes.</para>
    /// <para>WPF provides the <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> class, which is a built-in implementation of a data collection that implements the <see cref="INotifyCollectionChanged"/> interface.</para>
    /// <para>Before implementing your own collection, consider using <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> or one of the existing collection classes, such as <see cref="List{T}"/>, <see cref="Collection{T}"/>, and <see cref="BindingList{T}"/>, among many others. If you have an advanced scenario and want to implement your own collection, consider using <see cref="IList"/>, which provides a non-generic collection of objects that can be individually accessed by index. Implementing <see cref="IList"/> provides the best performance with the data binding engine.</para>
    /// <para>Notes: To fully support transferring data values from binding source objects to binding targets, each object in your collection that supports bindable properties must implement an appropriate property changed notification mechanism such as the <see cref="INotifyPropertyChanged"/> interface.</para>
    /// <para>For more information, see "Binding to Collections" in the Data Binding Overview article at: https://docs.microsoft.com/en-us/dotnet/framework/wpf/data/data-binding-overview?view=netframework-4.8</para>
    /// <para>For notes on XAML usage, see the following article: https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=netframework-4.8</para>
    /// </remarks>
    [Serializable]
    public class ObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>, IObservableCollection<T>, INotifyCollectionChanging
    {
        public virtual event NotifyCollectionChangingEventHandler CollectionChanging;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class.
        /// </summary>
        public ObservableCollection() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class that contains elements copied from the specified list.
        /// </summary>
        /// <param name="list">The list from which the elements are copied.</param>
        /// <remarks>The elements are copied onto the <see cref="ObservableCollection{T}"/> in the same order they are read by the enumerator of the list.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="list"/> parameter cannot be <see langword="null"/>.</exception>
        public ObservableCollection(List<T> list) : base(list) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <remarks>The elements are copied onto the <see cref="ObservableCollection{T}"/> in the same order they are read by the enumerator of the collection.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="collection"/> parameter cannot be <see langword="null"/>.</exception>
        public ObservableCollection(IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        /// <remarks><para>The base class calls this method when an item is added to the collection. This implementation raises the <see cref="INotifyCollectionChanging.CollectionChanging"/> and the <see cref="System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged"/> events.</para>
        /// <para>For more information, see the <see cref="Collection{T}.InsertItem(int, T)"/> method of the <see cref="Collection{T}"/> base class.</para></remarks>
        protected override void InsertItem(int index, T item)
        {
            CheckReentrancy();

            OnCollectionChanging(new DotNetFix.NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Add, item, index));

            base.InsertItem(index, item);
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            CheckReentrancy();

            OnCollectionChanging(new DotNetFix.NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Move, this[oldIndex], oldIndex, newIndex));

            base.MoveItem(oldIndex, newIndex);
        }

        protected override void SetItem(int index, T item)
        {
            CheckReentrancy();

            OnCollectionChanging(new DotNetFix.NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Replace, this[index], item));

            base.SetItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            CheckReentrancy();

            OnCollectionChanging(new DotNetFix.NotifyCollectionChangedEventArgs(true, NotifyCollectionChangedAction.Remove, this[index], index));

            base.RemoveItem(index);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        /// <remarks><para>The base class calls this method when the list is being cleared. This implementation raises the <see cref="INotifyCollectionChanging.CollectionChanging"/> and the <see cref="System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged"/> events.</para>
        /// <para>For more information, see the <see cref="Collection{T}.ClearItems"/> method of the <see cref="Collection{T}"/> base class.</para></remarks>
        protected override void ClearItems()
        {
            CheckReentrancy();

            OnCollectionChanging(new DotNetFix.NotifyCollectionChangedEventArgs(new ReadOnlyCollection<T>(this.ToList())));

            base.ClearItems();
        }

        protected virtual void OnCollectionChanging(DotNetFix.NotifyCollectionChangedEventArgs e)
        {
            if (!e.IsChangingEvent) throw new ArgumentException($"'{nameof(e)}' must have the IsChangingProperty set to true.");

            if (CollectionChanging != null)

                using (BlockReentrancy())

                    CollectionChanging(this, e);
        }
    }

    public delegate void NotifyCollectionChangingEventHandler(object sender, DotNetFix.NotifyCollectionChangedEventArgs e);

    public interface IObservableCollection<T> : System.Collections.Generic.IList<T>, IEnumerable<T>, IEnumerable, IList, ICollection, System.Collections.Generic.IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        void Move(int oldIndex, int newIndex);
    }

    //namespace Advanced

    //{

    //    //internal class ObservableCollectionInternal<T> : WinCopies.Collections.ObservableCollection<T>

    //    //{

    //    //    public ObservableCollectionInternal() : base() { }

    //    //    public ObservableCollectionInternal(List<T> list) : base(list) { }

    //    //    public ObservableCollectionInternal(IEnumerable<T> collection) : base(collection) { }

    //    //    internal new IList<T> Items => base.Items;

    //    //    internal new void CheckReentrancy() => base.CheckReentrancy();

    //    //    internal new IDisposable BlockReentrancy() => base.BlockReentrancy();

    //    //}

    //    [Serializable]
    //    public class ObservableCollection<T> : WinCopies.Collections.ObservableCollection<T>, IObservableCollection<T>

    //    {

    //        private ObservableCollectionInternal<T> items;

    //        private void SetItems(ObservableCollectionInternal<T> itemCollection)
    //        {

    //            items = itemCollection;

    //            items.CollectionChanging += (object sender, DotNetFix.NotifyCollectionChangedEventArgs e) => OnCollectionChanging(e);

    //            items.CollectionChanged += (object sender, System.Collections.Specialized.DotNetFix.NotifyCollectionChangedEventArgs e) => OnCollectionChanged(e);

    //            ((INotifyPropertyChanged)items).PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);

    //        }

    //        protected IList<T> Items => items.Items;

    //        // private Func<T, TModifier> getModifier;

    //        //public ObservableCollection(Func<T, TModifier> getModifier)
    //        //{

    //        //    items = new ObservableCollection<TModifier>();

    //        //    this.getModifier = getModifier;

    //        //}

    //        //public ObservableCollection(List<T> list, Func<T, TModifier> getModifier) : this((IEnumerable<T>)list, getModifier) { }

    //        //public ObservableCollection(IEnumerable<T> collection, Func<T, TModifier> getModifier)

    //        //{

    //        //    this.getModifier = getModifier;

    //        //    var builder = new ArrayAndListBuilder<TModifier>();

    //        //    foreach (var item in collection)

    //        //        builder.AddLast(getModifier(item));

    //        //    // todo: to add collections that can be built (see arrayandlistbuilder in WinCopies.Util

    //        //    items = new ObservableCollection<TModifier>(builder);

    //        //}

    //        public ObservableCollection() => SetItems(new ObservableCollectionInternal<T>());

    //        public ObservableCollection(List<T> list) => SetItems(new ObservableCollectionInternal<T>(list));

    //        public ObservableCollection(IEnumerable<T> collection) => SetItems(new ObservableCollectionInternal<T>(collection));

    //        T System.Collections.Generic.IList<T>.this[int index] { get => GetItem(index); set => Update(index, value); }

    //        object IList.this[int index] { get => this[index]; set => Update(index, GetOrThrowIfNotType<T>(value, nameof(value))); }

    //        public T this[int index] => GetItem(index);

    //        T System.Collections.Generic.IReadOnlyList<T>.this[int index] => this[index];

    //        public bool Update(int index, T item) => SetItem(index, item);

    //        public int Count => ItemsCount;

    //        protected virtual int ItemsCount => items.Count;

    //        protected virtual bool IsReadOnly => false;

    //        bool IList<T>.IsReadOnly => IsReadOnly;

    //        bool IList.IsReadOnly => IsReadOnly;

    //        protected virtual bool IsFixedSize => false;

    //        bool IList.IsFixedSize => IsFixedSize;

    //        object ICollection.SyncRoot => ((ICollection)items).SyncRoot;

    //        bool ICollection.IsSynchronized => ((ICollection)items).IsSynchronized;

    //        public event NotifyCollectionChangedEventHandler CollectionChanged;

    //        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged { add => PropertyChanged += value; remove => PropertyChanged -= value; }

    //        protected event PropertyChangedEventHandler PropertyChanged;

    //        public event NotifyCollectionChangingEventHandler CollectionChanging;

    //        protected IDisposable BlockReentrancy() => items.BlockReentrancy();

    //        protected void CheckReentrancy() => items.CheckReentrancy();

    //        public bool Add(T item) => InsertItem(Count, item);

    //        void IList<T>.Add(T item) => Add(item);

    //        int IList.Add(object value) => InsertItem(Count, GetOrThrowIfNotType<T>(value, nameof(value))) ? Count - 1 : -1;

    //        public bool Clear() => ClearItems();

    //        void IList.Clear() => ClearItems();

    //        void IList<T>.Clear() => ClearItems();

    //        public bool Contains(T item) => ContainsItem(item);

    //        protected virtual bool ContainsItem(T item)

    //        {

    //            if (item?.Equals(null) == true) return false;

    //            foreach (T _item in items)

    //                if (_item.Equals(item))

    //                    return true;

    //            return false;

    //        }

    //        public bool Contains(object value) => value is T item ? Contains(item) : false;

    //        public virtual void CopyTo(T[] array, int arrayIndex) =>

    //            //T[] items = new T[Count];

    //            //for (int i = 0; i < this.items.Count; i++)

    //            //    items[i] = this.items[i].Item;

    //            items.CopyTo(array, arrayIndex);

    //        void ICollection.CopyTo(Array array, int index) => CopyTo(array, index);

    //        protected virtual void CopyTo(Array array, int index) => ((IList)items).CopyTo(array, index);

    //        public virtual IEnumerator<T> GetEnumerator() => items.GetEnumerator();

    //        public int IndexOf(T item) => IndexOfItem(item);

    //        protected virtual int IndexOfItem(T item) => items.IndexOf(item);

    //        int IList.IndexOf(object value) => value is T browsableObjectInfo ? IndexOf(browsableObjectInfo) : -1;

    //        void System.Collections.Generic.IList<T>.Insert(int index, T item) => InsertItem(index, item);

    //        /// <summary>
    //        /// Inserts an element into the <see cref="ObservableCollection{T}"/> at the specified index. See the Remarks section.
    //        /// </summary>
    //        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
    //        /// <param name="item">The object to insert. The value can be <see langword="null"/> for reference types.</param>
    //        /// <returns><see langword="true"/> if the new item was successfully inserted in the colelction, otherwise <see langword="false"/>.</returns>
    //        /// <remarks>If the item could not be inserted because of an index error, an <see cref="ArgumentOutOfRangeException"/> is thrown. On its default implementation, this method always inserts the new item and returns <see langword="true"/> if the given index is in the required value range.</remarks>
    //        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.
    //        /// OR <paramref name="index"/> is greater than <see cref="Count"/>.</exception>
    //        public bool Insert(int index, T item) => InsertItem(index, item);

    //        void IList.Insert(int index, object value) => InsertItem(index, GetOrThrowIfNotType<T>(value, nameof(value)));

    //        void IObservableCollection<T>.Move(int oldIndex, int newIndex) => MoveItem(oldIndex, newIndex);

    //        /// <summary>
    //        /// Moves the item at the specified index to a new location in the collection. See the Remarks section.
    //        /// </summary>
    //        /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
    //        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
    //        /// <returns><see langword="true"/> if the new item was successfully inserted in the colelction, otherwise <see langword="false"/>.</returns>
    //        /// <remarks><para>If the item could not be inserted because of an index error, an <see cref="ArgumentOutOfRangeException"/> is thrown. On its default implementation, this method always inserts the new item and returns <see langword="true"/> if the given index is in the required value range.</para>
    //        /// <para>Subclasses can override the <see cref="MoveItem"/> method to provide custom behavior for this method.</para></remarks>
    //        /// <exception cref="ArgumentOutOfRangeException">At least <paramref name="oldIndex"/> or <paramref name="newIndex"/> is less than zero.
    //        /// OR At least <paramref name="oldIndex"/> or <paramref name="newIndex"/> is greater than <see cref="Count"/>.</exception>
    //        public bool Move(int oldIndex, int newIndex) => MoveItem(oldIndex, newIndex);

    //        public bool Move(T item, int newIndex) => TryAlter(item, i => MoveItem(i, newIndex));

    //        public bool Remove(T item) => TryAlter((object)item, i => RemoveItem(i));

    //        void IList.Remove(object value) => TryAlter(value, i => RemoveItem(i));

    //        private bool TryAlter(object value, Func<int, bool> func)
    //        {

    //            if (value is T _value)

    //            {

    //                int i = IndexOf(_value);

    //                return i == -1 ? false : func(i);

    //            }

    //            return false;

    //        }

    //        public bool RemoveAt(int index) => RemoveItem(index);

    //        void IList.RemoveAt(int index) => RemoveItem(index);

    //        void System.Collections.Generic.IList<T>.RemoveAt(int index) => RemoveItem(index);

    //        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    //        protected virtual bool ClearItems()
    //        {

    //            items.Clear();

    //            return true;

    //        }

    //        protected virtual bool InsertItem(int index, T item)
    //        {

    //            items.Insert(index, item);

    //            return true;

    //        }

    //        protected virtual bool MoveItem(int oldIndex, int newIndex)
    //        {

    //            items.Move(oldIndex, newIndex);

    //            return true;

    //        }

    //        protected virtual void OnCollectionChanging(DotNetFix.NotifyCollectionChangedEventArgs e) => CollectionChanging?.Invoke(this, e);

    //        protected virtual void OnCollectionChanged(System.Collections.Specialized.DotNetFix.NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);

    //        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

    //        protected virtual bool RemoveItem(int index)

    //        {

    //            items.RemoveAt(index);

    //            return true;

    //        }

    //        protected virtual T GetItem(int index) => items[index];

    //        protected virtual bool SetItem(int index, T item)
    //        {

    //            items[index] = item;

    //            return true;

    //        }
    //    }

    //}
}
