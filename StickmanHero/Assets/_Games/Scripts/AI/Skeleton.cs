using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class Skeleton : GroundEnemy
    {
        public override void Initialize()
        {
            base.Initialize();
            patrol = new BasicAttackState(this, "attack");
            chaseState = new BasicAttackState(this, "attack");
        }
    }

}
