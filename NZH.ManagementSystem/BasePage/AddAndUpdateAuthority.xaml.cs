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
    /// AddAndUpdateAuthority.xaml 的交互逻辑
    /// </summary>
    public partial class AddAndUpdateAuthority : BaseWindow, IDisposable
    {
        //获取所有权限
        public ObservableCollection<AuthorityInfo> Info = new ObservableCollection<AuthorityInfo>();
        public AuthorityInfo AuthorityInfo = new AuthorityInfo();
        public string type;
        public string FatherID;
        public event EventHandler AddAuthorityEvent = null;
        public event EventHandler AddSonAuthorityEvent = null;
        public event EventHandler UpdateAuthorityEvent = null;
        string mark;
        private BllBaseData bllBaseData = new BllBaseData();

        public AddAndUpdateAuthority()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(AddAndUpdateAuthority_Loaded);
            btnOk.Click += new RoutedEventHandler(btnOk_Click);
            btnCel.Click += new RoutedEventHandler(btnCel_Click);
        }

        public void Dispose()
        {
            System.GC.Collect();
            this.Loaded -= new RoutedEventHandler(AddAndUpdateAuthority_Loaded);
            btnOk.Click -= new RoutedEventHandler(btnOk_Click);
            btnCel.Click -= new RoutedEventHandler(btnCel_Click);
        }

        void AddAndUpdateAuthority_Loaded(object sender, RoutedEventArgs e)
        {
            if (type == "Edit")
            {
                textName.Text = AuthorityInfo.FunName;
                textMark.Text = AuthorityInfo.AuNode;
                textCode.Text = AuthorityInfo.Menu + "";
                textSortCode.Text = AuthorityInfo.SortCode + "";
                mark = AuthorityInfo.AuNode;
                //是否启用
                if (AuthorityInfo.Enable == 1)
                    ckbEnable.IsChecked = true;
                else
                    ckbEnable.IsChecked = false;
            }
        }

        void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (textName.Text + "" == "")
            {
                ReMessageBox.Show("名称不能为空");
                return;
            }
            if (textMark.Text + "" == "")
            {
                ReMessageBox.Show("标识码不能为空");
                return;
            }
            if (!ControlClass.CheckTextInt(textSortCode))
            {
                return;
            }
            foreach (var row in Info)
            {
                if (type == "Edit")
                {
                    if (row.AuNode + "" == textMark.Text + "" && mark != textMark.Text + "")
                    {
                        ReMessageBox.Show("输入标识码已经存在");
                        return;
                    }
                }
                else
                {
                    if (row.AuNode + "" == textMark.Text + "")
                    {
                        ReMessageBox.Show("输入标识码已经存在");
                        return;
                    }
                }
            }
            AuthorityInfo.FunName = textName.Text;
            AuthorityInfo.AuNode = textMark.Text;
            AuthorityInfo.Menu = textCode.Text;
            AuthorityInfo.SortCode = Convert.ToInt32(textSortCode.Text);
            if (ckbEnable.IsChecked == true)
            {
                AuthorityInfo.Enable = 1;//启用
            }
            else
            {
                AuthorityInfo.Enable = 0;
            }
            if (type == "Add" || type == "AddSon")
            {
                AuthorityInfo.FunCode = Guid.NewGuid().ToString();
                AuthorityInfo.ParentID = FatherID;
            }
            try
            {
                if (type == "Add" || type == "AddSon")
                    bllBaseData.AddAuthority(AuthorityInfo);
                else
                    bllBaseData.UpdateAuthority(AuthorityInfo);
            }
            catch
            {
                ReMessageBox.Show("服务调用异常，请检查网络后重试");
                return;
            }
            if (type == "Add")
            {
                if (AddAuthorityEvent != null)
                    AddAuthorityEvent(this, new EventArgs());
            }
            else if (type == "AddSon")
            {
                if (AddSonAuthorityEvent != null)
                    AddSonAuthorityEvent(this, new EventArgs());
            }
            else if (type == "Edit")
            {
                if (UpdateAuthorityEvent != null)
                    UpdateAuthorityEvent(this, new EventArgs());
            }
            this.Close();
        }

        void btnCel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
