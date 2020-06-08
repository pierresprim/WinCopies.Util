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

//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WinCopies.Collections
//{
    ///// <summary>
    ///// Represents an observable <see cref="TreeNode{TValue, TItems}"/>.
    ///// </summary>
    ///// <typeparam name="TValue">The value type.</typeparam>
    ///// <typeparam name="TItems">The items type.</typeparam>
    //[Serializable]
    //[DebuggerDisplay("Value = {Value}, Count = {Count}")]
    //public class ObservableTreeNode<TValue, TItems> : TreeNode<TValue, TItems>, IObservableTreeNode<TValue, TItems>
    //{

    //    [Serializable]
    //    private class Monitor : IDisposable
    //    {

    //        private int _busyCount;

    //        public bool IsBusy => _busyCount > 0;

    //        public void Enter() => _busyCount++;

    //        public void Dispose() => _busyCount--;

    //    }

    //    private readonly Monitor _monitor = new Monitor();

    //    #region Constructors

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="ObservableTreeNode{TValue, TItems}"/> class.
    //    /// </summary>
    //    public ObservableTreeNode() : base() { }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="ObservableTreeNode{TValue, TItems}"/> class and copy the items of <paramref name="items"/>.
    //    /// </summary>
    //    /// <param name="items">Items to copy to the new tree node.</param>
    //    public ObservableTreeNode(List<TreeNode<TItems>> items) : this(collection: items) { }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="ObservableTreeNode{TValue, TItems}"/> class and copy the items of <paramref name="collection"/>.
    //    /// </summary>
    //    /// <param name="collection">Items to copy to the new tree node.</param>
    //    public ObservableTreeNode(IEnumerable<TreeNode<TItems>> collection) : base() => CopyItems(collection);

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="ObservableTreeNode{TValue, TItems}"/> class with a custom value.
    //    /// </summary>
    //    public ObservableTreeNode(TValue value) : base(value: value) { }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="ObservableTreeNode{TValue, TItems}"/> class with a custom value and copy the items of <paramref name="items"/>.
    //    /// </summary>
    //    /// <param name="value">The value of the new tree node.</param>
    //    /// <param name="items">Items to copy to the new tree node.</param>
    //    public ObservableTreeNode(TValue value, List<TreeNode<TItems>> items) : this(value, collection: items) { }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="ObservableTreeNode{TValue, TItems}"/> class with a custom value and copy the items of <paramref name="collection"/>.
    //    /// </summary>
    //    /// <param name="value">The valeu of the new tree node.</param>
    //    /// <param name="collection">Items to copy to the new tree node.</param>
    //    public ObservableTreeNode(TValue value, IEnumerable<TreeNode<TItems>> collection) : base(value: value) => CopyItems(collection);

    //    private void CopyItems(IEnumerable<TreeNode<TItems>> collection)

    //    {

    //        foreach (TreeNode<TItems> node in collection)

    //            Items.Add(node);

    //    }

    //    #endregion

    //    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged { add => PropertyChanged += value; remove => PropertyChanged -= value; }

    //    protected virtual event PropertyChangedEventHandler PropertyChanged;

    //    public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

    //    public void Move(int oldIndex, int newIndex) => MoveItem(oldIndex, newIndex);

    //    #region Overrides

    //    public override ITreeNode Parent
    //    {
    //        get => base.Parent; protected internal set
    //        {
    //            base.Parent = value;

    //            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Parent)));
    //        }
    //    }

    //    protected override void SetValue(TValue newValue)
    //    {

    //        base.SetValue(newValue);

    //        OnPropertyChanged(new PropertyChangedEventArgs(nameof(Value)));

    //    }

    //    protected override void ClearItems()
    //    {

    //        CheckReentrancy();

    //        base.ClearItems();

    //        OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

    //    }

    //    protected override void RemoveItem(int index)
    //    {

    //        CheckReentrancy();

    //        TreeNode<TItems> removedItem = this[index];

    //        base.RemoveItem(index);

    //        OnCountChanged();

    //        OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index));

    //    }

    //    protected override void InsertItem(int index, TreeNode<TItems> item)
    //    {

    //        CheckReentrancy();

    //        base.InsertItem(index, item);

    //        OnCountChanged();

    //        OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));

    //    }

    //    protected override void SetItem(int index, TreeNode<TItems> item)
    //    {

    //        CheckReentrancy();

    //        TreeNode<TItems> originalItem = this[index];

    //        base.SetItem(index, item);

    //        OnIndexerChanged();

    //        OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, originalItem, item, index));

    //    }

    //    protected virtual void MoveItem(int oldIndex, int newIndex)

    //    {

    //        CheckReentrancy();

    //        TreeNode<TItems> removedItem = this[oldIndex];

    //        base.RemoveItem(oldIndex);

    //        base.InsertItem(newIndex, removedItem);

    //        OnIndexerChanged();

    //        OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex));

    //    }

    //    #endregion

    //    #region Protected Methods

    //    /// <summary>
    //    /// Calls <see cref="OnPropertyChanged(PropertyChangedEventArgs)"/> for the <see cref="TreeNode{TValue, TItems}.Count"/> property, then for the indexer property.
    //    /// </summary>
    //    protected virtual void OnCountChanged()

    //    {

    //        OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

    //        OnIndexerChanged();

    //    }

    //    protected virtual void OnIndexerChanged() =>

    //        OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));

    //    /// <summary>
    //    /// Raises the <see cref="PropertyChanged"/> event, if not null, for the given <see cref="PropertyChangedEventArgs"/>.
    //    /// </summary>
    //    /// <param name="e">The <see cref="PropertyChangedEventArgs"/> of the event.</param>
    //    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

    //    protected virtual void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)

    //    {

    //        if (CollectionChanged != null)

    //            using (BlockReentrancy())

    //                CollectionChanged(this, e);

    //    }

    //    protected void CheckReentrancy()
    //    {

    //        if (_monitor.IsBusy && CollectionChanged?.GetInvocationList().Length > 1)

    //            throw new InvalidOperationException("Reentrancy not allowed.");

    //    }

    //    protected IDisposable BlockReentrancy()
    //    {

    //        _monitor.Enter();

    //        return _monitor;

    //    }

    //    #endregion
    //}
//}
