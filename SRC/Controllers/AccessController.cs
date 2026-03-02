using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using QuanLySanBong.Classes;
using QuanLySanBong.Data;
using QuanLySanBong.Models;

namespace QuanLySanBong.Controllers
{
    public class AccessController : Controller
    {
        private FootballContext db;
        public AccessController(FootballContext context)
        {
            db = context;
        }

        [HttpGet]      
        public ActionResult Login()
        {
            ConstVar.User = new Models.User();
            ViewBag.CurrentUser = ConstVar.User;
            return View();           
        }

        [HttpPost]
        public ActionResult Login(String username, String password)
        {
            ConstVar.User = new Models.User(); ;
            User user = db.User.FirstOrDefault(u => u.Username == username && u.Password == password);
            ViewBag.CurrentUser = user;
            if (user != null)
            {
                ConstVar.User = user;
                return RedirectToAction("ListSanBong", "Home");
            }
            ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại.");
            return View();
        }

        [HttpGet]
        public IActionResult Register(User? user)
        {
            ViewBag.CurrentUser = new User();
            return View(user);
        }


        [HttpPost]
        public IActionResult CreateUser([FromForm] User user)
        {
            //if (db.User.Where(p => p.Username == user.Username).Count() > 0)
            //    ModelState.AddModelError("Username", "Đã tồn tại tên đăng nhập");
            //if (db.User.Where(p => p.PhoneNumber == user.PhoneNumber).Count() > 0)
            //    ModelState.AddModelError("PhoneNumber", "Số điện thoại đã được đăng ký");
            //if (!ModelState.IsValid)
            //{
            //    return View("Register", user);
            //}
            user.LoaiUser = false;
            db.User.Add(user);
            Cart cart = new Cart();
            cart.User = user;
            cart.Total = 0;
            db.Cart.Add(cart);
            db.SaveChanges();
            ViewBag.CurrentUser = user;
            return RedirectToAction("Login");
        }

    }
}
