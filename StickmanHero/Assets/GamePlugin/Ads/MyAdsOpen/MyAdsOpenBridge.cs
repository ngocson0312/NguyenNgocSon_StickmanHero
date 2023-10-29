using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_ANDROID
using mygame.plugin.Android;
#endif

namespace mygame.sdk
{
    public class MyAdsOpenBridge : MonoBehaviour
    {
        public static event Action onLoaded;
        public static event Action<string> onLoadFail;
        public static event Action onOpen;
        public static event Action onClose;

        // private MyAdsOpenIF myAdsOpenIF;

        public static MyAdsOpenBridge Instance { get; private set; }

#if UNITY_ANDROID
#endif

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                gameObject.name = "MyAdsOpenBridge";
#if !UNITY_EDITOR

#if UNITY_ANDROID
                // myAdsOpenIF = new MyAdsOpenAndroid();
#else //UNITY_ANDROID
                // myAdsOpenIF = new MyAdsOpeniOS();
#endif //UNITY_ANDROID

#endif //UNITY_EDITOR
            }
            else
            {
                //if (this != Instance) Destroy(gameObject);
            }
        }


#region My ads open implementation
        public void load(string path, bool isCache)
        {
#if !UNITY_EDITOR
            if (path == null || path.Length < 5)
            {
                if (onLoadFail != null) onLoadFail("Path not correct");
            }
            else
            {
                // myAdsOpenIF.load(path, isCache);
            }
#endif
        }
        public bool show(string path, int flagBtNo, int isFull)
        {
// #if !UNITY_EDITOR
//             return myAdsOpenIF.show(path, flagBtNo, isFull);
// #else
            return false;
// #endif
        }
        public void loadAndShow(string path, bool isCache, int flagBtNo, int isFull)
        {
// #if !UNITY_EDITOR
//             myAdsOpenIF.loadAndShow(path, isCache, flagBtNo, isFull);
// #endif
        }
#endregion

#region Callbacks from UnityInterstitialAdListener.
        public void CBLoaded()
        {
            if (onLoaded != null) onLoaded();
        }
        public void CBLoadFail(string err)
        {
            if (onLoadFail != null) onLoadFail(err);
        }
        public void CBOpen()
        {
            if (onOpen != null) onOpen();
        }
        public void CBClose()
        {
            if (onClose != null) onClose();
        }

#endregion

#region Callbacks from ios.
        public void iOSCBLoaded(string description)
        {
            CBLoaded();
        }
        public void iOSCBLoadFail(string err)
        {
            CBLoadFail(err);
        }
        public void iOSCBOpen(string description)
        {
            CBOpen();
        }
        public void iOSCBClose(string description)
        {
            CBClose();
        }

#endregion
    }
}
