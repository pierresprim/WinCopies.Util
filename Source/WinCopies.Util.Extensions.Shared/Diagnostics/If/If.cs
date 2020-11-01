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
using System.Text;

using WinCopies.Collections;
using WinCopies.Collections.Generic;

using static WinCopies.Diagnostics.HelperMethods;
using static WinCopies.Diagnostics.ThrowHelper;
using static WinCopies.UtilHelpers;

using IComparer = System.Collections.IComparer;

using IfCT = WinCopies.Diagnostics.ComparisonType;
using IfCM = WinCopies.Diagnostics.ComparisonMode;
using IfComp = WinCopies.Diagnostics.Comparison;

namespace WinCopies.Diagnostics
{
    public static class IfHelpers
    {
        public static KeyValuePair<TKey, TValue> GetKeyValuePair<TKey, TValue>(in TKey key, in TValue value) => new KeyValuePair<TKey, TValue>(key, value);

        public static KeyValuePair<TKey, Func<bool>> GetIfKeyValuePairPredicate<TKey>(in TKey key, in Func<bool> predicate) => new KeyValuePair<TKey, Func<bool>>(key, predicate);

        private static bool IfInternal(in IfCT comparisonType, in IfCM comparisonMode, CheckIfComparisonDelegate comparisonDelegate, in IIfValuesEnumerable values)
        {
            bool checkIfComparison(in KeyValuePair<object, Func<bool>> value) => comparisonDelegate(value.Key, value.Value);

            // We check the comparison type for the 'and' comparison.

            if (comparisonType == IfCT.And)
            {
                for (int i = 0; i < values.Array.Length; i++)

                    if (!checkIfComparison(values.GetValue(i)))
                    {
                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(values.GetValue(i));

                        return false;
                    }

                return true;
            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == IfCT.Or)
            {
                for (int i = 0; i < values.Array.Length; i++)

                    if (checkIfComparison(values.GetValue(i)))
                    {
                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(values.GetValue(i));

                        return true;
                    }

                return false;
            }

            // We check the comparison type for the 'xor' comparison.

            else
            {
                for (int i = 0; i < values.Array.Length; i++)

                    if (checkIfComparison(values.GetValue(i)))
                    {
                        for (i++; i < values.Array.Length; i++)

                            if (checkIfComparison(values.GetValue(i)))
                            {
                                if (comparisonMode == IfCM.Binary)

                                    for (i++; i < values.Array.Length; i++)

                                        _ = checkIfComparison(values.GetValue(i));

                                return false;
                            }

                        return true;
                    }

                return false;

            }
        }

        private static bool IfInternal(in IfCT comparisonType, in IfCM comparisonMode, CheckIfComparisonDelegate comparisonDelegate, out object key, in IIfKeyValuesEnumerable values)
        {
            bool checkIfComparison(in KeyValuePair<object, Func<bool>> value) => comparisonDelegate(value.Key, value.Value);

            KeyValuePair<object, KeyValuePair<object, Func<bool>>> _value;

            // We check the comparison type for the 'and' comparison.

            if (comparisonType == IfCT.And)
            {
                for (int i = 0; i < values.Array.Length; i++)
                {
                    _value = values.GetValue(i);

                    if (!checkIfComparison(_value.Value))
                    {
                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(_value.Value);

                        key = _value.Key;

                        return false;
                    }
                }

                key = null;

                return true;
            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == IfCT.Or)
            {
                for (int i = 0; i < values.Array.Length; i++)
                {
                    _value = values.GetValue(i);

                    if (checkIfComparison(_value.Value))
                    {
                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(_value.Value);

                        key = _value.Key;

                        return true;
                    }
                }

                key = null;

                return false;
            }

            // We check the comparison type for the 'xor' comparison.

            else
            {
                for (int i = 0; i < values.Array.Length; i++)
                {
                    _value = values.GetValue(i);

                    if (checkIfComparison(_value.Value))
                    {
                        for (i++; i < values.Array.Length; i++)
                        {
                            _value = values.GetValue(i);

                            if (checkIfComparison(_value.Value))
                            {
                                if (comparisonMode == IfCM.Binary)

                                    for (i++; i < values.Array.Length; i++)

                                        _ = checkIfComparison(values.GetValue(i).Value);

                                key = _value.Key;

                                return false;
                            }
                        }

                        key = _value.Key;

                        return true;
                    }
                }

                key = null;

                return false;
            }
        }

