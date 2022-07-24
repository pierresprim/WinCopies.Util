using System;

using WinCopies.Util;

namespace WinCopies.
#if !WinCopies3
    Util.
#endif
    ActionRunners
{
    public static class ValueChecks
    {
        #region Action
        #region AB1
        public static bool Run<T>(in T? value, in Action ifTrue) where T : struct => BoolChecks.Run(value.HasValue, ifTrue);

        public static bool Run<T>(in Func<T?> value, in Action ifTrue) where T : struct => Run(value(), ifTrue);

        public static bool Run<T>(in T? value, in Func<Action> ifTrue) where T : struct => Run(value, ifTrue.GetAction());

        public static bool Run<T>(in Func<T?> value, in Func<Action> ifTrue) where T : struct => Run(value(), ifTrue);
        #endregion AB1

        #region AB2
        public static bool Run<T>(in T? value, in Action ifTrue, in Action ifFalse) where T : struct => BoolChecks.Run(value.HasValue, ifTrue, ifFalse);

        public static bool Run<T>(in Func<T?> value, in Action ifTrue, in Action ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);

        public static bool Run<T>(in T? value, in Func<Action> ifTrue, in Func<Action> ifFalse) where T : struct => Run(value, ifTrue.GetAction(), ifFalse.GetAction());

        public static bool Run<T>(in Func<T?> value, in Func<Action> ifTrue, in Func<Action> ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);
        #endregion AB2

        #region ABA1
        public static bool Run<T>(T? value, Action<T> ifTrue) where T : struct => BoolChecks.Run(value.HasValue, () => ifTrue(value.Value));

        public static bool Run<T>(in Func<T?> value, in Action<T> ifTrue) where T : struct => Run(value(), ifTrue);

        public static bool Run<T>(in T? value, in Func<Action<T>> ifTrue) where T : struct => Run(value, ifTrue.GetAction());

        public static bool Run<T>(in Func<T?> value, in Func<Action<T>> ifTrue) where T : struct => Run(value(), ifTrue);
        #endregion ABA1

        #region ABA2
        public static bool Run<T>(T? value, Action<T> ifTrue, in Action ifFalse) where T : struct => BoolChecks.Run(value.HasValue, () => ifTrue(value.Value), ifFalse);

        public static bool Run<T>(in Func<T?> value, in Action<T> ifTrue, in Action ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);

        public static bool Run<T>(in T? value, in Func<Action<T>> ifTrue, in Func<Action> ifFalse) where T : struct => Run(value, ifTrue.GetAction(), ifFalse.GetAction());

        public static bool Run<T>(in Func<T?> value, in Func<Action<T>> ifTrue, in Func<Action> ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);
        #endregion ABA2

        #region ABAI1
        public static bool Run<T>(T? value, ActionIn<T> ifTrue) where T : struct => BoolChecks.Run(value.HasValue, () => ifTrue(value.Value));

        public static bool Run<T>(in Func<T?> value, in ActionIn<T> ifTrue) where T : struct => Run(value(), ifTrue);

        public static bool Run<T>(in T? value, in Func<ActionIn<T>> ifTrue) where T : struct => Run(value, ifTrue.GetAction());

        public static bool Run<T>(in Func<T?> value, in Func<ActionIn<T>> ifTrue) where T : struct => Run(value(), ifTrue);
        #endregion ABAI1

        #region ABAI2
        public static bool Run<T>(T? value, ActionIn<T> ifTrue, in Action ifFalse) where T : struct => BoolChecks.Run(value.HasValue, () => ifTrue(value.Value), ifFalse);

        public static bool Run<T>(in Func<T?> value, in ActionIn<T> ifTrue, in Action ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);

        public static bool Run<T>(in T? value, in Func<ActionIn<T>> ifTrue, in Func<Action> ifFalse) where T : struct => Run(value, ifTrue.GetAction(), ifFalse.GetAction());

        public static bool Run<T>(in Func<T?> value, in Func<ActionIn<T>> ifTrue, in Func<Action> ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);
        #endregion ABAI2

        #region ABF1
        public static bool Run<T>(T? value, Func<T, bool> ifTrue) where T : struct => BoolChecks.Run(value.HasValue, () => ifTrue(value.Value));

        public static bool Run<T>(in Func<T?> value, in Func<T, bool> ifTrue) where T : struct => Run(value(), ifTrue);

        public static bool Run<T>(in T? value, in Func<Func<T, bool>> ifTrue) where T : struct => Run(value, ifTrue.GetFunc());

        public static bool Run<T>(in Func<T?> value, in Func<Func<T, bool>> ifTrue) where T : struct => Run(value(), ifTrue);
        #endregion ABF1

        #region ABF2
        public static bool Run<T>(T? value, Func<T, bool> ifTrue, in Func<bool> ifFalse) where T : struct => BoolChecks.Run(value.HasValue, () => ifTrue(value.Value), ifFalse);

        public static bool Run<T>(in Func<T?> value, in Func<T, bool> ifTrue, in Func<bool> ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);

        public static bool Run<T>(in T? value, in Func<Func<T, bool>> ifTrue, in Func<Func<bool>> ifFalse) where T : struct => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static bool Run<T>(in Func<T?> value, in Func<Func<T, bool>> ifTrue, in Func<Func<bool>> ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);
        #endregion ABF2

        #region ABFI1
        public static bool Run<T>(T? value, FuncIn<T, bool> ifTrue) where T : struct => BoolChecks.Run(value.HasValue, () => ifTrue(value.Value));

        public static bool Run<T>(in Func<T?> value, in FuncIn<T, bool> ifTrue) where T : struct => Run(value(), ifTrue);

        public static bool Run<T>(in T? value, in Func<FuncIn<T, bool>> ifTrue) where T : struct => Run(value, ifTrue.GetFunc());

        public static bool Run<T>(in Func<T?> value, in Func<FuncIn<T, bool>> ifTrue) where T : struct => Run(value(), ifTrue);
        #endregion ABFI1

        #region ABFI2
        public static bool Run<T>(T? value, FuncIn<T, bool> ifTrue, in Func<bool> ifFalse) where T : struct => BoolChecks.Run(value.HasValue, () => ifTrue(value.Value), ifFalse);

        public static bool Run<T>(in Func<T?> value, in FuncIn<T, bool> ifTrue, in Func<bool> ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);

        public static bool Run<T>(in T? value, in Func<FuncIn<T, bool>> ifTrue, in Func<Func<bool>> ifFalse) where T : struct => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static bool Run<T>(in Func<T?> value, in Func<FuncIn<T, bool>> ifTrue, in Func<Func<bool>> ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);
        #endregion ABFI2
        #endregion Action

        #region Func
        #region FB1
        public static bool Run<T>(in T? value, in Func ifTrue, out object
#if CS8
                ?
#endif
            result) where T : struct => BoolChecks.Run(value.HasValue, ifTrue, out result);

        public static bool Run<T>(in Func<T?> value, in Func ifTrue, out object
#if CS8
                ?
#endif
            result) where T : struct => Run(value(), ifTrue, out result);

        public static bool Run<T>(in T? value, in Func<Func> ifTrue, out object
#if CS8
                ?
#endif
            result) where T : struct => Run(value, ifTrue.GetFunc(), out result);

        public static bool Run<T>(in Func<T?> value, in Func<Func> ifTrue, out object
#if CS8
                ?
#endif
            result) where T : struct => Run(value(), ifTrue, out result);
        #endregion FB1

        #region FB2
        public static bool Run<T>(in T? value, in Func ifTrue, in Func ifFalse, out object result) where T : struct => BoolChecks.Run(value.HasValue, ifTrue, ifFalse, out result);

        public static bool Run<T>(in Func<T?> value, in Func ifTrue, in Func ifFalse, out object result) where T : struct => Run(value(), ifTrue, ifFalse, out result);

        public static bool Run<T>(in T? value, in Func<Func> ifTrue, in Func<Func> ifFalse, out object result) where T : struct => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc(), out result);

        public static bool Run<T>(in Func<T?> value, in Func<Func> ifTrue, in Func<Func> ifFalse, out object result) where T : struct => Run(value(), ifTrue, ifFalse, out result);
        #endregion FB2

        #region FO1
        public static object
#if CS8
                ?
#endif
            Run<T>(in T? value, in Func ifTrue) where T : struct => BoolChecks.Run(value.HasValue, ifTrue);

        public static object
#if CS8
                ?
#endif
            Run<T>(in Func<T?> value, in Func ifTrue) where T : struct => Run(value(), ifTrue);

        public static object
#if CS8
                ?
#endif
            Run<T>(in T? value, in Func<Func> ifTrue) where T : struct => Run(value, ifTrue.GetFunc());

        public static object
#if CS8
                ?
#endif
            Run<T>(in Func<T?> value, in Func<Func> ifTrue) where T : struct => Run(value(), ifTrue);
        #endregion FO1

        #region FO2
        public static object Run<T>(in T? value, in Func ifTrue, in Func ifFalse) where T : struct => BoolChecks.Run(value.HasValue, ifTrue, ifFalse);

        public static object Run<T>(in Func<T?> value, in Func ifTrue, in Func ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);

        public static object Run<T>(in T? value, in Func<Func> ifTrue, in Func<Func> ifFalse) where T : struct => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static object Run<T>(in Func<T?> value, in Func<Func> ifTrue, in Func<Func> ifFalse) where T : struct => Run(value(), ifTrue, ifFalse);
        #endregion FO2
        #endregion Func

#if CS8
        #region FuncNull
        #region FBN1
            public static bool Run<T>(in T? value, in FuncNull ifTrue, out object? result) where T : struct => BoolChecks.Run(value.HasValue, ifTrue, out result);

            public static bool Run<T>(in Func<T?> value, in FuncNull ifTrue, out object? result) where T : struct => Run<T>(value(), ifTrue, out result);

            public static bool Run<T>(in T? value, in Func<FuncNull> ifTrue, out object? result) where T : struct => Run<T>(value, ifTrue.GetFuncNull(), out result);

            public static bool Run<T>(in Func<T?> value, in Func<FuncNull> ifTrue, out object? result) where T : struct => Run<T>(value(), ifTrue, out result);
        #endregion FBN1

        #region FBN2
            public static bool Run<T>(in T? value, in FuncNull ifTrue, in FuncNull ifFalse, out object? result) where T : struct => BoolChecks.Run(value.HasValue, ifTrue, ifFalse, out result);

            public static bool Run<T>(in Func<T?> value, in FuncNull ifTrue, in FuncNull ifFalse, out object? result) where T : struct => Run<T>(value(), ifTrue, ifFalse, out result);

            public static bool Run<T>(in T? value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse, out object? result) where T : struct => Run<T>(value, ifTrue.GetFuncNull(), ifFalse.GetFuncNull(), out result);

            public static bool Run<T>(in Func<T?> value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse, out object? result) where T : struct => Run<T>(value(), ifTrue, ifFalse, out result);
        #endregion FBN2

        #region FON1
            public static object? Run<T>(in T? value, in FuncNull ifTrue) where T : struct => BoolChecks.Run(value.HasValue, ifTrue);

            public static object? Run<T>(in Func<T?> value, in FuncNull ifTrue) where T : struct => Run<T>(value(), ifTrue);

            public static object? Run<T>(in T? value, in Func<FuncNull> ifTrue) where T : struct => Run<T>(value, ifTrue.GetFuncNull());

            public static object? Run<T>(in Func<T?> value, in Func<FuncNull> ifTrue) where T : struct => Run<T>(value(), ifTrue);
        #endregion FON1

        #region FON2
            public static object? Run<T>(in T? value, in FuncNull ifTrue, in FuncNull ifFalse) where T : struct => BoolChecks.Run(value.HasValue, ifTrue, ifFalse);

            public static object? Run<T>(in Func<T?> value, in FuncNull ifTrue, in FuncNull ifFalse) where T : struct => Run<T>(value(), ifTrue, ifFalse);

            public static object? Run<T>(in T? value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse) where T : struct => Run<T>(value, ifTrue.GetFuncNull(), ifFalse.GetFuncNull());

            public static object? Run<T>(in Func<T?> value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse) where T : struct => Run<T>(value(), ifTrue, ifFalse);
        #endregion FON2
        #endregion FuncNull
#endif

        #region Generic Func
        #region FBG1.1
        public static bool Run<TIn, TOut>(in TIn? value, in Func<TOut> ifTrue, out TOut
#if CS9
                ?
#endif
            result) where TIn : struct => BoolChecks.Run(value.HasValue, ifTrue, out result);

        public static bool Run<TIn, TOut>(in Func<TIn?> value, in Func<TOut> ifTrue, out TOut
#if CS9
                ?
#endif
            result) where TIn : struct => Run(value(), ifTrue, out result);

        public static bool Run<TIn, TOut>(in TIn? value, in Func<Func<TOut>> ifTrue, out TOut
#if CS9
                ?
#endif
            result) where TIn : struct => Run(value, ifTrue.GetFunc(), out result);

        public static bool Run<TIn, TOut>(in Func<TIn?> value, in Func<Func<TOut>> ifTrue, out TOut
#if CS9
                ?
#endif
            result) where TIn : struct => Run(value(), ifTrue, out result);
        #endregion FBG1.1

        #region FBG1.2.1
        public static bool Run<T>(in T? value, in Func<T> ifTrue, out T result) where T : struct => Run<T, T>(value, ifTrue, out result);

        public static bool Run<T>(in Func<T?> value, in Func<T> ifTrue, out T result) where T : struct => Run<T>(value(), ifTrue, out result);

        public static bool Run<T>(in T? value, in Func<Func<T>> ifTrue, out T result) where T : struct => Run<T>(value, ifTrue.GetFunc(), out result);

        public static bool Run<T>(in Func<T?> value, in Func<Func<T>> ifTrue, out T result) where T : struct => Run<T>(value(), ifTrue, out result);
        #endregion FBG1.2.1

        #region FBG1.2.2
        public static bool Run<T>(in T? value, in Func<T> ifTrue, out T? result) where T : struct
        {
            if (Run<T, T>(value, ifTrue, out T _result))
            {
                result = _result;

                return true;
            }

            result = null;

            return false;
        }

        public static bool Run<T>(in Func<T?> value, in Func<T> ifTrue, out T? result) where T : struct => Run(value(), ifTrue, out result);

        public static bool Run<T>(in T? value, in Func<Func<T>> ifTrue, out T? result) where T : struct => Run(value, ifTrue.GetFunc(), out result);

        public static bool Run<T>(in Func<T?> value, in Func<Func<T>> ifTrue, out T? result) where T : struct => Run(value(), ifTrue, out result);
        #endregion FBG1.2.2

        #region FBG2.1
        public static bool Run<TIn, TOut>(in TIn? value, in Func<TOut> ifTrue, in Func<TOut> ifFalse, out TOut result) where TIn : struct => BoolChecks.Run(value.HasValue, ifTrue, ifFalse, out result);

        public static bool Run<TIn, TOut>(in Func<TIn?> value, in Func<TOut> ifTrue, in Func<TOut> ifFalse, out TOut result) where TIn : struct => Run(value(), ifTrue, ifFalse, out result);

        public static bool Run<TIn, TOut>(in TIn? value, in Func<Func<TOut>> ifTrue, in Func<Func<TOut>> ifFalse, out TOut result) where TIn : struct => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc(), out result);

        public static bool Run<TIn, TOut>(in Func<TIn?> value, in Func<Func<TOut>> ifTrue, in Func<Func<TOut>> ifFalse, out TOut result) where TIn : struct => Run(value(), ifTrue, ifFalse, out result);
        #endregion FBG2.1

        #region FBG2.2.1
        public static bool Run<T>(in T? value, in Func<T> ifTrue, in Func<T> ifFalse, out T result) where T : struct => Run<T, T>(value, ifTrue, ifFalse, out result);

        public static bool Run<T>(in Func<T?> value, in Func<T> ifTrue, in Func<T> ifFalse, out T result) where T : struct => Run<T>(value(), ifTrue, ifFalse, out result);

        public static bool Run<T>(in T? value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse, out T result) where T : struct => Run<T>(value, ifTrue.GetFunc(), ifFalse.GetFunc(), out result);

        public static bool Run<T>(in Func<T?> value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse, out T result) where T : struct => Run<T>(value(), ifTrue, ifFalse, out result);
        #endregion FBG2.2.1

        #region FBG2.2.2
        public static bool Run<T>(in T? value, in Func<T> ifTrue, in Func<T> ifFalse, out T? result) where T : struct
        {
            if (Run<T, T>(value, ifTrue, ifFalse, out T _result))
            {
                result = _result;

                return true;
            }

            result = null;

            return false;
        }

        public static bool Run<T>(in Func<T?> value, in Func<T> ifTrue, in Func<T> ifFalse, out T? result) where T : struct => Run(value(), ifTrue, ifFalse, out result);

        public static bool Run<T>(in T? value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse, out T? result) where T : struct => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc(), out result);

        public static bool Run<T>(in Func<T?> value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse, out T? result) where T : struct => Run(value(), ifTrue, ifFalse, out result);
        #endregion FBG2.2.2

        #region FT1.1
        public static TOut
