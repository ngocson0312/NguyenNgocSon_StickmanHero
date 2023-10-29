using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class BearChaseState : State
    {
        Bear bear;
        public BearChaseState(Bear controller, string stateName) : base(controller, stateName)
        {
            bear = controller;
        }

        public override void EnterState()
        {
        }

        public override void ExitState()
        {
        }

        public override void UpdateLogic()
        {
        }

        public override void UpdatePhysic()
        {
        }
    }
}
