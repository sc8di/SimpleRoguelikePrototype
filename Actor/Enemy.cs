namespace TestingTest
{
    public class Enemy : Actor
    {
        public Enemy(int health, int damage) : base(health, damage) { }

        public override char Symbol => GetCellType().GetCharRepresentation();

        public override CellType GetCellType()
            => IsAlive ? CellType.Enemy : CellType.Corpse;

        public override void TakeDamage(int damage)
        {
            IsAlive = false;
            Draw();
        }

        public override void Heal(int value)
        {
            IsAlive = true;
            Draw();
        }
    }
}