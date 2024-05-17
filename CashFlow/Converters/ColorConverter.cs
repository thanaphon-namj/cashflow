using System;
using System.Globalization;

namespace CashFlow.Converters
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                "Income" => App.Current.Resources["IncomeColor"],
                "Spend" => App.Current.Resources["SpendColor"],
                _ => value,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

