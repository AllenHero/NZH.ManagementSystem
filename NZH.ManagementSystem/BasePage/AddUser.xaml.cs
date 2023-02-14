using NZH.Business.BaseData;
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
    public partial class AddUser : BaseWindow, IDisposable
    {
        public bool IsAdd;
        public UserInfo UserInfo = new UserInfo();
        public event EventHandler AddUserEvent = null;
        public event EventHandler UpdateUserEvent = null;
        ObservableCollection<RoleModel> RoleModel = new ObservableCollection<RoleModel>();
        ObservableCollection<RoleInfo> RoleInfo = new ObservableCollection<RoleInfo>();
        public UserInfo UserInfoByUser = new UserInfo();
        //所有用户
        public ObservableCollection<UserInfo> AllUserInfo = new ObservableCollection<UserInfo>();
        private BllBaseData bllBaseData = new BllBaseData();

        public AddUser()
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
                UserInfo AllRoleInfo = bllBaseData.GetRoleInfo(new RoleInfo());
                if (AllRoleInfo.Roles != null)
                {
                    foreach (var row in AllRoleInfo.Roles)
                    {
                        RoleModel item = new RoleModel();
                        item.RoleName = row.RoleName;
                        item.RoleID = row.RoleID;
                        item.IsCheck = false;
                        RoleModel.Add(item);
                    }
                }
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
                textUserName.Text = UserInfo.UserName;
                textName.Text = UserInfo.TrueName;
                textPassword.Text = UserInfo.UserPassword;
                try
                {
                    UserInfoByUser = bllBaseData.GetRoleInfoByUser(UserInfo.RoleID + "");
                }
                catch
                {
                    ReMessageBox.Show("获取数据异常，请检查网络后重试");
                    return;
                }

                if (UserInfoByUser.Roles != null)
                {
                    foreach (var row in UserInfoByUser.Roles)
                    {
                        foreach (var item in RoleModel)
                        {
                            if (item.RoleID == row.RoleID)
                                item.IsCheck = true;
                        }
                    }
                }
            }

            dgAddUser.ItemsSource = RoleModel;
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
            else if (FilterSpecial(textUserName.Text))
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
            else if (FilterSpecial(textName.Text))
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
                foreach (var row in AllUserInfo)
                {
                    if (row.UserName + "" == textUserName.Text + "")
                    {
                        ReMessageBox.Show("输入登录名已经存在");
                        return;
                    }
                }
            }

            string RoleID = "";
            string RoleName = "";
            //string ro
            foreach (var row in RoleModel)
            {
                if (row.IsCheck)
                {
                    RoleID += row.RoleID + "|";
                    RoleName += row.RoleName + "|";
                }
            }
            RoleID = RoleID.TrimEnd('|');
            RoleName = RoleName.TrimEnd('|');
            UserInfo.UserName = textUserName.Text + "";
            UserInfo.TrueName = textName.Text + "";
            UserInfo.UserPassword = textPassword.Text + "";
            UserInfo.RoleID = RoleID;
            UserInfo.RoleName = RoleName;
            try
            {
                if (IsAdd)
                {
                    UserInfo.UserUsable = 1;
                    bllBaseData.AddUser(UserInfo);
                }
                else
                    bllBaseData.UpdateUser(UserInfo);
            }
            catch
            {
                ReMessageBox.Show("获取数据异常，请检查网络后重试");
                return;
            }

            if (IsAdd)
            {
                if (AddUserEvent != null)
                    AddUserEvent(this, new EventArgs());
            }
            else
            {
                if (UpdateUserEvent != null)
                    UpdateUserEvent(this, new EventArgs());
            }
            this.Close();
        }

        void btnCel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

            var item = dgAddUser.SelectedItem as RoleModel;
            CheckBox ckb = sender as CheckBox;
            if (ckb.IsChecked == true)
                item.IsCheck = true;
            else
                item.IsCheck = false;
        }

        /// <summary>
        /// 验证字符串中是否包含特殊字符
        /// </summary>
        /// <param name="str">待判定字符串</param>
        /// <returns>是否为特殊字符（true：包含，false：不包含）</returns>
        public bool FilterSpecial(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            string[] aryReg = { "'", "'delete", "?", "<", ">", "%", "\"\"", ",", ".", ">=", "=<", "_", ";", "||", "[", "]", "&", "/", "-", "|", " ", "''" };
            for (int i = 0; i < aryReg.Length; i++)
            {
                if (str.Contains(aryReg[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
