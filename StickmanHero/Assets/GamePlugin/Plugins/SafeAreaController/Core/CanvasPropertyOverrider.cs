using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame.plugin.Android;

[RequireComponent(typeof(RectTransform))]
public class CanvasPropertyOverrider : MonoBehaviour
{
    public bool isSafeCanvas = true;

    private Rect offset
    {
        get { return new Rect(0, 0, 0, navigationBarHeight); }
    }

    private static int navigationBarHeight = 0;

    private void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // if (addSoftkeyArea)
            // RunOnAndroidUiThread(GetNavigationBarHeight);
#endif
    }

    private void Start()
    {
        UpdateCanvasProperty();
    }

    // Update Method
    public void UpdateCanvasProperty()
    {
        RectTransform myTransform = GetComponent<RectTransform>();
        Rect safeArea = Screen.safeArea;
        Vector2 screen = new Vector2(Screen.width, Screen.height);

        Vector2 _saAnchorMin;
        Vector2 _saAnchorMax;

        var offset_right = offset.x;
        var offset_left = offset.y;
        var offset_top = offset.width;
        var offset_bottom = offset.height;

        // 1. Setup and apply safe area
        if (isSafeCanvas)
        {
            // _saAnchorMin.x = (safeArea.x + offset_right) / screen.x;
            // _saAnchorMin.y = (safeArea.y + offset_bottom) / screen.y;
            // myTransform.anchorMin = _saAnchorMin;
            // GameHelperAndroid.getDisplayMetrics(out var w, out var h, out var dpi);
            var h = (screen.y - (screen.y - safeArea.yMax)) / screen.y;
            myTransform.anchorMax = new Vector2(1, h);
        }
       
    }

    private static void RunOnAndroidUiThread(Action target)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {

            activity.Call("runOnUiThread", new AndroidJavaRunnable(target));
        }}
#elif UNITY_EDITOR
        target();
#endif
    }

    private static void GetNavigationBarHeight()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
        using (var window = activity.Call<AndroidJavaObject>("getWindow")) {
        using (var resources = activity.Call<AndroidJavaObject>("getResources")) {
            var resourceId = resources.Call<int>("getIdentifier", "navigation_bar_height", "dimen", "android");
            if (resourceId > 0) {
                navigationBarHeight = resources.Call<int>("getDimensionPixelSize", resourceId);
            }
        }}}}
#elif UNITY_EDITOR
        navigationBarHeight = 0;
#endif
    }
}