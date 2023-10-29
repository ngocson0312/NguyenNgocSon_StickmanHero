using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class BossDragon : Boss
    {
        public int state;
        [Header("Roaming")]
        public Transform[] posMove;
        public int direction = -1;
        public float speed = 10f;
        private int indexPos;
        public float roamingTime = 5f;
        private float roamingTimer;
        private int roamingCount;
        [Header("Flame")]
        public float rayLenght = 20f;
        public ParticleSystem flame;
        public Transform flamePosition;
        public LayerMask targetLayer;
        [Header("SpanwEnemy")]
        public ParticleSystem[] spawnPos;
        // public AICharacter[] prefabEnemys;
        int turnCount;
        float spawnTimer;
        [Header("Stars")]
        public Transform[] posStars;
        public StarKick starPrefab;
        float starSpawnTimer;
        int currentPos;
        public ParticleSystem explosionFX;
        List<Collider2D> collider2Ds;
        public override void Initialize(BossFightArena bossFightArena)
        {
            base.Initialize(bossFightArena);
            collider2Ds = new List<Collider2D>();
            state = 0;
            roamingTimer = roamingTime;
            roamingCount = Random.Range(2, 3);
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
        void HandleFlyAround()
        {
            if (roamingTimer > 0)
            {
                roamingTimer -= Time.deltaTime;
                if (roamingTimer <= 0)
                {
                    indexPos++;
                    if (indexPos >= posMove.Length)
                    {
                        indexPos = 0;
                    }
                    roamingTimer = roamingTime;
                    roamingCount--;
                    collider2Ds = new List<Collider2D>();
                    if (roamingCount <= 0)
                    {
                        state = 1;
                        spawnTimer = 1f;
                        turnCount = Random.Range(1, 2);
                        flame.Stop();
                    }
                }
            }
            if (Vector2.Distance(transform.position, posMove[indexPos].position) > 0.5f)
            {
                Vector3 dir = posMove[indexPos].position - transform.position;
                transform.position += dir.normalized * speed * Time.deltaTime;
                if (dir.x < 0)
                {
                    direction = -1;
                    animatorHandle.transform.rotation = Quaternion.Lerp(animatorHandle.transform.rotation, Quaternion.Euler(0, 240, 0), 0.2f);
                }
                else if (dir.x > 0)
                {
                    direction = 1;
                    animatorHandle.transform.rotation = Quaternion.Lerp(animatorHandle.transform.rotation, Quaternion.Euler(0, 110, 0), 0.2f);
                }
                HandleBreathFlame();
                flame.Play();
                animatorHandle.SetFloat("MoveAmount", 1f);
            }
            else
            {
                if (direction == -1)
                {
                    animatorHandle.transform.rotation = Quaternion.Lerp(animatorHandle.transform.rotation, Quaternion.Euler(0, 110, 0), 0.2f);
                }
                else
                {
                    animatorHandle.transform.rotation = Quaternion.Lerp(animatorHandle.transform.rotation, Quaternion.Euler(0, 240, 0), 0.2f);
                }
                flame.Stop();
                animatorHandle.SetFloat("MoveAmount", 0f);
            }
        }
        void HandleBreathFlame()
        {
            RaycastHit2D[] r = Physics2D.RaycastAll(flamePosition.position, flamePosition.forward, rayLenght, targetLayer);
            List<Collider2D> targets = new List<Collider2D>();
            for (int i = 0; i < r.Length; i++)
            {
                if (!core.combat.IsSelfCollider(r[i].collider))
                {
                    targets.Add(r[i].collider);
                }
            }
            if (targets.Count > 0 && !collider2Ds.Contains(targets[0]))
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = stats.damage;
                damageInfo.isKnockBack = true;
                damageInfo.hitDirection = direction;
                damageInfo.stunTime = 1f;
                targets[0].transform.GetComponent<IDamage>()?.TakeDamage(damageInfo);
                collider2Ds.Add(targets[0]);
            }
        }

        void SpawnEnemyState()
        {
            if (spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;
                if (spawnTimer <= 0)
                {
                    HandleSpawn();
                    spawnTimer = 1f;
                }
            }
        }

        void HandleSpawn()
        {
            // int indexEnemy = Random.Range(0, prefabEnemys.Length);
            // for (int i = 0; i < 0; i++)
            // {
            //     spawnPos[i].Play();
            //     AICharacter c = Instantiate(prefabEnemys[indexEnemy]);
            //     c.transform.position = spawnPos[i].transform.position;
            //     c.transform.parent = arena.transform;
            //     c.Initialize();
            //     c.ResetStatEnemy(new CharacterStats(50, 10));

            // }
            turnCount--;
            if (turnCount == 0)
            {
                roamingCount = Random.Range(2, 3);
                roamingTimer = 10f;
                state = 0;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(flamePosition.position, flamePosition.forward * rayLenght);
        }
        public override void Die(bool deactiveCharacter)
        {
            base.Die(deactiveCharacter);
            explosionFX.Play();
            isActive = false;
            animatorHandle.PlayAnimation("Die", 0.1f, 1, true);
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effBossDie);
            GameplayCtrl.Instance.CreateCoinOnKillBoss(transform.position, 20, 40);
        }
        public override void Resume()
        {
            base.Resume();
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
        public override void OnTakeDamage(DamageInfo damageInfo)
        {
            if (!isActive) return;
            base.OnTakeDamage(damageInfo);
            if (stats.currHealth <= 0)
            {
                Die(true);
            }
            else
            {
                SoundManager.Instance.playRandFx(TYPE_RAND_FX.FX_TAKE_DAMAGE);
            }
        }
        public override void Active()
        {
            isActive = true;
        }

        protected override void UpdateLogic()
        {
            if (!isActive) return;
            if (state == 0)
            {
                HandleFlyAround();
            }
            else
            {
                SpawnEnemyState();
            }
            HandleStarItem();
        }

        protected override void UpdatePhysic()
        {

        }
    }
}
