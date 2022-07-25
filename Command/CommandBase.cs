namespace TestingTest
{
    public abstract class CommandBase
    {
        protected readonly Map Map;

        protected CommandBase(Map map)
        {
            Map = map;
        }

        public abstract void Execute();
        public abstract void Undo();
    }
}