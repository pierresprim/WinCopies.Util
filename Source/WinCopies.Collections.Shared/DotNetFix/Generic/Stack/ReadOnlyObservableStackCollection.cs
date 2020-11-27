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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using static WinCopies
#if WinCopies2
    .Util.Util;
using static WinCopies.Util.ThrowHelper;
#else
    .ThrowHelper;
#endif

namespace WinCopies.Collections.DotNetFix
{
#if !WinCopies2
    namespace Generic
    {
#endif

        [Serializable]
        public class ReadOnlyObservableStackCollection<T> : System.Collections.Generic.IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<T>
        {
            protected ObservableStackCollection<T> InnerStackCollection { get; }

            public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerStackCollection.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

            public bool IsReadOnly => true;

            bool ICollection.IsSynchronized => ((ICollection)InnerStackCollection).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerStackCollection).SyncRoot;

            public event PropertyChangedEventHandler PropertyChanged;

            public event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;

            public ReadOnlyObservableStackCollection(in ObservableStackCollection<T> stackCollection)
            {
                InnerStackCollection = stackCollection ?? throw GetArgumentNullException(nameof(stackCollection));

                InnerStackCollection.CollectionChanged += (object sender, SimpleLinkedCollectionChangedEventArgs<T> e) => OnCollectionChanged(e);

                InnerStackCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
            }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

            protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

            protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

            protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, T item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<T>(action, item));

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerStackCollection.GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStackCollection).GetEnumerator();

            public void CopyTo(T[] array, int arrayIndex) => InnerStackCollection.CopyTo(array, arrayIndex);

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStackCollection).CopyTo(array, index);
        }
#if !WinCopies2
    }
#endif
}
