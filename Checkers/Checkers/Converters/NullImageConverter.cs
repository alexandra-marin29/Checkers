using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Checkers.Converters
{
    public class NullImageConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty DefaultImageProperty =
            DependencyProperty.Register("DefaultImage", typeof(string), typeof(NullImageConverter));

        public string DefaultImage
        {
            get { return (string)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(int), typeof(NullImageConverter));

        public int ImageWidth
        {
            get { return (int)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(int), typeof(NullImageConverter));

        public int ImageHeight
        {
            get { return (int)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(DefaultImage, UriKind.Absolute);
                img.DecodePixelWidth = ImageWidth;
                img.DecodePixelHeight = ImageHeight;
                img.EndInit();
                return img;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
