using Peters_A3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peters_A3.Controllers
{
    public class InvoiceLineItemsController : Controller
    {
        // GET: InvoiceLineItems
        /// <summary>
        /// Creates the InvoiceLineItems Table, with sorting capabilities
        /// </summary>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public ActionResult InvoiceLineItems(int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<InvoiceLineItem> allItems = context.InvoiceLineItems.Where(i => i.IsDeleted == false).ToList();

            switch (sortBy)
            {
                case 1:
                    {
                        allItems = allItems.OrderBy(i => i.ProductCode).ToList();
                        break;
                    }
                case 2:
                    {
                        allItems = allItems.OrderBy(i => i.UnitPrice).ToList();
                        break;
                    }
                case 3:
                    {
                        allItems = allItems.OrderBy(i => i.Quantity).ToList();
                        break;
                    }
                case 4:
                    {
                        allItems = allItems.OrderBy(i => i.ItemTotal).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        allItems = allItems.OrderBy(i => i.InvoiceID).ToList();
                        break;
                    }
            }

            return View(allItems);
        }

        /// <summary>
        /// Creates the InvoiceLineItem being edited, or sends null if it's a new InvoiceLineItem
        /// </summary>
        /// <param name="model"></param>
        /// <param name="invoiceID"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(int id, string code)
        {
            BooksEntities context = new BooksEntities();
            InvoiceLineItem item = context.InvoiceLineItems.Where(i => i.InvoiceID == id && i.ProductCode == code).FirstOrDefault();
            List<Invoice> invoices = context.Invoices.ToList();
            List<Product> products = context.Products.ToList();
            AddInvoiceLineItemsModel viewModel = new AddInvoiceLineItemsModel()
            {
                Invoices = invoices,
                Products = products,
                InvoiceLineItem = item
            };

            return View(viewModel);
        }

        /// <summary>
        /// Using the created InvoiceLineItem/null InvoiceLineItem, it either edits an existing InvoiceLineItem with the edits, or creates a new InvoiceLineItem, then saves changes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(AddInvoiceLineItemsModel model, string invoiceID, string productCode)
        {
            InvoiceLineItem item = model.InvoiceLineItem;
            item.InvoiceID = Int32.Parse(invoiceID);
            item.ProductCode = productCode;
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.InvoiceLineItems.Where(i => i.InvoiceID == item.InvoiceID && i.ProductCode == item.ProductCode).Count() > 0)
                {
                    InvoiceLineItem itemSave = context.InvoiceLineItems.Where(i => i.InvoiceID == item.InvoiceID && i.ProductCode == item.ProductCode).FirstOrDefault();

                    itemSave.UnitPrice = item.UnitPrice;
                    itemSave.Quantity = item.Quantity;
                    itemSave.ItemTotal = item.ItemTotal;
                }
                else
                {
                    context.InvoiceLineItems.Add(item);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("InvoiceLineItems");
        }

        /// <summary>
        /// Creates the InvoiceLineItem being deleted
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id, string code)
        {
            BooksEntities context = new BooksEntities();
            InvoiceLineItem item = context.InvoiceLineItems.Where(i => i.InvoiceID == id && i.ProductCode == code).FirstOrDefault();
            return View(item);
        }

        /// <summary>
        /// Using the created InvoiceLineItem, it searches for the matching InvoiceLineItem and changes the IsDeleted property to true
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(InvoiceLineItem item)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.InvoiceLineItems.Where(i => i.InvoiceID == item.InvoiceID && i.ProductCode == item.ProductCode).Count() > 0)
                {
                    InvoiceLineItem itemDelete = context.InvoiceLineItems.Where(i => i.InvoiceID == item.InvoiceID && i.ProductCode == item.ProductCode).FirstOrDefault();
                    itemDelete.IsDeleted = true;
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("InvoiceLineItems");
        }
    }
}