namespace SuperFight
{
    public class NTFComboAction : AIAction
    {
        NineTailFox nineTailFox;
        float actionTime;
        public NTFComboAction(NineTailFox controller, float actionTime) : base(controller)
        {
            nineTailFox = controller;
            this.actionTime = actionTime;
        }

        public override void ExitAction()
        {
            nineTailFox.FinishAction();
        }

        public override void OnActionEnter()
        {
            nineTailFox.animator.PlayAnimation("Attack1", 0.1f, 1, true);
        }

        public override void UpdateLogic(float deltaTime)
        {
            actionTime -= deltaTime;
            if (actionTime <= 0)
            {
                ExitAction();
            }
        }

        public override void UpdatePhysic(float fixedDeltaTime)
        {

        }

    }
}

