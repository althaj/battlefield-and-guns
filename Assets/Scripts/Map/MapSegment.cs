using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Map
{
    public enum MapTileType
    {
        Ground,
        Road,
        GroundWithTowerSpace
    }

    [CreateAssetMenu(fileName = "MapSegment", menuName = "PSG/Create Map Segment")]
    public class MapSegment : ScriptableObject
    {
        public MapTileType[,] Tiles;

        public readonly int MAP_SIZE = 8;

        public bool IsNew
        {
            get => Tiles == null || Tiles.GetLength(0) != MAP_SIZE || Tiles.GetLength(1) != MAP_SIZE;
        }

        public void InitializeTiles()
        {
            Tiles = new MapTileType[MAP_SIZE, MAP_SIZE];

            Tiles[0, 4] = MapTileType.Road;
            Tiles[4, 0] = MapTileType.Road;
        }

        public Color GetColor(int x, int y)
        {
            ValidateIndex(x, y);

            switch (Tiles[x, y])
            {
                case MapTileType.Ground:
                    return Color.grey;
                case MapTileType.Road:
                    return Color.blue;
                case MapTileType.GroundWithTowerSpace:
                    return Color.green;
                default:
                    throw new InvalidOperationException($"Invalid enum value. Enum: {nameof(MapTileType)}, value: {Tiles[x, y]}.");
            }
        }

        public void SwitchTile(int x, int y)
        {
            ValidateIndex(x, y);

            switch (Tiles[x, y])
            {
                case MapTileType.Ground:
                    Tiles[x, y] = MapTileType.Road;
                    break;
                case MapTileType.Road:
                    Tiles[x, y] = MapTileType.GroundWithTowerSpace;
                    break;
                case MapTileType.GroundWithTowerSpace:
                    Tiles[x, y] = MapTileType.Ground;
                    break;
                default:
                    throw new InvalidOperationException($"Invalid enum value. Enum: {nameof(MapTileType)}, value: {Tiles[x, y]}.");
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

            switch (Tiles[x, y])
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
                    throw new InvalidOperationException($"Invalid enum value. Enum: {nameof(MapTileType)}, value: {Tiles[x, y]}.");

            }

            if (IsLocked(x, y))
                result += ", locked";

            return result;
        }

        private void ValidateIndex(int x, int y)
        {
            if (x < 0 || x >= Tiles.GetLength(0))
                throw new IndexOutOfRangeException($"Index out of bounds. Index x: {x}, bounds: 0 - {Tiles.GetLength(0)}.");

            if (y < 0 || y >= Tiles.GetLength(1))
                throw new IndexOutOfRangeException($"Index out of bounds. Index y: {y}, bounds: 0 - {Tiles.GetLength(1)}.");
        }
    }
}
