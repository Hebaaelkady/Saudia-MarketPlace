using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.InsideUserView
{
   public class InsideUserViewDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Count1 { get; set; }
        public int? CountStatus14 { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
