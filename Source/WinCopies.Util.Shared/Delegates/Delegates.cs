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

using System;
using System.Collections.Generic;

using static WinCopies.
#if WinCopies3
    ThrowHelper;
#else
    Util.Util;
#endif

namespace WinCopies
#if !WinCopies3
.Util
#endif
{
    public delegate bool PredicateIn<T>(in T value);

    public delegate bool PredicateOut<TIn, TOut>(TIn param, out TOut result);

    public delegate bool PredicateInOut<TIn, TOut>(in TIn param, out TOut result);

    /// <summary>
    /// This class contains static methods that can be used as delegates.
    /// </summary>
    public static class Delegates
    {
        public static void EmptyVoid(object parameter) { }

        public static void EmptyVoid<T>(T parameter) { }

        public static object Null(object parameter) => null;

        public static T Null<T>(T parameter) where T : class => null;

        public static TOut Null<TIn, TOut>(TIn parameter) where TOut : class => null;

        public static T Self<T>(T value) => value;

        public static T SelfIn<T>(in T value) => value;

        public static object NullIn(in object parameter) => null;

        public static T NullIn<T>(in T parameter) where T : class => null;

        public static TOut NullIn<TIn, TOut>(in TIn parameter) where TOut : class => null;

        public static TOut Convert<TIn, TOut>(TIn value) where TOut : TIn => (TOut)value;

        public static TOut ConvertIn<TIn, TOut>(in TIn value) where TOut : TIn => (TOut)value;

        public static TOut ConvertBack<TIn, TOut>(TIn value) where TIn : TOut => value;

        public static TOut ConvertBackIn<TIn, TOut>(in TIn value) where TIn : TOut => value;

        public static TKey GetKeyIn<TKey, TValue>(in KeyValuePair<TKey, TValue> item) => item.Key;

        public static TKey GetKey<TKey, TValue>(KeyValuePair<TKey, TValue> item) => GetKeyIn(item);

        public static TValue GetValueIn<TKey, TValue>(in KeyValuePair<TKey, TValue> item) => item.Value;

        public static TValue GetValue<TKey, TValue>(KeyValuePair<TKey, TValue> item) => GetValueIn(item);

        public static bool CompareEqualityIn(in object x, in object y) => object.Equals(x, y);

        public static bool CompareEquality(object x, object y) => CompareEqualityIn(x, y);

        public static bool CompareEqualityGenericIn<T>(in T x, in T y) => CompareEqualityIn(x, y);

        public static bool CompareEqualityGeneric<T>(T x, T y) => CompareEqualityIn(x, y);

        public static bool CompareHashCodeIn(in object x, in object y) => x == null ? y == null : y != null && x.GetHashCode() == y.GetHashCode();

        public static bool CompareHashCode(object x, object y) => CompareHashCodeIn(x, y);

        public static bool CompareHashCodeGenericIn<T>(in T x, in T y) => CompareHashCodeIn(x, y);

        public static bool CompareHashCodeGeneric<T>(T x, T y) => CompareHashCodeIn(x, y);
    }

    /// <summary>
    /// This class contains static methods that can be used as delegates for <see cref="bool"/> values.
    /// </summary>
    public static class Bool
    {
        public static bool And(bool x, bool y) => x && y;

        public static bool Or(bool x, bool y) => x || y;

        public static bool XOr(bool x, bool y) => x ^ y;

        public static bool Reversed(bool value) => !value;

        public static bool AndIn(in bool x, in bool y) => x && y;

        public static bool OrIn(in bool x, in bool y) => x || y;

        public static bool XOrIn(in bool x, in bool y) => x ^ y;

        public static bool ReversedIn(in bool value) => !value;

#if !WinCopies3
        public static bool True(bool value) => true;

        public static bool False(bool value) => false;
#endif

        public static bool True() => true;

        public static bool False() => false;

        public static bool True(object value) => true;

        public static bool False(object value) => false;

        public static bool True<T>(T value) => true;

        public static bool False<T>(T value) => false;

        public static bool IsTrue(bool? value) => value == true;

        public static bool IsFalse(bool? value) => value == false;

        public static bool TrueIn(in object value) => true;

        public static bool FalseIn(in object value) => false;

        public static bool TrueIn<T>(in T value) => true;

        public static bool FalseIn<T>(in T value) => false;

        public static bool IsTrueIn(in bool? value) => value == true;

        public static bool IsFalseIn(in bool? value) => value == false;

        public static bool GetReversedBoolFunc(in Func<bool> func) => !GetOrThrowIfNull(func, nameof(func))();

        public static bool GetReversedBoolFunc<T>(in FuncOut<T, bool> func, out T result) => !GetOrThrowIfNull(func, nameof(func))(out result);

        public static bool GetReversedBoolFunc<T>(in Predicate<T> func, in T param) => !GetOrThrowIfNull(func, nameof(func))(param);

        public static bool GetReversedBoolFunc<TPredicateParam, TOut>(in PredicateOut<TPredicateParam, TOut> func, in TPredicateParam param, out TOut result) => !GetOrThrowIfNull(func, nameof(func))(param, out result);
    }
}
