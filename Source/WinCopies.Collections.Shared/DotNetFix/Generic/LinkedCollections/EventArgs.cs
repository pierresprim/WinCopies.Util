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
using WinCopies.Util;

#if !WinCopies3
using static WinCopies.Util.Util;
#endif

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    namespace Generic
    {
#endif

        public class SimpleLinkedCollectionChangedEventArgs<T>
        {
            public NotifyCollectionChangedAction Action { get; }

            public T Item { get; }

            public SimpleLinkedCollectionChangedEventArgs(NotifyCollectionChangedAction action, T item)
            {
                Action = action;

                Item = item;
            }
        }

        public class LinkedCollectionChangedEventArgs<T>
        {
            public LinkedCollectionChangedAction Action { get; }

            public
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddedBefore
            { get; }

            public
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddedAfter
            { get; }

            public
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> Node
            { get; }

            public LinkedCollectionChangedEventArgs(LinkedCollectionChangedAction action,
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> addedBefore,
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> addedAfter,
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node)
            {
                bool check(LinkedCollectionChangedAction _action,
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> parameter) => (_action == action && parameter == null) || !(_action == action || parameter == null);

                if ((action == LinkedCollectionChangedAction.Reset && (node != null || addedBefore != null || addedAfter != null))
                    || (action != LinkedCollectionChangedAction.Reset && node == null)
                    || (action.IsValidEnumValue(true, LinkedCollectionChangedAction.AddFirst, LinkedCollectionChangedAction.AddLast) && (addedBefore != null || addedAfter != null))
                    || (action == LinkedCollectionChangedAction.Move && addedBefore == null && addedAfter == null)
                    || check(LinkedCollectionChangedAction.AddBefore, addedBefore)
                    || check(LinkedCollectionChangedAction.AddAfter, addedAfter)
                    || (action.IsValidEnumValue(true, LinkedCollectionChangedAction.Remove, LinkedCollectionChangedAction.Reset) && !(addedBefore == null && addedAfter == null)))

                    throw new ArgumentException($"Invalid combination of {nameof(action)} and {nameof(node)}, {nameof(addedBefore)} or {nameof(addedAfter)}.");

                Action = action;

                AddedBefore = addedBefore;

                AddedAfter = addedAfter;

                Node = node;
            }
        }

        public delegate void SimpleLinkedCollectionChangedEventHandler<T>(object sender, SimpleLinkedCollectionChangedEventArgs<T> e);

        public interface INotifySimpleLinkedCollectionChanged<T>
        {
            event SimpleLinkedCollectionChangedEventHandler<T> CollectionChanged;
        }

        public delegate void LinkedCollectionChangedEventHandler<T>(object sender, LinkedCollectionChangedEventArgs<T> e);

        public interface INotifyLinkedCollectionChanged<T>
        {
            event LinkedCollectionChangedEventHandler<T> CollectionChanged;
        }
#if WinCopies3
    }
#endif
}
#endif
