namespace GodlessBoard.GameModel
{
    public class Board
    {
        public Board(string name)
        {
            Name = name;
            Tokens = new List<Token>();
            Grid = new Grid();
            BoardImages = new List<BoardImage>();
            Height = 1000;
            Width = 2000;
        }
        public string Name { get; set; }
        public List<Token> Tokens { get; set; }
        public List<BoardImage> BoardImages { get; set; }
        public Grid Grid { get; set; }
        public string Background { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
