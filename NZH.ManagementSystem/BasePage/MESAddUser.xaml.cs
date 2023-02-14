using NZH.Business.BaseData;
using NZH.Common.Extensions;
using NZH.ManagementSystem.Base;
using NZH.ManagementSystem.Control;
using NZH.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NZH.ManagementSystem.BasePage
{
    /// <summary>
    /// AddUser.xaml 的交互逻辑
    /// </summary>
    public partial class MESAddUser : BaseWindow, IDisposable
    {
        public bool IsAdd;
        public MESUser MESUser = new MESUser();
        public event EventHandler AddUserEvent = null;
        public event EventHandler UpdateUserEvent = null;
        ObservableCollection<MESRole> MESRole = new ObservableCollection<MESRole>();
        public MESUser MESUserByUser = new MESUser();
        private BllBaseData bllBaseData = new BllBaseData();

        public MESAddUser()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Loaded += new RoutedEventHandler(AddUser_Loaded);
            btnOk.Click += new RoutedEventHandler(btnOk_Click);
            btnCel.Click += new RoutedEventHandler(btnCel_Click);
            ckbOldPassword.Click += new RoutedEventHandler(ckbOldPassword_Click);
        }

        public void Dispose()
        {
            btnOk.Click -= new RoutedEventHandler(btnOk_Click);
            btnCel.Click -= new RoutedEventHandler(btnCel_Click);
            ckbOldPassword.Click -= new RoutedEventHandler(ckbOldPassword_Click);
            System.GC.Collect();
        }

        void AddUser_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(AddUser_Loaded);
            try
            {
                MESRole = new ObservableCollection<MESRole>(bllBaseData.GetMESRole(MESUser));
            }
            catch
            {
                ReMessageBox.Show("获取数据异常，请检查网络后重试");
                return;
            }
            if (IsAdd)
            {
                ckbOldPassword.IsChecked = false;
                ckbOldPassword.IsEnabled = false;
            }
            else
            {
                ckbOldPassword.IsChecked = true;
                textUserName.IsEnabled = false;
                textPassword.IsEnabled = false;
                textUserName.Text = MESUser.UserName;
                textName.Text = MESUser.PersonName;
                textPassword.Text = MESUser.Password;
            }
            dataGrid.ItemsSource = MESRole;
        }

        void ckbOldPassword_Click(object sender, RoutedEventArgs e)
        {
            if (ckbOldPassword.IsChecked == true)
            {
                textPassword.Text = "";
                textPassword.IsEnabled = false;
            }
            else
                textPassword.IsEnabled = true;
        }

        void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (textUserName.Text + "" == "")
            {
                ReMessageBox.Show("登录名不能为空");
                return;
            }
            else if (Util.FilterSpecial(textUserName.Text))
            {
                ReMessageBox.Show("登录名包含特殊字符，请重新输入");
                textUserName.Focus();
                textUserName.SelectAll();
                return;
            }
            if (textName.Text + "" == "")
            {
                ReMessageBox.Show("姓名不能为空");
                return;
            }
            else if (Util.FilterSpecial(textName.Text))
            {
                ReMessageBox.Show("姓名包含特殊字符，请重新输入");
                textName.Focus();
                textName.SelectAll();
                return;
            }
            if (textPassword.Text + "" == "" && ckbOldPassword.IsChecked == false)
            {
                ReMessageBox.Show("密码不能为空");
                return;
            }
            if (IsAdd)
            {
                MESUser.UserID = Guid.NewGuid();
                MESUser.PersonID = Guid.NewGuid();
            }
            MESUser.UserName = textUserName.Text + "";
            MESUser.PersonName = textName.Text + "";
            if (ckbOldPassword.IsChecked == false)
            {
                MESUser.Password = AuthorizationHelper.EncodePassword(textPassword.Text + "");
                MESUser.ChangePassword = true;
            }
            else
            {
                MESUser.ChangePassword = false;
            }
            MESUser.MESRoles = MESRole.ToList();
            try
            {
                if (IsAdd)
                {
                    if (bllBaseData.CheckAddMESUser(MESUser) < 1)
                    {
                        bllBaseData.AddMESUser(MESUser);
                        if (AddUserEvent != null)
                            AddUserEvent(this, new EventArgs());
                    }
                    else
                    {
                        ReMessageBox.Show("新增用户失败，请检查用户名是否重复");
                        return;
                    }
                }
                else
                {
                    bllBaseData.UpdateMESUser(MESUser);
                    if (UpdateUserEvent != null)
                        UpdateUserEvent(this, new EventArgs());
                }
            }
            catch
            {
                ReMessageBox.Show("获取数据异常，请检查网络后重试");
                return;
            }
            this.Close();
        }

        void btnCel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var item = dataGrid.SelectedItem as MESRole;
            CheckBox ckb = sender as CheckBox;
            if (ckb.IsChecked == true)
                item.IsCheck = true;
            else
                item.IsCheck = false;
        }
    }
}
