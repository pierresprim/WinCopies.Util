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
using System.ComponentModel;

using static WinCopies
#if WinCopies2
    .Util.Util;

using System.Runtime.Serialization;

using WinCopies.Util;
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
        public class ReadOnlyObservableLinkedCollection<T> : ICollection<T>, System.Collections.Generic.IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>
        {
            protected ObservableLinkedCollection<T> InnerLinkedCollection { get; }

            public
#if WinCopies2
int
#else
                uint
#endif
                Count => InnerLinkedCollection.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;

            int ICollection<T>.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

            public bool IsReadOnly => true;

            bool ICollection.IsSynchronized => ((ICollection)InnerLinkedCollection).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerLinkedCollection).SyncRoot;

            public event PropertyChangedEventHandler PropertyChanged;

            public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

            public ReadOnlyObservableLinkedCollection(in ObservableLinkedCollection<T> linkedCollection)
            {
                InnerLinkedCollection = linkedCollection ?? throw GetArgumentNullException(nameof(linkedCollection));

                InnerLinkedCollection.CollectionChanged += (object sender, LinkedCollectionChangedEventArgs<T> e) => OnCollectionChanged(e);

                InnerLinkedCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
            }

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

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerLinkedCollection.GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerLinkedCollection).GetEnumerator();

            public void CopyTo(T[] array, int arrayIndex) => InnerLinkedCollection.CopyTo(array, arrayIndex);

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerLinkedCollection).CopyTo(array, index);

            void ICollection<T>.Add(T item) => ((ICollection<T>)InnerLinkedCollection).Add(item);

            void ICollection<T>.Clear() => InnerLinkedCollection.Clear();

            public bool Contains(T item) => InnerLinkedCollection.Contains(item);

            bool ICollection<T>.Remove(T item) => InnerLinkedCollection.Remove(item);
        }
#if !WinCopies2
    }
#endif
}