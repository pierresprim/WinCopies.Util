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

#if WinCopies3

using System;
using System.Collections.Generic;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
    /// <summary>
    /// Collection-oriented helper methods.
    /// </summary>
    public static class Util
    {
#if CS5
        public static ILinkedListNode<KeyValuePair<TKey,TValue>> Find<TDictionary, TKey, TValue>( in TDictionary dictionary, in TKey key) where TDictionary : DotNetFix.Generic.IDictionary<TKey, TValue>, ILinkedList3<KeyValuePair<TKey, TValue>>
        {
            var enumerable = new Enumerable<ILinkedListNode<KeyValuePair<TKey, TValue>>>(dictionary.GetNodeEnumerator);

            foreach (ILinkedListNode<KeyValuePair<TKey, TValue>> node in enumerable)

                if (Delegates.CompareHashCodeGenericIn(node.Value.Key, key))

                    return node;

            return null;
        }

        public static bool Remove<TDictionary, TKey, TValue>(in TDictionary dictionary, in TKey key) where TDictionary : DotNetFix.Generic.IDictionary<TKey, TValue>, ILinkedList3<KeyValuePair<TKey, TValue>>
        {
            ILinkedListNode<KeyValuePair<TKey, TValue>> node = Find<TDictionary,TKey,TValue>(dictionary,key);

            if (node == null)

                return false;

            dictionary.Remove(node);

            return true;
        }
#endif

        private static bool _HasItems<T>(in System.Collections.Generic.IEnumerable<T> enumerable)
        {
            bool result;

            using (var enumerator = new EmptyCheckEnumerator(enumerable.GetEnumerator()))

                result = enumerator.HasItems;

            return result;
        }

        public static bool TryGetHasItems<T>(in System.Collections.Generic.IEnumerable<T> enumerable) => enumerable == null ? false : _HasItems(enumerable);

        public static bool HasItems<T>(in System.Collections.Generic.IEnumerable<T> enumerable) => _HasItems(enumerable ?? throw GetArgumentNullException(nameof(enumerable)));

#if CS7
        public static System.Collections.Generic.IEnumerator<Collections.DotNetFix.
#if WinCopies3
            Generic.
#endif
            ILinkedListNode<T>> GetNodeEnumerator<T>(in ILinkedList<T> list, in EnumerationDirection enumerationDirection, in DotNetFix.
#if WinCopies3
            Generic.
#endif
      ILinkedListNode<T> start, DotNetFix.
#if WinCopies3
            Generic.
#endif
      ILinkedListNode<T> end) => new LinkedListEnumerator<T>(list, enumerationDirection, start, end);
#endif

        public static System.Collections.Generic.IEnumerable<T> GetEmptyEnumerable<T>() => new WinCopies.Collections.Generic.Enumerable<T>(() => new EmptyEnumerator<T>());

        /// <summary>
        /// Returns a value obtained by a <see cref="Func"/>, depending on the result of a comparison.
        /// </summary>
        /// <param name="x">The value to compare to <paramref name="y"/>.</param>
        /// <param name="y">The value to compare to <paramref name="x"/>.</param>
        /// <param name="comparison">The comparison delegate.</param>
        /// <param name="lower">The <see cref="Func"/> that provides the value for <paramref name="x"/> is lower than <paramref name="y"/>.</param>
        /// <param name="equals">The <see cref="Func"/> that provides the value for <paramref name="x"/> is equal to <paramref name="y"/>.</param>
        /// <param name="greater">The <see cref="Func"/> that provides the value for <paramref name="x"/> is greater than <paramref name="y"/>.</param>
        /// <returns>A value obtained by a <see cref="Func"/>, depending on the result of a comparison.</returns>
        /// <exception cref="ArgumentNullException">One or more of the given <see cref="Func"/>s are <see langword="null"/>.</exception>
        /// <remarks>See <see cref="GetIf{TValues, TResult}(in TValues, in TValues, in Comparison{TValues}, in Func{TResult}, in Func{TResult}, in Func{TResult})"/> for the generic version.</remarks>
        public static object GetIf(in object x, in object y, in WinCopies.Collections.Comparison comparison, in Func lower, in Func equals, in Func greater)
        {
            ThrowIfNull(lower, nameof(lower));
            ThrowIfNull(greater, nameof(greater));
            ThrowIfNull(equals, nameof(equals));

            int result = comparison(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();
        }

        /// <summary>
        /// Returns a value obtained by a <see cref="Func"/>, depending on the result of a comparison.
        /// </summary>
        /// <param name="x">The value to compare to <paramref name="y"/>.</param>
        /// <param name="y">The value to compare to <paramref name="x"/>.</param>
        /// <param name="comparison">The comparison delegate.</param>
        /// <param name="lower">The <see cref="Func"/> that provides the value for <paramref name="x"/> is lower than <paramref name="y"/>.</param>
        /// <param name="equals">The <see cref="Func"/> that provides the value for <paramref name="x"/> is equal to <paramref name="y"/>.</param>
        /// <param name="greater">The <see cref="Func"/> that provides the value for <paramref name="x"/> is greater than <paramref name="y"/>.</param>
        /// <returns>A value obtained by a <see cref="Func"/>, depending on the result of a comparison.</returns>
        /// <exception cref="ArgumentNullException">One or more of the given <see cref="Func"/>s are <see langword="null"/>.</exception>
        /// <remarks>See <see cref="GetIf(in object, in object, in Collections.Comparison, in Func, in Func, in Func)"/> for the non-generic version.</remarks>
        public static TResult GetIf<TValues, TResult>(in TValues x, in TValues y, in Comparison<TValues> comparison, in Func<TResult> lower, in Func<TResult> equals, in Func<TResult> greater)
        {
            ThrowIfNull(lower, nameof(lower));
            ThrowIfNull(greater, nameof(greater));
            ThrowIfNull(equals, nameof(equals));

            int result = comparison(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();
        }

        public static object GetIf(in object x, in object y, in IComparer comparer, in Func lower, in Func equals, in Func greater)
        {
            ThrowIfNull(lower, nameof(lower));
            ThrowIfNull(greater, nameof(greater));
            ThrowIfNull(equals, nameof(equals));

            int result = comparer.Compare(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();
        }

        public static TResult GetIf<TValues, TResult>(in TValues x, in TValues y, in WinCopies.Collections.Comparison comparison, in Func<TResult> lower, in Func<TResult> equals, in Func<TResult> greater)
        {
            ThrowIfNull(lower, nameof(lower));
            ThrowIfNull(greater, nameof(greater));
            ThrowIfNull(equals, nameof(equals));

            int result = comparison(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();
        }
    }
}
#endif
