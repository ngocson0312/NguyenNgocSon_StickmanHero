using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MutilLangToolPopup : EditorWindow
{
    string myString = "Find Key";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    public static void show(Dictionary<string, string> dic)
    {
        // Get existing open window or if none, make a new one:
        MutilLangToolPopup window = (MutilLangToolPopup)EditorWindow.GetWindow(typeof(MutilLangToolPopup));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}