using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class BossTitan : Boss
    {
        [Header("Charge Stats")]
        public LayerMask groundLayer;
        public LayerMask targetLayer;
        [Header("Particle")]
        public GameObject stunFX;
        public ParticleSystem FXBossTitanDir;
        public ParticleSystem FXBossTitanBlood;
        public ParticleSystem fxDeath;
        public GameObject GroundFake;
        public Vector2 headPosition;
        public float speed = 10f;
        public float rayLength;
        public float damageRange;
        public int chargeTime;
        int chargeCount;
        float delayChargingTimer;
        bool isCharging;
        float stunTimer;
        List<Collider2D> listColl;

        public override void Initialize(BossFightArena bossFightArena)
        {
            chargeCount = 0;
            isCharging = false;
            delayChargingTimer = 0.5f;
            stunTimer = 0;
            stunFX.SetActive(false);
            base.Initialize(bossFightArena);
            core.movement.Flip();
        }
        void HandleCharge()
        {
            if (stunTimer > 0)
            {
                stunTimer -= Time.deltaTime;
                core.movement.SetVelocityX(0);
                if (stunTimer <= 0)
                {
                    stunTimer = 0;
                    chargeCount = 0;
                    chargeTime = Random.Range(2, 4);
                    stunFX.SetActive(false);
                    isStunning = false;
                }
                return;
            }
            if (delayChargingTimer > 0 && !isStunning)
            {
                delayChargingTimer -= Time.fixedDeltaTime;
                if (delayChargingTimer <= 0)
                {
                    isCharging = true;
                    listColl = new List<Collider2D>();
                }
            }
            if (isCharging)
            {
                int direction = core.movement.facingDirection;
                core.movement.SetVelocityX(direction * speed);
                Collider2D[] colls = new Collider2D[3];
                Physics2D.OverlapCircleNonAlloc((Vector2)transform.position + new Vector2(headPosition.x * direction, headPosition.y), damageRange, colls, targetLayer);
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = stats.damage;
                damageInfo.stunTime = 0.5f;
                damageInfo.hitDirection = direction;
                for (int i = 0; i < colls.Length; i++)
                {
                    if (colls[i] != null && !listColl.Contains(colls[i]) && !core.combat.IsSelfCollider(colls[i]))
                    {
                        colls[i].GetComponent<IDamage>().TakeDamage(damageInfo);
                        listColl.Add(colls[i]);
                    }
                }
            }
            if (core.collisionSenses.IsTouchWall() && !isStunning)
            {
                core.movement.Flip();
                listColl = new List<Collider2D>();
                CameraController.Instance.ShakeCamera(.35f, 1f, 10, 90, true);
                FXBossTitanDir.Play();
                delayChargingTimer = Random.Range(0.2f, 1f);
                isCharging = false;
                chargeCount++;
                if (chargeCount >= chargeTime)
                {
                    stunFX.SetActive(true);
                    isStunning = true;

                    animatorHandle.PlayAnimation("Stun", 0.1f, 1, false);
                    stunTimer = 5f;
                }
            }
            if (!isStunning)
            {
                if (core.movement.facingDirection < 0)
                {
                    animatorHandle.transform.localRotation = Quaternion.Lerp(animatorHandle.transform.localRotation, Quaternion.Euler(0, 250, 0), 0.2f);
                }
                else if (core.movement.facingDirection > 0)
                {
                    animatorHandle.transform.localRotation = Quaternion.Lerp(animatorHandle.transform.localRotation, Quaternion.Euler(0, 110, 0), 0.2f);
                }
            }
            animatorHandle.SetFloat("MoveAmount", isCharging ? 1 : 0);
        }
        RaycastHit2D RayCast(Vector2 offset, Vector2 direction, float distance, LayerMask layerMask)
        {
            Vector2 pos = transform.position;
            RaycastHit2D raycast = Physics2D.Raycast(pos + offset, direction, distance, layerMask);
            Color color = raycast ? Color.red : Color.green;
            Debug.DrawRay(pos + offset, direction * distance, color);
            return raycast;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(headPosition.x * 1, headPosition.y), damageRange);
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay((Vector2)transform.position, transform.right * 1 * rayLength);
        }

        public override void OnTakeDamage(DamageInfo damageInfo)
        {
            if (!isActive) return;
            base.OnTakeDamage(damageInfo);
            FXBossTitanBlood.Play();
            if (stats.currHealth <= 0)
            {
                fxDeath.Play();
                isActive = false;
                animatorHandle.PlayAnimation("Die", 0.1f, 1, false);
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effBossTitanDie);
                FXBossTitanDir.gameObject.SetActive(true);
                GameplayCtrl.Instance.CreateCoinOnKillBoss(transform.position, 10, 20);
                if (GroundFake != null)
                {
                    GroundFake.SetActive(false);
                }
                Die(true);
            }
            else
            {
                animatorHandle.PlayAnimation("Hit", 0.1f, 1, true);
            }
        }

        public override void Die(bool deactiveCharacter)
        {
            base.Die(deactiveCharacter);
        }

        public override void Active()
        {
            isActive = true;
        }

        protected override void UpdateLogic()
        {
            animatorHandle.SetBool("IsStunning", isStunning);
        }

        protected override void UpdatePhysic()
        {
            if (!isActive) return;
            HandleCharge();
        }
    }
}

