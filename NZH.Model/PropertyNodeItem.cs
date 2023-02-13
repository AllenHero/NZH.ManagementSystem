using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZH.Model
{
    public class PropertyNodeItem
    {
        public string Icon { get; set; }
        public string EditIcon { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string id { get; set; }
        public string parentId { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsSelectedItem { get; set; }
        public bool? IsChecked { get; set; }
        public ObservableCollection<PropertyNodeItem> Children { get; set; }
        public PropertyNodeItem()
        {
            Children = new ObservableCollection<PropertyNodeItem>();
        }
        public string Menu { get; set; }
        public string AuNode { get; set; }
        public int MainID { get; set; }

        public PropertyNodeItem Parent { get; set; }

        public int SortCode { get; set; }
    }
}
