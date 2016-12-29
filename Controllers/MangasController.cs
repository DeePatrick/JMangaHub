using JMangaHub.Models;
using JMangaHub.Models.ViewModels;
using JMangaHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;


namespace JMangaHub.Controllers
{
    public class MangasController : Controller
    {
        private readonly ApplicationDbContext _context;


        public MangasController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Mine()
        {
            var UserId = User.Identity.GetUserId();
            var mangas = _context.Mangas
                .Where(m =>
                    m.CreatorId == UserId &&
                    m.DueDateTime > DateTime.Now &&
                    !m.IsDeleted)
                .Include(m => m.Genre)
                .ToList();

            return View(mangas);
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var mangas = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Manga)
                .Include(g => g.Creator)
                .Include(g => g.Genre)
                .ToList();

            var attendances = _context.Attendances
                .Where(a => a.AttendeeId == userId && a.Manga.DueDateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.MangaId);

            var viewModel = new MangaViewModel()
            {
                AvailableMangas = mangas,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Manga Subscriptions",
                Attendances = attendances
            };

            return View("Index", viewModel);
        }

        // GET: Mangas
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new MangaFormViewModel
            {
                Heading = "Add a Manga",
                Genres = _context.Genres.ToList()
            };

            return View("MangaForm", viewModel);
        }


        [HttpPost]
        public ActionResult Search(MangaViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }


        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId  = User.Identity.GetUserId();
            var manga = _context.Mangas.Single(m => m.Id == id && m.Creator.Id == userId);

            //now to add elements to be edited
            var viewModel = new MangaFormViewModel
            {
                Genres = _context.Genres.ToList(),

                Heading = "Edit Manga",
                Id = manga.Id,
                MangaName = manga.MangaName,
                Summary = manga.Summary,
                DueDate = manga.DueDateTime.ToString("d MMM yyyy"),
                DueTime = manga.DueDateTime.ToString("HH:mm"),
                Genre = manga.GenreId,
                Vendor = manga.Vendor
            };


            return View("MangaForm", viewModel);
        }



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MangaFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("Create", viewModel);
            }

            var manga = new Manga
            {

                CreatorId = User.Identity.GetUserId(),
                MangaName = viewModel.MangaName,
                Summary = viewModel.Summary,
                DueDateTime = viewModel.GetDueDateTime(),
                GenreId = viewModel.Genre,
                Vendor = viewModel.Vendor
            };

            _context.Mangas.Add(manga);

            try
            {
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }


            return RedirectToAction("Index", "Home");

        }


        [Authorize]
        [HttpPost]
        public ActionResult Update(MangaFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("MangaForm", viewModel);
            }

            var userId = User.Identity.GetUserId();

            var manga = _context.Mangas
                .Single(g => g.Id == viewModel.Id && g.CreatorId == userId);




            _context.SaveChanges();

            return RedirectToAction("Mine", "Mangas");
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var manga  = _context.Mangas 
                    .Include(g => g.Creator)
                    .Include(g => g.Genre)
                    .SingleOrDefault(g => g.Id == id);

            if (manga  == null)
                return HttpNotFound();

            var viewModel = new MangaDetailsViewModel { Manga = manga  };

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

        //desperate measures taken
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

            return View("Index", viewModel);
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

    }


}