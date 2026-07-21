using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.UsersStores
{
   public class UsersStoresDto
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int? StoreId { get; set; }
        public string StoreName { get; set; }
        public bool Shipinng { get; set; }
        public bool Storing { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
    }
}
