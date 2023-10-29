using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlugins;
using UnityEngine.UI;

namespace SuperFight
{
    public class KeepPlayingPopup : MonoBehaviour
    {
        public Button ButtonOK;
        void Awake()
        {
            ButtonOK.onClick.AddListener(() => ButtonClose());
        }
        void ButtonClose()
        {
            gameObject.SetActive(false);
        }
    }

}
