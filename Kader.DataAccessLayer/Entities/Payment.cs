using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Payment
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public double? Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int PId { get; set; }
        public int? StatusOrder { get; set; }
        public int? StatusOrderItems { get; set; }
        public string Refunded { get; set; }
        public DateTime? RefundedAt { get; set; }
        public string RefundedFormat { get; set; }
        public string InvoiceId { get; set; }
        public string Ip { get; set; }
        public string Type { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string ResponseCode { get; set; }
        public string IssuerName { get; set; }
        public string IssuerCardCategory { get; set; }
        public string GatewayId { get; set; }
        public string Token { get; set; }
        public string ReferenceNumber { get; set; }
        public string IssuerCardType { get; set; }
        public string AuthorizationCode { get; set; }
        public bool? RefundSuccess { get; set; }

        public virtual Orderss IdNavigation { get; set; }
        public virtual AspNetUsers InsertedByNavigation { get; set; }
        public virtual Status StatusOrderItemsNavigation { get; set; }
        public virtual Status StatusOrderNavigation { get; set; }
    }
}
