using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace WeddingPlanner.Controllers
{
    public class WeddingController : Controller
    {
        private WeddingContext context;
        public static PasswordHasher<User> RegisterHasher = new PasswordHasher<User>();
        public static PasswordHasher<LoginUser> LoginHasher = new PasswordHasher<LoginUser>();

        public WeddingController(WeddingContext wc)
        {
            context = wc;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(User u)
        {
            User userInDb = context.GetUserByEmail(u.Email);
            if (userInDb != null)
            {
                ModelState.AddModelError("Email", "Email already in use!");
            }
            if (ModelState.IsValid)
            {
                u.Password = RegisterHasher.HashPassword(u, u.Password);
                int UserId = context.Create(u);
                HttpContext.Session.SetInt32("UserId", UserId);
                return Redirect("/events");
            }
            return View("Index");
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginUser u)
        {
            User userInDb = context.GetUserByEmail(u.LoginEmail);
            if (userInDb == null)
            {
                ModelState.AddModelError("LoginEmail", "Unknown Email!");
            }
            if (ModelState.IsValid)
            {
                var result = LoginHasher.VerifyHashedPassword(u, userInDb.Password, u.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Incorrect!");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    return Redirect("/events");
                }
            }
            return View("Index");
        }

        [Route("events")]
        [HttpGet]
        public IActionResult Events(Wedding w)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return Redirect("/");
            }
            else
            {
                ViewBag.User = context.GetUserById((int)UserId);
                ViewBag.Events = context.Weddings.Include(wed => wed.Event).ToList();
                return View();
            }
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [Route("events/new")]
        [HttpGet]
        public IActionResult NewEvents()
        {
            return View();
        }

        [Route("event")]
        [HttpPost]
        public IActionResult CreateEvent(Wedding w)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(ModelState.IsValid)
            {
                w.PlannerId = (int) UserId;
                context.Create(w);
            }
            return Redirect("/events");
        }

        [Route("join/{WeddingId}")]
        [HttpGet]
        public IActionResult RSVP(int WeddingId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            Guest g = new Guest();
            g.WeddingId = WeddingId;
            g.UserId = (int)UserId;
            context.Create(g);
            return Redirect("/events");
        }

        [Route("leave/{WeddingId}")]
        [HttpGet]
        public IActionResult UnRSVP(int WeddingId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            context.Remove(WeddingId, (int)UserId);
            return Redirect("/events");
        }

        [Route("delete/{WeddingId}")]
        [HttpPost]
        public IActionResult Delete(int WeddingId)
        {
            context.Remove(WeddingId);
            return Redirect("/events");
        }

    }
}
