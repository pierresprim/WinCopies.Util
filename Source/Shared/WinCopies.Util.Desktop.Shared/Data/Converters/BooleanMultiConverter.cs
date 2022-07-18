#if WinCopies3
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using static WinCopies.ThrowHelper;

namespace WinCopies.Util.Data
{
    public class BooleanMultiConverter : AlwaysConvertibleOneWayMultiConverter<Diagnostics.ComparisonType, bool>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ConverterHelper.NotNull;

        protected override bool Convert(object[] values, Diagnostics.ComparisonType parameter, CultureInfo culture)
#if CS8
            =>
#else
        {
            if (
#endif
                Diagnostics.Determine.AreOfType<bool>(out int index, values ?? throw GetArgumentNullException(nameof(values)))
#if CS8
                ? ((
#else
                )
                {
#endif
                    PredicateIn<IEnumerable<bool>>
#if CS8
                    )(
#else
                        predicate;

                    switch (
#endif
                    parameter
#if CS8
                    switch
#else
                    )
#endif
                    {
#if !CS8
                        case
#endif
                        Diagnostics.ComparisonType.And
#if CS8
                            =>
#else
                            :
                            predicate =
#endif
                            Bool.AndIn
#if CS8
                                ,
#else
                            ; break;

                        case
#endif
                        Diagnostics.ComparisonType.Or
#if CS8
                            =>
#else
                            :
                            predicate =
#endif
                            Bool.OrIn
#if CS8
                                ,
#else
                            ; break;

                        case
#endif
                        Diagnostics.ComparisonType.Xor
#if CS8
                            =>
#else
                            :
                            predicate =
#endif
                            Bool.XOrIn
#if CS8
                                ,
                        _ =>
#else
                            ; break;

                        default:
#endif
                            throw new ArgumentOutOfRangeException(nameof(parameter))
#if !CS8
                                    ;
#endif
                    }
#if CS8
                        ))
#else
                    return predicate
#endif
                        (values.Cast<bool>())
#if CS8
                    :
#else
                        ;
                }

                else
#endif
                    throw GetValuesMustBeAnInstanceOfTypeException<
#if CS5
                        object,
#endif
                        bool>(
#if !CS5
                        GetAtOrThrowIfArrayNull(
#endif
                        values
#if !CS5
                        , index)
#endif
                        , index, nameof(values));
#if !CS8
        }
#endif
    }
}
#endif
