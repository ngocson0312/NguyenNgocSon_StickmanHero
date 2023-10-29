using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlugins
{
    [RequireComponent(typeof(Animation))]
    public class BasePopup : MonoBehaviour
    {
        [HideInInspector]
        public Animation animation;

        public AnimationClip showAnimationClip;

        public AnimationClip hideAnimationClip;
        public bool isCache = false;

        protected bool isShowed;

        private int mSortOrder;

        private Transform mTransform;

        private Stack<BasePopup> refStacks;

        private Action hideCompletedCallback;

        private Action showCompletedCallback;

        public virtual void Awake()
        {
            isShowed = false;
            animation = GetComponent<Animation>();
            mTransform = base.transform;
            mSortOrder = mTransform.GetSiblingIndex();
            refStacks = PopupManager.Instance.popupStacks;
            if (animation != null && showAnimationClip != null && hideAnimationClip != null)
            {
                animation.AddClip(showAnimationClip, showAnimationClip.name);
                animation.AddClip(hideAnimationClip, hideAnimationClip.name);
            }
            else
            {
                // BPDebug.LogMessage("Chưa gán Animator hoặc showAnimationClip, hideAnimationClip  cho popup " + GetType().ToString(), error: true);
            }
        }

        public void Show(Action showCompletedCallback = null, Action hideCompletedCallback = null, bool overlay = false, bool isRenderCamera = false)
        {
            if (ScreenManager.CheckInstance && refStacks.Count == 0)
            {
                ScreenManager.CheckInstance.showPopup();
            }
            if (isRenderCamera) PopupManager.ActiveCamera();
            else PopupManager.DisableCamera();
            if (isShowed)
            {
                Reshow();
                int num = refStacks.Peek().SortOrder();
                if (refStacks.Count > 1 && SortOrder() != num)
                {
                    MoveElementToTopStack(ref refStacks, SortOrder());
                }
                return;
            }
            this.showCompletedCallback = showCompletedCallback;
            this.hideCompletedCallback = hideCompletedCallback;
            float waitTime = 0f;
            isShowed = true;
            if (!overlay && refStacks.Count > 0)
            {
                ForceHideAllCurrent(ref waitTime);
            }
            if (!refStacks.Contains(this))
            {
                checkListCache();
                refStacks.Push(this);
            }
            if (refStacks.Count > 0)
            {
                ChangeSortOrder(refStacks.Peek().SortOrder() + 1);
            }
            if (waitTime != 0f)
            {
                Invoke("AnimateShow", waitTime);
            }
            else
            {
                AnimateShow();
            }
        }

        private void Reshow()
        {
            Debug.Log("mysdk: popup Reshow " + gameObject.name);
            if (animation != null && showAnimationClip != null)
            {
                animation.Play(showAnimationClip.name);
                float animationClipDuration = GetAnimationClipDuration(showAnimationClip);
                Invoke("OnShowFinish", animationClipDuration);
            }
            PopupManager.Instance.ChangeTransparentOrder(mTransform, active: true);
        }

        private void AnimateShow()
        {
            Debug.Log("mysdk: popup AnimateShow " + gameObject.name);
            gameObject.SetActive(true);
            if (animation != null && showAnimationClip != null)
            {
                float animationClipDuration = GetAnimationClipDuration(showAnimationClip);
                Invoke("OnShowFinish", animationClipDuration + .1f);
                animation.Play(showAnimationClip.name);
            }
            else
            {
                OnShowFinish();
            }
            PopupManager.Instance.ChangeTransparentOrder(mTransform, active: true);
        }

        public virtual void OnShowFinish()
        {
            Debug.Log("mysdk: popup OnShowFinish " + gameObject.name);
            if (showCompletedCallback != null)
            {
                showCompletedCallback();
            }
        }

        public void Hide(Action hideCompletedCallback = null, bool isRenderCamera = false)
        {
            Debug.Log("mysdk: popup hide " + gameObject.name);
            this.hideCompletedCallback = hideCompletedCallback;
            if (isRenderCamera) PopupManager.ActiveCamera();
            else PopupManager.DisableCamera();
            if (isShowed)
            {
                isShowed = false;
                AnimateHide();
            }
        }

        public virtual void OnCloseClick()
        {
            Hide();
        }

        private void AnimateHide()
        {
            Debug.Log("mysdk: popup AnimateHide " + gameObject.name);
            PopupManager.Instance.ChangeTransparentOrder(mTransform, active: false);
            if (animation != null && hideAnimationClip != null)
            {
                animation.Play(hideAnimationClip.name);
                float animationClipDuration = GetAnimationClipDuration(hideAnimationClip);
                if (Time.timeScale != 0) Invoke("Destroy", animationClipDuration);
                else Destroy();
            }
            else
            {
                Destroy();
            }
        }

        private void Destroy()
        {
            Debug.Log("mysdk: popup Destroy " + gameObject.name);
            if (refStacks.Contains(this))
            {
                List<BasePopup> ltmp = new List<BasePopup>();
                while (refStacks.Count > 0)
                {
                    BasePopup ob = refStacks.Pop();
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

            if (this.isCache)
            {
                base.gameObject.SetActive(false);
                if (!PopupManager.Instance.cachePopup.Contains(this))
                {
                    PopupManager.Instance.cachePopup.Add(this);
                }
                else
                {
                    Debug.Log("mysdk: popup Destroy " + gameObject.name + ", cache follow err");
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
            PopupManager.Instance.ResetOrder();

            if (refStacks.Count == 0)
            {
                if (ScreenManager.CheckInstance)
                {
                    ScreenManager.CheckInstance.hidePopup();
                }
            }
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

        private void ForceHideAllCurrent(ref float waitTime)
        {
            Debug.Log("mysdk: popup ForceHideAllCurrent " + gameObject.name);
            while (refStacks.Count > 0)
            {
                BasePopup basePopup = refStacks.Pop();
                waitTime += basePopup.GetAnimationClipDuration(basePopup.hideAnimationClip);
                basePopup.Hide();
            }
        }

        private float GetAnimationClipDuration(AnimationClip clip)
        {
            if (animation != null && clip != null)
            {
                return animation.GetClip(clip.name).length;
            }

            return 0f;
        }

        private void MoveElementToTopStack(ref Stack<BasePopup> stack, int order)
        {
            Debug.Log("mysdk: popup MoveElementToTopStack " + gameObject.name + ", order=" + order);
            Stack<BasePopup> stack2 = new Stack<BasePopup>();
            BasePopup basePopup = null;
            int num = 0;
            while (refStacks.Count > 0)
            {
                BasePopup basePopup2 = refStacks.Pop();
                if (basePopup2.SortOrder() != order)
                {
                    stack2.Push(basePopup2);
                    num = basePopup2.SortOrder();
                }
                else
                {
                    basePopup = basePopup2;
                }
            }
            while (stack2.Count > 0)
            {
                BasePopup basePopup3 = stack2.Pop();
                basePopup3.ChangeSortOrder(num++);
                stack.Push(basePopup3);
            }
            if (basePopup != null)
            {
                basePopup.ChangeSortOrder(num);
                stack.Push(basePopup);
            }
        }
        private void checkListCache()
        {
            PopupManager.Instance.cachePopup.Remove(this);
        }
    }
}
