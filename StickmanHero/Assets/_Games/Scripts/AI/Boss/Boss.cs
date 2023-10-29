using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public abstract class Boss : Controller
    {
        protected BossFightArena arena;
        protected State currentState;
        protected Queue<AIAction> queueAction = new Queue<AIAction>();
        public virtual void Initialize(BossFightArena arena)
        {
            this.arena = arena;
            isActive = false;
            core.Initialize(this);
            animatorHandle.Initialize(this);
            stats.ResetStats();
        }
        public abstract void Active();
        private void Update()
        {
            if (!isActive) return;
            core.UpdateLogicCore();
            if (currentState != null)
            {
                Debug.Log(currentState.stateName);
                currentState.UpdateLogic();
            }
            if (queueAction.Count > 0 && GameplayCtrl.Instance.gameState == GAME_STATE.GS_PLAYING)
            {
                queueAction.Peek().UpdateLogic(Time.deltaTime);
            }
            UpdateLogic();
        }
        private void FixedUpdate()
        {
            if (!isActive) return;
            if (currentState != null)
            {
                currentState.UpdatePhysic();
            }
            if (queueAction.Count > 0 && GameplayCtrl.Instance.gameState == GAME_STATE.GS_PLAYING)
            {
                queueAction.Peek().UpdatePhysic(Time.fixedDeltaTime);
            }
            UpdatePhysic();
        }
        public void SwitchState(State newState)
        {
            if (currentState != null)
            {
                currentState.ExitState();
            }
            currentState = newState;
            currentState.EnterState();
        }
        public void AddAction(AIAction newAction)
        {
            if (queueAction.Count == 0)
            {
                newAction.OnActionEnter();
            }
            queueAction.Enqueue(newAction);
        }
        public void FinishAction()
        {
            queueAction.Dequeue();
            if (queueAction.Count > 0)
            {
                queueAction.Peek().OnActionEnter();
            }
        }
        public override void OnTakeDamage(DamageInfo damageInfo)
        {
            UltimateTextDamageManager.Instance.Add(damageInfo.damage.ToString(), transform.position + Vector3.up * 2);
            arena.OnBossTakeDamage(damageInfo.damage, this);
            stats.ApplyDamage(damageInfo.damage);
        }
        protected abstract void UpdateLogic();
        protected abstract void UpdatePhysic();
        
        public override void Die(bool deactiveCharacter)
        {
            base.Die(deactiveCharacter);
            core.Deactive();
        }
    }
}

