using Dhanush_MVC_Test.Models;
using Dhanush_MVC_Test.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhanush_MVC_Test.Generic_Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(IUnitOfWork<InventoryContext> unitOfWork)
            : base(unitOfWork)
        {
        }
        public ProductRepository(InventoryContext context)
            : base(context)
        {
        }

        public Product GetProductByName(string name)
        {
            return Context.Products.Where(x=>x.Name==name).FirstOrDefault();
        }

        public IEnumerable<Product> GetProductByQuantity()
        {
            return Context.Products.Where(x=>x.Quantity==0);
        }
    }
}