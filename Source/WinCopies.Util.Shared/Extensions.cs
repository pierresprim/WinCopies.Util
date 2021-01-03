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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

#if WinCopies2
using System.Globalization;

using WinCopies.Collections;
using WinCopies.Collections.DotNetFix;
using WinCopies.Util.Resources;

using static WinCopies.Util.Util;
using static WinCopies.Util.ThrowHelper;

using IfCT = WinCopies.Util.Util.ComparisonType;

namespace WinCopies.Util
#else
using static WinCopies.UtilHelpers;
using static WinCopies.ThrowHelper;

namespace WinCopies
#endif
{
    /// <summary>
    /// This delegate represents the action that is performed for each iteration of a loop.
    /// </summary>
    /// <param name="obj">The object or value retrieved by the current iteration.</param>
    /// <returns><see langword="true"/> to break the loop; otherwise <see langword="false"/>.</returns>
    public delegate bool LoopIteration(object obj);

    /// <summary>
    /// This delegate represents the action that is performed for each iteration of a loop.
    /// </summary>
    /// <typeparam name="T">The type of the object or value that is retrieved.</typeparam>
    /// <param name="obj">The object or value retrieved by the current iteration.</param>
    /// <returns><see langword="true"/> to break the loop; otherwise <see langword="false"/>.</returns>
    public delegate bool LoopIteration<T>(T obj);

    public interface ISplitFactory<T, U, TContainer>
    {
        TContainer Container { get; }

        int SubCount { get; }

        void SubClear();
    }

    public interface IValueSplitFactory<T, U, TContainer> : ISplitFactory<T, U, TContainer>
    {
        void Add(U enumerable);

        U GetEnumerable();

        void SubAdd(T value);
    }

    public interface IRefSplitFactory<T, U, V, TContainer> : ISplitFactory<T, U, TContainer> where T : class
    {
        void Add(V enumerable);

        V GetEnumerable();

        void SubAdd(U value);

        U GetValueContainer(T value);
    }

#if CS7
    public delegate (bool result, Exception ex) FieldValidateValueCallback(object obj, object value, FieldInfo field, string paramName);
#endif

    public delegate void FieldValueChangedCallback(object obj, object value, FieldInfo field, string paramName);

#if CS7
    public delegate (bool result, Exception ex) PropertyValidateValueCallback(object obj, object value, PropertyInfo property, string paramName);
#endif

    public delegate void PropertyValueChangedCallback(object obj, object value, PropertyInfo property, string paramName);

#if WinCopies3
    namespace Util // To avoid name conflicts.
    {
#endif
        /// <summary>
        /// Provides some static extension methods.
        /// </summary>
        public static class Extensions
        {
#if WinCopies2
        private static void ThrowOnInvalidCopyToArrayParameters(in IEnumerable enumerable, in Array array)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(array, nameof(array));
        }

        public static void CopyTo(this IEnumerable enumerable, in Array array, in int arrayIndex, in int count)
        {
            ThrowOnInvalidCopyToArrayParameters(enumerable, array);
            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, count, nameof(array), nameof(arrayIndex));

            int i = -1;

            foreach (object value in enumerable)

                array.SetValue(value, ++i);
        }

        public static void CopyTo(this IEnumerable enumerable, in Array array, in int arrayIndex, in uint count)
        {
            ThrowOnInvalidCopyToArrayParameters(enumerable, array);
            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, count, nameof(array), nameof(arrayIndex));

            int i = -1;

            foreach (object value in enumerable)

                array.SetValue(value, ++i);
        }

        public static void CopyTo<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in T[] array, in int arrayIndex, in int count)
        {
            ThrowOnInvalidCopyToArrayParameters(enumerable, array);
            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, count, nameof(array), nameof(arrayIndex));

            int i = -1;

            foreach (object value in enumerable)

                array.SetValue(value, ++i);
        }

        public static void CopyTo<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in T[] array, in int arrayIndex, in uint count)
        {
            ThrowOnInvalidCopyToArrayParameters(enumerable, array);
            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, count, nameof(array), nameof(arrayIndex));

            int i = -1;

            foreach (object value in enumerable)

                array.SetValue(value, ++i);
        }

        public static bool IsEnumeratorNotStartedOrDisposed(this IDisposableEnumeratorInfo enumerator) => (enumerator ?? throw GetArgumentNullException(nameof(enumerator))).IsDisposed || !enumerator.IsStarted;

        /// <summary>
        /// Tries to add multiple values to an <see cref="System.Collections.ICollection"/> if it does not contain them already.
        /// </summary>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="values">The values to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        [Obsolete("This method has been replaced by the AddRangeIfNotContains(this System.Collections.IList collection, params object[] values) method.")]
        public static object[] AddRangeIfNotContains(this System.Collections.ICollection collection, params object[] values) => collection.AddRangeIfNotContains((IEnumerable)values);

#if CS7
        [Obsolete("This method has been replaced by the RemoveRangeIfContains<T>(this IList<T> collection, params T[] values) method.")]
        public static T[] RemoveRangeIfContains<T>(this ICollection<T> collection, params T[] values) => collection.RemoveRangeIfContains((System.Collections.Generic.IEnumerable<T>)values);
#endif
#endif

            public static Result ToResultEnum(this bool? value) => value.HasValue ? value.Value.ToResultEnum() : Result.None;

            public static Result ToResultEnum(this bool value) => value ? Result.True : Result.False;

            public static bool ToBool(this bool? value) => value.HasValue && value.Value;

            public static bool ToBoolIgnoreNull(this bool? value) => value.HasValue ? value.Value : true;



            public static bool AndAlso(this bool? value, bool other) => ToBool(value) && other;

            public static bool AndAlso(this bool? value, bool? other) => ToBool(value) && ToBool(other);

            public static bool AndAlsoIgnoreNull(this bool? value, bool other) => value.HasValue ? value.Value && other : other;

            public static bool AndAlsoIgnoreNull(this bool? value, bool? other)
            {
                if (value.HasValue)

                    return other.HasValue ? value.Value && other.Value : value.Value;

                return ToBool(other);
            }



            public static bool OrElse(this bool? value, bool other) => value.HasValue && (value.Value || other);

            public static bool OrElse(this bool? value, bool? other) => value.HasValue && other.HasValue && (value.Value || other.Value);

            public static bool OrElseIgnoreNull(this bool? value, bool other) => value.HasValue ? value.Value || other : other;

            public static bool OrElseIgnoreValue(this bool? value, bool? other)
            {
                if (value.HasValue)

                    return other.HasValue ? value.Value || other.Value : value.Value;

                return ToBool(other);
            }



            public static bool XOr(this bool? value, bool other) => value.HasValue && (value.Value ^ other);

            public static bool XOr(this bool? value, bool? other) => value.HasValue && other.HasValue && (value.Value ^ other.Value);

            public static bool XOrIgnoreNull(this bool? value, bool other) => value.HasValue ? value.Value ^ other : other;

            public static bool XOrIgnoreValue(this bool? value, bool? other)
            {
                if (value.HasValue)

                    return other.HasValue ? value.Value ^ other.Value : value.Value;

                return ToBool(other);
            }



            public static XOrResult GetXOrResult(this bool? value, bool other)
            {
                if (ToBool(value))

                    return other ? XOrResult.MoreThanOneTrueResult : XOrResult.OneTrueResult;

                return other ? XOrResult.OneTrueResult : XOrResult.NoTrueResult;
            }

            public static XOrResult GetXOrResult(this bool? value, bool? other)
            {
                if (ToBool(value))

                    return ToBool(other) ? XOrResult.MoreThanOneTrueResult : XOrResult.OneTrueResult;

                return ToBool(other) ? XOrResult.OneTrueResult : XOrResult.NoTrueResult;
            }

            public static XOrResult GetXOrResultIgnoreNull(this bool? value, bool other)
            {
                if (ToBool(value))

                    return other ? XOrResult.MoreThanOneTrueResult : XOrResult.OneTrueResult;

                return other ? XOrResult.OneTrueResult : XOrResult.NoTrueResult;
            }

            public static XOrResult GetXOrResultIgnoreValue(this bool? value, bool? other)
            {
                if (ToBool(value))

                    return ToBool(other) ? XOrResult.MoreThanOneTrueResult : XOrResult.OneTrueResult;

                return ToBool(other) ? XOrResult.OneTrueResult : XOrResult.NoTrueResult;
            }

#if WinCopies2
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

#if CS7
        /// <summary>
        /// Tries to add multiple values to an <see cref="ICollection{T}"/> if it does not contain them already.
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="collection">The collection to which try to add the value</param>
        /// <param name="values">The values to try to add to the collection</param>
        /// <returns><see langword="true"/> if the value has been added to the collection, otherwise <see langword="false"/>.</returns>
        public static T[] AddRangeIfNotContains<T>(this ICollection<T> collection, params T[] values) => collection.AddRangeIfNotContains((System.Collections.Generic.IEnumerable<T>)values);
#endif

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

        public static bool InsertIfNotContains<T>(this System.Collections.Generic.IList<T> collection, in int index, in T value)
        {
            ThrowIfNull(collection, nameof(collection));

            if (collection.Contains(value)) return false;

            collection.Insert(index, value);

            return true;
        }

#if CS7
        public static T[] InsertRangeIfNotContains<T>(this System.Collections.Generic.IList<T> collection, in int index, params T[] values) => collection.InsertRangeIfNotContains(index, (System.Collections.Generic.IEnumerable<T>)values);
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

        public static bool RemoveIfContains<T>(this ICollection<T> collection, in T value) => (collection ?? throw GetArgumentNullException(nameof(collection))).Contains(value) ? collection.Remove(value) : false;

#if CS7
        public static T[] RemoveRangeIfContains<T>(this IList<T> collection, params T[] values) => collection.RemoveRangeIfContains((System.Collections.Generic.IEnumerable<T>)values);
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

#if CS7
        public static void AddRange(this IList collection, in IEnumerable array, in int start, in int length) => collection.AddRange(array.ToArray(), start, length);
