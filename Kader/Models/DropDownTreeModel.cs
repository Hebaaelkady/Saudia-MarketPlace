using System.Collections.Generic;
using System.ComponentModel;

namespace Kader.Models
{
    public class DropDownTreeModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public List<DropDownTreeModel> subs { get; set; }
        [DefaultValue(true)]
        public bool isSelectable { get; set; }
    }
    public class DropDownModel
    {
        public string id { get; set; }
        public string title { get; set; }
    }
}
