﻿/* Copyright © Pierre Sprimont, 2021
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

        public static string
#if CS8
                ?
#endif
                ToString(object obj) => obj.ToString();
        public static string
#if CS8
                ?
#endif
                ToStringIn(in object obj) => obj.ToString();
        public static string
#if CS8
                ?
#endif
                ToStringRef(ref object obj) => obj.ToString();
        public static string
#if CS8
                ?
#endif
                ToStringT<T>(T value) => (value
#if CS8
            ??
#else
        == null ?
#endif
            throw GetArgumentNullException(nameof(value))
#if !CS8
             : value
#endif
            ).ToString();
        public static string
#if CS8
                ?
#endif
                ToStringInT<T>(in T value) => (value
#if CS8
            ??
#else
        == null ?
#endif
             throw GetArgumentNullException(nameof(value))
#if !CS8
             : value
#endif
            ).ToString();
        public static string
#if CS8
                ?
#endif
                ToStringRefT<T>(ref T value) => (value
#if CS8
            ??
#else
        == null ?
#endif
             throw GetArgumentNullException(nameof(value))
#if !CS8
             : value
#endif
            ).ToString();

        public static Converter<T, string
#if CS8
                ?
#endif
                > GetSurrounder<T>(in char decorator) => GetSurrounder<T>(decorator.ToString());
        public static Converter<T, string
#if CS8
                ?
#endif
                > GetSurrounder<T>(string decorator) => value => value?.ToString().Surround(decorator);

        private static T _GetIn<T>(in Func<bool> func, in T ifTrue, in T ifFalse) => GetIn(func, ifTrue, ifFalse);

        public static T Get<T>(Func<bool> func, T ifTrue, T ifFalse) => GetIn(func, ifTrue, ifFalse);
        public static T GetIn<T>(in Func<bool> func, in T ifTrue, in T ifFalse) => _GetIn(func ?? throw GetArgumentNullException(nameof(func)), ifTrue, ifFalse);
        public static T Get<T>(bool value, T ifTrue, T ifFalse) => GetIn(value, ifTrue, ifFalse);
        public static T GetIn<T>(in bool value, in T ifTrue, in T ifFalse) => value ? ifTrue : ifFalse;

        public static TOut GetIfNull<TIn, TOut>(Func<TIn> func, TOut ifTrue, TOut ifFalse) where TIn : class
#if CS8
                ?
#endif
                => GetIfNullIn(func, ifTrue, ifFalse);
        public static TOut GetIfNullIn<TIn, TOut>(in Func<TIn> func, in TOut ifTrue, in TOut ifFalse) where TIn : class
#if CS8
                ?
#endif
                => GetIfNullIn((func ?? throw GetArgumentNullException(nameof(func)))(), ifTrue, ifFalse);
        public static TOut GetIfNull<TIn, TOut>(TIn value, TOut ifTrue, TOut ifFalse) where TIn : class
#if CS8
                ?
#endif
                => GetIfNullIn(value, ifTrue, ifFalse);
        public static TOut GetIfNullIn<TIn, TOut>(TIn value, in TOut ifTrue, in TOut ifFalse) where TIn : class
#if CS8
                ?
#endif
                => _GetIn(() => value == null, ifTrue, ifFalse);
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

        public static bool FirstBool(bool ok, object
#if CS8
                ?
#endif
                obj) => FirstBoolIn(ok, obj);
        public static bool FirstBoolIn(in bool ok, in object
#if CS8
                ?
#endif
                obj) => ok;
        public static bool FirstBoolGeneric<T>(bool ok, T value) => FirstBoolIn(ok, value);
        public static bool FirstBoolInGeneric<T>(in bool ok, in T value) => FirstBoolIn(ok, value);

        private static T2 PrependPredicate<T1, T2>(T1 x, Func<T2> func) where T1 : class
#if CS8
                ?
#endif
                => x == null ? throw GetArgumentNullException(nameof(x)) : func();

        private static Predicate GetPredicate(Predicate x, Predicate y) => value => x(value) && y(value);
        private static Predicate<T> GetPredicate<T>(Predicate<T> x, Predicate<T> y) => value => x(value) && y(value);
        private static PredicateIn GetPredicateIn(Predicate x, PredicateIn y) => (in object value) => x(value) && y(value);
        private static PredicateIn GetPredicateIn(PredicateIn x, PredicateIn y) => (in object value) => x(value) && y(value);
        private static PredicateIn<T> GetPredicateIn<T>(Predicate<T> x, PredicateIn<T> y) => (in T value) => x(value) && y(value);
        private static PredicateIn<T> GetPredicateIn<T>(PredicateIn<T> x, PredicateIn<T> y) => (in T value) => x(value) && y(value);

        public static Predicate PrependPredicateNULL(Predicate x, Predicate
#if CS8
                ?
#endif
                y) => PrependPredicate(x, () => y == null ? x : GetPredicate(x, y));
        public static Predicate<T> PrependPredicateNULL<T>(Predicate<T> x, Predicate<T>
#if CS8
                ?
#endif
                y) => PrependPredicate(x, () => y == null ? x : value => x(value) && y(value));
        public static PredicateIn PrependPredicateInNULL(PredicateIn x, PredicateIn
#if CS8
                ?
#endif
                y) => PrependPredicate(x, () => y == null ? x : (in object obj) => x(obj) && y(obj));
        public static PredicateIn<T> PrependPredicateInNULL<T>(PredicateIn<T> x, PredicateIn<T>
#if CS8
                ?
#endif
                y) => PrependPredicate(x, () => y == null ? x : (in T value) => x(value) && y(value));

        public static Predicate PrependPredicate(Predicate x, Predicate y) => PrependPredicate(x, () => y == null ? throw GetArgumentNullException(nameof(y)) : GetPredicate(x, y));
        public static Predicate<T> PrependPredicate<T>(Predicate<T> x, Predicate<T> y) => PrependPredicate(x, () => y == null ? throw GetArgumentNullException(nameof(y)) : GetPredicate(x, y));
        public static PredicateIn PrependPredicateIn(PredicateIn x, PredicateIn y) => PrependPredicate(x, () => y == null ? throw GetArgumentNullException(nameof(y)) : GetPredicateIn(x, y));
        public static PredicateIn<T> PrependPredicateIn<T>(PredicateIn<T> x, PredicateIn<T> y) => PrependPredicate(x, () => y == null ? throw GetArgumentNullException(nameof(y)) : GetPredicateIn(x, y));

        public static PredicateIn PrependPredicateIn(Predicate x, PredicateIn y) => PrependPredicate(x, () => y == null ? throw GetArgumentNullException(nameof(y)) : GetPredicateIn(x, y));
        public static PredicateIn<T> PrependPredicateIn<T>(Predicate<T> x, PredicateIn<T> y) => PrependPredicate(x, () => y == null ? throw GetArgumentNullException(nameof(y)) : GetPredicateIn(x, y));

        public static Func<bool, object, bool> PrependPredicate(Predicate
#if CS8
                ?
#endif
                predicate)
#if CS9
            =>
#else
        {
            if (
#endif
                predicate == null
#if CS9
            ?
#else
            ) return
#endif
                FirstBool
#if CS9
                :
#else
                ;

            else return
#endif
            (bool ok, object value) => ok && predicate(value);
#if !CS9
        }
#endif

        public static Func<bool, T, bool> PrependPredicate<T>(Predicate<T>
#if CS8
                ?
#endif
                predicate)
#if CS9
            =>
#else
        {
            if (
#endif
                predicate == null
#if CS9
                ?
#else
                ) return
#endif
                FirstBoolGeneric

#if CS9
                :
#else
                ;

            else return
#endif
                    (bool ok, T value) => ok && predicate(value);
#if !CS9
        }
#endif

        public static FuncIn<bool, object, bool> PrependPredicateIn(Predicate
#if CS8
                ?
#endif
                predicate)
#if CS9
            =>
#else
        {
            if (
#endif
         predicate == null
#if CS9
        ?
#else
        ) return
#endif
        FirstBoolIn
#if CS9

            :
#else
            ;

            else return
#endif
            (in bool ok, in object value) => ok && predicate(value);
#if !CS9
        }
#endif

        public static FuncIn<bool, T, bool> PrependPredicateIn<T>(Predicate<T>
#if CS8
                ?
#endif
                predicate)
#if CS9
            =>
#else
        {
            if (
#endif
         predicate == null
#if CS9
         ?
#else
         )

                return
#endif
            FirstBoolInGeneric
#if CS9
                :
#else
                ;

            else return
#endif
                (in bool ok, in T value) => ok && predicate(value);
#if !CS9
        }
#endif
    }
}
