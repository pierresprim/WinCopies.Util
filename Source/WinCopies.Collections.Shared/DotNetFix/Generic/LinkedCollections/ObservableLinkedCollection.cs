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
using System.ComponentModel;

#if WinCopies2
using static WinCopies.Util.Util;

using System.Runtime.Serialization;

using WinCopies.Util;
#endif

namespace WinCopies.Collections.DotNetFix
{
#if !WinCopies2
    namespace Generic
    {
#endif

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

            protected void RaiseCollectionChangedEvent(in LinkedCollectionChangedAction action, in
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> addedBefore, in
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> addedAfter, in
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node) => OnCollectionChanged(new LinkedCollectionChangedEventArgs<T>(action, addedBefore, addedAfter, node));

#if WinCopies2
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
#endif

            protected override
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddFirstItem(T value)
            {
#if WinCopies2
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
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddItemAfter(
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value)
            {

#if WinCopies2
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
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddItemBefore(
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value)
            {

#if WinCopies2
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
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddLastItem(T value)
            {
#if WinCopies2
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

            protected override void RemoveFirstItem()
            {
#if WinCopies2
            System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node = InnerList.First;

                base.RemoveFirstItem();

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
            }

            protected override void RemoveItem(
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node)
            {
                base.RemoveItem(node);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
            }

            protected override bool RemoveItem(T item)
            {
                foreach (
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node in new Generic.LinkedListNodeEnumerator<T>(InnerList))

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
#if WinCopies2
            System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node = InnerList.Last;

                base.RemoveLastItem();

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(LinkedCollectionChangedAction.Remove, null, null, node);
            }
        }
#if !WinCopies2
    }
#endif
}
#endif
