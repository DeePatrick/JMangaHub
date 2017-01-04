using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JMangaHub.Models
{
    public class Manga
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string MangaName { get; set; }

        public ApplicationUser Creator { get; set; }

        [Required]
        public string CreatorId { get; set; }

        [Required]
        public byte GenreId { get; set; }

        public Genre Genre { get; set; }

        public string Summary { get; set; }

        [Required]
        [StringLength(255)]
        public string Vendor { get; set; }

        public DateTime DueDateTime { get; set; }

        public bool IsDeleted { get; internal set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Manga()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            IsDeleted = true;

            var notification = Notification.MangaDeleted(this);

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }

        public void Modify(DateTime dateTime, string vendor , byte genre)
        {
            var notification = Notification.MangaUpdated(this, DueDateTime, Vendor);

            Vendor = vendor;
            DueDateTime = dateTime;
            GenreId = genre;

            foreach (var attendee in Attendances.Select(a => a.Attendee))
                attendee.Notify(notification);
        }
    }
}