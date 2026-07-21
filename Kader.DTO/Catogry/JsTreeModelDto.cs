using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Catogry
{
    public class JsTreeModelDto
    {
        public int? ParentId { get; set; }
        public int Id { get; set; }
        public int? Level { get; set; }
        public string Text { get; set; }
        public bool Children { get; set; } // if node has sub-nodes set true or not set false
        public string Href { get; set; }
    }
}
