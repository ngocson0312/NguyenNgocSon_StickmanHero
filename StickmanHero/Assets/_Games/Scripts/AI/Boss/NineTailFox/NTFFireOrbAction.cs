using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class NTFFireOrbAction : AIAction
    {
        NineTailFox nineTailFox;
        float actionTimer;
        public NTFFireOrbAction(NineTailFox controller) : base(controller)
        {
            nineTailFox = controller;
            actionTimer = 2;
        }

        public override void ExitAction()
        {
            nineTailFox.FinishAction();
        }

        public override void OnActionEnter()
        {
            nineTailFox.animator.PlayAnimation("ThrowFlameOrb", 0.1f, 1, true);
            
        }

        public override void UpdateLogic(float deltaTime)
        {
            actionTimer -= deltaTime;
            if (actionTimer <= 0)
            {
                ExitAction();
            }
        }

        public override void UpdatePhysic(float fixedDeltaTime)
        {
        }
    }
}
