using Dhanush_MVC_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dhanush_MVC_Test.Generic_Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetUsersByUserName(string username);
        User GetUsersByEmail(string email);
        int GetUserIdByName(string username);
        
    }
}