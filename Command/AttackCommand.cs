namespace TestingTest
{
    public class AttackCommand : CommandBase
    {
        private readonly Actor _actor;
        private readonly int _damageTaken;

        public AttackCommand(Map map, Actor enemy, int damage) : base(map)
        {
            _actor = enemy;
            _damageTaken = damage;
        }

        public override void Execute()
        {
            _actor.TakeDamage(_damageTaken);
        }

        public override void Undo()
        {
            _actor.Heal(_damageTaken);
            _actor.Draw();
        }
    }
}