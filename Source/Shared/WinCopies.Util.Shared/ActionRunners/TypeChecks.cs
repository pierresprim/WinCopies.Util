using System;

using WinCopies.Util;

namespace WinCopies.
#if !WinCopies3
    Util.
#endif
    ActionRunners
{
    public static class TypeChecks
    {
        #region Action
        #region AB1
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Action ifTrue) => BoolChecks.Run(value is T, ifTrue);

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Action ifTrue) => Run<T>(value(), ifTrue);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Action> ifTrue) => Run<T>(value, ifTrue.GetAction());

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Action> ifTrue) => Run<T>(value(), ifTrue);
        #endregion AB1

        #region AB2
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Action ifTrue, in Action ifFalse) => BoolChecks.Run(value is T, ifTrue, ifFalse);

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Action ifTrue, in Action ifFalse) => Run<T>(value(), ifTrue, ifFalse);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Action> ifTrue, in Func<Action> ifFalse) => Run<T>(value, ifTrue.GetAction(), ifFalse.GetAction());

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Action> ifTrue, in Func<Action> ifFalse) => Run<T>(value(), ifTrue, ifFalse);
        #endregion AB2

        #region ABA1
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, Action<T> ifTrue)
        {
            if (value is T _value)
            {
                ifTrue(_value);

                return true;
            }

            return false;
        }

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Action<T> ifTrue) => Run(value(), ifTrue);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Action<T>> ifTrue) => Run(value, ifTrue.GetAction());

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Action<T>> ifTrue) => Run(value(), ifTrue);
        #endregion ABA1

        #region ABA2
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, Action<T> ifTrue, in Action ifFalse)
        {
            if (value is T _value)
            {
                ifTrue(_value);

                return true;
            }

            ifFalse();

            return false;
        }

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Action<T> ifTrue, in Action ifFalse) => Run(value(), ifTrue, ifFalse);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Action<T>> ifTrue, in Func<Action> ifFalse) => Run(value, ifTrue.GetAction(), ifFalse.GetAction());

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Action<T>> ifTrue, in Func<Action> ifFalse) => Run(value(), ifTrue, ifFalse);
        #endregion ABA2

        #region ABAI1
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, ActionIn<T> ifTrue)
        {
            if (value is T _value)
            {
                ifTrue(_value);

                return true;
            }

            return false;
        }

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in ActionIn<T> ifTrue) => Run(value(), ifTrue);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<ActionIn<T>> ifTrue) => Run(value, ifTrue.GetAction());

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<ActionIn<T>> ifTrue) => Run(value(), ifTrue);
        #endregion ABAI1

        #region ABAI2
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, ActionIn<T> ifTrue, in Action ifFalse)
        {
            if (value is T _value)
            {
                ifTrue(_value);

                return true;
            }

            ifFalse();

            return false;
        }

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in ActionIn<T> ifTrue, in Action ifFalse) => Run(value(), ifTrue, ifFalse);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<ActionIn<T>> ifTrue, in Func<Action> ifFalse) => Run(value, ifTrue.GetAction(), ifFalse.GetAction());

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<ActionIn<T>> ifTrue, in Func<Action> ifFalse) => Run(value(), ifTrue, ifFalse);
        #endregion ABAI2

        #region ABF1
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, Func<T, bool> ifTrue) => value is T _value ? ifTrue(_value) : false;

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<T, bool> ifTrue) => Run(value(), ifTrue);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Func<T, bool>> ifTrue) => Run(value, ifTrue.GetFunc());

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func<T, bool>> ifTrue) => Run(value(), ifTrue);
        #endregion ABF1

        #region ABF2
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, Func<T, bool> ifTrue, in Func<bool> ifFalse) => value is T _value ? ifTrue(_value) : ifFalse();

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<T, bool> ifTrue, in Func<bool> ifFalse) => Run(value(), ifTrue, ifFalse);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Func<T, bool>> ifTrue, in Func<Func<bool>> ifFalse) => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func<T, bool>> ifTrue, in Func<Func<bool>> ifFalse) => Run(value(), ifTrue, ifFalse);
        #endregion ABF2

        #region ABFI1
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, FuncIn<T, bool> ifTrue) => value is T _value ? ifTrue(_value) : false;

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in FuncIn<T, bool> ifTrue) => Run(value(), ifTrue);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<FuncIn<T, bool>> ifTrue) => Run(value, ifTrue.GetFunc());

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<FuncIn<T, bool>> ifTrue) => Run(value(), ifTrue);
        #endregion ABFI1

        #region ABFI2
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, FuncIn<T, bool> ifTrue, in Func<bool> ifFalse) => value is T _value ? ifTrue(_value) : ifFalse();

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in FuncIn<T, bool> ifTrue, in Func<bool> ifFalse) => Run(value(), ifTrue, ifFalse);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<FuncIn<T, bool>> ifTrue, in Func<Func<bool>> ifFalse) => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<FuncIn<T, bool>> ifTrue, in Func<Func<bool>> ifFalse) => Run(value(), ifTrue, ifFalse);
        #endregion ABFI2
        #endregion Action

        #region Func
        #region FB1
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func ifTrue, out object
#if CS8
                ?
