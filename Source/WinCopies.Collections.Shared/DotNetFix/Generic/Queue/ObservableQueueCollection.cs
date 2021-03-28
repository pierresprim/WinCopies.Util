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
using System.Collections;
using System.Collections.Generic;
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
    public class ObservableQueueCollection<
#if WinCopies3
        TQueue, TItems
#else
        T
#endif
        > : QueueCollection<
#if WinCopies3
        TQueue, TItems
#else
        T
#endif
       >, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<
#if WinCopies3
           TItems
#else
           T
#endif
           >
#if WinCopies3
where TQueue : IQueue<TItems>
#endif
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event SimpleLinkedCollectionChangedEventHandler<
#if WinCopies3
           TItems
#else
           T
#endif
          > CollectionChanged;

#if !WinCopies3
        public ObservableQueueCollection() : base() { }
#endif

        public ObservableQueueCollection(in
#if WinCopies3
            TQueue
#else
            System.Collections.Generic.Queue<T>
#endif
      queue) : base(queue) { }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

        protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<
#if WinCopies3
           TItems
#else
           T
#endif
         > e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action,
#if WinCopies3
           TItems
#else
           T
#endif
          item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<
#if WinCopies3
           TItems
#else
           T
#endif
         >(action, item));

        protected override void ClearItems()
        {
            base.ClearItems();

            RaiseCountPropertyChangedEvent();

            OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<
#if WinCopies3
           TItems
#else
           T
#endif
         >(NotifyCollectionChangedAction.Reset, default));
        }

        protected override
#if WinCopies3
           TItems
#else
           T
#endif
          DequeueItem()
        {
#if WinCopies3
                TItems
#else
            T
#endif
          result = base.DequeueItem();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);

            return result;
        }

        protected override void EnqueueItem(
#if WinCopies3
           TItems
#else
           T
#endif
          item)
        {
            base.EnqueueItem(item);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, item);
        }

#if CS8
        protected override bool TryDequeueItem([MaybeNullWhen(false)] out 
#if WinCopies3
           TItems
#else
           T
#endif
          result)
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
        public class ObservableQueueCollection<T> : ObservableQueueCollection<IQueue<T>, T>
        {
            public ObservableQueueCollection() : this(new Queue<T>()) { }

            public ObservableQueueCollection(in IQueue<T> queue) : base(queue) { }
        }
    }
#endif
}

#endif
