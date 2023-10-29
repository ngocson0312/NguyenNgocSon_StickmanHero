#if UNITY_IOS

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace mygame.sdk
{
    public class AdsAdmobMyiOSBridge
    {
		[DllImport ("__Internal")] private static extern void InitializeNative();
		public static void Initialize()
		{
			InitializeNative();
		}
		[DllImport ("__Internal")] private static extern void setBannerPosNative(int pos, float dxCenter);
		public static void setBannerPos(int pos, float dxCenter)
		{
			setBannerPosNative(pos, dxCenter);
		}
		[DllImport ("__Internal")] private static extern void showBannerNative(string adsId, int pos, int orien, bool iPad, float dxCenter);
		public static void showBanner(string adsId, int pos, int orien, bool iPad, float dxCenter)
		{
			showBannerNative(adsId, pos, orien, iPad, dxCenter);
		}

		[DllImport ("__Internal")] private static extern void hideBannerNative();
		public static void hideBanner()
		{
			hideBannerNative();
		}

		[DllImport("__Internal")] private static extern void clearCurrFullNative();
		public static void clearCurrFull()
        {
			clearCurrFullNative();
		}

		[DllImport ("__Internal")] private static extern void loadFullNative(string adsId);
		public static void loadFull(string adsId)
		{
			loadFullNative(adsId);
		}

		[DllImport("__Internal")] private static extern bool showFullNative();
		public static bool showFull()
		{
			return showFullNative();
		}

		[DllImport("__Internal")] private static extern void clearCurrGiftNative();
		public static void clearCurrGift()
		{
			clearCurrGiftNative();
		}

		[DllImport("__Internal")] private static extern void loadGiftNative(string adsId);
		public static void loadGift(string adsId)
		{
			loadGiftNative(adsId);
		}

		[DllImport("__Internal")] private static extern bool showGiftNative();
		public static bool showGift()
		{
			return showGiftNative();
		}
    }
}

#endif
