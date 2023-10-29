using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimationHandle : MonoBehaviour
    {
        public Animator animator { get; private set; }
        // AICharacter aiCharacter;
        // public virtual void Initialize(AICharacter ai)
        // {
        //     aiCharacter = ai;
        //     animator = GetComponent<Animator>();
        // }
        public void PlayAnimation(string stateName, int layer, bool isInteracting)
        {
            animator.CrossFade(stateName, 0.1f, layer);
            animator.SetBool("IsInteracting", isInteracting);
        }
        private void SendDamage()
        {
            // aiCharacter.SendDamage();
        }
    }
}