#endif

        public static void AddRange<T>(this ICollection<T> collection, params T[] values) => collection.AddRange((System.Collections.Generic.IEnumerable<T>)values);

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
        public static void AddRangeFirst<T>(this System.Collections.Generic.LinkedList<T> collection, in IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
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

            var result = new WinCopies.Collections.DotNetFix.Generic.EnumerableQueue<System.Collections.Generic.LinkedListNode<T>>();

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
        public static void AddRangeLast<T>(this System.Collections.Generic.LinkedList<T> collection, params System.Collections.Generic.LinkedListNode<T>[] nodes) => collection.AddRangeLast((IEnumerable<System.Collections.Generic.LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s at the end of a <see cref="System.Collections.Generic.LinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="array">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        public static void AddRangeLast<T>(this System.Collections.Generic.LinkedList<T> collection, in IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
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

            var result = new WinCopies.Collections.DotNetFix.Generic.EnumerableQueue<System.Collections.Generic.LinkedListNode<T>>();

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
        public static void AddRangeBefore<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, params System.Collections.Generic.LinkedListNode<T>[] nodes) => collection.AddRangeBefore(node, (IEnumerable<System.Collections.Generic.LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple values before a specified node in a <see cref="System.Collections.Generic.LinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="System.Collections.Generic.LinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="array">The values to add to a <see cref="System.Collections.Generic.LinkedList{T}"/></param>
        public static void AddRangeBefore<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, in IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
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
        public static void AddRangeAfter<T>(this System.Collections.Generic.LinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, in IEnumerable<System.Collections.Generic.LinkedListNode<T>> array)
        {
            ThrowIfNull(node, nameof(node));

            if (node.Next == null)

                collection.AddRangeLast(array);

            else

                collection.AddRangeBefore(node.Next, array);
        }
            #endregion

#if CS7
        public static ArrayList ToList(this IEnumerable array) => array.ToList(0);
#endif

        //public static List<T> ToList<T>(this System.Collections.Generic.IEnumerable<T> array)

        //{

        //    List<T> arrayList = new List<T>();

        //    foreach (T value in array)

        //        arrayList.Add(value);

        //    return arrayList;

        //}

        //public static T[] ToArray<T>(this System.Collections.Generic.IEnumerable<T> array)

        //{

        //    T[] _array = new T[length];

        //    int i = 0;

        //    int count = 0;

        //    foreach (T value in array)

        //    {

        //        if (i < startIndex)

        //            i++;

        //        else

        //            _array[count++] = value;

        //        if (count == length)

        //            break;

        //    }

        //    return _array;

        //}

        public static object[] ToArray(this IEnumerable array, in int startIndex, in int length)
        {
            ThrowIfNull(array, nameof(array));

            object[] _array = new object[length];

            int i = 0;

            int count = 0;

            foreach (object value in array)
            {
                if (i < startIndex)

                    i++;

                else

                    _array[count++] = value;

                if (count == length)

                    break;
            }

            return _array;
        }

        public static T[] ToArray<T>(this System.Collections.Generic.IEnumerable<T> array, in int startIndex, in int length)
        {
            ThrowIfNull(array, nameof(array));

            var _array = new T[length];

            int i = 0;

            int count = 0;

            foreach (T value in array)
            {
                if (i < startIndex)

                    i++;

                else

                    _array[count++] = value;

                if (count == length)

                    break;
            }

            return _array;
        }

        public static ArrayList ToList(this object[] array, in int startIndex, in int length)
        {
            ThrowIfNull(array, nameof(array));

            var arrayList = new ArrayList(length);

            int count = startIndex + length;

            int i;

            for (i = startIndex; i < count; i++)

                _ = arrayList.Add(array[i]);

            return arrayList;
        }

        public static List<T> ToList<T>(this T[] array, in int startIndex, in int length)
        {
            ThrowIfNull(array, nameof(array));

            var arrayList = new List<T>(length);

            int count = startIndex + length;

            int i;

            for (i = startIndex; i < count; i++)

                arrayList.Add(array[i]);

            return arrayList;
        }

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
        /// Appends data to the table. Arrays must have only one dimension.
        /// </summary>
        /// <param name="array">The source table.</param>
        /// <param name="arrays">The tables to concatenate.</param>
        /// <returns></returns>
        public static object[] Append(this Array array, params Array[] arrays) => Concatenate((object[])array, arrays);

        /// <summary>
        /// Appends data to the table using the <see cref="Array.LongLength"/> length property. Arrays must have only one dimension.
        /// </summary>
        /// <param name="array">The source table.</param>
        /// <param name="arrays">The tables to concatenate.</param>
        /// <returns></returns>
        public static object[] AppendLong(this Array array, params Array[] arrays) => ConcatenateLong((object[])array, arrays);

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

#if WinCopies2
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

        public static string ToString(this IEnumerable array, in bool parseSubEnumerables, in bool parseStrings = false)
        {
            ThrowIfNull(array, nameof(array));

            var stringBuilder = new StringBuilder();

            array.ToString(ref stringBuilder, parseSubEnumerables, parseStrings);

            return stringBuilder.ToString(0, stringBuilder.Length - 2);
        }

        static void Append(object _value, ref StringBuilder stringBuilder, in bool parseStrings, in bool parseSubEnumerables)

        {
            ThrowIfNull(stringBuilder, nameof(stringBuilder));

            if ((_value is string && parseStrings) || (!(_value is string) && _value is IEnumerable && parseSubEnumerables))

                ((IEnumerable)_value).ToString(ref stringBuilder, true);

            else

                _ = stringBuilder.AppendFormat("{0}, ", _value?.ToString());
        }

        private static void ToString(this IEnumerable array, ref StringBuilder stringBuilder, in bool parseSubEnumerables, in bool parseStrings = false)
        {
            _ = stringBuilder.Append("{");

            System.Collections.IEnumerator enumerator = array.GetEnumerator();

            bool atLeastOneLoop = false;

            if (enumerator.MoveNext())

            {

                atLeastOneLoop = true;

                Append(enumerator.Current, ref stringBuilder, parseStrings, parseSubEnumerables);

            }

            while (enumerator.MoveNext())

                Append(enumerator.Current, ref stringBuilder, parseStrings, parseSubEnumerables);

            _ = atLeastOneLoop ? stringBuilder.Insert(stringBuilder.Length - 2, "}") : stringBuilder.Append("}");
        }

        /// <summary>
        /// Yield returns each object of an <see cref="IEnumerable"/>, so the given <see cref="IEnumerable"/> will be considered as an <see cref="IEnumerable{Object}"/>.
        /// </summary>
        /// <param name="enumerable">An <see cref="IEnumerable"/> to consider as a <see cref="IEnumerable{Object}"/>.</param>
        /// <returns>Yield returns the same enumerable as the given <paramref name="enumerable"/>, as an <see cref="IEnumerable{Object}"/>.</returns>
        public static IEnumerable<object> AsObjectEnumerable(this IEnumerable enumerable)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object item in enumerable)

                yield return item;
        }

        /// <summary>
        /// Returns the first item, if any, from <typeparamref name="T"/> in a given <see cref="IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item to return.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable"/> in which to look for the first item of the given type.</param>
        /// <returns>The first item, if any, from <typeparamref name="T"/> in <paramref name="enumerable"/> or the default value for <typeparamref name="T"/> if none value was found.</returns>
        /// <seealso cref="LastOrDefault{T}(IEnumerable)"/>
        /// <seealso cref="FirstOrDefault{T}(IEnumerable, in Predicate{T})"/>
        /// <seealso cref="LastOrDefault{T}(IEnumerable, in Predicate{T})"/>
        public static T FirstOrDefault<T>(this IEnumerable enumerable)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object item in enumerable)

                if (item is T _item) return _item;

            return default;
        }

        /// <summary>
        /// Returns the first item, if any, from <typeparamref name="T"/> and that validates a given predicate in a given <see cref="IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item to return.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable"/> in which to look for the first item of the given type.</param>
        /// <param name="predicate">The predicate to validate.</param>
        /// <returns>The first item, if any, from <typeparamref name="T"/> in <paramref name="enumerable"/> or the default value for <typeparamref name="T"/> if none value was found.</returns>
        /// <seealso cref="LastOrDefault{T}(IEnumerable, in Predicate{T})"/>
        /// <seealso cref="FirstOrDefault{T}(IEnumerable)"/>
        /// <seealso cref="LastOrDefault{T}(IEnumerable)"/>
        public static T FirstOrDefault<T>(this IEnumerable enumerable, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object item in enumerable)

                if (item is T _item && predicate(_item)) return _item;

            return default;
        }

        /// <summary>
        /// Returns the last item, if any, from <typeparamref name="T"/> in a given <see cref="IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item to return.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable"/> in which to look for the last item of the given type.</param>
        /// <returns>The last item, if any, from <typeparamref name="T"/> in <paramref name="enumerable"/> or the default value for <typeparamref name="T"/> if none value was found.</returns>
        /// <seealso cref="FirstOrDefault{T}(IEnumerable)"/>
        /// <seealso cref="FirstOrDefault{T}(IEnumerable, in Predicate{T})"/>
        /// <seealso cref="LastOrDefault{T}(IEnumerable, in Predicate{T})"/>
        public static T LastOrDefault<T>(this IEnumerable enumerable)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            T value = default;

            foreach (object item in enumerable)

                if (item is T _item)

                    value = _item;

            return value;
        }

        /// <summary>
        /// Returns the last item, if any, from <typeparamref name="T"/> and that validates a given predicate in a given <see cref="IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item to return.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable"/> in which to look for the last item of the given type.</param>
        /// <param name="predicate">The predicate to validate.</param>
        /// <returns>The last item, if any, from <typeparamref name="T"/> in <paramref name="enumerable"/> or the default value for <typeparamref name="T"/> if none value was found.</returns>
        /// <seealso cref="FirstOrDefault{T}(IEnumerable, in Predicate{T})"/>
        /// <seealso cref="FirstOrDefault{T}(IEnumerable)"/>
        /// <seealso cref="LastOrDefault{T}(IEnumerable)"/>
        public static T LastOrDefault<T>(this IEnumerable enumerable, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            T value = default;

            foreach (object item in enumerable)

                if (item is T _item && predicate(_item))

                    value = _item;

            return value;
        }

        public static T FirstOrDefault<T>(this IEnumerable enumerable, in Func<object, object> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object item in enumerable)

                if (func(item) is T _item) return _item;

            return default;
        }

        public static T LastOrDefault<T>(this IEnumerable enumerable, in Func<object, object> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            T value = default;

            foreach (object item in enumerable)

                if (func(item) is T _item)

                    value = _item;

            return value;
        }

        public static TOut FirstOrDefault<TIn, TOut>(this IEnumerable<TIn> enumerable, in Func<TIn, object> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (TIn item in enumerable)

                if (func(item) is TOut _item) return _item;

            return default;
        }

        public static TOut LastOrDefault<TIn, TOut>(this IEnumerable enumerable, in Func<TIn, object> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            TOut value = default;

            foreach (TIn item in enumerable)

                if (func(item) is TOut _item)

                    value = _item;

            return value;
        }

        public static IEnumerable AppendValues(this IEnumerable enumerable, params IEnumerable[] newValues)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object obj in enumerable)

                yield return obj;

            foreach (IEnumerable _enumerable in newValues)

                foreach (object _obj in _enumerable)

                    yield return _obj;
        }

        public static System.Collections.Generic.IEnumerable<T> AppendValues<T>(this System.Collections.Generic.IEnumerable<T> enumerable, params System.Collections.Generic.IEnumerable<T>[] newValues)

        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (T obj in enumerable)

                yield return obj;

            foreach (IEnumerable _enumerable in newValues)

                foreach (T _obj in _enumerable)

                    yield return _obj;
        }

        public static System.Collections.Generic.LinkedListNode<T> RemoveAndGetFirstValue<T>(this System.Collections.Generic.LinkedList<T> items)
        {
            System.Collections.Generic.LinkedListNode<T> value = (items ?? throw GetArgumentNullException(nameof(items))).First;

            items.RemoveFirst();

            return value;
        }

        public static System.Collections.Generic.LinkedListNode<T> RemoveAndGetLastValue<T>(this System.Collections.Generic.LinkedList<T> items)
        {
            System.Collections.Generic.LinkedListNode<T> value = (items ?? throw GetArgumentNullException(nameof(items))).Last;

            items.RemoveLast();

            return value;
        }

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

                                    ThrowMoreThanOneOccurencesWereFoundException();

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

                                    ThrowMoreThanOneOccurencesWereFoundException();

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

                                    ThrowMoreThanOneOccurencesWereFoundException();

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

                                    ThrowMoreThanOneOccurencesWereFoundException();

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
#endif

            public static void CopyTo(this BitArray source, in BitArray array, in int startIndex)
            {
                ThrowIfNull(source, nameof(source));
                ThrowIfNull(array, nameof(array));

                if (array.Length > source.Length)

                    throw new ArgumentOutOfRangeException(nameof(array));

                if (startIndex < 0 || startIndex + array.Length > source.Length)

                    throw new IndexOutOfRangeException($"{nameof(startIndex)} is out of range.");

                for (int i = 0; i < source.Length; i++)

                    array[startIndex + i] = source[i];
            }

            public static void SetMultipleBits(this BitArray array, in byte[] bytes, in int startIndex)
            {
                long length = bytes.Length * 8;

                if (length > array.Length)

                    throw new ArgumentOutOfRangeException(nameof(bytes));

                if (startIndex < 0 || startIndex + length > array.Length)

                    throw new IndexOutOfRangeException($"{nameof(startIndex)} is out of range.");

                new BitArray(bytes).CopyTo(array, startIndex);
            }

            public static void SetMultipleBits(this BitArray array, in BitArray bits, in int startIndex)
            {
                if (bits.Length > array.Length)

                    throw new ArgumentOutOfRangeException(nameof(bits));

                if (startIndex < 0 || startIndex + bits.Length > array.Length)

                    throw new IndexOutOfRangeException($"{nameof(startIndex)} is out of range.");

                new BitArray(bits).CopyTo(array, startIndex);
            }

            /// <summary>
            /// Checks if the current object is assignable from at least one type of a given <see cref="Type"/> array.
            /// </summary>
            /// <param name="obj">The object from which check the type</param>
            /// <param name="typeEquality"><see langword="true"/> to preserve type equality, regardless of the type inheritance, otherwise <see langword="false"/></param>
            /// <param name="types">The types to compare</param>
            /// <returns><see langword="true"/> if the current object is assignable from at least one of the given types, otherwise <see langword="false"/>.</returns>
            public static bool Is(this object obj, in bool typeEquality, params Type[] types)
            {
                ThrowIfNull(obj, nameof(obj));

                Type objType = obj.GetType();

                foreach (Type type in types)

                    if (typeEquality ? objType == type : type.IsAssignableFrom(objType))

                        return true;

                return false;
            }

            public static bool IsType(this Type t, params Type[] types)
            {
                foreach (Type type in types)

                    if (t == type)

                        return true;

                return false;
            }

            public static IEnumerable<TKey> GetKeys<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)
            {
                ThrowIfNull(array, nameof(array));

                foreach (KeyValuePair<TKey, TValue> value in array)

                    yield return value.Key;
            }

            public static IEnumerable<TValue> GetValues<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)
            {
                ThrowIfNull(array, nameof(array));

                foreach (KeyValuePair<TKey, TValue> value in array)

                    yield return value.Value;
            }

            public static bool CheckIntegrity<TKey, TValue>(this KeyValuePair<TKey, TValue>[] array)
            {
#if CS8

                static

#endif

                bool predicateByVal(TKey keyA, TKey keyB) => Equals(keyA, keyB);

#if CS8

                static

#endif

                bool predicateByRef(TKey keyA, TKey keyB) => ReferenceEquals(keyA, keyB);

                Func<TKey, TKey, bool> predicate = typeof(TKey).IsClass ? predicateByRef : (Func<TKey, TKey, bool>)predicateByVal;

                IEnumerable<TKey> keys = array.GetKeys();

                IEnumerable<TKey> _keys = array.GetKeys();

                bool foundOneOccurrence = false;

                foreach (TKey key in keys)
                {
                    if (key == null)

                        throw GetOneOrMoreKeyIsNullException();

                    foreach (TKey _key in _keys)
                    {
                        if (predicate(key, _key))

                            if (foundOneOccurrence)

                                return false;

                            else

                                foundOneOccurrence = true;
                    }

                    foundOneOccurrence = false;
                }

                return true;
            }

            public static bool CheckPropertySetIntegrity(Type propertyObjectType, in string propertyName, out string methodName, in int skipFrames, in BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet)
            {
                ThrowIfNull(propertyObjectType, nameof(propertyObjectType));

                PropertyInfo property = propertyObjectType.GetProperty(propertyName, bindingFlags) ?? throw GetFieldOrPropertyNotFoundException(propertyName, propertyObjectType);

                MethodBase method = new StackFrame(skipFrames).GetMethod();

                methodName = method.Name;

                //#if DEBUG 

                //            Debug.WriteLine("Property: " + property.Name + ", " + property.DeclaringType);

                //            Debug.WriteLine("Method: " + method.Name + ", " + method.DeclaringType);

                //#endif 

                // todo: tuple and check DeclaringTypeNotCorrespond throws

                return (property.CanWrite && property.GetSetMethod() != null) || property.DeclaringType == method.DeclaringType;
            }

            internal static FieldInfo GetField(in string fieldName, in Type objectType, in BindingFlags bindingFlags) => objectType.GetField(fieldName, bindingFlags) ?? throw GetFieldOrPropertyNotFoundException(fieldName, objectType);

            internal static PropertyInfo GetProperty(in string propertyName, in Type objectType, in BindingFlags bindingFlags) => objectType.GetProperty(propertyName, bindingFlags) ?? throw GetFieldOrPropertyNotFoundException(propertyName, objectType);

            // todo: use attributes

