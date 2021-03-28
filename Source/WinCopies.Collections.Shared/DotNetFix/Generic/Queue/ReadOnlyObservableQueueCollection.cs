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

using static WinCopies
#if !WinCopies3
    .Util.Util;

using System.Runtime.Serialization;

using WinCopies.Util;
using static WinCopies.Util.ThrowHelper;
#else
    .ThrowHelper;
#endif

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    namespace Generic
    {
#endif
    [Serializable]
    public class ReadOnlyObservableQueueCollection<
#if WinCopies3
            TQueue, TItems
#else
            T
#endif
            > :
#if WinCopies3
            ReadOnlyQueueCollection<TQueue, TItems>
#else
            IReadOnlyCollection<T>, ICollection
#endif
        , INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<
#if WinCopies3
                TItems> where TQueue : IQueue<TItems>
#else
                T>
#endif
    {
        public
#if !WinCopies3
int
#else
                uint
#endif
                Count =>
#if WinCopies3
            InnerQueue
#else
            InnerQueueCollection
#endif
            .Count;

        public bool IsReadOnly => true;

#if !WinCopies3
        bool ICollection.IsSynchronized => ((ICollection)InnerQueueCollection).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueueCollection).SyncRoot;
#endif

        public event PropertyChangedEventHandler PropertyChanged;

        public event SimpleLinkedCollectionChangedEventHandler<
#if WinCopies3
                TItems
#else
                T
#endif
             > CollectionChanged;

        public ReadOnlyObservableQueueCollection(in ObservableQueueCollection<
#if WinCopies3
                TQueue, TItems
#else
                T
#endif
             > queueCollection)
#if WinCopies3
: base(queueCollection.InnerQueue)
#endif
        {
            (queueCollection ?? throw GetArgumentNullException(nameof(queueCollection))).CollectionChanged += (object sender, SimpleLinkedCollectionChangedEventArgs<
#if WinCopies3
                TItems
#else
                T
#endif
             > e) => OnCollectionChanged(e);

            queueCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

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

#if !WinCopies3
        protected ObservableQueueCollection<T> InnerQueueCollection { get; }

        public IEnumerator<T> GetEnumerator() => InnerQueueCollection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueueCollection).GetEnumerator();

        public void CopyTo(T[] array, int arrayIndex) => InnerQueueCollection.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueueCollection).CopyTo(array, index);
#endif
    }
#if WinCopies3
    }
#endif
}

#endif
