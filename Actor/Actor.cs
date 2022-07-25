using System;

namespace TestingTest
{
    public abstract class Actor : IDrawable
    {
        public Actor(int health, int damage)
        {
            Health = health;
            Damage = damage;
            HealthMax = Health;
        }

        public abstract char Symbol { get; }

        public int Health { get; protected set; }
        public int HealthMax { get; protected set; }
        public int Damage { get; protected set; }
        public Vector2 Position { get; set; }

        public bool IsAlive { get; protected set; } = true;

        public abstract CellType GetCellType();
        public abstract void TakeDamage(int damage);
        public abstract void Heal(int value);

        public void Draw()
        {
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write(Symbol);
            Console.SetCursorPosition(0, 0);
        }

        public Vector2 GetNewPositionByDirection(DirectionType direction)
        {
            return direction switch
            {
                DirectionType.North => new Vector2(Position.X, Position.Y - 1),
                DirectionType.South => new Vector2(Position.X, Position.Y + 1),
                DirectionType.West => new Vector2(Position.X - 1, Position.Y),
                DirectionType.East => new Vector2(Position.X + 1, Position.Y),
                _ => Position,
            };
        }
    }
}