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
    namespace Generic
    {
        [Serializable]
        public class ObservableQueueCollection<TQueue, TItems> : QueueCollection<TQueue, TItems>, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<TItems> where TQueue : IQueue<TItems>
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public event SimpleLinkedCollectionChangedEventHandler<TItems> CollectionChanged;

            public ObservableQueueCollection(in TQueue queue) : base(queue) { }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

            protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

            protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<TItems> e) => CollectionChanged?.Invoke(this, e);

            protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, TItems item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<TItems>(action, item));

            public override void Enqueue(TItems item)
            {
                base.Enqueue(item);

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, item);
            }

            public override TItems Dequeue()
            {
                TItems result = base.Dequeue();

                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);

                return result;
            }
#if CS8
            public override bool TryDequeue([MaybeNullWhen(false)] out TItems result)
            {
                bool succeeded = base.TryDequeue(out result);

                if (succeeded)
                {
                    RaiseCountPropertyChangedEvent();

                    RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);
                }

                return succeeded;
            }
#endif
            public override void Clear()
            {
                base.Clear();

                RaiseCountPropertyChangedEvent();

                OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<TItems>(NotifyCollectionChangedAction.Reset, default));
            }
        }

        public class ObservableQueueCollection<T> : ObservableQueueCollection<IQueue<T>, T>
        {
            public ObservableQueueCollection(in IQueue<T> queue) : base(queue) { /* Left empty. */ }
            public ObservableQueueCollection() : this(new Queue<T>()) { /* Left empty. */ }
        }
    }
}
#endif
