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

#if WinCopies3

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using WinCopies.Collections;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Util;
using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
    public static class Extensions
    {
        public static ReadOnlyQueueCollection<T> ToReadOnlyQueue<T>(this object[] array)
        {
            var queue = new EnumerableQueue<T>();

            foreach (object value in array)

                queue.Enqueue((T)value);

            return new ReadOnlyQueueCollection<T>(queue);
        }

        public static bool IsEnumeratorNotStartedOrDisposed(this IDisposableEnumeratorInfo enumerator) => (enumerator ?? throw GetArgumentNullException(nameof(enumerator))).IsDisposed || !enumerator.IsStarted;

        #region Enumerables extension methods

        // todo:

        //#region Contains methods

        //// public static bool Contains(this IEnumerable array, object value) => array.Contains(value, EqualityComparer<object>.Default);

        //public static bool Contains(this IEnumerable array, object value, IEqualityComparer comparer)

        //{

        //    foreach (object _value in array)

        //        if (comparer.Equals(_value, value)) return true;

        //    return false;

        //}

        //public static bool Contains(this IEnumerable array, object value, System.Collections.Generic.IComparer comparer) => array.Contains(value, (object x, object y) => comparer.Compare(x, y));

        //public static bool Contains(this IEnumerable array, object value, Comparison comparison)

        //{

        //    foreach (object _value in array)

        //        if (comparison(_value, value) == 0) return true;

        //    return false;

        //}

        //// todo: to add methods for the LongLength's Array property gesture.

        //public static bool Contains(this object[] array, object value, out int index) => array.Contains(value, EqualityComparer<object>.Default, out index);

        //public static bool Contains(this object[] array, object value, IEqualityComparer comparer, out int index)

        //{

        //    for (int i = 0; i < array.Length; i++)

        //        if (comparer.Equals(array[i], value))

        //        {

        //            index = i;

        //            return true;

        //        }

        //    index = -1;

        //    return false;

        //}

        //public static bool Contains(this object[] array, object value, System.Collections.Generic.IComparer comparer, out int index) => array.Contains(value, (object x, object y) => comparer.Compare(x, y), out index);

        //public static bool Contains(this object[] array, object value, Comparison comparison, out int index)

        //{

        //    for (int i = 0; i < array.Length; i++)

        //        if (comparison(array[i], value) == 0)

        //        {

        //            index = i;

        //            return true;

        //        }

        //    index = -1;

        //    return false;

        //}

        //public static bool ContainsRange(this IEnumerable array, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, IEqualityComparer comparer, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, IEqualityComparer comparer, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, System.Collections.Generic.IComparer comparer, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, System.Collections.Generic.IComparer comparer, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, Comparison comparison, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, Comparison comparison, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, out int index, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, out int index, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, IEqualityComparer comparer, out int index, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, IEqualityComparer comparer, out int index, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, System.Collections.Generic.IComparer comparer, out int index, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, System.Collections.Generic.IComparer comparer, out int index, IEnumerable values);

        //public static bool ContainsRange(this IEnumerable array, Comparison comparison, out int index, params object[] values);

        //public static bool ContainsRange(this IEnumerable array, Comparison comparison, out int index, IEnumerable values);

        //public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, T value);

        //public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, T value, IEqualityComparer comparer);

        //public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, T value, System.Collections.Generic.IComparer comparer);

        //public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, T value, Comparison comparison);

        //public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, T value, out int index);

        //public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, T value, IEqualityComparer comparer, out int index);

        //public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, T value, System.Collections.Generic.IComparer comparer, out int index);

        //public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, T value, Comparison comparison, out int index);

        //public static bool ContainsRange<T>(this System.Collections.Generic.IEnumerable<T> array, params T[] values);

        //public static bool ContainsRange<T>(this System.Collections.Generic.IEnumerable<T> array, IEqualityComparer comparer, params T[] values);

        //public static bool ContainsRange<T>(this System.Collections.Generic.IEnumerable<T> array, System.Collections.Generic.IComparer comparer, params T[] values);

        //public static bool ContainsRange<T>(this System.Collections.Generic.IEnumerable<T> array, Comparison comparison, params T[] values);

        //public static bool ContainsRange<T>(this System.Collections.Generic.IEnumerable<T> array, out int index, params T[] values);

        //public static bool ContainsRange<T>(this System.Collections.Generic.IEnumerable<T> array, IEqualityComparer comparer, out int index, params T[] values);

        //public static bool ContainsRange<T>(this System.Collections.Generic.IEnumerable<T> array, System.Collections.Generic.IComparer comparer, out int index, params T[] values);

        //public static bool ContainsRange<T>(this System.Collections.Generic.IEnumerable<T> array, Comparison comparison, out int index, params T[] values);

        //#endregion

        // todo: Add-, Insert-, Remove-If(Not)Contains methods: add parameters like the Contains methods

        #region Add(Range)IfNotContains methods

        /// <summary>
        /// Tries to add a value to an <see cref="IList"/> if it does not contain it already.
        /// </summary>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="value">The value to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static bool AddIfNotContains(this IList collection, in object value)
        {
            if ((collection ?? throw GetArgumentNullException(nameof(collection))).Contains(value)) return false;

            _ = collection.Add(value);

            return true;
        }

        /// <summary>
        /// Tries to add multiple values to an <see cref="System.Collections.IList"/> if it does not contain them already.
        /// </summary>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="values">The values to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static object[] AddRangeIfNotContains(this System.Collections.IList collection, params object[] values) => collection.AddRangeIfNotContains((IEnumerable)values);

#if CS7
        /// <summary>
        /// Tries to add multiple values to an <see cref="System.Collections.IList"/> if it does not contain them already.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="values">The values to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static object[] AddRangeIfNotContains(this System.Collections.IList collection, in IEnumerable values)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(values, nameof(values));

            var addedValues = new ArrayBuilder<object>();

            foreach (object value in values)
            {
                if (collection.Contains(value)) continue;

                _ = collection.Add(value);

                _ = addedValues.AddLast(value);
            }

            return addedValues.ToArray();
        }

        /// <summary>
        /// Tries to add multiple values to an <see cref="ICollection{T}"/> if it does not contain them already.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="values">The values to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static T[] AddRangeIfNotContains<T>(this ICollection<T> collection, in System.Collections.Generic.IEnumerable<T> values)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(values, nameof(values));

            var addedValues = new ArrayBuilder<T>();

            foreach (T value in values)
            {
                if (collection.Contains(value)) continue;

                collection.Add(value);

                _ = addedValues.AddLast(value);
            }

            return addedValues.ToArray();
        }

        /// <summary>
        /// Tries to add multiple values to an <see cref="ICollection{T}"/> if it does not contain them already.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="values">The values to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static T[] AddRangeIfNotContains<T>(this ICollection<T> collection, params T[] values) => collection.AddRangeIfNotContains((System.Collections.Generic.IEnumerable<T>)values);
