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

#if WinCopies3
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using WinCopies.Desktop;

#if WinCopies3
using static WinCopies.Util.Data.ConverterHelper;
#endif

namespace WinCopies.Util.Data
{
    [ValueConversion(typeof(Icon), typeof(ImageSource))]
    public class IconToImageSourceConverter : AlwaysConvertibleTwoWayConverter<Icon, object, ImageSource>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ParameterCanBeNull;

        public override IReadOnlyConversionOptions ConvertBackOptions => ParameterCanBeNull;

        protected override ImageSource Convert(Icon value, object parameter, CultureInfo culture) => value.ToImageSource();

        protected override Icon ConvertBack(ImageSource value, object parameter, CultureInfo culture) => Icon.FromHandle(((BitmapSource)value).ToBitmap().GetHicon());
    }
}
#endif
