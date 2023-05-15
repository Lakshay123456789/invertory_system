using Dhanush_MVC_Test.Generic_Repository;
using Dhanush_MVC_Test.Models;
using Dhanush_MVC_Test.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Dhanush_MVC_Test.Controllers
{
   
    public class AuthenticationController : Controller
    {
        private UnitOfWork<InventoryContext> unitOfWork = new UnitOfWork<InventoryContext>();
        private GenericRepository<User> genericRepository;
        private IUserRepository userRepository;
        public AuthenticationController()
        {
            genericRepository = new GenericRepository<User>(unitOfWork);
            userRepository = new UserRepository(unitOfWork);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                User usercheck = userRepository.GetUsersByUserName(model.UserName);
                if(usercheck != null)
                {
                    if (usercheck.Password == model.Password)
                    {
                        if (usercheck.Role == "admin")
                        {
                            Session["username"] = usercheck.UserName;
                            FormsAuthentication.SetAuthCookie(usercheck.UserName, false);
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (usercheck.Role == "salesman")
                        {
                            Session["Id"] = usercheck.Id;
                            Session["username"] = usercheck.UserName;
                            FormsAuthentication.SetAuthCookie(usercheck.UserName, false);
                            return RedirectToAction("Index", "Salesman");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Could not login because user does not belong to any of the roles");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect Password");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User with the current username does not exist");
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Invalid Credentials");
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Signup(User model)
        {
            if(ModelState.IsValid)
            {
                User emailcheck = userRepository.GetUsersByEmail(model.Email);
                User usernamecheck = userRepository.GetUsersByUserName(model.UserName);
                if(emailcheck == null)
                {
                    if(usernamecheck == null)
                    {
                        genericRepository.Insert(model);
                        unitOfWork.Save();
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username already in use");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email already exists");
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Invalid Details");
            return View(model);
        }
    }
}