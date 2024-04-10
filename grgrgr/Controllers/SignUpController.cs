using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using grgrgr;

namespace grgrgr.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUpProcess(string username, string password, int roleId)
        {
            try
            {
                using (var db = new dybsys1Entities())
                {
                    // Check if the role exists
                    var existingRole = db.roles.FirstOrDefault(r => r.RoleId == roleId);

                    if (existingRole == null)
                    {
                        // Role doesn't exist, handle this scenario
                        ViewBag.ErrorMessage = "Selected Role doesn't exist. Please select a valid role.";
                        return View("Index");
                    }

                    var newUser = new users
                    {
                        username = username,
                        password = password,
                        RoleId = roleId // Assign the selected RoleId
                    };

                    db.users.Add(newUser);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Login"); // Redirect to login page after successful sign-up
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the detailed error information for troubleshooting
                string errorMessage = $"Database Update Error: {ex.Message}";

                // Check for inner exceptions and log their messages as well
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner Exception: {ex.InnerException.Message}";
                }

                // Log the complete error message to the ViewBag
                ViewBag.ErrorMessage = errorMessage;

                return View("Index"); // Return to the sign-up page with an error message
            }
            catch (Exception ex)
            {
                // Handle other generic exceptions
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return View("Index"); // Return to the sign-up page with an error message
            }
        }
    }
}
