using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NZH.ManagementSystem.Control
{
    /// <summary>
    /// CellControl.xaml 的交互逻辑
    /// </summary>
    public partial class CellControl : UserControl
    {
        private double MIN_VALUE;
        private double MAX_VALUE;
        private string VALUE;
        public bool well = true;
        public CellControl(string _value, double _min_value, double _max_value)
        {
            InitializeComponent();
            VALUE = _value;
            MIN_VALUE = _min_value;
            MAX_VALUE = _max_value;
            this.Loaded += CellControl_Loaded;

        }

        private void CellControl_Loaded(object sender, RoutedEventArgs e)
        {
            double min = MIN_VALUE;
            double max = MAX_VALUE;
            lb.Content = VALUE;
            if (VALUE + "" != "")
            {
                double val = Convert.ToDouble(VALUE);
                if (val > max || val < min)
                {
                    lb.Background = new SolidColorBrush(Colors.Red);
                    well = false;
                }
            }

        }


    }

}
