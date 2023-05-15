using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Dhanush_MVC_Test.Models
{
    public class InventoryContext : DbContext
    {
        public InventoryContext() : base("name=InventoryDB")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }

        public System.Data.Entity.DbSet<Dhanush_MVC_Test.Models.Product> Products { get; set; }
        
    }
}