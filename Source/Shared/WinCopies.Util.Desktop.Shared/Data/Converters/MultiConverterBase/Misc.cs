/* Copyright © Pierre Sprimont, 2021
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

namespace WinCopies.Util.Data
{
#if WinCopies3
    public interface IConverterConverter
    {
        T GetDefaultValue<T>();
    }

    public interface IConverterConverter<TIn, TOut> : IConverterConverter
    {
        TOut Convert(in TIn value);
    }

    public interface IMultiConverterConverter<TIn, TOut> : IConverterConverter<TIn, TOut>
    {
        TIn ConvertBack(in TOut value);
    }

    public class ConverterConverter : IConverterConverter
    {
        private static ConverterConverter _instance;

        public static ConverterConverter Instance => _instance
#if CS8
            ??=
#else
            ?? (_instance =
#endif
            new ConverterConverter()
#if !CS8
            )
#endif
            ;

        public T GetDefaultValue<T>() => default;

        protected ConverterConverter() { /* Left empty. */ }
    }

    public abstract class ConverterConverter<TIn, TOut> : ConverterConverter, IConverterConverter<TIn, TOut>
    {
        public abstract TOut Convert(in TIn value);
    }

    public class ConverterConverter<T> : ConverterConverter<T, T>, IConverterConverter<T, T>
    {
        public override T Convert(in T value) => value;
    }

    public abstract class MultiConverterConverter<TIn, TOut> : ConverterConverter<TIn, TOut>, IMultiConverterConverter<TIn, TOut>
    {
        public abstract TIn ConvertBack(in TOut value);
    }

    public class MultiConverterConverter<T> : ConverterConverter<T, T>, IMultiConverterConverter<T, T>
    {
        public override T Convert(in T value) => value;

        public virtual T ConvertBack(in T value) => value;
    }
#else
        public interface IMultiConverterConverters<TParamIn, TParamOut, TDestinationIn, TDestinationOut>
    {
        TParamOut GetDefaultParameter();

        TParamOut ConvertParameter(in TParamIn parameter);

        TDestinationOut GetDefaultDestinationValue();

        TDestinationOut ConvertDestinationValue(in TDestinationIn value);
    }

    public sealed class MultiConverterConverters<TParam, TDestination> : IMultiConverterConverters<TParam, TParam, TDestination, TDestination>
    {
        public TParam GetDefaultParameter() => default;

        public TParam ConvertParameter(in TParam parameter) => parameter;

        public TDestination GetDefaultDestinationValue() => default;

        public TDestination ConvertDestinationValue(in TDestination value) => value;
    }
#endif
}
