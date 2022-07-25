namespace TestingTest
{
    public class Cell
    {
        public Cell(Vector2 position, bool isWalkable)
        {
            Position = position;
            IsWalkable = isWalkable;

            Type = CellType.Wall;
        }

        public Vector2 Position { get; set; }
        public bool IsWalkable { get; set; }
        public CellType Type { get; set; }
    }
}