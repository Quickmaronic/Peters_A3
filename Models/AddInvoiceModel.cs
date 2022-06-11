using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peters_A3.Models
{
    public class AddInvoiceModel
    {
        public List<Customer> Customers { get; set; }

        public Invoice Invoice { get; set; }
    }
}