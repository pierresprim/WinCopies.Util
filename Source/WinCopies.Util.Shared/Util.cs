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
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

using WinCopies.Util;

#if WinCopies3
using static WinCopies.ThrowHelper;

namespace WinCopies
{
#else
using System.ComponentModel;

using WinCopies.Collections;
using WinCopies.Util.Resources;

using static WinCopies.Util.ThrowHelper;

#if CS6
using IfCT = WinCopies.Util.Util.ComparisonType;
using IfCM = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;
#endif

namespace WinCopies.Util
{
#endif
    public sealed class NullableGeneric<T>
    {
        public T Value { get; }

        public NullableGeneric(T value) => Value = value;
    }

    public interface IPropertyObservable : DotNetFix.IDisposable
    {
        void AddPropertyChangedDelegate(Action<string> action);

        void RemovePropertyChangedDelegate(Action<string> action);
    }

    public sealed class NullableReference<T> where T : class
    {
        public T Value { get; }

        public NullableReference(T value) => Value = value;
    }

    /// <summary>
    /// Provides some static helper methods.
    /// </summary>
    public static class
#if !WinCopies3
        Util
#else
        UtilHelpers
#endif
    {
        public const string NotApplicable = "N/A";

        public const BindingFlags DefaultBindingFlagsForPropertySet = BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        public static void Lambda<T>(Action<Action> action, ActionRef<T> actionRef, ref T value)
        {
            T _value = value;

            action(() => actionRef(ref _value));

            value = _value;
        }

        public static void Lambda<T>(Action<Action, T> action, ActionRef<T> actionRef, ref T value)
        {
            T _value = value;

            action(() => actionRef(ref _value), value);

            value = _value;
        }

        public static void Lambda<T>(ActionIn<Action> action, ActionRef<T> actionRef, ref T value)
        {
            T _value = value;

            action(() => actionRef(ref _value));

            value = _value;
        }

        public static void Lambda<T>(ActionIn<Action, T> action, ActionRef<T> actionRef, ref T value)
        {
            T _value = value;

            action(() => actionRef(ref _value), value);

            value = _value;
        }

        public static void Lambda<T1, T2>(Action<Action, T1> action, ActionRef<T1, T2> actionRef, ref T1 value1, ref T2 value2)
        {
            T1 _value1 = value1;
            T2 _value2 = value2;

            action(() => actionRef(ref _value1, ref _value2), value1);

            value2 = _value2;
            value1 = _value1;
        }

        public static void Lambda<T1, T2>(Action<Action, T1, T2> action, ActionRef<T1, T2> actionRef, ref T1 value1, ref T2 value2)
        {
            T1 _value1 = value1;
            T2 _value2 = value2;

            action(() => actionRef(ref _value1, ref _value2), value1, value2);

            value2 = _value2;
            value1 = _value1;
        }

        public static void Lambda<T1, T2>(ActionIn<Action, T1> action, ActionRef<T1, T2> actionRef, ref T1 value1, ref T2 value2)
        {
            T1 _value1 = value1;
            T2 _value2 = value2;

            action(() => actionRef(ref _value1, ref _value2), value1);

            value2 = _value2;
            value1 = _value1;
        }

        public static void Lambda<T1, T2>(ActionIn<Action, T1, T2> action, ActionRef<T1, T2> actionRef, ref T1 value1, ref T2 value2)
        {
            T1 _value1 = value1;
            T2 _value2 = value2;

            action(() => actionRef(ref _value1, ref _value2), value1, value2);

            value2 = _value2;
            value1 = _value1;
        }

        public static T GetValue<T>(Func<T> func) => func == null ? default : func();
        public static TResult GetValue<TParam, TResult>(in Func<TParam, TResult> func, in TParam param) => func == null ? default : func(param);
        public static TResult GetValue<T1, T2, TResult>(in Func<T1, T2, TResult> func, in T1 param1, in T2 param2) => func == null ? default : func(param1, param2);
        public static TResult GetValue<T1, T2, T3, TResult>(in Func<T1, T2, T3, TResult> func, in T1 param1, in T2 param2, in T3 param3) => func == null ? default : func(param1, param2, param3);
        public static TResult GetValue<T1, T2, T3, T4, TResult>(in Func<T1, T2, T3, T4, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4) => func == null ? default : func(param1, param2, param3, param4);
#if CS5
        public static TResult GetValue<T1, T2, T3, T4, T5, TResult>(in Func<T1, T2, T3, T4, T5, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5) => func == null ? default : func(param1, param2, param3, param4, param5);
        public static TResult GetValue<T1, T2, T3, T4, T5, T6, TResult>(in Func<T1, T2, T3, T4, T5, T6, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6) => func == null ? default : func(param1, param2, param3, param4, param5, param6);
        public static TResult GetValue<T1, T2, T3, T4, T5, T6, T7, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7);
        public static TResult GetValue<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8);
        public static TResult GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9);
        public static TResult GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
        public static TResult GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11);
        public static TResult GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11, in T12 param12) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12);
        public static TResult GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11, in T12 param12, in T13 param13) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13);
        public static TResult GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11, in T12 param12, in T13 param13, in T14 param14) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14);
        public static TResult GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11, in T12 param12, in T13 param13, in T14 param14, in T15 param15) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15);
        public static TResult GetValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(in Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func, in T1 param1, in T2 param2, in T3 param3, in T4 param4, in T5 param5, in T6 param6, in T7 param7, in T8 param8, in T9 param9, in T10 param10, in T11 param11, in T12 param12, in T13 param13, in T14 param14, in T15 param15, in T16 param16) => func == null ? default : func(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14, param15, param16);
