using System;

namespace WinCopies.Collections.AbstractionInterop.Generic
{
    public static partial class AbstractionTypes<TSource, TDestination> where TSource : TDestination
    {
        public static ArgumentException GetArgumentException() => new ArgumentException($"The given value is not an instance of TSource.");
    }
}
