using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NZH.ManagementSystem.Control
{
    public class ReMessageBox
    {
        private ReMessageBox() { }
        public static System.Windows.Threading.Dispatcher Dispatcher = null;


        // 跨线程使用之必须
        private delegate MessageWindowResult ShowDelegate(Window owner);
        private static MessageWindowResult Show(Window owner, MessageWindow mbx)
        {
            if (owner != null
                && owner.Dispatcher != null)
            {
                ShowDelegate d = new ShowDelegate(mbx.ShowDialog);
                return (MessageWindowResult)owner.Dispatcher.Invoke(d, owner);
            }

            return mbx.ShowDialog(owner);
        }

        #region "overloads of Show ..."
        /// <summary>
        ///  显示自定义的消息框, 指定owner
        /// </summary>
        /// <param name="owner">宿主窗体</param>
        /// <param name="msg">消息文本</param>
        /// <param name="caption">消息标题</param>
        /// <param name="btns">要在界面上显示的按钮组合</param>
        /// <param name="icon">要在界面上显示的图标</param>
        /// <returns></returns>
        public static MessageWindowResult Show(Window owner, string msg, string caption,
                MessageWindowButtons btns, MessageWindowIcons icon)
        {
            if (Dispatcher != null)
            {
                return (MessageWindowResult)Dispatcher.Invoke(new ShowDelegate((Window owner1) =>
                {
                    MessageWindow mbx = new MessageWindow();
                    mbx.SetIcon(icon);
                    mbx.SetCaption(caption);
                    mbx.SetMsg(msg);
                    mbx.SetButton(btns);
                    MessageWindow.DoBeep(icon);
                    return Show(owner1, mbx);
                }), owner);
            }
            else
            {
                MessageWindow mbx = new MessageWindow();
                mbx.SetIcon(icon);
                mbx.SetCaption(caption);
                mbx.SetMsg(msg);
                mbx.SetButton(btns);
                MessageWindow.DoBeep(icon);
                return Show(owner, mbx);
            }
        }
        /// <summary>
        /// 显示自定义的消息框
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <returns></returns>
        public static MessageWindowResult Show(string msg)
        {
            return Show(null, msg, "提示", MessageWindowButtons.OK, MessageWindowIcons.Info);
        }
        /// <summary>
        /// 显示自定义的消息框
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <param name="caption">消息标题</param>
        /// <returns></returns>
        public static MessageWindowResult Show(string msg, string caption)
        {
            return Show(null, msg, caption, MessageWindowButtons.OK, MessageWindowIcons.Info);
        }
        /// <summary>
        /// 显示自定义的消息框
        /// </summary>
        ///<param name="owner">宿主窗体</param>
        /// <param name="msg">消息文本</param>
        /// <returns></returns>
        public static MessageWindowResult Show(Window owner, string msg)
        {
            return Show(owner, msg, "", MessageWindowButtons.OK, MessageWindowIcons.Info);
        }
        /// <summary>
        /// 显示自定义的消息框
        /// </summary>
        ///<param name="owner">宿主窗体</param>
        /// <param name="msg">消息文本</param>
        /// <param name="caption">消息标题</param>
        /// <returns></returns>
        public static MessageWindowResult Show(Window owner, string msg, string caption)
        {
            return Show(owner, msg, caption, MessageWindowButtons.OK, MessageWindowIcons.Info);
        }
        /// <summary>
        ///  显示自定义的消息框
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <param name="caption">消息标题</param>
        /// <param name="btns">要在界面上显示的按钮组合</param>
        /// <returns></returns>
        public static MessageWindowResult Show(string msg, string caption, MessageWindowButtons btns)
        {
            return Show(null, msg, caption, btns, MessageWindowIcons.Info);
        }
        /// <summary>
        ///  显示自定义的消息框, 指定owner
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <param name="caption">消息标题</param>
        /// <param name="btns">要在界面上显示的按钮组合</param>
        /// <returns></returns>
        public static MessageWindowResult Show(Window onwer, string msg, string caption, MessageWindowButtons btns)
        {
            return Show(onwer, msg, caption, btns, MessageWindowIcons.Info);
        }
        /// <summary>
        ///  显示自定义的消息框, 指定owner
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <param name="caption">消息标题</param>
        /// <param name="btns">要在界面上显示的按钮组合</param>
        /// <param name="icon">要在界面上显示的图标</param>
        /// <returns></returns>
        public static MessageWindowResult Show(string msg, string caption, MessageWindowButtons btns, MessageWindowIcons icon)
        {
            return Show(null, msg, caption, btns, icon);
        }




        ///// <summary>
        ///// 重载0.0: 显示自定义的消息框, 指定owner
        ///// </summary>
        ///// <param name="owner">宿主窗体</param>
        ///// <param name="msg">消息文本</param>
        ///// <param name="caption">消息标题</param>
        ///// <param name="btns">要在界面上显示的按钮组合</param>
        ///// <param name="icon">要在界面上显示的图标</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(
        //        Window owner,
        //        string msg, string caption,
        //        MessageWindowButtons btns, MessageWindowIcons icon)
        //{
        //    using (MessageWindow mbx = new MessageWindow())
        //    {
        //        mbx.SetIcon(icon);
        //        mbx.SetCaption(caption);
        //        mbx.SetMsg(msg);
        //        mbx.SetButton(btns);
        //        MessageWindow.DoBeep(icon);

        //        return Show(owner, mbx);
        //    }
        //}

        ///// <summary>
        ///// 重载0.1: 显示自定义的消息框, 不指定owner
        ///// </summary>
        ///// <param name="msg">消息文本</param>
        ///// <param name="caption">消息标题</param>
        ///// <param name="btns">要在界面上显示的按钮组合</param>
        ///// <param name="icon">要在界面上显示的图标</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(
        //        string msg, string caption,
        //        MessageWindowButtons btns, MessageWindowIcons icon)
        //{
        //    return Show(null, msg, caption, btns, icon);
        //}

        ///// <summary>
        ///// 重载1.1: 使用默认的OK按钮, 指定owner
        ///// </summary>
        ///// <param name="msg">消息文本</param>
        ///// <param name="caption">消息标题</param>
        ///// <param name="icon">要在界面上显示的图标</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(Window owner,
        //    string msg, string caption, MessageWindowIcons icon)
        //{
        //    return Show(owner, msg, caption, MessageWindowButtons.OK, icon);
        //}
        ///// <summary>
        ///// 重载1.2: 使用默认的OK按钮
        ///// </summary>
        ///// <param name="msg">消息文本</param>
        ///// <param name="caption">消息标题</param>
        ///// <param name="icon">要在界面上显示的图标</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(
        //    string msg, string caption, MessageWindowIcons icon)
        //{
        //    return Show(null, msg, caption, MessageWindowButtons.OK, icon);
        //}

        ///// <summary>
        ///// 重载2.1: 使用默认的消息标题, 指定owner
        ///// </summary>
        ///// <param name="msg">消息文本</param>
        ///// <param name="btns">要在界面上显示的按钮组合</param>
        ///// <param name="icon">要在界面上显示的图标</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(Window owner,
        //    string msg, MessageWindowButtons btns, MessageWindowIcons icon)
        //{
        //    return Show(owner, msg, "保存", btns, icon);
        //}
        ///// <summary>
        ///// 重载2.2: 使用默认的消息标题
        ///// </summary>
        ///// <param name="msg">消息文本</param>
        ///// <param name="btns">要在界面上显示的按钮组合</param>
        ///// <param name="icon">要在界面上显示的图标</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(
        //    string msg, MessageWindowButtons btns, MessageWindowIcons icon)
        //{
        //    return Show(null, msg, "提示", btns, icon);
        //}

        ///// <summary>
        ///// 重载3.1: 使用默认的OK按钮, 默认的图标, 指定owner
        ///// </summary>
        ///// <param name="msg">消息文本</param>
        ///// <param name="caption">消息标题</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(Window owner,
        //    string msg, string caption)
        //{
        //    return Show(owner, msg, caption, MessageWindowIcons.Info);
        //}
        ///// <summary>
        ///// 重载3.2: 使用默认的OK按钮, 默认的图标
        ///// </summary>
        ///// <param name="msg">消息文本</param>
        ///// <param name="caption">消息标题</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(string msg, string caption)
        //{
        //    return Show(null, msg, caption, MessageWindowIcons.Info);
        //}

        ///// <summary>
        ///// 重载4.1: 使用默认的OK按钮, 默认的消息标题, 指定owner
        ///// </summary>
        ///// <param name="msg">消息文本</param>
        ///// <param name="icon">要在界面上显示的图标</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(Window owner,
        //    string msg, MessageWindowIcons icon)
        //{
        //    return Show(owner, msg, "提示", icon);
        //}
        ///// <summary>
        ///// 重载4.2: 使用默认的OK按钮, 默认的消息标题
        ///// </summary>
        ///// <param name="msg">消息文本</param>
        ///// <param name="icon">要在界面上显示的图标</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(string msg, MessageWindowIcons icon)
        //{
        //    return Show(null, msg, "提示", icon);
        //}

        ///// <summary>
        ///// 重载5.1: 使用默认的OK按钮, 默认的消息标题, 默认的图标, 指定owner
        ///// </summary>
        ///// <param name="fmt">格式串或字符串</param>
        ///// <param name="args">参数</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(Window owner,
        //    string fmt, params object[] args)
        //{
        //    string msg = string.Format(fmt, args);
        //    return Show(owner, msg, MessageWindowIcons.Info);
        //}
        ///// <summary>
        ///// 重载5.2: 使用默认的OK按钮, 默认的消息标题, 默认的图标
        ///// </summary>
        ///// <param name="fmt">格式串或字符串</param>
        ///// <param name="args">参数</param>
        ///// <returns></returns>
        //public static MessageWindowResult Show(string fmt, params object[] args)
        //{
        //    string msg = string.Format(fmt, args);
        //    // NOTE: here may cause the misunderstandings, if without the cast
        //    return Show((Window)null, msg, MessageWindowIcons.Info);
        //}

        #endregion
    }

    /// <summary>
    /// 指定消息框上出现的图标
    /// </summary>
    public enum MessageWindowIcons
    {
        // NOTE: notice the order, corresponds to their images' order
        /// <summary>
        /// 不要出现任何标志
        /// </summary>
        None = -1,
        /// <summary>
        /// 成功标志
        /// </summary>
        Good = 0,
        /// <summary>
        /// 错误标志
        /// </summary>
        Error,
        /// <summary>
        /// 提示性标志
        /// </summary>
        Info,
        /// <summary>
        /// 询问标志
        /// </summary>
        Question,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
    }

    public enum MessageWindowResult
    {
        //无结果
        None = -1,

        //确定
        OK,

        //是
        Yes,

        //否
        No,

        //取消
        Cancel,

        //重试
        Retry,

        //忽略
        Ignore,

        //终止
        Abort
    }
    // 摘要:
    //     指定若干常数，用以定义 System.Windows.Windows.WTMessageBox 上将显示哪些按钮
    [ComVisible(true)]
    public enum MessageWindowButtons
    {

        // 摘要:
        //     消息框包含“确定”按钮。
        OK = 0,
        //
        // 摘要:
        //     消息框包含“确定”和“取消”按钮。
        OKCancel = 1,
        //
        // 摘要:
        //     消息框包含“中止”、“重试”和“忽略”按钮。
        AbortRetryIgnore = 2,
        //
        // 摘要:
        //     消息框包含“是”、“否”和“取消”按钮。
        YesNoCancel = 3,
        //
        // 摘要:
        //     消息框包含“是”和“否”按钮。
        YesNo = 4,
        //
        // 摘要:
        //     消息框包含“重试”和“取消”按钮。
        RetryCancel = 5,
        //
        // 摘要:
        //     消息框包含“确认”和“复制”按钮。
        OKCopy = 6,
    }

    // 摘要:
    //     指定若干常数，用以定义 System.Windows.Windows.WTMessageBox 上将显示按钮的文本
    [ComVisible(true)]
    internal class MessageWindowButtonText
    {
        // 摘要:
        //     消息框包含“确定”按钮的文本。
        public const string OK = "确认";
        // 摘要:
        //     消息框包含“是”按钮的文本。
        public const string Yes = "是";
        // 摘要:
        //     消息框包含“否”按钮的文本。
        public const string No = "否";
        // 摘要:
        //     消息框包含“取消”按钮的文本。
        public const string Cancel = "取消";
        // 摘要:
        //     消息框包含“重试”按钮的文本。
        public const string Retry = "重试";
        // 摘要:
        //     消息框包含“忽略”按钮的文本。
        public const string Ignore = "忽略";
        // 摘要:
        //     消息框包含“终止”按钮的文本。
        public const string Abort = "终止";
        // 摘要:
        //     消息框包含“复制”按钮的文本。
        public const string Copy = "复制";
    }
}
