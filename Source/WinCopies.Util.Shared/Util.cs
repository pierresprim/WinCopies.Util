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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using IfCT = WinCopies.Util.Util.ComparisonType;
using IfCM = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;
using WinCopies.Collections;
using IComparer = System.Collections.IComparer;
using WinCopies.Util.Resources;
using System.Diagnostics;

namespace WinCopies.Util
{

    /// <summary>
    /// Delegate for a non-generic predicate.
    /// </summary>
    /// <param name="value">The value to test</param>
    /// <returns><see langword="true"/> if the predicate success, otherwise <see langword="false"/>.</returns>
    public delegate bool Predicate(object value);

    public delegate void ActionParams(params object[] args);

    public delegate void ActionParams<in T>(params T[] args);

    public delegate object Func();

    public delegate object FuncParams(params object[] args);

    public delegate TResult FuncParams<in TParams, out TResult>(params TParams[] args);

    /// <summary>
    /// Provides some static helper methods.
    /// </summary>
    public static class Util
    {

        public const string NotApplicable = "N/A";

        public const BindingFlags DefaultBindingFlagsForPropertySet = BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        [Obsolete("This method has been replaced by the WinCopies.Util.Extensions.SetBackgroundWorkerProperty method overloads.")]
        public static (bool propertyChanged, object oldValue) SetPropertyWhenNotBusy<T>(T bgWorker, string propertyName, string fieldName, object newValue, Type declaringType, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, bool throwIfBusy = true) where T : IBackgroundWorker, INotifyPropertyChanged => bgWorker.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException("Cannot change property value when BackgroundWorker is busy.") : (false, Extensions.GetField(fieldName, declaringType, bindingFlags).GetValue(bgWorker))
                : bgWorker.SetProperty(propertyName, fieldName, newValue, declaringType, true, bindingFlags);

        /// <summary>
        /// Provides a <see cref="Predicate"/> implementation that always returns <see langword="true"/>.
        /// </summary>
        /// <returns>Returns the <see langword="true"/> value.</returns>
        public static Predicate GetCommonPredicate() => (object value) => true;

        /// <summary>
        /// Provides a <see cref="Predicate{T}"/> implementation that always returns <see langword="true"/>.
        /// </summary>
        /// <returns>Returns the <see langword="true"/> value.</returns>
        public static Predicate<T> GetCommonPredicate<T>() => (T value) => true;

        // todo: key-value pairs to raise an argument exception

        [Obsolete("This method has been replaced by the WinCopies.Util.Extensions.ThrowIfInvalidEnumValue(params Enum[] values) method.")]
        public static void ThrowOnNotValidEnumValue(params Enum[] values) => ThrowIfInvalidEnumValue(values);

        public static void ThrowIfInvalidEnumValue(params Enum[] values)

        {

            ThrowIfNull(values, nameof(values));

            foreach (Enum value in values)

                value.ThrowIfNotValidEnumValue();

        }

        public static void ThrowIfInvalidEnumValue(in string argumentName, params Enum[] values)

        {

            ThrowIfNull(values, nameof(values));

            foreach (Enum value in values)

                value.ThrowIfNotValidEnumValue(argumentName);

        }

        [Obsolete("This method has been replaced by the WinCopies.Util.Extensions.ThrowIfInvalidEnumValue(this Enum, bool, params Enum[]) method.")]
        public static void ThrowOnEnumNotValidEnumValue(in Enum value, params Enum[] values) => value.ThrowIfInvalidEnumValue(false, values);

        public static ArgumentException GetExceptionForNonFlagsEnum(in string argumentName) => new ArgumentException(ExceptionMessages.NonFlagsEnumException, argumentName);

        public static TypeArgumentException GetExceptionForNonFlagsEnumType(in string typeArgumentName) => new TypeArgumentException(ExceptionMessages.NonFlagsEnumTypeException, typeArgumentName);

        public static void ThrowIfNotFlagsEnumType<T>(in string typeArgumentName) where T : Enum
        {
            if (!IsFlagsEnum<T>())

                throw GetExceptionForNonFlagsEnum(typeArgumentName);
        }

        public static bool IsFlagsEnum<T>() where T : Enum => typeof(T).GetCustomAttribute<FlagsAttribute>() is object;

        // public static KeyValuePair<TKey, Func<bool>>[] GetIfKeyValuePairPredicateArray<TKey>(params KeyValuePair<TKey, Func<bool>>[] keyValuePairs) => keyValuePairs;

        #region 'If' methods

