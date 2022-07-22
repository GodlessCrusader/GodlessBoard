namespace GodlessBoard.GameModel
{
    public class Grid
    {
        public Grid()
        {
            Size = 50;
            LineWidth = 1;
            Type = GridType.Square;
        }
        public int LineWidth { get; set; }
        public int Size { get; set; }
        public GridType Type { get; set; }
    }
    public enum GridType
    {
        Square = 0,
        Hex = 1
    }
}
