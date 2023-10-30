

using GameModel;

namespace GodlessBoard.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserDisplayName { get; set; }
        public MediaType Type { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public long Weight { get; set; }
    }
    
}
