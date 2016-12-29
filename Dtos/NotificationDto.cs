using JMangaHub.Models;
using System;

namespace JMangaHub.Dtos
{
    public class NotificationDto
    {
        public DateTime DueDateTime { get; set; }
        public NotificationType Type { get; set; }
        public DateTime? OriginalDueDateTime { get; set; }
        public string OriginalVendor  { get; set; }
        public MangaDto Manga  { get; set; }
    }
}