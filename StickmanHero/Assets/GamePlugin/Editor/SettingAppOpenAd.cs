using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Android;
#if UNITY_IOS || UNITY_IPHONE
using UnityEditor.iOS;
#endif
using System.IO;
#endif

[CustomEditor(typeof(mygame.sdk.SDKManager)), CanEditMultipleObjects]
public class SettingBuildAndroid : Editor
{
    public static mygame.sdk.SDKManager groupControl = null;
    public static string unityver = "20";
    public static string defaultOrientation = "0";
    bool isTestInApp = false;
    bool forceGenBaseKey = false;
    string packageNameTest = "";
    string mempackageNameTest = "";
    string DefineSymbolAdd = "";//
    string memDefineSymbolAdd = "";
    string GameName = "";
    string MemGameName = "";
    public static bool isAdcanvas = false;
    public static bool isIronEnable = false;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(GameName) || string.IsNullOrEmpty(MemGameName))
        {
            GameName = getMemSetting("key_mem_gamename", "");
            MemGameName = GameName;
        }
        if (string.IsNullOrEmpty(packageNameTest) || string.IsNullOrEmpty(mempackageNameTest))
        {
            packageNameTest = getMemSetting("key_mem_pkgtest", "");
            mempackageNameTest = packageNameTest;
        }
        if (string.IsNullOrEmpty(DefineSymbolAdd) || string.IsNullOrEmpty(memDefineSymbolAdd))
        {
            DefineSymbolAdd = getMemSetting("key_mem_define_sb_a", "");
            memDefineSymbolAdd = DefineSymbolAdd;
        }
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical();
        GameName = EditorGUILayout.TextField("GameName", GameName);
        if (GameName.CompareTo(MemGameName) != 0)
        {
            setMemSetting("key_mem_gamename", GameName);
            MemGameName = GameName;
        }
        if (GUILayout.Button("SetupSdk"))
        {
            getGroupControl(target);
            doSetting4Game();
            AssetDatabase.Refresh();
        }
        if (GUILayout.Button("SetupSdkTest"))
        {
            getGroupControl(target);
            doSetting4Game(true);
            AssetDatabase.Refresh();
        }

        packageNameTest = EditorGUILayout.TextField("PackageNameTest", packageNameTest);
        if (packageNameTest.CompareTo(mempackageNameTest) != 0)
        {
            setMemSetting("key_mem_pkgtest", packageNameTest);
            mempackageNameTest = packageNameTest;
        }

        DefineSymbolAdd = EditorGUILayout.TextField("Define Symbols Add", DefineSymbolAdd);
        if (DefineSymbolAdd.CompareTo(memDefineSymbolAdd) != 0)
        {
            setMemSetting("key_mem_define_sb_a", DefineSymbolAdd);
            memDefineSymbolAdd = DefineSymbolAdd;
        }
        EditorGUILayout.BeginHorizontal();
        forceGenBaseKey = EditorGUILayout.Toggle("ForceGenBaseKey", forceGenBaseKey);
        bool ismem = (PlayerPrefs.GetInt("key_mem_is_inapp", 0) == 1);
        isTestInApp = EditorGUILayout.Toggle("isTestInapp", ismem);
        if (isTestInApp)
        {
            PlayerPrefs.SetInt("key_mem_is_inapp", 1);
        }
        else
        {
            PlayerPrefs.SetInt("key_mem_is_inapp", 0);
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("SetIcon"))
        {
            getGroupControl(target);
            setIcon();
            AssetDatabase.Refresh();
        }
        if (GUILayout.Button("setSpslashLoading"))
        {
            getGroupControl(target);
#if UNITY_ANDROID
            setSpslashLoading("Android");
#elif UNITY_IOS || UNITY_IPHONE
            setSpslashLoading("iOS");
#endif

            AssetDatabase.Refresh();
        }
        // if (GUILayout.Button("GetSupprtFlag"))
        // {
        //     getGroupControl(target);
        //     GetSupprtFlag();
        //     AssetDatabase.Refresh();
        // }
        // if (GUILayout.Button("WiteStringKey"))
        // {
        //     getGroupControl(target);
        //     WiteStringKey();
        //     AssetDatabase.Refresh();
        // }
        GUILayout.EndVertical();

        try
        {
            base.OnInspectorGUI();
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: OnInspectorGUI ex=" + ex.ToString());
        }
    }

    public static string getMemSetting(string key, string defaultvalue)
    {
        string re = defaultvalue;
        if (re == null)
        {
            re = "";
        }
        string levelDirectoryPathSrc = Application.dataPath + "/GamePlugin/Editor/.memsetting.txt";
        try
        {
            string[] alline = File.ReadAllLines(levelDirectoryPathSrc);
            for (int i = 0; i < alline.Length; i++)
            {
                if (alline[i].StartsWith(key + ":"))
                {
                    re = alline[i].Replace(key + ":", "");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("mysdk: ex=" + ex.ToString());
        }

        return re;
    }

    private static void setMemSetting(string key, string value)
    {
        string levelDirectoryPathSrc = Application.dataPath + "/GamePlugin/Editor/.memsetting.txt";
        try
        {
            List<string> lww = new List<string>();
            bool isWwww = false;
            bool hasInMem = false;
            if (File.Exists(levelDirectoryPathSrc))
            {
                string[] alline = File.ReadAllLines(levelDirectoryPathSrc);
                for (int i = 0; i < alline.Length; i++)
                {
                    if (alline[i].StartsWith(key + ":"))
                    {
                        hasInMem = true;
                        if (value == null || value.Length == 0)
                        {
                            isWwww = true;
                        }
                        else
                        {
                            string ladd = key + ":" + value;
                            if (alline[i].CompareTo(ladd) != 0)
                            {
                                isWwww = true;
                                lww.Add(ladd);
                            }
                        }
                    }
                    else
                    {
                        lww.Add(alline[i]);
                    }
                }
            }
            if (!hasInMem && value != null && value.Length > 0)
            {
                isWwww = true;
                lww.Add(key + ":" + value);
            }
            if (isWwww)
            {
                File.WriteAllLines(levelDirectoryPathSrc, lww);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: setMemSetting ex=" + ex.ToString());
        }
    }

    private static void getGroupControl(UnityEngine.Object ob)
    {
        if (groupControl == null)
        {
            groupControl = (mygame.sdk.SDKManager)ob;
            getVerUnity();
        }
        getOrientation();
    }

    static void getVerUnity()
    {
        try
        {
            string passet = Application.dataPath;
            string ppro = passet.Replace("/Assets", "");
            string psetting = ppro + "/ProjectSettings/ProjectVersion.txt";
            string[] allLinesMeta = File.ReadAllLines(psetting);
            foreach (var line in allLinesMeta)
            {
                if (line.Contains("m_EditorVersion"))
                {
                    if (line.Contains("2021"))
                    {
                        unityver = "21";
                    }
                    else if (line.Contains("2022"))
                    {
                        unityver = "22";
                    }
                    else if (line.Contains("2020"))
                    {
                        unityver = "20";
                    }
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("mysdk: ex=" + ex.ToString());
        }
    }

    static void getOrientation()
    {
        try
        {
            string passet = Application.dataPath;
            string ppro = passet.Replace("/Assets", "");
            string psetting = ppro + "/ProjectSettings/ProjectSettings.asset";
            string[] allLinesMeta = File.ReadAllLines(psetting);
            foreach (var line in allLinesMeta)
            {
                if (line.Contains("defaultScreenOrientation"))
                {
                    string[] llll = line.Split(':');
                    if (llll.Length == 2)
                    {
                        defaultOrientation = llll[1];
                        defaultOrientation = defaultOrientation.Replace(" ", "");
                        setMemSetting("key_mem_df_orien", defaultOrientation);
                    }
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("mysdk: ex=" + ex.ToString());
        }
    }

    private void doSetting4Game(bool istest = false)
    {
        string mydvid = SystemInfo.deviceUniqueIdentifier;
        Debug.Log("abc=" + mydvid);
        string[] whileListDevices = {
            "8b5e79c73f144af53538b0abc53bb15f23bec748",
            "174D9302-3683-5F19-B27C-0B4AEBD08F8D",
            "3826a02f015a27caa46cb0e09a9fe748a335ca53",
            "C2C39E20-8B14-5CE2-95C9-3206C611AE67",
            "9C1D7D4D-4727-5CA8-82E9-18D67A4D2317",
            "868E6C2A-FF96-5913-97B2-3342DDE97787",
            "DA664010-0E4B-530A-8C5C-69B44662FB20"
        };

        if (!istest)
        {
            istest = true;
            foreach (var item in whileListDevices)
            {
                if (item.CompareTo(mydvid) == 0)
                {
                    istest = false;
                    break;
                }
            }
            if (istest)
            {
                Debug.Log("abc=change is test");
            }
            else
            {
                Debug.Log("abc=set sdk main");
            }
        }

        try
        {
            parConfigGame(istest);
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: doSetting4Game ex=" + ex.ToString());
        }
        mygame.sdk.AdsHelper adshelper = groupControl.transform.Find("AdvHelper").GetComponent<mygame.sdk.AdsHelper>();
        SdkSetup.SetupSdk(groupControl, adshelper, istest);
        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.SplashScreen.showUnityLogo = false;
        coppyRes2Assets(GameName);
    }

    private void setIcon()
    {
        try
        {
#if UNITY_ANDROID
            setIconAndroid();
#elif UNITY_IOS || UNITY_IPHONE
            setIconIOS();
#endif
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: setIcon ex=" + ex.ToString());
        }
    }

    private void setSpslashLoading(string platformName)
    {
        try
        {
            string pathGetNameSplash = Application.dataPath + $"/IconSplash{GameName}/{platformName}";
            string nameFolderICSplash;
            if (!Directory.Exists(pathGetNameSplash))
            {
                nameFolderICSplash = "IconSplash";
                pathGetNameSplash = Application.dataPath + $"/IconSplash/{platformName}";
            }
            else
            {
                nameFolderICSplash = $"IconSplash{GameName}";
            }
            string[] listFileinSplash = Directory.GetFiles(pathGetNameSplash);
            string[] splarr = { "", "" };
            int count = 0;
            foreach (var item in listFileinSplash)
            {
                if (!item.Contains(".meta") && item.ToLower().Contains("splash_"))
                {
                    if (item.ToLower().Contains(".png") || item.ToLower().Contains(".jpg"))
                    {
                        string splashName = Path.GetFileName(item);
                        if (item.ToLower().Contains("_bg"))
                        {
                            splarr[0] = splashName;
                        }
                        else if (item.ToLower().Contains("_logo"))
                        {
                            splarr[1] = splashName;
                        }
                        string pasd = "Assets" + $"/{nameFolderICSplash}/{platformName}/{splashName}";
                        TextureImporter importer = AssetImporter.GetAtPath(pasd) as TextureImporter;
                        importer.textureType = TextureImporterType.Sprite;
                        importer.isReadable = true;
                        AssetDatabase.ImportAsset(pasd, ImportAssetOptions.ForceUpdate);
                        count++;
                        if (count >= 2)
                        {
                            break;
                        }
                    }
                }
            }

            string[] splashGuiid = { "", "" };
            for (int klp = 0; klp < 2; klp++)
            {
                string pathMetaSplash = Application.dataPath + $"/{nameFolderICSplash}/{platformName}/{splarr[klp]}.meta";
                string[] allLinesMeta = File.ReadAllLines(pathMetaSplash);
                foreach (var line in allLinesMeta)
                {
                    if (line.Contains("guid:"))
                    {
                        splashGuiid[klp] = line.Substring(6);
                        break;
                    }
                }
            }

            string pathPrefab = Application.dataPath + "/GamePlugin/Resources/Popup/PopupSplashLoading.prefab";
            string[] allLines = File.ReadAllLines(pathPrefab);
            bool flaggetdata = false;
            GameOb4Addrf obdata = new GameOb4Addrf();
            int countrf = 0;
            int flagAddRf = 0;
            int subflagAddRf = 0;
            for (int p = 0; p < allLines.Length; p++)
            {
                string line = allLines[p];
                if (flaggetdata)
                {
                    if (line.StartsWith("  m_Name:"))
                    {
                        flaggetdata = false;
                        subflagAddRf = 0;
                        string fileid = getValueMeta(line, "m_Name", "");
                        if (fileid.Equals("Bg"))
                        {
                            countrf++;
                            flagAddRf = 1;
                        }
                        else if (fileid.Equals("Logo"))
                        {
                            countrf++;
                            flagAddRf = 2;
                        }
                    }
                    else if (line.Contains("component"))
                    {
                        string fileid = getValueMeta(line, "component", "fileID");
                        obdata.listComponent.Add(fileid);
                    }
                }
                else
                {
                    if (line.StartsWith("  m_Component:"))
                    {
                        flaggetdata = true;
                        obdata.listComponent.Clear();
                    }
                    else
                    {
                        if (flagAddRf == 1)
                        {
                            if (line.StartsWith("--- !u!114 &"))
                            {
                                for (int i = 0; i < obdata.listComponent.Count; i++)
                                {
                                    if (line.Contains(obdata.listComponent[i]))
                                    {
                                        subflagAddRf = 1;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (subflagAddRf == 1)
                                {
                                    if (line.Contains("m_Sprite:"))
                                    {
                                        string grid = getValueMeta(line, "m_Sprite", "guid");
                                        allLines[p] = allLines[p].Replace(grid, splashGuiid[0]);
                                        Debug.Log("aaab=" + allLines[p]);
                                        subflagAddRf = 0;
                                        flagAddRf = 0;
                                        if (countrf >= 2)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (flagAddRf == 2)
                        {
                            if (line.StartsWith("--- !u!114 &"))
                            {
                                for (int i = 0; i < obdata.listComponent.Count; i++)
                                {
                                    if (line.Contains(obdata.listComponent[i]))
                                    {
                                        subflagAddRf = 1;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (subflagAddRf == 1)
                                {
                                    if (line.Contains("m_Sprite:"))
                                    {
                                        string grid = getValueMeta(line, "m_Sprite", "guid");
                                        allLines[p] = allLines[p].Replace(grid, splashGuiid[1]);
                                        Debug.Log("aaab=" + allLines[p]);
                                        subflagAddRf = 0;
                                        flagAddRf = 0;
                                        if (countrf >= 2)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            File.WriteAllLines(pathPrefab, allLines);
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: setSpslashLoading ex=" + ex.ToString());
        }
    }

    private string getValueMeta(string data, string key, string child)
    {
        int n = data.IndexOf(key);
        if (n > 0)
        {
            if (child == null || child.Length <= 0)
            {
                int nv = n + key.Length + 2;
                string va = data.Substring(nv, data.Length - nv);
                // Debug.Log("aaa=" + va);
                return va;
            }
            else
            {
                int nv = n + key.Length + 3;
                string va = data.Substring(nv, data.Length - nv - 1);
                // Debug.Log("aaa1=" + va);
                string[] llll = va.Split(',');
                for (int i = 0; i < llll.Length; i++)
                {
                    if (llll[i].Contains(child))
                    {
                        n = llll[i].IndexOf(child);
                        nv = n + child.Length + 2;
                        va = llll[i].Substring(nv, llll[i].Length - nv);
                        // Debug.Log("aaa1=" + va);
                        return va;
                    }
                }
            }
        }

        return "";
    }
    private void setIconIOS()
    {
#if UNITY_IOS || UNITY_IPHONE
        string pathgetname;
        string nameFolderICSplash;
        // if (Application.platform == RuntimePlatform.OSXEditor)
        {
            pathgetname = Application.dataPath + $"/IconSplash{GameName}/iOS";
            if (!Directory.Exists(pathgetname))
            {
                nameFolderICSplash = "IconSplash";
                pathgetname = Application.dataPath + $"/IconSplash/iOS";
            }
            else
            {
                nameFolderICSplash = $"IconSplash{GameName}";
            }
        }
        // else
        // {
        //     pathgetname = Application.dataPath + "\\IconSplash\\iOS";
        // }
        string[] arrname = Directory.GetFiles(pathgetname);
        string nameIcon = "";
        foreach (var item in arrname)
        {
            if (!item.Contains(".meta"))
            {
                String namefile = Path.GetFileName(item);
                if (namefile.ToLower().Contains("icon") && (namefile.ToLower().Contains("png") || namefile.ToLower().Contains("jpg")))
                {
                    nameIcon = Path.GetFileName(item);
                    break;
                }
            }
        }
        if (nameIcon.Length <= 1)
        {
            return;
        }
        string ppp = $"Assets/{nameFolderICSplash}/iOS/{nameIcon}";
        var tt = AssetDatabase.LoadAssetAtPath<Texture2D>(ppp);

        PlatformIcon[] Applications = PlayerSettings.GetPlatformIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Application);
        for (int i = 0; i < Applications.Length; i++)
        {
            if (Applications[i].width == 180 || Applications[i].width == 167)
            {
                Applications[i].SetTextures(new Texture2D[] { tt });
            }
        }
        PlayerSettings.SetPlatformIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Application, Applications);
        //
        PlatformIcon[] Spotlights = PlayerSettings.GetPlatformIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Spotlight);
        for (int i = 0; i < Spotlights.Length; i++)
        {
            if (Spotlights[i].width == 120 || (Spotlights[i].width == 80 && Spotlights[i].ToString().ToLower().Contains("ipad")))
            {
                Spotlights[i].SetTextures(new Texture2D[] { tt });
            }
        }
        PlayerSettings.SetPlatformIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Spotlight, Spotlights);
        //
        PlatformIcon[] Settingss = PlayerSettings.GetPlatformIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Settings);
        for (int i = 0; i < Settingss.Length; i++)
        {
            if (Settingss[i].width == 87 || (Settingss[i].width == 58 && Settingss[i].ToString().ToLower().Contains("ipad")))
            {
                Settingss[i].SetTextures(new Texture2D[] { tt });
            }
        }
        PlayerSettings.SetPlatformIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Settings, Settingss);
        //
        PlatformIcon[] Notifications = PlayerSettings.GetPlatformIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Notification);
        for (int i = 0; i < Notifications.Length; i++)
        {
            if (Notifications[i].width == 60 || (Notifications[i].width == 40 && Notifications[i].ToString().ToLower().Contains("ipad")))
            {
                Notifications[i].SetTextures(new Texture2D[] { tt });
            }
        }
        PlayerSettings.SetPlatformIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Notification, Notifications);
        //
        PlatformIcon[] Marketings = PlayerSettings.GetPlatformIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Marketing);
        for (int i = 0; i < Marketings.Length; i++)
        {
            if (Marketings[i].width == 1024)
            {
                Marketings[i].SetTextures(new Texture2D[] { tt });
            }
        }
        PlayerSettings.SetPlatformIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Marketing, Marketings);
        //
        arrname = Directory.GetFiles(pathgetname);
        string nameSplash = "";
        string nameStoryboardiPhone = "";
        string nameStoryboardiPad = "";
        foreach (var item in arrname)
        {
            if (!item.Contains(".meta"))
            {
                if (item.ToLower().Contains("splash") && (item.ToLower().Contains(".png") || item.ToLower().Contains(".jpg")))
                {
                    nameSplash = Path.GetFileName(item);
                }
                else if (item.Contains(".storyboard"))
                {
                    if (item.Contains("iPad"))
                    {
                        nameStoryboardiPad = Path.GetFileName(item);
                    }
                    else
                    {
                        nameStoryboardiPhone = Path.GetFileName(item);
                    }
                }
            }
        }
        if (nameStoryboardiPad.Length == 0)
        {
            nameStoryboardiPad = nameStoryboardiPhone;
        }
        else if (nameStoryboardiPhone.Length == 0)
        {
            nameStoryboardiPhone = nameStoryboardiPad;
        }

        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.SplashScreen.showUnityLogo = false;

        const string projectSettings = "ProjectSettings/ProjectSettings.asset";
        UnityEngine.Object obj = AssetDatabase.LoadAllAssetsAtPath(projectSettings)[0];
        SerializedObject psObj = new SerializedObject(obj);
        SerializedProperty iosUseStoryboard = psObj.FindProperty("iOSUseLaunchScreenStoryboard");
        if (iosUseStoryboard != null)
        {
            iosUseStoryboard.boolValue = true;
        }
        SerializedProperty iosPathStoryboard = psObj.FindProperty("iOSLaunchScreenCustomStoryboardPath");
        if (iosPathStoryboard != null)
        {
            iosPathStoryboard.stringValue = $"Assets/{nameFolderICSplash}/iOS/{nameStoryboardiPhone}";
        }
        SerializedProperty iosipadPathStoryboard = psObj.FindProperty("iOSLaunchScreeniPadCustomStoryboardPath");
        if (iosipadPathStoryboard != null)
        {
            iosipadPathStoryboard.stringValue = $"Assets/{nameFolderICSplash}/iOS/{nameStoryboardiPad}";
        }

        psObj.ApplyModifiedProperties();

        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.SplashScreen.showUnityLogo = false;
#endif
    }

    private void setIconAndroid()
    {
#if UNITY_ANDROID
        string pathgetname;
        string nameFolderICSplash;
        // if (Application.platform == RuntimePlatform.OSXEditor)
        {
            pathgetname = Application.dataPath + $"/IconSplash{GameName}/Android/mipmap-hdpi";
            if (!Directory.Exists(pathgetname))
            {
                nameFolderICSplash = "IconSplash";
                pathgetname = Application.dataPath + $"/IconSplash/Android/mipmap-hdpi";
            }
            else
            {
                nameFolderICSplash = $"IconSplash{GameName}";
            }
        }
        // else
        // {
        //     pathgetname = Application.dataPath + "\\IconSplash\\Android\\mipmap-hdpi";
        // }
        string[] arrname = Directory.GetFiles(pathgetname);
        string nameAdaptive = "";
        string namebg = "";
        string nameRound = "";
        string nameLegacy = "";
        foreach (var item in arrname)
        {
            if (!item.Contains(".meta"))
            {
                if (item.Contains("_foreground"))
                {
                    nameAdaptive = Path.GetFileName(item);
                }
                else if (item.Contains("_round"))
                {
                    nameRound = Path.GetFileName(item);
                }
                else if (item.Contains("_background"))
                {
                    namebg = Path.GetFileName(item);
                }
                else
                {
                    nameLegacy = Path.GetFileName(item);
                }
            }
        }
        if (nameRound.Length <= 1 || nameAdaptive.Length <= 1 || nameLegacy.Length <= 1)
        {
            return;
        }
        if (namebg.Length == 0)
        {
            namebg = nameAdaptive;
        }
        PlatformIcon[] Adaptives = PlayerSettings.GetPlatformIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Adaptive);
        for (int i = 0; i < Adaptives.Length; i++)
        {
            string ppp = "";
            string pbg = "";
            if (Adaptives[i].width == 432)
            {
                pbg = $"Assets/{nameFolderICSplash}/Android/mipmap-xxxhdpi/{namebg}";
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-xxxhdpi/{nameAdaptive}";
            }
            else if (Adaptives[i].width == 324)
            {
                pbg = $"Assets/{nameFolderICSplash}/Android/mipmap-xxhdpi/{namebg}";
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-xxhdpi/{nameAdaptive}";
            }
            else if (Adaptives[i].width == 216)
            {
                pbg = $"Assets/{nameFolderICSplash}/Android/mipmap-xhdpi/{namebg}";
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-xhdpi/{nameAdaptive}";
            }
            else if (Adaptives[i].width == 162)
            {
                pbg = $"Assets/{nameFolderICSplash}/Android/mipmap-hdpi/{namebg}";
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-hdpi/{nameAdaptive}";
            }
            else if (Adaptives[i].width == 108)
            {
                pbg = $"Assets/{nameFolderICSplash}/Android/mipmap-mdpi/{namebg}";
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-mdpi/{nameAdaptive}";
            }
            if (ppp.Length > 0)
            {
                var tbg = AssetDatabase.LoadAssetAtPath<Texture2D>(pbg);
                var tt = AssetDatabase.LoadAssetAtPath<Texture2D>(ppp);
                Adaptives[i].SetTextures(new Texture2D[] { tbg, tt });
            }
            else
            {
                Adaptives[i].SetTexture(null);
            }
        }
        PlayerSettings.SetPlatformIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Adaptive, Adaptives);
        //
        PlatformIcon[] Rounds = PlayerSettings.GetPlatformIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Round);
        for (int i = 0; i < Rounds.Length; i++)
        {
            string ppp = "";
            if (Rounds[i].width == 192)
            {
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-xxxhdpi/{nameRound}";
            }
            else if (Rounds[i].width == 144)
            {
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-xxhdpi/{nameRound}";
            }
            else if (Rounds[i].width == 96)
            {
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-xhdpi/{nameRound}";
            }
            else if (Rounds[i].width == 72)
            {
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-hdpi/{nameRound}";
            }
            else if (Rounds[i].width == 48)
            {
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-mdpi/{nameRound}";
            }
            if (ppp.Length > 0)
            {
                var tt = AssetDatabase.LoadAssetAtPath<Texture2D>(ppp);
                Rounds[i].SetTexture(tt);
            }
            else
            {
                Rounds[i].SetTexture(null);
            }
        }
        PlayerSettings.SetPlatformIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Round, Rounds);
        //
        PlatformIcon[] Legacys = PlayerSettings.GetPlatformIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Legacy);
        for (int i = 0; i < Legacys.Length; i++)
        {
            string ppp = "";
            if (Legacys[i].width == 192)
            {
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-xxxhdpi/{nameLegacy}";
            }
            else if (Legacys[i].width == 144)
            {
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-xxhdpi/{nameLegacy}";
            }
            else if (Legacys[i].width == 96)
            {
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-xhdpi/{nameLegacy}";
            }
            else if (Legacys[i].width == 72)
            {
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-hdpi/{nameLegacy}";
            }
            else if (Legacys[i].width == 48)
            {
                ppp = $"Assets/{nameFolderICSplash}/Android/mipmap-mdpi/{nameLegacy}";
            }
            if (ppp.Length > 0)
            {
                var tt = AssetDatabase.LoadAssetAtPath<Texture2D>(ppp);
                Legacys[i].SetTexture(tt);
            }
            else
            {
                Legacys[i].SetTexture(null);
            }
        }
        PlayerSettings.SetPlatformIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Legacy, Legacys);


        // if (Application.platform == RuntimePlatform.OSXEditor)
        {
            pathgetname = Application.dataPath + $"/{nameFolderICSplash}/Android";
        }
        // else
        // {
        //     pathgetname = Application.dataPath + "\\IconSplash\\Android";
        // }
        arrname = Directory.GetFiles(pathgetname);
        string nameSplashNative = "";
        foreach (var item in arrname)
        {
            if (!item.Contains(".meta"))
            {
                if (item.ToLower().Contains("splash_native"))
                {
                    nameSplashNative = Path.GetFileName(item);
                    break;
                }
            }
        }
        if (nameSplashNative.Length > 5)
        {
            PlayerSettings.SplashScreen.show = false;
            PlayerSettings.SplashScreen.showUnityLogo = false;
            var companyLogo = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/{nameFolderICSplash}/Android/{nameSplashNative}");
            PlayerSettings.virtualRealitySplashScreen = companyLogo;
            PlayerSettings.Android.splashScreenScale = AndroidSplashScreenScale.ScaleToFill;

            const string projectSettings = "ProjectSettings/ProjectSettings.asset";
            UnityEngine.Object obj = AssetDatabase.LoadAllAssetsAtPath(projectSettings)[0];
            SerializedObject psObj = new SerializedObject(obj);
            SerializedProperty androidSplashFileId = psObj.FindProperty("androidSplashScreen.m_FileID");
            if (androidSplashFileId != null)
            {
                androidSplashFileId.intValue = companyLogo.GetInstanceID();
            }
            psObj.ApplyModifiedProperties();
        }
#endif
    }

    void parConfigGame(bool isTest)
    {
        PlayerSettings.stripEngineCode = false;
        PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
        PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
        PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
        PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
        if (isTest)
        {
#if UNITY_ANDROID
            string pathFrom, pto;
            string passet = Application.dataPath;
            string ppro;
            // if (Application.platform == RuntimePlatform.OSXEditor)
            {
                ppro = passet.Replace("/Assets", "");
                pathFrom = passet + "/GamePlugin/Editor/FireBase4Test/fir_config.txt";
                pto = passet + "/GamePlugin/FirConfig/google-services.json";
            }
            // else
            // {
            //     ppro = passet.Replace("\\Assets", "");
            //     ppro = passet.Replace("/Assets", "");
            //     pathFrom = ppro + "\\Mem\\Android\\FirConfigTest\\google-services.json";
            //     pto = Application.dataPath + "\\GamePlugin\\FirConfig";
            // }

            mygame.sdk.AdsHelper adshelper = groupControl.transform.Find("AdvHelper").GetComponent<mygame.sdk.AdsHelper>();

            string pkgtest = packageNameTest;
            if (pkgtest.Length < 3)
            {
                pkgtest = "com.viet.game.app.dev";
            }
            coppyFileWithNewName(pathFrom, pto, pkgtest, true);

            pathFrom = passet + $"/GamePlugin/Editor/ForBuildAndroid_{unityver}/gradleTemplate.properties";
            pto = passet + "/Plugins/Android";
            coppyFile(pathFrom, pto, "", false);

            pathFrom = passet + $"/GamePlugin/Editor/ForBuildAndroid_{unityver}/MyGameRes.androidlib";
            pto = passet + "/Plugins/Android/MyGameRes.androidlib";
            Directory.CreateDirectory(pto);
            if (System.IO.Directory.Exists(pathFrom))
            {
                string[] files = System.IO.Directory.GetFiles(pathFrom);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    var fileName = System.IO.Path.GetFileName(s);
                    var destFile = System.IO.Path.Combine(pto, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, pkgtest);
            PlayerSettings.Android.useCustomKeystore = false;

            adshelper.AdmobAppID4Android = "ca-app-pub-3940256099942544~3347511713";

            adshelper.adsAdmobMy.android_app_id = "ca-app-pub-3940256099942544~3347511713";
            adshelper.adsAdmobMy.android_splash_id = "";
            adshelper.adsAdmobMy.android_bn_id = "ca-app-pub-3940256099942544/6300978111";
            adshelper.adsAdmobMy.android_full_id = "ca-app-pub-3940256099942544/1033173712";
            adshelper.adsAdmobMy.android_gift_id = "ca-app-pub-3940256099942544/5224354917";

            adshelper.adsAdmobLower.android_app_id = "ca-app-pub-3940256099942544~3347511713";
            adshelper.adsAdmobLower.android_full_id = "ca-app-pub-3940256099942544/1033173712";
            SetAppCf("public const string appid", $"\"{pkgtest}\"");

            EditorUtility.SetDirty(adshelper.adsAdmobMy);
            EditorUtility.SetDirty(adshelper.adsAdmobLower);
            EditorUtility.SetDirty(adshelper);

            // PlayerSettings.Android.useCustomKeystore = true;
            // PlayerSettings.Android.keyaliasName = "stic_dra_figh_w_al";
            // PlayerSettings.Android.keystorePass = "xgamest@123";
            // PlayerSettings.Android.keyaliasPass = "xgamest@123";

            string dft = "";
            if (isTestInApp)
            {
                dft = "ENABLE_ADS_ADMOB;USE_ADSMOB_MY;ENABLE_INAPP;ENABLE_TEST_INAPP;ENABLE_MYLOG";
            }
            else
            {
                dft = "ENABLE_ADS_ADMOB;USE_ADSMOB_MY;ENABLE_MYLOG";
            }
            if (DefineSymbolAdd.Length > 2)
            {
                dft = DefineSymbolAdd + ";" + dft;
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, dft);
#endif      
        }
        else
        {
            string pathFileConfig;
            string passet = Application.dataPath;
            string ppro;
            string pathFrom, pto;
            //if (Application.platform == RuntimePlatform.OSXEditor)
            {
                ppro = passet.Replace("/Assets", "");
#if UNITY_IOS || UNITY_IPHONE
                pathFileConfig = "/iOS/GameConfigiOS.json";
                pathFrom = "/iOS/GoogleService-Info.plist";
#else
                pathFileConfig = "/Android/GameConfigAndroid.json";
                pathFrom = "/Android/google-services.json";
#endif
                string pptt = ppro + $"/Mem{GameName}" + pathFileConfig;
                if (!File.Exists(pptt))
                {
                    pathFileConfig = ppro + $"/Mem" + pathFileConfig;
                    pathFrom = ppro + $"/Mem" + pathFrom;
                }
                else
                {
                    pathFileConfig = pptt;
                    pathFrom = ppro + $"/Mem{GameName}" + pathFrom;
                }
                pto = Application.dataPath + "/GamePlugin/FirConfig";
            }
            //             else
            //             {
            //                 ppro = passet.Replace("\\Assets", "");
            //                 ppro = passet.Replace("/Assets", "");
            // #if UNITY_IOS || UNITY_IPHONE
            //                 pathFileConfig = ppro + "\\Mem\\iOS\\GameConfigiOS.json";
            //                 pathFrom = ppro + "\\Mem\\iOS\\GoogleService-Info.plist";
            // #else
            //                 pathFileConfig = ppro + "\\Mem\\Android\\GameConfigAndroid.json";
            //                 pathFrom = ppro + "\\Mem\\Android\\google-services.json";
            // #endif
            //                 pto = Application.dataPath + "\\GamePlugin\\FirConfig";
            //             }
            coppyFile(pathFrom, pto, "", false);

            pathFrom = passet + $"/GamePlugin/Editor/ForBuildAndroid_{unityver}/gradleTemplate.properties";
            pto = passet + "/Plugins/Android";
            coppyFile(pathFrom, pto, "", false);

            pathFrom = passet + $"/GamePlugin/Editor/ForBuildAndroid_{unityver}/MyGameRes.androidlib";
            pto = passet + "/Plugins/Android/MyGameRes.androidlib";
            Directory.CreateDirectory(pto);
            if (System.IO.Directory.Exists(pathFrom))
            {
                string[] files = System.IO.Directory.GetFiles(pathFrom);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    var fileName = System.IO.Path.GetFileName(s);
                    var destFile = System.IO.Path.Combine(pto, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }

            mygame.sdk.AdsHelper adshelper = groupControl.transform.Find("AdvHelper").GetComponent<mygame.sdk.AdsHelper>();
            mygame.sdk.AdjustHelper adjustHelper = groupControl.transform.Find("AdjustHelper").GetComponent<mygame.sdk.AdjustHelper>();
            mygame.sdk.AppsFlyerHelperScript FlyerHelper = groupControl.transform.Find("AppFlyerHelper").GetComponent<mygame.sdk.AppsFlyerHelperScript>();
            adjustHelper.gameObject.SetActive(false);
            EditorUtility.SetDirty(adjustHelper);
            FlyerHelper.gameObject.SetActive(false);
            EditorUtility.SetDirty(FlyerHelper);
            // ObjectCfGame ob = new ObjectCfGame();
            string[] allLines = File.ReadAllLines(pathFileConfig);
            string datacf = "";
            foreach (var line in allLines)
            {
                datacf += line;
            }
            var dictmp = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(datacf);
            foreach (KeyValuePair<string, object> item in dictmp)
            {
                if (item.Key.Equals("name"))
                {
                    PlayerSettings.productName = (string)item.Value;
                }
                else if (item.Key.Equals("pkg"))
                {
#if UNITY_IOS || UNITY_IPHONE
                    PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, (string)item.Value);
#else
                    PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, (string)item.Value);
                    SetAppCf("public const string appid", $"\"{(string)item.Value}\"");
#endif
                }
                else if (item.Key.Equals("signcer"))
                {
#if UNITY_ANDROID
                    addValidSigngame((string)item.Value);
#endif
                }
                else if (item.Key.Equals("build_name"))
                {
                    PlayerSettings.bundleVersion = (string)item.Value;
                }
                else if (item.Key.Equals("build_code"))
                {
#if UNITY_IOS || UNITY_IPHONE
                    PlayerSettings.iOS.buildNumber = (string)item.Value;
#else
                    PlayerSettings.Android.bundleVersionCode = int.Parse((string)item.Value);
#endif
                }
                else if (item.Key.Equals("appId"))
                {
#if UNITY_IOS || UNITY_IPHONE
                    SetAppCf("public const string appid", $"\"{(string)item.Value}\"");
                    FlyerHelper.appID = (string)item.Value;
                    EditorUtility.SetDirty(FlyerHelper);
#endif
                }
                else if (item.Key.Equals("GameId"))
                {
                    SetAppCf("public const int gameID", (string)item.Value);
                }
                else if (item.Key.Equals("url_logevent"))
                {
                    SetAppCf("public const string urlLogEvent", $"\"{(string)item.Value}\"");
                }
                else if (item.Key.Equals("keystore"))
                {
                    IDictionary<string, object> keystorecf = (IDictionary<string, object>)item.Value;
                    PlayerSettings.Android.useCustomKeystore = true;
                    foreach (KeyValuePair<string, object> kcf in keystorecf)
                    {
                        if (kcf.Key.Equals("atlas"))
                        {
                            PlayerSettings.Android.keyaliasName = (string)kcf.Value;
                        }
                        else if (kcf.Key.Equals("pass"))
                        {
                            PlayerSettings.Android.keystorePass = (string)kcf.Value;
                            PlayerSettings.Android.keyaliasPass = (string)kcf.Value;
                        }
                    }
                }
                else if (item.Key.Equals("ads"))
                {
                    IDictionary<string, object> adscf = (IDictionary<string, object>)item.Value;
                    foreach (KeyValuePair<string, object> acf in adscf)
                    {
                        if (acf.Key.Equals("ads_max"))
                        {
#if UNITY_IOS || UNITY_IPHONE
                            adshelper.adsApplovinMaxLow.ios_bn_id = "";
                            adshelper.adsApplovinMaxLow.ios_full_id = "";
                            adshelper.adsApplovinMaxLow.ios_gift_id = "";
                            adshelper.adsApplovinMax.ios_bn_cn_id = "";
                            adshelper.adsApplovinMax.ios_full_cn_id = "";
                            adshelper.adsApplovinMax.ios_gift_cn_id = "";
#else
                            adshelper.adsApplovinMaxLow.android_bn_id = "";
                            adshelper.adsApplovinMaxLow.android_full_id = "";
                            adshelper.adsApplovinMaxLow.android_gift_id = "";
#endif
                            IDictionary<string, object> netadscf = (IDictionary<string, object>)acf.Value;
                            foreach (KeyValuePair<string, object> net in netadscf)
                            {
                                if (net.Key.Equals("devKey"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.adsApplovinMax.ios_app_id = (string)net.Value;
                                    if (adshelper.adsApplovinMaxLow != null)
                                    {
                                        adshelper.adsApplovinMaxLow.ios_app_id = (string)net.Value;
                                    }
                                    addMaxConfig("qwerq123", "");
#else
                                    adshelper.adsApplovinMax.android_app_id = (string)net.Value;
                                    if (adshelper.adsApplovinMaxLow != null)
                                    {
                                        adshelper.adsApplovinMaxLow.android_app_id = (string)net.Value;
                                    }
                                    addMaxConfig("qwerq123", "");
#endif
                                }
                                else if (net.Key.Equals("banner"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.adsApplovinMax.ios_bn_id = (string)net.Value;
#else
                                    adshelper.adsApplovinMax.android_bn_id = (string)net.Value;
#endif
                                }
                                else if (net.Key.Equals("full"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.adsApplovinMax.ios_full_id = (string)net.Value;
#else
                                    adshelper.adsApplovinMax.android_full_id = (string)net.Value;
#endif
                                }
                                else if (net.Key.Equals("gift"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.adsApplovinMax.ios_gift_id = (string)net.Value;
#else
                                    adshelper.adsApplovinMax.android_gift_id = (string)net.Value;
#endif
                                }
                                else if (net.Key.Equals("banner_low"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.adsApplovinMaxLow.ios_bn_id = (string)net.Value;
#else
                                    adshelper.adsApplovinMaxLow.android_bn_id = (string)net.Value;
#endif
                                }
                                else if (net.Key.Equals("full_low"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.adsApplovinMaxLow.ios_full_id = (string)net.Value;
#else
                                    adshelper.adsApplovinMaxLow.android_full_id = (string)net.Value;
#endif
                                }
                                else if (net.Key.Equals("gift_low"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.adsApplovinMaxLow.ios_gift_id = (string)net.Value;
#else
                                    adshelper.adsApplovinMaxLow.android_gift_id = (string)net.Value;
#endif
                                }
                                else if (net.Key.Equals("banner_china"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.adsApplovinMax.ios_bn_cn_id = (string)net.Value;
#endif
                                }
                                else if (net.Key.Equals("full_china"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.adsApplovinMax.ios_full_cn_id = (string)net.Value;
#endif
                                }
                                else if (net.Key.Equals("gift_china"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.adsApplovinMax.ios_gift_cn_id = (string)net.Value;
#endif
                                }
                                else if (net.Key.Equals("amazon"))
                                {
                                    IDictionary<string, object> amazonDic = (IDictionary<string, object>)net.Value;
                                    mygame.sdk.AdsMax maxama = (mygame.sdk.AdsMax)adshelper.adsApplovinMax;
                                    foreach (KeyValuePair<string, object> amazon in amazonDic)
                                    {
                                        if (amazon.Key.Equals("appid"))
                                        {
                                            maxama.amazonAppId = (string)amazon.Value;
                                        }
                                        else if (amazon.Key.Equals("banner"))
                                        {
                                            maxama.amazonBnSlotId = (string)amazon.Value;
                                        }
                                        else if (amazon.Key.Equals("leader"))
                                        {
                                            maxama.amazonBnLeaderSlotId = (string)amazon.Value;
                                        }
                                        else if (amazon.Key.Equals("full"))
                                        {
                                            maxama.amazonInterSlotId = (string)amazon.Value;
                                        }
                                        else if (amazon.Key.Equals("gift"))
                                        {
                                            maxama.amazonVideoRewardedSlotId = (string)amazon.Value;
                                        }
                                    }
                                }
                            }
                            EditorUtility.SetDirty(adshelper.adsApplovinMax);
                            EditorUtility.SetDirty(adshelper.adsApplovinMaxLow);
                        }
                        else if (acf.Key.Equals("ads_iron"))
                        {
                            IDictionary<string, object> netadscf = (IDictionary<string, object>)acf.Value;
                            foreach (KeyValuePair<string, object> net in netadscf)
                            {
                                if (net.Key.Equals("app_id"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.adsIron.ios_app_id = (string)net.Value;
                                    addIronConfig("IOSAppKey", (string)net.Value);
#else
                                    adshelper.adsIron.android_app_id = (string)net.Value;
                                    addIronConfig("AndroidAppKey", (string)net.Value);
#endif
                                }
                            }
                            EditorUtility.SetDirty(adshelper.adsIron);
                        }
                        else if (acf.Key.Equals("ads_admob"))
                        {
                            IDictionary<string, object> netadscf = (IDictionary<string, object>)acf.Value;
                            foreach (KeyValuePair<string, object> net in netadscf)
                            {
                                if (net.Key.Equals("appid"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.AdmobAppID4iOS = (string)net.Value;
                                    adshelper.adsAdmob.ios_app_id = (string)net.Value;
                                    adshelper.adsAdmobLower.ios_app_id = (string)net.Value;
                                    adshelper.adsAdmobMy.ios_app_id = (string)net.Value;
                                    adshelper.adsAdmobMyLower.ios_app_id = (string)net.Value;
                                    adshelper.OpenAdIdiOS = "";
                                    adshelper.adsAdmob.ios_splash_id = "";
                                    adshelper.adsAdmobMy.ios_splash_id = "";
                                    addMaxConfig("adMobIosAppId", adshelper.AdmobAppID4iOS);
                                    addIronConfig("AdmobIOSAppId", adshelper.AdmobAppID4iOS);
#else
                                    adshelper.AdmobAppID4Android = (string)net.Value;
                                    adshelper.adsAdmob.android_app_id = (string)net.Value;
                                    adshelper.adsAdmobLower.android_app_id = (string)net.Value;
                                    adshelper.adsAdmobMy.android_app_id = (string)net.Value;
                                    adshelper.adsAdmobMyLower.android_app_id = (string)net.Value;
                                    adshelper.OpenAdIdAndroid = "";
                                    adshelper.adsAdmob.android_splash_id = "";
                                    adshelper.adsAdmobMy.android_splash_id = "";
                                    addMaxConfig("adMobAndroidAppId", adshelper.AdmobAppID4Android);
                                    addIronConfig("AdmobAndroidAppId", adshelper.AdmobAppID4Android);
#endif
                                }
                                else if (net.Key.Equals("native_open_id"))
                                {
#if UNITY_IOS || UNITY_IPHONE
                                    adshelper.OpenAdIdiOS = (string)net.Value;
#else
                                    adshelper.OpenAdIdAndroid = (string)net.Value;
#endif
                                }
                                else if (net.Key.Equals("open_ads"))
                                {
#if UNITY_IOS || UNITY_IPHONE

#if USE_ADSMOB_MY
                                    adshelper.adsAdmobMyLower.ios_full_id = (string)net.Value;
#else
#endif
                                    adshelper.adsAdmobLower.ios_full_id = (string)net.Value;
#else

#if USE_ADSMOB_MY
                                    adshelper.adsAdmobMyLower.android_full_id = (string)net.Value;
#else
#endif
                                    adshelper.adsAdmobLower.android_full_id = (string)net.Value;
#endif
                                }
                                else if (net.Key.Equals("banner"))
                                {
#if UNITY_IOS || UNITY_IPHONE

#if USE_ADSMOB_MY
                                    adshelper.adsAdmobMy.ios_bn_id = (string)net.Value;
#else
                                    adshelper.adsAdmob.ios_bn_id = (string)net.Value;
#endif
                                    
#else

#if USE_ADSMOB_MY
                                    adshelper.adsAdmobMy.android_bn_id = (string)net.Value;
#else
                                    adshelper.adsAdmob.android_bn_id = (string)net.Value;
#endif

#endif
                                }
                                else if (net.Key.Equals("full"))
                                {
#if UNITY_IOS || UNITY_IPHONE

#if USE_ADSMOB_MY
                                    adshelper.adsAdmobMy.ios_full_id = (string)net.Value;
#else
                                    adshelper.adsAdmob.ios_full_id = (string)net.Value;
#endif
                                    
#else

#if USE_ADSMOB_MY
                                    adshelper.adsAdmobMy.android_full_id = (string)net.Value;
#else
                                    adshelper.adsAdmob.android_full_id = (string)net.Value;
#endif

#endif
                                }
                                else if (net.Key.Equals("rewarded"))
                                {
#if UNITY_IOS || UNITY_IPHONE

#if USE_ADSMOB_MY
                                    adshelper.adsAdmobMy.ios_gift_id = (string)net.Value;
#else
                                    adshelper.adsAdmob.ios_gift_id = (string)net.Value;
#endif
                                    
#else

#if USE_ADSMOB_MY
                                    adshelper.adsAdmobMy.android_gift_id = (string)net.Value;
#else
                                    adshelper.adsAdmob.android_gift_id = (string)net.Value;
#endif

#endif
                                }
                                EditorUtility.SetDirty(adshelper.adsAdmob);
                                EditorUtility.SetDirty(adshelper.adsAdmobLower);
                                EditorUtility.SetDirty(adshelper.adsAdmobMy);
                                EditorUtility.SetDirty(adshelper.adsAdmobMyLower);
                                EditorUtility.SetDirty(adshelper);
                            }
                            if (adshelper.adsAdmobMy.GetComponent<mygame.sdk.MediationAdmobMy>() != null)
                            {
                                addAdmobMyMediation(adshelper.adsAdmobMy.GetComponent<mygame.sdk.MediationAdmobMy>());
                            }
                        }
                    }
                }
                else if (item.Key.Equals("gadsme"))
                {
                    IDictionary<string, object> adcanvas = (IDictionary<string, object>)item.Value;
                    string apikey = "";
                    List<object> listplm = null;
                    foreach (KeyValuePair<string, object> acf in adcanvas)
                    {
                        if (acf.Key.Equals("apikey"))
                        {
                        }
                        else if (acf.Key.Equals("placements"))
                        {
                            listplm = (List<object>)acf.Value;
                        }
                    }
                    setupGadsme(apikey, listplm);
                }
                else if (item.Key.Equals("adverty"))
                {
                    IDictionary<string, object> adcanvas = (IDictionary<string, object>)item.Value;
                    foreach (KeyValuePair<string, object> acf in adcanvas)
                    {
                        if (acf.Key.Equals("apikey"))
                        {
                            setupAdverty((string)acf.Value);
                        }
                    }
                }
                else if (item.Key.Equals("fb"))
                {
                    IDictionary<string, object> adjcf = (IDictionary<string, object>)item.Value;
                    foreach (KeyValuePair<string, object> ajcf in adjcf)
                    {
                        if (ajcf.Key.Equals("appid"))
                        {
                            if (((string)ajcf.Value).Length > 5)
                            {
                                addFacebookConfig("appIds:", (string)ajcf.Value);
                            }
                        }
                    }
                    EditorUtility.SetDirty(FlyerHelper);
                }
                else if (item.Key.Equals("adjust"))
                {
                    IDictionary<string, object> adjcf = (IDictionary<string, object>)item.Value;
                    foreach (KeyValuePair<string, object> ajcf in adjcf)
                    {
                        if (ajcf.Key.Equals("app_id"))
                        {
                            adjustHelper.gameObject.SetActive(true);
#if UNITY_IOS || UNITY_IPHONE
                            adjustHelper.appTokeniOS = (string)ajcf.Value;
#else
                            adjustHelper.appTokenAndroid = (string)ajcf.Value;
#endif
                        }
                        else if (ajcf.Key.Equals("events"))
                        {
                            var evItems = (Dictionary<string, object>)ajcf.Value;
                            List<mygame.sdk.MyAdjustEvent> myAdjustEvents = adjustHelper.listEvent;
                            foreach (var newItem in evItems)
                            {
#if UNITY_IOS || UNITY_IPHONE
                                bool ishas = false;
                                for (int l = 0; l < myAdjustEvents.Count; l++)
                                {
                                    if (myAdjustEvents[l].name.CompareTo(newItem.Key) == 0)
                                    {
                                        ishas = true;
                                        myAdjustEvents[l].TokenIOS = (string)newItem.Value;
                                    }
                                }
                                if (!ishas)
                                {
                                    myAdjustEvents.Add(new mygame.sdk.MyAdjustEvent(newItem.Key, "", (string)newItem.Value));
                                }
#else
                                bool ishas = false;
                                for (int l = 0; l < myAdjustEvents.Count; l++)
                                {
                                    if (myAdjustEvents[l].name.CompareTo(newItem.Key) == 0)
                                    {
                                        ishas = true;
                                        myAdjustEvents[l].TokenAndroid = (string)newItem.Value;
                                        break;
                                    }
                                }
                                if (!ishas)
                                {
                                    myAdjustEvents.Add(new mygame.sdk.MyAdjustEvent(newItem.Key, (string)newItem.Value, ""));
                                }
#endif
                            }
                            adjustHelper.listEvent = myAdjustEvents;
                        }
                    }
                    EditorUtility.SetDirty(adjustHelper);
                }
                else if (item.Key.Equals("appflyer"))
                {
                    IDictionary<string, object> adjcf = (IDictionary<string, object>)item.Value;
                    foreach (KeyValuePair<string, object> ajcf in adjcf)
                    {
                        if (ajcf.Key.Equals("Dev_key"))
                        {
                            FlyerHelper.gameObject.SetActive(true);
                            FlyerHelper.getConversionData = true;
#if UNITY_IOS || UNITY_IPHONE
                            FlyerHelper.devKey = (string)ajcf.Value;
#else
                            FlyerHelper.devKey = (string)ajcf.Value;
#endif
                            customePodAppsflyeriOS();
                        }
                        else if (ajcf.Key.Equals("uwp_app_id"))
                        {
#if UNITY_IOS || UNITY_IPHONE
                            FlyerHelper.UWPAppID = (string)ajcf.Value;
#else
                            FlyerHelper.UWPAppID = (string)ajcf.Value;
#endif
                        }
                    }
                    EditorUtility.SetDirty(FlyerHelper);
                }
                else if (item.Key.Equals("define"))
                {
                    string dft = (string)item.Value;
                    if (DefineSymbolAdd.Length > 2)
                    {
                        dft = dft + ";" + DefineSymbolAdd;
                    }
                    if (dft.Contains("ENABLE_ADCANVAS"))
                    {
                        isAdcanvas = true;
                    }
                    else
                    {
                        isAdcanvas = false;
                    }
                    isIronEnable = false;
                    if (dft.Contains("ENABLE_ADS_IRON"))
                    {
                        isIronEnable = true;
                    }
#if UNITY_IOS || UNITY_IPHONE
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, dft);
#else
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, dft);
#endif
                }
                else if (item.Key.Equals("base_64"))
                {
#if UNITY_ANDROID
                    addValidIapAndroid((string)item.Value);
#elif UNITY_IOS || UNITY_IPHONE
#endif
                }
            }
#if UNITY_IOS || UNITY_IPHONE
            SetAppCf("public const int verapp", PlayerSettings.iOS.buildNumber);
            SetPropertyFile("Assets/GamePlugin/GameHelper/GameHelper.cs", "private const long Day_len_Luc", "" + (mygame.sdk.SdkUtil.systemCurrentMiliseconds() / 1000));
#else
            SetAppCf("public const int verapp", PlayerSettings.Android.bundleVersionCode.ToString());
#endif
        }
#if UNITY_ANDROID
        setup4Adcanvas();
#endif
        // return ob;
    }

    void customePodAppsflyeriOS()
    {
        try
        {
            string passet = Application.dataPath + "/AppsFlyer/Editor/AppsFlyerAdrevenueDependencies.xml";
            if (File.Exists(passet))
            {
                var appCf = File.ReadAllLines(passet);
                bool isw = false;
                for (int i = 0; i < appCf.Length; i++)
                {
                    if (appCf[i].Contains("<iosPod name=\"AppsFlyer-AdRevenue-MoPub\""))
                    {
                        isw = true;
                        appCf[i] = "    <iosPod name=\"AppsFlyer-AdRevenue\" minTargetSdk=\"10.0\">";
                    }
                }
                if (isw)
                {
                    File.WriteAllLines(passet, appCf);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: customePodAppsflyeriOS ex=" + ex.ToString());
        }
    }

    void addValidIapAndroid(string bkey)
    {
        try
        {
            if (forceGenBaseKey)
            {
                forceGenBaseKey = false;
                string pathfile = "Assets/GamePlugin/Inapp/InappHelper.cs";
                var appCf = File.ReadAllLines(pathfile);
                string linemakey = "                byte[] makey = { 0 };";
                string linepaskey = "                int[] paskey = { 0 };";
                string linepasva = "                byte[] pasva = { 0 };";

                if (bkey != null && bkey.Length > 5)
                {
                    byte[] makey;
                    int[] paskey;
                    byte[] pasva;
                    mygame.sdk.SdkUtil.myMaHoa(bkey, out makey, out paskey, out pasva);
                    //==========================
                    linemakey = "                byte[] makey = { " + makey[0];
                    for (int j = 1; j < makey.Length; j++)
                    {
                        linemakey += ", " + makey[j];
                    }
                    linemakey += " };";
                    //==========================
                    linepaskey = "                int[] paskey = { " + paskey[0];
                    for (int j = 1; j < paskey.Length; j++)
                    {
                        linepaskey += ", " + paskey[j];
                    }
                    linepaskey += " };";
                    //==========================
                    linepasva = "                byte[] pasva = { " + pasva[0];
                    for (int j = 1; j < pasva.Length; j++)
                    {
                        linepasva += ", " + pasva[j];
                    }
                    linepasva += " };";
                }
                for (int i = 0; i < appCf.Length; i++)
                {
                    if (appCf[i].Contains("byte[] makey = {"))
                    {
                        appCf[i] = linemakey;
                    }
                    else if (appCf[i].Contains("int[] paskey = {"))
                    {
                        appCf[i] = linepaskey;
                    }
                    else if (appCf[i].Contains("byte[] pasva = {"))
                    {
                        appCf[i] = linepasva;
                    }
                }
                File.WriteAllLines(pathfile, appCf);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: addValidIapAndroid=" + ex.ToString());
        }
    }

    void addValidSigngame(string bkey)
    {
        try
        {
            if (forceGenBaseKey)
            {
                string pathfile = "Assets/GamePlugin/Platforms/Android/GameHelperAndroid.cs";
                var appCf = File.ReadAllLines(pathfile);
                string linemakey = "                    byte[] makey = { 0 };";
                string linepaskey = "                    int[] paskey = { 0 };";
                string linepasva = "                    byte[] pasva = { 0 };";

                if (bkey != null && bkey.Length > 5)
                {
                    byte[] makey;
                    int[] paskey;
                    byte[] pasva;
                    mygame.sdk.SdkUtil.myMaHoa(bkey, out makey, out paskey, out pasva);
                    //==========================
                    linemakey = "                    byte[] makey = { " + makey[0];
                    for (int j = 1; j < makey.Length; j++)
                    {
                        linemakey += ", " + makey[j];
                    }
                    linemakey += " };";
                    //==========================
                    linepaskey = "                    int[] paskey = { " + paskey[0];
                    for (int j = 1; j < paskey.Length; j++)
                    {
                        linepaskey += ", " + paskey[j];
                    }
                    linepaskey += " };";
                    //==========================
                    linepasva = "                    byte[] pasva = { " + pasva[0];
                    for (int j = 1; j < pasva.Length; j++)
                    {
                        linepasva += ", " + pasva[j];
                    }
                    linepasva += " };";
                }
                for (int i = 0; i < appCf.Length; i++)
                {
                    if (appCf[i].Contains("byte[] makey = {"))
                    {
                        appCf[i] = linemakey;
                    }
                    else if (appCf[i].Contains("int[] paskey = {"))
                    {
                        appCf[i] = linepaskey;
                    }
                    else if (appCf[i].Contains("byte[] pasva = {"))
                    {
                        appCf[i] = linepasva;
                    }
                }
                File.WriteAllLines(pathfile, appCf);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: addValidSigngame=" + ex.ToString());
        }
    }

    void addAdmobMyMediation(mygame.sdk.MediationAdmobMy ad)
    {
        try
        {
            string passet = Application.dataPath + "/GamePlugin/AdvHelper/AdmobPlugin/Editor/Dependencies.xml";
            if (ad.Fb || ad.Unity)
            {
                string fdata = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
                fdata += "<dependencies>\r\n";
                fdata += "    <androidPackages>\r\n";
                if (ad.Fb)
                {
                    fdata += "        <androidPackage spec=\"com.google.ads.mediation:facebook:6.8.0.0\"/>\r\n";
                }
                if (ad.fyber)
                {
                    fdata += "        <androidPackage spec=\"com.google.ads.mediation:fyber:8.1.0.0\"/>\r\n";
                }
                if (ad.Unity)
                {
                    fdata += "        <androidPackage spec=\"com.unity3d.ads:unity-ads:3.7.5\"/>\r\n";
                    fdata += "        <androidPackage spec=\"com.google.ads.mediation:unity:3.7.5.0\"/>\r\n";
                }
                fdata += "    </androidPackages>\r\n";
                fdata += "    <iosPods>\r\n";
                if (ad.Fb)
                {
                    fdata += "        <iosPod name=\"GoogleMobileAdsMediationFacebook\"/>\r\n";
                }
                if (ad.fyber)
                {
                    fdata += "       <iosPod name=\"GoogleMobileAdsMediationFyber\"/>\r\n";
                }
                if (ad.Unity)
                {
                    fdata += "        <iosPod name=\"GoogleMobileAdsMediationUnity\"/>\r\n";
                }
                fdata += "    </iosPods>\r\n";
                fdata += "</dependencies>";
                File.WriteAllText(passet, fdata);
            }
            else
            {
                File.Delete(passet);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: addAdmobMyMediation ex=" + ex.ToString());
        }
    }

    private void addMaxConfig(string key, string value)
    {
        string passet = Application.dataPath + "/MaxSdk/Resources/AppLovinSettings.asset";
        if (File.Exists(passet))
        {
            bool isw = false;
            var appCf = File.ReadAllLines(passet);
            for (int i = 0; i < appCf.Length; i++)
            {
                if (appCf[i].Contains("sdkKey:"))
                {
                    if (!appCf[i].Contains("wGgT4dNmN5bz9i28swF-3T-V2"))
                    {
                        isw = true;
                        appCf[i] = "  sdkKey: wGgT4dNmN5bz9i28swF-3T-V2e29pP-f2E2av9SXbpQdHZ0rKLAuxQnkrEuwvFR7Bdzgd7XtPQP6y_XGM_P6_e";
                    }
                }
                else if (appCf[i].Contains(key))
                {
                    isw = true;
                    appCf[i] = $"  {key}: {value}";
                }
            }
            if (isw)
            {
                File.WriteAllLines(passet, appCf);
            }
        }

        passet = Application.dataPath + "/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset";
        if (File.Exists(passet))
        {
            bool isw = false;
            var appCf = File.ReadAllLines(passet);
            if (key.Contains("admobIos"))
            {
                key = key.Replace("admobIos", "admobIOS");
            }
            for (int i = 0; i < appCf.Length; i++)
            {
                if (appCf[i].Contains(key))
                {
                    isw = true;
                    appCf[i] = $"  {key}: {value}";
                }
            }
            if (isw)
            {
                File.WriteAllLines(passet, appCf);
            }
        }
    }

    private void addIronConfig(string key, string value)
    {
        if (key != null)
        {
            string passet = "";
            bool isappkey = false;
            if (key.Contains("AppKey"))
            {
                isappkey = true;
                passet = Application.dataPath + "/IronSource/Resources/IronSourceMediationSettings.asset";
            }
            else
            {
                passet = Application.dataPath + "/IronSource/Resources/IronSourceMediatedNetworkSettings.asset";
            }
            if (File.Exists(passet))
            {
                bool isw = false;
                var appCf = File.ReadAllLines(passet);
                for (int i = 0; i < appCf.Length; i++)
                {
                    if (isappkey && appCf[i].Contains("EnableIronsourceSDKInitAPI"))
                    {
                        //isw = true;
                        //appCf[i] = "  EnableIronsourceSDKInitAPI: 1";
                    }
                    if (isappkey && appCf[i].Contains("AddIronsourceSkadnetworkID"))
                    {
                        isw = true;
                        appCf[i] = "  AddIronsourceSkadnetworkID: 1";
                    }
                    if (isappkey && appCf[i].Contains("DeclareAD_IDPermission"))
                    {
                        isw = true;
                        appCf[i] = "  DeclareAD_IDPermission: 1";
                    }
                    if (!isappkey && appCf[i].Contains("EnableAdmob"))
                    {
                        isw = true;
                        appCf[i] = "  EnableAdmob: 1";
                    }
                    else if (appCf[i].Contains(key))
                    {
                        isw = true;
                        appCf[i] = $"  {key}: {value}";
                    }
                }
                if (isw)
                {
                    File.WriteAllLines(passet, appCf);
                }
            }
            if (key != null)
            {
                passet = "";
                if (key.StartsWith("Admob"))
                {
                    key = key.Replace("Admob", "admob");
                    passet = Application.dataPath + "/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset";
                    if (File.Exists(passet))
                    {
                        bool isw = false;
                        var appCf = File.ReadAllLines(passet);
                        for (int i = 0; i < appCf.Length; i++)
                        {
                            if (appCf[i].Contains(key))
                            {
                                isw = true;
                                appCf[i] = $"  {key}: {value}";
                            }
                        }
                        if (isw)
                        {
                            File.WriteAllLines(passet, appCf);
                        }
                    }
                }
            }
        }
    }

    private void addFacebookConfig(string key, string value)
    {
        string passet = Application.dataPath + "/FacebookSDK/SDK/Resources/FacebookSettings.asset";
        if (File.Exists(passet))
        {
            bool isw = false;
            var appCf = File.ReadAllLines(passet);
            for (int i = 0; i < appCf.Length; i++)
            {
                if (appCf[i].Contains(key))
                {
                    i++;
                    isw = true;
                    appCf[i] = $"  - {value}";
                }
                else if (appCf[i].Contains("appLabels:") && isw)
                {
                    i++;
                    if (!PlayerSettings.productName.Contains(":"))
                    {
                        appCf[i] = $"  - {PlayerSettings.productName}";
                    }
                    else
                    {
                        appCf[i] = $"  - '{PlayerSettings.productName}'";
                    }
                }
            }
            if (isw)
            {
                File.WriteAllLines(passet, appCf);
            }
        }

        passet = Application.dataPath + "/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset";
        if (File.Exists(passet))
        {
            bool isw = false;
            var appCf = File.ReadAllLines(passet);
            for (int i = 0; i < appCf.Length; i++)
            {
                if (appCf[i].Contains(key))
                {
                    isw = true;
                    appCf[i] = $"  {key}: {value}";
                }
            }
            if (isw)
            {
                File.WriteAllLines(passet, appCf);
            }
        }
    }

    private void setup4Adcanvas()
    {
        string passet = Application.dataPath + "/Plugins/Didomi/Editor/PostProcessor.cs";
        if (File.Exists(passet))
        {
            bool isw = false;
            var appCf = File.ReadAllLines(passet);
            for (int i = 0; i < appCf.Length; i++)
            {
                if (appCf[i].Contains("import android.support.v7.app.AppCompatActivity;"))
                {
                    isw = true;
                    appCf[i] = appCf[i].Replace("import android.support.v7.app.AppCompatActivity;", "import androidx.appcompat.app.AppCompatActivity;");
                }
                else if (!appCf[i].StartsWith("//") && appCf[i].Contains("implementation 'com.android.support:appcompat-v7:27.1.1"))
                {
                    isw = true;
                    appCf[i] = "//" + appCf[i];
                }
                else if (!appCf[i].StartsWith("//") && appCf[i].Contains("implementation 'com.google.android.gms:play-services-ads:15.0.1'"))
                {
                    isw = true;
                    appCf[i] = "//" + appCf[i];
                }
                else if (!appCf[i].StartsWith("//") && appCf[i].Contains("implementation 'com.android.support:customtabs:27.1.1'"))
                {
                    isw = true;
                    appCf[i] = "//" + appCf[i];
                }
                else if (!appCf[i].StartsWith("//") && appCf[i].Contains("implementation \"\"android.arch.lifecycle:extensions:1.1.0\"\""))
                {
                    isw = true;
                    appCf[i] = "//" + appCf[i];
                }
                else if (!appCf[i].StartsWith("//") && appCf[i].Contains("implementation 'android.arch.lifecycle:viewmodel:1.1.0'"))
                {
                    isw = true;
                    appCf[i] = "//" + appCf[i];
                }
            }
            if (isw)
            {
                File.WriteAllLines(passet, appCf);
            }
            isw = false;
            string pmani = Application.dataPath + "/Plugins/Android/AndroidManifest.xml";
            string[] lines = System.IO.File.ReadAllLines(pmani);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("com.google.firebase.MessagingUnityPlayerActivity")
                    || lines[i].Contains("com.unity3d.player.UnityPlayerActivity")
                )
                {
                    bool isaddtheme = true;
                    if (lines[i].Contains("android:theme="))
                    {
                        isaddtheme = false;
                        if (!lines[i].Contains(@"android:theme=""@style/DidomiTheme"""))
                        {
                            int idxt = lines[i].IndexOf("android:theme=");
                            int n = lines[i].IndexOf("\"", idxt + 15);
                            string ss = lines[i].Substring(idxt, (n - idxt + 1));
                            lines[i] = lines[i].Replace(ss, @"android:theme=""@style/DidomiTheme""");
                            isw = true;
                        }
                    }
                    else
                    {
                        for (int j = (i + 1); j < lines.Length; j++)
                        {
                            if (lines[j].Contains("android:theme="))
                            {
                                isaddtheme = false;
                                if (!lines[j].Contains(@"android:theme=""@style/DidomiTheme"""))
                                {
                                    int idxt = lines[j].IndexOf("android:theme=");
                                    int n = lines[j].IndexOf("\"", idxt + 15);
                                    string ss = lines[j].Substring(idxt, (n - idxt + 1));
                                    lines[j] = lines[j].Replace(ss, @"android:theme=""@style/DidomiTheme""");
                                    isw = true;
                                }
                                break;
                            }
                            else if (lines[j].Contains("<intent-filter>"))
                            {
                                break;
                            }
                        }
                    }
                    if (isaddtheme && !isw)
                    {
                        lines[i] = lines[i].Replace("Activity\"", "Activity\" android:theme=\"@style/DidomiTheme\"");
                        isw = true;
                    }
                    break;
                }
            }
            if (isw)
            {
                File.WriteAllLines(pmani, lines);
            }
        }
        else
        {
            bool isw = false;
            string pmani = Application.dataPath + "/Plugins/Android/AndroidManifest.xml";
            string[] lines = System.IO.File.ReadAllLines(pmani);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("com.google.firebase.MessagingUnityPlayerActivity")
                    || lines[i].Contains("com.unity3d.player.UnityPlayerActivity")
                )
                {
                    int sstht = 0;
                    if (lines[i].Contains("com.google.firebase.MessagingUnityPlayerActivity"))
                    {
                        sstht = 1;
                    }
                    if (lines[i].Contains("android:theme="))
                    {
                        if (lines[i].Contains(@"android:theme=""@style/DidomiTheme"""))
                        {
                            if (sstht == 0)
                            {
                                lines[i] = lines[i].Replace(@"android:theme=""@style/DidomiTheme""", @"android:theme=""@style/UnityThemeSelector""");
                            }
                            else
                            {
                                lines[i] = lines[i].Replace(@"android:theme=""@style/DidomiTheme""", @"");
                            }
                            isw = true;
                        }
                    }
                    else
                    {
                        for (int j = (i + 1); j < lines.Length; j++)
                        {
                            if (lines[j].Contains("android:theme="))
                            {
                                if (sstht == 0)
                                {
                                    lines[i] = lines[i].Replace(@"android:theme=""@style/DidomiTheme""", @"android:theme=""@style/UnityThemeSelector""");
                                }
                                else
                                {
                                    lines[i] = lines[i].Replace(@"android:theme=""@style/DidomiTheme""", @"");
                                }
                                isw = true;
                                break;
                            }
                            else if (lines[j].Contains("<intent-filter>"))
                            {
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            if (isw)
            {
                File.WriteAllLines(pmani, lines);
            }
        }
    }

    private void setupGadsme(string apikey, List<object> placemants)
    {
        // string passet = Application.dataPath + "/Gadsme/Resources/GadsmePreferences.asset";
        string passet = Application.dataPath + "/GamePlugin/AdCanvasHelper/AdCanvasHelper.prefab";
        if (File.Exists(passet) && placemants != null && placemants.Count > 0)
        {
            AdCanvasHelper adCanvasHelper = GadsmeObject.FindObjectOfType<AdCanvasHelper>();
            if (adCanvasHelper != null)
            {
                Transform tfgads = adCanvasHelper.transform.Find("GadsmeGroup");
                if (tfgads != null)
                {
                    GadsmeHelper gads = tfgads.GetComponent<GadsmeHelper>();
                    bool haspl = false;
                    for (int i = 0; i < gads.listPlacementsWithType.Count; i++)
                    {
                        if (gads.listPlacementsWithType[i] != null && gads.listPlacementsWithType[i].type.Equals(GadsmePlacementType.P_300x250))
                        {
                            haspl = true;
                            for (int j = 0; j < placemants.Count; j++)
                            {
                                if (j < gads.listPlacementsWithType[i].listPlacements.Count)
                                {
                                    gads.listPlacementsWithType[i].listPlacements[j].placementId = (string)placemants[j];
                                }
                                else
                                {
                                    GadsmePlacementChannel itempl = new GadsmePlacementChannel();
                                    itempl.channel = j + 1;
                                    itempl.placementId = (string)placemants[j];
                                    gads.listPlacementsWithType[i].listPlacements.Add(itempl);
                                }
                            }
                            int countrm = gads.listPlacementsWithType[i].listPlacements.Count - placemants.Count;
                            while (countrm > 0)
                            {
                                countrm--;
                                gads.listPlacementsWithType[i].listPlacements.RemoveAt(placemants.Count + countrm);
                            }
                            break;
                        }
                    }
                    if (!haspl)
                    {
                        GadsmePlacementIdWithType pty = new GadsmePlacementIdWithType();
                        pty.type = GadsmePlacementType.P_300x250;
                        for (int j = 0; j < placemants.Count; j++)
                        {
                            GadsmePlacementChannel itempl = new GadsmePlacementChannel();
                            itempl.channel = j + 1;
                            itempl.placementId = (string)placemants[j];
                            pty.listPlacements.Add(itempl);
                        }
                        gads.listPlacementsWithType.Add(pty);
                    }
                    EditorUtility.SetDirty(gads);
                    EditorUtility.SetDirty(adCanvasHelper);
                }
            }
            bool isw = false;
            var appCf = File.ReadAllLines(passet);
            List<string> allines = new List<string>();
            for (int i = 0; i < appCf.Length; i++)
            {
                allines.Add(appCf[i]);
                if (appCf[i].Contains("listPlacements:"))
                {
                    for (int kk = 0; kk < placemants.Count; kk++)
                    {
                        int cn = kk + 1;
                        allines.Add($"    - placementId: {(string)placemants[kk]}");
                        allines.Add($"      channel: {cn}");
                    }
                    i++;
                    isw = true;
                    for (int kk = i; kk < (i + 50); kk++)
                    {
                        if (appCf[kk].Contains("idx:"))
                        {
                            i = kk;
                            allines.Add(appCf[i]);
                            break;
                        }
                    }
                }
            }
            if (isw)
            {
                File.WriteAllLines(passet, allines);
            }
        }
    }

    private void setupAdverty(string apikey)
    {
        string passet = Application.dataPath + "/Adverty/Resources/Data/AdvertySettings.asset";
        if (File.Exists(passet) && apikey != null && apikey.Length > 5)
        {
            bool isw = false;
            var appCf = File.ReadAllLines(passet);
            for (int i = 0; i < appCf.Length; i++)
            {
                if (appCf[i].Contains("apiKey:"))
                {
                    if (!appCf[i].Contains(apikey))
                    {
                        isw = true;
                        string ss = appCf[i].Substring(0, 10);
                        appCf[i] = ss + apikey;
                    }
                }
                else if (appCf[i].Contains("hostUrl:"))
                {
                    if (!appCf[i].Contains("https"))
                    {
                        isw = true;
                        appCf[i] += "https://adserver.adverty.com/{0}";
                    }
                }
                else if (appCf[i].Contains("isLazyLoadAllowed:"))
                {
                    if (!appCf[i].Contains("0"))
                    {
                        isw = true;
                        appCf[i] = appCf[i].Replace("1", "0");
                    }
                }
            }
            if (isw)
            {
                File.WriteAllLines(passet, appCf);
            }
        }
        passet = Application.dataPath + "/GamePlugin/AdCanvasHelper/Adverty/Scripts/AdvertyHelper.cs";
        bool iswadvhp = false;
        var advertyhp = File.ReadAllLines(passet);
        for (int i = 0; i < advertyhp.Length; i++)
        {
#if UNITY_ANDROID || UNITY_EDITOR_WIN
            if (advertyhp[i].Contains("private const string ANDROID_API_KEY"))
            {
                advertyhp[i] = $"    private const string ANDROID_API_KEY = \"{apikey}\";";
                iswadvhp = true;
                break;
            }
#elif UNITY_IOS || UNITY_EDITOR_OSX
            if (advertyhp[i].Contains("private const string IOS_API_KEY"))
            {
                advertyhp[i] = $"    private const string IOS_API_KEY = \"{apikey}\";";
                iswadvhp = true;
                break;
            }
#endif
        }
        if (iswadvhp)
        {
            File.WriteAllLines(passet, advertyhp);
        }
    }

    public void SetAppCf(string key, string newValue)
    {
        String platf = "asdfasdfadfa";
#if UNITY_IOS || UNITY_IPHONE
        platf = "UNITY_IOS";
#else
        platf = "UNITY_ANDROID";
#endif
        var appCf = File.ReadAllLines("Assets/GamePlugin/GameManager/AppConfig.cs");
        bool flag = false;
        bool isWrite = false;
        for (int i = 0; i < appCf.Length; i++)
        {
            if (!flag && appCf[i].Contains(platf))
            {
                flag = true;
            }
            else if (flag)
            {
                if (appCf[i].Contains(key))
                {
                    bool isallow = true;
                    if (key.Contains("TimeSubmitReview"))
                    {
                        int n1 = appCf[i].IndexOf('=', 10);
                        int n2 = appCf[i].IndexOf(';', n1);
                        if (n1 > 0 && n2 > 0 && n2 > n1)
                        {
                            string stm = appCf[i].Substring(n1 + 2, n2 - n1 - 2);
                            long ltm;
                            if (long.TryParse(stm, out ltm))
                            {
                                Debug.Log("mysdk: setup ios TimeSubmitReview=" + ltm);
                                long tcu = mygame.sdk.SdkUtil.systemCurrentMiliseconds() / 1000;
                                if ((tcu - ltm) <= 4 * 60 * 60)
                                {
                                    isallow = false;
                                }
                            }
                        }
                    }
                    if (isallow)
                    {
                        isWrite = true;
                        appCf[i] = $"    {key} = {newValue};";
                    }
                    break;
                }
            }
        }
        if (isWrite)
        {
            File.WriteAllLines("Assets/GamePlugin/GameManager/AppConfig.cs", appCf);
        }
    }

    public void SetPropertyFile(string pathfile, string key, string newValue)
    {
        try
        {
            var appCf = File.ReadAllLines(pathfile);
            bool isWrite = false;
            for (int i = 0; i < appCf.Length; i++)
            {
                if (appCf[i].Contains(key))
                {
                    bool isallow = true;
                    if (key.Contains("Day_len_Luc"))
                    {
                        int n1 = appCf[i].IndexOf('=', 10);
                        int n2 = appCf[i].IndexOf(';', n1);
                        if (n1 > 0 && n2 > 0 && n2 > n1)
                        {
                            string stm = appCf[i].Substring(n1 + 2, n2 - n1 - 2);
                            long ltm;
                            if (long.TryParse(stm, out ltm))
                            {
                                Debug.Log("mysdk: setup ios TimeSubmitReview=" + ltm);
                                long tcu = mygame.sdk.SdkUtil.systemCurrentMiliseconds() / 1000;
                                if ((tcu - ltm) <= 4 * 60 * 60)
                                {
                                    isallow = false;
                                }
                            }
                        }
                    }
                    if (isallow)
                    {
                        isWrite = true;
                        appCf[i] = $"        {key} = {newValue};";
                    }
                    break;
                }
            }
            if (isWrite)
            {
                File.WriteAllLines(pathfile, appCf);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: SetPropertyFile=" + ex.ToString());
        }
    }

    private void GetSupprtFlag()
    {
        try
        {
            string levelDirectoryPathResFlag;
            string levelDirectoryPathFlagmanager;
            // if (Application.platform == RuntimePlatform.OSXEditor)
            {
                levelDirectoryPathResFlag = Application.dataPath + "/GamePlugin/Resources/Flags/";
                levelDirectoryPathFlagmanager = Application.dataPath + "/GamePlugin/FlagManager/FlagManger.cs";
            }
            // else
            // {
            //     levelDirectoryPathResFlag = Application.dataPath + "\\GamePlugin\\Resources\\Flags\\";
            //     levelDirectoryPathFlagmanager = Application.dataPath + "\\GamePlugin\\FlagManager\\FlagManger.cs";
            // }
            DirectoryInfo d = new DirectoryInfo(levelDirectoryPathResFlag);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files
            string str = "                    string[] arrflags = {";
            bool isbegin = true;
            foreach (FileInfo file in Files)
            {
                if (!file.Name.Contains(".meta"))
                {
                    int idx = file.Name.IndexOf(".");
                    if (idx > 0)
                    {
                        string code = file.Name.Substring(0, idx);
                        if (code != null && code.Length >= 2 && code.Length <= 3)
                        {
                            if (isbegin)
                            {
                                isbegin = false;
                                str += ("\"" + code + "\"");
                            }
                            else
                            {
                                str += (", \"" + code + "\"");
                            }
                        }
                    }
                }
            }
            str += "};";

            string[] lines = System.IO.File.ReadAllLines(levelDirectoryPathFlagmanager);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("string[] arrflags"))
                {
                    lines[i] = str;
                }
            }
            System.IO.File.WriteAllLines(levelDirectoryPathFlagmanager, lines);
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: GetSupprtFlag ex=" + ex.ToString());
        }
    }

    private void WiteStringKey()
    {
        try
        {
            string levelDirectoryPath;
            // if (Application.platform == RuntimePlatform.OSXEditor)
            {
                levelDirectoryPath = Application.dataPath + "/Plugins/Android/MyGameRes.androidlib/Assets/";
            }
            // else
            // {
            //     levelDirectoryPath = Application.dataPath + "\\Plugins\\Android\\Assets\\";
            // }
            Directory.CreateDirectory(levelDirectoryPath);
            string path = levelDirectoryPath + "sowe.dat";
            string[] arrstr = {
                "http",
                "fb:",
                "pkg:",
                "https://play.google.com/store/apps/details?id=",
                "play.google.com/store/apps/details?id=",
                "market://details?id=",
                "https://www.youtube.com/watch?v=",
                "vnd.youtube:"
            };
            int sizrbyte = 1;
            for (int i = 0; i < arrstr.Length; i++)
            {
                sizrbyte += (2 * arrstr[i].Length + 1);
            }
            byte[] bytesadid = new byte[sizrbyte];
            bytesadid[0] = (byte)arrstr.Length;
            int idx = 1;
            for (int i = 0; i < arrstr.Length; i++)
            {
                bytesadid[idx] = (byte)(2 * arrstr[i].Length);
                idx++;
                writestringtobytes(arrstr[i], bytesadid, idx);
                idx += 2 * arrstr[i].Length;
            }
            System.IO.File.WriteAllBytes(path, bytesadid);
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: WiteStringKey ex=" + ex.ToString());
        }
    }

    private void writestringtobytes(string txt, byte[] arr, int idxofarr)
    {
        for (int j = 0; j < txt.Length; j++)
        {
            int va = (byte)txt[j] - 15;
            int va1 = UnityEngine.Random.Range(0, 200);
            arr[idxofarr + 2 * j] = (byte)va1;
            arr[idxofarr + 2 * j + 1] = (byte)va;
        }
    }

    public static void coppyFileNormal(string from, string to)
    {
        try
        {
            byte[] alldata = File.ReadAllBytes(from);
            File.WriteAllBytes(to, alldata);
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: coppyFileNormal ex=" + ex.ToString());
        }
    }

    public static void coppyFolder(string from, string to, int deep)
    {
        if (Directory.Exists(from))
        {
            if (!Directory.Exists(to))
            {
                Directory.CreateDirectory(to);
            }
            string[] listfiles = Directory.GetFiles(from);
            if (listfiles != null && listfiles.Length > 0)
            {
                if (listfiles != null && listfiles.Length > 0)
                {
                    for (int i = 0; i < listfiles.Length; i++)
                    {
                        string namefile = Path.GetFileName(listfiles[i]);
                        string pto = to + "/" + namefile;
                        coppyFileNormal(listfiles[i], pto);
                    }
                }
            }
            if (deep <= 100)
            {
                string[] listd = Directory.GetDirectories(from);
                if (listd != null && listd.Length > 0)
                {
                    for (int i = 0; i < listd.Length; i++)
                    {
                        string namedir = Path.GetFileName(listd[i]);
                        string dto = to + "/" + namedir;
                        coppyFolder(listd[i], dto, deep + 1);
                    }
                }
            }
        }
    }

    public static void coppyRes2Assets(string GameName)
    {
        string passet = Application.dataPath;
        string ppro = passet.Replace("/Assets", "");
        string pathRes = ppro + $"/Mem{GameName}/Res";
        if (!Directory.Exists(pathRes))
        {
            return;
        }
        string[] listfiles = Directory.GetFiles(pathRes);
        if (listfiles != null)
        {
            for (int i = 0; i < listfiles.Length; i++)
            {
                string namefile = Path.GetFileName(listfiles[i]);
                string pto = passet + "/" + namefile;
                coppyFileNormal(listfiles[i], pto);
            }
        }
        string[] listd = Directory.GetDirectories(pathRes);
        if (listd != null && listd.Length > 0)
        {
            for (int i = 0; i < listd.Length; i++)
            {
                string namedir = Path.GetFileName(listd[i]);
                if (namedir.CompareTo("Android") != 0 && namedir.CompareTo("iOS") != 0)
                {
                    string dto = passet + "/" + namedir;
                    coppyFolder(listd[i], dto, 1);
                }
            }
        }
        string cplatform;
#if UNITY_ANDROID
        cplatform = pathRes + "/Android";
#else
        cplatform = pathRes + "/iOS";
#endif
        if (Directory.Exists(cplatform))
        {
            string[] listfilespl = Directory.GetFiles(cplatform);
            if (listfilespl != null)
            {
                for (int i = 0; i < listfilespl.Length; i++)
                {
                    string namefile = Path.GetFileName(listfilespl[i]);
                    string pto = passet + "/" + namefile;
                    coppyFileNormal(listfilespl[i], pto);
                }
            }
            string[] listdpl = Directory.GetDirectories(cplatform);
            if (listdpl != null && listdpl.Length > 0)
            {
                for (int i = 0; i < listdpl.Length; i++)
                {
                    string namedir = Path.GetFileName(listdpl[i]);
                    string dto = passet + "/" + namedir;
                    coppyFolder(listdpl[i], dto, 1);
                }
            }
        }
    }

    public static void backupGamePlugin(string GameName)
    {
        string passet = Application.dataPath;
        string ppro = passet.Replace("/Assets", "");
        string pathRes = ppro + $"/Mem{GameName}/Res";
        coppyFolder(passet + "/GamePlugin/Resources/Inapp", pathRes + "/GamePlugin/Resources/Inapp", 1);
        coppyFolder(passet + "/GamePlugin/Resources/Langs", pathRes + "/GamePlugin/Resources/Langs", 1);

        if (File.Exists(passet + "/GamePlugin/AdCanvasHelper/AdCanvasHelper.prefab"))
        {
            if (!Directory.Exists(pathRes + "/GamePlugin/AdCanvasHelper"))
            {
                Directory.CreateDirectory(pathRes + "/GamePlugin/AdCanvasHelper");
            }
            coppyFileNormal(passet + "/GamePlugin/AdCanvasHelper/AdCanvasHelper.prefab", pathRes + "/GamePlugin/AdCanvasHelper/AdCanvasHelper.prefab");
        }
        if (File.Exists(passet + "/GamePlugin/AdCanvasHelper/Res/ttDefault.jpg"))
        {
            if (!Directory.Exists(pathRes + "/GamePlugin/AdCanvasHelper/Res"))
            {
                Directory.CreateDirectory(pathRes + "/GamePlugin/AdCanvasHelper/Res");
            }
            coppyFileNormal(passet + "/GamePlugin/AdCanvasHelper/Res/ttDefault.jpg", pathRes + "/GamePlugin/AdCanvasHelper/Res/ttDefault.jpg");
        }
    }
    public static void coppyFile(string from, string to, string pkg, bool isTest)
    {
        try
        {
            string fname = Path.GetFileName(from);
            byte[] alldata = File.ReadAllBytes(from);
            string pto = to + "/" + fname;
            File.WriteAllBytes(pto, alldata);
            if (isTest)
            {
                string[] lines = File.ReadAllLines(pto);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("\"package_name\":"))
                    {
                        lines[i] = $"          \"package_name\": \"{pkg}\"";
                        break;
                    }
                }
                File.WriteAllLines(pto, lines);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: coppyFile ex=" + ex.ToString());
        }
    }
    public static void coppyFileWithNewName(string from, string to, string pkg, bool isTest)
    {
        try
        {
            byte[] alldata = File.ReadAllBytes(from);
            File.WriteAllBytes(to, alldata);
            if (isTest)
            {
                string[] lines = File.ReadAllLines(to);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("\"package_name\":"))
                    {
                        lines[i] = $"          \"package_name\": \"{pkg}\"";
                    }
                    else if (lines[i].Contains("\"bundle_id\":"))
                    {
                        lines[i] = $"                \"bundle_id\": \"{pkg}\"";
                        break;
                    }
                }
                File.WriteAllLines(to, lines);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: coppyFileWithNewName ex=" + ex.ToString());
        }
    }
}

public class SdkSetup
{
    public static void SetupSdk(mygame.sdk.SDKManager sdkManager, mygame.sdk.AdsHelper adsHelper, bool istest)
    {
        if (SettingBuildAndroid.defaultOrientation.Contains("0") || SettingBuildAndroid.defaultOrientation.Contains("1") || SettingBuildAndroid.defaultOrientation.Contains("4"))
        {
            sdkManager.OpenAdOrientation = mygame.sdk.App_Open_ad_Orien.Orien_Portraid;
        }
        else
        {
            sdkManager.OpenAdOrientation = mygame.sdk.App_Open_ad_Orien.Orien_Landscape;
        }
        EditorUtility.SetDirty(sdkManager);
        setAdCanvasSandbox(sdkManager.isAdCanvasSandbox);
#if UNITY_ANDROID
        doSettingAppOpenAdAndroid(sdkManager, adsHelper);
        doAddGradle(sdkManager.typeApplication, sdkManager.isAddandroidgms, adsHelper);
#endif
#if UNITY_IOS || UNITY_IPHONE
        doSettingiOS(sdkManager, adsHelper);
#endif
    }

    private static void setAdCanvasSandbox(bool isSanbox)
    {
        string passet = Application.dataPath + "/Gadsme/Resources/GadsmePreferences.asset";
        if (File.Exists(passet))
        {
            bool isw = false;
            var appCf = File.ReadAllLines(passet);
            for (int i = 0; i < appCf.Length; i++)
            {
                if (appCf[i].Contains("forceSandbox:"))
                {
                    if (isSanbox)
                    {
                        if (!appCf[i].Contains("1"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("0", "1");
                        }
                    }
                    else
                    {
                        if (!appCf[i].Contains("0"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("1", "0");
                        }
                    }
                }
                else if (appCf[i].Contains("LogLevel:"))
                {
                    if (isSanbox)
                    {
                        if (!appCf[i].Contains("3"))
                        {
                            isw = true;
                            appCf[i] = "  LogLevel: 3";
                        }
                    }
                    else
                    {
                        if (!appCf[i].Contains("1"))
                        {
                            isw = true;
                            appCf[i] = "  LogLevel: 1";
                        }
                    }
                }
                else if (appCf[i].Contains("showLoadingDistance:"))
                {
                    if (isSanbox)
                    {
                        if (!appCf[i].Contains("1"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("0", "1");
                        }
                    }
                    else
                    {
                        if (!appCf[i].Contains("0"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("1", "0");
                        }
                    }
                }
                else if (appCf[i].Contains("showVisibilityRays:"))
                {
                    if (isSanbox)
                    {
                        if (!appCf[i].Contains("1"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("0", "1");
                        }
                    }
                    else
                    {
                        if (!appCf[i].Contains("0"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("1", "0");
                        }
                    }
                }
                else if (appCf[i].Contains("debugRenderingEngine:"))
                {
                    if (isSanbox)
                    {
                        if (!appCf[i].Contains("1"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("0", "1");
                        }
                    }
                    else
                    {
                        if (!appCf[i].Contains("0"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("1", "0");
                        }
                    }
                }
            }
            if (isw)
            {
                File.WriteAllLines(passet, appCf);
            }
        }

        passet = Application.dataPath + "/Adverty/Resources/Data/AdvertySettings.asset";
        if (File.Exists(passet))
        {
            bool isw = false;
            var appCf = File.ReadAllLines(passet);
            for (int i = 0; i < appCf.Length; i++)
            {
                if (appCf[i].Contains("enableLogs:"))
                {
                    if (isSanbox)
                    {
                        if (!appCf[i].Contains("1"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("0", "1");
                        }
                    }
                    else
                    {
                        if (!appCf[i].Contains("0"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("1", "0");
                        }
                    }
                }
                else if (appCf[i].Contains("sandboxMode:"))
                {
                    if (isSanbox)
                    {
                        if (!appCf[i].Contains("1"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("0", "1");
                        }
                    }
                    else
                    {
                        if (!appCf[i].Contains("0"))
                        {
                            isw = true;
                            appCf[i] = appCf[i].Replace("1", "0");
                        }
                    }
                }
            }
            if (isw)
            {
                File.WriteAllLines(passet, appCf);
            }
        }
    }
    private static void doSettingAppOpenAdAndroid(mygame.sdk.SDKManager sdkManager, mygame.sdk.AdsHelper adsHelper, bool istest = false)
    {
#if UNITY_EDITOR
        try
        {
            string levelDirectoryPath;
            // if (Application.platform == RuntimePlatform.OSXEditor)
            {
                levelDirectoryPath = Application.dataPath + "/Plugins/Android/MyGameRes.androidlib/Assets/";
            }
            // else
            // {
            //     levelDirectoryPath = Application.dataPath + "\\Plugins\\Android\\Assets\\";
            // }
            Directory.CreateDirectory(levelDirectoryPath);
            string path = levelDirectoryPath + "appopenad.dat";

            char[] charid = adsHelper.OpenAdIdAndroid.Trim().ToCharArray();
            byte[] bytesadid = new byte[5 + charid.Length];

            bytesadid[0] = (byte)sdkManager.OpenAdOrientation;
            bytesadid[1] = 2;
            bytesadid[2] = 3;
            bytesadid[3] = 0;
            bytesadid[4] = 30;

            for (int i = 0; i < charid.Length; i++)
            {
                bytesadid[5 + i] = (byte)charid[i];
            }
            System.IO.File.WriteAllBytes(path, bytesadid);

            // if (Application.platform == RuntimePlatform.OSXEditor)
            {
                levelDirectoryPath = Application.dataPath + "/Plugins/Android/";
            }
            // else
            // {
            //     levelDirectoryPath = Application.dataPath + "\\Plugins\\Android\\";
            // }

            if (!adsHelper.AdmobAppID4Android.StartsWith("ca-app-pub-"))
            {
                Debug.LogError("mysdk: -------Error Android Adnob AppId");
            }
            path = levelDirectoryPath + "AndroidManifest.xml";
            string[] lines = System.IO.File.ReadAllLines(path);
            int statusAddAdmobAppId = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("ca-app-pub-"))
                {
                    string admobappid = "\"" + adsHelper.AdmobAppID4Android.Trim();
                    if (lines[i].Contains(admobappid))
                    {
                        statusAddAdmobAppId = 1;
                    }
                    else
                    {
                        statusAddAdmobAppId = 2;
                    }
                    break;
                }
            }
            List<string> allline = new List<string>();
            int idxaddAD_ID = -1;
            int idxAdd4Amazon = -1;
            int has4amazon = 0;
            int flagExport = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Contains("</application>"))
                {
                    if (lines[i].Contains("<application"))
                    {
                        string lapp = addApplication(sdkManager.typeApplication, lines[i]);
                        allline.Add(lapp);
                    }
                    else if (lines[i].Contains("ca-app-pub-") && statusAddAdmobAppId == 2)
                    {
                        allline.Add($"    <meta-data android:name=\"com.google.android.gms.ads.APPLICATION_ID\" android:value=\"{adsHelper.AdmobAppID4Android.Trim()}\" />");
                    }
                    else
                    {
                        if (lines[i].Contains("uses-permission"))
                        {
                            if (lines[i].Contains("com.google.android.gms.ads.permission.AD_ID"))
                            {
                                idxaddAD_ID = -2;
                                idxAdd4Amazon = allline.Count + 1;
                            }
                            else
                            {
                                if (idxaddAD_ID != -2)
                                {
                                    idxaddAD_ID = allline.Count;
                                }
                            }
                            if (lines[i].Contains("android.permission.ACCESS_FINE_LOCATION"))
                            {
                                has4amazon |= 2;
                            }
                            if (lines[i].Contains("android.permission.ACCESS_COARSE_LOCATION"))
                            {
                                has4amazon |= 1;
                            }
                        }
                        else if (lines[i].Contains("<activity")
                            || lines[i].Contains("<receiver")
                            || lines[i].Contains("<service")
                            )
                        {
                            if (lines[i].EndsWith(">"))
                            {
                                if (!lines[i].Contains("android:exported="))
                                {
                                    if (lines[i].EndsWith(" />"))
                                    {
                                        lines[i] = lines[i].Replace("/>", "android:exported=\"true\" />");
                                    }
                                    else if (lines[i].EndsWith("/>"))
                                    {
                                        lines[i] = lines[i].Replace("/>", " android:exported=\"true\" />");
                                    }
                                    else
                                    {
                                        lines[i] = lines[i].Replace(">", " android:exported=\"true\">");
                                    }
                                }
                            }
                            else
                            {
                                flagExport = 1;
                            }
                        }
                        if (flagExport == 1)
                        {
                            if (lines[i].Contains("android:exported="))
                            {
                                flagExport = 0;
                            }
                            if (lines[i].EndsWith(">") && flagExport == 1)
                            {
                                if (lines[i].EndsWith(" />"))
                                {
                                    lines[i] = lines[i].Replace("/>", "android:exported=\"true\" />");
                                }
                                else if (lines[i].EndsWith("/>"))
                                {
                                    lines[i] = lines[i].Replace("/>", " android:exported=\"true\" />");
                                }
                                else
                                {
                                    lines[i] = lines[i].Replace(">", " android:exported=\"true\">");
                                }
                                flagExport = 0;
                            }
                        }
                        allline.Add(lines[i]);
                    }
                }
                else
                {
                    if (statusAddAdmobAppId == 0)
                    {
                        allline.Add($"    <meta-data android:name=\"com.google.android.gms.ads.APPLICATION_ID\" android:value=\"{adsHelper.AdmobAppID4Android.Trim()}\" />");
                    }
                    allline.Add(lines[i]);
                    if (idxaddAD_ID == -1)
                    {
                        idxaddAD_ID = allline.Count;
                        idxAdd4Amazon = idxaddAD_ID;
                    }
                }
            }

            if (idxaddAD_ID >= 0)
            {
                allline.Insert(idxaddAD_ID, $"    <uses-permission android:name=\"com.google.android.gms.ads.permission.AD_ID\" />");
                idxAdd4Amazon = idxaddAD_ID + 1;
            }
#if ENABLE_ADS_AMAZON
            if (has4amazon == 3)
            {
                idxAdd4Amazon = -1;
            }
            if (idxAdd4Amazon >= 0)
            {
                if ((has4amazon & 2) == 0)
                {
                    allline.Insert(idxAdd4Amazon, $"  <uses-permission android:name=\"android.permission.ACCESS_FINE_LOCATION\" />");
                }
                if ((has4amazon & 1) == 0)
                {
                    allline.Insert(idxAdd4Amazon, $"  <uses-permission android:name=\"android.permission.ACCESS_COARSE_LOCATION\" />");
                }
            }
#endif

            System.IO.File.WriteAllLines(path, allline);
            allline.Clear();

            addOrRemvoeAdFly(sdkManager.isAddAdFly, sdkManager.AdFlyAdapterVersion);
        }
        catch (Exception ex)
        {
#if UNITY_ANDROID
            Debug.LogError("mysdk: doSettingAppOpenAdAndroid ex=" + ex.ToString());
#endif

        }
#endif
    }

    private static void addOrRemvoeAdFly(bool isAdd, string ver)
    {
        try
        {
            string pathdir = Application.dataPath + "/MaxSdk/Mediation/AdFly/Editor";
            if (isAdd)
            {
                string pathmax = Application.dataPath + "/MaxSdk/AppLovin.meta";
                if (File.Exists(pathmax))
                {
                    Directory.CreateDirectory(pathdir);
                    string pathfile = pathdir + "/Dependencies.xml";
                    List<string> allline = new List<string>();
                    allline.Add("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    allline.Add("<dependencies>");
                    allline.Add("    <androidPackages>");
                    allline.Add($"        <androidPackage spec=\"pub.adfly:adapter-max:{ver}\">");
                    allline.Add("            <repositories>");
                    allline.Add("                <repository>https://repo1.maven.org/maven2/</repository>");
                    allline.Add("            </repositories>");
                    allline.Add("        </androidPackage>");
                    allline.Add("    </androidPackages>");
                    allline.Add("</dependencies>");

                    System.IO.File.WriteAllLines(pathfile, allline);
                }
            }
            else
            {
                string pathfile = pathdir + "/Dependencies.xml";
                if (File.Exists(pathfile))
                {
                    File.Delete(pathfile);
                    pathfile = pathdir + "/Dependencies.xml.meta";
                    File.Delete(pathfile);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: addOrRemvoeAdFly ex=" + ex.ToString());
        }
    }

    //flag = 0-single, 1-mutil, 2-extraMutil
    private static string addApplication(mygame.sdk.AppType flag, string line)
    {
        string re = "";

        string nameapp = "";
        bool isExral = false;
        if (flag == mygame.sdk.AppType.Application)
        {
            nameapp = "\"mygame.plugin.app.PluginApp\"";
        }
        else if (flag == mygame.sdk.AppType.MultiApplication)
        {
            nameapp = "\"mygame.plugin.app.PluginAppMulti\"";
        }
        else if (flag == mygame.sdk.AppType.MultiApplicationStrictMode)
        {
            nameapp = "\"mygame.plugin.app.PluginStrictModeAppMulti\"";
        }
        else
        {
            isExral = true;
            nameapp = "\"mygame.plugin.app.PluginAppExtraMulti\"";
        }
        int idx = line.IndexOf("android:name=");
        if (idx < 0)
        {
            string appre = "<application android:name=" + nameapp;
            re = line.Replace("<application", appre);
        }
        else
        {
            int idx1 = line.IndexOf("\"", (idx + 16));
            string sreed = line.Substring(idx, (idx1 - idx + 1));
            string appre = "android:name=" + nameapp;
            re = line.Replace(sreed, appre);
        }
        string levelDirectoryPath;
        // if (Application.platform == RuntimePlatform.OSXEditor)
        {
            levelDirectoryPath = Application.dataPath + "/GamePlugin/Plugins/Android/";
        }
        // else
        // {
        //     levelDirectoryPath = Application.dataPath + "\\GamePlugin/Plugins/Android\\";
        // }
        string pathto = levelDirectoryPath + "myutilsmall.aar";
        if (isExral)
        {
            string passet = Application.dataPath;
            string ppr = passet.Replace("/Assets", "");
            string pathFrom = ppr + "/Mem/Android/For Build/MyApp/myutilsmall.aar";
            File.Copy(pathFrom, pathto, true);
        }
        else
        {
            File.Delete(pathto);
            File.Delete(pathto + ".meta");
        }
        return re;
    }

    static void doSettingiOS(mygame.sdk.SDKManager sdkManager, mygame.sdk.AdsHelper adsHelper)
    {
#if UNITY_EDITOR
        try
        {
            string levelDirectoryPath;
            // if (Application.platform == RuntimePlatform.OSXEditor)
            {
                levelDirectoryPath = Application.dataPath + "/GamePlugin/Plugins/iOS/";
            }
            // else
            // {
            //     levelDirectoryPath = Application.dataPath + "\\GamePlugin\\Plugins\\iOS\\";
            // }
            string admonappidori = adsHelper.AdmobAppID4iOS;
            string path = levelDirectoryPath + "MyGameAppController.mm";
            string[] lines = System.IO.File.ReadAllLines(path);
            List<string> allline = new List<string>();
            int statusAdd = 0;
            int orien = (int)sdkManager.OpenAdOrientation;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("-(void)setDefaultOpenAd") && statusAdd == 0)
                {
                    statusAdd = 1;
                    allline.Add(lines[i]);
                    allline.Add("    NSString* adid = [[NSUserDefaults standardUserDefaults] stringForKey:@\"openad_id\"];");
                    allline.Add("    if (adid == nil || adid.length < 3) {");
                    allline.Add("        [[NSUserDefaults standardUserDefaults] setInteger:" + orien + " forKey:@\"openad_orentation\"];");
                    allline.Add("        [[NSUserDefaults standardUserDefaults] setInteger:4 forKey:@\"openad_type_show\"];");
                    allline.Add("        [[NSUserDefaults standardUserDefaults] setInteger:3 forKey:@\"openad_show_at\"];");
                    allline.Add("        [[NSUserDefaults standardUserDefaults] setInteger:0 forKey:@\"openad_is_showfirst\"];");
                    allline.Add("        [[NSUserDefaults standardUserDefaults] setInteger:30 forKey:@\"openad_deltime\"];");
                    allline.Add("        [[NSUserDefaults standardUserDefaults] setObject:@\"" + adsHelper.OpenAdIdiOS.Trim() + "\" forKey:@\"openad_id\"];");
                    allline.Add("        [MyGameAppController mylog:@\"setDefaultOpenAd\"];");
                }
                else
                {
                    if (statusAdd == 0)
                    {
                        allline.Add(lines[i]);
                    }
                    else
                    {
                        if (statusAdd == 1)
                        {
                            if (lines[i].Contains("}"))
                            {
                                statusAdd = 0;
                                allline.Add(lines[i]);
                            }
                        }
                    }
                }
            }
            System.IO.File.WriteAllLines(path, allline);
            allline.Clear();

            //path = levelDirectoryPath + "MyGameAppController.h";
            //lines = System.IO.File.ReadAllLines(path);
            //for (int i = 0; i < lines.Length; i++)
            //{
            //    if (lines[i].Contains("#import <GoogleMobileAds/GoogleMobileAds.h>"))
            //    {
            //        allline.Add(lines[i]);
            //        int nl = i + 1;
            //        if (SettingBuildAndroid.isAdcanvas)
            //        {
            //            if (!lines[nl].Contains("#import \"VXWebViewAppController.h\""))
            //            {
            //                allline.Add("#import \"VXWebViewAppController.h\"");
            //            }
            //        }
            //        else
            //        {
            //            if (lines[nl].Contains("#import \"VXWebViewAppController.h\""))
            //            {
            //                i++;
            //            }
            //        }
            //    }
            //    else if (lines[i].Contains("@interface MyGameAppController"))
            //    {
            //        if (SettingBuildAndroid.isAdcanvas)
            //        {
            //            allline.Add("@interface MyGameAppController : VXWebViewAppController <GADFullScreenContentDelegate>");
            //        }
            //        else
            //        {
            //            allline.Add("@interface MyGameAppController : UnityAppController <GADFullScreenContentDelegate>");
            //        }
            //    }
            //    else
            //    {
            //        allline.Add(lines[i]);
            //    }
            //}
            //System.IO.File.WriteAllLines(path, allline);
            //allline.Clear();

            // if (Application.platform == RuntimePlatform.OSXEditor)
            {
                levelDirectoryPath = Application.dataPath + "/GamePlugin/Editor/";
            }
            // else
            // {
            //     levelDirectoryPath = Application.dataPath + "\\GamePlugin\\Editor\\";
            // }
            path = levelDirectoryPath + "BuildPostProcessor.cs";
            string[] linesBuil = System.IO.File.ReadAllLines(path);
            allline.Clear();
            for (int i = 0; i < linesBuil.Length; i++)
            {
                if (linesBuil[i].Contains("rootDict.SetString(\"GADApplicationIdentifier\""))
                {
                    if (!admonappidori.StartsWith("ca-app-pub-"))
                    {
                        Debug.LogError("mysdk: -------Error iOS Adnob AppId");
                    }
                    allline.Add($"            rootDict.SetString(\"GADApplicationIdentifier\", \"{admonappidori.Trim()}\");");
                }
                else
                {
                    allline.Add(linesBuil[i]);
                }
            }
            System.IO.File.WriteAllLines(path, allline);
            allline.Clear();

            //coppy button
            string pathimg = Application.dataPath + "/GamePlugin/Plugins/iOS/button_close.png";
            string patdest = Application.dataPath + "/Plugins/iOS/button_close.png";
            try
            { // get file length
                var buffer = File.ReadAllBytes(pathimg);
                File.WriteAllBytes(patdest, buffer);
            }
            catch (Exception ex)
            {
                Debug.Log("mysdk: ex=" + ex.ToString());
#if UNITY_IOS || UNITY_IPHONE
                Debug.LogError("mysdk: doSettingAppOpenAdiOS ex=" + ex.ToString());
#endif
            }
            finally
            {

            }
        }
        catch (Exception ex)
        {
            Debug.Log("mysdk: ex=" + ex.ToString());
#if UNITY_IOS || UNITY_IPHONE
            Debug.LogError("mysdk: doSettingAppOpenAdiOS all ex=" + ex.ToString());
#endif
        }
#endif
    }

    private static void doAddGradle(mygame.sdk.AppType typeapp, bool isAddAndroidGms, mygame.sdk.AdsHelper groupControl)
    {
#if UNITY_EDITOR
        try
        {
            string levelDirectoryPath;
            // if (Application.platform == RuntimePlatform.OSXEditor)
            {
                levelDirectoryPath = Application.dataPath + "/Plugins/Android/";
            }
            // else
            // {
            //     levelDirectoryPath = Application.dataPath + "\\Plugins\\Android\\";
            // }
            string path = levelDirectoryPath + "mainTemplate.gradle";
            string[] lines = System.IO.File.ReadAllLines(path);
            int statusAddAds = 0;
            int statusAddIdentifier = 0;
            bool ishaslifecycle = false;
            int isCheck = 0;
            int statusAndroidsupport = 0;
            int statusAddPlaycore = 0;
            int statusAddNativeCmp = 0;
            int statusAddAppset4Iron = 0;
            int statusAddPoraCheck1 = 0;
            int statusAddPoraCheck2 = 0;
            if (!isAddAndroidGms)
            {
                statusAddAds = 1;
                statusAddIdentifier = 1;
            }
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("def unityProjectPath"))
                {
#if UNITY_ANDROID && CHECK_4INAPP
                    statusAddPoraCheck1 = 1;
                    statusAddPoraCheck2 = 1;
                    isCheck = 2;
#endif
                }
                else if (lines[i].Contains("dependencies {"))
                {
                    isCheck = 1;
                }
                if (isCheck == 2)
                {
                    if (lines[i].Contains("url \"https://jitpack.io\""))
                    {
                        statusAddPoraCheck1 = 0;
                        isCheck = 0;
                    }
                }
                else if (isCheck == 1)
                {
                    if (lines[i].Contains("    implementation 'com.android.support:"))
                    {
                        if (groupControl.iSCommentAndroidSupport)
                        {
                            statusAndroidsupport = 1;
                        }
                    }
                    if (lines[i].Contains("com.google.android.gms:play-services-ads:"))
                    {
                        if (lines[i].Contains(groupControl.AdsVersion.Trim()))
                        {
                            statusAddAds = 1;
                        }
                        else
                        {
                            statusAddAds = 2;
                        }
                    }
                    else if (lines[i].Contains("com.google.android.gms:play-services-ads-identifier:"))
                    {
                        if (lines[i].Contains(groupControl.AdsIdentifierVersion.Trim()))
                        {
                            statusAddIdentifier = 1;
                        }
                        else
                        {
                            statusAddIdentifier = 2;
                        }
                    }
                    else if (lines[i].Contains("androidx.lifecycle:lifecycle-extensions:"))
                    {
                        ishaslifecycle = true;
                    }
                    else if (lines[i].Contains("implementation 'com.google.android.play:core:"))
                    {
                        statusAddPlaycore = 1;
                    }
                    else if (lines[i].Contains("com.google.android.ump:user-messaging-platform"))
                    {
                        if (SettingBuildAndroid.isAdcanvas)
                        {
                            statusAddNativeCmp = 1;
                        }
                        else
                        {
                            statusAddNativeCmp = 0;
                        }
                    }
                    else if (lines[i].Contains("com.google.android.gms:play-services-appset"))
                    {
                        statusAddAppset4Iron = 1;
                    }
                    else if (lines[i].Contains("com.github.javiersantos:PiracyChecker"))
                    {
#if UNITY_ANDROID && CHECK_4INAPP
                        statusAddPoraCheck2 = 0;
#endif
                    }
                    if (lines[i].StartsWith("**DEPS**}") || lines[i].StartsWith("}") || lines[i].Contains("// Android Resolver Dependencies End"))
                    {
                        isCheck = 10;
                        break;
                    }
                }
            }

            if ((statusAddPoraCheck1 == 1 || statusAddPoraCheck2 == 1)
                || statusAddAds != 1
                || statusAddIdentifier != 1
                || !ishaslifecycle
                || statusAndroidsupport != 0
                || statusAddPlaycore == 0
                || (statusAddNativeCmp == 0 && SettingBuildAndroid.isAdcanvas || statusAddNativeCmp == 1 && !SettingBuildAndroid.isAdcanvas)
                || (statusAddAppset4Iron == 0 && SettingBuildAndroid.isIronEnable)
                )
            {
                int isAdd = 0;
                List<string> allline = new List<string>();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("def unityProjectPath"))
                    {
                        if (statusAddPoraCheck1 == 1)
                        {
                            isAdd = 10;
                        }
                    }
                    else if (lines[i].Contains("dependencies {"))
                    {
                        isAdd = 1;
                    }

                    if (isAdd == 10)
                    {
                        allline.Add(lines[i]);
                        allline.Add("        maven {");
                        allline.Add("            url \"https://jitpack.io\"");
                        allline.Add("        }");
                        isAdd = 0;
                    }
                    else if (isAdd == 1)
                    {
                        if (lines[i].Contains("    implementation 'com.android.support:"))
                        {
                            if (statusAndroidsupport == 1 && !lines[i].StartsWith("//"))
                            {
                                allline.Add($"//{lines[i]}");
                            }
                            else if (lines[i].StartsWith("//"))
                            {
                                string ssss = lines[i].Substring(2);
                                allline.Add(ssss);
                            }
                            else
                            {
                                allline.Add(lines[i]);
                            }
                        }
                        else if (lines[i].StartsWith("**DEPS**}") || lines[i].StartsWith("}") || lines[i].Contains("// Android Resolver Dependencies End"))
                        {
                            isAdd = 2;

                            if (statusAddPlaycore == 0)
                            {
                                allline.Add(" ");
                                allline.Add($"    implementation 'com.google.android.play:core:1.9.0'");
                            }

                            if (statusAddNativeCmp == 0 && SettingBuildAndroid.isAdcanvas)
                            {
                                allline.Add($"    implementation 'com.google.android.ump:user-messaging-platform:2.0.0'");
                            }

                            if (statusAddAds == 0)
                            {
                                allline.Add($"    implementation 'com.google.android.gms:play-services-ads:{groupControl.AdsVersion.Trim()}'");
                            }
                            if (statusAddAppset4Iron == 0 && SettingBuildAndroid.isIronEnable)
                            {
                                allline.Add($"    implementation 'com.google.android.gms:play-services-appset:16.0.2'");
                            }
                            if (statusAddIdentifier == 0)
                            {
                                allline.Add($"    implementation 'com.google.android.gms:play-services-ads-identifier:{groupControl.AdsIdentifierVersion.Trim()}'");
                            }
                            if (statusAddPoraCheck2 == 1)
                            {
                                allline.Add("    implementation 'com.github.javiersantos:PiracyChecker:1.2.8'");
                            }
                            if (!ishaslifecycle)
                            {
                                allline.Add("  ");
                                allline.Add("    def lifecycle_version = \"2.2.0\"");
                                allline.Add("    implementation \"androidx.lifecycle:lifecycle-extensions:$lifecycle_version\"");
                                allline.Add("    implementation \"androidx.lifecycle:lifecycle-runtime:$lifecycle_version\"");
                                allline.Add("    annotationProcessor \"androidx.lifecycle:lifecycle-compiler:$lifecycle_version\"");
                                allline.Add("  ");
                            }
                            allline.Add(lines[i]);
                        }
                        else
                        {
                            if (lines[i].Contains("com.google.android.gms:play-services-ads:"))
                            {
                                if (statusAddAds == 2 && compareVersion(groupControl.AdsVersion.Trim(), lines[i]) > 0)
                                {
                                    allline.Add($"    implementation 'com.google.android.gms:play-services-ads:{groupControl.AdsVersion.Trim()}'");
                                }
                                else
                                {
                                    allline.Add(lines[i]);
                                }
                            }
                            else if (lines[i].Contains("com.google.android.gms:play-services-ads-identifier:"))
                            {
                                if (statusAddIdentifier == 2 && compareVersion(groupControl.AdsIdentifierVersion.Trim(), lines[i]) > 0)
                                {
                                    allline.Add($"    implementation 'com.google.android.gms:play-services-ads-identifier:{groupControl.AdsIdentifierVersion.Trim()}'");
                                }
                                else
                                {
                                    allline.Add(lines[i]);
                                }
                            }
                            else if (lines[i].Contains("com.google.android.ump:user-messaging-platform:"))
                            {
                                if (SettingBuildAndroid.isAdcanvas)
                                {
                                    allline.Add(lines[i]);
                                }
                            }
                            else
                            {
                                allline.Add(lines[i]);
                            }
                        }
                    }
                    else
                    {
                        allline.Add(lines[i]);
                    }
                }

                System.IO.File.WriteAllLines(path, allline);
                allline.Clear();

                path = levelDirectoryPath + "baseProjectTemplate.gradle";
                string[] lines1 = System.IO.File.ReadAllLines(path);
                int statusversiongradle = 0;
                for (int i = 0; i < lines1.Length; i++)
                {
                    if (lines1[i].Contains("classpath 'com.android.tools.build:gradle:"))
                    {
                        if (groupControl.gradleVersion.Length > 3)
                        {
                            statusversiongradle = 1;
                        }
                    }
                }
                if (statusversiongradle != 0)
                {
                    for (int i = 0; i < lines1.Length; i++)
                    {
                        if (lines1[i].Contains("classpath 'com.android.tools.build:gradle:"))
                        {
                            allline.Add($"            classpath 'com.android.tools.build:gradle:{groupControl.gradleVersion}'");
                        }
                        else
                        {
                            allline.Add(lines1[i]);
                        }
                    }
                    System.IO.File.WriteAllLines(path, allline);
                    allline.Clear();
                }

                allline.Clear();
                path = levelDirectoryPath + "launcherTemplate.gradle";
                string[] lines2 = System.IO.File.ReadAllLines(path);
                int statusmultidex = 0;
                int statussplit = 0;
                int statusFlagMulti = 0;
                for (int i = 0; i < lines2.Length; i++)
                {
                    if (lines2[i].Contains("androidx.multidex:multidex:"))
                    {
                        statusmultidex = 1;
                    }
                    else if (lines2[i].Contains("multiDexEnabled true"))
                    {
                        statusFlagMulti = 1;
                    }
                    else if (lines2[i].Contains("import com.android.build.OutputFile"))
                    {
                        statussplit = 1;
                    }
                }
                if (statussplit == 0 || ((statusmultidex == 0 || statusFlagMulti == 0) && typeapp != mygame.sdk.AppType.Application) || ((statusmultidex == 1 || statusFlagMulti == 1) && typeapp == mygame.sdk.AppType.Application))
                {
                    for (int i = 0; i < lines2.Length; i++)
                    {
                        if (lines2[i].Contains("dependencies"))
                        {
                            allline.Add(lines2[i]);
                            if (statusmultidex == 0 && typeapp != mygame.sdk.AppType.Application)
                            {
                                allline.Add("   implementation 'androidx.multidex:multidex:2.0.1'");
                            }
                        }
                        else if (lines2[i].Contains("androidx.multidex:multidex:"))
                        {
                            if (statusmultidex == 0 || typeapp != mygame.sdk.AppType.Application)
                            {
                                allline.Add(lines2[i]);
                            }
                        }
                        else if (lines2[i].Contains("versionName"))
                        {
                            allline.Add(lines2[i]);
                            if (statusFlagMulti == 0 && typeapp != mygame.sdk.AppType.Application)
                            {
                                allline.Add("       multiDexEnabled true");
                            }
                        }
                        else if (lines2[i].Contains("multiDexEnabled true"))
                        {
                            if (statusFlagMulti == 0 || typeapp != mygame.sdk.AppType.Application)
                            {
                                allline.Add(lines2[i]);
                            }
                        }
                        else if (lines2[i].Contains("}**SPLITS_VERSION_CODE****"))
                        {
                            allline.Add(lines2[i]);
                            if (statussplit == 0)
                            {
                                allline.Add("ext.abiCodes = ['armeabi-v7a':0, 'arm64-v8a':1]");
                                allline.Add("");
                                allline.Add("import com.android.build.OutputFile");
                                allline.Add("");
                                allline.Add("android.applicationVariants.all { variant ->");
                                allline.Add("  variant.outputs.each { output ->");
                                allline.Add("    def baseAbiVersionCode =");
                                allline.Add("            // Determines the ABI for this variant and returns the mapped value.");
                                allline.Add("            project.ext.abiCodes.get(output.getFilter(OutputFile.ABI))");
                                allline.Add("    if (baseAbiVersionCode != null) {");
                                allline.Add("      output.versionCodeOverride =");
                                allline.Add("              baseAbiVersionCode + variant.versionCode");
                                allline.Add("    }");
                                allline.Add("  }");
                                allline.Add("}");
                            }
                        }
                        else
                        {
                            allline.Add(lines2[i]);
                        }
                    }
                    System.IO.File.WriteAllLines(path, allline);
                }
            }
        }
        catch (Exception ex)
        {
#if UNITY_ANDROID
            Debug.LogError("mysdk: doAddGradle ex=" + ex.ToString());
#endif
        }
#endif
    }

    private static int compareVersion(string vero1, string vero2)
    {
        int idx1 = vero1.IndexOf("//");
        int idx2 = vero2.IndexOf("//");
        string v1 = "";
        string v2 = "";
        if (idx1 > 0)
        {
            v1 = vero1.Substring(0, idx1);
        }
        else
        {
            v1 = vero1;
        }
        if (idx2 > 0)
        {
            v2 = vero2.Substring(0, idx2);
        }
        else
        {
            v2 = vero2;
        }
        idx1 = v1.LastIndexOf(":");
        idx2 = v2.LastIndexOf(":");
        if (idx1 > 0)
        {
            v1 = v1.Substring(idx1 + 1);
        }
        idx1 = v1.IndexOf('\'');
        if (idx1 > 0)
        {
            v1 = v1.Substring(0, idx1);
        }
        else
        {
            idx1 = v1.IndexOf('"');
            if (idx1 > 0)
            {
                v1 = v1.Substring(0, idx1);
            }
        }
        //
        if (idx2 > 0)
        {
            v2 = v2.Substring(idx2 + 1);
        }
        idx2 = v2.IndexOf('\'');
        if (idx2 > 0)
        {
            v2 = v2.Substring(0, idx2);
        }
        else
        {
            idx2 = v2.IndexOf('"');
            if (idx2 > 0)
            {
                v2 = v2.Substring(0, idx2);
            }
        }
        try
        {
            while (v1.Contains("'"))
            {
                v1 = v1.Replace("'", "");
            }
            while (v1.Contains("\""))
            {
                v1 = v1.Replace("\"", "");
            }
            while (v2.Contains("'"))
            {
                v2 = v2.Replace("'", "");
            }
            while (v2.Contains("\""))
            {
                v2 = v2.Replace("\"", "");
            }
            string[] arrv1 = v1.Split(new char[] { '.' });
            string[] arrv2 = v2.Split(new char[] { '.' });
            int[] numv1 = new int[arrv1.Length];
            int[] numv2 = new int[arrv2.Length];
            for (int i = 0; i < arrv1.Length; i++)
            {
                numv1[i] = int.Parse(arrv1[i]);
            }
            for (int i = 0; i < arrv2.Length; i++)
            {
                numv2[i] = int.Parse(arrv2[i]);
            }
            int n = numv1.Length;
            if (n > numv2.Length)
            {
                n = numv2.Length;
            }
            for (int i = 0; i < n; i++)
            {
                if (numv1[i] > numv2[i])
                {
                    return 1;
                }
                else if (numv1[i] < numv2[i])
                {
                    return -1;
                }
            }
            if (numv1.Length > numv2.Length)
            {
                return 1;
            }
            else if (numv1.Length < numv2.Length)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("mysdk: doAddGradle ex=" + ex.ToString());
            return 0;
        }
    }

}

public class GameOb4Addrf
{
    public List<string> listComponent = new List<string>();
}