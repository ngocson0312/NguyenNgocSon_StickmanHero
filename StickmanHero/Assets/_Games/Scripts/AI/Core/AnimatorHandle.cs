using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public abstract class AnimatorHandle : MonoBehaviour
    {
        public Animator animator { get; protected set; }
        protected Controller controller;
        public virtual void Initialize(Controller controller)
        {
            animator = GetComponent<Animator>();
            this.controller = controller;
        }
        public abstract void ResetAnimator();
        public virtual void PauseAnimator()
        {
            animator.speed = 0;
        }
        public virtual void ResumeAnimator()
        {
            animator.speed = 1;
        }
        public void SetFloat(string parameter, float value)
        {
            animator.SetFloat(parameter, value);
        }
        public void SetBool(string parameter, bool status)
        {
            animator.SetBool(parameter, status);
        }
        public void PlayAnimation(string stateName, float normalizedTransitionDuration, int layer, bool isInteracting)
        {
            animator.CrossFade(stateName, normalizedTransitionDuration, layer);
            SetBool("IsInteracting", isInteracting);
        }
        public virtual void DeactiveCharacter()
        {
            gameObject.SetActive(false);
        }
    }

}
