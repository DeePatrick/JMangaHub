using System;

namespace JMangaHub.Dtos
{
    public class MangaDto 
    {
        public int Id { get; set; }
        public bool IsDeleted  { get; set; }
        public UserDto Creator { get; set; }
        public DateTime DueDateTime { get; set; }
        public string Vendor  { get; set; }
        public GenreDto Genre { get; set; }
    }
}