using System;
using System.Collections;
using System.Collections.Generic;
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

        public bool IsNew()
        {
            return tiles == null || tiles.Length != MAP_SIZE * MAP_SIZE;
        }

        public void InitializeTiles()
        {
            tiles = new MapTileType[MAP_SIZE * MAP_SIZE];

            tiles[4] = MapTileType.Road;
            tiles[4 * MAP_SIZE] = MapTileType.Road;
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
                    throw new InvalidOperationException($"Invalid enum value. Enum: {nameof(MapTileType)}, value: {Tiles[x * MAP_SIZE + y]}.");
            }
        }

        public void SwitchTile(int x, int y)
        {
            ValidateIndex(x, y);

            switch (Get(x, y))
            {
                case MapTileType.Ground:
                    Tiles[x * MAP_SIZE + y] = MapTileType.Road;
                    break;
                case MapTileType.Road:
                    Tiles[x * MAP_SIZE + y] = MapTileType.GroundWithTowerSpace;
                    break;
                case MapTileType.GroundWithTowerSpace:
                    Tiles[x * MAP_SIZE + y] = MapTileType.Ground;
                    break;
                default:
                    throw new InvalidOperationException($"Invalid enum value. Enum: {nameof(MapTileType)}, value: {Tiles[x * MAP_SIZE + y]}.");
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
                    throw new InvalidOperationException($"Invalid enum value. Enum: {nameof(MapTileType)}, value: {Tiles[x * MAP_SIZE + y]}.");

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
