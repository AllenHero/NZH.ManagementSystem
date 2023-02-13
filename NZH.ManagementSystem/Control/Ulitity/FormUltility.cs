using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.ManagementSystem.Control.Ulitity
{
    class Pair
    {
        public static string ValueName => "Value";
        public static string DisplayName => "Display";
    }

    class Pair<Tvalue, Tdisplay> : Pair
    {
        public Pair() { }

        public Pair(Tvalue value, Tdisplay display)
        {
            this.Value = value;
            this.Display = display;
        }

        public Tvalue Value { set; get; } = default(Tvalue);
        public Tdisplay Display { set; get; } = default(Tdisplay);

    }
    class PairCollection<Tvalue, Tdisplay>
    {
        public PairCollection()
        {
            list = new List<Pair<Tvalue, Tdisplay>>();
        }

        public Pair<Tvalue, Tdisplay> this[int index] => list[index];

        List<Pair<Tvalue, Tdisplay>> list;

        public void Add(Tvalue value, Tdisplay display)
        {
            list.Add(new Pair<Tvalue, Tdisplay>() { Value = value, Display = display });
        }

        public void AddRange(Tvalue[] values, Tdisplay[] displays)
        {
            if (values.Length != displays.Length) throw new ArgumentException("参数长度必须相等");
            for (int i = 0; i < values.Length; i++)
            {
                Add(values[i], displays[i]);
            }
        }

        public List<Pair<Tvalue, Tdisplay>> Datas => list;
    }
}