#if CS7
#if WinCopies2
        /// <summary>
        /// Converts an <see cref="IEnumerable"/> to an <see cref="ArrayList"/> from a given index for a given length.
        /// </summary>
        /// <param name="array">The <see cref="IEnumerable"/> to convert</param>
        /// <param name="startIndex">The index from which start the conversion.</param>
        /// <param name="length">The length of items to copy in the out <see cref="ArrayList"/>. Leave this parameter to null if you want to copy all the source <see cref="IEnumerable"/>.</param>
        /// <returns>The result <see cref="ArrayList"/>.</returns>
        public static ArrayList ToList(this IEnumerable array, in int startIndex, in int? length = null)
        {
            ThrowIfNull(array, nameof(array));

            int i = 0;

            if (length == null)
            {
                var arrayBuilder = new ArrayBuilder<object>();

                foreach (object value in array)

                    if (i < startIndex) i++;

                    else // We don't need to increment i anymore when we are here

                        _ = arrayBuilder.AddLast(value);

                return arrayBuilder.ToArrayList();
            }

            else
            {
                var arrayList = new ArrayList(length.Value);

                int count = 0;

                foreach (object value in array)
                {
                    if (i < startIndex)

                        i++;

                    else
                    {
                        _ = arrayList.Add(value);

                        count++;
                    }

                    if (count == length)

                        break;
                }

                return arrayList;
            }
        }

        public static object[] ToArray(this IEnumerable array)
        {
            ThrowIfNull(array, nameof(array));

            var _array = new ArrayBuilder<object>();

            foreach (object value in array)

                _ = _array.AddLast(value);

            return _array.ToArray<object>();
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable"/> to a <see cref="List{T}"/> from a given index for a given length.
        /// </summary>
        /// <param name="array">The <see cref="IEnumerable"/> to convert</param>
        /// <param name="startIndex">The index from which start the conversion.</param>
        /// <param name="length">The length of items to copy in the out <see cref="List{T}"/>. Leave this parameter to null if you want to copy all the source <see cref="IEnumerable"/>.</param>
        /// <returns>The result <see cref="List{T}"/>.</returns>
        public static List<T> ToList<T>(this System.Collections.Generic.IEnumerable<T> array, in int startIndex, in int? length = null)
        {
            ThrowIfNull(array, nameof(array));

            int i = 0;

            if (length == null)
            {
                var arrayBuilder = new ArrayBuilder<T>();

                foreach (T value in array)

                    if (i < startIndex) i++;

                    else    // We don't need to increment i anymore when we are here

                        _ = arrayBuilder.AddLast(value);

                return arrayBuilder.ToList();
            }

            else
            {
                var arrayList = new List<T>(length.Value);

                int count = 0;

                foreach (T value in array)
                {
                    if (i < startIndex)

                        i++;

                    else
                    {
                        arrayList.Add(value);

                        count++;
                    }

                    if (count == length)

                        break;
                }

                return arrayList;
            }
        }

        [Obsolete("This method has been replaced by the RemoveRangeIfContains<T>(this IList<T> collection, in System.Collections.Generic.IEnumerable<T> values) method.")]
        public static T[] RemoveRangeIfContains<T>(this ICollection<T> collection, in System.Collections.Generic.IEnumerable<T> values)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(values, nameof(values));

            var removedValues = new ArrayBuilder<T>();

            foreach (T value in values)

                if (collection.Contains(value))
                {
                    // todo: RemoveAt()

                    _ = collection.Remove(value);

                    _ = removedValues.AddLast(value);
                }

            return removedValues.ToArray();
        }

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
        /// Add multiple values before a specified node in a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="array">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeBefore<T>(this ILinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            var result = new WinCopies.Collections.DotNetFix.Generic.EnumerableQueue<System.Collections.Generic.LinkedListNode<T>>();

            foreach (T item in array)

                result.Enqueue(collection.AddBefore(node, item));

            return result.ToArray<System.Collections.Generic.LinkedListNode<T>>();
        }

        /// <summary>
        /// Add multiple values after a specified node in a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="values">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeAfter<T>(this ILinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, params T[] values)
        {
            ThrowIfNull(node, nameof(node));

            return node.Next == null ? collection.AddRangeLast(values) : collection.AddRangeBefore(node.Next, values);
        }

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

        public static T[] ToArray<T>(this ArrayBuilder<T>[] arrayBuilders, in bool remove = false)
        {
            ThrowIfNull(arrayBuilders, nameof(arrayBuilders));

            var items = new T[arrayBuilders.GetCapacityInternal()];

            foreach (ArrayBuilder<T> arrayBuilder in arrayBuilders)

                arrayBuilder.ToArray(items, remove);

            return items;
        }

        public static int GetCapacity<T>(this ArrayBuilder<T>[] arrayBuilders) => (arrayBuilders ?? throw GetArgumentNullException(nameof(arrayBuilders))).GetCapacityInternal();

        private static int GetCapacityInternal<T>(this ArrayBuilder<T>[] arrayBuilders)
        {
            int capacity = 0;

            foreach (ArrayBuilder<T> arrayBuilder in arrayBuilders)

                capacity += arrayBuilder.Count;

            return capacity;
        }

        public static ArrayList ToArrayList<T>(this ArrayBuilder<T>[] arrayBuilders, in bool remove = false)
        {
            ThrowIfNull(arrayBuilders, nameof(arrayBuilders));

            var items = new ArrayList(arrayBuilders.GetCapacityInternal());

            foreach (ArrayBuilder<T> arrayBuilder in arrayBuilders)

                arrayBuilder.ToArrayList(items, remove);

            return items;
        }

        public static List<T> ToList<T>(this ArrayBuilder<T>[] arrayBuilders, in bool remove = false)
        {
            ThrowIfNull(arrayBuilders, nameof(arrayBuilders));

            var items = new List<T>(arrayBuilders.GetCapacityInternal());

            foreach (ArrayBuilder<T> arrayBuilder in arrayBuilders)

                arrayBuilder.ToList(items, remove);

            return items;
        }

        public static void SplitToILinkedList(this string s, in bool splitEmptyValues, in StringBuilder stringBuilder, ILinkedList<string> list, params char[] separators)
        {
            ThrowIfNull(list, nameof(list));

            Split(s, splitEmptyValues, stringBuilder, _s => list.AddLast(_s), separators);
        }

        public static ILinkedList<string> SplitToILinkedList(this string s, in bool splitEmptyValues, params char[] separators)
        {
            var list = new Collections.DotNetFix.LinkedList<string>();

            SplitToILinkedList(s, splitEmptyValues, new StringBuilder(), list, separators);

            return list;
        }

        public static System.Collections.Generic.LinkedListNode<T> RemoveAndGetLastValue<T>(this ILinkedList<T> items)
        {
            System.Collections.Generic.LinkedListNode<T> value = (items ?? throw GetArgumentNullException(nameof(items))).Last;

            items.RemoveLast();

            return value;
        }

        public static System.Collections.Generic.LinkedListNode<T> RemoveAndGetFirstValue<T>(this ILinkedList<T> items)
        {
            System.Collections.Generic.LinkedListNode<T> value = (items ?? throw GetArgumentNullException(nameof(items))).First;

            items.RemoveFirst();

            return value;
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
        /// Add multiple values at the top of a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="values">The values to add to this <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeFirst<T>(this ILinkedList<T> collection, params T[] values)
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
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeFirst<T>(this ILinkedList<T> collection, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(collection, nameof(collection));

            return collection.First == null ? collection.AddRangeLast(array) : collection.AddRangeBefore(collection.First, array);
        }

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
        /// Add multiple values after a specified node in a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node after which to add the values</param>
        /// <param name="array">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeAfter<T>(this ILinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(node, nameof(node));

            return node.Next == null ? collection.AddRangeLast(array) : collection.AddRangeBefore(node.Next, array);
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

        /// <summary>
        /// Add multiple values before a specified node in a <see cref="ILinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="nodes">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="ILinkedList{T}"/></param>
        public static void AddRangeBefore<T>(this ILinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, params System.Collections.Generic.LinkedListNode<T>[] nodes) => collection.AddRangeBefore(node, (IEnumerable<System.Collections.Generic.LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple values before a specified node in a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="node">The node before which to add the values</param>
        /// <param name="values">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeBefore<T>(this ILinkedList<T> collection, in System.Collections.Generic.LinkedListNode<T> node, params T[] values) => collection.AddRangeBefore(node, (System.Collections.Generic.IEnumerable<T>)values);

        /// <summary>
        /// Add multiple <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s at the end of a <see cref="ILinkedList{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="nodes">The <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static void AddRangeLast<T>(this ILinkedList<T> collection, params System.Collections.Generic.LinkedListNode<T>[] nodes) => collection.AddRangeLast((IEnumerable<System.Collections.Generic.LinkedListNode<T>>)nodes);

        /// <summary>
        /// Add multiple values at the end of a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="values">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeLast<T>(this ILinkedList<T> collection, params T[] values) => collection.AddRangeLast((System.Collections.Generic.IEnumerable<T>)values);

        /// <summary>
        /// Add multiple values at the end of a <see cref="ILinkedList{T}"/>. For better performance, use the <see cref="ArrayBuilder{T}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="ILinkedList{T}"/> into which add the values.</param>
        /// <param name="array">The values to add to a <see cref="ILinkedList{T}"/></param>
        /// <returns>The added <see cref="System.Collections.Generic.LinkedListNode{T}"/>'s.</returns>
        public static System.Collections.Generic.LinkedListNode<T>[] AddRangeLast<T>(this ILinkedList<T> collection, in System.Collections.Generic.IEnumerable<T> array)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfNull(array, nameof(array));

            var result = new WinCopies.Collections.DotNetFix.Generic.EnumerableQueue<System.Collections.Generic.LinkedListNode<T>>();

            foreach (T item in array)

                result.Enqueue(collection.AddLast(item));

            return result.ToArray<System.Collections.Generic.LinkedListNode<T>>();
        }
#endif

            private static (bool fieldChanged, object oldValue) SetField(this object obj, in FieldInfo field, in object previousValue, in object newValue, in string paramName, in bool setOnlyIfNotNull, in bool throwIfNull, in bool disposeOldValue, in FieldValidateValueCallback validateValueCallback, in bool throwIfValidationFails, in FieldValueChangedCallback valueChangedCallback)
            {
                if (newValue is null)

                    if (throwIfNull)

                        throw GetArgumentNullException(paramName);

                    else if (setOnlyIfNotNull)

                        return (false, previousValue);

                (bool validationResult, Exception validationException) = validateValueCallback?.Invoke(obj, newValue, field, paramName) ?? (true, null);

                if (validationResult)

                    if ((newValue == null && previousValue != null) || (newValue != null && !newValue.Equals(previousValue)))
                    {
                        if (disposeOldValue)

                            ((IDisposable)previousValue).Dispose();

                        field.SetValue(obj, newValue);

                        valueChangedCallback?.Invoke(obj, newValue, field, paramName);

                        return (true, previousValue);

                        //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                        //             BindingFlags.Static | BindingFlags.Instance |
                        //             BindingFlags.DeclaredOnly;
                        //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);
                    }

                    else

                        return (false, previousValue);

                else

                    return throwIfValidationFails ? throw (validationException ?? new ArgumentException("Validation error.", paramName)) : (false, previousValue);
            }

            public static (bool fieldChanged, object oldValue) SetField(this object obj, in string fieldName, in object newValue, in Type declaringType, in BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, in string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, in FieldValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, in FieldValueChangedCallback valueChangedCallback = null)
            {
                ThrowIfNull(declaringType, nameof(declaringType));

                FieldInfo field = GetField(fieldName, declaringType, bindingFlags);

                return obj.SetField(field, field.GetValue(obj), newValue, paramName, setOnlyIfNotNull, throwIfNull, false, validateValueCallback, throwIfValidationFails, valueChangedCallback);
            }

            public static (bool fieldChanged, IDisposable oldValue) DisposeAndSetField(this object obj, in string fieldName, in IDisposable newValue, in Type declaringType, in BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, in string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, in FieldValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, in FieldValueChangedCallback valueChangedCallback = null)
            {
                ThrowIfNull(declaringType, nameof(declaringType));

                FieldInfo field = GetField(fieldName, declaringType, bindingFlags);

                return ((bool, IDisposable))obj.SetField(field, field.GetValue(obj), newValue, paramName, setOnlyIfNotNull, throwIfNull, true, validateValueCallback, throwIfValidationFails, valueChangedCallback);
            }

            // todo: update code (in, throw if null)

            /// <summary>
            /// Sets a value to a property if the new value is different.
            /// </summary>
            /// <param name="obj">The object in which to set the property.</param>
            /// <param name="propertyName">The name of the given property.</param>
            /// <param name="fieldName">The field related to the property.</param>
            /// <param name="newValue">The value to set.</param>
            /// <param name="declaringType">The actual declaring type of the property.</param>
            /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
            /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
            /// <param name="paramName">The parameter from which the value was passed to this method.</param>
            /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
            /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
            /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
            /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
            /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
            /// <exception cref="InvalidOperationException">The declaring types of the given property and field name doesn't correspond. OR The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
            /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
            /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
            public static (bool propertyChanged, object oldValue) SetProperty(this object obj, string propertyName, string fieldName, object newValue, Type declaringType, in bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null)
            {
                //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                //             BindingFlags.Static | BindingFlags.Instance |
                //             BindingFlags.DeclaredOnly;
                //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName, previousValue, newValue)); 

                // if (declaringType == null) 

                // {

                //while (objectType != declaringType && objectType != typeof(object))

                //    objectType = objectType.BaseType;

                //if (objectType != declaringType)

                //    throw new ArgumentException(string.Format((string)ResourcesHelper.GetResource("DeclaringTypeIsNotInObjectInheritanceHierarchyException"), declaringType, objectType));

                // }

                //#if DEBUG

                //            var fields = objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                //            foreach (var _field in fields)

                //                Debug.WriteLine("Object type: " + objectType + " " + _field.Name);

                //#endif

                // var objectType = obj.GetType();

                FieldInfo field = GetField(fieldName, declaringType, bindingFlags);

                object previousValue = field.GetValue(obj);

                if (!CheckPropertySetIntegrity(declaringType, propertyName, out string methodName, 3, bindingFlags))

                    throw GetDeclaringTypesNotCorrespondException(propertyName, methodName);

                PropertyInfo property = GetProperty(propertyName, declaringType, bindingFlags);

                return !property.CanWrite || property.SetMethod == null
                    ? throwIfReadOnly ? throw new InvalidOperationException(string.Format("This property is read-only. Property name: {0}, declaring type: {1}.", propertyName, declaringType)) : (false, previousValue)
                    : obj.SetField(field, previousValue, newValue, paramName, setOnlyIfNotNull, throwIfNull, false, validateValueCallback, throwIfValidationFails, valueChangedCallback);
            }

            /// <summary>
            /// Sets a value to a property if the new value is different.
            /// </summary>
            /// <param name="obj">The object in which to set the property.</param>
            /// <param name="propertyName">The name of the given property.</param>
            /// <param name="newValue">The value to set.</param>
            /// <param name="declaringType">The actual declaring type of the property.</param>
            /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
            /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
            /// <param name="paramName">The parameter from which the value was passed to this method.</param>
            /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
            /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
            /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
            /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
            /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
            /// <exception cref="InvalidOperationException">The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
            /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
            /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
            public static (bool propertyChanged, object oldValue) SetProperty(this object obj, string propertyName, object newValue, Type declaringType, in bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null)
            {
                PropertyInfo property = GetProperty(propertyName, declaringType, bindingFlags);

                object previousValue = property.GetValue(obj);

                if (!property.CanWrite || property.SetMethod == null)

                    return throwIfReadOnly ? throw new InvalidOperationException(string.Format("This property is read-only. Property name: {0}, declaring type: {1}.", propertyName, declaringType)) : (false, previousValue);

                if (newValue is null)

                    if (throwIfNull)

                        throw GetArgumentNullException(paramName);

                    else if (setOnlyIfNotNull)

                        return (false, previousValue);

                (bool validationResult, Exception validationException) = validateValueCallback?.Invoke(obj, newValue, property, paramName) ?? (true, null);

                if (validationResult)

                    if ((newValue == null && previousValue != null) || (newValue != null && !newValue.Equals(previousValue)))
                    {
                        property.SetValue(obj, newValue);

                        valueChangedCallback?.Invoke(obj, newValue, property, paramName);

                        return (true, previousValue);

                        //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                        //             BindingFlags.Static | BindingFlags.Instance |
                        //             BindingFlags.DeclaredOnly;
                        //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);
                    }

                    else

                        return (false, previousValue);

                else

                    return throwIfValidationFails ? throw (validationException ?? new ArgumentException("Validation error.", paramName)) : (false, previousValue);
            }

            /// <summary>
            /// Disposes an old value of a property then sets a new value to the given property if the new value is different.
            /// </summary>
            /// <param name="obj">The object in which to set the property.</param>
            /// <param name="propertyName">The name of the given property.</param>
            /// <param name="fieldName">The field related to the property.</param>
            /// <param name="newValue">The value to set.</param>
            /// <param name="declaringType">The actual declaring type of the property.</param>
            /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
            /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
            /// <param name="paramName">The parameter from which the value was passed to this method.</param>
            /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
            /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
            /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
            /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
            /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
            /// <exception cref="InvalidOperationException">The declaring types of the given property and field name doesn't correspond. OR The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
            /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
            /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
            public static (bool propertyChanged, IDisposable oldValue) DisposeAndSetProperty(this object obj, string propertyName, string fieldName, IDisposable newValue, Type declaringType, in bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, FieldValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, FieldValueChangedCallback valueChangedCallback = null)
            {
                //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                //             BindingFlags.Static | BindingFlags.Instance |
                //             BindingFlags.DeclaredOnly;
                //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);

                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName, previousValue, newValue)); 

                // if (declaringType == null) 

                // {

                //while (objectType != declaringType && objectType != typeof(object))

                //    objectType = objectType.BaseType;

                //if (objectType != declaringType)

                //    throw new ArgumentException(string.Format((string)ResourcesHelper.GetResource("DeclaringTypeIsNotInObjectInheritanceHierarchyException"), declaringType, objectType));

                // }

                //#if DEBUG

                //            var fields = objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                //            foreach (var _field in fields)

                //                Debug.WriteLine("Object type: " + objectType + " " + _field.Name);

                //#endif

                // var objectType = obj.GetType();

                FieldInfo field = GetField(fieldName, declaringType, bindingFlags);

                var previousValue = (IDisposable)field.GetValue(obj);

                if (!CheckPropertySetIntegrity(declaringType, propertyName, out string methodName, 3, bindingFlags))

                    throw GetDeclaringTypesNotCorrespondException(propertyName, methodName);

                PropertyInfo property = GetProperty(propertyName, declaringType, bindingFlags);

                return !property.CanWrite || property.SetMethod == null
                    ? throwIfReadOnly ? throw new InvalidOperationException(string.Format("This property is read-only. Property name: {0}, declaring type: {1}.", propertyName, declaringType)) : (false, previousValue)
                    : ((bool, IDisposable))obj.SetField(field, previousValue, newValue, paramName, setOnlyIfNotNull, throwIfNull, true, validateValueCallback, throwIfValidationFails, valueChangedCallback);
            }

            /// <summary>
            /// Disposes an old value of a property then sets a new value to the given property if the new value is different.
            /// </summary>
            /// <param name="obj">The object in which to set the property.</param>
            /// <param name="propertyName">The name of the given property.</param>
            /// <param name="newValue">The value to set.</param>
            /// <param name="declaringType">The actual declaring type of the property.</param>
            /// <param name="throwIfReadOnly">Whether to throw if the given property is read-only.</param>
            /// <param name="bindingFlags">The <see cref="BindingFlags"/> used to get the property.</param>
            /// <param name="paramName">The parameter from which the value was passed to this method.</param>
            /// <param name="setOnlyIfNotNull">Whether to set only if the given value is not null.</param>
            /// <param name="throwIfNull">Whether to throw if the given value is null.</param>
            /// <param name="validateValueCallback">The callback used to validate the given value. You can leave this parameter to null if you don't want to perform validation.</param>
            /// <param name="throwIfValidationFails">Whether to throw if the validation of <paramref name="validateValueCallback"/> fails.</param>
            /// <param name="valueChangedCallback">The callback used to perform actions after the property is set. You can leave this parameter to null if you don't want to perform actions after the property is set.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether the setting succeeded and the old value of the given property (or <see langword="null"/> if the property does not contain any value nor reference).</returns>
            /// <exception cref="InvalidOperationException">The given property is read-only and <paramref name="throwIfReadOnly"/> is set to <see langword="true"/>.</exception>
            /// <exception cref="ArgumentNullException">The new value is null and <paramref name="throwIfNull"/> is set to <see langword="true"/>.</exception>
            /// <exception cref="Exception"><paramref name="validateValueCallback"/> failed and <paramref name="throwIfValidationFails"/> is set to <see langword="true"/>. This exception is the exception that was returned by <paramref name="validateValueCallback"/> if it was not null or an <see cref="ArgumentException"/> otherwise.</exception>
            public static (bool propertyChanged, IDisposable oldValue) DisposeAndSetProperty(this object obj, string propertyName, IDisposable newValue, Type declaringType, in bool throwIfReadOnly = true, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, string paramName = null, in bool setOnlyIfNotNull = false, in bool throwIfNull = false, PropertyValidateValueCallback validateValueCallback = null, in bool throwIfValidationFails = false, PropertyValueChangedCallback valueChangedCallback = null)
            {
                PropertyInfo property = GetProperty(propertyName, declaringType, bindingFlags);

                var previousValue = (IDisposable)property.GetValue(obj);

                if (!property.CanWrite || property.SetMethod == null)

                    return throwIfReadOnly ? throw new InvalidOperationException(string.Format("This property is read-only. Property name: {0}, declaring type: {1}.", propertyName, declaringType)) : (false, previousValue);

                if (newValue is null)

                    if (throwIfNull)

                        throw GetArgumentNullException(paramName);

                    else if (setOnlyIfNotNull)

                        return (false, previousValue);

                (bool validationResult, Exception validationException) = validateValueCallback?.Invoke(obj, newValue, property, paramName) ?? (true, null);

                if (validationResult)

                    if ((newValue == null && previousValue != null) || (newValue != null && !newValue.Equals(previousValue)))
                    {
                        previousValue.Dispose();

                        property.SetValue(obj, newValue);

                        valueChangedCallback?.Invoke(obj, newValue, property, paramName);

                        return (true, previousValue);

                        //BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                        //             BindingFlags.Static | BindingFlags.Instance |
                        //             BindingFlags.DeclaredOnly;
                        //this.GetType().GetField(fieldName, flags).SetValue(this, newValue);
                    }

                    else

                        return (false, previousValue);

                else

                    return throwIfValidationFails ? throw (validationException ?? new ArgumentException("Validation error.", paramName)) : (false, previousValue);
            }
