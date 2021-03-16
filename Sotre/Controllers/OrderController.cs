using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        StoreEntities db = new StoreEntities();

        public ActionResult Index()
        {
            var user = Convert.ToInt32(Session["UserId"]);

            var data = db.OrderList.Where(w => w.UserId == user).ToList();

            return View(data);
        }

        [HttpPost]
        public ActionResult Tobeshipped(List<OrderList> data)
        {
            
            var getUserId = data.FirstOrDefault().UserId;

            if (ModelState.IsValid && data != null)
            {
                foreach (var item in data)
                {
                    if (item.IsPaid)
                    {
                        var checkId = db.OrderList.Find(item.Id);

                        checkId.IsPaid = true;
                        checkId.Status = "To be shipped";

                        var insertShipp = db.ShippingOrder.Create();
                        insertShipp.OrderId = item.OrderId;
                        insertShipp.Status = "New";
                        insertShipp.CreatedDateTime = DateTime.Now;
                        db.ShippingOrder.Add(insertShipp);
                        db.SaveChanges();
                    }
                    else
                    {
                        var checkId = db.OrderList.Find(item.Id);

                        checkId.IsPaid = false;
                        checkId.Status = "Payment completed";
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var data = db.OrderList.Find(id);

            return View(data);
        }
    }
}