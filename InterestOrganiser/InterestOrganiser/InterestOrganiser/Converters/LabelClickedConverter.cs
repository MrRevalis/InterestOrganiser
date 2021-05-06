using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace InterestOrganiser.Converters
{
    public class LabelClickedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string type = value as string;
            Label label = parameter as Label;
            if (type.Equals(label.Text))
            {
                return Color.FromHex("237A57");
            }
            else
                return Color.FromHex("215461");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
