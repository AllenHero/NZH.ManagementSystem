using NZH.Business.BaseData;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NZH.ManagementSystem.BasePage
{
    /// <summary>
    /// ManageUser.xaml 的交互逻辑
    /// </summary>
    public partial class ManageUser : Page, IDisposable
    {
        UserInfo UserInfo = new UserInfo();
        ObservableCollection<UserInfo> AllUserInfo = new ObservableCollection<UserInfo>();
        private UserBusiness userBusiness = new UserBusiness();

        public ManageUser()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ManageUser_Loaded);
            btnAddUser.Click += new RoutedEventHandler(btnAddUser_Click);
            btnQuery.Click += BtnQuery_Click;
        }

        public void Dispose()
        {
            btnAddUser.Click -= new RoutedEventHandler(btnAddUser_Click);
            btnQuery.Click -= BtnQuery_Click;
            System.GC.Collect();
        }

        void ManageUser_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(ManageUser_Loaded);
            Query();
        }

        private void Query()
        {
            AllUserInfo = new ObservableCollection<UserInfo>();
            try
            {
                UserInfo.TrueName = textTrueName.Text + "";
                UserInfo.UserName = textUserName.Text + "";
                UserInfo ui = userBusiness.GetUserInfo(UserInfo);
                foreach (var row in ui.Users)
                {
                    string RoleName = "";
                    foreach (var item in row.Roles)
                    {
                        RoleName += item.RoleName + "|";
                    }
                    RoleName = RoleName.TrimEnd('|');
                    row.RoleName = RoleName;
                    row.UserPassword = "";
                    AllUserInfo.Add(row);
                }
            }
            catch
            {
                ReMessageBox.Show("获取数据异常，请检查网络后重试");
                return;
            }
            dgManageUser.ItemsSource = AllUserInfo;
            if (AllUserInfo.Count > 0)
                dgManageUser.SelectedIndex = 0;
        }


        #region 按钮点击事件

        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            Query();
        }

        //点击新增事件
        void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUser page = new AddUser();
            page.IsAdd = true;
            page.AllUserInfo = AllUserInfo;
            page.AddUserEvent += new EventHandler(page_AddUserEvent);
            page.Show();
        }

        void page_AddUserEvent(object sender, EventArgs e)
        {
            var item = ((AddUser)sender).UserInfo;
            AllUserInfo.Insert(0, item);
            dgManageUser.SelectedIndex = 0;
        }

        //修改按钮点击事件
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = dgManageUser.SelectedItem as UserInfo;
            if (item == null)
                return;
            AddUser page = new AddUser();
            page.IsAdd = false;
            page.UserInfo = item;
            page.AllUserInfo = AllUserInfo;
            page.UpdateUserEvent += new EventHandler(page_UpdateUserEvent);
            page.Show();
        }

        void page_UpdateUserEvent(object sender, EventArgs e)
        {
            dgManageUser.ItemsSource = null;
            dgManageUser.ItemsSource = AllUserInfo;
        }

        //删除按钮点击事件
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var item = dgManageUser.SelectedItem as UserInfo;
            if (item.UserID == 1)
            {
                ReMessageBox.Show("管理员无法删除");
                return;
            }
            if (ReMessageBox.Show("是否删除用户？", "提示", MessageWindowButtons.OKCancel) == MessageWindowResult.OK)
            {
                if (item == null)
                    return;
                userBusiness.DeleteUser(item.UserID);
                AllUserInfo.Remove(item);
            }
        }

        #endregion

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var item = dgManageUser.SelectedItem as UserInfo;
            CheckBox ckb = sender as CheckBox;
            if (ckb.IsChecked == true)
                item.UserUsable = 1;
            else
                item.UserUsable = 0;
            userBusiness.UpdateUser(item);
        }
    }
}