#endif

        /// <summary>
        /// Tries to add a value to an <see cref="ICollection{T}"/> if it does not contain it already.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="value">The value to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static bool AddIfNotContains<T>(this ICollection<T> collection, in T value)
        {
            if ((collection ?? throw GetArgumentNullException(nameof(collection))).Contains(value)) return false;

            collection.Add(value);

            return true;
        }

#endregion

        #region Insert(Range)IfNotContains methods

        /// <summary>
        /// Inserts a value at the specified index in a given collection if the value does not already exists in the collection.
        /// </summary>
        /// <param name="collection">The collection in which to add the value.</param>
        /// <param name="index">The index at the collection to which add the value.</param>
        /// <param name="value">The value to add in the collection.</param>
        /// <returns><see langword="true"/> if the value is added to the collection, otherwise <see langword="false"/>.</returns>
        public static bool InsertIfNotContains(this IList collection, in int index, in object value)
        {
            if ((collection ?? throw GetArgumentNullException(nameof(collection))).Contains(value)) return false;

            collection.Insert(index, value);

            return true;
        }

        public static object[] InsertRangeIfNotContains(this IList collection, in int index, params object[] values) => collection.InsertRangeIfNotContains(index, (IEnumerable)values);

#if CS7
        public static object[] InsertRangeIfNotContains(this IList collection, in int index, in IEnumerable values)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(values, nameof(values));

            var addedValues = new ArrayBuilder<object>();

            foreach (object value in values)
            {
                if (collection.Contains(value)) continue;

                collection.Insert(index, value);

                _ = addedValues.AddLast(value);
            }

            return addedValues.ToArray();
        }
#endif

        public static bool InsertIfNotContains<T>(this System.Collections.Generic.IList<T> collection, in int index, in T value)
        {
            ThrowIfNull(collection, nameof(collection));

            if (collection.Contains(value)) return false;

            collection.Insert(index, value);

            return true;
        }

#if CS7
        public static T[] InsertRangeIfNotContains<T>(this System.Collections.Generic.IList<T> collection, in int index, params T[] values) => collection.InsertRangeIfNotContains(index, (System.Collections.Generic.IEnumerable<T>)values);

        public static T[] InsertRangeIfNotContains<T>(this System.Collections.Generic.IList<T> collection, in int index, in System.Collections.Generic.IEnumerable<T> values)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(values, nameof(values));

            var addedValues = new ArrayBuilder<T>();

            foreach (T value in values)
            {
                if (collection.Contains(value)) continue;

                collection.Insert(index, value);

                _ = addedValues.AddLast(value);
            }

            return addedValues.ToArray();
        }
#endif

#endregion

        #region Remove(Range)IfContains methods

        public static bool RemoveIfContains(this IList collection, in object value)
        {
            if ((collection ?? throw GetArgumentNullException(nameof(collection))).Contains(value))
            {
                collection.Remove(value);

                return true;
            }

            return false;
        }

        public static object[] RemoveRangeIfContains(this IList collection, params object[] values) => collection.RemoveRangeIfContains((IEnumerable)values);

#if CS7
        public static object[] RemoveRangeIfContains(this IList collection, in IEnumerable values)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(values, nameof(values));

            var removedValues = new ArrayBuilder<object>();

            foreach (object value in values)

                if (collection.Contains(value))
                {
                    // todo: RemoveAt()

                    collection.Remove(value);

                    _ = removedValues.AddLast(value);
                }

            return removedValues.ToArray();
        }
#endif

        public static bool RemoveIfContains<T>(this ICollection<T> collection, in T value) => (collection ?? throw GetArgumentNullException(nameof(collection))).Contains(value) ? collection.Remove(value) : false;

#if CS7
        public static T[] RemoveRangeIfContains<T>(this IList<T> collection, params T[] values) => collection.RemoveRangeIfContains((System.Collections.Generic.IEnumerable<T>)values);

        public static T[] RemoveRangeIfContains<T>(this IList<T> collection, in System.Collections.Generic.IEnumerable<T> values)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(values, nameof(values));

            var removedValues = new ArrayBuilder<T>();

            foreach (T value in values)

                for (int i = 0; i < collection.Count; i++)

                    if (collection[i].Equals(value))
                    {
                        // todo: RemoveAt()

                        collection.RemoveAt(i);

                        _ = removedValues.AddLast(value);
                    }

            return removedValues.ToArray();
        }
#endif

