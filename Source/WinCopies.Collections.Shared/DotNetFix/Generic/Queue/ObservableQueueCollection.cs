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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
        namespace Generic
        {
#endif

    [Serializable]
    public class ObservableQueueCollection<T> : QueueCollection<T>, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ObservableQueueCollection() : base() { }

        public ObservableQueueCollection(in
#if WinCopies2
            System.Collections.Generic.Queue
#else
            IEnumerableQueue
#endif
            <T> queue) : base(queue) { }

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
#if WinCopies3
    }
#endif
}

#endif
