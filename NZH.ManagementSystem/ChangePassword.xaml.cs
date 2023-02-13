using NZH.Business.BaseData;
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
    /// ChangePassword.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePassword : Base.BaseWindow, IDisposable
    {

        public UserInfo UserInfo;
        private BllBaseData bllBaseData = new BllBaseData();

        public ChangePassword()
        {
            InitializeComponent();
            btnOk.Click += new RoutedEventHandler(btnOk_Click);
            btnCel.Click += new RoutedEventHandler(btnCel_Click);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                textPersonID.Text = UserInfo.UserName;
            }
            catch (Exception ex)
            {
                ReMessageBox.Show("修改密码窗口加载失败");
            }
        }

        public void Dispose()
        {
            btnOk.Click -= new RoutedEventHandler(btnOk_Click);
            btnCel.Click -= new RoutedEventHandler(btnCel_Click);
            System.GC.Collect();
        }


        void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (textPersonID.Text + "" == "")
            {
                ReMessageBox.Show("登陆名不能为空");
                return;
            }

            if (textPassword.Password + "" == "")
            {
                ReMessageBox.Show("新密码不能为空");
                return;
            }
            if (textSurePassword.Password + "" == "")
            {
                ReMessageBox.Show("确认密码不能为空");
                return;
            }
            if (textSurePassword.Password + "" != textPassword.Password + "")
            {
                ReMessageBox.Show("新密码与确认密码不相同");
                return;
            }
            if (textOldPassword.Password + "" != UserInfo.UserPassword + "")
            {
                ReMessageBox.Show("原密码错误");
                return;
            }

            try
            {
                int result = bllBaseData.UpdatePassWord(textPersonID.Text + "", textPassword.Password + "");
                if (result == 0)
                {
                    ReMessageBox.Show("修改密码失败");
                    return;
                }
            }
            catch
            {
                ReMessageBox.Show("网络连接失败，请检查网络后重试");
                return;
            }
            UserInfo.UserPassword = textPassword.Password + "";
            this.Close();
            ReMessageBox.Show("修改密码成功");
        }

        void btnCel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
