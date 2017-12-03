using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace MediAid.Models
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {

                if (String.IsNullOrEmpty(value as string) || !File.Exists(value as string)) return ImageSource.FromResource("pills.png");
                return ImageSource.FromFile(value as string);
            }
            return ImageSource.FromResource("pills.png");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Image)
            {
                return ((Image)value).Source;
            }
            return null;
        }
    }
}
