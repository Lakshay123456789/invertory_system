using Dhanush_MVC_Test.Models;
using Dhanush_MVC_Test.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhanush_MVC_Test.Generic_Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork<InventoryContext> unitOfWork)
            : base(unitOfWork)
        {
        }
        public UserRepository(InventoryContext context)
            : base(context)
        {
        }

        public int GetUserIdByName(string username)
        {
            return Context.Users.Where(x=>x.UserName==username).FirstOrDefault().Id;
        }

        public User GetUsersByEmail(string email)
        {
            return Context.Users.Where(u=>u.Email==email).FirstOrDefault();
        }

        public User GetUsersByUserName(string username)
        {
            return Context.Users.Where(u => u.UserName == username).FirstOrDefault();
        }
    }
}