        private static bool IfInternal<T>(in IfCT comparisonType, in IfCM comparisonMode, CheckIfComparisonDelegate<T> comparisonDelegate, in IIfValuesEnumerable<T> values)
        {
            bool checkIfComparison(in KeyValuePair<T, Func<bool>> value) => comparisonDelegate(value.Key, value.Value);

            // We check the comparison type for the 'and' comparison.

            if (comparisonType == IfCT.And)
            {
                for (int i = 0; i < values.Array.Length; i++)

                    if (!checkIfComparison(values.GetValue(i)))
                    {
                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(values.GetValue(i));

                        return false;
                    }

                return true;
            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == IfCT.Or)
            {
                for (int i = 0; i < values.Array.Length; i++)

                    if (checkIfComparison(values.GetValue(i)))
                    {
                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(values.GetValue(i));

                        return true;
                    }

                return false;
            }

            // We check the comparison type for the 'xor' comparison.

            else
            {
                for (int i = 0; i < values.Array.Length; i++)

                    if (checkIfComparison(values.GetValue(i)))
                    {
                        for (i++; i < values.Array.Length; i++)

                            if (checkIfComparison(values.GetValue(i)))
                            {
                                if (comparisonMode == IfCM.Binary)

                                    for (i++; i < values.Array.Length; i++)

                                        _ = checkIfComparison(values.GetValue(i));

                                return false;
                            }

                        return true;
                    }

                return false;
            }
        }

        private static bool IfInternal<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, CheckIfComparisonDelegate<TValue> comparisonDelegate, out TKey key, in IIfKeyValuesEnumerable<TKey, TValue> values)
        {
            bool checkIfComparison(in KeyValuePair<TValue, Func<bool>> value) => comparisonDelegate(value.Key, value.Value);

            KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> _value;

            // We check the comparison type for the 'and' comparison.

            if (comparisonType == IfCT.And)
            {
                for (int i = 0; i < values.Array.Length; i++)
                {
                    _value = values.GetValue(i);

                    if (!checkIfComparison(_value.Value))
                    {
                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(_value.Value);

                        key = _value.Key;

                        return false;
                    }
                }

                key = default;

                return true;
            }

            // We check the comparison type for the 'or' comparison.

            else if (comparisonType == IfCT.Or)
            {
                for (int i = 0; i < values.Array.Length; i++)
                {
                    _value = values.GetValue(i);

                    if (checkIfComparison(_value.Value))

                    {
                        if (comparisonMode == IfCM.Binary)

                            for (i++; i < values.Array.Length; i++)

                                _ = checkIfComparison(_value.Value);

                        key = _value.Key;

                        return true;
                    }
                }

                key = default;

                return false;
            }

            // We check the comparison type for the 'xor' comparison.

            else
            {
                for (int i = 0; i < values.Array.Length; i++)
                {
                    _value = values.GetValue(i);

                    if (checkIfComparison(_value.Value))
                    {
                        for (i++; i < values.Array.Length; i++)
                        {
                            _value = values.GetValue(i);

                            if (checkIfComparison(_value.Value))
                            {
                                if (comparisonMode == IfCM.Binary)

                                    for (i++; i < values.Array.Length; i++)

                                        _ = checkIfComparison(values.GetValue(i).Value);

                                key = _value.Key;

                                return false;
                            }
                        }

                        key = _value.Key;

                        return true;
                    }
                }

                key = default;

                return false;
            }
        }

        #region Non generic methods

        #region Comparisons without key notification

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="value">The value to compare the values of the table with.</param>
        /// <param name="values">The values to compare.</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, System.Collections.Generic.EqualityComparer<object>.Default, GetCommonPredicate(), value, values);

