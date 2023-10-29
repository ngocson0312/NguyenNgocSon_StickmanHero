#if UNITY_IOS

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace mygame.sdk
{
    public class AdsAdmobMyLowiOSBridge
    {
		[DllImport ("__Internal")] private static extern void LowInitializeNative();
		public static void Initialize()
		{
			LowInitializeNative();
		}
		[DllImport ("__Internal")] private static extern void LowSetBannerPosNative(int pos, float dxCenter);
		public static void setBannerPos(int pos, float dxCenter)
		{
			LowSetBannerPosNative(pos, dxCenter);
		}
		[DllImport ("__Internal")] private static extern void LowShowBannerNative(string adsId, int pos, int orien, bool iPad, float dxCenter);
		public static void showBanner(string adsId, int pos, int orien, bool iPad, float dxCenter)
		{
			LowShowBannerNative(adsId, pos, orien, iPad, dxCenter);
		}

		[DllImport ("__Internal")] private static extern void LowHideBannerNative();
		public static void hideBanner()
		{
			LowHideBannerNative();
		}
		
		[DllImport ("__Internal")] private static extern void LowLoadFullNative(string adsId);
		public static void loadFull(string adsId)
		{
			LowLoadFullNative(adsId);
		}

		[DllImport("__Internal")] private static extern void LowClearCurrFullNative();
		public static void clearCurrFull()
		{
			LowClearCurrFullNative();
		}

		[DllImport("__Internal")] private static extern bool LowShowFullNative();
		public static bool showFull()
		{
			return LowShowFullNative();
		}

		[DllImport("__Internal")] private static extern void LowClearCurrGiftNative();
		public static void clearCurrGift()
		{
			LowClearCurrGiftNative();
		}

		[DllImport("__Internal")] private static extern void LowLoadGiftNative(string adsId);
		public static void loadGift(string adsId)
		{
			LowLoadGiftNative(adsId);
		}

		[DllImport("__Internal")] private static extern bool LowShowGiftNative();
		public static bool showGift()
		{
			return LowShowGiftNative();
		}
    }
}

#endif
