using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class EnemyAnimator : AnimatorHandle
    {
        public Weapon currentWeapon;
        protected int currentIndexMoveSet;
        public MoveSet currentMoveSet { get; set; }
        public override void Initialize(Controller controller)
        {
            base.Initialize(controller);
        }
        public override void ResetAnimator()
        {
            gameObject.SetActive(true);
            ResumeAnimator();
            animator.Rebind();
        }
        void TriggerSkill()
        {
            if (controller.isStunning) return;
            currentWeapon.TriggerSkill(controller, currentIndexMoveSet);
        }
        public virtual void HandleCombo()
        {
            bool canDoCombo = animator.GetBool("CandoCombo");
            if (canDoCombo)
            {
                currentIndexMoveSet++;
                if (currentIndexMoveSet >= currentWeapon.moveSets.Length)
                {
                    currentIndexMoveSet = 0;
                }
                currentMoveSet = currentWeapon.moveSets[currentIndexMoveSet];
                AudioManager.Instance.PlayOneShot(currentMoveSet.activeSound, 1f);
                PlayAnimation(currentMoveSet.animationName, 0.1f, 1, true);
                DeactiveCombo();
            }
            else
            {
                if (!controller.isInteracting && !canDoCombo)
                {
                    currentIndexMoveSet = 0;
                    currentMoveSet = currentWeapon.moveSets[currentIndexMoveSet];
                    AudioManager.Instance.PlayOneShot(currentMoveSet.activeSound, 1f);
                    PlayAnimation(currentMoveSet.animationName, 0.1f, 1, true);
                }
            }
        }
        void ActiveCombo()
        {
            animator.SetBool("CandoCombo", true);
        }
        void DeactiveCombo()
        {
            animator.SetBool("CandoCombo", false);
        }
    }
}

