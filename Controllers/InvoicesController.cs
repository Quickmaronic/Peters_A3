using Peters_A3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peters_A3.Controllers
{
    public class InvoicesController : Controller
    {
        // GET: Invoices
        /// <summary>
        /// Creates the Invoices Table, with sorting capabilities
        /// </summary>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public ActionResult Invoices(int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<Invoice> allInvoices = context.Invoices.Where(i => i.IsDeleted == false).ToList();

            switch (sortBy)
            {
                case 1:
                    {
                        allInvoices = allInvoices.OrderBy(i => i.InvoiceDate).ToList();
                        break;
                    }
                case 2:
                    {
                        allInvoices = allInvoices.OrderBy(i => i.ProductTotal).ToList();
                        break;
                    }
                case 3:
                    {
                        allInvoices = allInvoices.OrderBy(i => i.SalesTax).ToList();
                        break;
                    }
                case 4:
                    {
                        allInvoices = allInvoices.OrderBy(i => i.Shipping).ToList();
                        break;
                    }
                case 5:
                    {
                        allInvoices = allInvoices.OrderBy(i => i.InvoiceTotal).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        allInvoices = allInvoices.OrderBy(i => i.CustomerID).ToList();
                        break;
                    }
            }

            return View(allInvoices);
        }

        /// <summary>
        /// Creates the Invoice being edited, or sends null if it's a new Invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(int id)
        {
            BooksEntities context = new BooksEntities();
            Invoice invoice = context.Invoices.Where(i => i.InvoiceID == id).FirstOrDefault();
            List<Customer> customers = context.Customers.ToList();
            AddInvoiceModel viewModel = new AddInvoiceModel()
            {
                Customers = customers,
                Invoice = invoice
            };
            return View(viewModel);
        }

        /// <summary>
        /// Using the created Invoice/null Invoice, it either edits an existing Invoice with the edits, or creates a new Invoice, then saves changes
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(AddInvoiceModel model, string customerID)
        {
            Invoice invoice = model.Invoice;
            customerID = customerID.Split(',')[0];
            invoice.CustomerID = Int32.Parse(customerID);
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.Invoices.Where(i => i.InvoiceID == invoice.InvoiceID).Count() > 0)
                {
                    Invoice invoiceSave = context.Invoices.Where(i => i.InvoiceID == invoice.InvoiceID).FirstOrDefault();

                    invoiceSave.CustomerID = invoice.CustomerID;
                    invoiceSave.InvoiceDate = invoice.InvoiceDate;
                    invoiceSave.ProductTotal = invoice.ProductTotal;
                    invoiceSave.SalesTax = invoice.SalesTax;
                    invoiceSave.Shipping = invoice.Shipping;
                    invoiceSave.InvoiceTotal = invoice.InvoiceTotal;
                }
                else
                {
                    context.Invoices.Add(invoice);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Invoices");
        }

        /// <summary>
        /// Creates the Invoice being deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            BooksEntities context = new BooksEntities();
            Invoice invoice = context.Invoices.Where(i => i.InvoiceID == id).FirstOrDefault();
            return View(invoice);
        }

        /// <summary>
        /// Using the created Invoice, it searches for the matching Invoice and changes the IsDeleted property to true
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Invoice invoice)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.Invoices.Where(i => i.InvoiceID == invoice.InvoiceID).Count() > 0)
                {
                    Invoice invoiceDelete = context.Invoices.Where(i => i.InvoiceID == invoice.InvoiceID).FirstOrDefault();
                    invoiceDelete.IsDeleted = true;
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Invoices");
        }
    }
}