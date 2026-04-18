using System;
using System.Collections.Generic;

namespace MobileShop.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public decimal TotalAmount { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}