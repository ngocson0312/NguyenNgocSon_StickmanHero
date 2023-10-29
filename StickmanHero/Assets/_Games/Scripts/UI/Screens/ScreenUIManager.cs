using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class ScreenUIManager : Singleton<ScreenUIManager>
    {
        public ScreenUI[] screens;
        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < screens.Length; i++)
            {
                screens[i].Initialize(this);
            }
        }
        public T ShowScreen<T>(ScreenName screenName)
        {
            ScreenUI screen = null;
            for (int i = 0; i < screens.Length; i++)
            {
                screens[i].Hide();
                if (screens[i].screenName == screenName)
                {
                    screen = screens[i];
                }
            }
            screen.Show();
            return screen.GetComponent<T>();
        }
        public void ShowScreen(ScreenName screenName)
        {
            ScreenUI screen = null;
            for (int i = 0; i < screens.Length; i++)
            {
                screens[i].Hide();
                if (screens[i].screenName == screenName)
                {
                    screen = screens[i];
                }
            }
            screen.Show();
        }
        public T GetScreen<T>(ScreenName screenName)
        {
            ScreenUI screen = null;
            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i].screenName == screenName)
                {
                    screen = screens[i];
                }
            }
            return screen.GetComponent<T>();
        }
    }
}

