using System;

namespace TestingTest
{
    public interface IMap : IDrawable
    {
        Action<int> RollBackStepsChanged { get; set; }

        bool TrySetActorPosition(Actor actor, Vector2 position);
        void UpdateEnemiesPositions();
        void Rewind();
        void PrepareNextRecordList();
    }
}