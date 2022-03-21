using System;

namespace WinCopies.Collections.AbstractionInterop.Generic
{
    public static partial class AbstractionTypes<TSource, TDestination> where TSource : TDestination
    {
        public static ArgumentException GetArgumentException() => new
#if !CS9
            ArgumentException
#endif
            ($"The given value is not an instance of TSource.");
    }
}
