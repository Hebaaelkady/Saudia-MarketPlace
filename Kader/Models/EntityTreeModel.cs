using System.Collections.Generic;

namespace Kader.Models
{
    public class EntityTreeModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<EntityTreeModel> children { get; set; }
    }
}