#endif
            result) => BoolChecks.Run(value is T, ifTrue, out result);

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func ifTrue, out object
#if CS8
                ?
#endif
            result) => Run<T>(value(), ifTrue, out result);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Func> ifTrue, out object
#if CS8
                ?
#endif
            result) => Run<T>(value, ifTrue.GetFunc(), out result);

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func> ifTrue, out object
#if CS8
                ?
#endif
            result) => Run<T>(value(), ifTrue, out result);
        #endregion FB1

        #region FB2
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func ifTrue, in Func ifFalse, out object result) => BoolChecks.Run(value is T, ifTrue, ifFalse, out result);

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func ifTrue, in Func ifFalse, out object result) => Run<T>(value(), ifTrue, ifFalse, out result);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Func> ifTrue, in Func<Func> ifFalse, out object result) => Run<T>(value, ifTrue.GetFunc(), ifFalse.GetFunc(), out result);

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func> ifTrue, in Func<Func> ifFalse, out object result) => Run<T>(value(), ifTrue, ifFalse, out result);
        #endregion FB2

        #region FO1
        public static object
#if CS8
                ?
#endif
            Run<T>(in object
#if CS8
                ?
#endif
            value, in Func ifTrue) => BoolChecks.Run(value is T, ifTrue);

        public static object
#if CS8
                ?
#endif
            Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func ifTrue) => Run<T>(value(), ifTrue);

        public static object
#if CS8
                ?
#endif
            Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Func> ifTrue) => Run<T>(value, ifTrue.GetFunc());

        public static object
#if CS8
                ?
#endif
            Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func> ifTrue) => Run<T>(value(), ifTrue);
        #endregion FO1

        #region FO2
        public static object Run<T>(in object
#if CS8
                ?
#endif
            value, in Func ifTrue, in Func ifFalse) => BoolChecks.Run(value is T, ifTrue, ifFalse);

        public static object Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func ifTrue, in Func ifFalse) => Run<T>(value(), ifTrue, ifFalse);

        public static object Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Func> ifTrue, in Func<Func> ifFalse) => Run<T>(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static object Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func> ifTrue, in Func<Func> ifFalse) => Run<T>(value(), ifTrue, ifFalse);
        #endregion FO2
        #endregion Func

#if CS8
        #region FuncNull
        #region FBN1
            public static bool Run<T>(in object
#if CS8
                ?
#endif
                value, in FuncNull ifTrue, out object
#if CS8
                ?
#endif
                result) => BoolChecks.Run(value is T, ifTrue, out result);

            public static bool Run<T>(in
#if CS8
                FuncNull
#else
                Func
#endif
                value, in FuncNull ifTrue, out object
#if CS8
                ?
#endif
                result) => Run<T>(value(), ifTrue, out result);

            public static bool Run<T>(in object
#if CS8
                ?
#endif
                value, in Func<FuncNull> ifTrue, out object
#if CS8
                ?
#endif
                result) => Run<T>(value, ifTrue.GetFuncNull(), out result);

            public static bool Run<T>(in
#if CS8
                FuncNull
#else
                Func
#endif
                value, in Func<FuncNull> ifTrue, out object
#if CS8
                ?
#endif
                result) => Run<T>(value(), ifTrue, out result);
        #endregion FBN1

        #region FBN2
            public static bool Run<T>(in object
#if CS8
                ?
#endif
                value, in FuncNull ifTrue, in FuncNull ifFalse, out object
#if CS8
                ?
#endif
                result) => BoolChecks.Run(value is T, ifTrue, ifFalse, out result);

            public static bool Run<T>(in
#if CS8
                FuncNull
#else
                Func
#endif
                value, in FuncNull ifTrue, in FuncNull ifFalse, out object
#if CS8
                ?
#endif
                result) => Run<T>(value(), ifTrue, ifFalse, out result);

            public static bool Run<T>(in object
#if CS8
                ?
#endif
                value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse, out object
#if CS8
                ?
#endif
                result) => Run<T>(value, ifTrue.GetFuncNull(), ifFalse.GetFuncNull(), out result);

            public static bool Run<T>(in
#if CS8
                FuncNull
#else
                Func
#endif
                value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse, out object
#if CS8
                ?
#endif
                result) => Run<T>(value(), ifTrue, ifFalse, out result);
        #endregion FBN2

        #region FON1
            public static object
