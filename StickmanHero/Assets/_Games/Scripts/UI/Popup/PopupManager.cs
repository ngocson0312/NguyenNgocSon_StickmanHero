using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(PopupManager))]
    public class PopupManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("FindAllPopup"))
            {
                PopupManager p = (PopupManager)target;
                p.FindAllPopup();
            }
        }
    }
#endif
    public class PopupManager : Singleton<PopupManager>
    {
        public PopupUI[] popups;
        Queue<PopupUI> popupQueue;
        public void Initialize()
        {
            popupQueue = new Queue<PopupUI>();
            for (int i = 0; i < popups.Length; i++)
            {
                popups[i].Initialize(this);
            }
        }
        public void ClearQueue()
        {
            for (int i = 0; i < popups.Length; i++)
            {
                popups[i].Hide();
            }
            popupQueue.Clear();
        }
        public void ShowPopup(PopupName popupName)
        {
            for (int i = 0; i < popups.Length; i++)
            {
                if (popups[i].popupName == popupName)
                {
                    if (popupQueue.Count == 0)
                    {
                        popups[i].Show();
                    }
                    popupQueue.Enqueue(popups[i]);
                }
            }
        }
        public T ShowPopup<T>(PopupName popupName)
        {
            PopupUI popupUI = null;
            for (int i = 0; i < popups.Length; i++)
            {
                if (popups[i].popupName == popupName)
                {
                    if (popupQueue.Count == 0)
                    {
                        popups[i].Show();
                    }
                    popupQueue.Enqueue(popups[i]);
                    popupUI = popups[i];
                }
            }
            return popupUI.GetComponent<T>();
        }
        public void HideCurrentPopup()
        {
            if (popupQueue.Count > 0)
            {
                popupQueue.Peek().Hide();
            }
        }
        public void OnHidePopup()
        {
            if (popupQueue.Count > 0)
            {
                popupQueue.Dequeue();
            }
            if (popupQueue.Count > 0)
            {
                popupQueue.Peek().Show();
            }
        }
        public void FindAllPopup()
        {
            popups = FindObjectsOfType<PopupUI>(true);
        }
    }
    public enum PopupName
    {
        BUYSKIN, REWARDPOPUP, SHOPCOIN, DAILYREWARD, SETTING, SUBSCRIPTION
    }

}


