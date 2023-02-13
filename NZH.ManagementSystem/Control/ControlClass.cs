using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NZH.ManagementSystem.Control
{
    public class ControlClass
    {
        //判断是否为小数
        public static bool CheckTextDouble(TextBox txtBox)
        {
            double num;
            if (!double.TryParse(txtBox.Text, out num))
            {
                ReMessageBox.Show(txtBox.DataContext + "必须为数字");
                txtBox.Focus();
                txtBox.SelectAll();
                return false;
            }
            return true;
        }

        //判断是否为整数
        public static bool CheckTextInt(TextBox txtBox)
        {
            int num;
            if (!int.TryParse(txtBox.Text, out num))
            {
                ReMessageBox.Show(txtBox.DataContext + "必须为整数");
                txtBox.Focus();
                txtBox.SelectAll();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证字符串中是否包含特殊字符
        /// </summary>
        /// <param name="str">待判定字符串</param>
        /// <returns>是否为特殊字符（true：包含，false：不包含）</returns>
        public static bool FilterSpecial(string str)
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

        /// <summary>
        /// 验证字符串中是否包含特殊字符
        /// </summary>
        /// <param name="str">待判定字符串</param>
        /// <returns>是否为特殊字符（true：包含，false：不包含）</returns>
        public static bool FilterSpecialNoNull(string str)
        {
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
