using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.Linq
{
   public static class Extensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, Predicate<T> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (T value in enumerable)

                if (func(value))

                    yield return value;
        }

        public static IEnumerable Where(this IEnumerable enumerable, Predicate func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object value in enumerable)

                if (func(value))

                    yield return value;
        }
    }
}
