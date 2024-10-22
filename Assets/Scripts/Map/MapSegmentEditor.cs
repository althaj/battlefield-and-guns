using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Map
{
    [CustomEditor(typeof(MapSegment))]
    public class MapSegmentEditor : Editor
    {
        [SerializeField]
        MapSegment segment;

        protected void OnEnable()
        {
            hideFlags = HideFlags.HideAndDontSave;

            segment = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(target.GetInstanceID()), typeof(MapSegment)) as MapSegment;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _ = DrawDefaultInspector();

            EditorGUI.BeginChangeCheck();

            for (int x = 0; x < MapSegment.MAP_SIZE; x++)
            {
                GUILayout.BeginHorizontal();
                for (int y = 0; y < MapSegment.MAP_SIZE; y++)
                {
                    bool isLocked = segment.IsLocked(x, y);

                    GUI.backgroundColor = segment.GetColor(x, y);
                    if (GUILayout.Button(new GUIContent(isLocked ? "L" : "", segment.GetTooltip(x, y))))
                    {
                        segment.SwitchTile(x, y);
                    }
                }

                GUILayout.EndHorizontal();
                GUILayout.Space(10);
            }

            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(segment);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
