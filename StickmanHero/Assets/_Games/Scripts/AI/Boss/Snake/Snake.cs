using UnityEngine;
namespace SuperFight
{
    public class Snake : Boss
    {
        public override void Initialize(BossFightArena arena)
        {
            base.Initialize(arena);
        }
        public override void Active()
        {
            animatorHandle.PlayAnimation("ShowUp", 0, 1, true);
        }

        protected override void UpdateLogic()
        {
        }

        protected override void UpdatePhysic()
        {
        }
    }
}

