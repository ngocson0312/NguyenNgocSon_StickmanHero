#if UNITY_IOS

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace mygame.sdk
{
    public class MyAdsOpeniOSBridge
    {
		[DllImport ("__Internal")] private static extern void loadNative(string path, bool isCache);
		public static void load(string path, bool isCache)
		{
			loadNative(path, isCache);
		}

		[DllImport("__Internal")] private static extern bool showNative(string path, int flagBtNo, int isFull);
		public static bool show(string path, int flagBtNo, int isFull)
		{
			return showNative(path, flagBtNo, isFull);
		}

		[DllImport("__Internal")] private static extern void loadAndShowNative(string path, bool isCache, int flagBtNo, int isFull);
		public static void loadAndShow(string path, bool isCache, int flagBtNo, int isFull)
		{
			loadAndShowNative(path, isCache, flagBtNo, isFull);
		}
    }
}

#endif
