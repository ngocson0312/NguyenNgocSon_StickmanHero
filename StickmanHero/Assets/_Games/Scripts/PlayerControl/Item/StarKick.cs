using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class StarKick : MonoBehaviour
    {
        public LayerMask target;
        public LayerMask layerContact;
        public float speed;
        public float range;
        Rigidbody2D rb2d;
        Collider2D coll;
        [SerializeField] GameObject blockCollider;
        [SerializeField] GameObject trail;
        [SerializeField] GameObject impact;
        bool active = false;
        float timeMoveUp;
        Transform targetP;
        public ParticleSystem greenFx;
        public void Initialize()
        {
            rb2d = GetComponent<Rigidbody2D>();
            coll = GetComponent<Collider2D>();
            trail.SetActive(true);
            impact.SetActive(false);
            timeMoveUp = 0.2f;
        }
        private void Update()
        {
            if (!active) return;
            HandleMove();
        }
        void HandleMove()
        {
            timeMoveUp -= Time.deltaTime;
            trail.transform.Rotate(0, 0, 20);
            if (timeMoveUp > 0)
            {
                transform.position += Vector3.up * (speed / 3) * Time.deltaTime;
            }
            else
            {
                var colls = new Collider2D[3];
                Physics2D.OverlapCircleNonAlloc(transform.position, range, colls, target);
                List<Collider2D> cs = new List<Collider2D>();
                for (int i = 0; i < colls.Length; i++)
                {
                    if(colls[i] == null) continue;
                    if (colls[i].GetInstanceID() != PlayerManager.Instance.playerController.core.combat.getColliderInstanceID)
                    {
                        cs.Add(colls[i]);
                    }
                }
                if (cs.Count > 0 && cs[0] != null && targetP == null)
                {
                    targetP = cs[0].transform;
                    if (targetP.Find("Target") != null)
                    {
                        targetP = targetP.Find("Target");
                    }
                }
                if (targetP != null)
                {
                    Vector3 direction = targetP.position - transform.position;
                    direction.Normalize();
                    transform.position += direction * speed * Time.deltaTime;
                }
                else
                {
                    transform.position += Vector3.up * speed * Time.deltaTime;
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>() != null)
            {
                greenFx.Stop();
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effStarKick);
                Active();
            }
            else
            {
                if ((layerContact & (1 << other.gameObject.layer)) == 0) return;
                if (!active || other.gameObject.layer == gameObject.layer || PlayerManager.Instance.playerController.core.combat.IsSelfCollider(other)) return;
                Explosion(other.transform);
            }
        }
        public void Explosion(Transform other)
        {
            active = false;
            trail.SetActive(false);
            impact.SetActive(true);
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = PlayerManager.Instance.playerController.stats.damage;
            damageInfo.characterType = CharacterType.Character;
            coll.enabled = false;
            Debug.Log(damageInfo.damage);
            other.GetComponent<IDamage>()?.TakeDamage(damageInfo);
            Destroy(gameObject, 1f);
        }
        public void Active()
        {
            blockCollider.SetActive(false);
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            active = true;
        }
    }
}

