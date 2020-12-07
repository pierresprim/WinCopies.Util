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

#if !WinCopies2

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using static WinCopies.ThrowHelper;
using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections
{
    public static class ArrayExtensions
    {

        /// <summary>
        /// Appends data to the table. Arrays must have only one dimension.
        /// </summary>
        /// <param name="array">The source table.</param>
        /// <param name="arrays">The tables to concatenate.</param>
        /// <returns></returns>
        public static object[] Append(this Array array, params Array[] arrays) => WinCopies.UtilHelpers.Concatenate((object[])array, arrays);

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



        private static void ThrowOnInvalidArrayMoveOperation(in Array array, in string arrayArgumentName, in int x, in string xArgumentName, in int y, in string yArgumentName)
        {
            ThrowIfNull(array, arrayArgumentName);
            ThrowIfMultidimensionalArray(array, arrayArgumentName);
            ThrowIfNOTBetween(x, 0, array.Length - 1, xArgumentName);
            ThrowIfNOTBetween(y, 0, array.Length - 1, yArgumentName);
        }

        public static void Move(this Array array, int move, int at)
        {
            ThrowOnInvalidArrayMoveOperation(array, nameof(array), move, nameof(move), at, nameof(at));

            if (array.Length <= 1 || move == at)

                return;

            object value = array.GetValue(move);

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



        public static void Swap(this Array array, int x, int y)
        {
            ThrowOnInvalidArrayMoveOperation(array, nameof(array), x, nameof(x), y, nameof(y));

            if (array.Length <= 1 || x == y)

                return;

            object temp = array.GetValue(x);

            array.SetValue(array.GetValue(y), x);

            array.SetValue(temp, y);
        }

        public static void Swap<T>(this T[] array, int x, int y)
        {
            ThrowOnInvalidArrayMoveOperation(array, nameof(array), x, nameof(x), y, nameof(y));

            if (array.Length <= 1 || x == y)

                return;

            T temp = array[x];

            array[x] = array[y];

            array[y] = temp;
        }
    }
}

#endif
