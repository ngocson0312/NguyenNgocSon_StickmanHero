using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GamePlugins
{
    public class ScreenManager : MonoBehaviour
    {
        public Canvas canvas;
        public Transform parent;
        public BaseScreen currentScreen;

        public BaseScreen[] prefabs;

        public Stack<BaseScreen> screenStacks = new Stack<BaseScreen>();
        public List<BaseScreen> cacheScreen = new List<BaseScreen>();
        private List<BaseScreen> ExistScreen = new List<BaseScreen>();

        private int defaultSortingOrder;

        private static ScreenManager mInstance;

        public static ScreenManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = LoadResource<ScreenManager>("Screen/ScreenManager");
                }
                return mInstance;
            }
        }

        public static ScreenManager CheckInstance
        {
            get
            {
                return mInstance;
            }
        }

        private void Awake()
        {
            mInstance = this;
            defaultSortingOrder = canvas.sortingOrder;
            getExixtScreen();
            DontDestroyOnLoad(gameObject);
        }

        public static T CreateNewInstance<T>()
        {
            return Instance.CheckInstanceScreenPrebab<T>();
        }

        private void getExixtScreen()
        {
            ExistScreen.Clear();
            int idxstartEnable = -1;
            bool isacmorethanone = false;
            for (int i = 0; i < parent.childCount; i++)
            {
                BaseScreen bs = parent.GetChild(i).GetComponent<BaseScreen>();
                if (bs.gameObject.activeInHierarchy)
                {
                    if (idxstartEnable == -1)
                    {
                        idxstartEnable = i;
                    }
                    else
                    {
                        isacmorethanone = true;
                    }
                }
                if (bs != null)
                {
                    ExistScreen.Add(bs);
                }
            }
            if (isacmorethanone)
            {
                for (int i = (idxstartEnable + 1); i < parent.childCount; i++)
                {
                    parent.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        public T CheckExistScreen<T>() where T : BaseScreen
        {
            BaseScreen gameObject;
            for (int i = 0; i < ExistScreen.Count; i++)
            {
                if (IsOfType<T>(ExistScreen[i]))
                {
                    gameObject = ExistScreen[i];
                    ExistScreen.RemoveAt(i);
                    return gameObject as T;
                }
            }
            for (int i = 0; i < cacheScreen.Count; i++)
            {
                if (IsOfType<T>(cacheScreen[i]))
                {
                    gameObject = cacheScreen[i];
                    cacheScreen.RemoveAt(i);
                    return gameObject as T;
                }
            }
            return null;
        }

        public T CheckInstanceScreenPrebab<T>()
        {
            GameObject gameObject = null;
            for (int i = 0; i < prefabs.Length; i++)
            {
                if (IsOfType<T>(prefabs[i]))
                {
                    gameObject = Instantiate(prefabs[i].gameObject, parent);
                    break;
                }
            }
            return gameObject.GetComponent<T>();
        }

        public bool Contains<T>()
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                if (IsOfType<T>(prefabs[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsOfType<T>(object value)
        {
            return value is T;
        }

        public bool CheckStack(Type type)
        {
            return screenStacks.Count > 0 && screenStacks.Peek().GetType() == type;
        }

        public static T LoadResource<T>(string name)
        {
            GameObject gameObject = (GameObject)Instantiate(Resources.Load(name));
            gameObject.name = $"[{name}]";
            DontDestroyOnLoad(gameObject);
            return gameObject.GetComponent<T>();
        }

        public void showPopup()
        {
            if (currentScreen == null)
            {
                currentScreen.onDisableControl();
            }
        }

        public void hidePopup()
        {
            if (currentScreen == null)
            {
                currentScreen.onEnableControl();
            }
        }

        public void SetSortingOrder(int order)
        {
            canvas.sortingOrder = order;
        }

        public void ResetOrder()
        {
            canvas.sortingOrder = defaultSortingOrder;
        }
    }
}
