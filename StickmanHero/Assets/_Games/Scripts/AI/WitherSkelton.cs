namespace SuperFight
{
    public class WitherSkelton : GroundEnemy
    {
        public override void Initialize()
        {
            base.Initialize();
            patrol = new BasicAttackState(this, "attack");
            chaseState = new BasicAttackState(this, "attack");
        }
    }
}

