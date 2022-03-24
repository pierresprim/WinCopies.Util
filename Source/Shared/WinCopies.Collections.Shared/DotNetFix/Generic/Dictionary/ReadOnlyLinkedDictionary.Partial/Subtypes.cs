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

using WinCopies.Collections.Enumeration.Generic;
using WinCopies.Collections.Generic;

using static WinCopies.Delegates;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public partial class ReadOnlyLinkedDictionary<TKey, TValue>
    {
        public abstract class DictionaryItemCollectionBase<T> : IReadOnlyUIntCollection<T>
        {
            protected ReadOnlyLinkedDictionary<TKey, TValue> InnerDictionary { get; }

            public uint Count => InnerDictionary.Count;

            public DictionaryItemCollectionBase(in ReadOnlyLinkedDictionary<TKey, TValue> dictionary) => InnerDictionary = dictionary;

            protected abstract T Convert(KeyValuePair<TKey, TValue> value);

            protected abstract bool CompareEquality(in T x, in T y);

            public bool Contains(T item)
            {
                foreach (KeyValuePair<TKey, TValue> _item in InnerDictionary)

                    if (CompareEquality(Convert(_item), item))

                        return true;

                return false;
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                WinCopies.ThrowHelper.ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, Count, nameof(array), nameof(arrayIndex));

                void defaultAction(in T value) => array[arrayIndex] = value;

                Action<T> action = value =>
                {
                    action = _value =>
                    {
                        ++arrayIndex;

                        defaultAction(_value);
                    };

                    defaultAction(value);
                };

                foreach (T item in this)

                    action(item);
            }

            public IUIntCountableEnumerator<T> GetEnumerator() => new UIntCountableEnumerator<SelectEnumerator<KeyValuePair<TKey, TValue>, T>, T>(new SelectEnumerator<KeyValuePair<TKey, TValue>, T>(InnerDictionary, Convert), () => Count);

#if !CS8
                    System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
#endif

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public abstract class DictionaryItemCollection<T> : DictionaryItemCollectionBase<T>
        {
            protected abstract Converter<KeyValuePair<TKey, TValue>, T> Converter { get; }

            public DictionaryItemCollection(in ReadOnlyLinkedDictionary<TKey, TValue> dictionary) : base(dictionary) { /* Left empty. */ }

            protected sealed override T Convert(KeyValuePair<TKey, TValue> value) => Converter(value);
        }

        public class KeyCollection : DictionaryItemCollection<TKey>
        {
            protected override Converter<KeyValuePair<TKey, TValue>, TKey> Converter => GetKey;

            public KeyCollection(in ReadOnlyLinkedDictionary<TKey, TValue> dictionary) : base(dictionary) { /* Left empty. */ }

            public static KeyCollection GetNewCollection(ReadOnlyLinkedDictionary<TKey, TValue> dictionary) => new
#if !CS9
                KeyCollection
#endif
                (dictionary);

            protected override bool CompareEquality(in TKey x, in TKey y) => CompareHashCodeGenericIn(x, y);
        }

        public class ValueCollection : DictionaryItemCollection<TValue>
        {
            protected override Converter<KeyValuePair<TKey, TValue>, TValue> Converter => GetValue;

            public ValueCollection(in ReadOnlyLinkedDictionary<TKey, TValue> dictionary) : base(dictionary) { /* Left empty. */ }

            public static ValueCollection GetNewCollection(ReadOnlyLinkedDictionary<TKey, TValue> dictionary) => new
#if !CS9
                ValueCollection
#endif
                (dictionary);

            protected override bool CompareEquality(in TValue x, in TValue y) => CompareHashCodeGenericIn(x, y);
        }
    }
}
#endif
