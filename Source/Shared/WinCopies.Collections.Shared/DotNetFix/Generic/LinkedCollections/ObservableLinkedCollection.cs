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

#if !WinCopies3
using System.Runtime.Serialization;

using WinCopies.Util;

using static WinCopies.Util.Util;
#endif

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    namespace Generic
    {
#endif
        [Serializable]
        public class ObservableLinkedCollection<T> : LinkedCollection<T>, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>
#if WinCopies3
, ILinkedList3<T>
#endif
        {
            private
#if WinCopies3
            ILinkedListNode
#else
            LinkedListNode
#endif
            <T> _firstNode;
        private
#if WinCopies3
            ILinkedListNode
#else
            LinkedListNode
#endif
            <T> _lastNode;

            public event PropertyChangedEventHandler PropertyChanged;

            public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

            public ObservableLinkedCollection() : base() { /* Left empty. */ }

            public ObservableLinkedCollection(in
#if WinCopies3
                ILinkedList3
#else
                LinkedList
#endif
                <T> list) : base(list)
            {
                _firstNode = list.First;

                _lastNode = list.Last;
            }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

            protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

            protected virtual void OnCollectionChanged(LinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

            protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> addedBefore, in
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> addedAfter, in
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node)
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

            protected override
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddFirstItem(T value)
            {
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> result = base.AddFirstItem(value);

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

            protected override
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddItemAfter(
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value)
            {

#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> result = base.AddItemAfter(node, value);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, result);

                return result;
            }

            protected override
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddItemBefore(
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value)
            {

#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> result = base.AddItemBefore(node, value);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, result);

                return result;
            }

            protected override
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddLastItem(T value)
            {
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> result = base.AddLastItem(value);

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

#if WinCopies3
            protected override void OnNodeRemoved(ILinkedListNode<T> node)
            {
                base.OnNodeRemoved(node);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
            }
#else
            protected override void AddFirstItem(System.Collections.Generic.LinkedListNode<T> node)
        {
            base.AddFirstItem(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddFirst, null, null, node);
        }

        protected override void AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode)
        {
            base.AddItemAfter(node, newNode);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddAfter, null, node, newNode);
        }

        protected override void AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode)
        {
            base.AddItemBefore(node, newNode);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddBefore, node, null, newNode);
        }

        protected override void AddLastItem(System.Collections.Generic.LinkedListNode<T> node)
        {
            base.AddLastItem(node);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(LinkedCollectionChangedAction.AddLast, null, null, node);
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
            foreach (System.Collections.Generic.LinkedListNode<T> node in new Enumerable<ILinkedListNode<T>>(() => new LinkedListEnumerator<T>(new Abstraction.Generic.LinkedList<T>(InnerList), EnumerationDirection.FIFO, null, null)).Select(node=>((Abstraction.Generic.LinkedListNode<T>)node).InnerNode))

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
#endif
        }
#if WinCopies3
    }
#endif
}
#endif
