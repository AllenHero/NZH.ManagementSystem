using NZH.ManagementSystem.Control;
using NZH.Model;
using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NZH.ManagementSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Frame frame = new Frame();
        ObservableCollection<TreeModle> TreeModle = new ObservableCollection<TreeModle>();
        Dictionary<int, string> TabItemControl = new Dictionary<int, string>();
        public static UserInfo UserInfo;
        public static string LineNO;
        public static string PROCESS_NO;
        public static string WeightCom;
        public static string TemperatureCom;
        public static string TotalPass;

        //测试

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            ReMessageBox.Dispatcher = this.Dispatcher;
            imageUp.MouseEnter += new MouseEventHandler(ImageButton_MouseEnter);
            imageDown.MouseEnter += new MouseEventHandler(ImageButton_MouseEnter);
            imageUp.MouseLeave += new MouseEventHandler(ImageButton_MouseLeave);
            imageDown.MouseLeave += new MouseEventHandler(ImageButton_MouseLeave);
            imageUp.MouseLeftButtonDown += new MouseButtonEventHandler(ImageUp_MouseLeftButtonDown);
            imageDown.MouseLeftButtonDown += new MouseButtonEventHandler(ImageDown_MouseLeftButtonDown);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LineNO = ConfigurationManager.AppSettings["LineNO"] + "";
            PROCESS_NO = ConfigurationManager.AppSettings["PROCESS_NO"] + "";
            WeightCom = ConfigurationManager.AppSettings["WeightCom"] + "";
            TemperatureCom = ConfigurationManager.AppSettings["TemperatureCom"] + "";

            TabItemControl.Add(0, "首页");
            TreeModle = new ObservableCollection<TreeModle>();
            if (UserInfo != null && UserInfo.authority != null)
            {
                foreach (var row in UserInfo.authority)
                {
                    if (row.Menu + "" != "")
                    {
                        TreeModle test = new TreeModle();
                        test.ID = row.FunCode;
                        test.FATHER_ID = row.ParentID;
                        test.CODE = row.Menu;
                        test.NMAE = row.FunName;
                        test.AuNode = row.AuNode;
                        TreeModle.Add(test);
                    }
                }
                tbUserName.Text = "当前用户：【" + UserInfo.TrueName + "】";
            }
            try
            {
                LoadMenu();
            }
            catch (Exception ex)
            {
                ReMessageBox.Show(ex.Message);
            }

        }


        #region 主菜单无法显示完全时，滚动按钮
        private void ImageDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                menuStack.LineUp();
            }
            catch
            {
            }
        }
        private void ImageUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                menuStack.LineDown();
            }
            catch
            {
            }
        }
        private void ImageButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                image.Opacity = 1;
            }
        }
        private void ImageButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                image.Opacity = 0.5;
            }
        }
        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            CheckShowUpDownButton();
        }
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            CheckShowUpDownButton();
        }

        /// <summary>
        /// Expander展开或折叠事件触发后，并未立即改变Expander.ActualHeight的值，所以需要计算Expander的高度
        /// </summary>
        private void CheckShowUpDownButton()
        {
            double allHeight = 0;
            foreach (UIElement control in menuStack.Children)
            {
                Expander exp = control as Expander;
                if (exp != null)
                {
                    allHeight += 30;
                    if (exp.IsExpanded)
                    {
                        Grid grid = exp.Content as Grid;
                        if (grid != null)
                        {
                            int count = grid.RowDefinitions.Count;
                            allHeight += (count + 1) / 2 * 30;
                            allHeight += (count - 1) / 2 * 2;
                        }
                    }
                }
            }
            double aheight = menuStack.ActualHeight;
            if (allHeight > aheight)
            {
                gridUpDownButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                gridUpDownButton.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        #endregion

        public void Dispose()
        {
            this.Loaded -= new RoutedEventHandler(MainWindow_Loaded);
            imageUp.MouseEnter -= new MouseEventHandler(ImageButton_MouseEnter);
            imageDown.MouseEnter -= new MouseEventHandler(ImageButton_MouseEnter);
            imageUp.MouseLeave -= new MouseEventHandler(ImageButton_MouseLeave);
            imageDown.MouseLeave -= new MouseEventHandler(ImageButton_MouseLeave);
            imageUp.MouseLeftButtonDown -= new MouseButtonEventHandler(ImageUp_MouseLeftButtonDown);
            imageDown.MouseLeftButtonDown -= new MouseButtonEventHandler(ImageDown_MouseLeftButtonDown);
            GC.WaitForPendingFinalizers();
            System.GC.Collect();
        }


        //关闭窗体
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string header = btn.Tag.ToString();
            foreach (ControlTabItem item in MainTab.Items)
            {
                if (item.Header.ToString() == header && item.Header.ToString() != "首页")
                {
                    MainTabItemControl MainTabItemControl = item.Content as MainTabItemControl;
                    IDisposable dispose = MainTabItemControl.frame.Content as IDisposable;
                    if (dispose != null)
                        dispose.Dispose();
                    MainTab.Items.Remove(item);
                    List<string> list = new List<string>();
                    foreach (var row in TabItemControl)
                    {
                        if (row.Value != item.Header.ToString())
                        {
                            list.Add(row.Value);
                        }
                    }
                    TabItemControl = new Dictionary<int, string>();
                    int i = 0;
                    foreach (var row in list)
                    {
                        TabItemControl.Add(i, row);
                        i = i + 1;
                    }
                    break;
                }
            }
        }


        private void mainHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            try
            {
                this.DragMove();
                if (e.ClickCount >= 2)
                {
                    if (this.WindowState == System.Windows.WindowState.Maximized)
                        this.WindowState = System.Windows.WindowState.Normal;
                    else this.WindowState = System.Windows.WindowState.Maximized;
                }
            }
            catch
            {
            }
        }


        //显示和隐藏菜单
        private void dockShowMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender is Image)
                {
                    Image dock = sender as Image;
                    if (gcMenu.Width.Value >= 10)
                    {
                        gcMenu.Width = new GridLength(6);
                        rotatetransform.Angle = 180;
                    }
                    else
                    {
                        gcMenu.Width = new GridLength(200);
                        rotatetransform.Angle = 0;
                    }
                }
            }
            catch
            {
            }
        }
        private void dockShowMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            dockShowMenu.Opacity = 0.7;
        }
        private void dockShowMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            dockShowMenu.Opacity = 1;
        }


        #region 生成菜单
        private void LoadMenu()
        {
            List<PropertyNodeItem> itemList = new List<PropertyNodeItem>();

            foreach (var row in TreeModle)
            {
                if (row.FATHER_ID == "0")
                {
                    PropertyNodeItem node = new PropertyNodeItem()
                    {
                        Icon = row.CODE + "",
                        DisplayName = row.NMAE + "",
                        Name = row.NMAE + "",
                        id = row.ID,
                        AuNode = row.AuNode,
                        parentId = row.FATHER_ID,
                        IsExpanded = true

                    };
                    string id = row.ID;
                    string pid = row.FATHER_ID;
                    ForeachPropertyNode(node, id);
                    itemList.Add(node);
                }
            }
            foreach (PropertyNodeItem item in itemList)
            {
                Expander exp = CreateExpander(item);
                menuStack.Children.Add(exp);
                Grid grid = new Grid();
                exp.Content = grid;
                foreach (PropertyNodeItem sub in item.Children)
                {
                    CreateExpanderItem(grid, sub);
                }
                //exp.IsExpanded = true;
            }
        }
        private void ForeachPropertyNode(PropertyNodeItem node, string pid)
        {
            foreach (var row in TreeModle)
            {
                if (row.FATHER_ID == pid.ToString())
                {
                    string id = row.ID;
                    string name = row.NMAE + "";
                    string parentId = row.FATHER_ID;
                    PropertyNodeItem childNodeItem = new PropertyNodeItem()
                    {
                        Icon = row.CODE + "",
                        DisplayName = name,
                        Name = name,
                        id = id,
                        AuNode = row.AuNode,
                        parentId = parentId,
                        IsExpanded = false
                    };
                    ForeachPropertyNode(childNodeItem, id);
                    node.Children.Add(childNodeItem);
                }
            }
        }
        private Expander CreateExpander(PropertyNodeItem NodeItem)
        {
            Expander exp = new Expander();
            exp.Expanded += new RoutedEventHandler(Expander_Expanded);
            exp.Collapsed += new RoutedEventHandler(Expander_Collapsed);
            exp.Header = new TextBlock() { VerticalAlignment = System.Windows.VerticalAlignment.Center, Text = NodeItem.DisplayName, FontSize = 14, Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)) };
            return exp;
        }
        private void CreateExpanderItem(Grid grid, PropertyNodeItem NodeItem)
        {
            if (grid.RowDefinitions.Count != 0)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2) });
                Border border = new Border() { Width = 150, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)) };
                border.Child = new Image { Source = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/Line.jpg", UriKind.Relative)) };
                grid.Children.Add(border);
                border.SetValue(Grid.RowProperty, grid.RowDefinitions.Count - 1);
            }

            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            ExpanderItem item = new ExpanderItem();
            item.HeaderText = NodeItem.DisplayName;
            item.Tag = NodeItem;
            item.HeaderFontSize = 13;
            item.HeaderForeground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            if (IsExistResource("Image/Icon/" + NodeItem.DisplayName + ".ico"))
            {
                item.ImageSource = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/" + NodeItem.DisplayName + ".ico", UriKind.Relative));
            }
            else
            {
                item.ImageSource = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/DefaultIcon.ico", UriKind.Relative));
            }
            grid.Children.Add(item);
            item.SetValue(Grid.RowProperty, grid.RowDefinitions.Count - 1);
            item.MouseEnter += new MouseEventHandler(ExpanderItem_MouseEnter);
            item.MouseLeave += new MouseEventHandler(ExpanderItem_MouseLeave);
            item.MouseLeftButtonDown += new MouseButtonEventHandler(ExpanderItem_MouseLeftButtonDown);
        }
        /// <summary>
        /// 判断指定key的资源是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool IsExistResource(string key)
        {
            try
            {
                #region key值包含中文时，需要转化
                string newKey = "";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(key);
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] > 128)
                    {
                        newKey += "%" + Convert.ToString(data[i], 16);
                    }
                    else newKey += (char)data[i];
                }
                #endregion
                string resourceName = this.GetType().Assembly.GetName().Name + ".g";
                System.Resources.ResourceManager mgr = new System.Resources.ResourceManager(resourceName, this.GetType().Assembly);
                using (System.Resources.ResourceSet set = mgr.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true))
                {
                    object obj = set.GetObject(newKey, true);
                    if (obj == null) return false;
                    else return true;
                }
            }
            catch
            {
                return false;
            }
        }


        #endregion

        #region 菜单的鼠标效果和点击事件
        private void ExpanderItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ExpanderItem expItem = sender as ExpanderItem;
                if (expItem != null)
                {
                    if (expItem.Tag is PropertyNodeItem)
                    {
                        PropertyNodeItem item = expItem.Tag as PropertyNodeItem;
                        if (item == null)
                            return;
                        if (item.Icon + "" == "")
                            return;

                        foreach (var row in TabItemControl)
                        {
                            if (row.Value + "" == item.DisplayName + "")
                            {
                                MainTab.SelectedIndex = Convert.ToInt32(row.Key);
                                return;
                            }

                        }
                        if (TabItemControl.Count >= 8)
                        {
                            ReMessageBox.Show("加载的界面过多，请删除几个界面重试");
                            return;
                        }
                        MainTabItemControl MainTabItemControl = new MainTabItemControl();
                        MainTabItemControl.UICode = item.Icon;
                        MainTabItemControl.PropertyNodeItem = item;
                        MainTabItemControl.HeaderText = item.DisplayName;
                        ControlTabItem TabItem = new ControlTabItem();
                        TabItem.Margin = new Thickness(3, 0, 0, 0);
                        TabItem.Header = item.DisplayName;
                        MainTabItemControl.Margin = new Thickness(-5, -5, -3, -3);
                        TabItem.Content = MainTabItemControl;
                        MainTab.Items.Add(TabItem);

                        TabItemControl.Add(TabItemControl.Count, item.DisplayName);
                        MainTab.SelectedIndex = TabItemControl.Count - 1;
                    }
                    else if (expItem.Tag != null && expItem.Tag.ToString() == "ExitSystem")
                    { // 关闭主窗体，打开登陆界面
                        if (ReMessageBox.Show("是否注销系统？", "提示", MessageWindowButtons.YesNo, MessageWindowIcons.Question) == MessageWindowResult.Yes)
                        {
                            Dispose();
                            #region 重启系统
                            //目的是关闭所有线程，主要是控件内的线程
                            RestartSystem();
                            #endregion
                        }
                    }
                    else if (expItem.Tag != null && expItem.Tag.ToString() == "ChangePassword")
                    {//打开子窗体 
                        ChangePassword aChild = new ChangePassword();
                        aChild.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        aChild.UserInfo = UserInfo;
                        aChild.ShowDialog();
                    }
                }
            }
            catch
            {
            }
        }

        private void RestartSystem()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Process.GetCurrentProcess().MainModule.FileName; //Assembly.GetExecutingAssembly().Location;
            Process.Start(psi);
            Application.Current.Shutdown();
        }

        private void ExpanderItem_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                ExpanderItem expItem = sender as ExpanderItem;
                if (expItem != null)
                {
                    expItem.HeaderForeground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    expItem.BackgroundImage = null;
                }
            }
            catch
            {
            }
        }
        private void ExpanderItem_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                ExpanderItem expItem = sender as ExpanderItem;
                if (expItem != null)
                {
                    expItem.HeaderForeground = new SolidColorBrush(Color.FromArgb(255, 1, 62, 127));
                    expItem.BackgroundImage = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/ExpanderMouse.jpg", UriKind.Relative));
                }
            }
            catch
            {

            }
        }


        #endregion

        #region 最大化、最小化、关闭按钮、主菜单按钮
        private void WindowButton_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                Image button = sender as Image;
                if (button != null)
                {
                    button.Source = new BitmapImage(new Uri(string.Format("/NZH.ManagementSystem;component/Image/{0}_2.png", button.Tag), UriKind.Relative));
                }
            }
            catch
            {
            }
        }
        private void WindowButton_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                Image button = sender as Image;
                if (button != null)
                {
                    button.Source = new BitmapImage(new Uri(string.Format("/NZH.ManagementSystem;component/Image/{0}_1.png", button.Tag), UriKind.Relative));
                }
            }
            catch
            {
            }
        }
        private void WindowButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Image button = sender as Image;
                if (button != null)
                {
                    if (button.Tag != null)
                    {
                        string cmd = button.Tag.ToString();
                        if (cmd == "Exit")
                        {
                            if (ReMessageBox.Show("是否退出系统？", "提示", MessageWindowButtons.YesNo, MessageWindowIcons.Question) == MessageWindowResult.Yes)
                            {
                                Dispose();
                                this.Close();
                            }
                        }
                        else if (cmd == "Maximized")
                        {
                            imageMax.Tag = "Recovery";
                            this.WindowState = System.Windows.WindowState.Maximized;
                        }
                        else if (cmd == "Recovery")
                        {
                            imageMax.Tag = "Maximized";
                            this.WindowState = System.Windows.WindowState.Normal;
                        }
                        else if (cmd == "Minimized")
                        {
                            this.WindowState = System.Windows.WindowState.Minimized;
                        }

                    }
                }
            }
            catch
            {
            }
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            #region 判断是否显示按钮
            CheckShowUpDownButton();
            #endregion

            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                imageMax.Tag = "Recovery";
                imageMax.Source = new BitmapImage(new Uri(string.Format("/NZH.ManagementSystem;component/Image/{0}_1.png", imageMax.Tag), UriKind.Relative));
            }
            else if (this.WindowState == System.Windows.WindowState.Normal)
            {
                imageMax.Tag = "Maximized";
                imageMax.Source = new BitmapImage(new Uri(string.Format("/NZH.ManagementSystem;component/Image/{0}_1.png", imageMax.Tag), UriKind.Relative));
            }
        }

        #endregion
    }
}
