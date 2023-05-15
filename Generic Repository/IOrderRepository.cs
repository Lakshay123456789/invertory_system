using Dhanush_MVC_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhanush_MVC_Test.Generic_Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Order GetOrderByName(string name);
        IEnumerable<Order> GetOrdersByUserId(int id);
    }
}