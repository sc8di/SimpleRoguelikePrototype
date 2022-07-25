using System;

namespace TestingTest
{
    public class Player : Actor
    {
        public Action<int> HealthChanged;
        public Action PlayerDied;

        public override char Symbol => GetCellType().GetCharRepresentation();

        public Player(int health, int damage) : base(health, damage) { }

        public override CellType GetCellType()
            => IsAlive ? CellType.Player : CellType.Corpse;

        public override void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }

            Health = Health - damage < 0 ? 0 : Health - damage;

            HealthChanged?.Invoke(Health);

            if (Health == 0)
            {
                Draw();
                PlayerDied?.Invoke();
            }
        }

        public override void Heal(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            Health = Health + value > HealthMax ? HealthMax : Health + value;

            HealthChanged?.Invoke(Health);
        }
    }
}