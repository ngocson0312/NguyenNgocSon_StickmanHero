using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SuperFight
{
    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LevelData levelData = (LevelData)target;
            if(GUILayout.Button("GetAllObjects"))
            {
                levelData.GetAllItem();
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(levelData);
            }
        }
    }
}
