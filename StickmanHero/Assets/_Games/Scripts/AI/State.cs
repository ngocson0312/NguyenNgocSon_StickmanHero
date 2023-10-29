using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public abstract class State
    {
        public string stateName;
        protected Controller controller;
        public State(Controller controller, string stateName)
        {
            this.controller = controller;
            this.stateName = stateName;
        }
        public abstract void EnterState();
        public abstract void UpdateLogic();
        public abstract void UpdatePhysic();
        public abstract void ExitState();
    }
}
