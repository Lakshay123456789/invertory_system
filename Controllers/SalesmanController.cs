using Dhanush_MVC_Test.Generic_Repository;
using Dhanush_MVC_Test.Models;
using Dhanush_MVC_Test.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dhanush_MVC_Test.Controllers
{
    [Authorize(Roles ="salesman")]
    public class SalesmanController : Controller
    {
        private UnitOfWork<InventoryContext> unitOfWork = new UnitOfWork<InventoryContext>();
        private GenericRepository<Product> genericRepository;
        private GenericRepository<Order> orderRepository;
        private IOrderRepository orrepos;
        private IProductRepository productRepository;
        public SalesmanController()
        {
            genericRepository = new GenericRepository<Product>(unitOfWork);
            productRepository = new ProductRepository(unitOfWork);
            orrepos = new OrderRepository(unitOfWork);
            orderRepository = new GenericRepository<Order>(unitOfWork);
        }
        public ActionResult Index()
        {
            
            if (productRepository.GetProductByQuantity().Count()!=0)
            {
                foreach(var item in productRepository.GetProductByQuantity())
                {
                    productRepository.Delete(item);
                    
                }
                unitOfWork.Save();
            }
            return View(productRepository.GetAll());
        }

        public ActionResult Order(int id)
        {
            Product product = productRepository.GetById(id);

            Order order = new Order
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Image = product.Image,
                Quantity = 1,
                Country = product.Country,
                State = product.State,
                City = product.City,
            };
            Session["ImgPath"] = product.Image;
            return View(order);
        }

        [HttpPost]
        public ActionResult Order(Order model)
        {
            try
            {
                unitOfWork.CreateTransaction();
                if (ModelState.IsValid)
                {
                    Order ordercheck = orrepos.GetOrderByName(model.Name);
                    if (ordercheck == null)
                    {
                        model.Image = Session["ImgPath"].ToString();
                        model.UserId = int.Parse(Session["Id"].ToString());
                        int q = model.Quantity;
                        orderRepository.Insert(model);
                        unitOfWork.Save();
                        Product pro = productRepository.GetProductByName(model.Name);
                        pro.Quantity = pro.Quantity - q;
                        genericRepository.Update(pro);
                        unitOfWork.Save();
                        unitOfWork.Commit();
                        return RedirectToAction("Index");
                    }
                    else if (ordercheck != null)
                    {
                        ordercheck.Quantity = ordercheck.Quantity + model.Quantity;
                        orderRepository.Update(ordercheck);
                        unitOfWork.Save();
                        unitOfWork.Commit();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong");
                    unitOfWork.Rollback();
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                unitOfWork.Rollback();
            }
            return View(model);
        }

        public ActionResult ShowOrders()
        {
            List<Order> orders = (List<Order>)orderRepository.GetAll();
            int totalprice = 0;
            int totalquantity = 0;
            foreach(Order order in orders)
            {
                totalprice = totalprice + (order.Price * order.Quantity);
                totalquantity = totalquantity + order.Quantity;
            }
            ViewBag.TotalPrice = totalprice;
            ViewBag.TotalQuantity = totalquantity;
            return View(orders);
        }
    }
}