#endif

        public static void Reverse<T>(ref T x, ref T y)
        {
            T z = x;

            x = y;

            y = z;
        }

        public static void Reverse<T>(in T x, in T y, in Action<T> setX, in Action<T> setY)
        {
            ThrowIfNull(setX, nameof(setX));
            ThrowIfNull(setY, nameof(setY));

            setX(y);

            setY(x);
        }

        public static bool UpdateValue<T>(ref T value, in T newValue)
        {
            if (object.Equals(value, newValue))

                return false;

            value = newValue;

            return true;
        }

        public static bool UpdateValue<T>(ref T value, T newValue, Action<T, T> action)
        {
            T _value = value;

            return UpdateValue(ref value, newValue, () => action(_value, newValue));
        }

        public const string Etcetera = "...";

        private static bool _TruncateIfLonger(ref string s, in int index, in FuncIn2<string, int, string> func)
        {
            if ((s ?? throw GetArgumentNullException(nameof(s))).Length > index)
            {
                s = func(s, index);

                return true;
            }

            return false;
        }

        private static bool _TruncateIfLonger(ref string s, int index, string replace, FuncIn2<string, int, string, string> func) => _TruncateIfLonger(ref s, index, (string _s, in int _index) => func(_s, _index, replace));

        public static bool TruncateIfLonger(ref string s, int index, string replace) => _TruncateIfLonger(ref s, index, replace, Extensions.Truncate);

        public static bool TruncateIfLonger2(ref string s, in int index, in string replace) => _TruncateIfLonger(ref s, index, replace, Extensions.Truncate2);

        public static bool TruncateIfLonger(ref string s, in int index) => _TruncateIfLonger(ref s, index, Extensions.Truncate);

#if !CS5
        public static Type GetEnumUnderlyingType<T>() where T : Enum => typeof(T)._GetEnumUnderlyingType();
#endif

        public static bool For(in Func<bool> loopCondition, in Func<bool> action, in Action postIterationAction)
        {
            while (loopCondition())
            {
                if (action())

                    return true;

                postIterationAction();
            }

            return false;
        }

        public static bool For(in Func<bool> loopCondition, Action action, in Action postIterationAction) => For(loopCondition, () =>
        {
            try
            {
                action();

                return true;
            }

            catch { return false; }

        }, postIterationAction);

        public static bool PredicateRef<T>(object value, Predicate<T> predicate) where T : class
#if CS9
            => predicate(value is T _value ? _value : value == null ? null : throw GetInvalidTypeArgumentException(nameof(value)));
#else

        {
            if (value is T _value)

                return predicate(_value);

            else if (value == null)

                return predicate(null);

            throw GetInvalidTypeArgumentException(nameof(value));
        }
