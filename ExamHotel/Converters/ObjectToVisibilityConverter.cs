using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace ExamHotel.Converters
{
    public class ObjectToVisibilityConverter : IValueConverter
    {
        public static ObjectToVisibilityConverter Instance { get; } = new ObjectToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null; // Возвращаем true, если значение не null, иначе false
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}