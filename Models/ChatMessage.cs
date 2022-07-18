

namespace GodlessBoard.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Chat Chat { get; set; }
        public DateTime RecievingTime { get; set; }
        public string Text { get; set; }
    }
}
