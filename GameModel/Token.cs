

using GodlessBoard.Interfaces;

namespace GodlessBoard.GameModel
{
    public class Token : IDraggableBoardObject
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Layer CurrentLayer { get; set; }

        public bool IsGridBinded => throw new NotImplementedException();

        public int Height { get; set; }
        public int Width { get; set; }

        public IDraggableBoardObject Copy(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void Move(int x, int y)
        {
            throw new NotImplementedException();
        }
    }

    public enum Layer
    {
        GM = 0,
        Map = 1,
        Token = 2
    }
}
