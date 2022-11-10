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

#if CS5
using System;
using System.Collections.Generic;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class SortedLinkedDictionary<TKey, TValue> : LinkedDictionary<TKey, TValue>
    {
        protected IUIntCountable InnerCountable => InnerList;

        public bool Descending { get; }

        public SortedLinkedDictionary(in bool descending = false) : base() => Descending = descending;

        protected ILinkedListNode<KeyValuePair<TKey, TValue>> TryGetNearestNode(in TKey item)
        {
            if (InnerCountable.Count > 0)
            {
#if CS8
                static
#endif
                    int getDescendingComparisonResult(in int result) => -result;

                FuncIn<int> getComparisonResult;
                FuncIn<int?, int?, bool> getBoolComparisonResult;

                if (Descending)
                {
                    getComparisonResult = getDescendingComparisonResult;
                    getBoolComparisonResult = (in int? x, in int? y) => x > y;
                }

                else
                {
                    getComparisonResult = Delegates.SelfIn;
                    getBoolComparisonResult = (in int? x, in int? y) => x < y;
                }

                foreach (ILinkedListNode<KeyValuePair<TKey, TValue>> node in InnerList.AsNodeEnumerable())

                    if (node.Value.Key is IComparable<TKey> _key)
                    {
                        if (getComparisonResult(_key.CompareTo(item)) > 0)

                            return node;
                    }

                    else if (item is IComparable<TKey> __key)
                    {
                        if (getComparisonResult(__key.CompareTo(node.Value.Key)) < 0)

                            return node;
                    }

                    else if (getBoolComparisonResult(item?.GetHashCode(), node.Value.Key?.GetHashCode()))

                        return node;
            }

            return null;
        }

        protected override void OnAdding(KeyValuePair<TKey, TValue> item)
        {
            if (InnerCountable.Count > 0)
            {
                ILinkedListNode<KeyValuePair<TKey, TValue>> node = TryGetNearestNode(item.Key);

                if (node != null)

                    _ = InnerList.AddBefore(node, item);
            }

            InnerCollection.Add(item);
        }

        public bool TryAddBeforeNearest(TKey key, Converter<KeyValuePair<TKey, TValue>, TValue> func, out KeyValuePair<TKey, TValue> nearestFound)
        {
            if (InnerCountable.Count > 0)
            {
                nearestFound = default;

                return false;
            }

            ILinkedListNode<KeyValuePair<TKey, TValue>> node = TryGetNearestNode(key);

            if (node == null)
            {
                nearestFound = default;

                return false;
            }

            nearestFound = node.Value;

            KeyValuePair<TKey, TValue> item = new
#if !CS9
                KeyValuePair<TKey, TValue>
#endif
                (key, (func ?? throw WinCopies.ThrowHelper.GetArgumentNullException(nameof(func)))(nearestFound));

            _ = InnerList.AddBefore(node, item);

            return true;
        }

        public bool TryAddBeforeNearest(KeyValuePair<TKey, TValue> item, out KeyValuePair<TKey, TValue> nearestFound) => TryAddBeforeNearest(item.Key, _nearestFound => item.Value, out nearestFound);
    }
}
#endif