#endregion

        #region AddRange methods

        public static void AddRange(this IList collection, params object[] values) => collection.AddRange((IEnumerable)values);

        public static void AddRange(this IList collection, in IEnumerable array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            foreach (object item in array)

                _ = collection.Add(item);
        }

        public static void AddRange(this IList collection, in int start, in int length, params object[] values)
        {
            ThrowIfNull(collection, nameof(collection));

            for (int i = start; i < length; i++)

                _ = collection.Add(values[i]);
        }

        // todo: to add a version of the methods like this one with a 'contains' check:

        public static void AddRange(this IList collection, in IList values, in int start, in int length)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(values, nameof(values));

            for (int i = start; i < length; i++)

                _ = collection.Add(values[i]);
        }

        public static void AddRange<T>(this ICollection<T> collection, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            foreach (T item in array)

                collection.Add(item);
        }

        public static void AddRange<T>(this ICollection<T> collection, in int start, in int length, params T[] values)
        {
            ThrowIfNull(collection, nameof(collection));

            for (int i = start; i < length; i++)

                collection.Add(values[i]);
        }

        public static void AddRange<T>(this ICollection<T> collection, in System.Collections.Generic.IList<T> values, in int start, in int length)
        {
            ThrowIfNull(collection, nameof(collection));

            for (int i = start; i < length; i++)

                collection.Add(values[i]);
        }

        public static void AddRange<T>(this ICollection<T> collection, in System.Collections.Generic.IEnumerable<T> array, in int start, in int length) => collection.AddRange(array.ToArray<T>(), start, length);

        /// <summary>
        /// Add multiple values at the top of a <see cref="System.Collections.Generic.LinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="values">The values to add to this <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeFirst<T>(this System.Collections.Generic.LinkedList<T> collection, params T[] values)
        {
            ThrowIfNull(collection, nameof(collection));

            return collection.First == null ? collection.AddRangeLast(values) : collection.AddRangeBefore(collection.First, values);
        }

        /// <summary>
        /// Add multiple values at the top of a <see cref="System.Collections.Generic.LinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="array">The values to add to this <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeFirst<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(collection, nameof(collection));

            return collection.First == null ? collection.AddRangeLast(array) : collection.AddRangeBefore(collection.First, array);
        }

        /// <summary>
        /// Add multiple <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s at the top of a <see cref="System.Collections.Generic.LinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="nodes">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        public static void AddRangeFirst<T>(this System.Collections.Generic.LinkedList<T> collection, params System.Collections.Generic.LinkedListNode<T>[] nodes)
        {
            ThrowIfNull(collection, nameof(collection));

            if (collection.First == null)

                collection.AddRangeLast(nodes);

            else

                collection.AddRangeBefore(collection.First, nodes);
        }

        /// <summary>
        /// Add multiple <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s at the top of a <see cref="System.Collections.Generic.LinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="array">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        public static void AddRangeFirst<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
        {
            ThrowIfNull(collection, nameof(collection));

            if (collection.First == null)

                collection.AddRangeLast(array);

            else

                collection.AddRangeBefore(collection.First, array);
        }

        /// <summary>
        /// Add multiple values at the end of a <see cref="System.Collections.Generic.LinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="values">The values to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeLast<T>(this System.Collections.Generic.LinkedList<T> collection, params T[] values) => collection.AddRangeLast((System.Collections.Generic.IEnumerable<T>)values);

        /// <summary>
        /// Add multiple values at the end of a <see cref="System.Collections.Generic.LinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="array">The values to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeLast<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            var result = new EnumerableQueue<System.Collections.Generic.LinkedListNode<T>>();

            foreach (T item in array)

                result.Enqueue(collection.AddLast(item));

            return result.ToArray<System.Collections.Generic.LinkedListNode<T>>();
        }

        /// <summary>
        /// Add multiple <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s at the end of a <see cref="System.Collections.Generic.LinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="nodes">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static void AddRangeLast<T>(this System.Collections.Generic.LinkedList<T> collection, params System.Collections.Generic.LinkedListNode<T>[] nodes) => collection.AddRangeLast((System.Collections.Generic.IEnumerable<System.Collections.Generic.LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s at the end of a <see cref="System.Collections.Generic.LinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="array">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        public static void AddRangeLast<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            foreach (System.Collections.Generic.LinkedListNode<T> item in array)

                collection.AddLast(item);
        }

        /// <summary>
        /// Add multiple values before a specified node in a <see cref="System.Collections.Generic.LinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="values">The values to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeBefore<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, params T[] values) => collection.AddRangeBefore(node, (System.Collections.Generic.IEnumerable<T>)values);

        /// <summary>
        /// Add multiple values before a specified node in a <see cref="System.Collections.Generic.LinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="array">The values to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeBefore<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            var result = new EnumerableQueue<System.Collections.Generic.LinkedListNode<T>>();

            foreach (T item in array)

                result.Enqueue(collection.AddBefore(node, item));

            return result.ToArray<System.Collections.Generic.LinkedListNode<T>>();
        }

        /// <summary>
        /// Add multiple values before a specified node in a <see cref="System.Collections.Generic.LinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="nodes">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        public static void AddRangeBefore<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, params System.Collections.Generic.LinkedListNode<T>[] nodes) => collection.AddRangeBefore(node, (System.Collections.Generic.IEnumerable<System.Collections.Generic.LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple values before a specified node in a <see cref="System.Collections.Generic.LinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="array">The values to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        public static void AddRangeBefore<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            foreach (System.Collections.Generic.LinkedListNode<T> item in array)

                collection.AddBefore(node, item);
        }

        /// <summary>
        /// Add multiple values after a specified node in a <see cref="System.Collections.Generic.LinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="values">The values to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeAfter<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, params T[] values)
        {
            ThrowIfNull(node, nameof(node));

            return node.Next == null ? collection.AddRangeLast(values) : collection.AddRangeBefore(node.Next, values);
        }

        /// <summary>
        /// Add multiple values after a specified node in a <see cref="System.Collections.Generic.LinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="array">The values to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeAfter<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(node, nameof(node));

            return node.Next == null ? collection.AddRangeLast(array) : collection.AddRangeBefore(node.Next, array);
        }

        /// <summary>
        /// Add multiple values after a specified node in a <see cref="System.Collections.Generic.LinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="nodes">The values to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        public static void AddRangeAfter<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, params System.Collections.Generic.LinkedListNode<T>[] nodes)
        {
            ThrowIfNull(node, nameof(node));

            if (node.Next == null)

                collection.AddRangeLast(nodes);

            else

                collection.AddRangeBefore(node.Next, nodes);
        }

        /// <summary>
        /// Add multiple values after a specified node in a <see cref="System.Collections.Generic.LinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="array">The values to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        public static void AddRangeAfter<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
        {
            ThrowIfNull(node, nameof(node));

            if (node.Next == null)

                collection.AddRangeLast(array);

            else

                collection.AddRangeBefore(node.Next, array);
        }

