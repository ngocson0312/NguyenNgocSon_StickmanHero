using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class Combat : CoreComponent, IDamage
    {
        private float stunTimer;
        public CharacterType getType
        {
            get => core.controller.characterType;
        }
        public int getColliderInstanceID
        {
            get { return GetComponent<Collider2D>().GetInstanceID(); }
        }
        public bool IsSelfCollider(Collider2D other)
        {
            return GetComponent<Collider2D>().Equals(other);
        }
        public override void Initialize(Core core)
        {
            base.Initialize(core);
        }
        public void TakeDamage(DamageInfo damageInfo)
        {
            if (damageInfo.characterType == getType) return;
            if (core.controller.GetInstanceID() == damageInfo.idSender || core.controller.isInvincible) return;
            AudioManager.Instance.PlayOneShot(damageInfo.impactSound, 1f);
            core.controller.isStunning = true;
            stunTimer = damageInfo.stunTime;
            core.controller.OnTakeDamage(damageInfo);
        }
        public void UpdateLogic()
        {
            if (stunTimer > 0)
            {
                if (core.collisionSenses.IsTouchWallBehind())
                {
                    stunTimer -= Time.deltaTime * 3;
                }
                else
                {
                    stunTimer -= Time.deltaTime;
                }
            }
            else
            {
                core.controller.isStunning = false;
            }
        }
    }
}