#endif

#if WinCopies2

        /// <summary>
        /// Gets the numeric value for an enum.
        /// </summary>
        /// <param name="enum">The enum for which get the corresponding numeric value.</param>
        /// <param name="enumName">Not used.</param>
        /// <returns>The numeric value corresponding to this enum, in the given enum type underlying type.</returns>
        [Obsolete("This method has been replaced by the GetNumValue(this Enum @enum) and the WinCopies.Util.GetNumValue(Type enumType, string fieldName) methods and will be removed in later versions.")]
        public static object GetNumValue(this Enum @enum, in string enumName) => @enum.GetNumValue();

#endif

            // todo: IFormatProvider

            /// <summary>
            /// Gets the numeric value for an enum.
            /// </summary>
            /// <param name="enum">The enum for which get the corresponding numeric value.</param>
            /// <returns>The numeric value corresponding to this enum, in the given enum type underlying type.</returns>
            public static object GetNumValue(this Enum @enum) => Convert.ChangeType(@enum, Enum.GetUnderlyingType(@enum.GetType()));

            /// <summary>
            /// Gets the numeric value for an enum.
            /// </summary>
            /// <param name="enum">The enum for which get the corresponding numeric value.</param>
            /// <returns>The numeric value corresponding to this enum, in the given enum type underlying type.</returns>
            public static object GetNumValue<T>(this T @enum) where T : Enum => Convert.ChangeType(@enum, Enum.GetUnderlyingType(typeof(T)));

            // public static object GetNumValue(this Enum @enum) => GetNumValue(@enum, @enum.ToString());

            // todo : to test if Math.Log(Convert.ToInt64(flagsEnum), 2) == 'SomeInt64'; (no float, double ...) would be faster.

