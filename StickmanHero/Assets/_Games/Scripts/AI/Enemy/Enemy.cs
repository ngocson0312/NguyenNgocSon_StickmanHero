using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public abstract class Enemy : Controller
    {
        public float speed = 5f;
        private State currentState;
        public SpriteBar healthBar;
        public ParticleSystem hitFX;
        protected bool isInit;
        protected float delayTimePlaySoundFx;
        protected Vector3 startPosition;
        public EnemyAnimator animator{get;set;}
        public virtual void Initialize()
        {
            isInit = true;
            core.Initialize(this);
            animatorHandle.Initialize(this);
            animator = (EnemyAnimator)animatorHandle;
            stats.ResetStats();
            isActive = false;
        }
        protected virtual void LogicUpdate()
        {
            // character.SetBool("IsOnGround", core.collisionSenses.IsOnGround());
            // character.SetBool("IsStunning", isStunning);
            // character.SetFloat("MoveAmount", Mathf.Abs(core.movement.currentVelecity.x));
            // character.SetFloat("VelocityY", core.movement.currentVelecity.y);
            isInteracting = animatorHandle.animator.GetBool("IsInteracting");
            if (isActive == false || GameplayCtrl.Instance.gameState != GAME_STATE.GS_PLAYING) return;
            core.UpdateLogicCore();
            if (currentState != null && GameplayCtrl.Instance.gameState == GAME_STATE.GS_PLAYING)
            {
                currentState.UpdateLogic();
            }
            delayTimePlaySoundFx -= Time.deltaTime;
            
        }
        public virtual void ResetStatEnemy(CharacterStats overrideStats)
        {
            startPosition = transform.position;
            animatorHandle.ResetAnimator();
            isActive = true;
            core.Active();
            core.Resume();
            if (overrideStats == null)
            {
                stats.ResetStats();
            }
            else
            {
                stats = overrideStats;
                stats.ResetStats();
            }
            if (GameManager.Instance.CurrLevel > GameManager.Instance.maxLevel)
            {
                int health = DataManager.Instance.playerDamage * Random.Range(3, 8);
                int damage = (int)(DataManager.Instance.playerMaxHp / Random.Range(5, 10));
                stats = new CharacterStats(health, damage);
                stats.ResetStats();
            }
            GetComponent<Collider2D>().enabled = true;
        }
        protected virtual void PhysicUpdate()
        {
            if (isActive == false || GameplayCtrl.Instance.gameState != GAME_STATE.GS_PLAYING) return;
            if (currentState != null)
            {
                currentState.UpdatePhysic();
            }
        }
        private void Update()
        {
            LogicUpdate();
        }
        private void FixedUpdate()
        {
            PhysicUpdate();
        }
       
        public abstract void DetectPlayer();
        public void SwitchState(State newState)
        {
            if (currentState != null)
            {
                currentState.ExitState();
            }
            currentState = newState;
            currentState.EnterState();
        }
        public abstract Controller GetTargetInView();
        public override void Die(bool deactiveCharacter)
        {
            if(!isActive) return;
            base.Die(deactiveCharacter);
            healthBar.Deactive();
            core.Deactive();
            GetComponent<Collider2D>().enabled = false;
            GameplayCtrl.Instance.freeEnemyBeKill(this, 2f);
            GameplayCtrl.Instance.enemyBeKill(this);
        }
        public override void OnTakeDamage(DamageInfo damageInfo)
        {
            UltimateTextDamageManager.Instance.Add(damageInfo.damage.ToString(), transform.position + Vector3.up * 2);
            animatorHandle.PlayAnimation("Stun", 0.1f, 1, true);
            hitFX.Play();
        }
    }
}

