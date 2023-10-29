using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class BearAttackState : State
    {
        private Bear bear;
        private float timeRecover;
        public BearAttackState(Bear controller, string stateName) : base(controller, stateName)
        {
            bear = controller;
        }

        public override void EnterState()
        {
            bear.AddAction(new BearRagingAction(bear, bear.GetTargetsInView()[0].transform));
            timeRecover = 3;
        }

        public override void ExitState()
        {
        }

        public override void UpdateLogic()
        {
            timeRecover -= Time.deltaTime;
            if (timeRecover <= 0)
            {
                bear.SwitchState(bear.idleState);
                return;
            }
        }

        public override void UpdatePhysic()
        {
        }
    }
}

