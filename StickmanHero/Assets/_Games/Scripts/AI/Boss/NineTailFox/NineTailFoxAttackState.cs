using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class NineTailFoxAttackState : State
    {
        NineTailFox nineTailFox;
        Transform target;
        float timeRecover;
        int currentDirection;
        public NineTailFoxAttackState(NineTailFox controller, string stateName) : base(controller, stateName)
        {
            nineTailFox = controller;
        }
        public override void EnterState()
        {
            controller.core.movement.SetVelocityX(0);
            Collider2D[] colls = nineTailFox.GetTargetsInView();
            if (colls.Length == 0)
            {
                nineTailFox.SwitchState(nineTailFox.idleState);
                return;
            }
            target = colls[0].transform;
            Vector3 direction = (target.position - nineTailFox.transform.position);
            if (direction.normalized.x < 0)
            {
                currentDirection = -1;
            }
            if (direction.normalized.x > 0)
            {
                currentDirection = 1;
            }
            if (currentDirection != controller.core.movement.facingDirection)
            {
                nineTailFox.SwitchState(nineTailFox.chaseState);
                return;
            }
            float distance = Vector3.Distance(nineTailFox.transform.position, target.position);
            if (distance <= 4)
            {
                int r = Random.Range(0, 100);
                if (r < 30)
                {
                    nineTailFox.AddAction(new NTFComboAction(nineTailFox, 1.5f));
                    nineTailFox.AddAction(new NTFComboAction(nineTailFox, 1f));
                    timeRecover = 3;
                }
                else if (r >= 30 && r < 60)
                {
                    nineTailFox.AddAction(new NTFFireOrbAction(nineTailFox));
                    nineTailFox.AddAction(new NTFComboAction(nineTailFox, 1f));
                    timeRecover = 3;
                }
                else
                {
                    nineTailFox.AddAction(new NTFComboAction(nineTailFox, 1f));
                    nineTailFox.AddAction(new NTFLaserAction(nineTailFox, 3));
                    timeRecover = 4;
                }
            }
            else
            {
                int r = Random.Range(0, 100);
                if (r <= 50)
                {
                    nineTailFox.SwitchState(nineTailFox.chaseState);
                    return;
                }
                else
                {
                    nineTailFox.AddAction(new NTFFireOrbAction(nineTailFox));
                    nineTailFox.AddAction(new NTFLaserAction(nineTailFox, 3));
                    timeRecover = 4;
                }
            }
        }

        public override void ExitState()
        {

        }

        public override void UpdateLogic()
        {
            timeRecover -= Time.deltaTime;
            if (timeRecover <= 0)
            {
                nineTailFox.SwitchState(nineTailFox.idleState);
            }
        }

        public override void UpdatePhysic()
        {

        }
    }
}

