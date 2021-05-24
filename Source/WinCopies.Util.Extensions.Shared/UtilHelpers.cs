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

#if CS7

using System;
using System.Collections;

using WinCopies.Linq;
using WinCopies.Util;

using static WinCopies.Diagnostics.IfHelpers;

using IfCT = WinCopies.Diagnostics.ComparisonType;
using IfCM = WinCopies.Diagnostics.ComparisonMode;
using IfComp = WinCopies.Diagnostics.Comparison;

namespace WinCopies.Extensions // To avoid name conflicts.
{
    public static class UtilHelpers
    {
        public static System.Collections.Generic.IEnumerable<T> GetFieldValues<T>(this Type t, object obj)
        {
            Type _t = typeof(T);

            return t.GetFields().WhereSelect(f => f.FieldType.IsAssignableFrom(_t), f => (T)f.GetValue(obj));
        }

        public static bool ContainsFieldValue<T>(in Type t, in object obj, T value)
        {
            Predicate<T> p;

            if (value == null)

                p = _value => _value == null;

            else

                p = _value => value.Equals(_value);

            foreach (T _value in t.GetFieldValues<T>(obj))

                if (p(_value))

                    return true;

            return false;
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
        /// <remarks>See <see cref="GetIf{TValues, TResult}(in TValues, in TValues, in Comparison{TValues}, in Func{TResult}, in Func{TResult}, in Func{TResult})"/> for the generic version.</remarks>
        public static object GetIf(in object x, in object y, in WinCopies.Collections.Comparison comparison, in Func lower, in Func equals, in Func greater)
        {
            if (If(IfCT.Or, IfCM.Logical, IfComp.Equal, out string key, null, GetKeyValuePair(nameof(lower), lower), GetKeyValuePair(nameof(greater), greater), GetKeyValuePair(nameof(equals), equals)))

                throw new ArgumentNullException(key);

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
            if (If(IfCT.Or, IfCM.Logical, IfComp.Equal, out string key, null, GetKeyValuePair(nameof(lower), lower), GetKeyValuePair(nameof(greater), greater), GetKeyValuePair(nameof(equals), equals)))

                throw new ArgumentNullException(key);

            int result = comparison(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();
        }

        public static object GetIf(in object x, in object y, in IComparer comparer, in Func lower, in Func equals, in Func greater)
        {
            if (If(IfCT.Or, IfCM.Logical, IfComp.Equal, out string key, null, GetKeyValuePair(nameof(lower), lower), GetKeyValuePair(nameof(greater), greater), GetKeyValuePair(nameof(equals), equals)))

                throw new ArgumentNullException(key);

            int result = comparer.Compare(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();
        }

        public static TResult GetIf<TValues, TResult>(in TValues x, in TValues y, in WinCopies.Collections.Comparison comparison, in Func<TResult> lower, in Func<TResult> equals, in Func<TResult> greater)
        {
            if (If(IfCT.Or, IfCM.Logical, IfComp.Equal, out string key, null, GetKeyValuePair(nameof(lower), lower), GetKeyValuePair(nameof(greater), greater), GetKeyValuePair(nameof(equals), equals)))

                throw new ArgumentNullException(key);

            int result = comparison(x, y);

            return result < 0 ? lower() : result > 0 ? greater() : equals();
        }
    }
}

#endif