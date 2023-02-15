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

namespace NZH.ManagementSystem.BasePage
{
    /// <summary>
    /// AddRole.xaml 的交互逻辑
    /// </summary>
    public partial class AddRole : NZH.ManagementSystem.Base.BaseWindow
    {
        public AuthorityInfo[] listAuthority = null;//所有功能模块信息
        public List<RoleInfo> listRole = null;//所有角色信息，用来判断添加（修改）的角色名称是否存在
        private bool isUpdate = false;//标志  是否为修改角色
        private RoleInfo _role = null;//修改角色时，存放需要修改的角色信息
        private RoleBusiness roleBusiness = new RoleBusiness();

        public AddRole()
        {
            InitializeComponent();
        }

        public AddRole(RoleInfo role)
        {
            InitializeComponent();
            isUpdate = true;
            _role = role;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listAuthority != null)
                {
                    LoadAuthorityTree();
                }
                else
                {
                    tvFunction.ItemsSource = new List<Model.PropertyNodeItem>();
                }
                if (isUpdate)
                {
                    if (_role != null)
                    {
                        txtRoleName.Text = _role.RoleName;
                        txtDescription.Text = _role.RoleNode;
                        string[] auid = _role.AuthorityID.Split('|');
                        SetChecked(auid.ToList());
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadAuthorityTree()
        {
            List<AuthorityInfo> listAuth = listAuthority.ToList<AuthorityInfo>();
            List<AuthorityInfo> listRoot = new List<AuthorityInfo>();
            for (int i = listAuth.Count - 1; i >= 0; i--)
            {
                if (!string.IsNullOrEmpty(listAuth[i].ParentID) && listAuth[i].ParentID == "0")
                {
                    AuthorityInfo temp = listAuth[i];
                    listAuth.RemoveAt(i);
                    listRoot.Add(temp);
                }
            }
            List<Model.PropertyNodeItem> listNode = new List<Model.PropertyNodeItem>();
            foreach (AuthorityInfo auth in listRoot)
            {
                Model.PropertyNodeItem item = new Model.PropertyNodeItem()
                {
                    id = auth.AuthorityID.ToString(),
                    Name = auth.FunCode,
                    DisplayName = auth.FunName,
                    IsChecked = false,
                    IsExpanded = true
                };
                listNode.Add(item);
                if (listAuth.Count > 0)
                    LoadAuthorityTree(item, listAuth);
            }
            tvFunction.ItemsSource = listNode;
        }

        private void LoadAuthorityTree(Model.PropertyNodeItem parent, List<AuthorityInfo> listAuth)
        {
            List<AuthorityInfo> listRoot = new List<AuthorityInfo>();
            for (int i = listAuth.Count - 1; i >= 0; i--)
            {
                if (listAuth[i].ParentID == parent.Name)
                {
                    AuthorityInfo temp = listAuth[i];
                    listAuth.RemoveAt(i);
                    listRoot.Add(temp);
                }
            }
            foreach (AuthorityInfo auth in listRoot)
            {
                Model.PropertyNodeItem item = new Model.PropertyNodeItem()
                {
                    id = auth.AuthorityID.ToString(),
                    Name = auth.FunCode,
                    DisplayName = auth.FunName,
                    IsChecked = false,
                    IsExpanded = true
                };
                item.Parent = parent;
                parent.Children.Add(item);
                if (listAuth.Count > 0)
                    LoadAuthorityTree(item, listAuth);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isUpdate)
                {
                    //更新角色
                    if (txtRoleName.Text.Trim() == "")
                    {
                        ReMessageBox.Show("请输入角色名称");
                        return;
                    }
                    if (_role.RoleName != txtRoleName.Text.Trim())
                    {
                        RoleInfo temp = listRole.Find(r => r.RoleName == txtRoleName.Text.Trim());
                        if (temp != null)
                        {
                            ReMessageBox.Show("输入的角色名称已存在");
                            return;
                        }
                    }
                    List<Model.PropertyNodeItem> listChecked = GetCheckedNode();
                    if (listChecked.Count == 0)
                    {
                        ReMessageBox.Show("请选择角色权限");
                        return;
                    }
                    string AuID = "";
                    foreach (Model.PropertyNodeItem item in listChecked)
                    {
                        AuID += item.id + "|";
                    }
                    if (AuID != "") AuID = AuID.Substring(0, AuID.Length - 1);
                    RoleInfo tempRole = new RoleInfo();
                    tempRole.AuthorityID = AuID;
                    tempRole.RoleName = txtRoleName.Text.Trim();
                    tempRole.RoleNode = txtDescription.Text.Trim();
                    tempRole.RoleID = _role.RoleID;
                    try
                    {
                        if (roleBusiness.UpdateRole(tempRole) > 0)
                        {
                            //修改完成后，对传进来的对象进行修改，这样的管理界面才能显示正确信息。
                            _role.AuthorityID = AuID;
                            _role.RoleName = txtRoleName.Text.Trim();
                            _role.RoleNode = txtDescription.Text.Trim();
                            ReMessageBox.Show("修改完成");
                        }
                        else
                        {
                            ReMessageBox.Show("修改失败");
                            return;
                        }
                    }
                    catch
                    {
                        ReMessageBox.Show("修改失败，请检查网络后重试");
                        return;
                    }
                }
                else
                {
                    //添加角色
                    _role = new RoleInfo();
                    if (txtRoleName.Text.Trim() == "")
                    {
                        ReMessageBox.Show("请输入角色名称");
                        return;
                    }
                    if (listRole != null)
                    {
                        RoleInfo temp = listRole.Find(r => r.RoleName == txtRoleName.Text.Trim());
                        if (temp != null)
                        {
                            ReMessageBox.Show("输入的角色名称已存在");
                            return;
                        }
                    }
                    List<Model.PropertyNodeItem> listChecked = GetCheckedNode();
                    if (listChecked.Count == 0)
                    {
                        ReMessageBox.Show("请选择角色权限");
                        return;
                    }
                    string AuID = "";
                    foreach (Model.PropertyNodeItem item in listChecked)
                    {
                        AuID += item.id + "|";
                    }
                    if (AuID != "") AuID = AuID.Substring(0, AuID.Length - 1);
                    _role.RoleName = txtRoleName.Text;
                    _role.RoleNode = txtDescription.Text;
                    _role.AuthorityID = AuID;
                    try
                    {
                        if (roleBusiness.AddRole(_role) > 0)
                            ReMessageBox.Show("添加完成");
                        else
                        {
                            ReMessageBox.Show("添加失败");
                            return;
                        }
                    }
                    catch
                    {
                        ReMessageBox.Show("添加数据失败，请检查网络后重试");
                        return;
                    }
                }
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                ReMessageBox.Show("操作失败");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox check = sender as CheckBox;
                if (check != null)
                {
                    List<Model.PropertyNodeItem> listNode = tvFunction.ItemsSource as List<Model.PropertyNodeItem>;
                    string name = check.Tag.ToString();
                    Model.PropertyNodeItem checkedNode = GetNodeItem(listNode, name);
                    if (checkedNode == null) return;
                    CheckTreeDown(checkedNode);
                    CheckTreeUp(checkedNode);
                    tvFunction.ItemsSource = null;
                    tvFunction.ItemsSource = listNode;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private Model.PropertyNodeItem GetNodeItem(List<Model.PropertyNodeItem> listNode, string name)
        {
            if (listNode != null)
            {
                Model.PropertyNodeItem node = listNode.Find(r => r.Name == name);
                if (node != null)
                {
                    return node;
                }
                else
                {
                    foreach (Model.PropertyNodeItem child in listNode)
                    {
                        node = GetNodeItem(child.Children.ToList<Model.PropertyNodeItem>(), name);
                        if (node != null) return node;
                    }
                    return node;
                }
            }
            else return null;
        }

        private void CheckTreeUp(Model.PropertyNodeItem item)
        {
            if (item.IsChecked != false)
            {
                if (item.Parent != null)
                {
                    int count = 0;
                    foreach (Model.PropertyNodeItem sub in item.Parent.Children)
                    {
                        if (sub.IsChecked == true)
                        {
                            count++;
                        }
                    }
                    if (count < item.Parent.Children.Count)
                    {
                        item.Parent.IsChecked = null;
                    }
                    else item.Parent.IsChecked = true;
                    CheckTreeUp(item.Parent);
                }
            }
            else
            {
                if (item.Parent != null)
                {
                    int checkedcount = 0;
                    foreach (Model.PropertyNodeItem child in item.Parent.Children)
                    {
                        if (child.IsChecked == true) checkedcount++;
                    }
                    if (checkedcount > 0 && checkedcount < item.Parent.Children.Count)
                    {
                        item.Parent.IsChecked = null;
                    }
                    else if (checkedcount == item.Parent.Children.Count)
                    {
                        item.Parent.IsChecked = true;
                    }
                    else
                    {
                        item.Parent.IsChecked = false;
                    }
                    CheckTreeUp(item.Parent);
                }
            }
        }

        private void CheckTreeDown(Model.PropertyNodeItem item)
        {
            if (item.Children != null && item.Children.Count > 0)
            {
                foreach (Model.PropertyNodeItem child in item.Children)
                {
                    child.IsChecked = item.IsChecked;
                    CheckTreeDown(child);
                }
            }
        }

        public RoleInfo GetRoleInfo()
        {
            return _role;
        }

        private void ClearChecked()
        {
            List<Model.PropertyNodeItem> listNode = tvFunction.ItemsSource as List<Model.PropertyNodeItem>;
            foreach (Model.PropertyNodeItem item in listNode)
            {
                item.IsChecked = false;
                CheckTreeDown(item);
            }
            tvFunction.ItemsSource = null;
            tvFunction.ItemsSource = listNode;
        }

        private void SetChecked(List<string> listID)
        {
            ClearChecked();
            List<Model.PropertyNodeItem> listNode = tvFunction.ItemsSource as List<Model.PropertyNodeItem>;
            foreach (Model.PropertyNodeItem item in listNode)
            {
                SetChecked(listID, item);
            }
            tvFunction.ItemsSource = null;
            tvFunction.ItemsSource = listNode;
        }

        private void SetChecked(List<string> listID, Model.PropertyNodeItem item)
        {
            if (item.Children.Count == 0 && listID.Contains(item.id))
            {
                //只选中叶子
                item.IsChecked = true;
                CheckTreeUp(item);
                CheckTreeDown(item);
            }
            else
            {
                foreach (Model.PropertyNodeItem child in item.Children)
                {
                    SetChecked(listID, child);
                }
            }
        }

        private List<Model.PropertyNodeItem> GetCheckedNode()
        {
            List<Model.PropertyNodeItem> list = new List<Model.PropertyNodeItem>();
            List<Model.PropertyNodeItem> listNode = tvFunction.ItemsSource as List<Model.PropertyNodeItem>;
            foreach (Model.PropertyNodeItem item in listNode)
            {
                GetCheckedNode(list, item);
            }
            return list;
        }

        private void GetCheckedNode(List<Model.PropertyNodeItem> list, Model.PropertyNodeItem item)
        {
            if (item.IsChecked != false) //&& item.Children.Count == 0)
            {
                //获取所有选中节点（包括非叶子节点）
                list.Add(item);
            }
            foreach (Model.PropertyNodeItem child in item.Children)
            {
                GetCheckedNode(list, child);
            }
        }
    }
}
