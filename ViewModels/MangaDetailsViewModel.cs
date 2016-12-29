using JMangaHub.Models;

namespace JMangaHub.ViewModels
{
    public class MangaDetailsViewModel
    {
        public Manga Manga { get; set; }
        public bool IsAttending { get; set; }
        public bool IsFollowing { get; set; }
    }
}