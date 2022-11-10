using System;

using WinCopies.Util;

namespace WinCopies.ActionRunners
{
    public static class BoolChecks
    {
#region Action
#region AB1
        public static bool Run(in bool value, in Action ifTrue)
        {
            if (value)
            {
                ifTrue();

                return true;
            }

            return false;
        }

        public static bool Run(in Func<bool> value, in Action ifTrue) => Run(value(), ifTrue);

        public static bool Run(in bool value, in Func<Action> ifTrue) => Run(value, ifTrue.GetAction());

        public static bool Run(in Func<bool> value, in Func<Action> ifTrue) => Run(value(), ifTrue);
#endregion AB1

#region AB2
        public static bool Run(in bool value, in Action ifTrue, in Action ifFalse)
        {
            if (value)
            {
                ifTrue();

                return true;
            }

            ifFalse();

            return false;
        }

        public static bool Run(in Func<bool> value, in Action ifTrue, in Action ifFalse) => Run(value(), ifTrue, ifFalse);

        public static bool Run(in bool value, in Func<Action> ifTrue, in Func<Action> ifFalse) => Run(value, ifTrue.GetAction(), ifFalse.GetAction());

        public static bool Run(in Func<bool> value, in Func<Action> ifTrue, in Func<Action> ifFalse) => Run(value(), ifTrue, ifFalse);
#endregion AB2

#region ABF1
        public static bool Run(in bool value, in Func<bool> ifTrue) => value ? ifTrue() : false;

        public static bool Run(in Func<bool> value, in Func<bool> ifTrue) => Run(value(), ifTrue);

        public static bool Run(in bool value, in Func<Func<bool>> ifTrue) => Run(value, ifTrue.GetFunc());

        public static bool Run(in Func<bool> value, in Func<Func<bool>> ifTrue) => Run(value(), ifTrue);
#endregion ABF1

#region ABF2
        public static bool Run(in bool value, in Func<bool> ifTrue, in Func<bool> ifFalse) => value ? ifTrue() : ifFalse();

        public static bool Run(in Func<bool> value, in Func<bool> ifTrue, in Func<bool> ifFalse) => Run(value(), ifTrue, ifFalse);

        public static bool Run(in bool value, in Func<Func<bool>> ifTrue, in Func<Func<bool>> ifFalse) => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static bool Run(in Func<bool> value, in Func<Func<bool>> ifTrue, in Func<Func<bool>> ifFalse) => Run(value(), ifTrue, ifFalse);
#endregion ABF2
#endregion Action

#region Func
#region FB1
        public static bool Run(in bool value, in Func ifTrue, out object
#if CS8
                ?
#endif
            result)
        {
            if (value)
            {
                result = ifTrue();

                return true;
            }

            result = null;

            return false;
        }

        public static bool Run(in Func<bool> value, in Func ifTrue, out object
#if CS8
                ?
#endif
            result) => Run(value(), ifTrue, out result);

        public static bool Run(in bool value, in Func<Func> ifTrue, out object
#if CS8
                ?
#endif
            result) => Run(value, ifTrue.GetFunc(), out result);

        public static bool Run(in Func<bool> value, in Func<Func> ifTrue, out object
#if CS8
                ?
#endif
            result) => Run(value(), ifTrue, out result);
#endregion FB1

#region FB2
        public static bool Run(in bool value, in Func ifTrue, in Func ifFalse, out object result)
        {
            if (value)
            {
                result = ifTrue();

                return true;
            }

            result = ifFalse();

            return false;
        }

        public static bool Run(in Func<bool> value, in Func ifTrue, in Func ifFalse, out object result) => Run(value(), ifTrue, ifFalse, out result);

        public static bool Run(in bool value, in Func<Func> ifTrue, in Func<Func> ifFalse, out object result) => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc(), out result);

        public static bool Run(in Func<bool> value, in Func<Func> ifTrue, in Func<Func> ifFalse, out object result) => Run(value(), ifTrue, ifFalse, out result);
#endregion FB2

#region FO1
        public static object
#if CS8
                ?
#endif
            Run(in bool value, in Func ifTrue) => value ? ifTrue() : null;

        public static object
#if CS8
                ?
#endif
            Run(in Func<bool> value, in Func ifTrue) => Run(value(), ifTrue);

        public static object
#if CS8
                ?
#endif
            Run(in bool value, in Func<Func> ifTrue) => Run(value, ifTrue.GetFunc());

        public static object
#if CS8
                ?
#endif
            Run(in Func<bool> value, in Func<Func> ifTrue) => Run(value(), ifTrue);
#endregion FO1

#region FO2
        public static object Run(in bool value, in Func ifTrue, in Func ifFalse) => (value ? ifTrue : ifFalse)();

        public static object Run(in Func<bool> value, in Func ifTrue, in Func ifFalse) => Run(value(), ifTrue, ifFalse);

        public static object Run(in bool value, in Func<Func> ifTrue, in Func<Func> ifFalse) => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static object Run(in Func<bool> value, in Func<Func> ifTrue, in Func<Func> ifFalse) => Run(value(), ifTrue, ifFalse);
#endregion FO2
#endregion Func

#if CS8
#region FuncNull
#region FBN1
            public static bool Run(in bool value, in FuncNull ifTrue, out object? result)
            {
                if (value)
                {
                    result = ifTrue();

                    return true;
                }

                result = null;

                return false;
            }

