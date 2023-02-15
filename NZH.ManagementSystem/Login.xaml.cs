using NZH.Business.BaseData;
using NZH.ManagementSystem;
using NZH.ManagementSystem.Control;
using NZH.Model.BaseData;
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
using System.Windows.Shapes;

namespace NZH.ManagementSystem
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        //用户信息及权限
        public UserInfo UserInfo = new UserInfo();
        private System.ComponentModel.BackgroundWorker LoginWorker = null;
        private UserBusiness userBusiness = new UserBusiness();

        public Login()
        {
            InitializeComponent();
            this.Loaded += Login_Loaded;
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWorker = new System.ComponentModel.BackgroundWorker();
            LoginWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(LoginWorker_DoWork);
            LoginWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(LoginWorker_RunWorkerCompleted);
        }

        //登录
        private void imageLogin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (loading.Visibility == System.Windows.Visibility.Visible) return;
            if (textPersonID.Text + "" == "")
            {
                ReMessageBox.Show("登陆名不能为空");
                textPersonID.Focus();
                return;
            }
            if (textPassword.Password + "" == "")
            {
                ReMessageBox.Show("密码不能为空");
                textPassword.Focus();
                return;
            }
            UserInfo userInfo = new UserInfo()
            {
                UserName = textPersonID.Text,
                UserPassword = textPassword.Password
            };
            InputControlEnable(false);
            LoginWorker.RunWorkerAsync(userInfo);
            loading.Visibility = System.Windows.Visibility.Visible;
            loading.LableText = "登录中……";
        }

        void LoginWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                UserInfo userInfo = e.Argument as UserInfo;
                LoginSystem(userInfo);
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        private void LoginSystem(UserInfo userInfo)
        {
            UserInfo ui = null;
            try
            {
                ui = userBusiness.Login(userInfo.UserName + "", userInfo.UserPassword + "");
            }
            catch
            {
                throw new Exception("获取数据异常，请检查网络后重试");
            }
            UserInfo = ui;
        }

        void LoginWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null && e.Result is string)
                    {
                        ReMessageBox.Show(e.Result.ToString());
                    }
                    else
                    {
                        if (UserInfo != null && UserInfo.UserName != null)
                        {
                            MainWindow.UserInfo = UserInfo;
                            UserInfo.UserPassword = textPassword.Password + "";
                            MainWindow aChild = new MainWindow();
                            aChild.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            aChild.Show();
                            // 关闭自己(父窗体) 
                            this.Close();
                        }
                        else
                        {

                            ReMessageBox.Show("登录失败");
                            imageLogin.Source = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/LandWindow_2.png", UriKind.Relative));
                        }
                    }
                }
            }
            catch
            {
                ReMessageBox.Show("登录失败");
                imageLogin.Source = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/LandWindow_2.png", UriKind.Relative));
            }
            finally
            {
                InputControlEnable(true);
                loading.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void imageLogin_MouseEnter(object sender, MouseEventArgs e)
        {
            if (loading.Visibility == System.Windows.Visibility.Visible) return;
            imageLogin.Source = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/LandWindow_4.png", UriKind.Relative));
        }

        private void imageLogin_MouseLeave(object sender, MouseEventArgs e)
        {
            if (loading.Visibility == System.Windows.Visibility.Visible) return;
            imageLogin.Source = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/LandWindow_2.png", UriKind.Relative));
        }

        private void imageClose_MouseEnter(object sender, MouseEventArgs e)
        {
            if (loading.Visibility == System.Windows.Visibility.Visible) return;
            imageClose.Source = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/Exit_2.png", UriKind.Relative));
        }

        private void imageClose_MouseLeave(object sender, MouseEventArgs e)
        {
            if (loading.Visibility == System.Windows.Visibility.Visible) return;
            imageClose.Source = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/Exit_1.png", UriKind.Relative));
        }

        private void imageClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (loading.Visibility == System.Windows.Visibility.Visible) return;
            this.Close();
        }

        private void imageCancel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (loading.Visibility == System.Windows.Visibility.Visible) return;
            imageCancel.Source = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/LandWindow_5.png", UriKind.Relative));
        }

        private void imageCancel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (loading.Visibility == System.Windows.Visibility.Visible) return;
            imageCancel.Source = new BitmapImage(new Uri("/NZH.ManagementSystem;component/Image/LandWindow_3.png", UriKind.Relative));
        }

        private void imageCancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (loading.Visibility == System.Windows.Visibility.Visible) return;
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (loading.Visibility == System.Windows.Visibility.Visible) return;
                if (e.Key == Key.Enter)
                {
                    imageLogin_MouseLeftButtonDown(null, null);
                }
                else if (e.Key == Key.Escape)
                {
                    this.Close();
                }
            }
            catch { }
        }

        private void InputControlEnable(bool IsEnable)
        {
            textPersonID.IsEnabled = IsEnable;
            textPassword.IsEnabled = IsEnable;
        }
    }
}
