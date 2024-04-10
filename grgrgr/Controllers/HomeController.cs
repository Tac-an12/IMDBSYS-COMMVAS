using System;
using System.Linq;
using System.Web.Mvc;
using grgrgr;
using System.Data.Entity;
using System.IO;
using System.Web;

namespace grgrgr.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string userName = TempData["UserName"]?.ToString();
            string userRole = TempData["UserRole"]?.ToString();

            using (var db = new dybsys1Entities())
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userRole))
                {
                    userName = User.Identity.Name;
                    var user = db.users.FirstOrDefault(u => u.username == userName);
                    userRole = user != null && user.RoleId == 1 ? "Admin" : "User";
                }

                ViewBag.UserName = userName;
                ViewBag.UserRole = userRole;

                var allProducts = db.products.OrderByDescending(p => p.product_id).ToList();
                

                ViewBag.AllProducts = allProducts;
            }

            return View();
        }




        public ActionResult AddProduct()
        {
            return View(new products());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProduct(products product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Get the username of the currently logged-in user
                    string userName = User.Identity.Name;

                    // Get the user from the database based on the username
                    int userId = GetUserIdByUsername(userName);

                    // Set the user_id of the product to the retrieved user ID
                    product.userId = userId;

                    if (image != null && image.ContentLength > 0)
                    {
                        using (var binaryReader = new BinaryReader(image.InputStream))
                        {
                            product.product_img = binaryReader.ReadBytes(image.ContentLength);
                        }
                    }

                    using (var db = new dybsys1Entities())
                    {
                        db.products.Add(product);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                }
            }

            return View("AddProduct", product);
        }



        private bool IsImage(HttpPostedFileBase file)
        {
            try
            {
                string[] allowedFileTypes = { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (file.ContentLength > 0 && allowedFileTypes.Contains(fileExtension))
                {
                    using (var img = System.Drawing.Image.FromStream(file.InputStream))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

       
        private int GetUserIdByUsername(string username)
        {
            using (var db = new dybsys1Entities())
            {
                var user = db.users.FirstOrDefault(u => u.username == username);
                if (user != null)
                {
                    return user.id;
                }
            }

            throw new Exception($"User with username '{username}' not found in database.");
        }
        public static bool IsValidBase64String(string base64String)
        {
            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
