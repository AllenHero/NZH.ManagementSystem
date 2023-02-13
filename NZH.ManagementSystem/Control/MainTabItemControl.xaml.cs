using NZH.Model;
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
    /// MainTabItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class MainTabItemControl : UserControl, IDisposable
    {
        public string UICode { get; set; }
        public PropertyNodeItem PropertyNodeItem { get; set; }

        public MainTabItemControl()
        {
            this.Loaded += new RoutedEventHandler(MainTabItemControl_Loaded);
            InitializeComponent();
        }

        public string HeaderText
        {
            set
            {
                txtHeader.Text = "当前位置： " + value;
                int count = txtHeader.Text.Length;
                gridHeader.Width = count * 15 + 20;
            }
        }


        public void Dispose()
        {

            System.GC.Collect();
        }

        void MainTabItemControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(MainTabItemControl_Loaded);
            try
            {
                if (PropertyNodeItem != null && !string.IsNullOrEmpty(PropertyNodeItem.AuNode))
                {
                    frame.Navigate(new Uri(UICode, UriKind.RelativeOrAbsolute), PropertyNodeItem.AuNode);
                }
                else
                    frame.Navigate(new Uri(UICode, UriKind.RelativeOrAbsolute), "OK");

                frame.LoadCompleted += new LoadCompletedEventHandler(frame_LoadCompleted);
                frame.NavigationFailed += new NavigationFailedEventHandler(frame_NavigationFailed);
            }
            catch
            {
               
            }
        }

        void frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Page page = frame.Content as Page;
            page.DataContext = e.ExtraData;
        }

        void frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {

            frame.Source = new Uri("/ErrorPage.xaml", UriKind.RelativeOrAbsolute); ;
            e.Handled = true;
        }

    }
}
