using ASPNETWebAppMVCStudentApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class AccountController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tblUser user)
        {
            var obj = db.tblUsers.Where(a => a.Username.Equals(user.Username) && a.Password.Equals(user.Password)).FirstOrDefault();

            if (obj != null)
            {
                Session["UserID"] = obj.UserID.ToString();
                Session["Username"] = obj.Username.ToString();

                return RedirectToAction("Index", "Students");
            }

            ViewBag.Message = "Invalid Username or Password. Please try again or reset your password.";
            return View(user);
        }
    }
}