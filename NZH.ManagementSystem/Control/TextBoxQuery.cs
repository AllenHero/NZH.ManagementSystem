using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NZH.ManagementSystem.Control
{
    public class TextBoxQuery : TextBox
    {
        public new string Text
        {
            get
            { return base.Text.Replace("'", "''"); }
            set
            { base.Text = value.Replace("''", "'"); }
        }
    }
}
