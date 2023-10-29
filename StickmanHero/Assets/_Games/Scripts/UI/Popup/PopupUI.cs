using mygame.sdk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public abstract class PopupUI : MonoBehaviour
    {
        public PopupName popupName;
        protected PopupManager popupManager;
        public virtual void Initialize(PopupManager popupManager)
        {
            this.popupManager = popupManager;
        }
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            AdsHelper.Instance.showBanner(AD_BANNER_POS.TOP, App_Open_ad_Orien.Orien_Landscape, 320);
            popupManager.OnHidePopup();
        }
    }
}

