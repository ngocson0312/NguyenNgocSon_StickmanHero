using System.Collections;
using System.Collections.Generic;
using SuperFight;
using UnityEngine;
namespace SuperFight
{
    public abstract class Controller : MonoBehaviour
    {
        public Core core;
        public AnimatorHandle animatorHandle;
        public CharacterType characterType;
        public CharacterStats stats;
        public bool isInteracting;
        public bool isStunning;
        public bool isInvincible;
        public bool isActive;
        public virtual void OnTakeDamage(DamageInfo damageInfo)
        {
            UltimateTextDamageManager.Instance.Add(damageInfo.damage.ToString(), transform.position + Vector3.up * 2);
        }
        public virtual void Die(bool deactiveCharacter)
        {
            isActive = false;
            if (deactiveCharacter)
            {
                animatorHandle.DeactiveCharacter();
            }
        }
        public void ApplyNewStats(CharacterStats newStats)
        {
            stats = newStats;
            stats.ResetStats();
        }
        public virtual void Pause()
        {
            isActive = false;
            animatorHandle.PauseAnimator();
            core.Pause();
        }
        public virtual void Resume()
        {
            if (stats.health > 0)
            {
                isActive = true;
            }
            animatorHandle.ResumeAnimator();
            core.Resume();
        }
    }
}

