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

using WinCopies.Collections.Abstraction.Generic;
using WinCopies.Util;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
    public static class ArrayExtensions
    {
        public static void Clear<T>(this IArrayEnumerable<T
#if CS9
            ?
#endif
            > array)
        {
            ThrowIfNull(array, nameof(array));

            for (int i = 0; i < array.AsFromType<ICountable>().Count; i++)

                array[i] = default;
        }

        /// <summary>
        /// Appends data to the table. Arrays must have only one dimension.
        /// </summary>
        /// <param name="array">The source table.</param>
        /// <param name="arrays">The tables to concatenate.</param>
        /// <returns>The concatenated array.</returns>
        public static object[] Append(this System.Array array, params System.Array[] arrays) => UtilHelpers.Concatenate((object[])array, arrays);

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

        public static System.Collections.Generic.List<T> ToList<T>(this T[] array, in int startIndex, in int length)
        {
            ThrowIfNull(array, nameof(array));

            var arrayList = new System.Collections.Generic.List<T>(length);

            int count = startIndex + length;

            int i;

            for (i = startIndex; i < count; i++)

                arrayList.Add(array[i]);

            return arrayList;
        }



        public static void Move(this System.Array array, int move, int at)
        {
            ThrowOnInvalidArrayMoveOperation(array, nameof(array), move, nameof(move), at, nameof(at));

            if (array.Length <= 1 || move == at)

                return;

            object
#if CS8
                ?
#endif
                value = array.GetValue(move);

            int i;

            if (at > move)
            {
                for (i = move + 1; i < at - move; i++)

                    array.SetValue(array.GetValue(i), i - 1);

                array.SetValue(value, i);
            }

            else
            {
                for (i = move - 1; i > move - at; i--)

                    array.SetValue(array.GetValue(i), i + 1);

                array.SetValue(value, i);
            }
        }

        public static void Move<T>(this T[] array, int move, int at)
        {
            ThrowOnInvalidArrayMoveOperation(array, nameof(array), move, nameof(move), at, nameof(at));

            if (array.Length <= 1 || move == at)

                return;

            T value = array[move];

            int i;

            if (at > move)
            {
                for (i = move + 1; i < at - move; i++)

                    array[i - 1] = array[i];

                array[i] = value;
            }

            else
            {
                for (i = move - 1; i > move - at; i--)

                    array[i + 1] = array[i];

                array[i] = value;
            }
        }

        public static void Swap(this System.Array array, int x, int y)
        {
            ThrowOnInvalidArrayMoveOperation(array, nameof(array), x, nameof(x), y, nameof(y));

            if (x == y)

                return;

            object
#if CS8
                ?
#endif
                temp = array.GetValue(x);

            array.SetValue(array.GetValue(y), x);

            array.SetValue(temp, y);
        }

        public static void Swap<T>(this T[] array, int x, int y)
        {
            ThrowOnInvalidArrayMoveOperation(array, nameof(array), x, nameof(x), y, nameof(y));

            if (x == y)

                return;
#if CS5
            (array[y], array[x]) = (array[x], array[y]);
#else
            T temp = array[x];

            array[x] = array[y];

            array[y] = temp;
#endif
        }
    }
}
