using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using grgrgr;

namespace grgrgr.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authenticate(string username, string password)
        {
            try
            {
                if (IsValidUser(username, password))
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    using (var db = new dybsys1Entities())
                    {
                        var user = db.users.FirstOrDefault(u => u.username == username);
                        if (user != null)
                        {
                            var userRole = user.RoleId == 1 ? "Admin" : "User";
                            TempData["UserName"] = username;
                            TempData["UserRole"] = userRole;
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid username or password";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while authenticating. Please try again.";
                return View("Index");
            }
        }

        private bool IsValidUser(string username, string password)
        {
            using (var db = new dybsys1Entities())
            {
                var user = db.users.FirstOrDefault(u => u.username == username && u.password == password);
                return user != null;
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}
