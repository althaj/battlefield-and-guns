using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSG.RNG;
using PSG.BattlefieldAndGuns.Utility;
using System.Linq;
using System;
using PSG.BattlefieldAndGuns.Managers;
using Unity.AI.Navigation;

namespace PSG.BattlefieldAndGuns.Map
{
    public class MapBuilder : MonoBehaviour
    {
        #region serialized variables

        [SerializeField] private GameObject[] roadStraightPrefabs;
        [SerializeField] private GameObject[] roadCornerPrefabs;
        [SerializeField] private GameObject[] wallStraightPrefabs;
        [SerializeField] private GameObject[] wallCornerOuterPrefabs;
        [SerializeField] private GameObject[] wallCornerInnerPrefabs;
        [SerializeField] private GameObject[] groundPrefabs;

        [SerializeField] private GameObject towerSpacePrefab;

        [SerializeField] private GameObject mapParent;

        #endregion

        #region private variables

        private MapSegment[] segments;

        private enum TilePrefabType
        {
            RoadStraight,
            RoadCorner,
            WallStraight,
            WallCornerOuter,
            WallCornerInner,
            Ground
        }

        private float tileSize = 1f;

        #endregion

        #region properties

        #endregion

        // Start is called before the first frame update
        private void Start()
        {
            segments = Resources.LoadAll("MapSegments", typeof(MapSegment)).Cast<MapSegment>().Where(x => x.IsValid()).ToArray();

            for (int i = 0; i < 4; i++)
            {
                MapSegment segment = RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(segments);
                GameObject mapChunk = new GameObject("Map chunk");

                GameObject instance = null;

                int towerSpaceCount = 0;
                float allTowerSpaceCount = (float)segment.TowerSpaceCount();

                for (int x = 0; x < MapSegment.MAP_SIZE; x++)
                {
                    for (int y = 0; y < MapSegment.MAP_SIZE; y++)
                    {
                        switch (segment.Get(x, y))
                        {
                            case MapTileType.Ground:
                                CreateTile(TilePrefabType.Ground, x, y, 0, mapChunk);
                                break;
                            case MapTileType.Road:
                                CreateRoad(segment, x, y, mapChunk);
                                break;
                            case MapTileType.GroundWithTowerSpace:
                                CreateTile(TilePrefabType.Ground, x, y, 0, mapChunk);

                                if (RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextBool((MapSegment.TOWER_SPACE_COUNT - towerSpaceCount) / allTowerSpaceCount))
                                {
                                    CreateTowerSpace(x, y, mapChunk);
                                    towerSpaceCount++;
                                }

                                allTowerSpaceCount--;
                                break;
                            default:
                                break;
                        }

                        instance?.transform.SetParent(mapChunk.transform);
                    }
                }

                switch (i)
                {
                    case 0:
                        mapChunk.transform.position = new Vector3(-0.5f, 0, 0.5f);
                        mapChunk.transform.Rotate(transform.up, -90);
                        break;
                    case 1:
                        mapChunk.transform.position = new Vector3(0.5f, 0, 8.5f);
                        mapChunk.transform.Rotate(transform.up, 90);
                        break;
                    case 2:
                        mapChunk.transform.position = new Vector3(0.5f, 0, -8.5f);
                        break;
                    case 3:
                        mapChunk.transform.position = new Vector3(-0.5f, 0, -0.5f);
                        mapChunk.transform.Rotate(transform.up, 180);
                        break;
                }

                mapChunk.transform.SetParent(mapParent.transform);
            }

            mapParent.GetComponent<NavMeshSurface>().BuildNavMesh();

            FindObjectOfType<TowerManager>().GetTowerSpaces();
        }

        private void CreateTowerSpace(int x, int y, GameObject parent)
        {
            GameObject go = Instantiate(towerSpacePrefab, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.identity);
            go.transform.SetParent(parent.transform);
        }

        private void CreateTile(TilePrefabType prefabType, int x, int y, float rotation, GameObject parent)
        {
            switch (prefabType)
            {
                case TilePrefabType.RoadStraight:
                    InstantiateTile(RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(roadStraightPrefabs), x, y, rotation, parent);
                    break;
                case TilePrefabType.RoadCorner:
                    InstantiateTile(RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(roadCornerPrefabs), x, y, rotation, parent);
                    break;
                case TilePrefabType.WallStraight:
                    InstantiateTile(RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(wallStraightPrefabs), x, y, rotation, parent);
                    break;
                case TilePrefabType.WallCornerOuter:
                    InstantiateTile(RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(wallCornerOuterPrefabs), x, y, rotation, parent);
                    break;
                case TilePrefabType.WallCornerInner:
                    InstantiateTile(RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(wallCornerInnerPrefabs), x, y, rotation, parent);
                    break;
                case TilePrefabType.Ground:
                    InstantiateTile(RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(groundPrefabs), x, y, rotation, parent);
                    break;
                default:
                    break;
            }
        }

        private void InstantiateTile(GameObject prefab, int x, int y, float rotation, GameObject parent)
        {
            GameObject go = Instantiate(prefab, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.Euler(90, rotation, 0));
            go.transform.SetParent(parent.transform);
        }

        private void CreateRoad(MapSegment segment, int x, int y, GameObject parent)
        {
            if (x == 0 || x == MapSegment.MAP_SIZE)
            {
                CreateRoadStraight(x, y, 0, parent);
                return;
            }

            if (y == 0 || y == MapSegment.MAP_SIZE)
            {
                CreateRoadStraight(x, y, 90, parent);
                return;
            }

            if(segment.Get(x - 1, y) == MapTileType.Road && segment.Get(x + 1, y) == MapTileType.Road)
            {
                CreateRoadStraight(x, y, 0, parent);
                return;
            }

            if (segment.Get(x, y - 1) == MapTileType.Road && segment.Get(x, y + 1) == MapTileType.Road)
            {
                CreateRoadStraight(x, y, 90, parent);
                return;
            }

            if (segment.Get(x, y + 1) == MapTileType.Road && segment.Get(x - 1, y) == MapTileType.Road)
            {
                CreateRoadCorner(x, y, 180, parent);
                return;
            }

            if (segment.Get(x, y - 1) == MapTileType.Road && segment.Get(x - 1, y) == MapTileType.Road)
            {
                CreateRoadCorner(x, y, 90, parent);
                return;
            }

            if (segment.Get(x, y - 1) == MapTileType.Road && segment.Get(x + 1, y) == MapTileType.Road)
            {
                CreateRoadCorner(x, y, 0, parent);
                return;
            }

            if (segment.Get(x, y + 1) == MapTileType.Road && segment.Get(x + 1, y) == MapTileType.Road)
            {
                CreateRoadCorner(x, y, 270, parent);
                return;
            }

        }

        private void CreateRoadStraight(int x, int y, float rotation, GameObject parent)
        {
            CreateTile(TilePrefabType.RoadStraight, x, y, rotation, parent);
            CreateTile(TilePrefabType.WallStraight, x, y, rotation, parent);
            CreateTile(TilePrefabType.WallStraight, x, y, rotation + 180, parent);
        }

        private void CreateRoadCorner(int x, int y, float rotation, GameObject parent)
        {
            CreateTile(TilePrefabType.RoadCorner, x, y, rotation, parent);
            CreateTile(TilePrefabType.WallCornerOuter, x, y, rotation, parent);
            CreateTile(TilePrefabType.WallCornerInner, x, y, rotation + 180, parent);
        }
    }
}
