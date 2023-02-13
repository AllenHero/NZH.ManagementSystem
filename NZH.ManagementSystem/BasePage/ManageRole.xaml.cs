﻿using NZH.Business.BaseData;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NZH.ManagementSystem.BasePage
{
    /// <summary>
    /// ManageRole.xaml 的交互逻辑
    /// </summary>
    public partial class ManageRole : Page, IDisposable
    {
        private BllBaseData bllBaseData = new BllBaseData();
        public ManageRole()
        {
            InitializeComponent();
            this.Loaded += ManageRole_Loaded;

        }



        public void Dispose()
        {
            GC.Collect();
        }
        private List<RoleInfo> listRole = null;//存放所有角色
        private AuthorityInfo[] listAuthority = null;//存放所有功能模块
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RoleInfo role = dgManageUser.SelectedItem as RoleInfo;
                if (role != null)
                {
                    AddRole updateRole = new  AddRole(role);
                    updateRole.listAuthority = listAuthority;
                    updateRole.listRole = listRole;
                    if (updateRole.ShowDialog() == true)
                    {
                        dgManageUser.ItemsSource = null;
                        dgManageUser.ItemsSource = listRole;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RoleInfo role = dgManageUser.SelectedItem as RoleInfo;
                if (role != null)
                {
                    if (ReMessageBox.Show("确认删除角色 " + role.RoleName, "角色管理", MessageWindowButtons.YesNo) == MessageWindowResult.Yes)
                    {
                        //判断是否可以删除该角色 
                        try
                        {
                            if (bllBaseData.DeleteRole(role.RoleID) > 0)
                                ReMessageBox.Show("删除完成");
                            else
                            {
                                ReMessageBox.Show("删除失败");
                                return;
                            }
                        }
                        catch
                        {
                            ReMessageBox.Show("删除失败，请检查网络后重试");
                            return;
                        }
                        listRole.Remove(role);
                        dgManageUser.ItemsSource = null;
                        dgManageUser.ItemsSource = listRole;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ManageRole_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Loaded -= new RoutedEventHandler(ManageRole_Loaded);
                UserInfo userinfo = bllBaseData.GetAuthorityInfo(new AuthorityInfo());
                if (userinfo != null && userinfo.authority != null)
                {
                    listAuthority = userinfo.authority.ToArray();
                }
                userinfo = null;
                userinfo = bllBaseData.GetRoleInfo(new RoleInfo());
                if (userinfo != null && userinfo.role != null)
                {
                    listRole = userinfo.role.ToList();
                }
                else
                {
                    listRole = new List<RoleInfo>();
                }
                dgManageUser.ItemsSource = listRole;
            }
            catch (Exception ex)
            {
                ReMessageBox.Show("服务调用异常，请检查网络后重试");
            }
        }

        private void BtnAddRole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddRole addRole = new AddRole();
                addRole.listAuthority = listAuthority;
                addRole.listRole = listRole;
                if (addRole.ShowDialog() == true)
                {
                    UserInfo userinfo = bllBaseData.GetRoleInfo(new RoleInfo());
                    if (userinfo != null && userinfo.role != null)
                    {
                        listRole = userinfo.role.ToList();
                    }
                    else
                    {
                        listRole = new List<RoleInfo>();
                    }
                    dgManageUser.ItemsSource = listRole;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}