#if CS8
                ?
#endif
                Run<T>(in object
#if CS8
                ?
#endif
                value, in FuncNull ifTrue) => BoolChecks.Run(value is T, ifTrue);

            public static object
#if CS8
                ?
#endif
                Run<T>(in
#if CS8
                FuncNull
#else
                Func
#endif
                value, in FuncNull ifTrue) => Run<T>(value(), ifTrue);

            public static object
#if CS8
                ?
#endif
                Run<T>(in object
#if CS8
                ?
#endif
                value, in Func<FuncNull> ifTrue) => Run<T>(value, ifTrue.GetFuncNull());

            public static object
#if CS8
                ?
#endif
                Run<T>(in
#if CS8
                FuncNull
#else
                Func
#endif
                value, in Func<FuncNull> ifTrue) => Run<T>(value(), ifTrue);
        #endregion FON1

        #region FON2
            public static object
#if CS8
                ?
#endif
                Run<T>(in object
#if CS8
                ?
#endif
                value, in FuncNull ifTrue, in FuncNull ifFalse) => BoolChecks.Run(value is T, ifTrue, ifFalse);

            public static object
#if CS8
                ?
#endif
                Run<T>(in
#if CS8
                FuncNull
#else
                Func
#endif
                value, in FuncNull ifTrue, in FuncNull ifFalse) => Run<T>(value(), ifTrue, ifFalse);

            public static object
#if CS8
                ?
#endif
                Run<T>(in object
#if CS8
                ?
#endif
                value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse) => Run<T>(value, ifTrue.GetFuncNull(), ifFalse.GetFuncNull());

            public static object
#if CS8
                ?
#endif
                Run<T>(in
#if CS8
                FuncNull
#else
                Func
#endif
                value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse) => Run<T>(value(), ifTrue, ifFalse);
        #endregion FON2
        #endregion FuncNull
#endif

        #region Generic Func
        #region FBG1.1
        public static bool Run<TIn, TOut>(in object
#if CS8
                ?
#endif
            value, in Func<TOut> ifTrue, out TOut
#if CS9
                ?
#endif
            result) => BoolChecks.Run(value is TIn, ifTrue, out result);

        public static bool Run<TIn, TOut>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<TOut> ifTrue, out TOut
#if CS9
                ?
#endif
            result) => Run(value(), ifTrue, out result);

        public static bool Run<TIn, TOut>(in object
#if CS8
                ?
#endif
            value, in Func<Func<TOut>> ifTrue, out TOut
#if CS9
                ?
#endif
            result) => Run(value, ifTrue.GetFunc(), out result);

        public static bool Run<TIn, TOut>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func<TOut>> ifTrue, out TOut
#if CS9
                ?
#endif
            result) => Run(value(), ifTrue, out result);
        #endregion FBG1.1

        #region FBG1.2
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<T> ifTrue, out T
#if CS9
                ?
#endif
            result) => BoolChecks.Run(value is T, ifTrue, out result);

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<T> ifTrue, out T
#if CS9
                ?
#endif
            result) => Run(value(), ifTrue, out result);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Func<T>> ifTrue, out T
#if CS9
                ?
#endif
            result) => Run(value, ifTrue.GetFunc(), out result);

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func<T>> ifTrue, out T
#if CS9
                ?
#endif
            result) => Run(value(), ifTrue, out result);
        #endregion FBG1.2

        #region FBG2.1
        public static bool Run<TIn, TOut>(in object
#if CS8
                ?
#endif
            value, in Func<TOut> ifTrue, in Func<TOut> ifFalse, out TOut
#if CS9
                ?
#endif
            result) => BoolChecks.Run(value is TIn, ifTrue, ifFalse, out result);

        public static bool Run<TIn, TOut>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<TOut> ifTrue, in Func<TOut> ifFalse, out TOut
