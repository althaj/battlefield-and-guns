using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSG.RNG;
using PSG.BattlefieldAndGuns.Utility;

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

        [SerializeField] private GameObject[] mapChunks;

        #endregion

        #region private variables

        #endregion

        #region properties

        #endregion

        // Start is called before the first frame update
        private void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject mapChunk = Instantiate(RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(mapChunks));

                switch (i)
                {
                    case 0:
                        mapChunk.transform.position = new Vector3(-5, 0, -5);
                        break;
                    case 1:
                        mapChunk.transform.position = new Vector3(5, 0, 5);
                        mapChunk.transform.Rotate(transform.up, -90);
                        break;
                    case 2:
                        mapChunk.transform.position = new Vector3(5, 0, -5);
                        mapChunk.transform.Rotate(transform.up, -180);
                        break;
                    case 3:
                        mapChunk.transform.position = new Vector3(-5, 0, -5);
                        mapChunk.transform.Rotate(transform.up, 90);
                        break;
                }

                MapPiece[] mapPieces = mapChunk.GetComponentsInChildren<MapPiece>();
                List<MapPiece> towerSpaceMapPieces = new List<MapPiece>();


                foreach (MapPiece piece in mapPieces)
                {
                    GameObject prefab;
                    Vector3 pieceRotation = piece.transform.eulerAngles;
                    Quaternion rotation = Quaternion.Euler(pieceRotation.x + 90, pieceRotation.y, pieceRotation.z);

                    switch (piece.PieceType)
                    {
                        case MapPieceType.Ground:
                        default:
                            prefab = RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(groundPrefabs);
                            break;
                        case MapPieceType.GroundWithTowerSpace:
                            prefab = RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(groundPrefabs);
                            towerSpaceMapPieces.Add(piece);
                            break;
                        case MapPieceType.RoadStraight:
                            prefab = RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(roadStraightPrefabs);
                            rotation = Quaternion.Euler(pieceRotation.x + 90, pieceRotation.y, RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextBool() ? pieceRotation.z + 90 : pieceRotation.z - 90);
                            break;
                        case MapPieceType.RoadCorner:
                            prefab = RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(roadCornerPrefabs);
                            break;
                        case MapPieceType.WallStraight:
                            prefab = RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(wallStraightPrefabs);
                            rotation = Quaternion.Euler(pieceRotation.x + 90, pieceRotation.y, RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextBool() ? pieceRotation.z + 90 : pieceRotation.z - 90);
                            break;
                        case MapPieceType.WallCornerOuter:
                            prefab = RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(wallCornerOuterPrefabs);
                            break;
                        case MapPieceType.WallCornerInner:
                            prefab = RNGManager.Manager[Constants.MAP_BUILDER_RNG_TITLE].NextElement(wallCornerInnerPrefabs);
                            break;
                    }

                    GameObject tile = Instantiate(prefab, piece.transform.position, rotation);

                    tile.transform.parent = transform;
                }

                Destroy(mapChunk);
            }
        }
    }
}
