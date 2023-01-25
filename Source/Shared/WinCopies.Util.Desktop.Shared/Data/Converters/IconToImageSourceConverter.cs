/* Copyright © Pierre Sprimont, 2020
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

using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using WinCopies.Desktop;

using static WinCopies.Util.Data.ConverterHelper;

namespace WinCopies.Util.Data
{
    [ValueConversion(typeof(Icon), typeof(ImageSource))]
    public class IconToImageSourceConverter : AlwaysConvertibleTwoWayConverter<Icon
#if CS8
            ?
#endif
            , object
#if CS8
            ?
#endif
            , ImageSource
#if CS8
            ?
#endif
            >
    {
        public override IReadOnlyConversionOptions ConvertOptions => AllowNull;
        public override IReadOnlyConversionOptions ConvertBackOptions => AllowNull;

        protected override ImageSource
#if CS8
            ?
#endif
            Convert(Icon
#if CS8
            ?
#endif
            value, object
#if CS8
            ?
#endif
            parameter, CultureInfo culture) => value?.ToImageSource();

        protected override Icon
#if CS8
            ?
#endif
            ConvertBack(ImageSource
#if CS8
            ?
#endif
            value, object
#if CS8
            ?
#endif
            parameter, CultureInfo culture) => value == null ? null : Icon.FromHandle(((BitmapSource)value).ToBitmap().GetHicon());
    }
}
