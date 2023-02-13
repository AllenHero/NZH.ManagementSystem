using NZH.ManagementSystem.Base;
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

namespace NZH.ManagementSystem.Control
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow : BaseWindow, IDisposable
    {
        public MessageWindow()
        {
            InitializeComponent();
            dicImages.Clear();
            for (int i = 0; i < 4; i++)
            {
                dicImages.Add(i, "/NZH.ManagementSystem;component/Control/Icon/" + i.ToString() + ".png");
            }
        }
        public void Dispose()
        {
            GC.Collect();
        }

        private MessageWindowResult Result = MessageWindowResult.None;
        private Dictionary<int, string> dicImages = new Dictionary<int, string>();

        public MessageWindowResult ShowDialog(Window owner)
        {
            this.Owner = owner;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.ShowDialog();
            return Result;
        }

        const int DefBtnWidth = 75;  // TODO -- this may not be right
        const int GapTwoBtns = 150;
        const int GapTriBtns = 100;

        #region 窗体出现效果
        private const Int32 AW_HOR_POSITIVE = 0x00000001;    //自左向右显示窗体
        private const Int32 AW_HOR_NEGATIVE = 0x00000002;    //自右向左显示窗体
        private const Int32 AW_VER_POSITIVE = 0x00000004;    //自上而下显示窗体
        private const Int32 AW_VER_NEGATIVE = 0x00000008;    //自下而上显示窗体
        private const Int32 AW_CENTER = 0x00000010;          //窗体向外扩展
        private const Int32 AW_HIDE = 0x00010000;            //隐藏窗体
        private const Int32 AW_ACTIVATE = 0x00020000;        //激活窗体
        private const Int32 AW_SLIDE = 0x00040000;           //使用滚动动画类型
        private const Int32 AW_BLEND = 0x00080000;           //使用淡入效果
        //声明AnimateWindow函数
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        // private string sShowType = "自右向左滑动窗体动画效果";
        #endregion

        internal void SetIcon(MessageWindowIcons icon)
        {
            // set icon
            if (icon == MessageWindowIcons.None || !dicImages.ContainsKey((int)icon))
                this.image.Source = null;
            this.image.Source = new BitmapImage(new Uri(dicImages[(int)icon], UriKind.Relative));
        }
        internal void SetCaption(string caption)
        {
            this.Title = caption;
        }
        internal void SetMsg(string msg)
        {
            //msg = msg.Replace("\n", "").Replace("\r", "");
            this.txtMessage.Text = msg;
            Size size = MeasureString(msg, this.txtMessage);
            if (size.Width > this.Width || size.Height > this.Height)
            {//如果计算出来字符串占用区域的宽度或高度超过窗口的，则每隔n个字符插入一个换行，
                //然后设置窗口SizeToContent属性，让窗口自动调整大小，适应其内容
                string newmsg = "";
                int length = 0;
                foreach (char ch in msg)
                {
                    if (ch > 256)
                    {
                        length += 2;
                    }
                    else length++;
                    if (length > 100)
                    {
                        newmsg += "\n";
                        length = 0;
                    }
                    newmsg += ch;
                }
                txtMessage.Text = newmsg;
                this.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
            }
            //int count = System.Text.Encoding.Default.GetByteCount(msg);

            //int row = count / 24;
            //row += count % 24 == 0 ? 0 : 1;
            //if (row >= 4)
            //{
            //    double w = Math.Sqrt(count * 800);
            //    double h = 0.66 * w;
            //    this.Width = w > this.Width ? w : this.Width;
            //    this.Height = h > this.Height ? h : this.Height;
            //}
            //this.Width = size.Width > this.Width ? size.Width : this.Width;
            //this.Height = (size.Height + 100) > this.Height ? (size.Height + 100) : this.Height;


            //double area = size.Height * size.Width;
            //double h = Math.Sqrt(area / 1.2);
            //double w = 1.4 * h;
            //this.Width = (w + 90) > this.Width ? (w + 90) : this.Width;
            //this.Height = (h + 120) > this.Height ? (h + 120) : this.Height;
        }

        private Size MeasureString(string candidate, TextBlock control)
        {
            var formattedText = new FormattedText(
                candidate,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(control.FontFamily, control.FontStyle, control.FontWeight, control.FontStretch),
                control.FontSize,
                Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }
        // <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="button">需产生的按钮组</param>
        internal void SetButton(MessageWindowButtons buttons)
        {
            double width = buttonPanel.Width;
            double widthspace = 8;
            switch (buttons)
            {
                case MessageWindowButtons.OK:
                    {
                        //“确认”按钮
                        Button btn1 = new Button();
                        btn1.Width = 75;
                        btn1.Height = 23;
                        btn1.Name = "btnOK";
                        btn1.Content = MessageWindowButtonText.OK;
                        btn1.Click += btnOK_Click;
                        btn1.Margin = new Thickness((width - 75) / 2, 0, 0, 0);
                        btn1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn1.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        buttonPanel.Children.Add(btn1);
                    }
                    break;
                case MessageWindowButtons.OKCancel:
                    {
                        //“确认”按钮
                        Button btn1 = new Button();
                        btn1.Width = 75;
                        btn1.Height = 23;
                        btn1.Name = "btnOK";
                        btn1.Content = MessageWindowButtonText.OK;
                        btn1.Click += btnOK_Click;
                        btn1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn1.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn1.Margin = new Thickness((width - 158) / 2, 0, 0, 0);
                        buttonPanel.Children.Add(btn1);

                        //“取消”按钮
                        Button btn2 = new Button();
                        btn2.Width = 75;
                        btn2.Height = 23;
                        btn2.Name = "btnCancel";
                        btn2.Content = MessageWindowButtonText.Cancel;
                        btn2.Click += btnCancel_Click;
                        btn2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn2.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn2.Margin = new Thickness((width - 158) / 2 + 75 + 8, 0, 0, 0);
                        buttonPanel.Children.Add(btn2);

                    }
                    break;
                case MessageWindowButtons.YesNo:
                    {
                        //“是”按钮
                        Button btn1 = new Button();
                        btn1.Width = 75;
                        btn1.Height = 23;
                        btn1.Name = "btnYes";
                        btn1.Content = MessageWindowButtonText.Yes;
                        btn1.Click += btnYes_Click;
                        btn1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn1.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn1.Margin = new Thickness((width - 158) / 2, 0, 0, 0);
                        buttonPanel.Children.Add(btn1);

                        //“否”按钮
                        Button btn2 = new Button();
                        btn2.Width = 75;
                        btn2.Height = 23;
                        btn2.Name = "btnNo";
                        btn2.Content = MessageWindowButtonText.No;
                        btn2.Click += btnNo_Click;
                        btn2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn2.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn2.Margin = new Thickness((width - 158) / 2 + 75 + 8, 0, 0, 0);
                        buttonPanel.Children.Add(btn2);

                    }
                    break;
                case MessageWindowButtons.YesNoCancel:
                    {
                        //“是”按钮
                        Button btn1 = new Button();
                        btn1.Width = 75;
                        btn1.Height = 23;
                        btn1.Name = "btnYes";
                        btn1.Content = MessageWindowButtonText.Yes;
                        btn1.Click += btnYes_Click;
                        btn1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn1.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn1.Margin = new Thickness(0, 0, 0, 0);
                        buttonPanel.Children.Add(btn1);

                        //“否”按钮
                        Button btn2 = new Button();
                        btn2.Width = 75;
                        btn2.Height = 23;
                        btn2.Name = "btnNo";
                        btn2.Content = MessageWindowButtonText.No;
                        btn2.Click += btnNo_Click;
                        btn2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn2.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn2.Margin = new Thickness(83, 0, 0, 0);
                        buttonPanel.Children.Add(btn2);

                        //“取消”按钮
                        Button btn3 = new Button();
                        btn3.Width = 75;
                        btn3.Height = 23;
                        btn3.Name = "btnCancel";
                        btn3.Content = MessageWindowButtonText.Cancel;
                        btn3.Click += btnCancel_Click;
                        btn3.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn3.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn3.Margin = new Thickness(83 + 75 + 8, 0, 0, 0);
                        buttonPanel.Children.Add(btn3);

                    }
                    break;
                case MessageWindowButtons.AbortRetryIgnore:
                    {

                        //“终止”按钮
                        Button btn1 = new Button();
                        btn1.Width = 75;
                        btn1.Height = 23;
                        btn1.Name = "btnAbort";
                        btn1.Content = MessageWindowButtonText.Abort;
                        btn1.Click += btnAbort_Click;
                        btn1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn1.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn1.Margin = new Thickness(0, 0, 0, 0);
                        buttonPanel.Children.Add(btn1);

                        //“重试”按钮
                        Button btn2 = new Button();
                        btn2.Width = 75;
                        btn2.Height = 23;
                        btn2.Name = "btnRetry";
                        btn2.Content = MessageWindowButtonText.Retry;
                        btn2.Click += btnRetry_Click;
                        btn2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn2.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn2.Margin = new Thickness(75 + 8, 0, 0, 0);
                        buttonPanel.Children.Add(btn2);

                        //“忽略”按钮
                        Button btn3 = new Button();
                        btn3.Width = 75;
                        btn3.Height = 23;
                        btn3.Name = "btnIgnore";
                        btn3.Content = MessageWindowButtonText.Ignore;
                        btn3.Click += btnIgnore_Click;
                        btn3.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn3.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn2.Margin = new Thickness(75 + 75 + 8 + 8, 0, 0, 0);
                        buttonPanel.Children.Add(btn3);


                    }
                    break;
                case MessageWindowButtons.RetryCancel:
                    {
                        //“重试”按钮
                        Button btn1 = new Button();
                        btn1.Width = 75;
                        btn1.Height = 23;
                        btn1.Name = "btnRetry";
                        btn1.Content = MessageWindowButtonText.Retry;
                        btn1.Click += btnRetry_Click;
                        btn1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn1.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn1.Margin = new Thickness((width - 158) / 2, 0, 0, 0);
                        buttonPanel.Children.Add(btn1);

                        //“取消”按钮
                        Button btn2 = new Button();
                        btn2.Width = 75;
                        btn2.Height = 23;
                        btn2.Name = "btnCancel";
                        btn2.Content = MessageWindowButtonText.Cancel;
                        btn2.Click += btnCancel_Click;
                        btn2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn2.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn2.Margin = new Thickness((width - 158) / 2 + 75 + 8, 0, 0, 0);
                        buttonPanel.Children.Add(btn2);

                    }
                    break;
                case MessageWindowButtons.OKCopy:
                    {
                        //“确认”按钮
                        Button btn1 = new Button();
                        btn1.Width = 75;
                        btn1.Height = 23;
                        btn1.Name = "btnOK";
                        btn1.Content = MessageWindowButtonText.OK;
                        btn1.Click += btnOK_Click;
                        btn1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn1.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn1.Margin = new Thickness((width - 158) / 2, 0, 0, 0);
                        buttonPanel.Children.Add(btn1);

                        //“复制”按钮
                        Button btn2 = new Button();
                        btn2.Width = 75;
                        btn2.Height = 23;
                        btn2.Name = "btnCopy";
                        btn2.Content = MessageWindowButtonText.Copy;
                        btn2.Click += btnCopy_Click;
                        btn2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn2.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn2.Margin = new Thickness((width - 158) / 2 + 75 + 8, 0, 0, 0);
                        buttonPanel.Children.Add(btn2);

                    }
                    break;
                default:
                    {
                        //“确认”按钮
                        Button btn1 = new Button();
                        btn1.Width = 75;
                        btn1.Height = 23;
                        btn1.Name = "btnOK";
                        btn1.Content = MessageWindowButtonText.OK;
                        btn1.Click += btnOK_Click;
                        btn1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        btn1.SetResourceReference(Button.StyleProperty, "ButtonStlye");
                        btn1.Margin = new Thickness((width - 75) / 2, 0, 0, 0);
                        buttonPanel.Children.Add(btn1);
                    }
                    break;
            }
        }
        private void SetAnimate()
        {
            //string sShowType = "淡入窗体动画效果";
            //switch (sShowType)
            //{
            //    case "自左向右滚动窗体动画效果":
            //        AnimateWindow(this.Handle, 500, AW_HOR_POSITIVE);
            //        break;
            //    case "自左向右滑动窗体动画效果":
            //        AnimateWindow(this.Handle, 500, AW_SLIDE + AW_HOR_POSITIVE);
            //        break;
            //    case "自右向左滚动窗体动画效果":
            //        AnimateWindow(this.Handle, 500, AW_HOR_NEGATIVE);
            //        break;
            //    case "自右向左滑动窗体动画效果":
            //        AnimateWindow(this.Handle, 500, AW_SLIDE + AW_HOR_NEGATIVE);
            //        break;
            //    case "自上向下滚动窗体动画效果":
            //        AnimateWindow(this.Handle, 500, AW_VER_POSITIVE);
            //        break;
            //    case "自上向下滑动窗体动画效果":
            //        AnimateWindow(this.Handle, 500, AW_SLIDE + AW_VER_POSITIVE);
            //        break;
            //    case "自下向上滚动窗体动画效果":
            //        AnimateWindow(this.Handle, 500, AW_VER_NEGATIVE);
            //        break;
            //    case "自下向上滑动窗体动画效果":
            //        AnimateWindow(this.Handle, 500, AW_SLIDE + AW_VER_NEGATIVE);
            //        break;
            //    case "向外扩展窗体动画效果":
            //        AnimateWindow(this.Handle, 500, AW_SLIDE + AW_CENTER);
            //        break;
            //    case "淡入窗体动画效果":
            //        AnimateWindow(this.Handle, 500, AW_BLEND);
            //        break;
            //}
        }

        internal static void DoBeep(MessageWindowIcons icon)
        {
            // Play the associated SystemSound
            switch (icon)
            {
                case MessageWindowIcons.Error:
                    System.Media.SystemSounds.Hand.Play();
                    break;
                case MessageWindowIcons.Good:
                case MessageWindowIcons.Info:
                    System.Media.SystemSounds.Asterisk.Play();
                    break;
                case MessageWindowIcons.Question:
                    System.Media.SystemSounds.Question.Play();
                    break;
                default:
                    System.Media.SystemSounds.Beep.Play();
                    break;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Result = MessageWindowResult.OK;
            this.Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Result = MessageWindowResult.Cancel;
            this.Close();
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Result = MessageWindowResult.OK;
            this.Close();
        }
        private void btnYes_Click(object sender, EventArgs e)
        {
            this.Result = MessageWindowResult.Yes;
            this.Close();
        }
        private void btnRetry_Click(object sender, EventArgs e)
        {
            this.Result = MessageWindowResult.Retry;
            this.Close();
        }
        private void btnIgnore_Click(object sender, EventArgs e)
        {
            this.Result = MessageWindowResult.Ignore;
            this.Close();
        }
        private void btnAbort_Click(object sender, EventArgs e)
        {
            this.Result = MessageWindowResult.Abort;
            this.Close();
        }
        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Result = MessageWindowResult.No;
            this.Close();
        }
        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtMessage.Text);
        }

        /// <summary>
        /// 内容信息过长，自动换行
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string getMessage(string message)
        {
            Char[] newC = message.ToCharArray();
            int j = 0;
            string result = string.Empty;
            for (int i = 0; i < newC.Length; i++)
            {
                if (j == 19)
                {
                    j = 0;
                    result += "\r\n";
                }
                result += newC[i].ToString();
                j++;
            }
            return result;
        }

        private void llb_Info_Click(object sender, EventArgs e)
        {
            //this.Height = pnl_InfoDetail.Visible ? this.Height - 80 : this.Height + 80;
            //pnl_InfoDetail.Height = pnl_InfoDetail.Visible ? pnl_InfoDetail.Height - 40 : pnl_InfoDetail.Height + 40;
            //pnl_InfoDetail.Visible = !pnl_InfoDetail.Visible;

        }

        private void BaseWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.C)
            {
                Clipboard.SetText(txtMessage.Text);
            }
        }
    }
}
