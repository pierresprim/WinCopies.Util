using System;

using static WinCopies.
#if WinCopies3
    ThrowHelper;
#else
    Util.Util;
#endif

namespace WinCopies
#if !WinCopies3
    .Util
#endif
{
    public interface IConvertible<TIn, TOut>
    {
        TOut ConvertFrom(TIn obj);

        TIn ConvertTo(TOut obj);
    }

    public interface IDelegateConvertible<TIn, TOut> : IConvertible<TIn, TOut>
    {
        Converter<TIn, TOut> Converter { get; }

        Converter<TOut, TIn> BackConverter { get; }
    }

    public static class Convertible
    {
        private struct ConvertibleStruct<TIn, TOut> : IDelegateConvertible<TIn, TOut>
        {
            public Converter<TIn, TOut> Converter { get; }

            public Converter<TOut, TIn> BackConverter { get; }

            internal ConvertibleStruct(in Converter<TIn, TOut> convertFrom, in Converter<TOut, TIn> convertTo)
            {
                Converter = convertFrom ?? throw GetArgumentNullException(nameof(convertFrom));

                BackConverter = convertTo ?? throw GetArgumentNullException(nameof(convertTo));
            }

            public TOut ConvertFrom(TIn obj) => Converter(obj);

            public TIn ConvertTo(TOut obj) => BackConverter(obj);
        }

        public static IDelegateConvertible<TIn, TOut> GetConvertible<TIn, TOut>(in Converter<TIn, TOut> convertFrom, in Converter<TOut, TIn> convertTo) => new ConvertibleStruct<TIn, TOut>(convertFrom, convertTo);

        public static IDelegateConvertible<T,T> GetConvertible<T>() => GetConvertible<T, T>(Delegates.Self, Delegates.Self);
    }
}
