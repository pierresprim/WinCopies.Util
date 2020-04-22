/* Copyright © Pierre Sprimont, 2019
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using WinCopies.Util;

namespace WinCopies.Collections
{

    //public interface IReadOnlyObservableCollection<T> : IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged, INotifyCollectionChanging

    //{



    //}

    //public class ReadOnlyObservableCollection<T> : System.Collections.ObjectModel.ReadOnlyObservableCollection<T>, IReadOnlyObservableCollection<T>
    //{

    //    protected virtual event NotifyCollectionChangingEventHandler CollectionChanging;

    //    event NotifyCollectionChangingEventHandler INotifyCollectionChanging.CollectionChanging
    //    {
    //        add => CollectionChanging += value;

    //        remove => CollectionChanging -= value;
    //    }

    //    public ReadOnlyObservableCollection(ObservableCollection<T> list) : base(list) => list.CollectionChanging += (object sender, NotifyCollectionChangedEventArgs e) => OnCollectionChanging(e);

    //    protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e) => CollectionChanging?.Invoke(this, e);

    //    void IReadOnlyList<T>.Clear() => ((IReadOnlyList<T>)this).Clear();

    //    void IReadOnlyList<T>.RemoveAt(int index) => ((IReadOnlyList<T>)this).RemoveAt(index);

    //    T IReadOnlyList<T>.this[int index] { get => this[index]; set => ((IReadOnlyList<T>)this)[index] = value; }

    //}using WinCopies.Util;

    namespace DotNetFix
    {
        [Serializable]
        public class ReadOnlyObservableCollection<T> : System.Collections.ObjectModel.ReadOnlyObservableCollection<T>, INotifyCollectionChanging
        {

            public event NotifyCollectionChangingEventHandler CollectionChanging;

            public ReadOnlyObservableCollection(ObservableCollection<T> list) : base(list) => list.CollectionChanging += (object sender, DotNetFix.NotifyCollectionChangedEventArgs e) => CollectionChanging?.Invoke(this, e);
        }
    }

    [Obsolete("This class has been moved to the WinCopies.Collections.DotNetFix namespace. This implementation is still here temporarily only.")]
    [Serializable]
    public class ReadOnlyObservableCollection<T> : System.Collections.ObjectModel.ReadOnlyObservableCollection<T>, INotifyCollectionChanging
    {

        public event NotifyCollectionChangingEventHandler CollectionChanging;

        public ReadOnlyObservableCollection(ObservableCollection<T> list) : base(list) => list.CollectionChanging += (object sender, DotNetFix.NotifyCollectionChangedEventArgs e) => CollectionChanging?.Invoke(this, e);
    }

}
