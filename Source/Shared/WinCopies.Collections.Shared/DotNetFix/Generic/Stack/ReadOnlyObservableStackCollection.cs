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

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    [Serializable]
    public class ReadOnlyObservableStackCollection<TStack, TItems> : ReadOnlyStackCollection<TStack, TItems>, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<TItems> where TStack : IStack<TItems>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event SimpleLinkedCollectionChangedEventHandler<TItems> CollectionChanged;

        public ReadOnlyObservableStackCollection(in ObservableStackCollection<TStack, TItems> stackCollection) : base(stackCollection.InnerList)
        {
            (stackCollection ?? throw GetArgumentNullException(nameof(StackCollection))).CollectionChanged += (object sender, SimpleLinkedCollectionChangedEventArgs<TItems> e) => OnCollectionChanged(e);

            stackCollection.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<TItems> e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, TItems item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<TItems>(action, item));
    }
}
#endif
