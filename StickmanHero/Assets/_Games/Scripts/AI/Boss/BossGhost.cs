using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace SuperFight
{
    public class BossGhost : Boss
    {
        public ParticleSystem deathFX;
        public Transform[] posMove;
        public Renderer renderObj;
        int indexPos;
        [Header("Roaming Stats")]
        public float flySpeed = 10f;
        public float roamingTime = 2f;
        float roamingTimer;
        [Header("Attack Stats")]
        public FireBall fireBall;
        public float attackRate;
        float attackTimer;
        int attackCount;
        int maxAttackInTurn;
        PlayerManager player;
        [Header("Stars")]
        public Transform[] posStars;
        public StarKick starPrefab;
        float starSpawnTimer;
        int currentPos;

        public override void Initialize(BossFightArena bossFightArena)
        {
            base.Initialize(bossFightArena);
            roamingTimer = roamingTime;
            attackTimer = attackRate;
            player = PlayerManager.Instance;
            maxAttackInTurn = Random.Range(5, 7);
            starSpawnTimer = 2f;
        }

        void HandleStarItem()
        {
            if (starSpawnTimer > 0)
            {
                starSpawnTimer -= Time.deltaTime;
                if (starSpawnTimer <= 0)
                {
                    starSpawnTimer = Random.Range(3f, 5f);
                    StarKick s = Instantiate(starPrefab, arena.transform);
                    s.transform.position = posStars[currentPos].position;
                    s.Initialize();
                    currentPos++;
                    if (currentPos >= posStars.Length)
                    {
                        currentPos = 0;
                    }
                }
            }
        }

        void HandleRoaming()
        {
            if (roamingTimer > 0)
            {
                roamingTimer -= Time.deltaTime;
                if (roamingTimer <= 0)
                {
                    roamingTimer = roamingTime;
                    SwitchPos();
                }
            }
            if (Vector2.Distance(transform.position, posMove[indexPos].position) > 1f)
            {
                Vector3 direction = (posMove[indexPos].position - transform.position).normalized;
                transform.position += direction * Time.deltaTime * flySpeed;
                if (direction.x < 0)
                {
                    animatorHandle.transform.rotation = Quaternion.Lerp(animatorHandle.transform.rotation, Quaternion.Euler(0, 240, 0), 0.2f);
                }
                else if (direction.x > 0)
                {
                    animatorHandle.transform.rotation = Quaternion.Lerp(animatorHandle.transform.rotation, Quaternion.Euler(0, 110, 0), 0.2f);
                }
            }
            else
            {
                HandleAttack();
            }
        }
        void HandleAttack()
        {
            if (attackCount == maxAttackInTurn) return;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            if (direction.x < 0)
            {
                animatorHandle.transform.rotation = Quaternion.Lerp(animatorHandle.transform.rotation, Quaternion.Euler(0, 240, 0), 0.2f);
            }
            else if (direction.x > 0)
            {
                animatorHandle.transform.rotation = Quaternion.Lerp(animatorHandle.transform.rotation, Quaternion.Euler(0, 110, 0), 0.2f);
            }
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    ThrowFireBall();
                    attackTimer = attackRate;
                }
            }
        }
        void ThrowFireBall()
        {
            attackCount++;
            animatorHandle.PlayAnimation("Attack", 0.1f, 1, true);
            Vector2 direction = (player.transform.position - transform.position).normalized;
            FireBall f = PathologicalGames.PoolManager.Pools["Projectile"].Spawn(fireBall.transform).GetComponent<FireBall>();
            f.transform.position = transform.position;
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = stats.damage;
            damageInfo.idSender = core.combat.getColliderInstanceID;
            f.Initialize(direction, damageInfo);
        }
        void SwitchPos()
        {
            maxAttackInTurn = Random.Range(5, 8);
            attackCount = 0;
            indexPos++;
            if (indexPos >= posMove.Length)
            {
                indexPos = 0;
            }
        }
        public override void OnTakeDamage(DamageInfo damageInfo)
        {
            if (!isActive) return;
            base.OnTakeDamage(damageInfo);
            // base.takeDamageFX.Play();
            if (stats.currHealth <= 0)
            {
                isActive = false;
                animatorHandle.PlayAnimation("Die", 0.1f, 1, true);
                Die(true);
            }
            else
            {
                animatorHandle.PlayAnimation("Hit", 0.1f, 1, true);
                SoundManager.Instance.playRandFx(TYPE_RAND_FX.FX_TAKE_DAMAGE);
            }
            renderObj.material.DOColor(Color.red, 0.2f).OnComplete(() =>
            {
                renderObj.material.color = Color.white;
            });
        }
        public override void Die(bool deactiveCharacter)
        {
            deathFX.transform.parent = transform.parent;
            deathFX.gameObject.SetActive(true);
            deathFX.Play();
            GameplayCtrl.Instance.CreateCoinBoss(transform.position + new Vector3(0, 0, -1));
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effBossDie);
            base.Die(deactiveCharacter);
        }

        public override void Active()
        {
            isActive = true;
        }

        protected override void UpdateLogic()
        {
            if (!isActive) return;
            HandleRoaming();
            HandleStarItem();
        }
        public override void Resume()
        {
            base.Resume();
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
        protected override void UpdatePhysic()
        {
        }
    }
}

