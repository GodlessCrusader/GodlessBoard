namespace GodlessBoard.Data
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool EmailIsConfirmed { get; set; }
        public string DisplayName { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? Bio { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}
