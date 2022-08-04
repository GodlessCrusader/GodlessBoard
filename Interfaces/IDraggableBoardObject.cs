namespace GodlessBoard.Interfaces
{
    public interface IDraggableBoardObject
    {
       
        public int X { get;}
        public int Y { get;}
        public int Height { get; set; }
        public int Width { get; set; }
        public bool IsGridBinded { get; }        
        public void Move(int x, int y);
        public IDraggableBoardObject Copy(int x, int y);
    }
}
