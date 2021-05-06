using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace InterestOrganiser.Converters
{
    public class ItemTypeToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            string type = value.ToString();
            switch (type)
            {
                case "movies": return true;
                case "tv series": return true;
                case "books": return false;
                case "games": return false;
                default: return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}