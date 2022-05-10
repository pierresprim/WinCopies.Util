using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

#if !WinCopies3
using WinCopies.Util;
#endif

namespace WinCopies.Diagnostics
{
    public static class Determine
    {
        private static bool CheckItems(in object[] items, out int index, in PredicateIn predicate, in bool @default = false)
        {
            for (int i = 0; i < items.Length; i++)

                if (predicate(items[i]))
                {
                    index = i;

                    return @default;
                }

            index = -1;

            return !@default;
        }

        private static bool CheckNullity(in object[] items, out int index, in bool mustBeNull, in bool @default = false) => CheckItems(items, out index, mustBeNull ?
#if !CS9
                (PredicateIn)(
#endif
                (in object obj) => obj != null
#if !CS9
                )
#endif
                : (in object _obj) => _obj == null, @default);

        private static bool CheckType(Type t, in object[] items, out int index, in bool mustBeOfType, in bool @default = false)
        {
            bool _predicate(in Type _t) => t.IsAssignableFrom(_t);

#if CS8
            static
#endif
            bool predicate(in object obj, in PredicateIn<Type> __predicate) => obj != null && __predicate(obj.GetType());

            return CheckItems(items, out index, mustBeOfType ?
#if !CS9
                (PredicateIn)(
#endif
                (in object obj) => predicate(obj, (in Type _t) => !_predicate(_t))
#if !CS9
                )
#endif
                : (in object _obj) => predicate(_obj, _predicate), @default);
        }

        private static bool CheckType<T>(in object[] items, out int index, in bool mustBeOfType, in bool @default = false) => CheckItems(items, out index, mustBeOfType ?
#if !CS9
                (PredicateIn)(
#endif
                (in object obj) =>
#if CS9
                obj is not T
#else
                !(obj is T)
#endif
#if !CS9
                )
#endif
                : (in object _obj) => _obj is T, @default);



        public static bool AreNull(out int index, params object[] items) => CheckNullity(items, out index, true);
        public static bool AreNull(params object[] items) => AreNull(out _, items);

        public static bool AreOfType(in Type t, out int index, params object[] items) => CheckType(t, items, out index, true);
        public static bool AreOfType(in Type t, params object[] items) => AreOfType(t, out _, items, true);
        public static bool AreOfType<T>(out int index, params object[] items) => CheckType<T>(items, out index, true);
        public static bool AreOfType<T>(params object[] items) => AreOfType<T>(out _, items, true);



        public static bool AreNotNull(out int index, params object[] items) => CheckNullity(items, out index, false);
        public static bool AreNotNull(params object[] items) => AreNotNull(out _, items);

        public static bool AreNotOfType(in Type t, out int index, params object[] items) => CheckType(t, items, out index, false);
        public static bool AreNotOfType(in Type t, params object[] items) => AreNotOfType(t, out _, items);
        public static bool AreNotOfType<T>(out int index, params object[] items) => CheckType<T>(items, out index, false);
        public static bool AreNotOfType<T>(params object[] items) => AreNotOfType<T>(out _, items);



        public static bool OneOrMoreNull(out int index, params object[] items) => CheckNullity(items, out index, true, true);
        public static bool OneOrMoreNull(params object[] items) => OneOrMoreNull(out _, items);

        public static bool OneOrMoreOfType(in Type t, out int index, params object[] items) => CheckType(t, items, out index, true, true);
        public static bool OneOrMoreOfType(in Type t, params object[] items) => OneOrMoreOfType(t, out _, items);
        public static bool OneOrMoreOfType<T>(out int index, params object[] items) => CheckType<T>(items, out index, true, true);
        public static bool OneOrMoreOfType<T>(params object[] items) => OneOrMoreOfType<T>(out _, items);
    }

    public struct AssertExceptionInfo
    {
        public string Method { get; }

        public string FileName { get; }

        public int LineNumber { get; }

        public int ColumnNumber { get; }

        public AssertExceptionInfo(in MethodBase method, in string fileName, in int lineNumber, in int columnNumber)
        {
            var stringBuilder = new StringBuilder();

            void append(in string text) => stringBuilder.Append(text);
            void appendChar(in char c) => stringBuilder.Append(c);

            append(method.Name);

            if (method.IsGenericMethod)
            {
                appendChar('<');

                ActionIn<string> action = (in string type) =>
                   {
                       append(type);

                       action = (in string _type) =>
                         {
                             append(", ");

                             append(_type);
                         };
                   };

                foreach (Type genericParam in method.GetGenericArguments())

                    action(genericParam.FullName);

                appendChar('>');
            }

            appendChar('(');

            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length > 0)
            {
                void appendParam(in ParameterInfo parameter)
                {
                    append(parameter.ParameterType.FullName);
                    appendChar(' ');
                    append(parameter.Name);
                }

                ActionIn<ParameterInfo> _action = (in ParameterInfo type) =>
                {
                    appendParam(type);

                    _action = (in ParameterInfo _type) =>
                    {
                        append(", ");

                        appendParam(_type);
                    };
                };

                foreach (ParameterInfo param in parameters)

                    _action(param);
            }

            append(");");

            Method = stringBuilder.ToString();

            FileName = fileName;

            LineNumber = lineNumber;

            ColumnNumber = columnNumber;
        }
    }

    public class AssertException : Exception
    {
        public AssertExceptionInfo AssertExceptionInfo { get; }

        public AssertException(in string errorMessage, in byte offset) : base(errorMessage)
        {
            var stackFrame = new StackTrace().GetFrame(1 + offset);

            AssertExceptionInfo = new AssertExceptionInfo(stackFrame.GetMethod(), stackFrame.GetFileName(), stackFrame.GetFileLineNumber(), stackFrame.GetFileColumnNumber());
        }
    }

    public class MultiAssertException : AssertException
    {
        public int Index { get; }

        public MultiAssertException(in string errorMessage, in int index, in byte offset) : base(errorMessage, offset) => Index = index;

        public MultiAssertException(in int index, in byte offset) : this($"The item at index {index} does not match the required condition(s).", index, offset) { /* Left empty. */ }
    }

    public static class Assert
    {
        private static void AreNull(in bool increment, in string errorMessage, params object[] items)
        {
            if (!Determine.AreNull(out int index, items))
            {
                byte offset = 3;

                if (increment)

                    offset++;

                throw string.IsNullOrEmpty(errorMessage) ? new MultiAssertException(index, offset) : new MultiAssertException(errorMessage, index, offset);
            }
        }
        public static void AreNull(in string errorMessage, params object[] items) => AreNull(false, errorMessage, items);
        public static void AreNull(params object[] items) => AreNull(true, null, items);
    }
}
