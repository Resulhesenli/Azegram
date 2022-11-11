using ImageMagick;
using Instagram.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Instagram.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly InstagramContext _sql;
        public HomeController(InstagramContext sql)
        {
            _sql = sql;
        }
        public IActionResult Index()
        {
            ViewBag.Post = _sql.Posts.Include(x => x.PostUser).Include(x => x.Likes).Include(x => x.Comments).ThenInclude(x => x.CommentUser).OrderByDescending(x => x.PostId).Take(20).ToList();
            return View();
        }
        public IActionResult Profil(string UserNickName)
        {
            if (UserNickName == null)
            {
                ViewBag.follower = _sql.Followers.Where(x => x.FollowerToUserId == int.Parse(User.FindFirst("Id").Value) && x.FollowerIsAccept == true).Count();
                ViewBag.following = _sql.Followers.Where(x => x.FollowerFromUserId == int.Parse(User.FindFirst("Id").Value) && x.FollowerIsAccept == true).Count();
                ViewBag.Post = _sql.Posts.Where(x => x.PostUserId == int.Parse(User.FindFirst("Id").Value)).Include(x=>x.Likes).Include(x => x.Comments).Include(x => x.PostUser).OrderByDescending(x => x.PostId).ToList();
                var user = _sql.Users.Include(x => x.FollowerFollowerToUsers.Where(x => x.FollowerFromUserId == int.Parse(User.FindFirst("Id").Value))).SingleOrDefault(x => x.UserId == int.Parse(User.FindFirst("Id").Value));
                return View(user);
            }
            else
            {
                ViewBag.follower = _sql.Followers.Include(x => x.FollowerToUser).Where(x =>x.FollowerToUser.UserNickName == UserNickName && x.FollowerIsAccept ==true).Count();
                ViewBag.following = _sql.Followers.Include(x => x.FollowerFromUser).Where(x => x.FollowerFromUser.UserNickName == UserNickName && x.FollowerIsAccept == true).Count();
                ViewBag.Post = _sql.Posts.Include(x => x.PostUser).Where(x => x.PostUser.UserNickName == UserNickName).Include(x => x.Likes).Include(x => x.Comments).OrderByDescending(x => x.PostId).ToList();
                var user = _sql.Users.Include(x => x.FollowerFollowerToUsers.Where(x => x.FollowerFromUserId == int.Parse(User.FindFirst("Id").Value))).SingleOrDefault(x => x.UserNickName == UserNickName);
                return View(user);
            }
        }
        //var user = _sql.Users.Include(x => x.FollowerFollowerFromUsers.Where(x => x.FollowerIsAccept == true && x.FollowerFromUserId == int.Parse(User.FindFirst("Id").Value))).Include(x => x.FollowerFollowerToUsers.Where(x => x.FollowerToUserId == int.Parse(User.FindFirst("Id").Value) && x.FollowerIsAccept == true)).SingleOrDefault(x => x.UserId == int.Parse(User.FindFirst("Id").Value));
        public IActionResult FollowerRequest()
        {
            ViewBag.Followers = _sql.Followers.Include(x => x.FollowerFromUser).Where(x => x.FollowerToUserId == int.Parse(User.FindFirst("Id").Value) && x.FollowerIsAccept == false).ToList();
            return View();
        }
        public IActionResult EditProfile()
        {
            var a = _sql.Users.SingleOrDefault(x => x.UserId == int.Parse(User.FindFirst("Id").Value));
            return View(a);
        }
        public IActionResult UpdateProfil(User user, IFormFile UserProfilePhotoUrl, int id)
        {
            var a = _sql.Users.SingleOrDefault(x => x.UserId == int.Parse(User.FindFirst("Id").Value));
            a.UserName = user.UserName;
            a.UserSurname = user.UserSurname;
            a.UserPassword = user.UserPassword;
            a.UserBio = user.UserBio;
            a.UserNickName = user.UserNickName;
            a.UserPassword = user.UserPassword;
            if (UserProfilePhotoUrl != null)
            {
                string photoname = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(UserProfilePhotoUrl.FileName);

                using (Stream fileStream = new FileStream("wwwroot/img/" + photoname, FileMode.Create))
                {
                    UserProfilePhotoUrl.CopyTo(fileStream);
                }

                a.UserProfilePhotoUrl = photoname;
            }
            _sql.SaveChanges();
            return RedirectToAction("Profil", new { id = int.Parse(User.FindFirst("Id").Value) });
        }
        public IActionResult Paylasim()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Paylasim(Post post, IFormFile PostPhotoUrl)
        {
            string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(PostPhotoUrl.FileName);
            using (Stream stream = new FileStream("wwwroot/img/" + filename, FileMode.Create))
            {
                PostPhotoUrl.CopyTo(stream);
            }
            using (IMagickImage magickImage = new MagickImage("wwwroot/img/" + filename))
            {
                magickImage.Quality = 10;
                magickImage.Write("wwwroot/img/small/" + filename);
            }
            post.PostUserId = int.Parse(User.FindFirst("Id").Value);
            post.PostPhotoUrl = filename;
            post.PostWriteDate = DateTime.Now;
            _sql.Posts.Add(post);
            _sql.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult InstaPost(int id)
        {
            var a = _sql.Posts.Include(x => x.Comments).Include(x => x.Likes).Include(x => x.PostUser).SingleOrDefault(x => x.PostId == id);
            ViewBag.Comment = _sql.Comments.Where(x => x.CommentPostId == id).Include(x => x.CommentUser).ToList();
            return View(a);
        }
        [HttpPost]
        public IActionResult InstaPost(Comment comment)
        {
            _sql.Comments.Add(comment);
            _sql.SaveChanges();
            return RedirectToAction("InstaPost");
        }
       
        [HttpPost]
        public Comment AddComment(Comment comment, int id)
        {
            comment.CommentUserId = int.Parse(User.FindFirst("Id").Value);
            comment.CommentPostId = id;
            _sql.Comments.Add(comment);
            _sql.SaveChanges();
            return comment;
        }

        public void DeleteComment(int id)
        {
            var a = _sql.Comments.SingleOrDefault(x => x.CommentId == id);
            _sql.Comments.Remove(a);
            _sql.SaveChanges();
        }
        public IActionResult DeletePost(int id)
        {
            var a = _sql.Posts.SingleOrDefault(x => x.PostId == id);
            _sql.Posts.Remove(a);
            _sql.SaveChanges();
            return RedirectToAction("Index");
        }
        public void PostLike(int id)
        {
            var oldcardlike = _sql.Likes.SingleOrDefault(x => x.LikePostId == id && x.LikeUserId == int.Parse(User.FindFirst("Id").Value));
            if (oldcardlike != null)
            {
                oldcardlike.IsLike = !oldcardlike.IsLike;
            }
            else
            {
                Like like = new Like();
                like.LikeUserId = int.Parse(User.FindFirst("Id").Value);
                like.IsLike = true;
                like.LikePostId = id;
                _sql.Likes.Add(like);
            }
            _sql.SaveChanges();
        }
        
        public void SendFollow(int id)
        {
            Follower follower = new Follower();
            follower.FollowerToUserId = id;
            follower.FollowerFromUserId = int.Parse(User.FindFirst("Id").Value);
            follower.FollowerIsAccept = false;
            _sql.Followers.Add(follower);
            _sql.SaveChanges();
        }
        public void RemoveFollow(int id)
        {
            Follower follower = _sql.Followers.SingleOrDefault(x => x.FollowerToUserId == id && x.FollowerFromUserId == int.Parse(User.FindFirst("Id").Value));
            _sql.Followers.Remove(follower);
            _sql.SaveChanges();
        }
        public void FollowDelete(int id)
        {
            Follower follower = _sql.Followers.SingleOrDefault(x => x.FollowerFromUserId == id && x.FollowerToUserId == int.Parse(User.FindFirst("Id").Value));
            _sql.Followers.Remove(follower);
            _sql.SaveChanges();
        }
        public void AcceptFollow(int id)
        {
            Follower follower = _sql.Followers.SingleOrDefault(x => x.FollowerFromUserId == id && x.FollowerToUserId == int.Parse(User.FindFirst("Id").Value));
            follower.FollowerIsAccept = true;
            _sql.SaveChanges();
        }
        public List<User> Search(string q)
        {
            return _sql.Users.Where(x => x.UserNickName.ToLower().Contains(q.ToLower()) || x.UserName.ToLower().Contains(q.ToLower()) || x.UserSurname.ToLower().Contains(q.ToLower()) || x.UserProfilePhotoUrl.Contains(q)).ToList();
        }
        public IActionResult FriendsPosts()
        {
            var u = _sql.Followers.Where(x => x.FollowerFromUserId == int.Parse(User.FindFirst("Id").Value) && x.FollowerIsAccept == true).Select(x => x.FollowerToUser).ToList();
            List<Post> posts = new List<Post>();
            foreach (var item in u)
            {
                posts.AddRange(_sql.Posts.Include(x => x.PostUser).Include(x => x.Comments).ThenInclude(x => x.CommentUser).Include(x => x.Likes).Where(x => x.PostUser.UserId == item.UserId).ToList());
            }
            return View(posts.OrderByDescending(x =>x.PostId).ToList());
        }
        public IActionResult Following(string UserNickName)
        {
            ViewBag.Followers = _sql.Followers.Include(x => x.FollowerToUser).Where(x => x.FollowerFromUser.UserNickName == UserNickName && x.FollowerIsAccept == true).ToList();
            return View();
        }
        public IActionResult Followers(string UserNickName)
        {
            ViewBag.Followers = _sql.Followers.Include(x => x.FollowerFromUser).Where(x => x.FollowerToUser.UserNickName == UserNickName && x.FollowerIsAccept == true).ToList();
            return View();
        }
    }
}
//.Include(x => x.FollowerFromUser).Where(x => x.FollowerToUserId == x.FollowerToUserId)