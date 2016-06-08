using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcApplication30.Models;
using UserAuth.Data;

namespace MvcApplication30.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IndexViewModel viewModel = new IndexViewModel();
            viewModel.IsLoggedIn = User.Identity.IsAuthenticated;
            if (viewModel.IsLoggedIn)
            {
                viewModel.Username = User.Identity.Name;
            }
            return View(viewModel);
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(string username, string password)
        {
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            manager.AddUser(username, password);
            return Redirect("/home/login");
        }

        public ActionResult Login()
        {
            LoginViewModel viewModel = new LoginViewModel();
            viewModel.Message = (string)TempData["message"];
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var manager = new UserManager(Properties.Settings.Default.ConStr);
            User user = manager.Login(username, password);
            if (user == null)
            {
                TempData["message"] = "Invalid login";
                return Redirect("/home/login");
            }

            FormsAuthentication.SetAuthCookie(user.Username, true);
            return Redirect("/home/secret");
        }

        [Authorize]
        public ActionResult Secret()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/home/index");
        }

    }
}
