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

#if CS5
using System;
using System.Collections.Generic;

using WinCopies.Collections.Generic;
using WinCopies.Util;

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class LinkedDictionary<TKey, TValue> : ReadOnlyLinkedDictionary<TKey, TValue>, ILinkedDictionary<TKey, TValue>
    {
        public bool IsReadOnly => false;

        public new TValue this[TKey key] { get => base[key]; set => (Find(key) ?? throw GetKeyNotFoundException(nameof(key))).Value = new KeyValuePair<TKey, TValue>(key, value); }

        public new ILinkedListNodeBase<KeyValuePair<TKey, TValue>> First => InnerList.First;
        public new ILinkedListNodeBase<KeyValuePair<TKey, TValue>> Last => InnerList.Last;
#if !CS8
        IReadOnlyLinkedListNodeBase<KeyValuePair<TKey, TValue>> IReadOnlyLinkedDictionary<TKey, TValue>.First => First;
        IReadOnlyLinkedListNodeBase<KeyValuePair<TKey, TValue>> IReadOnlyLinkedDictionary<TKey, TValue>.Last => Last;
#endif
        protected LinkedDictionary(in ILinkedList<KeyValuePair<TKey, TValue>> list, in Converter<ReadOnlyLinkedDictionary<TKey, TValue>, IReadOnlyUIntCollection<TKey>> keyCollectionConverter, in Converter<ReadOnlyLinkedDictionary<TKey, TValue>, IReadOnlyUIntCollection<TValue>> valueCollectionConverter) : base(list, keyCollectionConverter, valueCollectionConverter) { /* Left empty. */ }

        public LinkedDictionary(in ILinkedList<KeyValuePair<TKey, TValue>> list) : base(list) { /* Left empty. */ }
        public LinkedDictionary() : base() { /* Left empty. */ }

        protected virtual void OnAdding(KeyValuePair<TKey, TValue> item) => InnerList.AsFromType<System.Collections.Generic.ICollection<KeyValuePair<TKey, TValue>>>().Add(item);

        protected void Add(in KeyValuePair<TKey, TValue> item, in string exceptionMessage, in string paramName) => OnAdding(Keys.Contains(item.Key) ? throw new ArgumentException(exceptionMessage, paramName) : item);
        public void Add(KeyValuePair<TKey, TValue> item) => Add(item, "The key of the given item is already registered.", nameof(item));
        public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value), "The given key is already registered.", nameof(key));

        public void Clear() => InnerList.AsFromType<IClearable>().Clear();

        public bool Remove(TKey key)
        {
            var node = Find(key);

            if (node == null)

                return false;

            InnerList.Remove(node);

            return true;
        }

        public KeyValuePair<TKey, TValue> RemoveFirst() => InnerList.RemoveAndGetFirstValue();
        public KeyValuePair<TKey, TValue> RemoveLast() => InnerList.RemoveAndGetLastValue();

        bool ICollectionBase<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);
    }
}
#endif