#if !WinCopies3
        /// <summary>
        /// Add multiple <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s at the top of a <see cref="ILinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="nodes">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="ILinkedList{T}"/></param>
        public static void AddRangeFirst<T>(this ILinkedList<T> collection, params System.Collections.Generic.LinkedListNode<T>[] nodes)
        {
            ThrowIfNull(collection, nameof(collection));

            if (collection.First == null)

                collection.AddRangeLast(nodes);

            else

                collection.AddRangeBefore(collection.First, nodes);
        }

        /// <summary>
        /// Add multiple <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s at the top of a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="array">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="ILinkedList{T}"/></param>
        public static void AddRangeFirst<T>(this ILinkedList<T> collection, in IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
        {
            ThrowIfNull(collection, nameof(collection));

            if (collection.First == null)

                collection.AddRangeLast(array);

            else

                collection.AddRangeBefore(collection.First, array);
        }

        /// <summary>
        /// Add multiple <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s at the end of a <see cref="ILinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="nodes">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static void AddRangeLast<T>(this ILinkedList<T> collection, params System.Collections.Generic.LinkedListNode<T>[] nodes) => collection.AddRangeLast((IEnumerable<System.Collections.Generic.LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s at the end of a <see cref="ILinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="array">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="ILinkedList{T}"/></param>
        public static void AddRangeLast<T>(this ILinkedList<T> collection, in IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            foreach (System.Collections.Generic.LinkedListNode<T> item in array)

                collection.AddLast(item);
        }

        /// <summary>
        /// Add multiple values before a specified node in a <see cref="ILinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="nodes">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="ILinkedList{T}"/></param>
        public static void AddRangeBefore<T>(this ILinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, params System.Collections.Generic.LinkedListNode<T>[] nodes) => collection.AddRangeBefore(node, (IEnumerable<System.Collections.Generic.LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple values before a specified node in a <see cref="ILinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="array">The values to add to a <see cref="ILinkedList{T}"/></param>
        public static void AddRangeBefore<T>(this ILinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, in IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            foreach (System.Collections.Generic.LinkedListNode<T> item in array)

                collection.AddBefore(node, item);
        }
#endif

#if CS7
        public static void AddRange(this IList collection, in IEnumerable array, in int start, in int length) => collection.AddRange(array.ToArray(), start, length);

        public static void AddRange<T>(this ICollection<T> collection, params T[] values) => collection.AddRange((System.Collections.Generic.IEnumerable<T>)values);

        /// <summary>
        /// Add multiple values after a specified node in a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="values">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>[] AddRangeAfter<T>(this ILinkedList<T> collection, in
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T> node, params T[] values)
        {
            ThrowIfNull(node, nameof(node));

            return node.Next == null ? collection.AddRangeLast(values) : collection.AddRangeBefore(node.Next, values);
        }

        /// <summary>
        /// Add multiple values after a specified node in a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="array">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>[] AddRangeAfter<T>(this ILinkedList<T> collection, in
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T> node, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(node, nameof(node));

            return node.Next == null ? collection.AddRangeLast(array) : collection.AddRangeBefore(node.Next, array);
        }

        /// <summary>
        /// Add multiple values at the top of a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="values">The values to add to this <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>[] AddRangeFirst<T>(this ILinkedList<T> collection, params T[] values)
        {
            ThrowIfNull(collection, nameof(collection));

            return collection.First == null ? collection.AddRangeLast(values) : collection.AddRangeBefore(collection.First, values);
        }

        /// <summary>
        /// Add multiple values at the top of a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="array">The values to add to this <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>[] AddRangeFirst<T>(this ILinkedList<T> collection, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(collection, nameof(collection));

            return collection.First == null ? collection.AddRangeLast(array) : collection.AddRangeBefore(collection.First, array);
        }

        /// <summary>
        /// Add multiple values at the end of a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="values">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>[] AddRangeLast<T>(this ILinkedList<T> collection, params T[] values) => collection.AddRangeLast((System.Collections.Generic.IEnumerable<T>)values);

        /// <summary>
        /// Add multiple values at the end of a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="array">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>[] AddRangeLast<T>(this ILinkedList<T> collection, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            var result = new DotNetFix.Generic.EnumerableQueue<
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>>();

            foreach (T item in array)

                result.Enqueue(collection.AddLast(item));

            return result.ToArray<
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>>();
        }

        /// <summary>
        /// Add multiple values before a specified node in a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="values">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>[] AddRangeBefore<T>(this ILinkedList<T> collection, in
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T> node, params T[] values) => collection.AddRangeBefore(node, (System.Collections.Generic.IEnumerable<T>)values);

        /// <summary>
        /// Add multiple values before a specified node in a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="array">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>[] AddRangeBefore<T>(this ILinkedList<T> collection, in
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T> node, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            var result = new EnumerableQueue<
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>>();

            foreach (T item in array)

                result.Enqueue(collection.AddBefore(node, item));

            return result.ToArray<
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T>>();
        }
#endif