#if CS9
                ?
#endif
            Run<TIn, TOut>(in TIn? value, in Func<TOut> ifTrue) where TIn : struct => BoolChecks.Run(value.HasValue, ifTrue);

        public static TOut
#if CS9
                ?
#endif
            Run<TIn, TOut>(in Func<TIn?> value, in Func<TOut> ifTrue) where TIn : struct => Run(value(), ifTrue);

        public static TOut
#if CS9
                ?
#endif
            Run<TIn, TOut>(in TIn? value, in Func<Func<TOut>> ifTrue) where TIn : struct => Run(value, ifTrue.GetFunc());

        public static TOut
#if CS9
                ?
#endif
            Run<TIn, TOut>(in Func<TIn?> value, in Func<Func<TOut>> ifTrue) where TIn : struct => Run(value(), ifTrue);
        #endregion FT1.1

        #region FT1.2.1
        public static T Run<T>(in T? value, in Func<T> ifTrue) where T : struct => Run<T, T>(value, ifTrue);

        public static T Run<T>(in Func<T?> value, in Func<T> ifTrue) where T : struct => Run<T>(value(), ifTrue);

        public static T Run<T>(in T? value, in Func<Func<T>> ifTrue) where T : struct => Run<T>(value, ifTrue.GetFunc());

        public static T Run<T>(in Func<T?> value, in Func<Func<T>> ifTrue) where T : struct => Run<T>(value(), ifTrue);
        #endregion FT1.2.1

        #region FT1.2.2
        public static T? RunNull<T>(in T? value, in Func<T> ifTrue) where T : struct => Run(value, ifTrue, out T? result) ? result : null;

        public static T? RunNull<T>(in Func<T?> value, in Func<T> ifTrue) where T : struct => RunNull(value(), ifTrue);

        public static T? RunNull<T>(in T? value, in Func<Func<T>> ifTrue) where T : struct => RunNull(value, ifTrue.GetFunc());

        public static T? RunNull<T>(in Func<T?> value, in Func<Func<T>> ifTrue) where T : struct => RunNull(value(), ifTrue);
        #endregion FT1.2.2

        #region FT2.1
        public static TOut Run<TIn, TOut>(in TIn? value, in Func<TOut> ifTrue, in Func<TOut> ifFalse) where TIn : struct => BoolChecks.Run(value.HasValue, ifTrue, ifFalse);

        public static TOut Run<TIn, TOut>(in Func<TIn?> value, in Func<TOut> ifTrue, in Func<TOut> ifFalse) where TIn : struct => Run(value(), ifTrue, ifFalse);

        public static TOut Run<TIn, TOut>(in TIn? value, in Func<Func<TOut>> ifTrue, in Func<Func<TOut>> ifFalse) where TIn : struct => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static TOut Run<TIn, TOut>(in Func<TIn?> value, in Func<Func<TOut>> ifTrue, in Func<Func<TOut>> ifFalse) where TIn : struct => Run(value(), ifTrue, ifFalse);
        #endregion FT2.1

        #region FT2.2.1
        public static T Run<T>(in T? value, in Func<T> ifTrue, in Func<T> ifFalse) where T : struct => Run<T, T>(value, ifTrue, ifFalse);

        public static T Run<T>(in Func<T?> value, in Func<T> ifTrue, in Func<T> ifFalse) where T : struct => Run<T>(value(), ifTrue, ifFalse);

        public static T Run<T>(in T? value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse) where T : struct => Run<T>(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static T Run<T>(in Func<T?> value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse) where T : struct => Run<T>(value(), ifTrue, ifFalse);
        #endregion FT2.2.1

        #region FT2.2.2
        public static T? RunNull<T>(in T? value, in Func<T> ifTrue, in Func<T> ifFalse) where T : struct => Run<T, T>(value, ifTrue, ifFalse);

        public static T? RunNull<T>(in Func<T?> value, in Func<T> ifTrue, in Func<T> ifFalse) where T : struct => RunNull(value(), ifTrue, ifFalse);

        public static T? RunNull<T>(in T? value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse) where T : struct => RunNull(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static T? RunNull<T>(in Func<T?> value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse) where T : struct => RunNull(value(), ifTrue, ifFalse);
        #endregion FT2.2.2
        #endregion Generic Func
    }
}