#endif

        public static bool PredicateVal<T>(object value, Predicate<T> predicate) where T : struct => predicate(value is T _value ? _value : throw GetInvalidTypeArgumentException(nameof(value)));

        public static bool UpdateValue<T>(ref T value, in T newValue, in Action action)
        {
            if (UpdateValue(ref value, newValue))
            {
                action();

                return true;
            }

            return false;
        }

        public static bool IsNullOrEmpty(in Array array) => array == null || array.Length == 0;

        public static TValue GetValue<TKey, TValue>(KeyValuePair<TKey, TValue> keyValuePair) => keyValuePair.Value;

#if !WinCopies3 && CS7
        [Obsolete("This method has been replaced by the WinCopies.Util.Extensions.SetBackgroundWorkerProperty method overloads.")]
        public static (bool propertyChanged, object oldValue) SetPropertyWhenNotBusy<T>(T bgWorker, string propertyName, string fieldName, object newValue, Type declaringType, BindingFlags bindingFlags = DefaultBindingFlagsForPropertySet, bool throwIfBusy = true) where T : IBackgroundWorker, INotifyPropertyChanged => bgWorker.IsBusy
                ? throwIfBusy ? throw new InvalidOperationException("Cannot change property value when BackgroundWorker is busy.") : (false, Extensions.GetField(fieldName, declaringType, bindingFlags).GetValue(bgWorker))
                : bgWorker.SetProperty(propertyName, fieldName, newValue, declaringType, true, bindingFlags);
#endif

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

#if !WinCopies3
#if CS6
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
#endif

        [Obsolete("This method has been replaced by the WinCopies.Util.Extensions.ThrowIfInvalidEnumValue(this Enum, bool, params Enum[]) method.")]
        public static void ThrowOnEnumNotValidEnumValue(in Enum value, params Enum[] values) => value.ThrowIfInvalidEnumValue(false, values);

        public static ArgumentException GetExceptionForNonFlagsEnum(in string argumentName) => new ArgumentException(ExceptionMessages.NonFlagsEnumException, argumentName);

        public static TypeArgumentException GetExceptionForNonFlagsEnumType(in string typeArgumentName) => new TypeArgumentException(ExceptionMessages.NonFlagsEnumTypeException, typeArgumentName);

#if CS7
        public static void ThrowIfNotFlagsEnumType<T>(in string typeArgumentName) where T : Enum
        {
            if (!IsFlagsEnum<T>())

                throw GetExceptionForNonFlagsEnum(typeArgumentName);
        }
#endif
#endif

#if CS7
        public static bool IsFlagsEnum<T>() where T : Enum => typeof(T).GetCustomAttribute<FlagsAttribute>() is object;
#endif

        // public static KeyValuePair<TKey, Func<bool>>[] GetIfKeyValuePairPredicateArray<TKey>(params KeyValuePair<TKey, Func<bool>>[] keyValuePairs) => keyValuePairs;

#if !WinCopies3 && CS6
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
#if CS6
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
#endif
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

#if CS8

            return comparison switch
            {
                IfComp.Equal => comparisonDelegate(value, valueToCompare),

                IfComp.NotEqual => !predicateResult() || !comparisonDelegate(value, valueToCompare),

#pragma warning disable IDE0002

                IfComp.ReferenceEqual => object.ReferenceEquals(value, valueToCompare),

#pragma warning restore IDE0002

                _ => false
            };

#else

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

