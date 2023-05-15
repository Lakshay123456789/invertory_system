using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dhanush_MVC_Test.Generic_Repository;
using Dhanush_MVC_Test.Models;
using Dhanush_MVC_Test.UnitOfWork;

namespace Dhanush_MVC_Test.Controllers
{
    public class AdminController : Controller
    {
        private UnitOfWork<InventoryContext> unitOfWork = new UnitOfWork<InventoryContext>();
        private GenericRepository<Product> genericRepository;
        private GenericRepository<User> userRepository;
        private IProductRepository productRepository;
        private IUserRepository uRepository;
        private GenericRepository<Order> orderRepository;
        private IOrderRepository orrepos;
        public AdminController()
        {
            genericRepository = new GenericRepository<Product>(unitOfWork);
            productRepository = new ProductRepository(unitOfWork);
            userRepository = new GenericRepository<User>(unitOfWork);
            uRepository = new UserRepository(unitOfWork);
            orrepos = new OrderRepository(unitOfWork);
            orderRepository = new GenericRepository<Order>(unitOfWork);
        }

        public ActionResult Index()
        {
            if (productRepository.GetProductByQuantity().Count() != 0)
            {
                foreach (var item in productRepository.GetProductByQuantity())
                {
                    productRepository.Delete(item);
                    
                }
                unitOfWork.Save();
            }
            return View(productRepository.GetAll());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productRepository.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

      
        public ActionResult Create()
        {
            return View(new Product());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            try
            {
                unitOfWork.CreateTransaction();
                if (ModelState.IsValid)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("/Content/Images/"), _filename);
                    product.Image = "/Content/Images/" + _filename;
                    
                    genericRepository.Insert(product);
                    unitOfWork.Save();
                    file.SaveAs(path);
                    ModelState.Clear();
                    unitOfWork.Commit();
                    return RedirectToAction("Index");
                }
                return View(product);

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
            }
            return View(product);
            
        }
        public JsonResult GetStates(string id)
        {
            List<SelectListItem> states = new List<SelectListItem>();
            switch (id)
            {
                case "india":
                    states.Add(new SelectListItem { Text = "Select", Value = "select" });
                    states.Add(new SelectListItem { Text = "ANDHRA PRADESH", Value = "andhrapradesh" });
                    states.Add(new SelectListItem { Text = "MAHARASHTRA", Value = "maharashtra" });
                    states.Add(new SelectListItem { Text = "PUNJAB", Value = "punjab" });
                    break;
                case "china":
                    states.Add(new SelectListItem { Text = "Select", Value = "select" });
                    states.Add(new SelectListItem { Text = "Anhui", Value = "anhui" });
                    states.Add(new SelectListItem { Text = "Fujian", Value = "fujin" });
                    states.Add(new SelectListItem { Text = "Gansu", Value = "gansu" });
                    break;
                case "japan":
                    states.Add(new SelectListItem { Text = "Select", Value = "select" });
                    states.Add(new SelectListItem { Text = "Kanto", Value = "kanto" });
                    states.Add(new SelectListItem { Text = "Tohoku", Value = "tohoku" });
                    states.Add(new SelectListItem { Text = "Kyushu", Value = "kyushu" });
                    break;
            }
            return Json(new SelectList(states, "Value", "Text"));
        }

        public JsonResult GetCity(string id)
        {
            List<SelectListItem> City = new List<SelectListItem>();
            switch (id)
            {
                case "maharashtra":
                    City.Add(new SelectListItem { Text = "Select", Value = "select" });
                    City.Add(new SelectListItem { Text = "MUMBAI", Value = "mumbai" });
                    City.Add(new SelectListItem { Text = "PUNE", Value = "pune" });
                    City.Add(new SelectListItem { Text = "KOLHAPUR", Value = "kolhapur" });
                    break;
                case "andhrapradesh":
                    City.Add(new SelectListItem { Text = "Select", Value = "select" });
                    City.Add(new SelectListItem { Text = "Vijayawada", Value = "vijayawada" });
                    City.Add(new SelectListItem { Text = "Vizag", Value = "vizag" });
                    City.Add(new SelectListItem { Text = "Guntur", Value = "guntur" });
                    break;
                case "punjab":
                    City.Add(new SelectListItem { Text = "Select", Value = "select" });
                    City.Add(new SelectListItem { Text = "Phagwara", Value = "phagwara" });
                    break;
                case "anhui":
                    City.Add(new SelectListItem { Text = "Select", Value = "select" });
                    City.Add(new SelectListItem { Text = "Hefei", Value = "hefei" });
                    break;
                case "fujian":
                    City.Add(new SelectListItem { Text = "Select", Value = "select" });
                    City.Add(new SelectListItem { Text = "Fuding", Value = "fuding" });
                    break;
                case "gansu":
                    City.Add(new SelectListItem { Text = "Select", Value = "select" });
                    City.Add(new SelectListItem { Text = "Lognan", Value = "lognan" });
                    break;
                case "kanto":
                    City.Add(new SelectListItem { Text = "Select", Value = "select" });
                    City.Add(new SelectListItem { Text = "Pallet", Value = "pallet" });
                    break;
                case "tohoku":
                    City.Add(new SelectListItem { Text = "Select", Value = "select" });
                    City.Add(new SelectListItem { Text = "Sendai", Value = "sendai" });
                    break;
                case "kyushu":
                    City.Add(new SelectListItem { Text = "Select", Value = "select" });
                    City.Add(new SelectListItem { Text = "Fukuoka", Value = "fukuoka" });
                    break;
            }

            return Json(new SelectList(City, "Value", "Text"));
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productRepository.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            Session["ImgPath"] = product.Image;
            Session["state"] = product.State;
            Session["city"] = product.City;
            return View(product);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase file)
        {
            if(product.State==null && product.City == null)
            {
                product.State = Session["state"].ToString();
                product.City = Session["city"].ToString();
            }
            else if (product.State == null)
            {
                product.State = Session["state"].ToString();
            }
            else if (product.City == null)
            {
                product.State = Session["state"].ToString();
            }
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("/Content/Images/"), _filename);
                    product.Image = "/Content/Images/" + _filename;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        string OldImgPath = Request.MapPath(Session["ImgPath"].ToString());
                        genericRepository.Update(product);
                        unitOfWork.Save();
                        file.SaveAs(path);
                        if (System.IO.File.Exists(OldImgPath))
                        {
                            System.IO.File.Delete(OldImgPath);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid File Type");
                    }
                }
                else if (file == null)
                {
                    product.Image = Session["ImgPath"].ToString();
                    genericRepository.Update(product);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                product.Image = Session["ImgPath"].ToString();
                genericRepository.Update(product);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Please complete the edit");
            return View(product);
        }

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = genericRepository.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = genericRepository.GetById(id);
            genericRepository.Delete(product);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public ActionResult UserOrders()
        {
            List<User> users = new List<User>();
            users.AddRange(userRepository.GetAll().Where(x=>x.Role=="salesman"));
            ViewBag.Users = users;
            return View(users);
        }

        public ActionResult Filter(string filter)
        {
            User u = uRepository.GetUsersByUserName(filter);
            int uid = uRepository.GetUserIdByName(u.UserName);
            List<Order> orders = new List<Order>();
            orders.AddRange(orrepos.GetOrdersByUserId(uid));
            int totalprice = 0;
            int totalquantity = 0;
            foreach (Order order in orders)
            {
                totalprice = totalprice + (order.Price * order.Quantity);
                totalquantity = totalquantity + order.Quantity;
            }
            ViewBag.TotalPrice = totalprice;
            ViewBag.TotalQuantity = totalquantity;
            return View(orders);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
