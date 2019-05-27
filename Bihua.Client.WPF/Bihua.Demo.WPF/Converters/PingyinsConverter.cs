using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

/// <summary>
/// 拼音转换器
/// </summary>
namespace Bihua.Demo.WPF.Converters
{
    //拼音转换器
    public class PingyinsConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] pingyins = value as string[];
            if (value == null) return DependencyProperty.UnsetValue;

            return string.Join("；", pingyins);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
