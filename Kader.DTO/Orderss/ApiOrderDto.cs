using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Orderss
{
    public class OrderParam
    {
        public string barcode { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int P_Id { get; set; }
        public int Tax_Id { get; set; }
        public int Unit_Id { get; set; }
        public decimal DiscountParcent { get; set; }
        public decimal DiscountCash { get; set; }
    }

    public class ApiOrderDto
    {
        public List<OrderParam> Param { get; set; }
        public int CustomerID { get; set; }
        public string Discount { get; set; }
        public int PaymentType_ID { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public string CustomerName { get; set; }
        public string UsrRefNbr { get; set; }
        public DateTime DateInvoice { get; set; }
        public string CustomerMobile { get; set; }
    }

}
