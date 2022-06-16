using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Map
{
    [Serializable]
    public enum MapTileType
    {
        Ground = 0,
        Road = 1,
        GroundWithTowerSpace = 2
    }

    [CreateAssetMenu(fileName = "MapSegment", menuName = "PSG/Create Map Segment")]
    public class MapSegment : ScriptableObject
    {
        [SerializeField]
        private MapTileType[] tiles;
        public MapTileType[] Tiles
        {
            get
            {
                if (IsNew())
                    InitializeTiles();

                return tiles;
            }
            private set => tiles = value;
        }

        public static readonly int MAP_SIZE = 9;
        public static readonly int TOWER_SPACE_COUNT = 4;

        public bool IsNew()
        {
            return tiles == null || tiles.Length != MAP_SIZE * MAP_SIZE;
        }

        public void InitializeTiles()
        {
            tiles = new MapTileType[MAP_SIZE * MAP_SIZE];

            Set(0, 4, MapTileType.Road);
            Set(4, 0, MapTileType.Road);
        }

        public Color GetColor(int x, int y)
        {
            ValidateIndex(x, y);

            switch (Get(x, y))
            {
                case MapTileType.Ground:
                    return Color.grey;
                case MapTileType.Road:
                    return Color.blue;
                case MapTileType.GroundWithTowerSpace:
                    return Color.green;
                default:
                    throw new InvalidOperationException($"Invalid enum value. Enum: {nameof(MapTileType)}, value: {Get(x, y)}.");
            }
        }

        public void SwitchTile(int x, int y)
        {
            ValidateIndex(x, y);

            switch (Get(x, y))
            {
                case MapTileType.Ground:
                    if (IsLocked(x, y))
                        Set(x, y, MapTileType.GroundWithTowerSpace);
                    else
                        Set(x,y, MapTileType.Road);
                    break;
                case MapTileType.Road:
                    Set(x, y, MapTileType.GroundWithTowerSpace);
                    break;
                case MapTileType.GroundWithTowerSpace:
                    Set(x, y, MapTileType.Ground);
                    break;
                default:
                    throw new InvalidOperationException($"Invalid enum value. Enum: {nameof(MapTileType)}, value: {Get(x, y)}.");
            }
        }

        public bool IsLocked(int x, int y)
        {
            ValidateIndex(x, y);

            return x == 0 || x == MAP_SIZE - 1 || y == 0 || y == MAP_SIZE - 1;
        }

        public string GetTooltip(int x, int y)
        {
            ValidateIndex(x, y);

            string result = "";

            switch (Get(x, y))
            {
                case MapTileType.Ground:
                    result += "Ground";
                    break;
                case MapTileType.Road:
                    result += "Road";
                    break;
                case MapTileType.GroundWithTowerSpace:
                    result += "Ground with tower space";
                    break;
                default:
                    throw new InvalidOperationException($"Invalid enum value. Enum: {nameof(MapTileType)}, value: {Get(x, y)}.");

            }

            if (IsLocked(x, y))
                result += ", locked";

            return result;
        }

        public MapTileType Get(int x, int y)
        {
            ValidateIndex(x, y);

            return Tiles[x * MAP_SIZE + y];
        }

        public void Set(int x, int y, MapTileType tileType)
        {
            ValidateIndex(x, y);
            Tiles[x * MAP_SIZE + y] = tileType;
        }

        public bool IsValid()
        {
            if (Tiles == null || Tiles.Length != MAP_SIZE * MAP_SIZE)
                return false;

            if (TowerSpaceCount() < TOWER_SPACE_COUNT)
                return false;

            try
            {
                for (int x = 1; x < MapSegment.MAP_SIZE - 1; x++)
                {
                    for (int y = 1 ; y < MapSegment.MAP_SIZE - 1; y++)
                    {
                        if(Get(x, y) == MapTileType.Road)
                        {
                            int count = 0;
                            if (Get(x - 1, y) == MapTileType.Road)
                                count++;
                            if (Get(x + 1, y) == MapTileType.Road)
                                count++;
                            if (Get(x, y - 1) == MapTileType.Road)
                                count++;
                            if (Get(x, y + 1) == MapTileType.Road)
                                count++;

                            if (count != 2)
                                return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int TowerSpaceCount()
        {
            return Tiles.Count(x => x == MapTileType.GroundWithTowerSpace);
        }

        private void ValidateIndex(int x, int y)
        {
            if (IsNew())
                InitializeTiles();

            if (Tiles == null)
                throw new NullReferenceException("Tiles is not initialized.");

            if (x < 0 || x >= MAP_SIZE)
                throw new IndexOutOfRangeException($"Index out of bounds. Index x: {x}, bounds: 0 - {Tiles.GetLength(0)}.");

            if (y < 0 || y >= MAP_SIZE)
                throw new IndexOutOfRangeException($"Index out of bounds. Index y: {y}, bounds: 0 - {Tiles.GetLength(1)}.");
        }
    }
}
