using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class NineTailFoxIdleState : State
    {
        NineTailFox nineTailFox;
        public float idleTime;
        public NineTailFoxIdleState(NineTailFox controller, string stateName) : base(controller, stateName)
        {
            nineTailFox = controller;
        }
        public override void EnterState()
        {
            nineTailFox.core.movement.SetVelocityX(0);
        }
        public override void ExitState()
        {

        }
        public override void UpdateLogic()
        {
            if (!controller.isInteracting)
            {
                nineTailFox.SwitchState(nineTailFox.attackState);
            }

        }
        public override void UpdatePhysic()
        {

        }
    }

}
