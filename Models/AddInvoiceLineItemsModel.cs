using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peters_A3.Models
{
    public class AddInvoiceLineItemsModel
    {
        public List<Invoice> Invoices { get; set; }

        public List<Product> Products { get; set; }

        public InvoiceLineItem InvoiceLineItem { get; set; }
    }
}