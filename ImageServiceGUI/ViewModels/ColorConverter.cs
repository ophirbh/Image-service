using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ImageServiceGUI.ViewModels
{
    /// <summary>
    /// Represents a color converter.
    /// </summary>
    class ColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts the color of the GUI's background.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MessageTypeEnum type =(MessageTypeEnum)value;
            switch (type)
            {
                case MessageTypeEnum.INFO:
                    return "GREEN";
                case MessageTypeEnum.WARNING:
                    return "YELLOW";
                case MessageTypeEnum.FAIL:
                    return "RED";
                default:
                    return "TRANSPARENT";

            }
        }

        /// <summary>
        /// Converts back the color of the GUI's background.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
