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

using static WinCopies.
#if !WinCopies3
    Util.
#endif
    Bool;

using static WinCopies.
#if WinCopies3
    ThrowHelper;
#else
    Util.Util;
#endif

namespace WinCopies.Util.Shared.Delegates
{
    public static class ActionDelegates
    {
        private static bool _RunActionIfTRUE(in Func<bool> func, in Action action)
        {
            ThrowIfNull(action, nameof(action));

            if (func())
            {
                action();

                return true;
            }

            return false;
        }

        public static bool RunActionIfTRUE(in Func<bool> func, in Action action) => _RunActionIfTRUE(GetOrThrowIfNull(func, nameof(func)), action);

        public static bool RunActionIfFALSE(Func<bool> func, in Action action) => _RunActionIfTRUE(() => GetReversedBoolFunc(func), action);

        private static T _RunFuncIfTRUE<T>(in Func<bool> func, in Func<T> action, in T defaultValue, out bool result)
        {
            ThrowIfNull(action, nameof(action));

            if (func())
            {
                result = true;

                return action();
            }

            result = false;

            return defaultValue;
        }

        public static T RunFuncIfTRUE<T>(in Func<bool> func, in Func<T> action, in T defaultValue, out bool result) => _RunFuncIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, defaultValue, out result);

        public static T RunFuncIfFALSE<T>(Func<bool> func, in Func<T> action, in T defaultValue, out bool result) => _RunFuncIfTRUE(() => GetReversedBoolFunc(func), action, defaultValue, out result);

        private static bool _RunActionIfTRUE<T>(in Predicate<T> func, in Action action, in T param)
        {
            ThrowIfNull(action, nameof(action));

            if (func(param))
            {
                action();

                return true;
            }

            return false;
        }