#if CS7
            /// <summary>
            /// Determines whether an enum has multiple flags.
            /// </summary>
            /// <param name="flagsEnum">The enum to check.</param>
            /// <returns><see langword="true"/> if <paramref name="flagsEnum"/> type has the <see cref="FlagsAttribute"/> and has multiple flags; otherwise, <see langword="false"/>.</returns>
            /// <remarks><paramref name="flagsEnum"/> type must have the <see cref="FlagsAttribute"/>.</remarks>
            public static bool HasMultipleFlags(this Enum flagsEnum)
            {
                Type type = flagsEnum.GetType();

                if (type.GetCustomAttributes(typeof(FlagsAttribute)).Count() == 0)

                    return false; // throw new ArgumentException(string.Format("This enum does not implement the {0} attribute.", typeof(FlagsAttribute).Name));



                bool alreadyFoundAFlag = false;

                Enum enumValue;

                // FieldInfo field = null;



                foreach (string s in type.GetEnumNames())
                {
                    enumValue = (Enum)Enum.Parse(type, s);



                    if (enumValue.GetNumValue().Equals(0)) continue;



                    if (flagsEnum.HasFlag(enumValue))

                        if (alreadyFoundAFlag) return true;

                        else alreadyFoundAFlag = true;
                }

                return false;
            }
