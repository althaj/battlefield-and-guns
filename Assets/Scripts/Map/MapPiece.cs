using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Map
{
    public class MapPiece : MonoBehaviour
    {
        #region serialized variables
        [SerializeField] private MapPieceType pieceType;
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

            switch (pieceType)
            {
                case MapPieceType.Ground:
                    Gizmos.DrawWireCube(transform.up * 0.5f, Vector3.one);
                    break;
                case MapPieceType.GroundWithTowerSpace:
                    Gizmos.DrawWireCube(transform.up * 0.5f, Vector3.one);
                    Gizmos.DrawWireCube(transform.up * 1.05f, new Vector3(0.8f, 0.1f, 0.8f));
                    Gizmos.DrawLine(new Vector3(0, 1.1f, -0.4f), new Vector3(0, 1.1f, 0.4f));
                    break;
                case MapPieceType.RoadStraight:
                    Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 0, 1));
                    Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.7f, 0, 1));
                    break;
                case MapPieceType.RoadCorner:
                    Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 0, 1));
                    Gizmos.DrawLine(new Vector3(-0.35f, 0, -0.5f), new Vector3(-0.35f, 0, 0.35f));
                    Gizmos.DrawLine(new Vector3(0.35f, 0, -0.5f), new Vector3(0.35f, 0, -0.35f));
                    Gizmos.DrawLine(new Vector3(0.5f, 0, 0.35f), new Vector3(-0.35f, 0, 0.35f));
                    Gizmos.DrawLine(new Vector3(0.5f, 0, -0.35f), new Vector3(0.35f, 0, -0.35f));
                    break;
                case MapPieceType.WallStraight:
                    Gizmos.DrawWireCube(transform.right * 0.45f + transform.up * 0.5f, new Vector3(0.1f, 1, 1));
                    break;
                case MapPieceType.WallCornerOuter:
                    Gizmos.DrawWireCube(-transform.right * 0.45f + transform.up * 0.5f, new Vector3(0.1f, 1, 1));
                    Gizmos.DrawWireCube(transform.forward * 0.45f + transform.up * 0.5f, new Vector3(1, 1, 0.1f));
                    break;
                case MapPieceType.WallCornerInner:
                    Gizmos.DrawWireCube(transform.right * 0.45f + transform.up * 0.5f - transform.forward * 0.45f, new Vector3(0.1f, 1, 0.1f));
                    break;
                default:
                    break;
            }
        }
    }
}
