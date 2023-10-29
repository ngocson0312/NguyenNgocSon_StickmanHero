using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class Core : MonoBehaviour
    {
        public Controller controller { get; private set; }
        public Movement movement;
        public CollisionSenses collisionSenses;
        public Combat combat;
        bool pause;
        public void Initialize(Controller controller)
        {
            this.controller = controller;
            movement.Initialize(this);
            collisionSenses.Initialize(this);
            combat.Initialize(this);
            pause = false;
        }
        public void UpdateLogicCore()
        {
            if (pause) return;
            movement.UpdateLogic();
            combat.UpdateLogic();
        }
        public void Active()
        {
            pause = false;
            gameObject.SetActive(true);
            movement.SetVelocityZero();
        }
        public void Deactive()
        {
            gameObject.SetActive(false);
        }
        public void Pause()
        {
            pause = true;
            movement.SetBodyType(RigidbodyType2D.Static);
        }
        public void Resume()
        {
            pause = false;
            movement.SetBodyType(RigidbodyType2D.Dynamic);
        }
    }
}
