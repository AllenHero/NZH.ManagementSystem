using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NZH.ManagementSystem.Base
{
    [TemplatePart(Name = HeaderContainerName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MinimizeButtonName, Type = typeof(Image))]
    [TemplatePart(Name = RestoreButtonName, Type = typeof(Image))]
    [TemplatePart(Name = CloseButtonName, Type = typeof(Image))]
    [TemplatePart(Name = TopResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = LeftResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = RightResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = BottomResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = BottomRightResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = TopRightResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = TopLeftResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = BottomLeftResizerName, Type = typeof(Thumb))]

    public class BaseWindow : Window
    {
        IntPtr ActiveWindowHandle;  //定义活动窗体的句柄  

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetActiveWindow();  //获得父窗体句柄  

        static BaseWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseWindow), new FrameworkPropertyMetadata(typeof(BaseWindow)));
        }

        private const string HeaderContainerName = "PART_HeaderContainer";
        private const string MinimizeButtonName = "PART_MinimizeButton";
        private const string RestoreButtonName = "PART_RestoreButton";
        private const string CloseButtonName = "PART_CloseButton";
        private const string TopResizerName = "PART_TopResizer";
        private const string LeftResizerName = "PART_LeftResizer";
        private const string RightResizerName = "PART_RightResizer";
        private const string BottomResizerName = "PART_BottomResizer";
        private const string BottomRightResizerName = "PART_BottomRightResizer";
        private const string TopRightResizerName = "PART_TopRightResizer";
        private const string TopLeftResizerName = "PART_TopLeftResizer";
        private const string BottomLeftResizerName = "PART_BottomLeftResizer";

        public static readonly DependencyProperty ShowDefaultHeaderProperty = DependencyProperty.Register("ShowDefaultHeader", typeof(bool), typeof(BaseWindow), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty ShowResizeGripProperty = DependencyProperty.Register("ShowResizeGrip", typeof(bool), typeof(BaseWindow), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty CanResizeProperty = DependencyProperty.Register("CanResize", typeof(bool), typeof(BaseWindow), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(BaseWindow), new FrameworkPropertyMetadata(null, OnHeaderChanged));
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(BaseWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty HeaderTempateSelectorProperty = DependencyProperty.Register("HeaderTempateSelector", typeof(DataTemplateSelector), typeof(BaseWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty IsFullScreenMaximizeProperty = DependencyProperty.Register("IsFullScreenMaximize", typeof(bool), typeof(BaseWindow), new FrameworkPropertyMetadata(false));

        private static void OnHeaderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            BaseWindow win = sender as BaseWindow;
            win.RemoveLogicalChild(e.OldValue);
            win.AddLogicalChild(e.NewValue);
        }

        public bool ShowDefaultHeader
        {
            get { return (bool)GetValue(ShowDefaultHeaderProperty); }
            set { SetValue(ShowDefaultHeaderProperty, value); }
        }

        public bool CanResize
        {
            get { return (bool)GetValue(CanResizeProperty); }
            set
            {
                SetValue(CanResizeProperty, value);
            }
        }

        public bool ShowResizeGrip
        {
            get { return (bool)GetValue(ShowResizeGripProperty); }
            set { SetValue(ShowResizeGripProperty, value); }
        }

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public DataTemplateSelector HeaderTempateSelector
        {
            get { return (DataTemplateSelector)GetValue(HeaderTempateSelectorProperty); }
            set { SetValue(HeaderTempateSelectorProperty, value); }
        }

        public bool IsFullScreenMaximize
        {
            get { return (bool)GetValue(IsFullScreenMaximizeProperty); }
            set { SetValue(IsFullScreenMaximizeProperty, value); }
        }

        private FrameworkElement headerContainer;
        private Image minimizeButton;
        private Image restoreButton;
        private Image closeButton;
        private Thumb topResizer;
        private Thumb leftResizer;
        private Thumb rightResizer;
        private Thumb bottomResizer;
        private Thumb bottomRightResizer;
        private Thumb topRightResizer;
        private Thumb topLeftResizer;
        private Thumb bottomLeftResizer;

        public bool? ShowDialog(Window owner)
        {
            this.Owner = owner;
            if (this.WindowStartupLocation == WindowStartupLocation.CenterScreen)
                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            return base.ShowDialog();
        }

        public new void Show()
        {
            SetStartLocation();
            base.Show();
        }

        public new bool? ShowDialog()
        {
            SetStartLocation();
            return base.ShowDialog();
        }

        /// <summary>
        /// 设置程序的起始位置
        /// 使子窗体居于父窗体中间
        /// </summary>
        private void SetStartLocation()
        {
            if (this.Owner == null && this.WindowStartupLocation == WindowStartupLocation.CenterScreen)
            {
                try
                {
                    ActiveWindowHandle = GetActiveWindow();  //获取父窗体句柄  
                    this.Owner = (Window)System.Windows.Interop.HwndSource.FromHwnd(ActiveWindowHandle).RootVisual;
                    this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                }
                catch
                {

                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            headerContainer = GetTemplateChild<FrameworkElement>(HeaderContainerName);
            headerContainer.MouseLeftButtonDown += HeaderContainerMouseLeftButtonDown;
            closeButton = GetTemplateChild<Image>(CloseButtonName);
            closeButton.MouseEnter += new MouseEventHandler(WindowButton_MouseEnter);
            closeButton.MouseLeave += new MouseEventHandler(WindowButton_MouseLeave);
            closeButton.MouseLeftButtonDown += new MouseButtonEventHandler(WindowButton_MouseLeftButtonDown);
            restoreButton = GetTemplateChild<Image>(RestoreButtonName);
            restoreButton.MouseEnter += new MouseEventHandler(WindowButton_MouseEnter);
            restoreButton.MouseLeave += new MouseEventHandler(WindowButton_MouseLeave);
            restoreButton.MouseLeftButtonDown += new MouseButtonEventHandler(WindowButton_MouseLeftButtonDown);
            minimizeButton = GetTemplateChild<Image>(MinimizeButtonName);
            minimizeButton.MouseEnter += new MouseEventHandler(WindowButton_MouseEnter);
            minimizeButton.MouseLeave += new MouseEventHandler(WindowButton_MouseLeave);
            minimizeButton.MouseLeftButtonDown += new MouseButtonEventHandler(WindowButton_MouseLeftButtonDown);
            topResizer = GetTemplateChild<Thumb>(TopResizerName);
            topResizer.DragDelta += new DragDeltaEventHandler(ResizeTop);
            leftResizer = GetTemplateChild<Thumb>(LeftResizerName);
            leftResizer.DragDelta += new DragDeltaEventHandler(ResizeLeft);
            rightResizer = GetTemplateChild<Thumb>(RightResizerName);
            rightResizer.DragDelta += new DragDeltaEventHandler(ResizeRight);
            bottomResizer = GetTemplateChild<Thumb>(BottomResizerName);
            bottomResizer.DragDelta += new DragDeltaEventHandler(ResizeBottom);
            bottomRightResizer = GetTemplateChild<Thumb>(BottomRightResizerName);
            bottomRightResizer.DragDelta += new DragDeltaEventHandler(ResizeBottomRight);
            topRightResizer = GetTemplateChild<Thumb>(TopRightResizerName);
            topRightResizer.DragDelta += new DragDeltaEventHandler(ResizeTopRight);
            topLeftResizer = GetTemplateChild<Thumb>(TopLeftResizerName);
            topLeftResizer.DragDelta += new DragDeltaEventHandler(ResizeTopLeft);
            bottomLeftResizer = GetTemplateChild<Thumb>(BottomLeftResizerName);
            bottomLeftResizer.DragDelta += new DragDeltaEventHandler(ResizeBottomLeft);

            if (!CanResize)
            {
                restoreButton.Visibility = System.Windows.Visibility.Hidden;
                minimizeButton.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private T GetTemplateChild<T>(string childName) where T : FrameworkElement, new()
        {
            return (GetTemplateChild(childName) as T) ?? new T();
        }

        private void HeaderContainerMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                DragMove();
            }
            else
            {
                if (CanResize)
                {
                    ChangeWindowState(WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
                }
            }
        }

        private void ChangeWindowState(WindowState state)
        {
            if (state == WindowState.Maximized)
            {
                if (!IsFullScreenMaximize && IsLocationOnPrimaryScreen())
                {
                    MaxHeight = SystemParameters.WorkArea.Height;
                    MaxWidth = SystemParameters.WorkArea.Width;
                }
                else
                {
                    MaxHeight = double.PositiveInfinity;
                    MaxWidth = double.PositiveInfinity;
                }
            }
            WindowState = state;
        }

        private bool IsLocationOnPrimaryScreen()
        {
            return Left < SystemParameters.PrimaryScreenWidth && Top < SystemParameters.PrimaryScreenHeight;
        }

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
                            this.Close();
                        }
                        else if (cmd == "Maximized")
                        {
                            button.Tag = "Recovery";
                            this.WindowState = System.Windows.WindowState.Maximized;
                        }
                        else if (cmd == "Recovery")
                        {
                            button.Tag = "Maximized";
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

        private void ResizeBottomLeft(object sender, DragDeltaEventArgs e)
        {
            ResizeLeft(sender, e);
            ResizeBottom(sender, e);
        }

        private void ResizeTopLeft(object sender, DragDeltaEventArgs e)
        {
            ResizeTop(sender, e);
            ResizeLeft(sender, e);
        }

        private void ResizeTopRight(object sender, DragDeltaEventArgs e)
        {
            ResizeRight(sender, e);
            ResizeTop(sender, e);
        }

        private void ResizeBottomRight(object sender, DragDeltaEventArgs e)
        {
            ResizeBottom(sender, e);
            ResizeRight(sender, e);
        }

        private void ResizeBottom(object sender, DragDeltaEventArgs e)
        {
            if (ActualHeight <= MinHeight && e.VerticalChange < 0)
            {
                return;
            }
            if (double.IsNaN(Height))
            {
                Height = ActualHeight;
            }
            Height += e.VerticalChange;
        }

        private void ResizeRight(object sender, DragDeltaEventArgs e)
        {
            if (ActualWidth <= MinWidth && e.HorizontalChange < 0)
            {
                return;
            }
            if (double.IsNaN(Width))
            {
                Width = ActualWidth;
            }
            Width += e.HorizontalChange;
        }

        private void ResizeLeft(object sender, DragDeltaEventArgs e)
        {
            if (ActualWidth <= MinWidth && e.HorizontalChange > 0)
            {
                return;
            }
            if (double.IsNaN(Width))
            {
                Width = ActualWidth;
            }
            Width -= e.HorizontalChange;
            Left += e.HorizontalChange;
        }

        private void ResizeTop(object sender, DragDeltaEventArgs e)
        {
            if (ActualHeight <= MinHeight && e.VerticalChange > 0)
            {
                return;
            }
            if (double.IsNaN(Height))
            {
                Height = ActualHeight;
            }
            Height -= e.VerticalChange;
            Top += e.VerticalChange;
        }
    }
}
