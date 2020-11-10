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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace WinCopies.Collections
{
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
    [Obsolete("This class has been moved to the WinCopies.Collections.DotNetFix namespace. This implementation is still here temporarily only.")]
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
        public ObservableCollection(System.Collections.Generic.IEnumerable<T> collection) : base(collection) { }

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

            OnCollectionChanging(new DotNetFix.NotifyCollectionChangedEventArgs(new ReadOnlyCollection<T>(this)));

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

#endif