        ///// <summary>
        ///// Performs a comparison by testing a value compared to an array of values using a custom <see cref="System.Collections.Generic.IComparer{T}"/> and <see cref="Predicate{T}"/>.
        ///// </summary>
        ///// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        ///// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        ///// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        ///// <param name="comparer">The comparer used to compare the values.</param>
        ///// <param name="value">The value to compare with.</param>
        ///// <param name="values">The values to compare.</param>
        ///// <param name="predicate">The comparison predicate</param>
        ///// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        //[Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, IComparer, Predicate, object, params object[])")]
        //public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, IComparer comparer, in Predicate<object> predicate, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="IComparer"/> and <see cref="Predicate"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, IComparer comparer, in Predicate predicate, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        ///// <summary>
        ///// Performs a comparison by testing a value compared to an array of values using a custom <see cref="Comparison{T}"/> and <see cref="Predicate{T}"/>.
        ///// </summary>
        ///// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        ///// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        ///// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        ///// <param name="comparisonDelegate">The comparison delegate used to compare the values.</param>
        ///// <param name="value">The value to compare with.</param>
        ///// <param name="values">The values to compare.</param>
        ///// <param name="predicate">The comparison predicate</param>
        ///// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        //[Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, WinCopies.Collections.Comparison, Predicate, object, params object[])")]
        //public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, Comparison<object> comparisonDelegate, Predicate<object> predicate, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, new WinCopies.Collections.Comparison((object x, object y) => comparisonDelegate(x, y)), new Predicate(o => predicate(o)), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="WinCopies.Collections.Comparison"/> and <see cref="Predicate"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="comparisonDelegate">The comparison delegate used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, WinCopies.Collections.Comparison comparisonDelegate, in Predicate predicate, object value, params object[] values)
        {
            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckIfComparison(comparison, _predicate, comparisonDelegate(value, _value)), new IfValuesEnumerable(values, predicate));
        }

        ///// <summary>
        ///// Performs a comparison by testing a value compared to an array of values using a custom <see cref="IComparer"/> and <see cref="Predicate{T}"/>.
        ///// </summary>
        ///// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        ///// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        ///// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        ///// <param name="equalityComparer">The equality comparer used to compare the values.</param>
        ///// <param name="value">The value to compare with.</param>
        ///// <param name="values">The values to compare.</param>
        ///// <param name="predicate">The comparison predicate</param>
        ///// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        //[Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, System.Collections.Generic.IEqualityComparer, Predicate, object, params object[])")]
        //public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, in System.Collections.Generic.IEqualityComparer equalityComparer, Predicate<object> predicate, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, equalityComparer, new Predicate(o => predicate(o)), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="IComparer"/> and <see cref="Predicate"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="equalityComparer">The equality comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, System.Collections.IEqualityComparer equalityComparer, in Predicate predicate, in object value, params object[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, (object x, object y) => equalityComparer.Equals(x, y), predicate, value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="EqualityComparison"/> and <see cref="Predicate"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="comparisonDelegate">The comparison delegate used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, EqualityComparison comparisonDelegate, in Predicate predicate, object value, params object[] values)
        {
            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, value.GetType(), comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate, comparisonDelegate), new IfValuesEnumerable(values, predicate));
        }

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, IComparer comparer, in object value, params KeyValuePair<object, Func<bool>>[] values) => If(comparisonType, comparisonMode, comparison, new WinCopies.Collections.Comparison((object x, object y) => comparer.Compare(x, y)), value, values);

        //[Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, WinCopies.Collections.Comparison, object, params KeyValuePair<object, Func<bool>>[])")]
        //public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, Comparison<object> comparisonDelegate, in object value, params KeyValuePair<object, Func<bool>>[] values) => If(comparisonType, comparisonMode, comparison, new WinCopies.Collections.Comparison((object x, object y) => comparisonDelegate(x, y)), value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, WinCopies.Collections.Comparison comparisonDelegate, object value, params KeyValuePair<object, Func<bool>>[] values)
        {
            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckIfComparison(comparison, _predicate, comparisonDelegate(value, _value)), new IfKeyValuePairEnumerable(values));
        }

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, System.Collections.IEqualityComparer equalityComparer, in object value, params KeyValuePair<object, Func<bool>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison)null, value, values) : If(comparisonType, comparisonMode, comparison, new EqualityComparison((object x, object y) => equalityComparer.Equals(x, y)), value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, EqualityComparison comparisonDelegate, object value, params KeyValuePair<object, Func<bool>>[] values)
        {
            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, value.GetType(), comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate, comparisonDelegate), new IfKeyValuePairEnumerable(values));
        }

        #endregion

        #region Comparisons with key notification

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="value">The value to compare the values of the table with.</param>
        /// <param name="values">The values to compare.</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, System.Collections.Generic.EqualityComparer<object>.Default, GetCommonPredicate(), value, values);

        ///// <summary>
        ///// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="System.Collections.Generic.IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
        ///// </summary>
        ///// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        ///// <param name="comparison">The comparison type</param>
        ///// <param name="comparer">The comparer used to compare the values.</param>
        ///// <param name="value">The value to compare with.</param>
        ///// <param name="values">The values to compare.</param>
        ///// <param name="predicate">The comparison predicate</param>
        ///// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        //[Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, IComparer, Predicate, object, params KeyValuePair<object, object>[])")]
        //public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, IComparer comparer, in Predicate<object> predicate, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="System.Collections.Generic.IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, IComparer comparer, in Predicate predicate, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        //[Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, WinCopies.Collections.Comparison, Predicate, object, params KeyValuePair<object, object>[])")]
        //public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, Comparison<object> comparisonDelegate, Predicate<object> predicate, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, new WinCopies.Collections.Comparison((object x, object y) => comparisonDelegate(x, y)), new Predicate(o => predicate(o)), value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out object key, WinCopies.Collections.Comparison comparisonDelegate, in Predicate predicate, object value, params KeyValuePair<object, object>[] values)
        {
            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckIfComparison(comparison, _predicate, comparisonDelegate(value, _value)), out key, new IfKeyValuesEnumerable(values, predicate));
        }

        //[Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, System.Collections.Generic.IEqualityComparer, Predicate, object, params KeyValuePair<object, object>[])")]
        //public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, in System.Collections.Generic.IEqualityComparer equalityComparer, Predicate<object> predicate, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, equalityComparer, new Predicate(o => predicate(o)), value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, System.Collections.IEqualityComparer equalityComparer, in Predicate predicate, in object value, params KeyValuePair<object, object>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, out key, (object x, object y) => equalityComparer.Equals(x, y), predicate, value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out object key, EqualityComparison comparisonDelegate, in Predicate predicate, object value, params KeyValuePair<object, object>[] values)

        {
            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, value.GetType(), comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate, comparisonDelegate), out key, new IfKeyValuesEnumerable(values, predicate));
        }

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, IComparer comparer, in object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values) => If(comparisonType, comparisonMode, comparison, out key, new WinCopies.Collections.Comparison((object x, object y) => comparer.Compare(x, y)), value, values);

        //[Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, WinCopies.Collections.Comparison, object, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[])")]
        //public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, Comparison<object> comparisonDelegate, in object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values) => If(comparisonType, comparisonMode, comparison, out key, new WinCopies.Collections.Comparison((object x, object y) => comparisonDelegate(x, y)), value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out object key, WinCopies.Collections.Comparison comparisonDelegate, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values)
        {
            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckIfComparison(comparison, _predicate, comparisonDelegate(value, _value)), out key, new IfKeyKeyValuePairEnumerable(values));
        }

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, System.Collections.IEqualityComparer equalityComparer, in object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison)null, value, values) : If(comparisonType, comparisonMode, comparison, out key, new EqualityComparison((object x, object y) => equalityComparer.Equals(x, y)), value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out object key, EqualityComparison comparisonDelegate, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values)
        {
            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, value.GetType(), comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate, comparisonDelegate), out key, new IfKeyKeyValuePairEnumerable(values));
        }

        #endregion

        #endregion

        #region Generic methods

        #region Comparisons without key notification

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="value">The value to compare the values of the table with.</param>
        /// <param name="values">The values to compare.</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, in T value, params T[] values) => If(comparisonType, comparisonMode, comparison, System.Collections.Generic.EqualityComparer<T>.Default, GetCommonPredicate<T>(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="System.Collections.Generic.IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, System.Collections.Generic.IComparer<T> comparer, in Predicate<T> predicate, in T value, params T[] values) => If(comparisonType, comparisonMode, comparison, (T x, T y) => comparer.Compare(x, y), predicate, value, values);

        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, Comparison<T> comparisonDelegate, in Predicate<T> predicate, T value, params T[] values)

        {
            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (in T _value, in Func<bool> _predicate) => CheckIfComparison(comparison, _predicate, comparisonDelegate(value, _value)), new IfValuesEnumerable<T>(values, predicate));
        }

        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, System.Collections.Generic.IEqualityComparer<T> equalityComparer, in Predicate<T> predicate, in T value, params T[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison<T>)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, (T x, T y) => equalityComparer.Equals(x, y), predicate, value, values);

        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, EqualityComparison<T> comparisonDelegate, in Predicate<T> predicate, T value, params T[] values)
        {
            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (in T _value, in Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate, comparisonDelegate), new IfValuesEnumerable<T>(values, predicate));
        }

        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, System.Collections.Generic.IComparer<T> comparer, in T value, params KeyValuePair<T, Func<bool>>[] values) => If(comparisonType, comparisonMode, comparison, (T x, T y) => comparer.Compare(x, y), value, values);

        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, Comparison<T> comparisonDelegate, T value, params KeyValuePair<T, Func<bool>>[] values)
        {
            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (in T _value, in Func<bool> _predicate) => CheckIfComparison(comparison, _predicate, comparisonDelegate(value, _value)), new IfKeyValuePairEnumerable<T>(values));
        }

        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, System.Collections.Generic.IEqualityComparer<T> equalityComparer, in T value, params KeyValuePair<T, Func<bool>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison<T>)null, value, values) : If(comparisonType, comparisonMode, comparison, (T x, T y) => equalityComparer.Equals(x, y), value, values);

        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, EqualityComparison<T> comparisonDelegate, T value, params KeyValuePair<T, Func<bool>>[] values)
        {
            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (in T _value, in Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate, comparisonDelegate), new IfKeyValuePairEnumerable<T>(values));
        }

        #endregion

        #region Comparisons with key notification

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="value">The value to compare the values of the table with.</param>
        /// <param name="values">The values to compare.</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out TKey key, in TValue value, params KeyValuePair<TKey, TValue>[] values) => If(comparisonType, comparisonMode, comparison, out key, System.Collections.Generic.EqualityComparer<TValue>.Default, GetCommonPredicate<TValue>(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="System.Collections.Generic.System.Collections.Generic.IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out TKey key, System.Collections.Generic.IComparer<TValue> comparer, in Predicate<TValue> predicate, in TValue value, params KeyValuePair<TKey, TValue>[] values) => If(comparisonType, comparisonMode, comparison, out key, (TValue x, TValue y) => comparer.Compare(x, y), predicate, value, values);

        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out TKey key, Comparison<TValue> comparisonDelegate, in Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values)
        {
            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (in TValue _value, in Func<bool> _predicate) => CheckIfComparison(comparison, _predicate, comparisonDelegate(value, _value)), out key, new IfKeyValuesEnumerable<TKey, TValue>(values, predicate));
        }

        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out TKey key, System.Collections.Generic.IEqualityComparer<TValue> equalityComparer, in Predicate<TValue> predicate, in TValue value, params KeyValuePair<TKey, TValue>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison<TValue>)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, out key, (TValue x, TValue y) => equalityComparer.Equals(x, y), predicate, value, values);

        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out TKey key, EqualityComparison<TValue> comparisonDelegate, in Predicate<TValue> predicate, TValue value, params KeyValuePair<TKey, TValue>[] values)
        {
            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (in TValue _value, in Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate, comparisonDelegate), out key, new IfKeyValuesEnumerable<TKey, TValue>(values, predicate));
        }

        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out TKey key, System.Collections.Generic.IComparer<TValue> comparer, in TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values) => If(comparisonType, comparisonMode, comparison, out key, (TValue x, TValue y) => comparer.Compare(x, y), value, values);

        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out TKey key, Comparison<TValue> comparisonDelegate, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values)
        {
            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (in TValue _value, in Func<bool> _predicate) => CheckIfComparison(comparison, _predicate, comparisonDelegate(value, _value)), out key, new IfKeyKeyValuePairEnumerable<TKey, TValue>(values));
        }

        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out TKey key, System.Collections.Generic.IEqualityComparer<TValue> equalityComparer, in TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison<TValue>)null, value, values) : If(comparisonType, comparisonMode, comparison, out key, (TValue x, TValue y) => equalityComparer.Equals(x, y), value, values);

        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out TKey key, EqualityComparison<TValue> comparisonDelegate, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values)
        {
            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (in TValue _value, in Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate, comparisonDelegate), out key, new IfKeyKeyValuePairEnumerable<TKey, TValue>(values));
        }

        #endregion

        #endregion
    }
}
