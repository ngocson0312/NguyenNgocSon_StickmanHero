
namespace SuperFight
{
    public abstract class AIAction
    {
        protected Controller controller;
        public AIAction(Controller controller)
        {
           this.controller = controller;
        }
        public abstract void OnActionEnter();
        public abstract void UpdatePhysic(float fixedDeltaTime);
        public abstract void UpdateLogic(float deltaTime);
        public abstract void ExitAction();
    }
}