#endif

            ///// <summary>
            ///// Determines whether the current enum value is within the enum values range delimited by the first and the last fields; see the Remarks section for more information.
            ///// </summary>
            ///// <param name="enum">The enum value to check.</param>
            ///// <returns><see langword="true"/> if the given value is in the enum values range, otherwise <see langword="false"/>.</returns>
            ///// <remarks>This method doesn't read all the enum fields, but only takes care of the first and last numeric enum fields, so if the value is 1, and the enum has only defined fields for 0 and 2, this method still returns <see langword="true"/>. For a method that actually reads all the enum fields, see the <see cref="Type.IsEnumDefined(object)"/> method.</remarks>
            ///// <seealso cref="ThrowIfNotValidEnumValue(Enum)"/>
            ///// <seealso cref="ThrowIfNotDefinedEnumValue(Enum)"/>
            ///// <seealso cref="ThrowIfNotValidEnumValue(Enum, in string)"/>
            ///// <seealso cref="ThrowIfNotDefinedEnumValue(Enum,in string)"/>
            public static bool IsValidEnumValue(this Enum @enum)
            {
                var values = new ArrayList(@enum.GetType().GetEnumValues());

                values.Sort();

                // object _value = Convert.ChangeType(value, value.GetType().GetEnumUnderlyingType());

                return @enum.CompareTo(values[0]) >= 0 && @enum.CompareTo(values[
#if CS8
                ^1
#else
                values.Count - 1
#endif
                ]) <= 0;
            }

#if CS7
            public static bool IsFlagsEnum(this Enum @enum) => (@enum ?? throw GetArgumentNullException(nameof(@enum))).GetType().GetCustomAttribute<FlagsAttribute>() is object;
#endif

#if WinCopies2

#if CS7
        /// <summary>
        /// Determines whether the current enum value is within the enum values range.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <param name="throwIfNotFlagsEnum">Whether to throw if the given enum does not have the <see cref="FlagsAttribute"/> attribute.</param>
        /// <param name="throwIfZero">Whether to throw if the given enum is zero.</param>
        /// <returns><see langword="true"/> if the given value is in the enum values range, otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="enum"/> does not have the <see cref="FlagsAttribute"/> and <paramref name="throwIfNotFlagsEnum"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="enum"/> is equal to zero and the <paramref name="throwIfZero"/> parameter is set to true or <paramref name="enum"/> is lesser than zero.</exception>
        /// <seealso cref="ThrowIfNotValidFlagsEnumValue(Enum, in bool, in bool)"/>
        /// <seealso cref="ThrowIfNotValidFlagsEnumValue(Enum, in string, in bool, in bool)"/>
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

                return throwIfNotFlagsEnum ? throw Util.GetExceptionForNonFlagsEnum(nameof(@enum)) : false;

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
        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the enum value is not in the required enum value range.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <param name="throwIfNotFlagsEnum">Whether to throw if the given enum does not have the <see cref="FlagsAttribute"/> attribute.</param>
        /// <param name="throwIfZero">Whether to throw if the given enum is zero.</param>
        /// <exception cref="ArgumentException"><paramref name="enum"/> does not have the <see cref="FlagsAttribute"/> and <paramref name="throwIfNotFlagsEnum"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="enum"/> is equal to zero and the <paramref name="throwIfZero"/> parameter is set to true or <paramref name="enum"/> is lesser than zero.</exception>
        /// <seealso cref="IsValidEnumValue(Enum)"/>
        /// <seealso cref="ThrowIfNotValidEnumValue(Enum, in string)"/>
        public static void ThrowIfNotValidFlagsEnumValue(this Enum @enum, in bool throwIfNotFlagsEnum, in bool throwIfZero)
        {
            if (!@enum.IsValidFlagsEnumValue(throwIfNotFlagsEnum, throwIfZero))

                throw GetInvalidEnumArgumentException(@enum, nameof(@enum));
        }

        /// <summary>
        /// Throws an <see cref="InvalidEnumArgumentException"/> if the enum value is not in the required enum value range.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <param name="argumentName">The parameter name.</param>
        /// <param name="throwIfNotFlagsEnum">Whether to throw if the given enum does not have the <see cref="FlagsAttribute"/> attribute.</param>
        /// <param name="throwIfZero">Whether to throw if the given enum is zero.</param>
        /// <exception cref="ArgumentException"><paramref name="enum"/> does not have the <see cref="FlagsAttribute"/> and <paramref name="throwIfNotFlagsEnum"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="enum"/> is equal to zero and the <paramref name="throwIfZero"/> parameter is set to true or <paramref name="enum"/> is lesser than zero.</exception>
        /// <seealso cref="IsValidEnumValue(Enum)"/>
        /// <seealso cref="ThrowIfNotValidEnumValue(Enum)"/>
        public static void ThrowIfNotValidFlagsEnumValue(this Enum @enum, in string argumentName, in bool throwIfNotFlagsEnum, in bool throwIfZero)
        {
            if (!@enum.IsValidFlagsEnumValue(throwIfNotFlagsEnum, throwIfZero))

                throw new InvalidEnumArgumentException(argumentName, (int)Convert.ChangeType(@enum, TypeCode.Int32), @enum.GetType());
        }

        public static void ThrowIfNotFlagsEnum(this Enum value, in string argumentName)
        {
            if (!value.IsFlagsEnum())

                throw Util.GetExceptionForNonFlagsEnum(argumentName);
        }

        public static bool IsValidFlagsEnumValue<T>(this T value, in IfCT comparisonType, in string argumentName, params T[] values) where T : Enum
        {
            ThrowIfNull(values, nameof(values));

            Util.ThrowIfNotFlagsEnumType<T>(nameof(T));

#if WinCopies2
            comparisonType.ThrowIfNotValidEnumValue(nameof(comparisonType));
#else
            ThrowIfNotValidEnumValue(nameof(comparisonType), comparisonType);
#endif

            switch (comparisonType)
            {
                case IfCT.And:

                    foreach (T _value in values)

                        if (!value.HasFlag(_value))

                            return false;

                    return true;

                case IfCT.Or:

                    foreach (T _value in values)

                        if (value.HasFlag(_value))

                            return true;

                    return false;

                case IfCT.Xor:

                    bool oneResultFound = false;

                    foreach (T _value in values)

                        if (value.HasFlag(_value))

                            if (oneResultFound)

                                return false;

                            else

                                oneResultFound = true;

                    return oneResultFound;

                default:

                    Debug.Assert(false);

                    return false;
            }
        }

        public static void ThrowInInvalidFlagsEnumValue<T>(this T value, in IfCT comparisonType, in string argumentName, params T[] values) where T : Enum
        {
            if (!value.IsValidFlagsEnumValue(comparisonType, argumentName, values))

                throw GetInvalidEnumArgumentException(value, argumentName);
        }
