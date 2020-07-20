using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using WinCopies.Util;
#if WinCopies2
using static WinCopies.Util.Util;
#else
using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Linq
{
    public static class Extensions
    {
        public static IEnumerable<T> WherePredicate<T>(this IEnumerable<T> enumerable, Predicate<T> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));

            foreach (T value in enumerable)

                if (func(value))

                    yield return value;
        }

        public static IEnumerable WherePredicate(this IEnumerable enumerable, Predicate func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));

            foreach (object value in enumerable)

                if (func(value))

                    yield return value;
        }
    }
}