        public static KeyValuePair<TKey, TValue> GetKeyValuePair<TKey, TValue>(in TKey key, in TValue value) => new KeyValuePair<TKey, TValue>(key, value);

        public static KeyValuePair<TKey, Func<bool>> GetIfKeyValuePairPredicate<TKey>(in TKey key, in Func<bool> predicate) => new KeyValuePair<TKey, Func<bool>>(key, predicate);

        #region Enums

        /// <summary>
        /// Comparison types for the If functions.
        /// </summary>
        public enum ComparisonType

        {

            /// <summary>
            /// Check if all conditions are checked.
            /// </summary>
            And = 0,

            /// <summary>
            /// Check if at least one condition is checked.
            /// </summary>
            Or = 1,

            /// <summary>
            /// Check if exactly one condition is checked.
            /// </summary>
            Xor = 2

        }

        /// <summary>
        /// Comparison modes for the If functions.
        /// </summary>
        public enum ComparisonMode
        {

            /// <summary>
            /// Use a binary comparison
            /// </summary>
            Binary = 0,

            /// <summary>
            /// Use a logical comparison
            /// </summary>
            Logical = 1

        }

        /// <summary>
        /// Comparison to perform.
        /// </summary>
        public enum Comparison

        {

            /// <summary>
            /// Check for values equality
            /// </summary>
            Equal = 0,

            /// <summary>
            /// Check for values non-equality
            /// </summary>
            NotEqual = 1,

            /// <summary>
            /// Check if a value is lesser than a given value. This field only works for methods that use lesser/greater/equal comparers.
            /// </summary>
            Lesser = 2,

            /// <summary>
            /// Check if a value is lesser than or equal to a given value. This field only works for methods that use lesser/greater/equal comparers.
            /// </summary>
            LesserOrEqual = 3,

            /// <summary>
            /// Check if a value is greater than a given value. This field only works for methods that use lesser/greater/equal comparers.
            /// </summary>
            Greater = 4,

            /// <summary>
            /// Check if a value is greater than or equal to a given value. This field only works for methods that use lesser/greater/equal comparers.
            /// </summary>
            GreaterOrEqual = 5,

            /// <summary>
            /// Check if an object reference is equal to a given object reference. This field only works for methods that use equality comparers (not lesser/greater ones).
            /// </summary>
            ReferenceEqual = 6

        }

        #endregion

        #region 'Throw' methods

        private static void ThrowOnInvalidIfMethodArg(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison)

