using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Pages
{
   public class PagesDto
    {
        public int PageId { get; set; } 
        public string PageContent { get; set; }
    }
}
