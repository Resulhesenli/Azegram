using Instagram.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace Instagram.Controllers
{
    public class UserController : Controller
    {
        private readonly InstagramContext _sql;
        public UserController(InstagramContext sql)
        {
            _sql = sql;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Registr()
        {
            ViewBag.Genders = new SelectList(_sql.Genders.ToList(), "GenderId", "GenderName");
            return View();
        }
        [HttpPost]
        public IActionResult Registr(User u)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (u.UserGenderId == 1)
            {
                u.UserProfilePhotoUrl = "Anonim.png";
            }
            else
            {
                u.UserProfilePhotoUrl = "famale.png";
            }
            u.UserBio = "...";
            _sql.Users.Add(u);
            _sql.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //public IActionResult SignUp(User newuser)
        //{
        //    var user = _sql.Users.Any(x => x.UserNickName == newuser.UserNickName);
        //    if(!user)
        //    {

        //        _sql.Add(newuser);
        //        _sql.SaveChanges();
        //        return RedirectToAction("Login");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("UserNickName", "Bu istifadəci adı artıq mövcuddur");
        //        return View("Registration");
        //    }
        //}
        [HttpPost]
        public IActionResult Login(User u)
        {
            var getUser = _sql.Users.SingleOrDefault(x => x.UserNickName == u.UserNickName && x.UserPassword == u.UserPassword);
            if (getUser != null)
            {
                var claims = new List<Claim>
                {   new Claim("Id", getUser.UserId.ToString()),
                    new Claim(ClaimTypes.Uri,getUser.UserProfilePhotoUrl),
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var princitial = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, princitial, props).Wait();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("Login", "User");
        }
    }
}