        {

            ThrowIfInvalidEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == IfComp.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)IfComp.ReferenceEqual, typeof(IfComp));

        }

        private static void ThrowOnInvalidEqualityIfMethodEnumValue(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison)

        {

            ThrowIfInvalidEnumValue(comparisonType, comparisonMode);

            if (!(comparison == IfComp.Equal || comparison == IfComp.NotEqual || comparison == IfComp.ReferenceEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(IfComp.Equal)}, {nameof(IfComp.NotEqual)} or {nameof(IfComp.ReferenceEqual)}");

        }

        private static void ThrowOnInvalidEqualityIfMethodArg(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, in Type valueType, in EqualityComparison comparisonDelegate)

        {

            ThrowOnInvalidEqualityIfMethodEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == IfComp.ReferenceEqual && comparisonDelegate != null)

                throw new ArgumentException($"{nameof(comparisonDelegate)} have to be set to null in order to use this method with the {nameof(IfComp.ReferenceEqual)} enum value.");

            if (comparison == IfComp.ReferenceEqual && !valueType.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class types.");

        }

        private static void ThrowOnInvalidEqualityIfMethodArg<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, EqualityComparison<T> comparisonDelegate)

        {

            ThrowOnInvalidEqualityIfMethodEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == IfComp.ReferenceEqual && comparisonDelegate != null)

                throw new ArgumentException($"{nameof(comparisonDelegate)} have to be set to null in order to use this method with the {nameof(IfComp.ReferenceEqual)} enum value.");

            if (comparison == IfComp.ReferenceEqual && !typeof(T).IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class types.");

        }

        #endregion

        #region 'Check comparison' methods

        private static bool CheckIfComparison(in IfComp comparison, in Func<bool> predicateResult, in int result)
        {

            if (comparison != IfComp.NotEqual && !predicateResult()) return false;

            switch (comparison)

            {

                case IfComp.Equal:
                case IfComp.ReferenceEqual:

                    return result == 0;

                case IfComp.LesserOrEqual:

                    return result <= 0;

                case IfComp.GreaterOrEqual:

                    return result >= 0;

                case IfComp.Lesser:

                    return result < 0;

                case IfComp.Greater:

                    return result > 0;

                case IfComp.NotEqual:

                    return !predicateResult() || result != 0;

                default:

                    return false;//: comparisonType == ComparisonType.Or ?//(result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||//    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||//    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))

            }

        }

        private static bool CheckEqualityComparison(in IfComp comparison, in object value, in object valueToCompare, in Func<bool> predicateResult, in EqualityComparison comparisonDelegate)
        {

            if (comparison == IfComp.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class types.");

            if (comparison != IfComp.NotEqual && !predicateResult()) return false;

#if CS7

            switch (comparison)
            {

                case IfComp.Equal:

                    return comparisonDelegate(value, valueToCompare);

                case IfComp.NotEqual:
            
                    return !predicateResult() || !comparisonDelegate(value, valueToCompare);

#pragma warning disable IDE0002

                case IfComp.ReferenceEqual:
            
                    return object.ReferenceEquals(value, valueToCompare);

#pragma warning restore IDE0002

                default:
            
                    return false;

            }

#else

            return comparison switch
            {
                IfComp.Equal => comparisonDelegate(value, valueToCompare),

                IfComp.NotEqual => !predicateResult() || !comparisonDelegate(value, valueToCompare),

#pragma warning disable IDE0002

                IfComp.ReferenceEqual => object.ReferenceEquals(value, valueToCompare),

#pragma warning restore IDE0002

                _ => false
            };

#endif
        }

        private static bool CheckEqualityComparison<T>(in IfComp comparison, in T value, in T valueToCompare, in Func<bool> predicateResult, in EqualityComparison<T> comparisonDelegate)
        {

            // Because we've already checked that for the 'T' type in the 'If' method and assuming that 'T' is the base type of all the values to test, if 'T' is actually a class, we don't need to check here if the type of the current value is actually a class when comparison is set to ReferenceEqual.

            if (comparison != IfComp.NotEqual && !predicateResult()) return false;

#if CS7

            switch (comparison)
            {
                case IfComp.Equal:
            
                    return comparisonDelegate(value, valueToCompare);

                case IfComp.NotEqual:
            
                    return !predicateResult() || !comparisonDelegate(value, valueToCompare);

#pragma warning disable IDE0002

                case IfComp.ReferenceEqual:
            
                    return object.ReferenceEquals(value, valueToCompare);

#pragma warning restore IDE0002

                default:
            
                    return false;
            }

#else

            return comparison switch
            {
                IfComp.Equal => comparisonDelegate(value, valueToCompare),

                IfComp.NotEqual => !predicateResult() || !comparisonDelegate(value, valueToCompare),

#pragma warning disable IDE0002

                IfComp.ReferenceEqual => object.ReferenceEquals(value, valueToCompare),

#pragma warning restore IDE0002

                _ => false
            };

#endif 
        }

        private delegate bool CheckIfComparisonDelegate(in object value, in Func<bool> predicate);

        private delegate bool CheckIfComparisonDelegate<T>(in T value, in Func<bool> predicate);

        #endregion

        #region Enumerables

        private interface IIfValuesEnumerable
        {

            Array Array { get; }

            KeyValuePair<object, Func<bool>> GetValue(in int index);

        }

        private class IfValuesEnumerable : IIfValuesEnumerable
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

        private class IfKeyValuePairEnumerable : IIfValuesEnumerable
        {

            public KeyValuePair<object, Func<bool>>[] Array { get; }

            Array IIfValuesEnumerable.Array => Array;

            public IfKeyValuePairEnumerable(in KeyValuePair<object, Func<bool>>[] array) => Array = array;

            public KeyValuePair<object, Func<bool>> GetValue(in int index) => Array[index];

        }

        private interface IIfKeyValuesEnumerable
        {

            Array Array { get; }

            KeyValuePair<object, KeyValuePair<object, Func<bool>>> GetValue(in int index);

        }

        private class IfKeyValuesEnumerable : IIfKeyValuesEnumerable
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

        private class IfKeyKeyValuePairEnumerable : IIfKeyValuesEnumerable
        {

            public KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] Array { get; }

            Array IIfKeyValuesEnumerable.Array => Array;

            public IfKeyKeyValuePairEnumerable(KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] array) => Array = array;

            public KeyValuePair<object, KeyValuePair<object, Func<bool>>> GetValue(in int index) => Array[index];

        }

        private interface IIfValuesEnumerable<T>
        {

            Array Array { get; }

            KeyValuePair<T, Func<bool>> GetValue(in int index);

        }

        private class IfValuesEnumerable<T> : IIfValuesEnumerable<T>
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

        private class IfKeyValuePairEnumerable<T> : IIfValuesEnumerable<T>
        {

            public KeyValuePair<T, Func<bool>>[] Array { get; }

            Array IIfValuesEnumerable<T>.Array => Array;

            public IfKeyValuePairEnumerable(in KeyValuePair<T, Func<bool>>[] array) => Array = array;

            public KeyValuePair<T, Func<bool>> GetValue(in int index) => Array[index];

        }

        private interface IIfKeyValuesEnumerable<TKey, TValue>
        {

            Array Array { get; }

            KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> GetValue(in int index);

        }

        private class IfKeyValuesEnumerable<TKey, TValue> : IIfKeyValuesEnumerable<TKey, TValue>
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

        private class IfKeyKeyValuePairEnumerable<TKey, TValue> : IIfKeyValuesEnumerable<TKey, TValue>
        {

            public KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] Array { get; }

            Array IIfKeyValuesEnumerable<TKey, TValue>.Array => Array;

            public IfKeyKeyValuePairEnumerable(in KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] array) => Array = array;

            public KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>> GetValue(in int index) => Array[index];

        }

        #endregion

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
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, EqualityComparer<object>.Default, GetCommonPredicate(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="IComparer{T}"/> and <see cref="Predicate{T}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, IComparer, Predicate, object, params object[])")]
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, IComparer comparer, in Predicate<object> predicate, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

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

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="Comparison{T}"/> and <see cref="Predicate{T}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="comparisonDelegate">The comparison delegate used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, WinCopies.Collections.Comparison, Predicate, object, params object[])")]
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, Comparison<object> comparisonDelegate, Predicate<object> predicate, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, new WinCopies.Collections.Comparison((object x, object y) => comparisonDelegate(x, y)), new Predicate(o => predicate(o)), value, values);

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

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="IComparer"/> and <see cref="Predicate{T}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="equalityComparer">The equality comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, IEqualityComparer, Predicate, object, params object[])")]
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, in IEqualityComparer equalityComparer, Predicate<object> predicate, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, equalityComparer, new Predicate(o => predicate(o)), value, values);

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
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, IEqualityComparer equalityComparer, in Predicate predicate, in object value, params object[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, (object x, object y) => equalityComparer.Equals(x, y), predicate, value, values);

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

        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, WinCopies.Collections.Comparison, object, params KeyValuePair<object, Func<bool>>[])")]
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, Comparison<object> comparisonDelegate, in object value, params KeyValuePair<object, Func<bool>>[] values) => If(comparisonType, comparisonMode, comparison, new WinCopies.Collections.Comparison((object x, object y) => comparisonDelegate(x, y)), value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, WinCopies.Collections.Comparison comparisonDelegate, object value, params KeyValuePair<object, Func<bool>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckIfComparison(comparison, _predicate, comparisonDelegate(value, _value)), new IfKeyValuePairEnumerable(values));

        }

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, IEqualityComparer equalityComparer, in object value, params KeyValuePair<object, Func<bool>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison)null, value, values) : If(comparisonType, comparisonMode, comparison, new EqualityComparison((object x, object y) => equalityComparer.Equals(x, y)), value, values);

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
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, EqualityComparer<object>.Default, GetCommonPredicate(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, IComparer, Predicate, object, params KeyValuePair<object, object>[])")]
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, IComparer comparer, in Predicate<object> predicate, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, IComparer comparer, in Predicate predicate, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, WinCopies.Collections.Comparison, Predicate, object, params KeyValuePair<object, object>[])")]
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, Comparison<object> comparisonDelegate, Predicate<object> predicate, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, new WinCopies.Collections.Comparison((object x, object y) => comparisonDelegate(x, y)), new Predicate(o => predicate(o)), value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out object key, WinCopies.Collections.Comparison comparisonDelegate, in Predicate predicate, object value, params KeyValuePair<object, object>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckIfComparison(comparison, _predicate, comparisonDelegate(value, _value)), out key, new IfKeyValuesEnumerable(values, predicate));

        }

        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, IEqualityComparer, Predicate, object, params KeyValuePair<object, object>[])")]
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, in IEqualityComparer equalityComparer, Predicate<object> predicate, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, equalityComparer, new Predicate(o => predicate(o)), value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, IEqualityComparer equalityComparer, in Predicate predicate, in object value, params KeyValuePair<object, object>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, out key, (object x, object y) => equalityComparer.Equals(x, y), predicate, value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out object key, EqualityComparison comparisonDelegate, in Predicate predicate, object value, params KeyValuePair<object, object>[] values)

        {

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, value.GetType(), comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate, comparisonDelegate), out key, new IfKeyValuesEnumerable(values, predicate));

        }

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, IComparer comparer, in object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values) => If(comparisonType, comparisonMode, comparison, out key, new WinCopies.Collections.Comparison((object x, object y) => comparer.Compare(x, y)), value, values);

        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, WinCopies.Collections.Comparison, object, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[])")]
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, Comparison<object> comparisonDelegate, in object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values) => If(comparisonType, comparisonMode, comparison, out key, new WinCopies.Collections.Comparison((object x, object y) => comparisonDelegate(x, y)), value, values);

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out object key, WinCopies.Collections.Comparison comparisonDelegate, object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values)

        {

            // First, we check if comparisonType and comparison are in the required value range.

            ThrowOnInvalidIfMethodArg(comparisonType, comparisonMode, comparison);

            return IfInternal(comparisonType, comparisonMode, (in object _value, in Func<bool> _predicate) => CheckIfComparison(comparison, _predicate, comparisonDelegate(value, _value)), out key, new IfKeyKeyValuePairEnumerable(values));

        }

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, IEqualityComparer equalityComparer, in object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison)null, value, values) : If(comparisonType, comparisonMode, comparison, out key, new EqualityComparison((object x, object y) => equalityComparer.Equals(x, y)), value, values);

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
        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, in T value, params T[] values) => If(comparisonType, comparisonMode, comparison, EqualityComparer<T>.Default, GetCommonPredicate<T>(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
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

        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, IEqualityComparer<T> equalityComparer, in Predicate<T> predicate, in T value, params T[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison<T>)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, (T x, T y) => equalityComparer.Equals(x, y), predicate, value, values);

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

        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, IEqualityComparer<T> equalityComparer, in T value, params KeyValuePair<T, Func<bool>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, (EqualityComparison<T>)null, value, values) : If(comparisonType, comparisonMode, comparison, (T x, T y) => equalityComparer.Equals(x, y), value, values);

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
        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out TKey key, in TValue value, params KeyValuePair<TKey, TValue>[] values) => If(comparisonType, comparisonMode, comparison, out key, EqualityComparer<TValue>.Default, GetCommonPredicate<TValue>(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="IComparer{Object}"/> and <see cref="Predicate{Object}"/>.
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

        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out TKey key, IEqualityComparer<TValue> equalityComparer, in Predicate<TValue> predicate, in TValue value, params KeyValuePair<TKey, TValue>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison<TValue>)null, predicate, value, values) : If(comparisonType, comparisonMode, comparison, out key, (TValue x, TValue y) => equalityComparer.Equals(x, y), predicate, value, values);

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

        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out TKey key, IEqualityComparer<TValue> equalityComparer, in TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values) => equalityComparer == null ? If(comparisonType, comparisonMode, comparison, out key, (EqualityComparison<TValue>)null, value, values) : If(comparisonType, comparisonMode, comparison, out key, (TValue x, TValue y) => equalityComparer.Equals(x, y), value, values);

        public static bool If<TKey, TValue>(in IfCT comparisonType, in IfCM comparisonMode, IfComp comparison, out TKey key, EqualityComparison<TValue> comparisonDelegate, TValue value, params KeyValuePair<TKey, KeyValuePair<TValue, Func<bool>>>[] values)

        {

            ThrowOnInvalidEqualityIfMethodArg(comparisonType, comparisonMode, comparison, comparisonDelegate);

            return IfInternal(comparisonType, comparisonMode, (in TValue _value, in Func<bool> _predicate) => CheckEqualityComparison(comparison, _value, value, _predicate, comparisonDelegate), out key, new IfKeyKeyValuePairEnumerable<TKey, TValue>(values));

        }

        #endregion

        #endregion

        #endregion

        public static bool IsNullEmptyOrWhiteSpace(in string value) => string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

        public static void ThrowIfNullEmptyOrWhiteSpace(in string value)

        {

            if (IsNullEmptyOrWhiteSpace(value))

                throw new InvalidOperationException($"The given value is null, empty or white space. The given value is: '{value ?? ""}'");

        }

        public static void ThrowIfNullEmptyOrWhiteSpace(in string value, in string argumentName)

        {

            if (IsNullEmptyOrWhiteSpace(value))

                throw new ArgumentException($"The given value is null, empty or white space. The given value is: '{value ?? ""}'", argumentName);

        }

        /// <summary>
        /// Concatenates multiple arrays from a same item type. Arrays must have only one dimension.
        /// </summary>
        /// <param name="arrays">The different tables to concatenate.</param>
        /// <returns></returns>
        public static T[] Concatenate<T>(params T[][] arrays)

        {
            // /// <param name="elementType">The type of the items inside the tables.</param>

            T[] newArray;

            int totalArraysLength = 0;

            int totalArraysIndex = 0;

            foreach (T[] array in arrays)

            {

                // todo : in a newer version, instead, get the maximum rank of arrays in params Array[] arrays and add a gesture of this in the process (also for the ConcatenateLong method) ; and not forgetting to change the comments of the xmldoc about this.

                if (array.Rank != 1) throw new ArgumentException(ExceptionMessages.ArrayWithMoreThanOneDimension);

                totalArraysLength += array.Length;

            }

            newArray = new T[totalArraysLength];

            for (int i = 0; i < arrays.Length - 1; i++)

            {

                T[] array = arrays[i];

                array.CopyTo(newArray, totalArraysIndex);

                totalArraysIndex += array.Length;

            }

#if CS7

            arrays[arrays.Length - 1].CopyTo(newArray, totalArraysIndex);

#else

            arrays[^1].CopyTo(newArray, totalArraysIndex);

#endif

            return newArray;

        }

        /// <summary>
        /// Concatenates multiple arrays from a same item type using the <see cref="Array.LongLength"/> length property. Arrays must have only one dimension.
        /// </summary>
        /// <param name="arrays">The different tables to concatenate.</param>
        /// <returns></returns>
        public static T[] ConcatenateLong<T>(params T[][] arrays)

        {

            // /// <param name="elementType">The type of the items inside the tables.</param>

            T[] newArray;

            long totalArraysLength = 0;

            long totalArraysIndex = 0;

            foreach (T[] array in arrays)

            {

                // todo:

                if (array.Rank != 1) throw new ArgumentException("Arrays must have only one dimension.");

                totalArraysLength += array.LongLength;

            }

            newArray = new T[totalArraysLength];

            for (long i = 0; i < arrays.LongLength - 1; i++)

            {

                T[] array = arrays[i];

                array.CopyTo(newArray, totalArraysIndex);

                totalArraysIndex += array.LongLength;

            }

            arrays[arrays.LongLength - 1].CopyTo(newArray, totalArraysIndex);

            return newArray;

        }

        /// <summary>
        /// Checks if a object is numeric.
        /// </summary>
        /// <remarks>This function makes a check at the object type. For a string-parsing-checking for numerical value, look at the <see cref="IsNumeric(string, out decimal)"/> function.</remarks>
        /// <param name="value">The object to check</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the object given is a numerical type.</returns>
        public static bool IsNumber(in object value) => value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;

        /// <summary>
        /// Checks if a <see cref="string"/> is a numerical value.
        /// </summary>
        /// <remarks>This function tries to parse a <see cref="string"/> value to a <see cref="decimal"/> value. Given that <see cref="decimal"/> type is the greatest numerical type in the .NET framework, all the numbers can be supported in the .NET framework can be set in a <see cref="decimal"/> object.</remarks>
        /// <param name="s">The <see cref="string"/> to check</param>
        /// <param name="d">The <see cref="decimal"/> in which one set the <see cref="decimal"/> value</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the <see cref="string"/> given is a <see cref="decimal"/>.</returns>
        public static bool IsNumeric(in string s, out decimal d) => decimal.TryParse(s, out d);

        /// <summary>
        /// Get all the flags in a flags enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <returns>All the flags in the given enum type.</returns>
        public static T GetAllEnumFlags<T>() where T : Enum

        {

            Type enumType = typeof(T);

            if (enumType.GetCustomAttribute<FlagsAttribute>() == null)

                throw new ArgumentException("Enum is not a 'flags' enum.");

            Array array = Enum.GetValues(enumType);

            long values = 0;

            foreach (object value in array)

                values |= (long)Convert.ChangeType(value, TypeCode.Int64);

            return (T)Enum.ToObject(enumType, values);

        }

        /// <summary>
        /// Gets the numeric value for a field in an enum.
        /// </summary>
        /// <param name="enumType">The enum type in which to look for the specified enum field value.</param>
        /// <param name="fieldName">The enum field to look for.</param>
        /// <returns>The numeric value corresponding to this enum, in the given enum type underlying type.</returns>
        public static object GetNumValue(in Type enumType, in string fieldName)
        {

            ThrowIfNull(enumType, nameof(enumType));

#pragma warning disable CA1062 // Validate arguments of public methods

            return enumType.IsEnum ? Convert.ChangeType(enumType.GetField(fieldName).GetValue(null), Enum.GetUnderlyingType(enumType)) : throw new ArgumentException("'enumType' is not an enum type.");

#pragma warning restore CA1062 // Validate arguments of public methods

        }

        /// <summary>
        /// Returns an <see cref="ArgumentNullException"/> for a given argument name.
        /// </summary>
        /// <param name="argumentName">The name of the <see langword="null"/> argument.</param>
        /// <returns>An <see cref="ArgumentNullException"/> with the given argument name.</returns>
        public static ArgumentNullException GetArgumentNullException(in string argumentName) => new ArgumentNullException(argumentName);

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if a given object is null.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="ArgumentNullException"/> that is thrown.</param>
        public static void ThrowIfNull(in object obj, in string argumentName)
        {

            if (obj is null)

                throw GetArgumentNullException(argumentName);

        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if a given object is null.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="obj"/>. This must be a class type.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="ArgumentNullException"/> that is thrown.</param>
        public static void ThrowIfNull<T>(in T obj, in string argumentName) where T : class

        {

            if (obj is null)

                throw GetArgumentNullException(argumentName);

        }

        /// <summary>
        /// Returns <paramref name="obj"/> if it is not null, otherwise throws the <see cref="ArgumentNullException"/> that is returned by the <see cref="GetArgumentNullException(in string)"/> method.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="ArgumentNullException"/> that is thrown.</param>
        /// <returns></returns>
        public static object GetOrThrowIfNull(in object obj, in string argumentName) => obj ?? throw GetArgumentNullException(argumentName);

        /// <summary>
        /// Returns <paramref name="obj"/> if it is not null, otherwise throws the <see cref="ArgumentNullException"/> that is returned by the <see cref="GetArgumentNullException(in string)"/> method.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="obj"/>. This must be a class type.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="ArgumentNullException"/> that is thrown.</param>
        /// <returns></returns>
        public static T GetOrThrowIfNull<T>(in T obj, in string argumentName) where T : class => obj ?? throw GetArgumentNullException(argumentName);

        /// <summary>
        /// Returns an <see cref="ArgumentException"/> for the given object and argument name.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="objTypeName">The type name of the object of the exception.</param>
        /// <param name="argumentName">The argument name for the <see cref="Exception"/> that is returned.</param>
        /// <returns>An <see cref="ArgumentException"/> with the given argument name.</returns>
        public static Exception GetExceptionForInvalidType<T>(in string objTypeName, in string argumentName) => new ArgumentException($"{argumentName} must be an instance of {typeof(T)}. {argumentName} is an instance of {objTypeName}", argumentName);

        /// <summary>
        /// Returns an <see cref="ArgumentException"/> for the given object and argument name.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="objType">The type of the object of the exception.</param>
        /// <param name="argumentName">The argument name for the <see cref="Exception"/> that is returned.</param>
        /// <returns>An <see cref="ArgumentException"/> with the given argument name.</returns>
        public static Exception GetExceptionForInvalidType<T>(in Type objType, in string argumentName) => new ArgumentException($"{argumentName} must be an instance of {typeof(T)}. {argumentName} is an instance of {objType}", argumentName);

        /// <summary>
        /// If <paramref name="obj"/> is not <typeparamref name="T"/>, throws the exception that is returned by the <see cref="GetExceptionForInvalidType{T}(in string, in string)"/> method.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="Exception"/> that is thrown.</param>
        public static void ThrowIfNotType<T>(in object obj, in string argumentName) where T : struct

        {

            if (!(obj is T))

                throw GetExceptionForInvalidType<T>(obj.GetType().ToString(), argumentName);

        }

        /// <summary>
        /// If <paramref name="obj"/> is not <typeparamref name="T"/>, throws the exception that is returned by the <see cref="GetExceptionForInvalidType{T}(in string, in string)"/> method.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="Exception"/> that is thrown.</param>
        public static void ThrowIfNotTypeOrNull<T>(in object obj, in string argumentName) where T : class
        {
            if (obj is null)

                throw GetArgumentNullException(argumentName);

            else if (!(obj is T))

                throw GetExceptionForInvalidType<T>(obj.GetType().ToString(), argumentName);
        }

        /// <summary>
        /// Returns a given object when it is an instance of a given type, otherwise throws an <see cref="ArgumentException"/> with a given argument name.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="ArgumentException"/>.</param>
        /// <returns><paramref name="obj"/> when it is an instance of <typeparamref name="T"/>, otherwise throws an <see cref="ArgumentException"/> with <paramref name="argumentName"/> for the argument name.</returns>
        public static T GetOrThrowIfNotType<T>(in object obj, in string argumentName) where T : struct => obj is T _obj ? _obj : throw GetExceptionForInvalidType<T>(obj.GetType().ToString(), argumentName);

        /// <summary>
        /// Returns a given object when it is an instance of a given type, otherwise throws an <see cref="ArgumentNullException"/> if <paramref name="obj"/> is <see langword="null"/> or an <see cref="ArgumentException"/> with a given argument name otherwise.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <param name="argumentName">The argument name for the <see cref="ArgumentException"/>.</param>
        /// <returns><paramref name="obj"/> when it is an instance of <typeparamref name="T"/>, otherwise throws an <see cref="ArgumentNullException"/> if <paramref name="obj"/> is <see langword="null"/> or an <see cref="ArgumentException"/> with <paramref name="argumentName"/> for the argument name otherwise.</returns>
        public static T GetOrThrowIfNotTypeOrNull<T>(in object obj, in string argumentName) where T : class => (obj ?? throw GetArgumentNullException(argumentName)) is T _obj ? _obj : throw GetExceptionForInvalidType<T>(obj.GetType().ToString(), argumentName);

        public static InvalidOperationException GetExceptionForDispose(in string objectName, in bool forDisposing) => forDisposing
                ? new ObjectDisposingException(objectName)
                : (InvalidOperationException)new ObjectDisposedException(objectName, "The current object or value is disposed.");

        public static InvalidOperationException GetExceptionForDispose(in bool forDisposing) => new InvalidOperationException($"The current object or value is {(forDisposing ? "disposing" : "disposed")}.");

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

        public static void ThrowOnInvalidCopyToArrayOperation(in Array array, in int arrayIndex, in int count, in string arrayArgumentName, in string arrayIndexArgumentName)

        {

            if (array is null)

                throw new ArgumentNullException(arrayArgumentName);

            if (array.Rank != 1)

                throw new ArgumentException("Multidimensional arrays are not supported.", arrayArgumentName);

            if (array.GetLowerBound(0) != 0)

                throw new ArgumentException("The given array has a non-zero lower bound.", arrayArgumentName);

            if (arrayIndex < 0)

                throw new ArgumentOutOfRangeException(arrayIndexArgumentName);

            if (array.Length - arrayIndex < count)

                throw new ArgumentException("The given array has not enough space.", arrayArgumentName);

        }

        internal static void ThrowIfDisposedInternal(WinCopies.Util.DotNetFix.IDisposable obj)

        {

            if (obj.IsDisposed)

                throw GetExceptionForDispose(false);

        }

        public static void ThrowIfDisposed(WinCopies.Util.DotNetFix.IDisposable obj)

        {

            ThrowIfNull(obj, nameof(obj));

            ThrowIfDisposedInternal(obj);

        }

        internal static void ThrowIfDisposingInternal(IDisposable obj)

        {

            if (obj.IsDisposing)

                throw GetExceptionForDispose(true);

        }

        public static void ThrowIfDisposing(IDisposable obj)

        {

            ThrowIfNull(obj, nameof(obj));

            ThrowIfDisposingInternal(obj);

        }

        public static void ThrowIfDisposingOrDisposed(IDisposable obj)

        {

            ThrowIfNull(obj, nameof(obj));

            ThrowIfDisposedInternal(obj);

            ThrowIfDisposingInternal(obj);

        }

#if NETCORE || NETSTANDARD
        // https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/

        public static Process StartProcessNetCore(in string url)
        {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            // if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))

                return Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}") { CreateNoWindow = true });

            //else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            //{
            //    Process.Start("xdg-open", url);
            //}
            //else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            //{
            //    Process.Start("open", url);
            //}
            //else
            //{
            //    throw;
            //}
        }

        public static void StartProcessNetCore(in Uri url)
        {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))

                _ = Process.Start(new ProcessStartInfo("cmd", $"/c start {url.ToString().Replace("&", "^&")}") { CreateNoWindow = true });

            //else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            //{
            //    Process.Start("xdg-open", url);
            //}
            //else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            //{
            //    Process.Start("open", url);
            //}
            //else
            //{
            //    throw;
            //}
        }
#endif

    }

}
