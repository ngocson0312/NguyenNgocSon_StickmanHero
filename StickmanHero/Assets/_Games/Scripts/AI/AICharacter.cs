// using System.Collections;
// using System.Collections.Generic;
// using mygame.sdk;
// using UnityEngine;


// namespace SuperFight
// {
//     public class AICharacter : MonoBehaviour, IDamage
//     {
//         [Header("Components")]
//         [SerializeField] private ParticleSystem effTakeDamage;
//         public TYPE_ENEMY TypeEnemy { protected set; get; }
//         public Renderer renderChar;
//         public Rigidbody2D rigidBody2D { get; protected set; }
//         public EnemyAnimationHandle character;
//         public CharacterStats baseStats;
//         public CharacterStats myStats { get; private set; }

//         [Header("Patrol Stats")]
//         public LayerMask groundLayer;
//         public Vector2 offsetRightRay = Vector2.zero;
//         public Vector2 offsetDownRay = new Vector2(0.5f, 0f);
//         //public float maxPatrolDistance = 10f;
//         public float movementSpeed = 3f;
//         public float maxFallSpeed = -20;
//         public Vector3 startPosition;
//         [Header("FOV Check")]
//         public Vector2 offsetFOV;
//         public float xViewDistance = 5f;
//         public float yViewDistance = 5f;
//         [Header("Attack Stats")]
//         public Vector2 offsetRange;
//         public LayerMask targetLayer;
//         public float attackSpeed = 1f;
//         public float attackRange = 3f;
//         public float angleDamageable = 90;
//         public SpriteBar healthBar;
//         public Vector2 offsetHealthBar = new Vector2(0f, 2.5f);
//         [Header("Flags Status")]
//         public bool isOnGround;
//         public bool isTouchingWall;
//         public bool hasGroundAhead;
//         public bool isUnstopable;
//         public bool isInteracting;
//         public bool isDead;
//         public int facingDirection { get; protected set; }//-1 left, 1 right
//         [SerializeField] private State firstState;
//         private State currentState;
//         Vector2 currentVelocity;
//         bool isInit = false;
//         protected float delayTimePlaySoundFx;
//         public bool isTest;

//         public virtual void Initialize()
//         {
//             if (isInit) return;
//             isInit = true;
//             startPosition = transform.position;
//             healthBar = Instantiate(healthBar, transform);
//             healthBar.transform.localPosition = offsetHealthBar;
//             healthBar.Deactive();
//             SwitchState(firstState);
//             rigidBody2D = GetComponent<Rigidbody2D>();
//             character.Initialize(this);
//         }
//         void Update()
//         {
//             if (GameplayCtrl.Instance.getStateGame() == GAME_STATE.GS_PAUSEGAME || GameplayCtrl.Instance.getStateGame() == GAME_STATE.GS_WAIT_REVIVE || GameplayCtrl.Instance.getStateGame() == GAME_STATE.GS_GAMEOVER) return;
//             isInteracting = character.animator.GetBool("IsInteracting");
//             if (currentState != null)
//             {
//                 // currentState.UpdateState();
//                 if (delayTimePlaySoundFx > 0)
//                 {
//                     delayTimePlaySoundFx -= Time.deltaTime;
//                 }
//             }
//         }
//         private void FixedUpdate()
//         {
//             if (rigidBody2D.velocity.y <= maxFallSpeed)
//             {
//                 rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, maxFallSpeed);
//             }
//             if (isDead)
//             {
//                 CheckOutView();
//                 return;
//             }
//             if (GameplayCtrl.Instance.getStateGame() == GAME_STATE.GS_PAUSEGAME || GameplayCtrl.Instance.getStateGame() == GAME_STATE.GS_WAIT_REVIVE || GameplayCtrl.Instance.getStateGame() == GAME_STATE.GS_GAMEOVER) return;
//             PhysicCheck();
//             if (currentState != null)
//             {
//                 // currentState.UpdatePhysics();
//             }
//         }
//         protected void PhysicCheck()
//         {
//             Vector2 offsetDown = new Vector2(offsetDownRay.x * facingDirection, offsetDownRay.y);
//             isOnGround = RayCast(Vector2.up, Vector2.down, 1.5f, groundLayer);
//             isTouchingWall = RayCast(new Vector2(offsetRightRay.x * facingDirection, offsetRightRay.y), Vector2.right * facingDirection, 1f, groundLayer);
//             hasGroundAhead = RayCast(offsetDown, Vector2.down, 1f, groundLayer);
//         }
//         public void SwitchState(State newState)
//         {
//             if (currentState != null)
//             {
//                 currentState.ExitState();
//             }
//             currentState = newState;
//             // currentState.EnterState(this);
//         }
//         public virtual void Attack()
//         {
//             character.PlayAnimation("Attack", 1, true);
//         }
//         public RaycastHit2D RayCast(Vector2 offset, Vector2 direction, float distance, LayerMask layerMask)
//         {
// #if UNITY_EDITOR
//             RaycastHit2D raycast = Physics2D.Raycast((Vector2)transform.position + offset, direction, distance, layerMask);
//             Color color = raycast ? Color.red : Color.green;
//             Debug.DrawRay((Vector2)transform.position + offset, direction * distance, color);    
//             return raycast;
// #else
//             return Physics2D.Raycast((Vector2)transform.position + offset, direction, distance, layerMask);
// #endif

