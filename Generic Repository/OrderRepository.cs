using Dhanush_MVC_Test.Models;
using Dhanush_MVC_Test.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhanush_MVC_Test.Generic_Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(IUnitOfWork<InventoryContext> unitOfWork)
            : base(unitOfWork)
        {
        }
        public OrderRepository(InventoryContext context)
            : base(context)
        {
        }

        public Order GetOrderByName(string name)
        {
            return Context.Orders.Where(x=>x.Name == name).FirstOrDefault();
        }

        public IEnumerable<Order> GetOrdersByUserId(int id)
        {
            return Context.Orders.Where(x=>x.UserId==id);
        }
    }
}