        public static bool RunActionIfTRUE<T>(in Predicate<T> func, in Action action, in T param) => _RunActionIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, param);

        public static bool RunActionIfFALSE<T>(Predicate<T> func, in Action action, in T param) => _RunActionIfTRUE(_param => GetReversedBoolFunc(func, _param), action, param);

        private static TFuncReturnParam _RunFuncIfTRUE<TPredicateParam, TFuncReturnParam>(in Predicate<TPredicateParam> func, in Func<TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out bool result)
        {
            ThrowIfNull(action, nameof(action));

            if (func(param))
            {
                result = true;

                return action();
            }

            result = false;

            return defaultValue;
        }

        public static TFuncReturnParam RunFuncIfTRUE<TPredicateParam, TFuncReturnParam>(in Predicate<TPredicateParam> func, in Func<TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out bool result) => _RunFuncIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, param, defaultValue, out result);

        public static TFuncReturnParam RunFuncIfFALSE<TPredicateParam, TFuncReturnParam>(Predicate<TPredicateParam> func, in Func<TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out bool result) => _RunFuncIfTRUE(_param => GetReversedBoolFunc(func, _param), action, param, defaultValue, out result);

        private static bool _RunActionIfTRUE<T>(in FuncOut<T, bool> func, in Action action, out T result)
        {
            ThrowIfNull(action, nameof(action));

            if (func(out result))
            {
                action();

                return true;
            }

            return false;
        }

        public static bool RunActionIfTRUE<T>(in FuncOut<T, bool> func, in Action action, out T result) => _RunActionIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, out result);

        public static bool RunActionIfFALSE<T>(FuncOut<T, bool> func, in Action action, out T result) => _RunActionIfTRUE((out T _result) => GetReversedBoolFunc(func, out _result), action, out result);

        private static TResult _RunFuncIfTRUE<TOut, TResult>(in FuncOut<TOut, bool> func, in Func<TResult> action, in TResult defaultValue, out TOut funcResult, out bool result)
        {
            ThrowIfNull(action, nameof(action));

            if (func(out funcResult))
            {
                result = true;

                return action();
            }

            result = false;

            return defaultValue;
        }

        public static TResult RunFuncIfTRUE<TOut, TResult>(in FuncOut<TOut, bool> func, in Func<TResult> action, in TResult defaultValue, out TOut funcResult, out bool result) => _RunFuncIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, defaultValue, out funcResult, out result);

        public static TResult RunFuncIfFALSE<TOut, TResult>(FuncOut<TOut, bool> func, in Func<TResult> action, in TResult defaultValue, out TOut funcResult, out bool result) => _RunFuncIfTRUE((out TOut _funcResult) => GetReversedBoolFunc(func, out _funcResult), action, defaultValue, out funcResult, out result);

        private static bool _RunActionIfTRUE<TPredicateParam, TOut>(in PredicateOut<TPredicateParam, TOut> func, in Action action, in TPredicateParam param, out TOut result)
        {
            ThrowIfNull(action, nameof(action));

            if (func(param, out result))
            {
                action();

                return true;
            }

            return false;
        }

        public static bool RunActionIfTRUE<TPredicateParam, TOut>(in PredicateOut<TPredicateParam, TOut> func, in Action action, in TPredicateParam param, out TOut result) => _RunActionIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, param, out result);

        public static bool RunActionIfFALSE<TPredicateParam, TOut>(PredicateOut<TPredicateParam, TOut> func, in Action action, in TPredicateParam param, out TOut result) => _RunActionIfTRUE((TPredicateParam _param, out TOut _result) => GetReversedBoolFunc(func, _param, out _result), action, param, out result);

        private static TFuncReturnParam _RunFuncIfTRUE<TPredicateParam, TOut, TFuncReturnParam>(in PredicateOut<TPredicateParam, TOut> func, in Func<TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out TOut predicateResult, out bool result)
        {
            ThrowIfNull(action, nameof(action));

            if (func(param, out predicateResult))
            {
                result = true;

                return action();
            }

            result = false;

            return defaultValue;
        }

        public static TFuncReturnParam RunFuncIfTRUE<TPredicateParam, TOut, TFuncReturnParam>(in PredicateOut<TPredicateParam, TOut> func, in Func<TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out TOut predicateResult, out bool result) => _RunFuncIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, param, defaultValue, out predicateResult, out result);

        public static TFuncReturnParam RunFuncIfFALSE<TPredicateParam, TOut, TFuncReturnParam>(PredicateOut<TPredicateParam, TOut> func, in Func<TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out TOut predicateResult, out bool result) => _RunFuncIfTRUE((TPredicateParam _param, out TOut _result) => GetReversedBoolFunc(func, _param, out _result), action, param, defaultValue, out predicateResult, out result);

        private static bool _RunActionIfTRUE<T>(in FuncOut<T, bool> func, in Action<T> action, out T result)
        {
            ThrowIfNull(action, nameof(action));

            if (func(out result))
            {
                action(result);

                return true;
            }

            return false;
        }

        public static bool RunActionIfTRUE<T>(in FuncOut<T, bool> func, in Action<T> action, out T result) => _RunActionIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, out result);

        public static bool RunActionIfFALSE<T>(FuncOut<T, bool> func, in Action<T> action, out T result) => _RunActionIfTRUE((out T _result) => GetReversedBoolFunc(func, out _result), action, out result);

        private static TResult _RunFuncIfTRUE<TOut, TResult>(in FuncOut<TOut, bool> func, in Func<TOut, TResult> action, in TResult defaultValue, out TOut funcResult, out bool result)
        {
            ThrowIfNull(action, nameof(action));

            if (func(out funcResult))
            {
                result = true;

                return action(funcResult);
            }

            result = false;

            return defaultValue;
        }

        public static TResult RunFuncIfTRUE<TOut, TResult>(in FuncOut<TOut, bool> func, in Func<TOut, TResult> action, in TResult defaultValue, out TOut funcResult, out bool result) => _RunFuncIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, defaultValue, out funcResult, out result);

        public static TResult RunFuncIfFALSE<TOut, TResult>(FuncOut<TOut, bool> func, in Func<TOut, TResult> action, in TResult defaultValue, out TOut funcResult, out bool result) => _RunFuncIfTRUE((out TOut _funcResult) => GetReversedBoolFunc(func, out _funcResult), action, defaultValue, out funcResult, out result);

        private static bool _RunActionIfTRUE<TPredicateParam, TOut>(in PredicateOut<TPredicateParam, TOut> func, in Action<TOut> action, in TPredicateParam param, out TOut result)
        {
            ThrowIfNull(action, nameof(action));

            if (func(param, out result))
            {
                action(result);

                return true;
            }

            return false;
        }

        public static bool RunActionIfTRUE<TPredicateParam, TOut>(in PredicateOut<TPredicateParam, TOut> func, in Action<TOut> action, in TPredicateParam param, out TOut result) => _RunActionIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, param, out result);

        public static bool RunActionIfFALSE<TPredicateParam, TOut>(PredicateOut<TPredicateParam, TOut> func, in Action<TOut> action, in TPredicateParam param, out TOut result) => _RunActionIfTRUE((TPredicateParam _param, out TOut _result) => GetReversedBoolFunc(func, _param, out _result), action, param, out result);

        private static TFuncReturnParam _RunFuncIfTRUE<TPredicateParam, TOut, TFuncReturnParam>(in PredicateOut<TPredicateParam, TOut> func, in Func<TOut, TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out TOut predicateResult, out bool result)
        {
            ThrowIfNull(action, nameof(action));

            if (func(param, out predicateResult))
            {
                result = true;

                return action(predicateResult);
            }

            result = false;

            return defaultValue;
        }

        public static TFuncReturnParam RunFuncIfTRUE<TPredicateParam, TOut, TFuncReturnParam>(in PredicateOut<TPredicateParam, TOut> func, in Func<TOut, TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out TOut predicateResult, out bool result) => _RunFuncIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, param, defaultValue, out predicateResult, out result);

        public static TFuncReturnParam RunFuncIfFALSE<TPredicateParam, TOut, TFuncReturnParam>(PredicateOut<TPredicateParam, TOut> func, in Func<TOut, TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out TOut predicateResult, out bool result) => _RunFuncIfTRUE((TPredicateParam _param, out TOut _result) => GetReversedBoolFunc(func, _param, out _result), action, param, defaultValue, out predicateResult, out result);

        private static bool _RunActionIfTRUE<T>(in FuncOut<T, bool> func, in Action<T> action)
        {
            ThrowIfNull(action, nameof(action));

            if (func(out T result))
            {
                action(result);

                return true;
            }

            return false;
        }

        public static bool RunActionIfTRUE<T>(in FuncOut<T, bool> func, in Action<T> action) => _RunActionIfTRUE(GetOrThrowIfNull(func, nameof(func)), action);

        public static bool RunActionIfFALSE<T>(FuncOut<T, bool> func, in Action<T> action) => _RunActionIfTRUE((out T result) => GetReversedBoolFunc(func, out result), action);

        private static TResult _RunFuncIfTRUE<TOut, TResult>(in FuncOut<TOut, bool> func, in Func<TOut, TResult> action, in TResult defaultValue, out bool result)
        {
            ThrowIfNull(action, nameof(action));

            if (func(out TOut funcResult))
            {
                result = true;

                return action(funcResult);
            }

            result = false;

            return defaultValue;
        }

        public static TResult RunFuncIfTRUE<TOut, TResult>(in FuncOut<TOut, bool> func, in Func<TOut, TResult> action, in TResult defaultValue, out bool result) => _RunFuncIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, defaultValue, out result);

        public static TResult RunFuncIfFALSE<TOut, TResult>(FuncOut<TOut, bool> func, in Func<TOut, TResult> action, in TResult defaultValue, out bool result) => _RunFuncIfTRUE((out TOut funcResult) => GetReversedBoolFunc(func, out funcResult), action, defaultValue, out result);

        private static bool _RunActionIfTRUE<TPredicateParam, TOut>(in PredicateOut<TPredicateParam, TOut> func, in Action<TOut> action, in TPredicateParam param)
        {
            ThrowIfNull(action, nameof(action));

            if (func(param, out TOut result))
            {
                action(result);

                return true;
            }

            return false;
        }

        public static bool RunActionIfTRUE<TPredicateParam, TOut>(in PredicateOut<TPredicateParam, TOut> func, in Action<TOut> action, in TPredicateParam param) => _RunActionIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, param);

        public static bool RunActionIfFALSE<TPredicateParam, TOut>(PredicateOut<TPredicateParam, TOut> func, in Action<TOut> action, in TPredicateParam param) => _RunActionIfTRUE((TPredicateParam _param, out TOut _result) => GetReversedBoolFunc(func, _param, out _result), action, param);

        private static TFuncReturnParam _RunFuncIfTRUE<TPredicateParam, TOut, TFuncReturnParam>(in PredicateOut<TPredicateParam, TOut> func, in Func<TOut, TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out bool result)
        {
            ThrowIfNull(action, nameof(action));

            if (func(param, out TOut predicateResult))
            {
                result = true;

                return action(predicateResult);
            }

            result = false;

            return defaultValue;
        }

        public static TFuncReturnParam RunFuncIfTRUE<TPredicateParam, TOut, TFuncReturnParam>(in PredicateOut<TPredicateParam, TOut> func, in Func<TOut, TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out bool result) => _RunFuncIfTRUE(GetOrThrowIfNull(func, nameof(func)), action, param, defaultValue, out result);

        public static TFuncReturnParam RunFuncIfFALSE<TPredicateParam, TOut, TFuncReturnParam>(PredicateOut<TPredicateParam, TOut> func, in Func<TOut, TFuncReturnParam> action, in TPredicateParam param, in TFuncReturnParam defaultValue, out bool result) => _RunFuncIfTRUE((TPredicateParam _param, out TOut _result) => GetReversedBoolFunc(func, _param, out _result), action, param, defaultValue, out result);
    }
}
