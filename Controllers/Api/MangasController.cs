using JMangaHub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;


namespace JMangaHub.Controllers.Api
{
    public class MangasController : ApiController
    {
        private ApplicationDbContext _context;

        public MangasController ()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var manga  = _context.Mangas 
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == id && g.CreatorId == userId);

            if (manga.IsDeleted)
                return NotFound();

            manga.Cancel();

            _context.SaveChanges();

            return Ok();
        }
    }
}

