namespace LibraryBookFinder.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class BoolToOppositeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool turnOpposite)
            {
                return !turnOpposite;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
