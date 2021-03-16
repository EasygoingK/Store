using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Store.Controllers
{
    public class LoginController : Controller
    {

        StoreEntities db = new StoreEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users users)
        {
            if (ModelState.IsValid)
            {
                var checkUser = db.Users.Where(w => w.UserName == users.UserName && w.Password == users.Password).FirstOrDefault();

                if (checkUser != null)
                {
                    FormsAuthentication.SetAuthCookie(Convert.ToString(checkUser.UserId), false);

                    Session["UserId"] = checkUser.UserId;

                    return RedirectToAction("Index","Order");
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }


    }
}