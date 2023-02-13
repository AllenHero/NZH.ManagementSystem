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
    /// ExpanderItem.xaml 的交互逻辑
    /// </summary>
    public partial class ExpanderItem : UserControl
    {
        public ExpanderItem()
        {
            InitializeComponent();
           
        }

        public string HeaderText
        {
            get { return headerTextBlock.Text; }
            set { headerTextBlock.Text = value; }
        }
        public double HeaderFontSize
        {
            get { return headerTextBlock.FontSize; }
            set { headerTextBlock.FontSize = value; }
        }
        public Brush HeaderForeground
        {
            get { return headerTextBlock.Foreground; }
            set { headerTextBlock.Foreground = value; }
        }
        public ImageSource ImageSource
        {
            get { return headerImage.Source; }
            set { headerImage.Source = value; }
        }
        public ImageSource BackgroundImage
        {
            get
            {
                return bgImage.Source;
            }
            set
            {
                bgImage.Source = value;
            }
        }


    }
}
