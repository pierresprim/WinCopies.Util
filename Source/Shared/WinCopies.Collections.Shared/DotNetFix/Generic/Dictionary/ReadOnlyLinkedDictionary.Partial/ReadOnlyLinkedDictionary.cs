/* Copyright © Pierre Sprimont, 2021
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

#if WinCopies3 && CS5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using WinCopies.Collections.Generic;

using static WinCopies.Collections.ThrowHelper;
using static WinCopies.Delegates;
using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public partial class ReadOnlyLinkedDictionary<TKey, TValue> : IReadOnlyLinkedDictionary<TKey, TValue>
    {
        protected ILinkedList<KeyValuePair<TKey, TValue>> InnerList { get; }

        public IReadOnlyUIntCollection<TKey> Keys { get; }

        public IReadOnlyUIntCollection<TValue> Values { get; }

        public uint Count => InnerList.Count;

        public IReadOnlyLinkedListNodeBase<KeyValuePair<TKey, TValue>> First => InnerList.First.ToReadOnly();

        public IReadOnlyLinkedListNodeBase<KeyValuePair<TKey, TValue>> Last => InnerList.Last.ToReadOnly();

        public TValue this[TKey key] => TryGetValue(key, out TValue value) ? value : throw GetKeyNotFoundException(nameof(key));

        protected ReadOnlyLinkedDictionary(in ILinkedList<KeyValuePair<TKey, TValue>> list, in Converter<ReadOnlyLinkedDictionary<TKey, TValue>, IReadOnlyUIntCollection<TKey>> keyCollectionConverter, in Converter<ReadOnlyLinkedDictionary<TKey, TValue>, IReadOnlyUIntCollection<TValue>> valueCollectionConverter)
        {
            InnerList = list ?? throw GetArgumentNullException(nameof(list));

            IReadOnlyUIntCollection<T> getCollection<T>(in Converter<ReadOnlyLinkedDictionary<TKey, TValue>, IReadOnlyUIntCollection<T>> converter, in string paramName) => (converter ?? throw GetArgumentNullException(paramName))(this);

            Keys = getCollection(keyCollectionConverter, nameof(keyCollectionConverter));

            Values = getCollection(valueCollectionConverter, nameof(valueCollectionConverter));
        }

        public ReadOnlyLinkedDictionary(in ILinkedList<KeyValuePair<TKey, TValue>> list) : this(list, KeyCollection.GetNewCollection, ValueCollection.GetNewCollection) { /* Left empty. */ }

        protected ReadOnlyLinkedDictionary() : this(new LinkedList<KeyValuePair<TKey, TValue>>()) { /* Left empty. */ }

        public bool Contains(KeyValuePair<TKey, TValue> item) => InnerList.Contains(item);

        public bool ContainsKey(TKey key) => Keys.Contains(key);

        public bool ContainsValue(TValue value) => Values.Contains(value);

        public bool TryGetValue(in TKey key, in EqualityComparison<TKey> comparison, out TValue value)
        {
            ThrowIfNull(comparison, nameof(comparison));

            foreach (KeyValuePair<TKey, TValue> item in this)

                if (comparison(item.Key, key))
                {
                    value = item.Value;

                    return true;
                }

            value = default;

            return false;
        }

        public bool TryGetValue(TKey key,
#if CS8
                    [MaybeNullWhen(false)] 
#endif
                    out TValue value) => TryGetValue(key, CompareHashCodeGeneric, out value);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        protected ILinkedListNode<KeyValuePair<TKey, TValue>> Find(TKey key)
        {
            foreach (ILinkedListNode<KeyValuePair<TKey, TValue>> item in InnerList.AsNodeEnumerable())

                if (CompareHashCodeGenericIn(key, item.Value.Key))

                    return item;

            return null;
        }

        public IUIntCountableEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => InnerList.GetEnumerator();

#if !CS8
        System.Collections.Generic.IEnumerator<KeyValuePair<TKey, TValue>> System.Collections.Generic.IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#endif
    }
}
#endif
