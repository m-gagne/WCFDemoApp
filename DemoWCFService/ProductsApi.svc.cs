using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DemoWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DemoWCFService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DemoWCFService.svc or DemoWCFService.svc.cs at the Solution Explorer and start debugging.
    public class ProductsApi : IProductService
    {
        public Product GetProduct(int id)
        {
            var context = new ProductsContext();
            var productEntity = (from p in context.Products where p.Id == id select p).FirstOrDefault();
            if(productEntity != null)
            {
                return TranslateProductEntityToProduct(productEntity);
            }
            else
            {
                throw new Exception("Invalid product id");
            }
            
        }

        public List<Product> GetProducts()
        {
            var context = new ProductsContext();
            var productEntities = (from p in context.Products select p).ToList();
            if (productEntities != null)
            {
                return TranslateProductEntitiesToProducts(productEntities);
            }
            else
            {
                throw new Exception("Invalid product id");
            }

        }

        private Product TranslateProductEntityToProduct(ProductEntity productEntity)
        {
            Product product = new Product();
            product.ID = productEntity.Id;
            product.Name = productEntity.Name;
            product.Price = productEntity.Price;
            return product;
        }

        private List<Product> TranslateProductEntitiesToProducts(List<ProductEntity> productEntities)
        {
            List<Product> products = new List<Product>();

            foreach (var e in productEntities)
            {
                products.Add(TranslateProductEntityToProduct(e));
            }
            return products;
        }
    }
}
