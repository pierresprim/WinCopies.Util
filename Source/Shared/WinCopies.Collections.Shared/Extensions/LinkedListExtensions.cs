/* Copyright © Pierre Sprimont, 2022
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

using static WinCopies.UtilHelpers;

using GenericCollections = WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections
{
    public static class LinkedListExtensions
    {
#if CS5
        public static DotNetFix.IReadOnlyLinkedListNode<GenericCollections.ILinkedListNode<T>> AsLinkedListNode<T>(this GenericCollections.ILinkedListNode<T> node) => node;
#endif
        public static bool TryPeek<T>(this GenericCollections.IPeekable<T> collection, out object
#if CS8
            ?
#endif
            value) => TryGetValue<T>(collection.TryPeek, out value);

        public static bool TryDequeue<T>(this GenericCollections.IQueueBase<T> queue, out object
#if CS8
            ?
#endif
            value) => TryGetValue<T>(queue.TryDequeue, out value);

        public static bool TryPop<T>(this GenericCollections.IStackBase<T> stack, out object
#if CS8
            ?
#endif
            value) => TryGetValue<T>(stack.TryPop, out value);

        private static IEnumerable<T> GetEnumerable<T>(FuncOut<T, bool> func)
        {
            while (func(out T value))

                yield return value;
        }

        private static FuncOut<T, bool> GetFunc<T>(FuncOut<T, bool> func, Predicate<T> predicate) => (out T value) =>
        {
            if (func(out T _value) && predicate(_value))
            {
                value = _value;

                return true;
            }

            value = default;

            return false;
        };

        private static IEnumerable<T> GetEnumerable<T>(FuncOut<T, bool> func, Predicate<T> predicate) => GetEnumerable(GetFunc(func, predicate));

        private static IEnumerable<T> GetEnumerable2<T>(FuncOut<T, bool> func, Predicate<T> predicate)
        {
            if (func(out T value))
            {
                yield return value;

                while (predicate(value) && func(out value))

                    yield return value;
            }
        }

        private static IEnumerable<T> GetEnumerable<T>(FuncOut<T, bool> func, Predicate<T> pre, Predicate<T> post) => GetEnumerable2(GetFunc(func, pre), post);

        public static IEnumerable<T> GetPopperEnumerable<T>(this GenericCollections.IStackBase<T> stack) => GetEnumerable<T>(stack.TryPop);
        public static IEnumerable<T> GetPopperEnumerable<T>(this GenericCollections.IStackBase<T> stack, in Predicate<T> predicate) => GetEnumerable(stack.TryPop, predicate);
        public static IEnumerable<T> GetPopperEnumerable2<T>(this GenericCollections.IStackBase<T> stack, in Predicate<T> predicate) => GetEnumerable2(stack.TryPop, predicate);
        public static IEnumerable<T> GetPopperEnumerable<T>(this GenericCollections.IStackBase<T> stack, in Predicate<T> pre, in Predicate<T> post) => GetEnumerable(stack.TryPop, pre, post);

        public static IEnumerable<T> GetDequeuerEnumerable<T>(this GenericCollections.IQueueBase<T> queue) => GetEnumerable<T>(queue.TryDequeue);
        public static IEnumerable<T> GetDequeuerEnumerable<T>(this GenericCollections.IQueueBase<T> queue, in Predicate<T> predicate) => GetEnumerable(queue.TryDequeue, predicate);
        public static IEnumerable<T> GetDequeuerEnumerable2<T>(this GenericCollections.IQueueBase<T> queue, in Predicate<T> predicate) => GetEnumerable2(queue.TryDequeue, predicate);
        public static IEnumerable<T> GetDequeuerEnumerable<T>(this GenericCollections.IQueueBase<T> queue, in Predicate<T> pre, in Predicate<T> post) => GetEnumerable<T>(queue.TryDequeue, pre, post);
    }
}