#if CS9
                ?
#endif
            result) => Run(value(), ifTrue, ifFalse, out result);

        public static bool Run<TIn, TOut>(in object
#if CS8
                ?
#endif
            value, in Func<Func<TOut>> ifTrue, in Func<Func<TOut>> ifFalse, out TOut
#if CS9
                ?
#endif
            result) => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc(), out result);

        public static bool Run<TIn, TOut>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func<TOut>> ifTrue, in Func<Func<TOut>> ifFalse, out TOut
#if CS9
                ?
#endif
            result) => Run(value(), ifTrue, ifFalse, out result);
        #endregion FBG2.1

        #region FBG2.2
        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<T> ifTrue, in Func<T> ifFalse, out T
#if CS9
                ?
#endif
            result) => Run<T, T>(value, ifTrue, ifFalse, out result);

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<T> ifTrue, in Func<T> ifFalse, out T
#if CS9
                ?
#endif
            result) => Run(value(), ifTrue, ifFalse, out result);

        public static bool Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse, out T
#if CS9
                ?
#endif
            result) => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc(), out result);

        public static bool Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse, out T
#if CS9
                ?
#endif
            result) => Run(value(), ifTrue, ifFalse, out result);
        #endregion FBG2.2

        #region FT1.1
        public static TOut
#if CS9
                ?
#endif
            Run<TIn, TOut>(in object
#if CS8
                ?
#endif
            value, in Func<TOut> ifTrue) => BoolChecks.Run(value is TIn, ifTrue);

        public static TOut
#if CS9
                ?
#endif
            Run<TIn, TOut>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<TOut> ifTrue) => Run<TIn, TOut>(value(), ifTrue);

        public static TOut
#if CS9
                ?
#endif
            Run<TIn, TOut>(in object
#if CS8
                ?
#endif
            value, in Func<Func<TOut>> ifTrue) => Run<TIn, TOut>(value, ifTrue.GetFunc());

        public static TOut
#if CS9
                ?
#endif
            Run<TIn, TOut>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func<TOut>> ifTrue) => Run<TIn, TOut>(value(), ifTrue);
        #endregion FT1.1

        #region FT1.2
        public static T
#if CS9
                ?
#endif
            Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<T> ifTrue) => Run(value, ifTrue, out T
#if CS9
                ?
#endif
            result) ? result : default;

        public static T
#if CS9
                ?
#endif
            Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<T> ifTrue) => Run(value(), ifTrue);

        public static T
#if CS9
                ?
#endif
            Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Func<T>> ifTrue) => Run(value, ifTrue.GetFunc());

        public static T
#if CS9
                ?
#endif
            Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func<T>> ifTrue) => Run(value(), ifTrue);
        #endregion FT1.2

        #region FT2.1
        public static TOut Run<TIn, TOut>(in object
#if CS8
                ?
#endif
            value, in Func<TOut> ifTrue, in Func<TOut> ifFalse) => BoolChecks.Run(value is TIn, ifTrue, ifFalse);

        public static TOut Run<TIn, TOut>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<TOut> ifTrue, in Func<TOut> ifFalse) => Run<TIn, TOut>(value(), ifTrue, ifFalse);

        public static TOut Run<TIn, TOut>(in object
#if CS8
                ?
#endif
            value, in Func<Func<TOut>> ifTrue, in Func<Func<TOut>> ifFalse) => Run<TIn, TOut>(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static TOut Run<TIn, TOut>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func<TOut>> ifTrue, in Func<Func<TOut>> ifFalse) => Run<TIn, TOut>(value(), ifTrue, ifFalse);
        #endregion FT2.1

        #region FT2.2
        public static T
#if CS9
                ?
#endif
            Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<T> ifTrue, in Func<T> ifFalse) => Run<T, T>(value, ifTrue, ifFalse);

        public static T
#if CS9
                ?
#endif
            Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<T> ifTrue, in Func<T> ifFalse) => Run(value(), ifTrue, ifFalse);

        public static T
#if CS9
                ?
#endif
            Run<T>(in object
#if CS8
                ?
#endif
            value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse) => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static T
#if CS9
                ?
#endif
            Run<T>(in
#if CS8
                FuncNull
#else
            Func
#endif
            value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse) => Run(value(), ifTrue, ifFalse);
        #endregion FT2.2
        #endregion Generic Func
    }
}
