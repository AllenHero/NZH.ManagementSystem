using NZH.Business.BaseData;
using NZH.ManagementSystem.Control;
using NZH.Model;
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
    /// ManageAuthority.xaml 的交互逻辑
    /// </summary>
    public partial class ManageAuthority : Page, IDisposable
    {
        ObservableCollection<AuthorityInfo> Info = new ObservableCollection<AuthorityInfo>();
        ObservableCollection<PropertyNodeItem> itemList = new ObservableCollection<PropertyNodeItem>();
        private BllBaseData bllBaseData = new BllBaseData();

        public ManageAuthority()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ManageAuthority_Loaded);
            btnNode.Click += new RoutedEventHandler(btnNode_Click);
            btnChlidNode.Click += new RoutedEventHandler(btnChlidNode_Click);
            btnEdit.Click += new RoutedEventHandler(btnEdit_Click);
            btnDel.Click += new RoutedEventHandler(btnDel_Click);
            treeview.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(treeview_SelectedItemChanged);
        }

        public void Dispose()
        {
            btnNode.Click -= new RoutedEventHandler(btnNode_Click);
            btnChlidNode.Click -= new RoutedEventHandler(btnChlidNode_Click);
            btnEdit.Click -= new RoutedEventHandler(btnEdit_Click);
            btnDel.Click -= new RoutedEventHandler(btnDel_Click);
            treeview.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(treeview_SelectedItemChanged);
            System.GC.Collect();
        }

        void ManageAuthority_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(ManageAuthority_Loaded);
            AuthorityInfo AuthorityInfo = new AuthorityInfo();
            Info = new ObservableCollection<AuthorityInfo>();
            try
            {
                UserInfo ui = bllBaseData.GetAuthorityInfo(AuthorityInfo);
                if (ui.Authoritys != null)
                {
                    foreach (var row in ui.Authoritys)
                    {
                        Info.Add(row);
                    }
                }
            }
            catch
            {
                ReMessageBox.Show("加载数据异常，请检查网络后重试");
                return;
            }
            loadTree("0");
        }

        #region 绑定树结构

        public void loadTree(string FatherID)
        {
            //加载根节点前先清除Treeview控件项
            treeview.ItemsSource = null;
            foreach (var row in Info)
            {
                if (row.ParentID == FatherID + "")
                {
                    PropertyNodeItem node = new PropertyNodeItem()
                    {
                        Menu = row.Menu + "",
                        DisplayName = row.FunName + "",
                        Name = row.FunName + "",
                        id = row.FunCode,
                        parentId = row.ParentID,
                        IsExpanded = true,
                        AuNode = row.AuNode + "",
                        SortCode = row.SortCode
                    };
                    string id = row.FunCode;
                    string pid = row.ParentID;
                    ForeachPropertyNode(node, id);
                    itemList.Add(node);
                }
            }
            this.treeview.ItemsSource = itemList;
        }

        private void ForeachPropertyNode(PropertyNodeItem node, string pid)
        {
            foreach (var row in Info)
            {
                if (row.ParentID == pid.ToString())
                {
                    string id = row.FunCode;
                    string name = row.FunName + "";
                    string parentId = row.ParentID;
                    PropertyNodeItem childNodeItem = new PropertyNodeItem()
                    {
                        Menu = row.Menu + "",
                        DisplayName = name,
                        Name = name,
                        id = id,
                        parentId = parentId,
                        IsExpanded = false,
                        AuNode = row.AuNode + "",
                        SortCode = row.SortCode,
                        Parent = node
                    };
                    ForeachPropertyNode(childNodeItem, id);
                    node.Children.Add(childNodeItem);
                }
            }
        }

        #endregion

        #region 树结构增删改

        //添加节点
        void btnNode_Click(object sender, RoutedEventArgs e)
        {
            var item = treeview.SelectedItem as PropertyNodeItem;
            AddAndUpdateAuthority page = new AddAndUpdateAuthority();
            if (item == null)
            {
                page.FatherID = "0";
            }
            else
            {
                page.FatherID = item.parentId;
            }
            page.type = "Add";
            page.Info = Info;
            page.AddAuthorityEvent += new EventHandler(page_AddAuthorityEvent);
            page.Show();
        }

        //新增节点操作
        void page_AddAuthorityEvent(object sender, EventArgs e)
        {
            var obj = ((AddAndUpdateAuthority)sender).AuthorityInfo;
            PropertyNodeItem item = new PropertyNodeItem();
            item.id = obj.FunCode;
            item.parentId = obj.ParentID;
            item.Menu = obj.Menu + "";
            item.DisplayName = obj.FunName;
            item.AuNode = obj.AuNode + "";
            item.SortCode = obj.SortCode;
            var row = treeview.SelectedItem as PropertyNodeItem;
            if (row == null)
            {
                itemList.Add(item);
                itemList = new ObservableCollection<PropertyNodeItem>(itemList.OrderBy(v => v.SortCode));
            }
            else
            {
                if (row.Parent == null)
                {
                    itemList.Add(item);
                    itemList = new ObservableCollection<PropertyNodeItem>(itemList.OrderBy(v => v.SortCode));
                }
                else
                {
                    row.IsExpanded = true;
                    ObservableCollection<PropertyNodeItem> Parent = row.Parent.Children;
                    Parent.Add(item);
                    Parent = new ObservableCollection<PropertyNodeItem>(Parent.OrderBy(v => v.SortCode));
                    row.Parent.Children.Clear();
                    foreach (var children in Parent)
                    {
                        row.Parent.Children.Add(children);
                    }
                    item.Parent = row.Parent;
                    item.IsExpanded = true;
                }
            }
            this.treeview.ItemsSource = null;
            this.treeview.ItemsSource = itemList;
            item.IsSelectedItem = true;
            treeview.Focus();
        }

        //添加子节点
        void btnChlidNode_Click(object sender, RoutedEventArgs e)
        {
            //获取当前选中的节点    
            var item = treeview.SelectedItem as PropertyNodeItem;
            if (item == null)
            {
                ReMessageBox.Show("请在添加子节点之前选中一个节点", "提示");
                return;
            }
            AddAndUpdateAuthority page = new AddAndUpdateAuthority();
            page.FatherID = item.id;
            page.type = "AddSon";
            page.Info = Info;
            page.AddSonAuthorityEvent += new EventHandler(page_AddSonAuthorityEvent);
            page.Show();
        }

        //添加子节点操作
        void page_AddSonAuthorityEvent(object sender, EventArgs e)
        {
            var obj = ((AddAndUpdateAuthority)sender).AuthorityInfo;
            PropertyNodeItem item = new PropertyNodeItem();
            item.id = obj.FunCode;
            item.parentId = obj.ParentID;
            item.Menu = obj.Menu + "";
            item.DisplayName = obj.FunName;
            item.AuNode = obj.AuNode + "";
            item.SortCode = obj.SortCode;
            var row = treeview.SelectedItem as PropertyNodeItem;
            if (row != null)
            {
                row.IsExpanded = true;
                ObservableCollection<PropertyNodeItem> Parent = row.Children;
                Parent.Add(item);
                Parent = new ObservableCollection<PropertyNodeItem>(Parent.OrderBy(v => v.SortCode));
                row.Children.Clear();
                foreach (var children in Parent)
                {
                    row.Children.Add(children);
                }
                item.Parent = row;
                item.IsExpanded = true;
            }
            else
            {
                item.Parent = null;
                itemList.Add(item);
                itemList = new ObservableCollection<PropertyNodeItem>(itemList.OrderBy(v => v.SortCode));
            }
            this.treeview.ItemsSource = null;
            this.treeview.ItemsSource = itemList;
            item.IsSelectedItem = true;
            treeview.Focus();
        }

        //编辑节点
        void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            //获取当前选中的节点    
            var row = treeview.SelectedItem as PropertyNodeItem;
            if (row == null)
            {
                ReMessageBox.Show("请在编辑之前选中一个节点", "提示");
                return;
            }
            AuthorityInfo item = new AuthorityInfo();
            item.FunCode = row.id;
            item.ParentID = row.parentId;
            item.Menu = row.Menu;
            item.FunName = row.DisplayName;
            item.AuNode = row.AuNode + "";
            item.SortCode = row.SortCode;
            AddAndUpdateAuthority page = new AddAndUpdateAuthority();
            page.AuthorityInfo = item;
            page.type = "Edit";
            page.Info = Info;
            page.UpdateAuthorityEvent += new EventHandler(page_UpdateAuthorityEvent);
            page.Show();
        }

        void page_UpdateAuthorityEvent(object sender, EventArgs e)
        {
            var row = treeview.SelectedItem as PropertyNodeItem;
            var obj = ((AddAndUpdateAuthority)sender).AuthorityInfo;
            row.DisplayName = obj.FunName;
            row.Menu = obj.Menu;
            row.AuNode = obj.AuNode;
            row.SortCode = obj.SortCode;
            if (row.Parent == null)
            {
                itemList = new ObservableCollection<PropertyNodeItem>(itemList.OrderBy(v => v.SortCode));
            }
            else
            {
                row.IsExpanded = true;
                ObservableCollection<PropertyNodeItem> Parent = row.Parent.Children;
                Parent = new ObservableCollection<PropertyNodeItem>(Parent.OrderBy(v => v.SortCode));
                row.Parent.Children.Clear();
                foreach (var children in Parent)
                {
                    row.Parent.Children.Add(children);
                }
                row.IsExpanded = true;
            }
            this.treeview.ItemsSource = null;
            this.treeview.ItemsSource = itemList;
            row.IsSelectedItem = true;
            treeview.Focus();
        }

        //删除节点
        void btnDel_Click(object sender, RoutedEventArgs e)
        {
            //获取当前选中的节点    
            var item = treeview.SelectedItem as PropertyNodeItem;
            if (item == null)
            {
                ReMessageBox.Show("请在删除之前选中一个节点", "提示");
                return;
            }
            if (ReMessageBox.Show("是否删除该节点？", "提示", MessageWindowButtons.OKCancel) == MessageWindowResult.OK)
            {
                try
                {
                    bllBaseData.DeleteAuthority(item.id);
                }
                catch
                {
                    ReMessageBox.Show("删除数据异常，请检查网络后重试");
                    return;
                }
                DelTreeViewChildren(item);
                this.treeview.ItemsSource = itemList;
                treeview.Focus();
            }
        }

        private void DelTreeViewChildren(PropertyNodeItem node)
        {
            if (node.Children != null)
            {
                for (int i = 0; i < node.Children.Count; i++)
                {
                    DelTreeViewChildren(node.Children[i]);
                    i = i - 1;
                }
            }
            if (node.Parent != null)
                node.Parent.Children.Remove(node);
            else
                itemList.Remove(node);
        }

        #endregion

        //树节点改变
        void treeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = treeview.SelectedItem as PropertyNodeItem;
            if (item == null)
            {
                textName.Text = "";
                textMark.Text = "";
                textCode.Text = "";
                textSortCode.Text = "";
            }
            else
            {
                textName.Text = item.DisplayName;
                textMark.Text = item.AuNode;
                textCode.Text = item.Menu + "";
                textSortCode.Text = item.SortCode + "";
            }
        }
    }
}
