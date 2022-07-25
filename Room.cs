namespace TestingTest
{
    public struct Room
    {
        public Room(int width, int height, int x, int y)
        {
            Width = width;
            Height = height;
            Position = new Vector2(x, y);
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Position { get; set; }

        public static Room Empty { get; } = new Room();

        public Vector2 Center => new Vector2(Position.X + Width / 2, Position.Y + Height / 2);
        public bool IsEmpty => Width == 0 && Height == 0 && Position.X == 0 && Position.Y == 0;
        public int Left => Position.X;
        public int Right => Position.X + Width;
        public int Top => Position.Y;
        public int Bottom => Position.Y + Height;

        public bool Intersects(Room room)
            => room.Left < Right && Left < room.Right && room.Top < Bottom && Top < room.Bottom;
    }
}