#endif

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the enum value is not in the required enum value range. See the Remarks section.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <remarks>This method doesn't read all the enum fields, but only takes care of the first and last numeric enum fields, so if the value is 1, and the enum has only defined fields for 0 and 2, this method still doesn't throw. For a method that actually reads all the enum fields, see the <see cref="ThrowIfNotDefinedEnumValue(Enum)"/> method.</remarks>
        /// <seealso cref="IsValidEnumValue(Enum)"/>
        /// <seealso cref="ThrowIfNotValidEnumValue(Enum,in string)"/>
        public static void ThrowIfNotValidEnumValue(this Enum @enum)
        {
            if (!@enum.IsValidEnumValue()) throw new InvalidOperationException(string.Format(WinCopies.Util.Resources.ExceptionMessages.InvalidEnumValue, @enum.ToString()));
        }

        /// <summary>
        /// Throws an <see cref="InvalidEnumArgumentException"/> if the enum value is not in the required enum value range. See the Remarks section.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <param name="argumentName">The parameter name.</param>
        /// <remarks>This method doesn't read all the enum fields, but only takes care of the first and last numeric enum fields, so if the value is 1, and the enum has only defined fields for 0 and 2, this method still doesn't throw. For a method that actually reads all the enum fields, see the <see cref="ThrowIfNotDefinedEnumValue(Enum)"/> method.</remarks>
        /// <seealso cref="IsValidEnumValue(Enum)"/>
        /// <seealso cref="ThrowIfNotValidEnumValue(Enum)"/>
        public static void ThrowIfNotValidEnumValue(this Enum @enum, in string argumentName)
        {
            if (!@enum.IsValidEnumValue()) throw new InvalidEnumArgumentException(argumentName, @enum);
            // .GetType().IsEnumDefined(@enum)
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the enum value is not in the required enum value range.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <seealso cref="Type.IsEnumDefined(object)"/>
        /// <seealso cref="ThrowIfNotDefinedEnumValue(Enum,in string)"/>
        public static void ThrowIfNotDefinedEnumValue(this Enum @enum)
        {
            if (!@enum.GetType().IsEnumDefined(@enum)) throw new InvalidOperationException(string.Format(WinCopies.Util.Resources.ExceptionMessages.InvalidEnumValue, @enum.ToString()));
        }

        /// <summary>
        /// Throws an <see cref="InvalidEnumArgumentException"/> if the enum value is not in the required enum value range. See the Remarks section.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <param name="argumentName">The parameter name.</param>
        /// <remarks>This method doesn't read all the enum fields, but only takes care of the first and last numeric enum fields, so if the value is 1, and the enum has only defined fields for 0 and 2, this method still doesn't throw. For a method that actually reads all the enum fields, see the <see cref="ThrowIfNotDefinedEnumValue(Enum)"/> method.</remarks>
        /// <seealso cref="IsValidEnumValue(Enum)"/>
        /// <seealso cref="ThrowIfNotDefinedEnumValue(Enum)"/>
        public static void ThrowIfNotDefinedEnumValue(this Enum @enum, in string argumentName)
        {
            if (!(@enum ?? throw GetArgumentNullException(nameof(@enum))).GetType().IsEnumDefined(@enum))

                throw new InvalidEnumArgumentException(argumentName, @enum);
        }

        public static InvalidEnumArgumentException GetInvalidEnumArgumentException(this Enum value, string argumentName) => new InvalidEnumArgumentException(string.Format(ExceptionMessages.ParameterIsNotAnExpectedValue, value), argumentName, value);

        public static void ThrowIfInvalidEnumValue(this Enum value, in bool valuesAreExpected, params Enum[] values)
        {
            if (!value.IsValidEnumValue(valuesAreExpected, values))

                throw new InvalidOperationException(string.Format(ExceptionMessages.ParameterIsNotAnExpectedValue, value));
        }

        public static void ThrowIfInvalidEnumValue(this Enum value, in bool valuesAreExpected, in string argumentName, params Enum[] values)
        {
            if (!value.IsValidEnumValue(valuesAreExpected, values))

                throw value.GetInvalidEnumArgumentException(argumentName);
        }

        public static void ThrowIfInvalidEnumValue<T>(this T value, in bool valuesAreExpected, params T[] values) where T : Enum
        {
            if (!value.IsValidEnumValue(valuesAreExpected, values))

                throw new InvalidOperationException(string.Format(ExceptionMessages.ParameterIsNotAnExpectedValue, value));
        }

        public static void ThrowIfInvalidEnumValue<T>(this T value, in bool valuesAreExpected, in string argumentName, params T[] values) where T : Enum
        {
            if (!value.IsValidEnumValue(valuesAreExpected, values))

                throw value.GetInvalidEnumArgumentException(argumentName);
        }
#endif

            public static bool IsValidEnumValue<T>(this T value, in bool valuesAreExpected, params T[] values) where T : Enum
            {
                ThrowIfNull(values, nameof(values));

                if (valuesAreExpected)
                {
                    foreach (T _value in values)

                        if ((Enum)_value == (Enum)value)

                            return true;

                    return false;
                }

                foreach (T _value in values)

                    if ((Enum)_value == (Enum)value)

                        return false;

                return true;
            }

            /// <summary>
            /// Checks if a number is between two given numbers.
            /// </summary>
            /// <param name="b">The number to check.</param>
            /// <param name="x">The left operand.</param>
            /// <param name="y">The right operand.</param>
            /// <returns><see langword="true"/> if <paramref name="b"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
            public static bool Between(this sbyte b, in sbyte x, in sbyte y) => b >= x && b <= y;

            /// <summary>
            /// Checks if a number is between two given numbers.
            /// </summary>
            /// <param name="b">The number to check.</param>
            /// <param name="x">The left operand.</param>
            /// <param name="y">The right operand.</param>
            /// <returns><see langword="true"/> if <paramref name="b"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
            public static bool Between(this byte b, in byte x, in byte y) => b >= x && b <= y;

            /// <summary>
            /// Checks if a number is between two given numbers.
            /// </summary>
            /// <param name="s">The number to check.</param>
            /// <param name="x">The left operand.</param>
            /// <param name="y">The right operand.</param>
            /// <returns><see langword="true"/> if <paramref name="s"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
            public static bool Between(this short s, in short x, in short y) => s >= x && s <= y;

            /// <summary>
            /// Checks if a number is between two given numbers.
            /// </summary>
            /// <param name="s">The number to check.</param>
            /// <param name="x">The left operand.</param>
            /// <param name="y">The right operand.</param>
            /// <returns><see langword="true"/> if <paramref name="s"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
            public static bool Between(this ushort s, in ushort x, in ushort y) => s >= x && s <= y;

            /// <summary>
            /// Checks if a number is between two given numbers.
            /// </summary>
            /// <param name="i">The number to check.</param>
            /// <param name="x">The left operand.</param>
            /// <param name="y">The right operand.</param>
            /// <returns><see langword="true"/> if <paramref name="i"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
            public static bool Between(this int i, in int x, in int y) => i >= x && i <= y;

            /// <summary>
            /// Checks if a number is between two given numbers.
            /// </summary>
            /// <param name="i">The number to check.</param>
            /// <param name="x">The left operand.</param>
            /// <param name="y">The right operand.</param>
            /// <returns><see langword="true"/> if <paramref name="i"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
            public static bool Between(this uint i, in uint x, in uint y) => i >= x && i <= y;

            /// <summary>
            /// Checks if a number is between two given numbers.
            /// </summary>
            /// <param name="l">The number to check.</param>
            /// <param name="x">The left operand.</param>
            /// <param name="y">The right operand.</param>
            /// <returns><see langword="true"/> if <paramref name="l"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
            public static bool Between(this long l, in long x, in long y) => l >= x && l <= y;

            /// <summary>
            /// Checks if a number is between two given numbers.
            /// </summary>
            /// <param name="l">The number to check.</param>
            /// <param name="x">The left operand.</param>
            /// <param name="y">The right operand.</param>
            /// <returns><see langword="true"/> if <paramref name="l"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
            public static bool Between(this ulong l, in ulong x, in ulong y) => l >= x && l <= y;

            /// <summary>
            /// Checks if a number is between two given numbers.
            /// </summary>
            /// <param name="f">The number to check.</param>
            /// <param name="x">The left operand.</param>
            /// <param name="y">The right operand.</param>
            /// <returns><see langword="true"/> if <paramref name="f"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
            public static bool Between(this float f, in float x, in float y) => f >= x && f <= y;

            /// <summary>
            /// Checks if a number is between two given numbers.
            /// </summary>
            /// <param name="d">The number to check.</param>
            /// <param name="x">The left operand.</param>
            /// <param name="y">The right operand.</param>
            /// <returns><see langword="true"/> if <paramref name="d"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
            public static bool Between(this double d, in double x, in double y) => d >= x && d <= y;

            /// <summary>
            /// Checks if a number is between two given numbers.
            /// </summary>
            /// <param name="d">The number to check.</param>
            /// <param name="x">The left operand.</param>
            /// <param name="y">The right operand.</param>
            /// <returns><see langword="true"/> if <paramref name="d"/> is between <paramref name="x"/> and <paramref name="y"/>, otherwise <see langword="false"/>.</returns>
            public static bool Between(this decimal d, in decimal x, in decimal y) => d >= x && d <= y;

#if WinCopies2
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
#endif

            public static void SplitValues<T, U, TContainer>(this System.Collections.Generic.IEnumerable<T> enumerable, in bool skipEmptyEnumerables, IValueSplitFactory<T, U, TContainer> splitFactory, params T[] separators) where T : struct where U : System.Collections.Generic.IEnumerable<T>
            {
                ThrowIfNull(enumerable, nameof(enumerable));
                ThrowIfNull(splitFactory, nameof(splitFactory));
                // ThrowIfNull(enumerableNullableValueEntryCallback, nameof(enumerableNullableValueEntryCallback));
                ThrowIfNull(separators, nameof(separators));

                if (separators.Length == 0)

                    throw new ArgumentException($"{nameof(separators)} does not contain values.");

                Predicate<T> predicate;

                if (separators.Length == 1)

                    predicate = value => /*value != null && */ value.Equals(separators[0]);

                else

                    predicate = value => separators.Contains(value);

                System.Collections.Generic.IEnumerator<T> enumerator = enumerable.GetEnumerator();

                enumerable = null;

                void subAddAndAdd(in T value)
                {
                    splitFactory.SubAdd(value);

                    splitFactory.Add(splitFactory.GetEnumerable());
                }

                if (skipEmptyEnumerables)
                {
                    if (enumerator.MoveNext())
                    {
                        T? value = enumerator.Current;

                        if (enumerator.MoveNext()) // There are more than one value.
                        {
                            value = null;

                            void tryAdd()
                            {
                                if (predicate(enumerator.Current) && splitFactory.SubCount > 0)
                                {
                                    splitFactory.Add(splitFactory.GetEnumerable());

                                    splitFactory.SubClear();
                                }

                                else

                                    splitFactory.SubAdd(enumerator.Current);
                            }

                            tryAdd();

                            while (enumerator.MoveNext())

                                tryAdd();
                        }

                        else // There is one value.
                        {
                            if (predicate(value.Value))

                                return;

                            else

                                subAddAndAdd(enumerator.Current);
                        }
                    }

                    else // There is no value.

                        return;
                }

                else if (enumerator.MoveNext())
                {
                    T? value = enumerator.Current;

                    if (enumerator.MoveNext()) // There are more than one value.
                    {
                        value = null;

                        void tryAdd()
                        {
                            if (predicate(enumerator.Current))
                            {
                                splitFactory.Add(splitFactory.GetEnumerable());

                                if (splitFactory.SubCount > 0)

                                    splitFactory.SubClear();
                            }

                            else

                                splitFactory.SubAdd(enumerator.Current);
                        }

                        tryAdd();

                        while (enumerator.MoveNext())

                            tryAdd();
                    }

                    else // There is one value.
                    {
                        if (predicate(value.Value))

                            splitFactory.Add(splitFactory.GetEnumerable());

                        else

                            subAddAndAdd(enumerator.Current);
                    }
                }

                else // There is no value.

                    splitFactory.Add(splitFactory.GetEnumerable());
            }

#if WinCopies2
        public static void SplitReferences<T, U, V, TContainer>(this System.Collections.Generic.IEnumerable<T> enumerable, in bool skipEmptyEnumerables, IRefSplitFactory<T, U, V, TContainer> splitFactory, params T[] separators) where T : class where U : INullableRefEntry<T> where V : IEnumerable<U>
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(splitFactory, nameof(splitFactory));
            // ThrowIfNull(enumerableNullableValueEntryCallback, nameof(enumerableNullableValueEntryCallback));
            ThrowIfNull(separators, nameof(separators));

            if (separators.Length == 0)

                throw new ArgumentException($"{nameof(separators)} does not contain any value.");

            Predicate<T> predicate;

            if (separators.Length == 1)

                predicate = value => value == null ? separators[0] == null : value.Equals(separators[0]);

            else

                predicate = value => separators.Contains(value);

            System.Collections.Generic.IEnumerator<T> enumerator = enumerable.GetEnumerator();

            enumerable = null;

            void subAddAndAdd(in T value)
            {
                splitFactory.SubAdd(splitFactory.GetValueContainer(value));

                splitFactory.Add(splitFactory.GetEnumerable());
            }

            if (skipEmptyEnumerables)
            {
                if (enumerator.MoveNext())
                {
                    T value = enumerator.Current;

                    if (enumerator.MoveNext()) // There are more than one value.
                    {
                        value = null;

                        void tryAdd()
                        {
                            if (predicate(enumerator.Current) && splitFactory.SubCount > 0)
                            {
                                splitFactory.Add(splitFactory.GetEnumerable());

                                splitFactory.SubClear();
                            }

                            else

                                splitFactory.SubAdd(splitFactory.GetValueContainer(enumerator.Current));
                        }

                        tryAdd();

                        while (enumerator.MoveNext())

                            tryAdd();
                    }

                    else // There is one value.
                    {
                        if (predicate(value))

                            return;

                        else

                            subAddAndAdd(enumerator.Current);
                    }
                }

                else // There is no value.

                    return;
            }

            else if (enumerator.MoveNext())
            {
                T value = enumerator.Current;

                if (enumerator.MoveNext()) // There are more than one value.
                {
                    value = null;

                    void tryAdd()
                    {
                        if (predicate(enumerator.Current))
                        {
                            if (splitFactory.SubCount == 0)
                            {
                                splitFactory.SubAdd(splitFactory.GetValueContainer(null));

                                splitFactory.Add(splitFactory.GetEnumerable());
                            }

                            else
                            {
                                splitFactory.Add(splitFactory.GetEnumerable());

                                splitFactory.SubClear();
                            }
                        }

                        else

                            splitFactory.SubAdd(splitFactory.GetValueContainer(enumerator.Current));
                    }

                    tryAdd();

                    while (enumerator.MoveNext())

                        tryAdd();
                }

                else // There is one value.
                {
                    if (predicate(value))
                    {
                        subAddAndAdd(null);

                        subAddAndAdd(null);
                    }

                    else

                        subAddAndAdd(enumerator.Current);
                }
            }

            else // There is no value.
            {
                splitFactory.SubAdd(splitFactory.GetValueContainer(null));

                splitFactory.Add(splitFactory.GetEnumerable());
            }
        }

        public static System.Collections.Generic.IEnumerable<T> Join<T>(this IEnumerable<System.Collections.Generic.IEnumerable<T>> enumerable, bool keepEmptyEnumerables, params T[] join) => new Enumerable<T>(() => new JoinEnumerator<T>(enumerable, keepEmptyEnumerables, join));

            #region String extension methods
            #region Split
        private static void Split(this string s, in bool skipEmptyValues, in StringBuilder stringBuilder, in Action<string> action, params char[] separators)
        {
            ThrowIfNull(s, nameof(s));
            ThrowIfNull(stringBuilder, nameof(stringBuilder));
            ThrowIfNull(separators, nameof(separators));

            if (separators.Length == 0)

                throw new ArgumentException($"{nameof(separators)} does not contain values.");

            Debug.Assert(action != null, $"{nameof(action)} must be not null.");

            Predicate<char> getPredicate()
            {
                Predicate<char> predicate;

                if (separators.Length == 1)

                    predicate = __c => __c == separators[0];

                else

                    predicate = __c => separators.Contains(__c);

                return predicate;
            }

            if (skipEmptyValues)

                if (s.Length == 0)

                    return;

                else if (s.Length == 1)

                    if ((separators.Length == 1 && s[0] == separators[0]) || separators.Contains(s[0]))

                        return;

                    else

                        action(s);

                else
                {
                    Predicate<char> predicate = getPredicate();

                    foreach (char _c in s)

                        if (predicate(_c) && stringBuilder.Length > 0)
                        {
                            action(stringBuilder.ToString());

                            _ = stringBuilder.Clear();
                        }

                        else

                            _ = stringBuilder.Append(_c);

                    if (stringBuilder.Length > 0)

                        action(stringBuilder.ToString());
                }

            else if (s.Length == 0)

                action("");

            else if (s.Length == 1)

                if ((separators.Length == 1 && s[0] == separators[0]) || separators.Contains(s[0]))
                {
                    action("");

                    action("");
                }

                else

                    action(s);

            else
            {
                Predicate<char> predicate = getPredicate();

                foreach (char _c in s)

                    if (predicate(_c))

                        if (stringBuilder.Length == 0)

                            action("");

                        else
                        {
                            action(stringBuilder.ToString());

                            _ = stringBuilder.Clear();
                        }

                    else

                        _ = stringBuilder.Append(_c);

                if (stringBuilder.Length > 0)

                    action(stringBuilder.ToString());
            }
        }

        public static System.Collections.Generic.Queue<string> SplitToQueue(this string s, in bool skipEmptyValues, params char[] separators)
        {
            var queue = new System.Collections.Generic.Queue<string>();

            SplitToQueue(s, skipEmptyValues, new StringBuilder(), queue, separators);

            return queue;
        }

        public static void SplitToQueue(this string s, in bool skipEmptyValues, in StringBuilder stringBuilder, System.Collections.Generic.Queue<string> queue, params char[] separators)
        {
            ThrowIfNull(queue, nameof(queue));

            Split(s, skipEmptyValues, stringBuilder, _s => queue.Enqueue(_s), separators);
        }

        public static System.Collections.Generic.Stack<string> SplitToStack(this string s, in bool splitEmptyValues, params char[] separators)
        {
            var stack = new System.Collections.Generic.Stack<string>();

            SplitToStack(s, splitEmptyValues, new StringBuilder(), stack, separators);

            return stack;
        }

        public static void SplitToStack(this string s, in bool splitEmptyValues, in StringBuilder stringBuilder, System.Collections.Generic.Stack<string> stack, params char[] separators)
        {
            ThrowIfNull(stack, nameof(stack));

            Split(s, splitEmptyValues, stringBuilder, _s => stack.Push(_s), separators);
        }

        public static System.Collections.Generic.LinkedList<string> SplitToLinkedList(this string s, in bool splitEmptyValues, params char[] separators)
        {
            var list = new System.Collections.Generic.LinkedList<string>();

            SplitToLinkedList(s, splitEmptyValues, new StringBuilder(), list, separators);

            return list;
        }

        public static void SplitToLinkedList(this string s, in bool splitEmptyValues, in StringBuilder stringBuilder, System.Collections.Generic.LinkedList<string> list, params char[] separators)
        {
            ThrowIfNull(list, nameof(list));

            Split(s, splitEmptyValues, stringBuilder, _s => list.AddLast(_s), separators);
        }

            #endregion

        public static string Join(this IEnumerable<string> enumerable, in bool keepEmptyValues, params char[] join) => Join(enumerable, keepEmptyValues, new string(join));

        public static string Join(this IEnumerable<string> enumerable, in bool keepEmptyValues, in string join, StringBuilder stringBuilder = null)
        {
            System.Collections.Generic.IEnumerator<string> enumerator = (enumerable ?? throw GetArgumentNullException(nameof(enumerable))).GetEnumerator();

#if CS8
            stringBuilder ??= new StringBuilder();
#else
            if (stringBuilder == null)

                stringBuilder = new StringBuilder();
#endif

            try
            {
                void append() => _ = stringBuilder.Append(enumerator.Current);

                bool moveNext() => enumerator.MoveNext();

                if (moveNext())

                    append();

                while (moveNext() && (keepEmptyValues || enumerator.Current.Length > 0))
                {
                    _ = stringBuilder.Append(join);

                    append();
                }
            }
            finally
            {
                enumerator.Dispose();
            }

            return stringBuilder.ToString();
        }

        // todo: add other methods and overloads for StringComparison, IEqualityComparer<char>, Comparer<char>, Comparison<char>, ignore case and CultureInfo parameters

        [Obsolete("This method has been replaced by the Contains(this string, string, IEqualityComparer<char>) method.")]
        public static bool Contains(this string s, System.Collections.Generic.IEqualityComparer<char> comparer, string value) => s.Contains(value, comparer);

        public static bool Contains(this string s, string value, System.Collections.Generic.IEqualityComparer<char> comparer)
        {
            bool contains(ref int i)
            {
                for (int j = 0; j < value.Length; j++)

                    if (!comparer.Equals(s[i + j], value[j]))

                        return false;

                return true;
            }

            for (int i = 0; i < s.Length; i++)
            {
                if (value.Length > s.Length - i)

                    return false;

                if (contains(ref i))

                    return true;
            }

            return false;
        }

        // todo: To replace by arrays-common methods

        public static bool Contains(this string s, char value, out int index)
        {
            for (int i = 0; i < s.Length; i++)

                if (s[i] == value)
                {
                    index = i;

                    return true;
                }

            index = default;

            return false;
        }

        public static bool Contains(this string s, string value, out int index)
        {
            bool contains(ref int i)
            {
                for (int j = 0; j < value.Length; j++)

                    if (s[i + j] != value[j])

                        return false;

                return true;
            }

            for (int i = 0; i < s.Length; i++)
            {

                if (value.Length > s.Length - i)
                {
                    index = -1;

                    return false;
                }

                if (contains(ref i))
                {
                    index = i;

                    return true;
                }
            }

            index = -1;

            return false;
        }

        [Obsolete("This method has been replaced by arrays-common methods.")]
        public static bool Contains(this string s, char value, System.Collections.Generic.IEqualityComparer<char> comparer, out int index)
        {
            for (int i = 0; i < s.Length; i++)

                if (comparer.Equals(s[i], value))

                {
                    index = i;

                    return true;
                }

            index = default;

            return false;
        }

        public static bool Contains(this string s, string value, System.Collections.Generic.IEqualityComparer<char> comparer, out int index)
        {
            bool contains(ref int i)
            {
                for (int j = 0; j < value.Length; j++)

                    if (!comparer.Equals(s[i + j], value[j]))

                        return false;

                return true;
            }

            for (int i = 0; i < s.Length; i++)

            {
                if (value.Length > s.Length - i)
                {
                    index = -1;

                    return false;
                }

                if (contains(ref i))
                {
                    index = i;

                    return true;
                }
            }

            index = -1;

            return false;
        }

        public static bool StartsWith(this string s, char value) => s[0] == value;

        public static string RemoveAccents(this string s)
        {
            var stringBuilder = new StringBuilder();

            s = s.Normalize(NormalizationForm.FormD);

            foreach (char c in s)

                if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)

                    _ = stringBuilder.Append(c);

            return stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

            #endregion

        internal static void ThrowIfDisposedInternal(this WinCopies.Util.DotNetFix.IDisposable obj, in string objectName)
        {
            if (obj.IsDisposed)

                throw GetExceptionForDispose(objectName, false);
        }

        public static void ThrowIfDisposed(this WinCopies.Util.DotNetFix.IDisposable obj, in string objectName)
        {
            ThrowIfNull(obj, nameof(obj));

            obj.ThrowIfDisposedInternal(objectName);
        }

        internal static void ThrowIfDisposingInternal(this IDisposable obj, in string objectName)
        {
            if (obj.IsDisposing)

                throw GetExceptionForDispose(objectName, true);
        }

        public static void ThrowIfDisposing(this IDisposable obj, in string objectName)
        {
            ThrowIfNull(obj, nameof(obj));

            obj.ThrowIfDisposingInternal(objectName);
        }

        public static void ThrowIfDisposingOrDisposed(this IDisposable obj, in string objectName)
        {
            ThrowIfNull(obj, nameof(obj));

            obj.ThrowIfDisposedInternal(objectName);

            obj.ThrowIfDisposingInternal(objectName);
        }
