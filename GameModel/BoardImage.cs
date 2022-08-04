

using GodlessBoard.Interfaces;

namespace GodlessBoard.GameModel
{
    public class BoardImage : IDraggableBoardObject
    {
        public BoardImage(string imageUrl, int x, int y, bool isGridBinded, int width, int height)
        {
            X = x;
            Y = y;
            ImageUrl = imageUrl;
            IsGridBinded = isGridBinded;
            Height = height;
            Width = width;
        }

        public int X { get; private set; } = 0;

        public int Y { get; private set; } = 0;

        public string ImageUrl { get; set; }
        public bool IsGridBinded { get; set; } = false;
        public int Height { get; set; }
        public int Width { get; set; }

        public IDraggableBoardObject Copy(int x, int y)
        {
            return new BoardImage(ImageUrl, x, y, IsGridBinded, Width, Height);
        }

        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
