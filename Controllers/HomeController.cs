using JMangaHub.Models;
using JMangaHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace JMangaHub.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index(string query = null)
        {
            var availableMangas = _context.Mangas
                .Include(g => g.Creator)
                .Include(g => g.Genre)
                .Where(g => g.DueDateTime > DateTime.Now && !g.IsDeleted);

            if (!String.IsNullOrWhiteSpace(query))
            {
                availableMangas = availableMangas
                    .Where(g =>
                            g.Creator.Name.Contains(query) ||
                            g.Genre.Name.Contains(query) ||
                            g.Vendor.Contains(query));
            }

            var userId = User.Identity.GetUserId();
            var attendances = _context.Attendances
                .Where(a => a.AttendeeId == userId && a.Manga.DueDateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.MangaId);

            var viewModel = new MangaViewModel  
            {
                AvailableMangas = availableMangas,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Available Mangas",
                SearchTerm = query,
                Attendances = attendances
            };

            return View("Index",viewModel);
        }

        //This code is to return image from binary db.Delete if not working.
        //start here.
        public FileContentResult getImg(int id)
        {
            byte[] byteArray = _context.Users.Find(id).ProfilePicture;
            return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
        }
        //ends here.

        public ActionResult Chat()
        {         
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var manga = _context.Mangas
                    .Include(g => g.Creator)
                    .Include(g => g.Genre)
                    .SingleOrDefault(g => g.Id == id);

            if (manga == null)
                return HttpNotFound();

            var viewModel = new MangaDetailsViewModel { Manga = manga };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                viewModel.IsAttending = _context.Attendances
                    .Any(a => a.MangaId == manga.Id && a.AttendeeId == userId);

                viewModel.IsFollowing = _context.Followings
                    .Any(f => f.FolloweeId == manga.CreatorId && f.FollowerId == userId);
            }

            return View("Details", viewModel);
        }
    }

}