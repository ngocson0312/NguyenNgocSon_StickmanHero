//#define Use_TMPro
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
#if Use_TMPro
using TMPro;
#endif

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class EditDataLang
{
    public Dictionary<string, string> dicLangs = new Dictionary<string, string>();
}

[CustomEditor(typeof(mygame.sdk.TextMutil)), CanEditMultipleObjects]
public class MultiLangTool : Editor
{
    private const string keyDefault = "default";
    public mygame.sdk.TextMutil groupControl = null;
    public static Dictionary<string, EditDataLang> dicDefault = null;

    string[] ChoicesKey = new[] { "" };
    int choicesKeyIdx = 0;
    string searchKey = "";
    private bool isLoadData = false;

    public override void OnInspectorGUI()
    {

        // Update the selected choice in the underlying object
        // someClass.choice = _choices[_choiceIndex];
        GUILayout.BeginVertical();
        if (GUILayout.Button("LoadFromData"))
        {
            getGroupControl(target);
            loadDefault();
            AssetDatabase.Refresh();
        }
        if (GUILayout.Button("AddKey"))
        {
            getGroupControl(target);
            AddKey();
            AssetDatabase.Refresh();
        }
        if (GUILayout.Button("DelKey"))
        {
            getGroupControl(target);
            DelKey();
            AssetDatabase.Refresh();
        }
        searchKey = EditorGUILayout.TextField("SearchKey", searchKey);

        choicesKeyIdx = EditorGUILayout.Popup(choicesKeyIdx, ChoicesKey);
        if (GUILayout.Button("ChangeKey"))
        {
            getGroupControl(target);
            ChangeKey();
            AssetDatabase.Refresh();
        }
        if (GUILayout.Button("UpdateValue"))
        {
            getGroupControl(target);
            UpdateValue();
            AssetDatabase.Refresh();
        }
        
        if (GUILayout.Button("SetValue"))
        {
            getGroupControl(target);
            groupControl.key = ChoicesKey[choicesKeyIdx];
            loadDefault();
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
        getGroupControl(target);
        onSearchKeyChange();
    }

    private void getGroupControl(UnityEngine.Object ob)
    {
        groupControl = (mygame.sdk.TextMutil)ob;//dsfd
        if (dicDefault == null)
        {
            dicDefault = new Dictionary<string, EditDataLang>();
            loadDefault();
        }
        else if (!dicDefault.ContainsKey(keyDefault) || !isLoadData)
        {
            isLoadData = true;
            loadDefault();
        }
    }

    private void onSearchKeyChange(bool isforcus = false)
    {
#if UNITY_EDITOR
        if (groupControl.memKey.CompareTo(searchKey) != 0 || isforcus)
        {
            groupControl.memKey = searchKey;
            List<string> listkeys = new List<string>();
            if (searchKey.Length == 0)
            {
                foreach (var item in dicDefault[keyDefault].dicLangs)
                {
                    listkeys.Add(item.Key);
                }
            }
            else
            {
                foreach (var item in dicDefault[keyDefault].dicLangs)
                {
                    if (item.Key.Contains(searchKey))
                    {
                        listkeys.Add(item.Key);
                    }
                }
            }
            ChoicesKey = listkeys.ToArray();
        }
#endif
    }

    private void FindKey()
    {
        MutilLangToolPopup.show(dicDefault[keyDefault].dicLangs);
    }

    private void loadDefault()
    {
#if UNITY_EDITOR
        try
        {
            string levelDirectoryPath;
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                levelDirectoryPath = Application.dataPath + "/GamePlugin/Resources/Langs/";
            }
            else
            {
                levelDirectoryPath = Application.dataPath + "\\GamePlugin\\Resources\\Langs\\";
            }
            string path = levelDirectoryPath + "default.txt";

            EditDataLang langs;
            if (!dicDefault.ContainsKey(keyDefault))
            {
                langs = new EditDataLang();
                dicDefault.Add(keyDefault, langs);
            }
            else
            {
                langs = dicDefault[keyDefault];
            }
            langs.dicLangs.Clear();
            string datajson = System.IO.File.ReadAllText(path);
            var adsplcf = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(datajson);
            foreach (KeyValuePair<string, object> item in adsplcf)
            {
                if (!langs.dicLangs.ContainsKey(item.Key))
                {
                    langs.dicLangs.Add(item.Key, (string)item.Value);
                }
                else
                {
                    Debug.Log($"mysdk: multi lang err key={item.Key} has exist");
                }
                if (item.Key.CompareTo(groupControl.key) == 0 && !Application.isPlaying)
                {
                    groupControl.value = (string)item.Value;
                    setTextVa(groupControl.value);
                }
            }
            onSearchKeyChange(true);
            foreach (var item in dicDefault[keyDefault].dicLangs)
            {
                if (item.Key.CompareTo(groupControl.key) == 0 && !Application.isPlaying)
                {
                    groupControl.value = (string)item.Value;
                    setTextVa(groupControl.value);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: multi lang load default ex=" + ex.ToString());
        }
#endif
    }

    private void AddKey()
    {
#if UNITY_EDITOR
        try
        {
            string levelDirectoryPath;
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                levelDirectoryPath = Application.dataPath + "/GamePlugin/Resources/Langs/";
            }
            else
            {
                levelDirectoryPath = Application.dataPath + "\\GamePlugin\\Resources\\Langs\\";
            }
            string path = levelDirectoryPath + "default.txt";
            if (groupControl.key != null && groupControl.key.Length > 0 && !dicDefault[keyDefault].dicLangs.ContainsKey(groupControl.key))
            {
                string valuelan = "";
                if (groupControl.typeText == mygame.sdk.TypeText.Text_UI)
                {
                    valuelan = groupControl.GetComponent<Text>().text;
                }
                else if (groupControl.typeText == mygame.sdk.TypeText.Text_UI_Label)
                {
                    // valuelan = groupControl.GetComponent<UILabel>().text;
                }
#if Use_TMPro
                else if (groupControl.typeText == TypeText.Text_Mesh_pro_ui)
                {
                    valuelan = groupControl.GetComponent<TextMeshProUGUI>().text;
                }
                else if (groupControl.typeText == TypeText.Text_Mesh_pro)
                {
                    valuelan = groupControl.GetComponent<TextMeshPro>().text;
                }
#endif
                if (valuelan.Length == 0)
                {
                    Debug.LogWarning($"mysdk: multi lang add key={groupControl.key} not value default");
                }
                dicDefault[keyDefault].dicLangs.Add(groupControl.key, valuelan);
                groupControl.value = valuelan;
                string alllang = "{";
                bool isbegin = true;
                foreach (var item in dicDefault[keyDefault].dicLangs)
                {
                    if (isbegin)
                    {
                        isbegin = false;
                    }
                    else
                    {
                        alllang += ",";
                    }
                    alllang += $"\"{item.Key}\":\"{item.Value}\"";
                }
                alllang += "}";
                System.IO.File.WriteAllText(path, alllang);
            }
            else
            {
                Debug.Log($"mysdk: multi lang add key={groupControl.key} has exist or empty");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: multi lang add key ex=" + ex.ToString());
        }
#endif
    }

    private void DelKey()
    {
#if UNITY_EDITOR
        try
        {
            string levelDirectoryPath;
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                levelDirectoryPath = Application.dataPath + "/GamePlugin/Resources/Langs/";
            }
            else
            {
                levelDirectoryPath = Application.dataPath + "\\GamePlugin\\Resources\\Langs\\";
            }
            string path = levelDirectoryPath + "default.txt";
            if (dicDefault[keyDefault].dicLangs.ContainsKey(groupControl.key))
            {
                string alllang = "{";
                bool isbegin = true;
                foreach (var item in dicDefault[keyDefault].dicLangs)
                {
                    if (item.Key.CompareTo(groupControl.key) != 0)
                    {
                        if (isbegin)
                        {
                            isbegin = false;
                        }
                        else
                        {
                            alllang += ",";
                        }
                        alllang += $"\"{item.Key}\":\"{item.Value}\"";
                    }
                }
                alllang += "}";
                System.IO.File.WriteAllText(path, alllang);
            }
            else
            {
                Debug.Log($"mysdk: multi lang add key={groupControl.key} has exist");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: multi lang add key ex=" + ex.ToString());
        }
#endif 
    }

    private void setTextVa(string txt)
    {
        if (groupControl.typeText == mygame.sdk.TypeText.Text_UI)
        {
            groupControl.GetComponent<Text>().text = txt;
        }
        else if (groupControl.typeText == mygame.sdk.TypeText.Text_UI_Label)
        {
            //groupControl.GetComponent<UILabel>().text = txt;
        }
#if Use_TMPro
                else if (groupControl.typeText == TypeText.Text_Mesh_pro_ui)
                {
                    groupControl.GetComponent<TextMeshProUGUI>().text = txt;;
                }
                else if (groupControl.typeText == TypeText.Text_Mesh_pro)
                {
                    groupControl.GetComponent<TextMeshPro>().text = txt;;
                }
#endif
        EditorUtility.SetDirty(groupControl);
    }
    private void ChangeKey()
    {
#if UNITY_EDITOR
        if (choicesKeyIdx >= 0 && choicesKeyIdx < ChoicesKey.Length)
        {
            groupControl.key = ChoicesKey[choicesKeyIdx];
            foreach (var item in dicDefault[keyDefault].dicLangs)
            {
                if (item.Key.CompareTo(groupControl.key) == 0)
                {
                    groupControl.value = (string)item.Value;
                    setTextVa(groupControl.value);
                }
            }
        }
#endif
    }
    private void UpdateValue()
    {
#if UNITY_EDITOR
        bool isedit = false;
        foreach (var item in dicDefault[keyDefault].dicLangs)
        {
            if (item.Key.CompareTo(groupControl.key) == 0)
            {
                dicDefault[keyDefault].dicLangs[item.Key] = groupControl.value;
                setTextVa(groupControl.value);
                isedit = true;
                break;
            }
        }
        if (isedit)
        {
            string levelDirectoryPath;
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                levelDirectoryPath = Application.dataPath + "/GamePlugin/Resources/Langs/";
            }
            else
            {
                levelDirectoryPath = Application.dataPath + "\\GamePlugin\\Resources\\Langs\\";
            }
            string path = levelDirectoryPath + "default.txt";
            string alllang = "{";
            bool isbegin = true;
            foreach (var item in dicDefault[keyDefault].dicLangs)
            {
                if (isbegin)
                {
                    isbegin = false;
                }
                else
                {
                    alllang += ",";
                }
                alllang += $"\"{item.Key}\":\"{item.Value}\"";
            }
            alllang += "}";
            System.IO.File.WriteAllText(path, alllang);
        }
#endif
    }
}