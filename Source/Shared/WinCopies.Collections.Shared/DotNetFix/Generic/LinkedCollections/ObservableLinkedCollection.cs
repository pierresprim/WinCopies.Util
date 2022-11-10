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

#if CS7
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WinCopies.Collections.DotNetFix.Generic
{
    [Serializable]
    public class ObservableLinkedCollection<T> : LinkedCollection<T>, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>
    {
        private ILinkedListNode<T> _firstNode;
        private ILinkedListNode<T> _lastNode;

        public event PropertyChangedEventHandler PropertyChanged;
        public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ObservableLinkedCollection() : base() { /* Left empty. */ }
        public ObservableLinkedCollection(ILinkedList3<T> list) : base(list)
        {
            IReadOnlyLinkedListBase<T, ILinkedListNode<T>> getList() => list;

            _firstNode = getList().First;
            _lastNode = getList().Last;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

        protected virtual void OnCollectionChanged(LinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in ILinkedListNode<T> addedBefore, in ILinkedListNode<T> addedAfter, in ILinkedListNode<T> node)
        {
            if (First != _firstNode)
            {
                _firstNode = First;

                RaisePropertyChangedEvent(nameof(First));
                RaisePropertyChangedEvent(nameof(FirstValue));
            }

            else if (Last != _lastNode)
            {
                _lastNode = Last;

                RaisePropertyChangedEvent(nameof(Last));
                RaisePropertyChangedEvent(nameof(LastValue));
            }

            OnCollectionChanged(new LinkedCollectionChangedEventArgs<T>(action, addedBefore, addedAfter, node));
        }

        protected override ILinkedListNode<T> AddFirstItem(T value)
        {
            ILinkedListNode<T> result = base.AddFirstItem(value);

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

        protected override ILinkedListNode<T> AddItemAfter(ILinkedListNode<T> node, T value)
        {
            ILinkedListNode<T> result = base.AddItemAfter(node, value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, result);

            return result;
        }

        protected override ILinkedListNode<T> AddItemBefore(ILinkedListNode<T> node, T value)
        {
            ILinkedListNode<T> result = base.AddItemBefore(node, value);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, result);

            return result;
        }

        protected override ILinkedListNode<T> AddLastItem(T value)
        {
            ILinkedListNode<T> result = base.AddLastItem(value);

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

        protected override void OnNodeRemoved(ILinkedListNode<T> node)
        {
            base.OnNodeRemoved(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
        }
    }
}
#endif
