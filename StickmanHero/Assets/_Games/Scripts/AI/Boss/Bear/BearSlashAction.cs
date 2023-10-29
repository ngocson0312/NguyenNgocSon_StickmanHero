using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class BearSlashAction : AIAction
    {

        public BearSlashAction(Controller controller) : base(controller)
        {
        }

        public override void ExitAction()
        {
        }

        public override void OnActionEnter()
        {
            controller.animatorHandle.PlayAnimation("Slash", 0.1f, 1, true);
        }

        public override void UpdateLogic(float deltaTime)
        {
        }

        public override void UpdatePhysic(float fixedDeltaTime)
        {
        }
    }
}