#endif
        }

        private static bool CheckEqualityComparison<T>(in IfComp comparison, in T value, in T valueToCompare, in Func<bool> predicateResult, in EqualityComparison<T> comparisonDelegate)
        {
            // Because we've already checked that for the 'T' type in the 'If' method and assuming that 'T' is the base type of all the values to test, if 'T' is actually a class, we don't need to check here if the type of the current value is actually a class when comparison is set to ReferenceEqual.

            if (comparison != IfComp.NotEqual && !predicateResult()) return false;

#if CS8
            return comparison switch
            {
                IfComp.Equal => comparisonDelegate(value, valueToCompare),

                IfComp.NotEqual => !predicateResult() || !comparisonDelegate(value, valueToCompare),

#pragma warning disable IDE0002

                IfComp.ReferenceEqual => object.ReferenceEquals(value, valueToCompare),

#pragma warning restore IDE0002

                _ => false
            };
#else
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
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, System.Collections.Generic.EqualityComparer<object>.Default, GetCommonPredicate(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="System.Collections.IComparer"/> and <see cref="Predicate{T}"/>.
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
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, System.Collections.IComparer comparer, in Predicate<object> predicate, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of values using a custom <see cref="System.Collections.IComparer"/> and <see cref="Predicate"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparisonMode">Whether to perform a binary or a logical comparison.</param>
        /// <param name="comparison">Whether to perform an equality or an inequality comparison, and, if to perform an inequality comparison, the type of the inequality comparison to perform.</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, System.Collections.IComparer comparer, in Predicate predicate, in object value, params object[] values) => If(comparisonType, comparisonMode, comparison, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

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

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, System.Collections.IComparer comparer, in object value, params KeyValuePair<object, Func<bool>>[] values) => If(comparisonType, comparisonMode, comparison, new WinCopies.Collections.Comparison((object x, object y) => comparer.Compare(x, y)), value, values);

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
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, System.Collections.Generic.EqualityComparer<object>.Default, GetCommonPredicate(), value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="System.Collections.IComparer"/> and <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        [Obsolete("This method has been replaced by the following method: If(ComparisonType, ComparisonMode, Comparison, out object, IComparer, Predicate, object, params KeyValuePair<object, object>[])")]
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, System.Collections.IComparer comparer, in Predicate<object> predicate, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

        /// <summary>
        /// Performs a comparison by testing a value compared to an array of objects or values using a custom <see cref="System.Collections.IComparer"/> and <see cref="Predicate{Object}"/>.
        /// </summary>
        /// <param name="comparisonType">Whether to perform an 'and', 'or' or 'xor' comparison.</param>
        /// <param name="comparison">The comparison type</param>
        /// <param name="comparer">The comparer used to compare the values.</param>
        /// <param name="value">The value to compare with.</param>
        /// <param name="values">The values to compare.</param>
        /// <param name="predicate">The comparison predicate</param>
        /// <returns><see langword="true"/> if the comparison has succeeded for all values, otherwise <see langword="false"/>.</returns>
        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, System.Collections.IComparer comparer, in Predicate predicate, in object value, params KeyValuePair<object, object>[] values) => If(comparisonType, comparisonMode, comparison, out key, (object x, object y) => comparer.Compare(x, y), predicate, value, values);

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

        public static bool If(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, out object key, System.Collections.IComparer comparer, in object value, params KeyValuePair<object, KeyValuePair<object, Func<bool>>>[] values) => If(comparisonType, comparisonMode, comparison, out key, new WinCopies.Collections.Comparison((object x, object y) => comparer.Compare(x, y)), value, values);

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
        public static bool If<T>(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, in T value, params T[] values) => If(comparisonType, comparisonMode, comparison, System.Collections.Generic.EqualityComparer<T>.Default, GetCommonPredicate<T>(), value, values);

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
        #endregion
#endif

#if CS5
        public static bool IsNullEmptyOrWhiteSpace(in string value) => string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
#endif

#if !WinCopies3 && CS5
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
#endif

        public static BitArray GetBitArray(long[] values)
        {
            var array = new BitArray(values.Length * 64);

            for (int i = 0; i < values.Length; i++)

                new BitArray(BitConverter.GetBytes(values[i])).CopyTo(array, i * 64);

            return array;
        }

        /// <summary>
        /// Concatenates multiple arrays from a same item type. Arrays must have only one dimension.
        /// </summary>
        /// <param name="arrays">The different arrays to concatenate.</param>
        /// <returns>An array with a copy of all values of the given arrays.</returns>
        public static T[] Concatenate<T>(params T[][] arrays)
        {
            // /// <param name="elementType">The type of the items inside the tables.</param>

            int totalArraysLength = 0;

            int totalArraysIndex = 0;

            foreach (T[] array in arrays)

                totalArraysLength += array.Length;

            var newArray = new T[totalArraysLength];

            T[] _array;

            for (int i = 0; i < arrays.Length - 1; i++)
            {
                _array = arrays[i];

                _array.CopyTo(newArray, totalArraysIndex);

                totalArraysIndex += _array.Length;
            }

#if CS8
            arrays[^1].CopyTo(newArray, totalArraysIndex);
#else
            arrays[arrays.Length - 1].CopyTo(newArray, totalArraysIndex);
#endif

            return newArray;
        }

