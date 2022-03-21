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

using System.Collections;
using System.Collections.Specialized;

namespace WinCopies.Collections.DotNetFix
{
    public class NotifyCollectionChangedEventArgs : System.Collections.Specialized.NotifyCollectionChangedEventArgs
    {
        public bool IsChangingEvent { get; }

#if WinCopies3
        public NotifyCollectionChangedEventArgs(in bool isChangingEvent, in NotifyCollectionChangedAction action) : base(action) => IsChangingEvent = isChangingEvent;
#else
        public IList ResetItems { get; }

        public NotifyCollectionChangedEventArgs(IList resetItems) : base(NotifyCollectionChangedAction.Reset)
        {
            IsChangingEvent = true;

            ResetItems = resetItems;
        }
#endif

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem) : base(action, changedItem) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems) : base(action, changedItems) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem, int index) : base(action, changedItem, index) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems, int startingIndex) : base(action, changedItems, startingIndex) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object newItem, object oldItem) : base(action, newItem, oldItem) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList newItems, IList oldItems) : base(action, newItems, oldItems) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object newItem, object oldItem, int index) : base(action, newItem, oldItem, index) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex) : base(action, newItems, oldItems, startingIndex) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex) : base(action, changedItem, index, oldIndex) => IsChangingEvent = isChangingEvent;

        public NotifyCollectionChangedEventArgs(bool isChangingEvent, NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex) : base(action, changedItems, index, oldIndex) => IsChangingEvent = isChangingEvent;
    }
}
