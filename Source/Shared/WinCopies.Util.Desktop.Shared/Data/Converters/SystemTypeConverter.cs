using System;
using System.Globalization;

using static System.Convert;

namespace WinCopies.Util.Data
{
    public class SystemTypeConverter : ConverterBase
    {
        public bool AllowNull { get; set; }

        public override object
#if CS8
            ?
#endif
            Convert(object value, Type targetType, object parameter, CultureInfo
#if CS8
            ?
#endif
            culture)
        {
            try
            {
                return AllowNull && ((value as IConvertible) == null || targetType == null)
                       ? null
                       /*: parameter is TypeCode typeCode
                       ? culture == null
                           ? ChangeType(value, typeCode)
                           : ChangeType(value, typeCode, culture)*/
                       //: targetType == null
                       //? null
                       : culture == null
                       ? ChangeType(value, targetType)
                       : ChangeType(value, targetType, culture);
            }

            catch (Exception ex) when (ex.Is(false, typeof(InvalidCastException), typeof(FormatException)))
            {
                if (AllowNull)

                    return null;

                throw;
            }
        }

        public override object
#if CS8
            ?
#endif
            ConvertBack(object value, Type targetType, object parameter, CultureInfo
#if CS8
            ?
#endif
            culture) => Convert(value, targetType, parameter, culture);
    }
}
