using System;
using System.Collections.Generic;

namespace TestingTest
{
    public class MapCreator
    {
        private int _width;
        private int _height;
        private int _roomsCount;
        private int _roomSizeMin;
        private int _roomSizeMax;
        private Map _map;

        public Action<Player> PlayerCreated;

        public IMap CreateMap(int width, int height, int roomsCount, int roomSizeMin, int roomSizeMax)
        {
            _width = width;
            _height = height;
            _roomsCount = roomsCount;
            _roomSizeMin = roomSizeMin;
            _roomSizeMax = roomSizeMax;

            var player = new Player(3, 10);
            _map = new Map(_width, _height, player);
            _map.Initialize();

            for (int i = 0; i < _roomsCount; i++)
            {
                int roomWidth = RandomUtils.Range(_roomSizeMin, _roomSizeMax);
                int roomHeight = RandomUtils.Range(_roomSizeMin, _roomSizeMax);
                int roomPositionX = RandomUtils.Range(0, _width - roomWidth - 1);
                int roomPositionY = RandomUtils.Range(0, _height - roomHeight - 1);

                var newRoom = new Room(roomWidth, roomHeight, roomPositionX, roomPositionY);

                bool isNewRoomIntersects = false;
                foreach (Room room in _map.Rooms)
                {
                    if (newRoom.Intersects(room))
                    {
                        isNewRoomIntersects = true;
                        break;
                    }
                }

                if (!isNewRoomIntersects)
                {
                    _map.AddRoom(newRoom);
                }
            }

            foreach (Room room in _map.Rooms)
            {
                CreateRoom(_map, room);
            }

            CreateTunnels(_map.Rooms, _map);
            PlacePlayer(player);
            PlaceEnemies();

            return _map;
        }

        private void CreateRoom(Map map, Room room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    map.SetCellWalkability(new Vector2(x, y), CellType.Floor, true);
                }
            }
        }

        private void CreateTunnels(IReadOnlyList<Room> rooms, Map map)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == 0)
                {
                    continue;
                }

                int previousRoomCenterX = rooms[i - 1].Center.X;
                int previousRoomCenterY = rooms[i - 1].Center.Y;
                int currentRoomCenterX = rooms[i].Center.X;
                int currentRoomCenterY = rooms[i].Center.Y;

                if (RandomUtils.Range(0, 2) == 0)
                {
                    CreateHorizontalTunnel(map, previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    CreateVerticalTunnel(map, previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
                }
                else
                {
                    CreateVerticalTunnel(map, previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    CreateHorizontalTunnel(map, previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
                }
            }
        }

        private void CreateHorizontalTunnel(Map map, int startX, int endX, int positionY)
        {
            for (int x = Math.Min(startX, endX); x <= Math.Max(startX, endX); x++)
            {
                map.SetCellWalkability(new Vector2(x, positionY), CellType.Floor, true);
            }
        }

        private void CreateVerticalTunnel(Map map, int startY, int endY, int positionX)
        {
            for (int y = Math.Min(startY, endY); y <= Math.Max(startY, endY); y++)
            {
                map.SetCellWalkability(new Vector2(positionX, y), CellType.Floor, true);
            }
        }

        private void PlacePlayer(Player player)
        {
            PlayerCreated?.Invoke(player);
            player.Position = new Vector2(_map.Rooms[0].Center.X, _map.Rooms[0].Center.Y);
            _map.SetCellWalkability(player.Position, player.GetCellType(), false);
        }

        private void PlaceEnemies()
        {
            foreach (var room in _map.Rooms)
            {
                if (RandomUtils.Range(0, 2) == 0)
                {
                    Vector2 randomPosition = _map.GetRandomPositionAtRoom(room);
                    if (randomPosition.X != 0 && randomPosition.Y != 0)
                    {
                        var enemy = new Enemy(1, 1);
                        enemy.Position = randomPosition;
                        _map.AddEnemy(enemy);
                        _map.SetCellWalkability(enemy.Position, enemy.GetCellType(), false);
                    }
                }
            }
        }
    }
}