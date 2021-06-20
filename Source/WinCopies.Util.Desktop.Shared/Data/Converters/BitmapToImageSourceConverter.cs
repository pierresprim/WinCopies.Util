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

#if WinCopies3
using WinCopies.Desktop;

using static WinCopies.Util.Data.ConverterHelper;
#else
using System;
#endif

namespace WinCopies.Util.Data
{
    [ValueConversion(typeof(Bitmap), typeof(ImageSource))]
    public class
#if WinCopies3
        BitmapToImageSourceConverter : AlwaysConvertibleTwoWayConverter<Bitmap, object, ImageSource>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ParameterCanBeNull;

        public override IReadOnlyConversionOptions ConvertBackOptions => ParameterCanBeNull;

        protected override ImageSource Convert(Bitmap value, object parameter, CultureInfo culture) => value.ToImageSource();

        protected override Bitmap ConvertBack(ImageSource value, object parameter, CultureInfo culture) => ((BitmapSource)value).ToBitmap();
#else
        IconToImageSourceConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is Bitmap _value ? _value.ToImageSource() : null;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        //public static ImageSource ToImageSource(this Icon icon)

        //{

        //    IntPtr hIcon = icon.Handle;

        //    BitmapSource wpfIcon = Imaging.CreateBitmapSourceFromHIcon(
        //        hIcon,
        //        Int32Rect.Empty,
        //        BitmapSizeOptions.FromEmptyOptions());

        //    //if (!Util.DeleteObject(hIcon))

        //    //    throw new Win32Exception();

        //    //using (MemoryStream memoryStream = new MemoryStream())

        //    //{

        //    //    icon.ToBitmap().Save(memoryStream, ImageFormat.Png);

        //    //    IconBitmapDecoder iconBitmapDecoder = new IconBitmapDecoder(memoryStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.Default);

        //    //    return (ImageSource) new ImageSourceConverter().ConvertFrom( iconBitmapDecoder);

        //    //}

        //    ImageSource imageSource;

        //    // Icon icon = Icon.ExtractAssociatedIcon(path);

        //    using (Bitmap bmp = icon.ToBitmap())
        //    {
        //        var stream = new MemoryStream();
        //        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        imageSource = BitmapFrame.Create(stream);
        //    }

        //    return imageSource;

        //    return icon.ToBitmap().ToImageSource();

        //    return wpfIcon;

        //}

        //CS7

        //#endif
#endif
    }
}
