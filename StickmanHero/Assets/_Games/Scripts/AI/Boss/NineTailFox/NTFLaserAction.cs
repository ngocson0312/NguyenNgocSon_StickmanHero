using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class NTFLaserAction : AIAction
    {
        NineTailFox nineTailFox;
        float skillTimer;
        float timeRecover = 0;
        int skillState;
        public NTFLaserAction(NineTailFox controller, float timeRecover) : base(controller)
        {
            nineTailFox = controller;
            skillTimer = 1;
            this.timeRecover = timeRecover;
        }

        public override void ExitAction()
        {
            nineTailFox.FinishAction();
        }
        public override void OnActionEnter()
        {
            controller.animatorHandle.PlayAnimation("Laser", 0.1f, 1, true);
            skillState = 0;
        }

        public override void UpdateLogic(float deltaTime)
        {
            skillTimer -= Time.deltaTime;
            if (skillTimer <= 0f && skillState == 0)
            {
                controller.animatorHandle.PlayAnimation("LaserEnd", 0.1f, 1, true);
                skillState = 1;
            }
            if(skillTimer <= -0.5f && skillState == 1)
            {
                ExitAction();
            }
        }

        public override void UpdatePhysic(float fixedDeltaTime)
        {

        }
    }
}

