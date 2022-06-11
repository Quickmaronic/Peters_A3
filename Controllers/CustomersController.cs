using Peters_A3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peters_A3.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        /// <summary>
        /// Creates the Customers Table, with sorting capabilities
        /// </summary>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public ActionResult Customers(int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<Customer> allCustomers = context.Customers.Where(c => c.IsDeleted == false).ToList();

            switch (sortBy)
            {
                case 1:
                    {
                        allCustomers = allCustomers.OrderBy(c => c.Address).ToList();
                        break;
                    }
                case 2:
                    {
                        allCustomers = allCustomers.OrderBy(c => c.City).ToList();
                        break;
                    }
                case 3:
                    {
                        allCustomers = allCustomers.OrderBy(c => c.State).ToList();
                        break;
                    }
                case 4:
                    {
                        allCustomers = allCustomers.OrderBy(c => c.ZipCode).ToList(); ;
                        break;
                    }
                case 0:
                default:
                    {
                        allCustomers = allCustomers.OrderBy(c => c.Name).ToList();
                        break;
                    }
            }

            return View(allCustomers);
        }

        /// <summary>
        /// Creates the Customer being edited, or sends null if it's a new Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(int id)
        {
            BooksEntities context = new BooksEntities();
            Customer customer = context.Customers.Where(c => c.CustomerID == id).FirstOrDefault();

            if (customer == null)
            {
                customer = new Customer();
            }
            return View(customer);
        }

        /// <summary>
        /// Using the created Customer/null Customer, it either edits an existing Customer with the edits, or creates a new Customer, then saves changes
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(Customer customer)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.Customers.Where(c => c.CustomerID == customer.CustomerID).Count() > 0)
                {
                    Customer customerSave = context.Customers.Where(c => c.CustomerID == customer.CustomerID).FirstOrDefault();

                    customerSave.Name = customer.Name;
                    customerSave.Address = customer.Address;
                    customerSave.City = customer.City;
                    customerSave.State = customer.State;
                    customerSave.ZipCode = customer.ZipCode;
                }
                else
                {
                    context.Customers.Add(customer);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Customers");
        }

        /// <summary>
        /// Creates the Customer being deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            BooksEntities context = new BooksEntities();
            Customer customer = context.Customers.Where(c => c.CustomerID == id).FirstOrDefault();
            return View(customer);
        }

        /// <summary>
        /// Using the created Customer, it searches for the matching State and changes the IsDeleted property to true
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Customer customer)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.Customers.Where(c => c.CustomerID == customer.CustomerID).Count() > 0)
                {
                    Customer customerDelete = context.Customers.Where(c => c.CustomerID == customer.CustomerID).FirstOrDefault();
                    customerDelete.IsDeleted = true;
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Customers");
        }
    }
}