#if !WinCopies3
        /// <summary>
        /// Add multiple values after a specified node in a <see cref="ILinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="nodes">The values to add to a <see cref="ILinkedList{T}"/></param>
        public static void AddRangeAfter<T>(this ILinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, params System.Collections.Generic.LinkedListNode<T>[] nodes)
        {
            ThrowIfNull(node, nameof(node));

            if (node.Next == null)

                collection.AddRangeLast(nodes);

            else

                collection.AddRangeBefore(node.Next, nodes);
        }

        /// <summary>
        /// Add multiple values after a specified node in a <see cref="ILinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="array">The values to add to a <see cref="ILinkedList{T}"/></param>
        public static void AddRangeAfter<T>(this ILinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, in IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
        {
            ThrowIfNull(node, nameof(node));

            if (node.Next == null)

                collection.AddRangeLast(array);

            else

                collection.AddRangeBefore(node.Next, array);
        }
#endif
        #endregion

        public static object[] ToArray(this IList arrayList, in int startIndex, in int length)
        {
            ThrowIfNull(arrayList, nameof(arrayList));

            object[] array = new object[length];

            int i;

            for (i = 0; i < length; i++)

                array[i] = arrayList[i + startIndex];

            return array;
        }

        public static T[] ToArray<T>(this System.Collections.Generic.IList<T> arrayList, in int startIndex, in int length)
        {
            ThrowIfNull(arrayList, nameof(arrayList));

            var array = new T[length];

            int i;

            for (i = 0; i < length; i++)

                array[i] = arrayList[i + startIndex];

            return array;
        }

        // todo: add null checks, out-of-range checks, ...

        // todo: add same methods for arrays

        /// <summary>
        /// Removes multiple items in an <see cref="IList"/> collection, from a given start index for a given length.
        /// </summary>
        /// <param name="collection">The collection from which remove the items.</param>
        /// <param name="start">The start index in the collection from which delete the items.</param>
        /// <param name="length">The length to remove.</param>
        public static void RemoveRange(this IList collection, in int start, in int length)
        {
            ThrowIfNull(collection, nameof(collection));

            for (int i = 0; i < length; i++)

                collection.RemoveAt(start);
        }

        /// <summary>
        /// Sorts an <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the values in the <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.</typeparam>
        /// <param name="oc">The <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> to sort.</param>
        public static void Sort<T>(this System.Collections.ObjectModel.ObservableCollection<T> oc)
        {
            ThrowIfNull(oc, nameof(oc));

            System.Collections.Generic.IList<T> sorted = oc.OrderBy(x => x).ToList<T>();

            for (int i = 0; i < sorted.Count; i++)

                oc.Move(oc.IndexOf(sorted[i]), i);
        }

        /// <summary>
        /// Sorts an <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> with a user-defined comparer.
        /// </summary>
        /// <typeparam name="T">The type of the values in the <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.</typeparam>
        /// <param name="oc">The <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/> to sort.</param>
        /// <param name="comparer">An <see cref="System.Collections.Generic.IComparer{T}"/> providing comparison for sorting the <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.</param>
        public static void Sort<T>(this System.Collections.ObjectModel.ObservableCollection<T> oc, in System.Collections.Generic.IComparer<T> comparer)
        {
            ThrowIfNull(oc, nameof(oc));

            System.Collections.Generic.IList<T> sorted = oc.OrderBy(x => x, comparer).ToList<T>();

            for (int i = 0; i < sorted.Count; i++)

                oc.Move(oc.IndexOf(sorted[i]), i);
        }

        #region Contains methods

        #region Non generic methods

        #region ContainsOneValue overloads

        public static bool ContainsOneValue(this IEnumerable array, in EqualityComparison comparison, out bool containsMoreThanOneValue, in object[] values)
        {
            ThrowIfNull(array, nameof(array));

            bool matchFound = false;

            foreach (object value in array)

                foreach (object _value in values)

                    if (comparison(value, _value))
                    {
                        if (matchFound)
                        {
                            containsMoreThanOneValue = true;

                            return false;
                        }

                        matchFound = true;
                    }

            containsMoreThanOneValue = false;

            return matchFound;
        }

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue(this IEnumerable array, out bool containsMoreThanOneValue, params object[] values) => ContainsOneValue(array, (object value, object _value) => object.Equals(value, _value), out containsMoreThanOneValue, values);

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="System.Collections.IComparer"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue(this IEnumerable array, System.Collections.IComparer comparer, out bool containsMoreThanOneValue, params object[] values)
        {
            ThrowIfNull(comparer, nameof(comparer));

            return ContainsOneValue(array, (object value, object _value) => comparer.Compare(value, _value) == 0, out containsMoreThanOneValue, values);
        }

#if !WinCopies3
        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        [Obsolete("This method has been replaced by the overload with the comparison parameter from WinCopies.Collections.Comparison.")]
        public static bool ContainsOneValue(this IEnumerable array, Comparison<object> comparison, out bool containsMoreThanOneValue, params object[] values) => ContainsOneValue(array, new WinCopies.Collections.Comparison((object x, object y) => comparison(x, y)), out containsMoreThanOneValue, values);
#endif

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="WinCopies.Collections.Comparison"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue(this IEnumerable array, WinCopies.Collections.Comparison comparison, out bool containsMoreThanOneValue, params object[] values)
        {
            ThrowIfNull(comparison, nameof(comparison));

            return ContainsOneValue(array, (object value, object _value) => comparison(value, _value) == 0, out containsMoreThanOneValue, values);
        }

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom equality comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue(this IEnumerable array, IEqualityComparer equalityComparer, out bool containsMoreThanOneValue, params object[] values)
        {
            ThrowIfNull(equalityComparer, nameof(equalityComparer));

            return ContainsOneValue(array, (object value, object _value) => equalityComparer.Equals(value, _value), out containsMoreThanOneValue, values);
        }
        #endregion

        #region ContainsOneOrMoreValues with notification whether contains more than one values overloads

        public static bool ContainsOneOrMoreValues(IEnumerable array, in EqualityComparison comparison, out bool containsMoreThanOneValue, object[] values)
        {
            ThrowIfNull(array, nameof(array));

            bool matchFound = false;

            foreach (object value in array)

                foreach (object _value in values)

                    if (comparison(value, _value))
                    {
                        if (matchFound)
                        {
                            containsMoreThanOneValue = true;

                            return true;
                        }

                        matchFound = true;
                    }

            containsMoreThanOneValue = false;

            return matchFound;
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, out bool containsMoreThanOneValue, params object[] values) => ContainsOneOrMoreValues(array, (object value, object _value) => object.Equals(value, _value), out containsMoreThanOneValue, values);

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="System.Collections.IComparer"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, System.Collections.IComparer comparer, out bool containsMoreThanOneValue, params object[] values)
        {
            ThrowIfNull(comparer, nameof(comparer));

            return ContainsOneOrMoreValues(array, (object value, object _value) => comparer.Compare(value, _value) == 0, out containsMoreThanOneValue, values);
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, Comparison<object> comparison, out bool containsMoreThanOneValue, params object[] values)
        {
            ThrowIfNull(comparison, nameof(comparison));

            return ContainsOneOrMoreValues(array, (object value, object _value) => comparison(value, _value) == 0, out containsMoreThanOneValue, values);
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom equality comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, IEqualityComparer equalityComparer, out bool containsMoreThanOneValue, params object[] values)
        {
            ThrowIfNull(equalityComparer, nameof(equalityComparer));

            return ContainsOneOrMoreValues(array, (object value, object _value) => equalityComparer.Equals(value, _value), out containsMoreThanOneValue, values);
        }
        #endregion

        #region ContainsOneOrMoreValues without notification whether contains more than one values overloads

        public static bool ContainsOneOrMoreValues(IEnumerable array, in Func<object, object, bool> comparison, object[] values)
        {
            ThrowIfNull(array, nameof(array));

            foreach (object value in array)

                foreach (object _value in values)

                    if (comparison(value, _value))

                        return true;

            return false;
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, params object[] values) => ContainsOneOrMoreValues(array, (object value, object _value) => object.Equals(value, _value), values);

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="System.Collections.IComparer"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, System.Collections.IComparer comparer, params object[] values)
        {
            ThrowIfNull(comparer, nameof(comparer));

            return ContainsOneOrMoreValues(array, (object value, object _value) => comparer.Compare(value, _value) == 0, values);
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, Comparison<object> comparison, params object[] values)
        {
            ThrowIfNull(comparison, nameof(comparison));

            return ContainsOneOrMoreValues(array, (object value, object _value) => comparison(value, _value) == 0, values);
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues(this IEnumerable array, IEqualityComparer equalityComparer, params object[] values)
        {
            ThrowIfNull(equalityComparer, nameof(equalityComparer));

            return ContainsOneOrMoreValues(array, (object value, object _value) => equalityComparer.Equals(value, _value), values);
        }
        #endregion

        #region Contains array overloads

        public static bool Contains(IEnumerable array, in EqualityComparison comparison, object[] values)
        {
            ThrowIfNull(array, nameof(array));

            bool matchFound;

            foreach (object value in array)
            {
                matchFound = false;

                foreach (object _value in values)

                    if (comparison(value, _value))
                    {
                        matchFound = true;

                        break;
                    }

                if (!matchFound)

                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether an array contains all values of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains(this IEnumerable array, params object[] values) => Contains(array, (object value, object _value) => object.Equals(value, _value), values);

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="System.Collections.IComparer"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains(this IEnumerable array, System.Collections.IComparer comparer, params object[] values)
        {
            ThrowIfNull(comparer, nameof(comparer));

            return Contains(array, (object value, object _value) => comparer.Compare(value, _value) == 0, values);
        }

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains(this IEnumerable array, Comparison<object> comparison, params object[] values)
        {
            ThrowIfNull(comparison, nameof(comparison));

            return Contains(array, (object value, object _value) => comparison(value, _value) == 0, values);
        }

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains(this IEnumerable array, IEqualityComparer equalityComparer, params object[] values)
        {
            ThrowIfNull(equalityComparer, nameof(equalityComparer));

            return Contains(array, (object value, object _value) => equalityComparer.Equals(value, _value), values);
        }
        #endregion

        #endregion

        #region Generic methods

        #region ContainsOneValue overloads

        public static bool ContainsOneValue<T>(System.Collections.Generic.IEnumerable<T> array, in EqualityComparison<T> comparison, out bool containsMoreThanOneValue, in T[] values)
        {
            ThrowIfNull(array, nameof(array));

            bool matchFound = false;

            foreach (T value in array)

                foreach (T _value in values)

                    if (comparison(value, _value))
                    {
                        if (matchFound)
                        {
                            containsMoreThanOneValue = true;

                            return false;
                        }
                        matchFound = true;
                    }

            containsMoreThanOneValue = false;

            return matchFound;
        }

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue<T>(this System.Collections.Generic.IEnumerable<T> array, out bool containsMoreThanOneValue, params T[] values) => ContainsOneValue(array, (T value, T _value) => object.Equals(value, _value), out containsMoreThanOneValue, values);

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="System.Collections.Generic.IComparer{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue<T>(this System.Collections.Generic.IEnumerable<T> array, System.Collections.Generic.IComparer<T> comparer, out bool containsMoreThanOneValue, params T[] values)
        {
            ThrowIfNull(comparer, nameof(comparer));

            return ContainsOneValue(array, (T value, T _value) => comparer.Compare(value, _value) == 0, out containsMoreThanOneValue, values);
        }

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue<T>(this System.Collections.Generic.IEnumerable<T> array, Comparison<T> comparison, out bool containsMoreThanOneValue, params T[] values)
        {
            ThrowIfNull(comparison, nameof(comparison));

            return ContainsOneValue(array, (T value, T _value) => comparison(value, _value) == 0, out containsMoreThanOneValue, values);
        }

        /// <summary>
        /// Checks whether an array contains <i>exactly</i> one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="System.Collections.Generic.IEqualityComparer{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if <i>exactly</i> one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneValue<T>(this System.Collections.Generic.IEnumerable<T> array, System.Collections.Generic.IEqualityComparer<T> equalityComparer, out bool containsMoreThanOneValue, params T[] values)
        {
            ThrowIfNull(equalityComparer, nameof(equalityComparer));

            return ContainsOneValue(array, (T value, T _value) => equalityComparer.Equals(value, _value), out containsMoreThanOneValue, values); ;
        }
        #endregion

        #region ContainsOneOrMoreValues with notification whether contains more than one values overloads

        public static bool ContainsOneOrMoreValues<T>(System.Collections.Generic.IEnumerable<T> array, in EqualityComparison<T> comparison, out bool containsMoreThanOneValue, in T[] values)
        {
            ThrowIfNull(array, nameof(array));

            bool matchFound = false;

            foreach (T value in array)

                foreach (T _value in values)

                    if (comparison(value, _value))
                    {
                        if (matchFound)
                        {
                            containsMoreThanOneValue = true;

                            return true;
                        }

                        matchFound = true;
                    }

            containsMoreThanOneValue = false;

            return matchFound;
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this System.Collections.Generic.IEnumerable<T> array, out bool containsMoreThanOneValue, params T[] values) => ContainsOneOrMoreValues(array, (T value, T _value) => object.Equals(value, _value), out containsMoreThanOneValue, values);

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="System.Collections.Generic.IComparer{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this System.Collections.Generic.IEnumerable<T> array, System.Collections.Generic.IComparer<T> comparer, out bool containsMoreThanOneValue, params T[] values)
        {
            ThrowIfNull(comparer, nameof(comparer));

            return ContainsOneOrMoreValues(array, (T value, T _value) => comparer.Compare(value, _value) == 0, out containsMoreThanOneValue, values);
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this System.Collections.Generic.IEnumerable<T> array, Comparison<T> comparison, out bool containsMoreThanOneValue, params T[] values)
        {
            ThrowIfNull(comparison, nameof(comparison));

            return ContainsOneOrMoreValues(array, (T value, T _value) => comparison(value, _value) == 0, out containsMoreThanOneValue, values);
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="System.Collections.Generic.IEqualityComparer{T}"/> used to compare the values</param>
        /// <param name="containsMoreThanOneValue"><see langword="true"/> if more than one value has been found, otherwise <see langword="false"/></param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this System.Collections.Generic.IEnumerable<T> array, System.Collections.Generic.IEqualityComparer<T> equalityComparer, out bool containsMoreThanOneValue, params T[] values)
        {
            ThrowIfNull(equalityComparer, nameof(equalityComparer));

            return ContainsOneOrMoreValues(array, (T value, T _value) => equalityComparer.Equals(value, _value), out containsMoreThanOneValue, values);
        }
        #endregion

        #region ContainsOneOrMoreValues without notification whether contains more than one values overloads

        public static bool ContainsOneOrMoreValues<T>(System.Collections.Generic.IEnumerable<T> array, in EqualityComparison<T> comparison, in T[] values)
        {
            ThrowIfNull(array, nameof(array));

            foreach (T value in array)

                foreach (T _value in values)

                    if (comparison(value, _value))

                        return true;

            return false;
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this System.Collections.Generic.IEnumerable<T> array, params T[] values) => ContainsOneOrMoreValues(array, (T value, T _value) => object.Equals(value, _value), values);

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="System.Collections.Generic.IComparer{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this System.Collections.Generic.IEnumerable<T> array, System.Collections.Generic.IComparer<T> comparer, params T[] values)
        {
            ThrowIfNull(comparer, nameof(comparer));

            return ContainsOneOrMoreValues(array, (T value, T _value) => comparer.Compare(value, _value) == 0, values);
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this System.Collections.Generic.IEnumerable<T> array, Comparison<T> comparison, params T[] values)
        {
            ThrowIfNull(comparison, nameof(comparison));

            return ContainsOneOrMoreValues(array, (T value, T _value) => comparison(value, _value) == 0, values);
        }

        /// <summary>
        /// Checks whether an array contains at least one value of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="System.Collections.Generic.IEqualityComparer{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool ContainsOneOrMoreValues<T>(this System.Collections.Generic.IEnumerable<T> array, System.Collections.Generic.IEqualityComparer<T> equalityComparer, params T[] values)
        {
            ThrowIfNull(equalityComparer, nameof(equalityComparer));

            return ContainsOneOrMoreValues(array, (T value, T _value) => equalityComparer.Equals(value, _value), values);
        }
        #endregion

        #region Contains array overloads

        public static bool Contains<T>(System.Collections.Generic.IEnumerable<T> array, in EqualityComparison<T> comparison, in T[] values)
        {
            ThrowIfNull(array, nameof(array));

            bool matchFound;

            foreach (T value in array)
            {
                matchFound = false;

                foreach (T _value in values)

                    if (comparison(value, _value))
                    {
                        matchFound = true;

                        break;
                    }

                if (!matchFound)

                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether an array contains all values of a given array.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, params T[] values) => Contains(array, (T value, T _value) => object.Equals(value, _value), values);

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparer">The <see cref="System.Collections.Generic.IComparer{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, System.Collections.Generic.IComparer<T> comparer, params T[] values)
        {
            ThrowIfNull(comparer, nameof(comparer));

            return Contains(array, (T value, T _value) => comparer.Compare(value, _value) == 0, values);
        }

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, Comparison<T> comparison, params T[] values)
        {
            ThrowIfNull(comparison, nameof(comparison));

            return Contains(array, (T value, T _value) => comparison(value, _value) == 0, values);
        }

        /// <summary>
        /// Checks whether an array contains all values of a given array using a custom comparer.
        /// </summary>
        /// <param name="array">The array to browse</param>
        /// <param name="equalityComparer">The <see cref="System.Collections.Generic.IEqualityComparer{T}"/> used to compare the values</param>
        /// <param name="values">The values to compare</param>
        /// <returns><see langword="true"/> if at least one value has been found, otherwise <see langword="false"/>.</returns>
        public static bool Contains<T>(this System.Collections.Generic.IEnumerable<T> array, System.Collections.Generic.IEqualityComparer<T> equalityComparer, params T[] values)
        {
            ThrowIfNull(equalityComparer, nameof(equalityComparer));

            return Contains(array, (T value, T _value) => equalityComparer.Equals(value, _value), values);
        }
        #endregion

        #endregion

        #endregion

        public static System.Collections.Generic.LinkedListNode<T> RemoveAndGetFirstValue<T>(this System.Collections.Generic.LinkedList<T> items)
        {
            System.Collections.Generic.LinkedListNode<T> value = (items ?? throw GetArgumentNullException(nameof(items))).First;

            items.RemoveFirst();

            return value;
        }

#if CS7
        public static
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T> RemoveAndGetFirstValue<T>(this ILinkedList<T> items)
        {

#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T> value = (items ?? throw GetArgumentNullException(nameof(items))).First;

            items.RemoveFirst();

            return value;
        }
#endif

        public static System.Collections.Generic.LinkedListNode<T> RemoveAndGetLastValue<T>(this System.Collections.Generic.LinkedList<T> items)
        {
            System.Collections.Generic.LinkedListNode<T> value = (items ?? throw GetArgumentNullException(nameof(items))).Last;

            items.RemoveLast();

            return value;
        }

#if CS7
        public static
#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T> RemoveAndGetLastValue<T>(this ILinkedList<T> items)
        {

#if !WinCopies3
            System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
            <T> value = (items ?? throw GetArgumentNullException(nameof(items))).Last;

            items.RemoveLast();

            return value;
        }
#endif

        public static bool RemoveAll<T>(this IList<T> collection, in T itemToKeep, in bool onlyOne, in bool throwIfMultiple) where T : class
        {
            while (collection.Count != 1)
            {
                if (collection[0] == itemToKeep)
                {
                    if (onlyOne)
                    {
                        while (collection.Count != 1)
                        {
                            if (collection[1] == itemToKeep)
                            {
                                if (throwIfMultiple)

                                    throw GetMoreThanOneOccurencesWereFoundException();

                                else

                                    while (collection.Count != 1)

                                        collection.RemoveAt(1);

                                return false;
                            }

                            collection.RemoveAt(1);

                            return true;
                        }
                    }

                    while (collection.Count != 1)

                        collection.RemoveAt(1);

                    return true;
                }

                collection.RemoveAt(0);
            }

            return false;
        }

        public static bool RemoveAllEquatable<T>(this IList<T> collection, in T itemToKeep, in bool onlyOne, in bool throwIfMultiple) where T : IEquatable<T>
        {
            while (collection.Count != 1)
            {
                if (collection[0]?.Equals(itemToKeep) == true)
                {
                    if (onlyOne)
                    {
                        while (collection.Count != 1)
                        {
                            if (collection[1]?.Equals(itemToKeep) == true)
                            {
                                if (throwIfMultiple)

                                    throw GetMoreThanOneOccurencesWereFoundException();

                                else

                                    while (collection.Count != 1)

                                        collection.RemoveAt(1);

                                return false;
                            }

                            collection.RemoveAt(1);

                            return true;
                        }
                    }

                    while (collection.Count != 1)

                        collection.RemoveAt(1);

                    return true;
                }

                collection.RemoveAt(0);
            }

            return false;
        }

        public static bool RemoveAll<T>(this IList<T> collection, in T itemToKeep, in Comparison<T> comparison, in bool onlyOne, in bool throwIfMultiple)
        {
            while (collection.Count != 1)

            {
                if (comparison(collection[0], itemToKeep) == 0)
                {
                    if (onlyOne)
                    {
                        while (collection.Count != 1)
                        {
                            if (comparison(collection[1], itemToKeep) == 0)
                            {
                                if (throwIfMultiple)

                                    throw GetMoreThanOneOccurencesWereFoundException();

                                else

                                    while (collection.Count != 1)

                                        collection.RemoveAt(1);

                                return false;
                            }

                            collection.RemoveAt(1);

                            return true;

                        }
                    }

                    while (collection.Count != 1)

                        collection.RemoveAt(1);

                    return true;
                }

                collection.RemoveAt(0);
            }

            return false;
        }

        public static bool RemoveAll<T>(this IList<T> collection, in T itemToKeep, in System.Collections.Generic.IComparer<T> comparer, in bool onlyOne, in bool throwIfMultiple)
        {
            while (collection.Count != 1)
            {

                if (comparer.Compare(collection[0], itemToKeep) == 0)
                {

                    if (onlyOne)
                    {

                        while (collection.Count != 1)
                        {

                            if (comparer.Compare(collection[1], itemToKeep) == 0)
                            {

                                if (throwIfMultiple)

                                    throw GetMoreThanOneOccurencesWereFoundException();

                                else

                                    while (collection.Count != 1)

                                        collection.RemoveAt(1);

                                return false;
                            }

                            collection.RemoveAt(1);

                            return true;

                        }
                    }

                    while (collection.Count != 1)

                        collection.RemoveAt(1);

                    return true;
                }

                collection.RemoveAt(0);
            }

            return false;
        }

        #endregion

#if CS7
        public static
#if !WinCopies3
int 
#else
            uint
#endif
            GetCapacity<T>(this ArrayBuilder<T>[] arrayBuilders) => (arrayBuilders ?? throw GetArgumentNullException(nameof(arrayBuilders))).GetCapacityInternal();

        private static
#if !WinCopies3
            int
#else
            uint
#endif
            GetCapacityInternal<T>(this ArrayBuilder<T>[] arrayBuilders)
        {
#if !WinCopies3
int
#else
            uint
#endif
                capacity = 0;

            foreach (ArrayBuilder<T> arrayBuilder in arrayBuilders)

                capacity += arrayBuilder.Count;

            return capacity;
        }

        public static T[] ToArray<T>(this ArrayBuilder<T>[] arrayBuilders, in bool remove = false)
        {
            ThrowIfNull(arrayBuilders, nameof(arrayBuilders));

            var items = new T[arrayBuilders.GetCapacityInternal()];

            foreach (ArrayBuilder<T> arrayBuilder in arrayBuilders)

                arrayBuilder.ToArray(items, remove);

            return items;
        }

        public static ArrayList ToArrayList<T>(this ArrayBuilder<T>[] arrayBuilders, in bool remove = false)
        {
            ThrowIfNull(arrayBuilders, nameof(arrayBuilders));

            var items = new ArrayList(
#if WinCopies3
                (int)
#endif
                arrayBuilders.GetCapacityInternal());

            foreach (ArrayBuilder<T> arrayBuilder in arrayBuilders)

                arrayBuilder.ToArrayList(items, remove);

            return items;
        }

        public static List<T> ToList<T>(this ArrayBuilder<T>[] arrayBuilders, in bool remove = false)
        {
            ThrowIfNull(arrayBuilders, nameof(arrayBuilders));

            var items = new List<T>(
#if WinCopies3
                (int)
#endif
                arrayBuilders.GetCapacityInternal());

            foreach (ArrayBuilder<T> arrayBuilder in arrayBuilders)

                arrayBuilder.ToList(items, remove);

            return items;
        }
#endif

        public static void ForEach(this IEnumerableEnumerator enumerator, LoopIteration func)
        {
            while (enumerator.MoveNext())

                if (func(enumerator.Current))

                    break;
        }

        public static void ForEach<T>(this IEnumerableEnumerator<T> enumerator, LoopIteration<T> func)
        {
            try
            {
                while (enumerator.MoveNext())

                    if (func(enumerator.Current))

                        break;
            }
            finally
            {
                enumerator.Dispose();
            }
        }

        public static bool ForEach(this EmptyCheckEnumerator enumerator, LoopIteration func)
        {
            if (enumerator.HasItems)
            {
                while (enumerator.MoveNext())

                    if (func(enumerator.Current))

                        break;

                return true;
            }

            return false;
        }

        public static bool ForEach<T>(this EmptyCheckEnumerator<T> enumerator, LoopIteration<T> func)
        {
            try
            {
                if (enumerator.HasItems)
                {
                    while (enumerator.MoveNext())

                        if (func(enumerator.Current))

                            break;

                    return true;
                }

                return false;
            }
            finally
            {
                enumerator.Dispose();
            }
        }

#if CS7
        ///// <summary>
        ///// Determines whether the current enum value is within the enum values range.
        ///// </summary>
        ///// <param name="enum">The enum value to check.</param>
        ///// <param name="throwIfNotFlagsEnum">Whether to throw if the given enum does not have the <see cref="FlagsAttribute"/> attribute.</param>
        ///// <param name="throwIfZero">Whether to throw if the given enum is zero.</param>
        ///// <returns><see langword="true"/> if the given value is in the enum values range, otherwise <see langword="false"/>.</returns>
        ///// <exception cref="ArgumentException"><paramref name="enum"/> does not have the <see cref="FlagsAttribute"/> and <paramref name="throwIfNotFlagsEnum"/> is set to <see langword="true"/>.</exception>
        ///// <exception cref="InvalidEnumArgumentException"><paramref name="enum"/> is equal to zero and the <paramref name="throwIfZero"/> parameter is set to true or <paramref name="enum"/> is lesser than zero.</exception>
        ///// <seealso cref="ThrowIfNotValidFlagsEnumValue(Enum, in bool, in bool)"/>
        ///// <seealso cref="ThrowIfNotValidFlagsEnumValue(Enum, in string, in bool, in bool)"/>
        public static bool IsValidFlagsEnumValue(this Enum @enum, in bool throwIfNotFlagsEnum, in bool throwIfZero)
        {
            Type enumType = (@enum ?? throw GetArgumentNullException(nameof(@enum))).GetType();

            var enumComparer = new EnumComparer();

            int comparisonResult = enumComparer.CompareToObject(@enum, 0);

            object value = @enum.GetNumValue();

            // If the value is lesser than zero, this is not a flags enum value.

            if (comparisonResult < 0 || (comparisonResult == 0 && throwIfZero))

                throw new InvalidEnumArgumentException("The given value must be greater than zero if the 'throwIfZero' parameter is set to true, or greater or equal to zero otherwise.", nameof(@enum), value is long __value ? __value : (int)value, enumType);

            if (!@enum.IsFlagsEnum())

                return throwIfNotFlagsEnum ? throw GetExceptionForNonFlagsEnum(nameof(@enum)) : false;

            // Now, we have to check if the given value is directly defined in the enum.

            if (enumType.IsEnumDefined(@enum))

                return true;

            // If not, we have to check if the given value is a power of 2.

            double valueDouble = (double)Convert.ChangeType(value, TypeCode.Double);

            // If yes and if we reached this point, that means that the value is a power of 2 -- and therefore represents a flag in the enum --, but is not defined in the enum.

            double log = System.Math.Log(valueDouble, 2);

            if (System.Math.Truncate(log) == log) return false;

            // If not, we have to check if all the flags represented by the given value are actually set in the enum.

            double _value = System.Math.Pow(2, System.Math.Ceiling(log));

            double valueToCheck;

            do
            {
                valueToCheck = _value - valueDouble;

                if (!enumType.IsEnumDefined(Enum.ToObject(enumType, valueToCheck))) return false;

                valueDouble -= valueToCheck;
            }

            while (valueDouble > 0);

            return true;
        }
#endif
    }
}
#endif
