using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public abstract class ScreenUI : MonoBehaviour
    {
        public ScreenName screenName;
        protected ScreenUIManager screenManager;
        public virtual void Initialize(ScreenUIManager manager)
        {
            screenManager = manager;
        }
        public abstract void Show();
        public abstract void Hide();
    }
    public enum ScreenName
    {
        MAINSCREEN, PLAYSCREEN, LOSESCREEN, WINSCREEN
    }
}

