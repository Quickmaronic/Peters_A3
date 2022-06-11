using Peters_A3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peters_A3.Controllers
{
    public class OrderOptionsController : Controller
    {
        // GET: OrderOptions
        /// <summary>
        /// Creates the OrderOptions Table, with sorting capabilities
        /// </summary>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public ActionResult OrderOptions(int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<OrderOption> allOptions = context.OrderOptions.Where(o => o.IsDeleted == false).ToList();

            switch (sortBy)
            {
                case 1:
                    {
                        allOptions = allOptions.OrderBy(o => o.FirstBookShipCharge).ToList();
                        break;
                    }
                case 2:
                    {
                        allOptions = allOptions.OrderBy(o => o.AdditionalBookShipCharge).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        allOptions = allOptions.OrderBy(o => o.SalesTaxRate).ToList();
                        break;
                    }
            }

            return View(allOptions);
        }

        /// <summary>
        /// Creates the OrderOption being edited, or sends null if it's a new OrderOption
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(decimal id)
        {
            BooksEntities context = new BooksEntities();
            OrderOption option = context.OrderOptions.Where(o => o.SalesTaxRate == id).FirstOrDefault();
            return View(option);
        }

        /// <summary>
        /// Using the created OrderOption/null OrderOption, it either edits an existing OrderOption with the edits, or creates a new OrderOption, then saves changes
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(OrderOption option)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.OrderOptions.Where(o => o.SalesTaxRate == option.SalesTaxRate).Count() > 0)
                {
                    OrderOption optionSave = context.OrderOptions.Where(o => o.SalesTaxRate == option.SalesTaxRate).FirstOrDefault();

                    optionSave.FirstBookShipCharge = option.FirstBookShipCharge;
                    optionSave.AdditionalBookShipCharge = option.AdditionalBookShipCharge;
                }
                else
                {
                    context.OrderOptions.Add(option);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("OrderOptions");
        }

        /// <summary>
        /// Creates the OrderOption being deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(decimal id)
        {
            BooksEntities context = new BooksEntities();
            OrderOption option = context.OrderOptions.Where(o => o.SalesTaxRate == id).FirstOrDefault();
            return View(option);
        }

        /// <summary>
        /// Using the created OrderOption, it searches for the matching OrderOption and changes the IsDeleted property to true
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(OrderOption option)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.OrderOptions.Where(o => o.SalesTaxRate == option.SalesTaxRate).Count() > 0)
                {
                    OrderOption optionDelete = context.OrderOptions.Where(o => o.SalesTaxRate == option.SalesTaxRate).FirstOrDefault();
                    optionDelete.IsDeleted = true;
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("OrderOptions");
        }
    }
}