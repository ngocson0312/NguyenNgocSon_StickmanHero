using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class ExecutionerAttackState : State
    {
        Executioner executioner;
        float timeRecover;
        public ExecutionerAttackState(Executioner controller, string stateName) : base(controller, stateName)
        {
            executioner = controller;
        }

        public override void EnterState()
        {
            Collider2D[] colls = executioner.GetTargetsInView();
            if(colls.Length == 0)
            {
                executioner.SwitchState(executioner.idleState);
                return;
            }
            Transform target = colls[0].transform;
            if (Vector3.SqrMagnitude(controller.transform.position - target.position) > 16)
            {
                executioner.AddAction(new JumpingAttackAction(executioner, target, 0));
                executioner.AddAction(new ThrowAxeAction(executioner, target, 0.5f));
                timeRecover = 3f;
            }
            else
            {
                int r = Random.Range(0, 100);
                if (r < 30)
                {
                    executioner.AddAction(new ComboAction(executioner, target, 0f));
                    executioner.AddAction(new JumpingAttackAction(executioner, target, 0));
                    timeRecover = 4f;
                }
                else if (r > 70)
                {
                    executioner.AddAction(new ComboAction(executioner, target, 0));
                    executioner.AddAction(new ThrowAxeAction(executioner, target, 0.5f));
                    executioner.AddAction(new JumpingAttackAction(executioner, target, 0));
                    timeRecover = 5f;
                }
                else
                {
                    executioner.AddAction(new ThrowAxeAction(executioner, target, 0.5f));
                    executioner.AddAction(new ComboAction(executioner, target, 0));
                    executioner.AddAction(new ThrowAxeAction(executioner, target, 0.5f));
                    timeRecover = 4f;
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
                executioner.SwitchState(executioner.idleState);
                return;
            }
        }

        public override void UpdatePhysic()
        {

        }
    }
}

