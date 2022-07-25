using System;

namespace TestingTest
{
    public class GameLoop
    {
        private MapCreator _mapCreator;
        private InputHandler _input;
        private Player _player;
        private IMap _map;
        private bool _isPlaying;

        public void StartGame()
        {
            _input = new InputHandler();
            _mapCreator = new MapCreator();
            _mapCreator.PlayerCreated += OnPlayerCreated;

            int mapWidth = Console.WindowWidth / 2;
            int mapHeight = Console.WindowHeight / 2;
            int roomsCount = 10;
            int roomSizeMin = 4;
            int roomSizeMax = 10;

            _map = _mapCreator.CreateMap(mapWidth, mapHeight, roomsCount, roomSizeMin, roomSizeMax);
            _map.RollBackStepsChanged += OnRollBackStepsChanged;
            _map.Draw();
            OnHealthChanged(_player.Health);
            OnRollBackStepsChanged(0);

            _isPlaying = true;
            while (_isPlaying)
            {
                DirectionType direction = _input.GetInputDirection(Console.ReadKey(true));

                if (direction == DirectionType.Back)
                {
                    _map.Rewind();
                    continue;
                }

                _map.PrepareNextRecordList();
                _map.TrySetActorPosition(_player, _player.GetNewPositionByDirection(direction));
                _map.UpdateEnemiesPositions();
            }

            _player.HealthChanged -= OnHealthChanged;
            _player.PlayerDied -= OnPlayerDied;

            Console.ReadKey();
        }

        private void OnPlayerCreated(Player player)
        {
            _mapCreator.PlayerCreated -= OnPlayerCreated;
            _player = player;
            _player.HealthChanged += OnHealthChanged;
            _player.PlayerDied += OnPlayerDied;
        }

        private void OnHealthChanged(int health)
        {
            Console.SetCursorPosition(0, Console.WindowHeight / 2 + 1);
            Console.WriteLine($"Player health: {health}");
            Console.SetCursorPosition(0, 0);
        }

        private void OnRollBackStepsChanged(int stepsLeft)
        {
            Console.SetCursorPosition(0, Console.WindowHeight / 2 + 2);
            Console.WriteLine("RollBackStepsLeft: " + (stepsLeft.ToString().Length > 1 
                ? stepsLeft 
                : "0" + stepsLeft.ToString()));
            Console.WriteLine("RollBackButton: Backspace.");
            Console.SetCursorPosition(0, 0);
        }

        private void OnPlayerDied()
        {
            _isPlaying = false;
            Console.SetCursorPosition(0, Console.WindowHeight / 2 + 3);
            Console.WriteLine("GAME OVER");
        }

    }
}