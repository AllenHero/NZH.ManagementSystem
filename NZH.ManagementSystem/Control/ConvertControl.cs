using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NZH.ManagementSystem.Control
{
    public class ConvertControl
    {
    }

    public class GradeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value == 1)
            {
                return "等级一";
            }
            else if ((int)value == 2)
            {
                return "等级二";
            }
            else if ((int)value == 3)
            {
                return "等级三";
            }
            else if ((int)value == 4)
            {
                return "等级四";
            }
            else if ((int)value == 5)
            {
                return "等级五";
            }
            else if ((int)value == 6)
            {
                return "等级六";
            }
            else if ((int)value == 7)
            {
                return "等级七";
            }
            else if ((int)value == 8)
            {
                return "等级八";
            }
            else if ((int)value == 9)
            {
                return "等级九";
            }
            else
                return "";

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }


    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string reslut = "";
            if (value + "" != "")
                reslut = System.Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
            return reslut;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class IsWrokingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value == 0)
            {
                return "";
            }
            else
            {
                return "生产中";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class IsScanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value == 1)
            {
                return "是";
            }
            else
            {
                return "否";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
