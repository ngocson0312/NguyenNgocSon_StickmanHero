using UnityEngine;
using System.Collections;
namespace SuperFight
{
    public class PlayerController : Controller
    {
        [Header("Refs")]
        PlayerManager player;
        InputHandle input;
        [Header("Movement Properties")]
        public float speed = 8f;
        [Header("Jump Properties")]
        public float jumpForce = 10f;
        public float jumpDuration = 0.2f;
        public float maxFallVelocity = -10f;
        public int jumpCount;
        [Header("Dash Properties")]
        public float dashForce = 10f;
        public float dashTime = 0.2f;
        private float dashTimer = 0;
        public float dashRecoveryTime = 0.5f;
        private float dashRecoveryTimer = 0f;
        [Header("Check Properties")]
        public float footOffset = 0.2f;
        public float groundRayDistance = 0.2f;
        public LayerMask groundLayer;
        [Header("Status Flags")]
        public bool isOnGround;
        public bool isDashing;
        public float moveAmount;
        public ParticleSystem dashFx;
        public ParticleSystem bloodFx;
        private FillBar healthBar;
        public void Initialize(PlayerManager playerManager)
        {
            player = playerManager;
            input = player.input;
            moveAmount = 0;
            jumpCount = 0;
            core.Initialize(this);
            PlayScreenCtl playScreenCtl = ScreenUIManager.Instance.GetScreen<PlayScreenCtl>(ScreenName.PLAYSCREEN);
            healthBar = playScreenCtl.healthBar;
        }
        public void ResetScript()
        {
            isActive = true;
            healthBar.ResetFillBar();
            stats = new CharacterStats(DataManager.Instance.playerMaxHp, DataManager.Instance.playerDamage);
            stats.ResetStats();
        }
        public void UpdateScript()
        {
            moveAmount = Mathf.Abs(input.horizontal);
            animatorHandle.SetFloat("MoveAmount", moveAmount);
            animatorHandle.SetFloat("VelocityY", core.movement.currentVelecity.y);
            animatorHandle.SetBool("IsStunning", isStunning);
            isInteracting = animatorHandle.animator.GetBool("IsInteracting");
            core.UpdateLogicCore();
            if (isOnGround && core.movement.currentVelecity.y <= 0.001f)
            {
                animatorHandle.SetBool("IsOnGround", true);
            }
            else
            {
                animatorHandle.SetBool("IsOnGround", false);
            }
            animatorHandle.SetBool("IsDashing", isDashing);
            player.transform.localScale = new Vector3(core.movement.facingDirection, 1, 1);
            PhysicCheck();
            HandleMovement();
            HandleJump();
            HandleFalling();
            HandleDash();
            HandleCombo();
        }
        public void FixedUpdateScript()
        {

        }
        void HandleCombo()
        {
            if (player.input.comboPressed)
            {
                player.character.HandleCombo();
            }
        }
        void PhysicCheck()
        {
            isOnGround = false;
            SetParentPlayer(null);
            RaycastHit2D rightFoot = RayCast(new Vector2(footOffset, 0), Vector2.down, groundRayDistance, groundLayer);
            RaycastHit2D leftFoot = RayCast(new Vector2(-footOffset, 0), Vector2.down, groundRayDistance, groundLayer);
            if (rightFoot || leftFoot)
            {
                isOnGround = true;

                if (rightFoot && rightFoot.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
                {
                    SetParentPlayer(rightFoot.collider.gameObject.transform);
                }
                else if (leftFoot && leftFoot.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
                {
                    SetParentPlayer(leftFoot.collider.gameObject.transform);
                }
            }
        }
        void SetParentPlayer(Transform _parent)
        {
            player.transform.SetParent(_parent);
        }
        void HandleMovement()
        {
            float xVelocity = player.input.horizontal * speed;
            if (isInteracting || isStunning)
            {
                xVelocity = player.input.horizontal * speed / 3;
            }
            if (player.isDead)
            {
                xVelocity = 0;
            }
            core.movement.SetVelocityX(xVelocity);
            if (player.input.horizontal != 0 && player.input.horizontal != core.movement.facingDirection)
            {
                core.movement.Flip();
            }
        }

        void HandleJump()
        {
            if (isInteracting || isStunning || player.isDead) return;
            if (isOnGround && core.movement.currentVelecity.y <= 0.001f)
            {
                jumpCount = 0;
            }
            if (input.jumpPressed && isOnGround)
            {
                jumpCount++;
                isOnGround = false;
                core.movement.SetVelocityY(jumpForce);
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effJump);
            }
            else if (input.jumpPressed && !isOnGround && jumpCount < 2)
            {
                jumpCount++;
                core.movement.SetVelocityY(jumpForce);
            }

        }
        void HandleFalling()
        {
            if (core.movement.currentVelecity.y < maxFallVelocity)
            {
                core.movement.SetVelocityY(maxFallVelocity);
            }
        }
        public override void OnTakeDamage(DamageInfo damageInfo)
        {
            if (!isActive || isInvincible || GameplayCtrl.Instance.isTestLevel) return;
            animatorHandle.PlayAnimation("Hit", 0.1f, 1, true);
            bloodFx.Play();
            SoundManager.Instance.playRandFx(TYPE_RAND_FX.FX_HIT);
            base.OnTakeDamage(damageInfo);
            stats.ApplyDamage(damageInfo.damage);
            healthBar.UpdateFillBar(stats.normalizedHealth);
            if (stats.currHealth <= 0)
            {
                animatorHandle.PlayAnimation("Dead", 0f, 1, false);
                Die(false);
                player.PlayerDie();
            }
        }
        void HandleDash()
        {
            if (input.dashPressed && dashTimer <= Time.time && dashRecoveryTimer <= Time.time)
            {
                dashTimer = Time.time + dashTime;
                dashRecoveryTimer = Time.time + dashRecoveryTime;
                player.character.PlayAnimation("Dash", 1, true);
                isDashing = true;
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effDash);
                dashFx.Play();
            }
            if (Time.time < dashTimer)
            {
                core.movement.SetVelocityX(dashForce * core.movement.facingDirection);
            }
            else
            {
                dashFx.Stop();
                isDashing = false;
            }
        }
        public void Heal(float percentValue)
        {
            float hp = stats.health;
            stats.SetCurrentHealth(hp);
            healthBar.UpdateFillBar(stats.normalizedHealth);
        }
        RaycastHit2D RayCast(Vector2 offset, Vector2 direction, float distance, LayerMask layerMask)
        {
            RaycastHit2D raycast = Physics2D.Raycast((Vector2) transform.position + offset, Vector2.down, distance, layerMask);
#if UNITY_EDITOR
            Color color = raycast ? Color.red : Color.green;
            Debug.DrawRay((Vector2)transform.position + offset, direction * distance, color);
#endif
            return raycast;
        }
    }
}
