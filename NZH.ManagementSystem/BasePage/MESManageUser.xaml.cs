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
    public partial class MESManageUser : Page, IDisposable
    {
        MESUser MESUser = new MESUser();
        ObservableCollection<MESUser> AllMESUser = new ObservableCollection<MESUser>();
        private BllBaseData bllBaseData = new BllBaseData();

        public MESManageUser()
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
            try
            {
                MESUser.UserName = textUserName.Text + "";
                MESUser.PersonName = textTrueName.Text + "";
                AllMESUser = new ObservableCollection<MESUser>(bllBaseData.GetMESUser(MESUser));
                dataGrid.ItemsSource = AllMESUser;
            }
            catch
            {
                ReMessageBox.Show("获取数据异常，请检查网络后重试");
                return;
            }
        }

        #region 按钮点击事件

        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            Query();
        }

        //点击新增事件
        void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            MESAddUser page = new MESAddUser();
            page.IsAdd = true;
            page.AddUserEvent += new EventHandler(page_AddUserEvent);
            page.Show();
        }

        void page_AddUserEvent(object sender, EventArgs e)
        {
            var item = ((MESAddUser)sender).MESUser;
            AllMESUser.Insert(0, item);
            dataGrid.SelectedIndex = 0;
        }

        //修改按钮点击事件
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = dataGrid.SelectedItem as MESUser;
            if (item == null)
                return;
            MESAddUser page = new MESAddUser();
            page.IsAdd = false;
            page.MESUser = item;
            page.UpdateUserEvent += new EventHandler(page_UpdateUserEvent);
            page.Show();
        }

        void page_UpdateUserEvent(object sender, EventArgs e)
        {
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = AllMESUser;
        }

        //删除按钮点击事件
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var item = dataGrid.SelectedItem as MESUser;
            if (ReMessageBox.Show("是否删除用户？", "提示", MessageWindowButtons.OKCancel) == MessageWindowResult.OK)
            {
                if (item == null)
                    return;
                bllBaseData.DeleteMESUser(item);
                AllMESUser.Remove(item);
            }
        }

        #endregion
    }
}
