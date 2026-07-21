using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Pages
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageContent { get; set; }
    }
}
