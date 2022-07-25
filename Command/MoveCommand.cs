using System;

namespace TestingTest
{
    public class MoveCommand : CommandBase
    {
        private readonly Actor _actor;
        private readonly Cell _cell;
        private readonly Vector2 _previousPosition;

        public MoveCommand(Map map, Actor actor, Cell cell) : base(map)
        {
            _actor = actor;
            _cell = cell;

            _previousPosition = _actor.Position;
        }

        public override void Execute()
        {
            Map.SetCellWalkability(_actor.Position, CellType.Floor, true);
            RedrawPreviousActorPosition();
            _actor.Position = _cell.Position;
            Map.SetCellWalkability(_actor.Position, _actor.GetCellType(), false);
        }

        public override void Undo()
        {
            Map.SetCellWalkability(_actor.Position, CellType.Floor, true);
            RedrawPreviousActorPosition();
            _actor.Position = _previousPosition;
            Map.SetCellWalkability(_actor.Position, _actor.GetCellType(), false);

            _actor.Draw();
            Map.DrawDeadEnemies();
        }

        private void RedrawPreviousActorPosition()
        {
            Console.SetCursorPosition(_actor.Position.X, _actor.Position.Y);
            Console.Write(CellType.Floor.GetCharRepresentation());
        }
    }
}