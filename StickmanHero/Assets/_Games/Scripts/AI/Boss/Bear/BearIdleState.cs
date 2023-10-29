using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class BearIdleState : State
    {
        private Bear bear;
        private int currentDirection;
        private float idleTimer;
        public BearIdleState(Bear controller, string stateName) : base(controller, stateName)
        {
            bear = controller;
        }

        public override void EnterState()
        {
            idleTimer = Random.Range(1f, 3f);
        }

        public override void ExitState()
        {

        }

        public override void UpdateLogic()
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0)
            {
                bear.SwitchState(bear.attackState);
                return;
            }
        }

        public override void UpdatePhysic()
        {
            Collider2D[] colls = bear.GetTargetsInView();
            if (colls.Length == 0 || controller.isInteracting) return;
            Vector3 direction = (colls[0].transform.position - bear.transform.position);
            if (Vector3.SqrMagnitude(colls[0].transform.position - controller.transform.position) <= 9 && Vector3.Dot(Vector3.right * controller.core.movement.facingDirection, direction) > 0)
            {
                return;
            }
            if (direction.x < 0)
            {
                currentDirection = -1;
            }
            if (direction.x > 0)
            {
                currentDirection = 1;
            }
            if (currentDirection != bear.core.movement.facingDirection)
            {
                bear.core.movement.Flip();
            }
        }
    }
}

