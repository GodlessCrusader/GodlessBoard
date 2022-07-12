﻿namespace GodlessBoard.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string? JsonRepresentation { get; set; }
        public string Name { get; set; }
        public string? PicUrl { get; set; }
        public string? Bio { get; set; }
        public User Owner { get; set; }
        public int MyProperty { get; set; }
        public ICollection<User> Players { get; set; }
        public ICollection<Media>? Medias { get; set; }
    }
}