#endif

            public static string Repeat(this char c, in int length) => c.Repeat(null, null, length);

            public static string Repeat(this char c, in char left, in char right, in int length) => c.Repeat(left, right, length);

            private static string Repeat(this char c, in char? left, in char? right, in int length)
            {
                var sb = new StringBuilder();

                Action action;

                if (left.HasValue)

                {

                    char _left = left.Value;

                    if (right.HasValue)

                    {
                        char _right = right.Value;

                        action = () => { _ = sb.Append(_left); _ = sb.Append(c); _ = sb.Append(_right); };
                    }

                    else

                        action = () => { _ = sb.Append(_left); _ = sb.Append(c); };

                }

                else if (right.HasValue)

                {
                    char _right = right.Value;

                    action = () => { _ = sb.Append(c); _ = sb.Append(_right); };
                }

                else

                    action = () => sb.Append(c);

                for (int i = 0; i < length; i++)

                    action();

                return sb.ToString();
            }

            public static string Repeat(this string s, in int length) => s.Repeat(null, null, length);

            public static string Repeat(this string s, string left, string right, in int length)
            {
                var sb = new StringBuilder();

                Action action;

                if (left != null)

                {

                    if (right == null)

                        action = () => { _ = sb.Append(left); _ = sb.Append(s); };

                    else

                        action = () => { _ = sb.Append(left); _ = sb.Append(s); _ = sb.Append(right); };

                }

                else if (right != null)

                    action = () => { _ = sb.Append(s); _ = sb.Append(right); };

                else

                    action = () => sb.Append(s);

                for (int i = 0; i < length; i++)

                    action();

                return sb.ToString();
            }
        }
#if WinCopies3
    }
#endif
}
