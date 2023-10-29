using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CustomEditor(typeof(mygame.sdk.AdjustHelper)), CanEditMultipleObjects]
public class AdjustSetting : Editor
{
    public static mygame.sdk.AdjustHelper groupControl = null;

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical();
        if (GUILayout.Button("Gen Enum Event"))
        {
            getGroupControl(target);
            doGenEnumEvent();
            AssetDatabase.Refresh();
        }
        GUILayout.EndVertical();

        try
        {
            base.OnInspectorGUI();
        }
        catch (Exception ex)
        {
            Debug.Log("mysdk: ex=" + ex.ToString());
        }
    }

    private static void getGroupControl(UnityEngine.Object ob)
    {
        if (groupControl == null)
        {
            groupControl = (mygame.sdk.AdjustHelper)ob;
        }
    }

    private static void doGenEnumEvent()
    {
#if UNITY_EDITOR
        List<string> enumAdjustEventName = new List<string>();
        string levelDirectoryPath;
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            levelDirectoryPath = Application.dataPath + "/GamePlugin/Analytic/";
        }
        else
        {
            levelDirectoryPath = Application.dataPath + "\\GamePlugin\\Analytic\\";
        }
        string path = levelDirectoryPath + "AdjustEventName.cs";
        string[] lines = System.IO.File.ReadAllLines(path);
        List<string> allline = new List<string>();

        int statedit = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (statedit == 0)
            {
                allline.Add(lines[i]);
                if (lines[i].Contains("public enum AdjustEventName"))
                {
                    statedit = 1;
                    allline.Add("    {");
                    for (int kk = 0; kk < groupControl.listEvent.Count; kk++)
                    {
                        string enumName = groupControl.listEvent[kk].name;
                        enumName = enumName.Replace(' ', '_');
                        string ladd = "        " + enumName;
                        if (kk == 0)
                        {
                            ladd += " = 0";
                        }
                        if (kk < (groupControl.listEvent.Count - 1))
                        {
                            ladd += ",";
                        }
                        allline.Add(ladd);
                    }
                }
            }
            else if (statedit == 1)
            {
                if (lines[i].Contains("    }"))
                {
                    allline.Add(lines[i]);
                    allline.Add("}");
                }
            }
        }

        System.IO.File.WriteAllLines(path, allline);
#endif
    }

}
