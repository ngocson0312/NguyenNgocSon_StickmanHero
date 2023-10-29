using UnityEngine;
namespace SuperFight
{
    public class CollisionSenses : CoreComponent
    {
        [Header("GroundCheck")]
        public float footOffset = 0.2f;
        public float groundRayDistance = 0.2f;
        public LayerMask groundLayer;
        public Vector2 groundAheadRayOffset;
        public Vector2 wallRayOffset;
        public bool IsOnGround()
        {
            RaycastHit2D rightFoot = RayCast(new Vector2(footOffset, 0), Vector2.down, groundRayDistance, groundLayer);
            RaycastHit2D leftFoot = RayCast(new Vector2(-footOffset, 0), Vector2.down, groundRayDistance, groundLayer);
            if (rightFoot || leftFoot)
            {
                return true;
            }
            return false;
        }
        public bool GroundAhead()
        {
            Vector2 offsetDown = new Vector2(groundAheadRayOffset.x * core.movement.facingDirection, groundAheadRayOffset.y);
            return RayCast(offsetDown, Vector2.down, 1f, groundLayer);
        }
        public bool IsTouchWall()
        {
            return RayCast(new Vector2(wallRayOffset.x * core.movement.facingDirection, wallRayOffset.y), new Vector2(core.movement.facingDirection, 0), 1f, groundLayer);
        }
        public bool IsTouchWallBehind()
        {
            return RayCast(new Vector2(wallRayOffset.x * -core.movement.facingDirection, wallRayOffset.y), new Vector2(-core.movement.facingDirection, 0), 1f, groundLayer);
        }
        RaycastHit2D RayCast(Vector2 offset, Vector2 direction, float distance, LayerMask layerMask)
        {
            RaycastHit2D raycast = Physics2D.Raycast((Vector2)core.controller.transform.position + offset, direction, distance, layerMask);
#if UNITY_EDITOR
            Color color = raycast ? Color.red : Color.green;
            Debug.DrawRay((Vector2)core.controller.transform.position + offset, direction * distance, color);
#endif

            return raycast;
        }
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                Gizmos.DrawRay(transform.position - new Vector3(footOffset, 0), Vector3.down);
                Gizmos.DrawRay(transform.position + new Vector3(footOffset, 0), Vector3.down);
#endif
                int direction = 1;
                Vector3 offsetDown = new Vector3(groundAheadRayOffset.x * direction, groundAheadRayOffset.y);
#if UNITY_EDITOR
                // Gizmos.DrawRay(new Vector2(wallRayOffset.x * -direction, wallRayOffset.y), Vector2.left * direction);
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position + new Vector3(wallRayOffset.x * direction, wallRayOffset.y), Vector2.left * direction);
                Gizmos.DrawRay(transform.position + offsetDown, Vector2.down);
#endif
            }
        }
    }
}

