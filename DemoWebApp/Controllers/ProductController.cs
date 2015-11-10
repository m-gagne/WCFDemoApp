using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebApp.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            var client = new Products.ProductServiceClient();
            var products = client.GetProducts();
            return View(products);
        }

        // GET: Product
        public ActionResult Details(int id)
        {
            var client = new Products.ProductServiceClient();
            var product = client.GetProduct(id);
            return View(product);
        }
    }
}