            public static bool Run(in Func<bool> value, in FuncNull ifTrue, out object? result) => Run(value(), ifTrue, out result);

            public static bool Run(in bool value, in Func<FuncNull> ifTrue, out object? result) => Run(value, ifTrue.GetFuncNull(), out result);

            public static bool Run(in Func<bool> value, in Func<FuncNull> ifTrue, out object? result) => Run(value(), ifTrue, out result);
#endregion FBN1

#region FBN2
            public static bool Run(in bool value, in FuncNull ifTrue, in FuncNull ifFalse, out object? result)
            {
                if (value)
                {
                    result = ifTrue();

                    return true;
                }

                result = ifFalse();

                return false;
            }

            public static bool Run(in Func<bool> value, in FuncNull ifTrue, in FuncNull ifFalse, out object? result) => Run(value(), ifTrue, ifFalse, out result);

            public static bool Run(in bool value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse, out object? result) => Run(value, ifTrue.GetFuncNull(), ifFalse.GetFuncNull(), out result);

            public static bool Run(in Func<bool> value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse, out object? result) => Run(value(), ifTrue, ifFalse, out result);
#endregion FBN2

#region FON1
            public static object? Run(in bool value, in FuncNull ifTrue) => value ? ifTrue() : null;

            public static object? Run(in Func<bool> value, in FuncNull ifTrue) => Run(value(), ifTrue);

            public static object? Run(in bool value, in Func<FuncNull> ifTrue) => Run(value, ifTrue.GetFuncNull());

            public static object? Run(in Func<bool> value, in Func<FuncNull> ifTrue) => Run(value(), ifTrue);
#endregion FON1

#region FON2
            public static object? Run(in bool value, in FuncNull ifTrue, in FuncNull ifFalse) => (value ? ifTrue : ifFalse)();

            public static object? Run(in Func<bool> value, in FuncNull ifTrue, in FuncNull ifFalse) => Run(value(), ifTrue, ifFalse);

            public static object? Run(in bool value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse) => Run(value, ifTrue.GetFuncNull(), ifFalse.GetFuncNull());

            public static object? Run(in Func<bool> value, in Func<FuncNull> ifTrue, in Func<FuncNull> ifFalse) => Run(value(), ifTrue, ifFalse);
#endregion FON2
#endregion FuncNull
#endif

#region Func<T>
#region FBG1
        public static bool Run<T>(in bool value, in Func<T> ifTrue, out T
#if CS9
                ?
#endif
            result)
        {
            if (value)
            {
                result = ifTrue();

                return true;
            }

            result = default;

            return false;
        }

        public static bool Run<T>(in Func<bool> value, in Func<T> ifTrue, out T
#if CS9
                ?
#endif
            result) => Run(value(), ifTrue, out result);

        public static bool Run<T>(in bool value, in Func<Func<T>> ifTrue, out T
#if CS9
                ?
#endif
            result) => Run(value, ifTrue.GetFunc(), out result);

        public static bool Run<T>(in Func<bool> value, in Func<Func<T>> ifTrue, out T
#if CS9
                ?
#endif
            result) => Run(value(), ifTrue, out result);
#endregion FBG1

#region FBG2
        public static bool Run<T>(in bool value, in Func<T> ifTrue, in Func<T> ifFalse, out T result)
        {
            if (value)
            {
                result = ifTrue();

                return true;
            }

            result = ifFalse();

            return false;
        }

        public static bool Run<T>(in Func<bool> value, in Func<T> ifTrue, in Func<T> ifFalse, out T result) => Run(value(), ifTrue, ifFalse, out result);

        public static bool Run<T>(in bool value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse, out T result) => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc(), out result);

        public static bool Run<T>(in Func<bool> value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse, out T result) => Run(value(), ifTrue, ifFalse, out result);
#endregion FBG2

#region FT1
        public static T
#if CS9
                ?
#endif
            Run<T>(in bool value, in Func<T> ifTrue) => value ? ifTrue() : default;

        public static T
#if CS9
                ?
#endif
            Run<T>(in Func<bool> value, in Func<T> ifTrue) => Run(value(), ifTrue);

        public static T
#if CS9
                ?
#endif
            Run<T>(in bool value, in Func<Func<T>> ifTrue) => Run(value, ifTrue.GetFunc());

        public static T
#if CS9
                ?
#endif
            Run<T>(in Func<bool> value, in Func<Func<T>> ifTrue) => Run(value(), ifTrue);
#endregion FT1

#region FT2
        public static T Run<T>(in bool value, in Func<T> ifTrue, in Func<T> ifFalse) => (value ? ifTrue : ifFalse)();

        public static T Run<T>(in Func<bool> value, in Func<T> ifTrue, in Func<T> ifFalse) => Run(value(), ifTrue, ifFalse);

        public static T Run<T>(in bool value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse) => Run(value, ifTrue.GetFunc(), ifFalse.GetFunc());

        public static T Run<T>(in Func<bool> value, in Func<Func<T>> ifTrue, in Func<Func<T>> ifFalse) => Run(value(), ifTrue, ifFalse);
#endregion FT2
#endregion Func<T>
    }
}