#if !WinCopies3
        /// <summary>
        /// Concatenates multiple arrays from a same item type using the <see cref="Array.LongLength"/> length property. Arrays must have only one dimension.
        /// </summary>
        /// <param name="arrays">The different tables to concatenate.</param>
        /// <returns></returns>
        [Obsolete("Use the Concatenate method instead.")]
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
#endif

        /// <summary>
        /// Checks if a object is a numeric value (an instance of a numeric value type).
        /// </summary>
        /// <remarks>This function makes a check for the object type. For a string-parsing-checking for numerical value, look at the <see cref="IsNumeric(in string, out decimal)"/> function.</remarks>
        /// <param name="value">The object to check</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the object given is from a numerical type.</returns>
        public static bool IsNumber(in object value)
        {
            switch (value)
            {
                case byte _:
                case sbyte _:
                case short _:
                case ushort _:
                case int _:
                case uint _:
                case long _:
                case ulong _:
                case float _:
                case double _:
                case decimal _:

                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a <see cref="string"/> is a numerical value.
        /// </summary>
        /// <remarks>This function tries to parse a <see cref="string"/> value to a <see cref="decimal"/> value. Given that <see cref="decimal"/> type is the greatest numerical type in the .NET framework, all the numbers can be supported in the .NET framework can be set in a <see cref="decimal"/> object.</remarks>
        /// <param name="s">The <see cref="string"/> to check</param>
        /// <param name="d">The <see cref="decimal"/> in which one set the <see cref="decimal"/> value</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the <see cref="string"/> given is a <see cref="decimal"/>.</returns>
        public static bool IsNumeric(in string s, out decimal d) => decimal.TryParse(s, out d);

#if CS7
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

            return enumType.GetEnumUnderlyingType().IsType(typeof(sbyte), typeof(short), typeof(int), typeof(long)) ? (T)Enum.ToObject(enumType, NumericArrayToLongValue(array)) : (T)Enum.ToObject(enumType, NumericArrayToULongValue(array));
        }
#endif

        public static long NumericArrayToLongValue(Array array)
        {
            ThrowIfNull(array, nameof(array));

            long values = 0;

            foreach (object value in array)

                values |= (long)Convert.ChangeType(value, TypeCode.Int64, CultureInfo.CurrentCulture);

            return values;
        }

        public static ulong NumericArrayToULongValue(Array array)
        {
            ThrowIfNull(array, nameof(array));

            ulong values = 0;

            foreach (object value in array)

                values |= (ulong)Convert.ChangeType(value, TypeCode.UInt64, CultureInfo.CurrentCulture);

            return values;
        }

        /// <summary>
        /// Gets the numeric value for a field in an enum.
        /// </summary>
        /// <param name="enumType">The enum type in which to look for the specified enum field value.</param>
        /// <param name="fieldName">The enum field to look for.</param>
        /// <returns>The numeric value corresponding to this enum, in the given enum type underlying type.</returns>
        public static object GetNumValue
#if !WinCopies3
            (in Type enumType,
#else
            <T>(
#endif
            in string fieldName)
#if WinCopies3
where T : Enum
#endif
        {
#if !WinCopies3
            ThrowIfNull(enumType, nameof(enumType));
#else
            Type enumType = typeof(T);
#endif

            return enumType.IsEnum ? Convert.ChangeType(enumType.GetField(fieldName).GetValue(null), Enum.GetUnderlyingType(enumType)) : throw new ArgumentException("'enumType' is not an enum type.");
        }

#if !WinCopies3
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

        public static void ThrowOnInvalidCopyToArrayOperation(in Array array, in int arrayIndex, in uint count, in string arrayArgumentName, in string arrayIndexArgumentName)
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

        internal static void ThrowIfDisposedInternal(DotNetFix.IDisposable obj)
        {
            if (obj.IsDisposed)

                throw GetExceptionForDispose(false);
        }

        public static void ThrowIfDisposed(DotNetFix.IDisposable obj)
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
#endif

#if CS8
        // https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/

        public static Process StartProcessNetCore(in string url) =>

             // hack because of this: https://github.com/dotnet/corefx/issues/10361
             // if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))

             Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}") { CreateNoWindow = true });

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
