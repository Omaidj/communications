using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

// In the 'communications' namespace
namespace communications
{
    // Define the BytesToImageConverter class
    public class BytesToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var imageData = (byte[])value;
            var stream = new MemoryStream(imageData);
            return ImageSource.FromStream(() => stream);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
