using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace InterestOrganiser.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        static WebClient Client = new WebClient();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fileName = parameter as string == null ? "defaultCover.jpg" : parameter as string;

            if (value == null)
                return ImageSource.FromResource($"InterestOrganiser.Resources.Images.{fileName}");
            try
            {
                var byteArray = Client.DownloadData(value.ToString());
                return ImageSource.FromStream(() => new MemoryStream(byteArray));
            }
            catch
            {
                return ImageSource.FromResource($"InterestOrganiser.Resources.Images.{fileName}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
