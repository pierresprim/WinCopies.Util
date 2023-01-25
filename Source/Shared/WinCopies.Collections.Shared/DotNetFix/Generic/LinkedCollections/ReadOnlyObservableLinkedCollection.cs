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
using System.ComponentModel;

namespace WinCopies.Collections.DotNetFix.Generic
{
    [Serializable]
    public class ReadOnlyObservableLinkedCollection<T> : ReadOnlyLinkedCollection<T, ILinkedListNode<T>, ObservableLinkedCollection<T>>, System.Collections.Generic.ICollection<T>, IEnumerable<T>, IEnumerable, System.Collections.Generic.IReadOnlyCollection<T>, ICollection, INotifyPropertyChanged, INotifyLinkedCollectionChanged<T>, IReadOnlyLinkedList<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event LinkedCollectionChangedEventHandler<T> CollectionChanged;

        public ReadOnlyObservableLinkedCollection(in ObservableLinkedCollection<T> linkedCollection) : base(linkedCollection)
        {
            linkedCollection.CollectionChanged += InnerLinkedCollection_CollectionChanged;
            linkedCollection.PropertyChanged += InnerLinkedCollection_PropertyChanged;
        }

        private void InnerLinkedCollection_CollectionChanged(object sender, LinkedCollectionChangedEventArgs<T> e) => OnCollectionChanged(e);
        private void InnerLinkedCollection_PropertyChanged(object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

        protected virtual void OnCollectionChanged(LinkedCollectionChangedEventArgs<T> e) => CollectionChanged?.Invoke(this, e);

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();

            InnerList.CollectionChanged -= InnerLinkedCollection_CollectionChanged;
            InnerList.PropertyChanged -= InnerLinkedCollection_PropertyChanged;
        }
    }
}
#endif
