

using GodlessBoard.Interfaces;

namespace GodlessBoard.GameModel
{
    public class BoardImage : IDraggableBoardObject
    {
        public BoardImage(int x, int y, string imageUrl, bool isGridBinded)
        {
            X = x;
            Y = y;
            ImageUrl = imageUrl;
            IsGridBinded = isGridBinded;
        }

        public int X { get; private set; } = 0;

        public int Y { get; private set; } = 0;

        public string ImageUrl { get; set; }
        public bool IsGridBinded { get; set; } = false;
        public int Height { get; set; } = 100;
        public int Width { get; set; } = 100;

        public IDraggableBoardObject Copy(int x, int y)
        {
            return new BoardImage(x, y, ImageUrl, IsGridBinded);
        }

        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
