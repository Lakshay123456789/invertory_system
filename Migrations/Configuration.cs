namespace Dhanush_MVC_Test.Migrations
{
    using Dhanush_MVC_Test.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Dhanush_MVC_Test.Models.InventoryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Dhanush_MVC_Test.Models.InventoryContext context)
        {
            User admin = new User() { UserName = "admin", Email = "admin@123.gmail.com", Password = "admin@123", ConfirmPassword = "admin@123", Role = "admin" };
            User salesman = new User() { UserName = "salesman", Email = "salesman@123.gmail.com", Password = "salesman@123", ConfirmPassword = "salesman@123", Role = "salesman" };
            context.Users.AddOrUpdate(x => x.UserName, admin);
            context.Users.AddOrUpdate(x => x.UserName, salesman);
        }
    }
}
