using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlugins
{
    public enum ScreenStatus
    {
        Foreground = 0,

        Background,
        Destroy
    }
    [RequireComponent(typeof(Animation))]
    public class BaseScreen : MonoBehaviour
    {
        [HideInInspector]
        public Animation screenanimation;

        public AnimationClip showAnimationClip;

        public AnimationClip hideAnimationClip;
        public bool isCache = false;

        public ScreenStatus screenStatus;
        private ScreenStatus screenStatusinUnityGameObjectLifecycle;
        private bool isCallShow = false;

        private int mSortOrder;

        private Transform mTransform;

        private Stack<BaseScreen> refStacks;

        private Action hideCompletedCallback;

        private Action showCompletedCallback;
        private bool isAniShow = false;
        private bool isAniHide = false;
        private int flagCheckFollow = 0;

        public virtual void Awake()
        {
            flagCheckFollow++;
            initScreen();
        }

        private void initScreen()
        {
            if (mTransform == null || refStacks == null)
            {
                screenanimation = GetComponent<Animation>();
                mTransform = base.transform;
                mSortOrder = mTransform.GetSiblingIndex();
                if (ScreenManager.CheckInstance != null)
                {
                    refStacks = ScreenManager.Instance.screenStacks;
                }
                if (screenanimation != null && showAnimationClip != null && hideAnimationClip != null)
                {
                    screenanimation.AddClip(showAnimationClip, showAnimationClip.name);
                    screenanimation.AddClip(hideAnimationClip, hideAnimationClip.name);
                }
                else
                {
                    // BPDebug.LogMessage("Chưa gán Animator hoặc showAnimationClip, hideAnimationClip  cho popup " + GetType().ToString(), error: true);
                }
            }
        }

        protected virtual void OnEnable()
        {
            flagCheckFollow++;
            screenStatusinUnityGameObjectLifecycle = ScreenStatus.Foreground;
        }

        protected virtual void Start()
        {
            flagCheckFollow++;
            checkFlowScreen();
            refStacks = ScreenManager.Instance.screenStacks;
            if (!isCallShow)
            {
                Show(false);
            }
        }
        private void checkFlowScreen() {
            if (flagCheckFollow < 3) {
                throw new Exception("Screen flow incorrect please check override Awake, OnEnable");
            }
        }
        protected virtual void OnDisable()
        {
            screenStatusinUnityGameObjectLifecycle = ScreenStatus.Background;
        }
        public virtual void onEnableControl()
        {
            Debug.Log("mysdk: Screen onEnableControl of " + gameObject.name);
        }
        public virtual void onDisableControl()
        {
            Debug.Log("mysdk: Screen onDisableControl of " + gameObject.name);
        }
        public void Show(bool isAni = false, Action showCompletedCallback = null, Action hideCompletedCallback = null)
        {
            Debug.Log("mysdk: Screen show " + gameObject.name);
            initScreen();
            isCallShow = true;
            if (!isAni && isAniHide)
            {
                isAni = true;
            }
            isAniShow = isAni;
            gameObject.SetActive(true);
            screenStatus = ScreenStatus.Foreground;
            this.showCompletedCallback = showCompletedCallback;
            this.hideCompletedCallback = hideCompletedCallback;
            if (refStacks.Count > 0)
            {
                pushCurrScreenToBg();
            }
            ScreenManager.Instance.currentScreen = this;
            if (!refStacks.Contains(this))
            {
                checkListCache();
                refStacks.Push(this);
            }
            else
            {
                int num = refStacks.Peek().SortOrder();
                if (refStacks.Count > 1 && SortOrder() != num)
                {
                    MoveElementToTopStack(ref refStacks, SortOrder());
                }
            }
            if (refStacks.Count > 0)
            {
                ChangeSortOrder(refStacks.Peek().SortOrder() + 1);
            }

            AnimateShow(isAni);
        }

        private void GotoForeground()
        {
            Debug.Log("mysdk: Screen GotoForeground " + gameObject.name);
            if (screenanimation != null && showAnimationClip != null)
            {
                screenanimation.Play(showAnimationClip.name);
                float animationClipDuration = GetAnimationClipDuration(showAnimationClip);
                Invoke("OnShowFinish", animationClipDuration);
            }
        }

        private void AnimateShow(bool isAni)
        {
            Debug.Log("mysdk: Screen " + gameObject.name + " isAni=" + isAni);
            if (isAni && screenanimation != null && showAnimationClip != null)
            {
                float animationClipDuration = GetAnimationClipDuration(showAnimationClip);
                Invoke("OnShowFinish", animationClipDuration + .1f);
                screenanimation.Play(showAnimationClip.name);
            }
            else
            {
                OnShowFinish();
            }
        }

        public virtual void OnShowFinish()
        {
            Debug.Log("mysdk: Screen " + gameObject.name + " OnShowFinish");
            onEnableControl();
            if (showCompletedCallback != null)
            {
                showCompletedCallback();
            }
            Invoke("checkFlowScreen", 0.5f);
        }

        private void Hide(Action hideCompletedCallback = null)
        {
            Debug.Log("mysdk: Screen hide " + gameObject.name);
            screenStatus = ScreenStatus.Background;
            this.hideCompletedCallback = hideCompletedCallback;
            onDisableControl();
            if (isCallShow || gameObject.activeInHierarchy)
            {
                isCallShow = false;
                AnimateHide(false, false);
            }
        }
        public void Finish(bool isAni, Action hideCompletedCallback = null)
        {
            Debug.Log("mysdk: Screen finish " + gameObject.name);
            screenStatus = ScreenStatus.Destroy;
            this.hideCompletedCallback = hideCompletedCallback;
            onDisableControl();
            if (isCallShow)
            {
                isCallShow = false;
                AnimateHide(isAni, true);
            }

        }

        private void AnimateHide(bool isAni, bool isFinish)
        {
            Debug.Log("mysdk: Screen " + gameObject.name + " AnimateHide, isAni=" + isAni + ", isFinish=" + isFinish);
            isAniHide = false;
            if (isAniShow && screenanimation != null && hideAnimationClip != null)
            {
                screenanimation.Play(hideAnimationClip.name);
                isAniHide = true;
                float animationClipDuration = GetAnimationClipDuration(hideAnimationClip);
                if (!isFinish)
                {
                    if (Time.timeScale != 0)
                    {
                        Invoke("gotoBackground", animationClipDuration);
                    }
                    else
                    {
                        gotoBackground();
                    }
                }
                else
                {
                    if (Time.timeScale != 0)
                    {
                        Invoke("Destroy", animationClipDuration);
                    }
                    else
                    {
                        Destroy();
                    }
                }
            }
            else
            {
                if (!isFinish)
                {
                    gotoBackground();
                }
                else
                {
                    Destroy();
                }

            }
        }

        private void gotoBackground()
        {
            Debug.Log("mysdk: Screen gotoBackground " + gameObject.name);
            base.gameObject.SetActive(false);
            hideCompletedCallback?.Invoke();
            ScreenManager.Instance.ResetOrder();
        }

        private void Destroy()
        {
            Debug.Log("mysdk: Screen Destroy " + gameObject.name);
            if (refStacks.Contains(this))
            {
                List<BaseScreen> ltmp = new List<BaseScreen>();
                while (refStacks.Count > 0)
                {
                    BaseScreen ob = refStacks.Pop();
                    if (ob.Equals(this))
                    {
                        break;
                    }
                    else
                    {
                        ltmp.Add(ob);
                    }
                }
                for (int i = (ltmp.Count - 1); i >= 0; i--)
                {
                    refStacks.Push(ltmp[i]);
                }
                ltmp.Clear();
            }
            if (this.Equals(ScreenManager.Instance.currentScreen) && refStacks.Count > 0)
            {
                ScreenManager.Instance.currentScreen = null;
                refStacks.Peek().Show(false);
            }
            if (this.isCache)
            {
                base.gameObject.SetActive(false);
                if (!ScreenManager.Instance.cacheScreen.Contains(this))
                {
                    ScreenManager.Instance.cacheScreen.Add(this);
                }
                else
                {
                    Debug.Log("mysdk: Screen Destroy " + gameObject.name + ", cache follow err");
                }
            }
            else
            {
                if (gameObject.activeSelf)
                {
                    DestroyImmediate(base.gameObject);
                }
            }

            hideCompletedCallback?.Invoke();
            ScreenManager.Instance.ResetOrder();
        }

        public int SortOrder()
        {
            return mSortOrder;
        }

        public void ChangeSortOrder(int newSortOrder = -1)
        {
            if (newSortOrder != -1)
            {
                mTransform.SetSiblingIndex(newSortOrder);
                mSortOrder = newSortOrder;
            }
        }

        private void pushCurrScreenToBg()
        {
            Debug.Log("mysdk: Screen pushCurrScreenToBg " + gameObject.name);
            if (ScreenManager.Instance.currentScreen != null)
            {
                Debug.Log("mysdk: Screen pushCurrScreenToBg " + ScreenManager.Instance.currentScreen.gameObject.name);
                ScreenManager.Instance.currentScreen.Hide();
            }
        }

        private float GetAnimationClipDuration(AnimationClip clip)
        {
            if (screenanimation != null && clip != null)
            {
                return screenanimation.GetClip(clip.name).length;
            }

            return 0f;
        }

        private void MoveElementToTopStack(ref Stack<BaseScreen> stack, int order)
        {
            Debug.Log("mysdk: Screen MoveElementToTopStack " + gameObject.name + ", order=" + order);
            Stack<BaseScreen> stack2 = new Stack<BaseScreen>();
            BaseScreen baseScreen = null;
            int num = 0;
            while (refStacks.Count > 0)
            {
                BaseScreen baseScreen2 = refStacks.Pop();
                if (baseScreen2.SortOrder() != order)
                {
                    stack2.Push(baseScreen2);
                    num = baseScreen2.SortOrder();
                }
                else
                {
                    baseScreen = baseScreen2;
                }
            }
            while (stack2.Count > 0)
            {
                BaseScreen baseScreen3 = stack2.Pop();
                baseScreen3.ChangeSortOrder(num++);
                stack.Push(baseScreen3);
            }
            if (baseScreen != null)
            {
                baseScreen.ChangeSortOrder(num);
                stack.Push(baseScreen);
            }
        }
        private void checkListCache()
        {
            ScreenManager.Instance.cacheScreen.Remove(this);
        }
    }
}
