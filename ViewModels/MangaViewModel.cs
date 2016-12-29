using JMangaHub.Models;
using System.Collections.Generic;
using System.Linq;


namespace JMangaHub.ViewModels
{
    public class MangaViewModel 
    {
        public ILookup<int, Attendance> Attendances { get; set; }
        public IEnumerable<Manga> AvailableMangas { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }
        public bool ShowActions { get; set; }
    }
} 