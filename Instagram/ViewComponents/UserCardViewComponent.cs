using Instagram.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Instagram.ViewComponents
{
    public class UserCardViewComponent : ViewComponent
    {
        private readonly InstagramContext _sql;
        public UserCardViewComponent(InstagramContext sql)
        {
            _sql = sql;
        }
        public IViewComponentResult Invoke()
        {
            User loginuser = _sql.Users.SingleOrDefault(x => x.UserId == int.Parse(HttpContext.User.FindFirst("Id").Value));
            return View(loginuser);
        }
    }
}
