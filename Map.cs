using System;
using System.Collections.Generic;
using System.Linq;

namespace TestingTest
{
    public class Map : IMap
    {
        private const int SearchRandomPositionAtRoomIterationMax = 100;

        private readonly CommandRecorder _commandRecorder = new CommandRecorder();

        private Cell[,] _cells;
        private Player _player;
        private List<Room> _rooms;
        private List<Enemy> _enemies;

        public Map(int width, int height, Player player)
        {
            Width = width;
            Height = height;

            _cells = new Cell[Width, Height];
            _player = player;

            _commandRecorder.RollBackStepsChanged += value => RollBackStepsChanged?.Invoke(value);
        }

        public Action<int> RollBackStepsChanged { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public IReadOnlyList<Room> Rooms => _rooms;
        public IReadOnlyList<Enemy> Enemies => _enemies;

        public void SetCellWalkability(Vector2 position, CellType type, bool isWalkable)
        {
            Cell cell = GetCell(position);
            cell.IsWalkable = isWalkable;
            cell.Type = type;
        }

        public void Rewind()
        {
            _commandRecorder.Rewind();
        }
        
        public void PrepareNextRecordList()
        {
            _commandRecorder.PrepareNextRecordList();
        }

        public bool TrySetActorPosition(Actor actor, Vector2 position)
        {
            var cell = GetCell(position);

            bool isSuccess;

            switch (cell.Type)
            {
                case CellType.Corpse:
                case CellType.Floor:
                    {
                        var command = new MoveCommand(this, actor, cell);
                        _commandRecorder.Record(command);
                        isSuccess = true;
                        break;
                    }
                case CellType.Player:
                    {
                        var command = new AttackCommand(this, _player, actor.Damage);
                        _commandRecorder.Record(command);
                        isSuccess = false;

                        break;
                    }
                case CellType.Enemy:
                    {
                        if (actor.GetCellType() == CellType.Enemy)
                        {
                            isSuccess = false;
                            break;
                        }

                        var enemy = _enemies.FirstOrDefault(x => x.Position.Equals(position));
                        if (enemy != null && enemy.IsAlive)
                        {
                            var command = new AttackCommand(this, enemy, actor.Damage);
                            _commandRecorder.Record(command);
                            isSuccess = false;
                        }
                        else
                        {
                            var command = new MoveCommand(this, actor, cell);
                            _commandRecorder.Record(command);
                            isSuccess = true;
                        }

                        break;
                    }
                case CellType.Wall:
                default:
                    isSuccess = false;
                    break;
            }

            actor.Draw();

            return isSuccess;
        }

        public void Initialize()
        {
            _cells = new Cell[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _cells[x, y] = new Cell(new Vector2(x, y), false);
                }
            }

            _rooms = new List<Room>();
            _enemies = new List<Enemy>();
        }

        public Cell GetCell(Vector2 position)
        {
            return _cells[position.X, position.Y];
        }

        public void AddRoom(Room room)
        {
            _rooms.Add(room);
        }

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
        }

        public void Draw()
        {
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            Console.Clear();

            bool isFirstRow = true;
            foreach (var cell in GetCells())
            {
                if (!isFirstRow && cell.Position.X == 0)
                {
                    Console.WriteLine();
                }

                Console.Write(cell.Type.GetCharRepresentation());
                isFirstRow = false;
            }
        }

        public Vector2 GetRandomPositionAtRoom(Room room)
        {
            if (IsRoomWalkable(room))
            {
                for (int i = 0; i < SearchRandomPositionAtRoomIterationMax; i++)
                {
                    int x = RandomUtils.Range(1, room.Width - 2) + room.Position.X;
                    int y = RandomUtils.Range(1, room.Height - 2) + room.Position.Y;

                    var position = new Vector2(x, y);
                    if (GetCell(position).IsWalkable)
                    {
                        return position;
                    }
                }
            }

            return new Vector2(0, 0);
        }

        public void UpdateEnemiesPositions()
        {
            foreach (var enemy in _enemies.FindAll(x => x.IsAlive))
            {
                TrySetActorPosition(enemy, enemy.GetNewPositionByDirection(RandomUtils.GetRandomEnumValue(typeof(DirectionType),
                    new List<DirectionType> { DirectionType.Undefined, DirectionType.Back })));
            }

            DrawDeadEnemies();
        }

        public void DrawDeadEnemies()
        {
            foreach (var enemy in _enemies.FindAll(x => !x.IsAlive))
            {
                if (!enemy.Position.Equals(_player.Position))
                {
                    enemy.Draw();
                }
            }
        }

        private bool IsRoomWalkable(Room room)
        {
            for (int x = 1; x <= room.Width - 2; x++)
            {
                for (int y = 1; y <= room.Height - 2; y++)
                {
                    if (GetCell(new Vector2(x + room.Position.X, y + room.Position.Y)).IsWalkable)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private IEnumerable<Cell> GetCells()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return _cells[x, y];
                }
            }
        }
    }
}