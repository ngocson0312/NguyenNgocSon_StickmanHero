#if UNITY_IOS

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace mygame.sdk
{
    public class MyAdsOpeniOS : MyAdsOpenIF
    {
#region IGoogleMobileAdsInterstitialClient implementation
        public void load(string path, bool isCache)
        {
            MyAdsOpeniOSBridge.load(path, isCache);
        }
        public bool show(string path, int flagBtNo, int isFull)
        {
            return MyAdsOpeniOSBridge.show(path, flagBtNo, isFull);
        }

        public void loadAndShow(string path, bool isCache, int flagBtNo, int isFull)
        {
            MyAdsOpeniOSBridge.loadAndShow(path, isCache, flagBtNo, isFull);
        }
#endregion
    }
}

#endif
