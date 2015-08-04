using MvcProject.BuyNet;
using MvcProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class OrderController : Controller
    {
        public static Service1Client client = new Service1Client();
        // GET: Order
     
        public ActionResult CurrentCart()
        {

            return View();
        }
        public ActionResult Remove(int orderProductId)
        {
            ushort RemoveOneProduct = 1;
            OrderProduct orderProduct = client.GetOrderProduct(orderProductId);
            if (orderProduct.Quantity > 0)
            {
                orderProduct.Quantity = (ushort)(orderProduct.Quantity - RemoveOneProduct);
            }


            return RedirectToAction("CurrentCart");
        }
        public ActionResult Add(int orderProductId)
        {
            ushort AddOneProduct = 1;
            OrderProduct orderProduct = client.GetOrderProduct(orderProductId);

            orderProduct.Quantity = (ushort)(orderProduct.Quantity + AddOneProduct);



            return RedirectToAction("CurrentCart");
        }
        [HttpGet]
        public ActionResult Payment(decimal sum)
        {
            Session["sum"] = sum;
            OrderProduct or = (OrderProduct)Session["orderProduct"];

            return View();
        }

        [HttpPost]
        public ActionResult Payment(string card, string cardNumber, string cvc, string firstName, string lastName, string Id, string street, string city, string post_code)
        {
            try
            {
                OrderProduct or = (OrderProduct)Session["orderProduct"];
                Model1.PayOrde(card, cardNumber, cvc, firstName, lastName, Id, street, city, post_code, or);
                ViewBag.Success = "success";
                ViewBag.Message = "Payment aprovved ";

            }
            catch (Exception ex)
            {
                ViewBag.Success = "warning";
                ViewBag.Message = ex.Message;
            }


            return View();
        }
        public ActionResult ShoopingHistory()
        {


            return View();
        }
        public ActionResult Details(int Id)
        {
            Order order = client.GetOrder(Id);
            Session["order"] = order;
            return View();
        }
    }
}

