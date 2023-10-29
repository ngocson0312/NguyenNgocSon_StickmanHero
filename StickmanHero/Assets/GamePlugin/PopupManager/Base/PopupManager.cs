using Spine.Unity;

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GamePlugins
{
    public class PopupManager : MonoBehaviour
    {
        public Camera camera;

        public Canvas canvas;
        public Transform parent;
        public GameObject transparent;
        public bool isCache = false;

        public bool usingDefaultTransparent = true;

        public BasePopup[] prefabs;
        private Transform mTransparentTrans;
        public Stack<BasePopup> popupStacks = new Stack<BasePopup>();
        public List<BasePopup> cachePopup = new List<BasePopup>();
        private List<BasePopup> ExistPopup = new List<BasePopup>();

        private int defaultSortingOrder;

        private static PopupManager mInstance;

        public static PopupManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = LoadResource<PopupManager>("Popup/PopupManager");
                }
                return mInstance;
            }
        }

        public static PopupManager CheckInstance
        {
            get
            {
                return mInstance;
            }
        }

        private void Awake()
        {
            mInstance = this;
            mTransparentTrans = transparent.transform;
            defaultSortingOrder = canvas.sortingOrder;
            camera.gameObject.SetActive(false);
            camera.orthographicSize = Camera.main.orthographicSize;
            getExixtPopup();
            DontDestroyOnLoad(gameObject);
        }

        private void getExixtPopup()
        {
            ExistPopup.Clear();
            for (int i = 0; i < parent.childCount; i++)
            {
                BasePopup bs = parent.GetChild(i).GetComponent<BasePopup>();
    
                if (bs != null)
                {
                    ExistPopup.Add(bs);
                }
            }
        }

        public static T CreateNewInstance<T>()
        {
            return Instance.CheckInstancePopupPrebab<T>();
        }

        public T CheckExistPopup<T>() where T : BasePopup
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                GameObject bs = parent.GetChild(i).gameObject;
                if (IsOfType<T>(bs))
                {
                    return bs.GetComponent<T>();
                }
            }
            return null;
        }

        public T CheckInstancePopupPrebab<T>()
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
                    break;
                }
            }

            return false;
        }

        private bool IsOfType<T>(object value)
        {
            return value is T;
        }

        public void ChangeTransparentOrder(Transform topPopupTransform, bool active)
        {
            if (active)
            {
                mTransparentTrans.SetSiblingIndex(topPopupTransform.GetSiblingIndex() - 1);
                transparent.SetActive(true && usingDefaultTransparent);
            }
            else if (parent.childCount > 2)
            {
                mTransparentTrans.SetSiblingIndex(topPopupTransform.GetSiblingIndex() - 2);
                transparent.SetActive(active);
            }
            else
            {
                transparent.SetActive(value: false);
            }
        }

        public bool SequenceHidePopup(Action cb = null)
        {
            if (popupStacks.Count > 0)
            {
                popupStacks.Peek().Hide(cb);
            }
            else
            {
                transparent.SetActive(value: false);
            }
            return popupStacks.Count > 0;
        }

        public bool CheckStack(Type type)
        {
            return popupStacks.Count > 0 && popupStacks.Peek().GetType() == type;
        }

        public static T LoadResource<T>(string name)
        {
            GameObject gameObject = (GameObject)Instantiate(Resources.Load(name));
            gameObject.name = $"[{name}]";
            DontDestroyOnLoad(gameObject);
            return gameObject.GetComponent<T>();
        }

        public void SetSortingOrder(int order)
        {
            canvas.sortingOrder = order;
        }

        public void ResetOrder()
        {
            canvas.sortingOrder = defaultSortingOrder;
        }

        public static void ActiveCamera()
        {
            Instance.camera.gameObject.SetActive(true);
            Instance.canvas.worldCamera = Instance.camera;
        }

        public static void DisableCamera()
        {
            Instance.camera.gameObject.SetActive(false);
            Instance.canvas.worldCamera = null;
        }
    }
}
