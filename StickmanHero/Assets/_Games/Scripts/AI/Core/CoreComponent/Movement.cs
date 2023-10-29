using UnityEngine;
namespace SuperFight
{
    public class Movement : CoreComponent
    {
        private Rigidbody2D rb;
        public Vector2 currentVelecity;
        public Vector2 workSpace;
        public int facingDirection { get; private set; }
        public override void Initialize(Core core)
        {
            base.Initialize(core);
            rb = core.controller.GetComponent<Rigidbody2D>();
            facingDirection = 1;
        }
        public void UpdateLogic()
        {
            currentVelecity = rb.velocity;
        }
        public void SetVelocityX(float velocityX)
        {
            workSpace.Set(velocityX, currentVelecity.y);
            SetFinalVelocity();
        }
        public void SetVelocityY(float velocityY)
        {
            workSpace.Set(currentVelecity.x, velocityY);
            SetFinalVelocity();
        }
        public void SetVelocityZero()
        {
            workSpace = Vector2.zero;
            SetFinalVelocity();
        }
        public void SetVelocity(Vector2 direction, float velocity)
        {
            workSpace = direction * velocity;
            SetFinalVelocity();
        }
        public void SetVelocity(Vector2 velocity)
        {
            workSpace = velocity;
            SetFinalVelocity();
        }
        public void SetBodyType(RigidbodyType2D type)
        {
            rb.bodyType = type;
        }
        private void SetFinalVelocity()
        {
            rb.velocity = workSpace;
            currentVelecity = workSpace;
        }
        public void Flip()
        {
            facingDirection *= -1;
        }
    }
}

