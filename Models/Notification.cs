using System;
using System.ComponentModel.DataAnnotations;

namespace JMangaHub.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime? OriginalDateTime { get; private set; }
        public string OriginalVenue { get; private set; }

        [Required]
        public Manga Manga  { get; private set; }

        protected Notification()
        {
        }

        private Notification(NotificationType type, Manga manga )
        {
            if (manga  == null)
                throw new ArgumentNullException("manga");

            Type = type;
            Manga = manga;
            DateTime = DateTime.Now;
        }

        public static Notification GigCreated(Manga manga )
        {
            return new Notification(NotificationType.MangaCreated, manga);
        }

        public static Notification GigUpdated(Manga newManga , DateTime originalDateTime, string originalVenue)
        {
            var notification = new Notification(NotificationType.MangaUpdated, newManga );
            notification.OriginalDateTime = originalDateTime;
            notification.OriginalVenue = originalVenue;

            return notification;
        }

        public static Notification MangaDeleted (Manga manga)
        {
            return new Notification(NotificationType.MangaDeleted, manga);
        }
    }
}