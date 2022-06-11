using Peters_A3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peters_A3.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        /// <summary>
        /// Creates the Products Table, with sorting capabilities
        /// </summary>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public ActionResult Products(int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<Product> allProducts = context.Products.Where(p => p.IsDeleted == false).ToList();

            switch (sortBy)
            {
                case 1:
                    {
                        allProducts = allProducts.OrderBy(p => p.Description).ToList();
                        break;
                    }
                case 2:
                    {
                        allProducts = allProducts.OrderBy(p => p.UnitPrice).ToList();
                        break;
                    }
                case 3:
                    {
                        allProducts = allProducts.OrderBy(p => p.OnHandQuantity).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        allProducts = allProducts.OrderBy(p => p.ProductCode).ToList();
                        break;
                    }
            }

            return View(allProducts);
        }

        /// <summary>
        /// Creates the Product being edited, or sends null if it's a new Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(string id)
        {
            BooksEntities context = new BooksEntities();
            Product product = context.Products.Where(p => p.ProductCode == id).FirstOrDefault();
            if (product == null)
            {
                product = new Product();
            }
            return View(product);
        }

        /// <summary>
        /// Using the created Product/null Product, it either edits an existing Product with the edits, or creates a new Product, then saves changes
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(Product product)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.Products.Where(p => p.ProductCode == product.ProductCode).Count() > 0)
                {
                    Product productSave = context.Products.Where(p => p.ProductCode == product.ProductCode).FirstOrDefault();

                    productSave.Description = product.Description;
                    productSave.UnitPrice = product.UnitPrice;
                    productSave.OnHandQuantity = product.OnHandQuantity;
                }
                else
                {
                    context.Products.Add(product);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Products");
        }

        /// <summary>
        /// Creates the Product being deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            BooksEntities context = new BooksEntities();
            Product product = context.Products.Where(p => p.ProductCode == id).FirstOrDefault();
            return View(product);
        }

        /// <summary>
        /// Using the created Product, it searches for the matching Product and changes the IsDeleted property to true
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Product product)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.Products.Where(p => p.ProductCode == p.ProductCode).Count() > 0)
                {
                    Product productDelete = context.Products.Where(p => p.ProductCode == product.ProductCode).FirstOrDefault();
                    productDelete.IsDeleted = true;
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Products");
        }
    }
}