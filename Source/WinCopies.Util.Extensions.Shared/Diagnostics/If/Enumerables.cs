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
using System.Collections.Generic;
using WinCopies.Util;

namespace WinCopies.Diagnostics
{
    internal interface IIfValuesEnumerable
    {
        Array Array { get; }

        KeyValuePair<object, Func<bool>> GetValue(in int index);
    }

    internal class IfValuesEnumerable : IIfValuesEnumerable
    {
        private static KeyValuePair<object, Func<bool>> GetValue(in object[] array, in int index, Predicate predicate)
        {
            object result = array[index];

            return new KeyValuePair<object, Func<bool>>(result, () => predicate(result));
        }

        public object[] Array { get; }

        Array IIfValuesEnumerable.Array => Array;

        public Predicate Predicate { get; }

        public IfValuesEnumerable(in object[] array, in Predicate predicate)
        {
            Array = array;

            Predicate = predicate;
        }

        public KeyValuePair<object, Func<bool>> GetValue(in int index) => GetValue(Array, index, Predicate);
    }

    internal class IfKeyValuePairEnumerable : IIfValuesEnumerable
    {
        public KeyValuePair<object, Func<bool>>[] Array { get; }

        Array IIfValuesEnumerable.Array => Array;

        public IfKeyValuePairEnumerable(in KeyValuePair<object, Func<bool>>[] array) => Array = array;

        public KeyValuePair<object, Func<bool>> GetValue(in int index) => Array[index];
    }

    internal interface IIfKeyValuesEnumerable
    {
        Array Array { get; }

        KeyValuePair<object, KeyValuePair<object, Func<bool>>> GetValue(in int index);
    }

    internal class IfKeyValuesEnumerable : IIfKeyValuesEnumerable
    {
        private static KeyValuePair<object, KeyValuePair<object, Func<bool>>> GetValue(in KeyValuePair<object, object>[] array, in int index, Predicate predicate)

        {
            KeyValuePair<object, object> result = array[index];

            return new KeyValuePair<object, KeyValuePair<object, Func<bool>>>(result.Key, new KeyValuePair<object, Func<bool>>(result.Value, () => predicate(result.Value)));
        }

        public KeyValuePair<object, object>[] Array { get; }

        Array IIfKeyValuesEnumerable.Array => Array;

        public Predicate Predicate { get; }

        public IfKeyValuesEnumerable(in KeyValuePair<object, object>[] array, in Predicate predicate)
        {
            Array = array;

            Predicate = predicate;
        }

        public KeyValuePair<object, KeyValuePair<object, Func<bool>>> GetValue(in int index) => GetValue(Array, index, Predicate);
    }

    internal class IfKeyKeyValuePairEnumerable : IIfKeyValuesEnumerable
    {
        public KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] Array { get; }

        Array IIfKeyValuesEnumerable.Array => Array;

        public IfKeyKeyValuePairEnumerable(KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] array) => Array = array;

        public KeyValuePair<object, KeyValuePair<object, Func<bool>>> GetValue(in int index) => Array[index];
    }

    internal interface IIfValuesEnumerable<T>
    {
        Array Array { get; }

        KeyValuePair<T, Func<bool>> GetValue(in int index);
    }

    internal class IfValuesEnumerable<T> : IIfValuesEnumerable<T>
    {
        private static KeyValuePair<T, Func<bool>> GetValue(in T[] array, in int index, Predicate<T> predicate)
        {
            T result = array[index];

            return new KeyValuePair<T, Func<bool>>(result, () => predicate(result));
        }

        public T[] Array { get; }

        Array IIfValuesEnumerable<T>.Array => Array;

        public Predicate<T> Predicate { get; }

        public IfValuesEnumerable(in T[] array, in Predicate<T> predicate)
        {
            Array = array;

            Predicate = predicate;
        }

        public KeyValuePair<T, Func<bool>> GetValue(in int index) => GetValue(Array, index, Predicate);
    }

    internal class IfKeyValuePairEnumerable<T> : IIfValuesEnumerable<T>
    {
        public KeyValuePair<T, Func<bool>>[] Array { get; }

        Array IIfValuesEnumerable<T>.Array => Array;

        public IfKeyValuePairEnumerable(in KeyValuePair<T, Func<bool>>[] array) => Array = array;

        public KeyValuePair<T, Func<bool>> GetValue(in int index) => Array[index];
    }

    internal interface IIfKeyValuesEnumerable<TKey, TValue>
    {
        Array Array { get; }

        KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> GetValue(in int index);
    }

    internal class IfKeyValuesEnumerable<TKey, TValue> : IIfKeyValuesEnumerable<TKey, TValue>
    {
        private static KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> GetValue(in KeyValuePair<TKey, TValue>[] array, in int index, Predicate<TValue> predicate)
        {
            KeyValuePair<TKey, TValue> result = array[index];

            return new KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>(result.Key, new KeyValuePair<TValue, Func<bool>>(result.Value, () => predicate(result.Value)));
        }

        public KeyValuePair<TKey, TValue>[] Array { get; }

        Array IIfKeyValuesEnumerable<TKey, TValue>.Array => Array;

        public Predicate<TValue> Predicate { get; }

        public IfKeyValuesEnumerable(in KeyValuePair<TKey, TValue>[] array, in Predicate<TValue> predicate)
        {
            Array = array;

            Predicate = predicate;
        }

        public KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> GetValue(in int index) => GetValue(Array, index, Predicate);
    }

    internal class IfKeyKeyValuePairEnumerable<TKey, TValue> : IIfKeyValuesEnumerable<TKey, TValue>
    {
        public KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] Array { get; }

        Array IIfKeyValuesEnumerable<TKey, TValue>.Array => Array;

        public IfKeyKeyValuePairEnumerable(in KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] array) => Array = array;

        public KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> GetValue(in int index) => Array[index];
    }
}
