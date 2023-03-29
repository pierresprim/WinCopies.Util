using System;
using System.Diagnostics.CodeAnalysis;

using WinCopies.Util;

using static WinCopies.ThrowHelper;

namespace WinCopies
{
    public static class Convert
    {
#if CS8
        private const string VALUE = "value";
#endif

        public static bool TryChangeType(in object
#if CS8
            ?
#endif
            value, Type type,
#if CS8
            [NotNullIfNotNull(VALUE)]
#endif
            out object
#if CS8
            ?
#endif
            result)
        {
            if (value is IConvertible && (type = System.Nullable.GetUnderlyingType(type) ?? type).IsAssignableTo<IConvertible>())
            {
                result = System.Convert.ChangeType(value, type);

                return true;
            }

            result = value;

            return false;
        }

#if CS8
        [return: NotNullIfNotNull(nameof(VALUE))]
#endif
        public static object
#if CS8
            ?
#endif
            TryChangeType(in object
#if CS8
            ?
#endif
            value, in Type type)
        {
            _ = TryChangeType(value, type, out object
#if CS8
            ?
#endif
            result);

            return result;
        }

        public static bool TryChangeType<T>(in object
#if CS8
            ?
#endif
            value,
#if CS8
        [NotNullIfNotNull(nameof(VALUE))]
#endif
        out T
#if CS9
            ?
#endif
            result)
        {
            if (TryChangeType(value, typeof(T), out object
#if CS8
                ?
#endif
                _result))
            {
                result = (T)_result;

                return true;
            }

            result = default;

            return false;
        }

#if CS8
        [return: NotNullIfNotNull(nameof(VALUE))]
#endif
        public static T
#if CS9
            ?
#endif
            TryChangeType<T>(in object
#if CS8
            ?
#endif
            value) => TryChangeType<T>(value, out T result) ? result : default;

#if CS8
        [return: NotNullIfNotNull(nameof(VALUE))]
#endif
        public static T ChangeType<T>(in object
#if CS8
            ?
#endif
            value) => TryChangeType(value, out T
#if CS9
                ?
#endif
                result) ? result : throw GetInvalidCastException<ulong>(value);
    }
}
