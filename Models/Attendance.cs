using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JMangaHub.Models
{
    public class Attendance
    {
        public Manga Manga { get; set; }

        [Key]
        [Column(Order = 1)]
        public int MangaId { set; get; }

        public ApplicationUser Attendee { get; set; }

        [Key]
        [Column(Order = 2)]
        public string AttendeeId { get; set; }

    }
}