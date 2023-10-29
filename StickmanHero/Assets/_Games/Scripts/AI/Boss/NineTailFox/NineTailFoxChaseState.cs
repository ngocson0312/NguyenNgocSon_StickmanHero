using UnityEngine;
namespace SuperFight
{
    public class NineTailFoxChaseState : State
    {
        NineTailFox nineTailFox;
        int currentDirection;
        float chaseTimer;
        public NineTailFoxChaseState(NineTailFox controller, string stateName) : base(controller, stateName)
        {
            nineTailFox = controller;
        }

        public override void EnterState()
        {
            currentDirection = controller.core.movement.facingDirection;
            chaseTimer = Random.Range(2f, 3f);
        }

        public override void ExitState()
        {

        }
        public override void UpdateLogic()
        {
            chaseTimer -= Time.deltaTime;
            if(chaseTimer <= 0)
            {
                nineTailFox.SwitchState(nineTailFox.attackState);
                return;
            }
            Collider2D[] colls = nineTailFox.GetTargetsInView();
            if (colls.Length == 0)
            {
                nineTailFox.SwitchState(nineTailFox.idleState);
                return;
            }
            Vector3 targetPostion = colls[0].transform.position;
            Vector3 direction = (targetPostion - nineTailFox.transform.position).normalized;
            float distance = Vector3.Distance(nineTailFox.transform.position, targetPostion);
            if (distance <= 4)
            {
                nineTailFox.SwitchState(nineTailFox.attackState);
                return;
            }
        }

        public override void UpdatePhysic()
        {
            Collider2D[] colls = nineTailFox.GetTargetsInView();
            Vector3 targetPostion = colls[0].transform.position;
            Vector3 direction = (targetPostion - nineTailFox.transform.position);
            // Debug.Log(distance);
            // if (distance > 4)
            // {
            if (direction.x < 0)
            {
                currentDirection = -1;
            }
            if (direction.x > 0)
            {
                currentDirection = 1;
            }
            //}
            if (currentDirection != controller.core.movement.facingDirection || controller.core.collisionSenses.IsTouchWall())
            {
                controller.core.movement.Flip();
            }

            if (!controller.isInteracting)
            {
                controller.core.movement.SetVelocityX(nineTailFox.speed * currentDirection);
            }
        }
    }
}

