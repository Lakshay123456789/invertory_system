using Dhanush_MVC_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhanush_MVC_Test.Generic_Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Product GetProductByName(string name);
        IEnumerable<Product> GetProductByQuantity();
    }
}