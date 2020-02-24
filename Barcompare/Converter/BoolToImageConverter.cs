using System;
using System.Globalization;
using System.Windows.Data;

namespace HVision.Barcompare.Converter
{
    /// <summary>
    /// bool ==> 해당하는 이미지 소스.
    /// True : 체크 이미지.
    /// False : 엑스마크 이미지.
    /// </summary>
    public class BoolToImageConverter : IValueConverter
    {
        #region Fields

        private readonly string CHECK_IMAGE_SOURCE = @"/Asset/Image/Check_24x24.png";
        private readonly string CROSS_IMAGE_SOURCE = @"/Asset/Image/Cross_24x24.png";

        #endregion

        #region IValueConverter members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Parameter of converter is null.");
            }

            return (bool)value == true ? CHECK_IMAGE_SOURCE : CROSS_IMAGE_SOURCE;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
