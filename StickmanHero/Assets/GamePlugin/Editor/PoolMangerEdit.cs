// using System;
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// #if UNITY_EDITOR
// using UnityEditor;
// using System.IO;
// #endif

// [CustomEditor(typeof(MyPool.PoolManager)), CanEditMultipleObjects]  
// public class PoolMangerEdit : Editor
// {
//     public static MyPool.PoolManager groupControl = null;

//     public override void OnInspectorGUI()
//     {
//         GUILayout.BeginVertical();
//         if (GUILayout.Button("GenCode"))
//         {
//             getGroupControl(target);
//             genCode();
//             AssetDatabase.Refresh();
//         }
//         GUILayout.EndVertical();

//         try
//         {
//             base.OnInspectorGUI();
//         }
//         catch (Exception ex)
//         {

//         }
//     }

//     private static void getGroupControl(UnityEngine.Object ob)
//     {
//         if (groupControl == null)
//         {
//             groupControl = (MyPool.PoolManager)ob;
//         }
//     }

//     private static void genCode()
//     {
// #if UNITY_EDITOR
//         try
//         {
//             string levelDirectoryPath;
//             if (Application.platform == RuntimePlatform.OSXEditor)
//             {
//                 levelDirectoryPath = Application.dataPath + "/Sdk/Pools/";
//             }
//             else
//             {
//                 levelDirectoryPath = Application.dataPath + "\\Sdk\\Pools\\";
//             }
//             string path = levelDirectoryPath + "PoolManager.cs";
//             string[] lines = System.IO.File.ReadAllLines(path);
//             List<string> allline = new List<string>();
//             int statusAdd = 0;
//             for (int i = 0; i < lines.Length; i++)
//             {
//                 if (lines[i].Contains("public enum PoolName") && statusAdd == 0)
//                 {
//                     statusAdd = 1;
//                     allline.Add(lines[i]);
//                     allline.Add("    {");
//                     for (int jj = 0; jj < groupControl.ListPool.Count; jj++)
//                     {
//                         string ladd = "";
//                         if (jj == 0)
//                         {
//                             if (groupControl.ListPool.Count > 1)
//                             {
//                                 ladd = "        " + groupControl.ListPool[jj].poolName + " = 0,";
//                             }
//                             else
//                             {
//                                 ladd = "        " + groupControl.ListPool[jj].poolName + " = 0";
//                             }
//                         }
//                         else
//                         {
//                             if (groupControl.ListPool.Count == jj + 1)
//                             {
//                                 ladd = "        " + groupControl.ListPool[jj].poolName;
//                             }
//                             else
//                             {
//                                 ladd = "        " + groupControl.ListPool[jj].poolName + ",";
//                             }
//                         }
//                         allline.Add(ladd);
//                     }
//                     allline.Add("    }");
//                     for (int kk = 0; kk < groupControl.ListPool.Count; kk++)
//                     {
//                         allline.Add("    public enum " + groupControl.ListPool[kk].poolName);
//                         allline.Add("    {");
//                         for (int jj = 0; jj < groupControl.ListPool[kk]._perPrefabPoolOptions.Count; jj++)
//                         {
//                             string ladd = "";
//                             if (jj == 0)
//                             {
//                                 if (groupControl.ListPool[kk]._perPrefabPoolOptions.Count > 1)
//                                 {
//                                     ladd = "        " + groupControl.ListPool[kk]._perPrefabPoolOptions[jj].prefab.name + " = 0,";
//                                 }
//                                 else
//                                 {
//                                     ladd = "        " + groupControl.ListPool[kk]._perPrefabPoolOptions[jj].prefab.name + " = 0";
//                                 }
//                             }
//                             else
//                             {
//                                 if (groupControl.ListPool[kk]._perPrefabPoolOptions.Count == jj + 1)
//                                 {
//                                     ladd = "        " + groupControl.ListPool[kk]._perPrefabPoolOptions[jj].prefab.name;
//                                 }
//                                 else
//                                 {
//                                     ladd = "        " + groupControl.ListPool[kk]._perPrefabPoolOptions[jj].prefab.name + ",";
//                                 }
//                             }
//                             allline.Add(ladd);
//                         }
//                         allline.Add("    }");
//                     }
//                 }
//                 else
//                 {
//                     if (statusAdd == 0)
//                     {
//                         allline.Add(lines[i]);
//                     }
//                     else
//                     {
//                         if (statusAdd == 1)
//                         {
//                             if (lines[i].Contains("public class PoolManager"))
//                             {
//                                 statusAdd = 0;
//                                 allline.Add(lines[i]);
//                             }
//                         }
//                     }
//                 }
//             }

//             System.IO.File.WriteAllLines(path, allline);
//             allline.Clear();
//         }
//         catch (Exception ex)
//         {
//             Debug.LogError("genCode pool manager: " + ex.ToString());
//         }
// #endif
//     }
// }