//         }
//         public void SetFacingDirection(int newDirection)
//         {
//             facingDirection = newDirection;
//         }
//         public void TakeDamage(DamageInfo damageInfo)
//         {
//             if (isDead) return;
//             myStats.currHealth -= damageInfo.damage;
//             UltimateTextDamageManager.Instance.Add(damageInfo.damage.ToString(), transform.position + Vector3.up * 2);
//             effTakeDamage.Play();
//             // healthBar.UpdateBar(myStats.currHealth / myStats.maxHealth);
//             if (myStats.currHealth <= 0)
//             {
//                 character.PlayAnimation("Dead", 1, false);
//                 Die(damageInfo.hitDirection);
//             }
//             else
//             {
//                 SoundManager.Instance.playRandFx(TYPE_RAND_FX.FX_TAKE_DAMAGE);
//                 character.PlayAnimation("Stun", 1, true);
//             }
//         }

//         public virtual void ResetStatEnemy(CharacterStats overrideStats)
//         {
//             isDead = false;
//             startPosition = transform.position;
//             if (overrideStats == null)
//             {
//                 myStats = baseStats;
//                 myStats.ResetStats();
//             }
//             else
//             {
//                 // myStats = new CharacterStats((int)overrideStats.maxHealth, overrideStats.damage);
//                 myStats.ResetStats();
//             }
//             if (GameManager.Instance.CurrLevel > GameManager.Instance.maxLevel)
//             {
//                 int health = DataManager.Instance.playerDamage * Random.Range(3, 8);
//                 int damage = (int)(DataManager.Instance.playerMaxHp / Random.Range(5, 10));
//                 myStats = new CharacterStats(health, damage);
//                 myStats.ResetStats();
//             }

//             SwitchState(firstState);
//             GetComponent<Collider2D>().enabled = true;
//             character.animator.speed = 1;
//         }

//         public virtual void Die(int _direction)
//         {
//             if (isDead) return;
//             isDead = true;
//             healthBar.Deactive();
//             Vector2 direction = new Vector2(0.5f * _direction, 1f);
//             rigidBody2D.AddForce(direction * 20, ForceMode2D.Impulse);
//             GetComponent<Collider2D>().enabled = false;
//             // GameplayCtrl.Instance.enemyBeKill(this);
//         }

//         public virtual void CheckOutView()
//         {
//             // if (isDead && !renderChar.isVisible) GameplayCtrl.Instance.freeEnemyBeKill(this);
//         }

//         public virtual void SendDamage()
//         {
//             Collider2D[] colls = new Collider2D[3];
//             Physics2D.OverlapCircleNonAlloc(transform.position, attackRange, colls, targetLayer);
//             DamageInfo damageInfo = new DamageInfo();
//             Vector2 direction;
//             damageInfo.damage = myStats.damage;
//             for (int i = 0; i < colls.Length; i++)
//             {
//                 if (colls[i] == null) continue;
//                 direction = (colls[i].transform.position - transform.position).normalized;
//                 float targetAngle = Vector3.Angle(direction, Vector2.right * facingDirection);
//                 if (targetAngle <= angleDamageable)
//                 {
//                     colls[i].GetComponent<IDamage>()?.TakeDamage(damageInfo);
//                 }
//             }
//         }
//         public Bounds GetViewBound()
//         {
//             return new Bounds((Vector2)transform.position + new Vector2(offsetFOV.x * facingDirection, offsetFOV.y), new Vector2(xViewDistance, yViewDistance));
//         }
//         private void OnDrawGizmos()
//         {
//             // Draw FOV
//             Bounds bound = GetViewBound();
//             if (facingDirection == 0)
//             {
//                 bound.center = (Vector2)transform.position + new Vector2(offsetFOV.x, offsetFOV.y);
//             }
//             Gizmos.DrawWireCube(bound.center, bound.size);
//             //Draw Attack
//             Gizmos.color = Color.black;
//             Gizmos.DrawWireSphere((Vector2)transform.position + offsetRange, attackRange);

//             Gizmos.color = Color.blue;
//             //Gizmos.DrawWireSphere(startPosition, maxPatrolDistance);
//         }

//         public void OnPauseGame()
//         {
//             currentVelocity = rigidBody2D.velocity;
//             rigidBody2D.velocity = Vector2.zero;
//             character.animator.speed = 0;
//         }

//         public void OnResumeGame()
//         {
//             rigidBody2D.velocity = currentVelocity;
//             character.animator.speed = 1;
//         }

//         public virtual void DetectMainPlayer()
//         {
//         }
//     }
// }

