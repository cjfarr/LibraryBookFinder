namespace LibraryBookFinder.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = false;
            if (value?.GetType() == typeof(bool))
            {
                isVisible = bool.Parse(value.ToString());
            }

            if (parameter != null)
            {
                if (bool.TryParse(parameter?.ToString(), out bool doOpposite))
                {
                    isVisible = !isVisible;
                }
            }

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
