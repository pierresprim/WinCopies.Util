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

namespace WinCopies.Collections.DotNetFix.Generic
{
    [Serializable]
    public class ObservableStackCollection<TStack, TItems> : StackCollection<TStack, TItems>, INotifyPropertyChanged, INotifySimpleLinkedCollectionChanged<TItems> where TStack : IStack<TItems>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event SimpleLinkedCollectionChangedEventHandler<TItems> CollectionChanged;

        public ObservableStackCollection(in TStack stack) : base(stack) { /* Left empty. */ }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected void RaisePropertyChangedEvent(in string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaiseCountPropertyChangedEvent() => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));

        protected virtual void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<TItems> e) => CollectionChanged?.Invoke(this, e);

        protected void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, TItems item) => OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<TItems>(action, item));

        public override void Clear()
        {
            base.Clear();

            RaiseCountPropertyChangedEvent();

            OnCollectionChanged(new SimpleLinkedCollectionChangedEventArgs<TItems>(NotifyCollectionChangedAction.Reset, default));
        }

        public override TItems Remove()
        {
            TItems result = base.Remove();

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);

            return result;
        }

        public override void Add(TItems item)
        {
            base.Add(item);

            RaiseCountPropertyChangedEvent();

            RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, item);
        }
#if CS8
        public override bool TryRemove([MaybeNullWhen(false)] out TItems result)
        {
            bool succeeded = base.TryRemove(out result);

            if (succeeded)
            {
                RaiseCountPropertyChangedEvent();

                RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Remove, result);
            }

            return succeeded;
        }
#endif
    }

    public class ObservableStackCollection<T> : ObservableStackCollection<IStack<T>, T>
    {
        public ObservableStackCollection() : this(new Stack<T>()) { /* Left empty. */ }

        public ObservableStackCollection(in IStack<T> stack) : base(stack) { /* Left empty. */ }
    }
}
#endif
