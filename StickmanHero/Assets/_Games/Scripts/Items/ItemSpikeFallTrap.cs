using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFight
{
    public class ItemSpikeFallTrap : MonoBehaviour
    {
        [SerializeField] GameObject SprSpike;
        [SerializeField] ParticleSystem FXBreak;
        [SerializeField] float distance;
        [SerializeField] LayerMask layerMask;
        [SerializeField] Transform EndAnchor;
        [SerializeField] int dam = 50;
        bool isBreak = false;
        bool isActive = false;

        public bool isLevelBoss = false;

        private void Start()
        {
            distance = transform.position.y - EndAnchor.position.y;
            if(distance <= 7.5)
            {
                GetComponent<Rigidbody2D>().gravityScale = 1;
            }
            isActive = false;
        }
        private void Update()
        {
            if (isLevelBoss) return;
            if (!isBreak)
            {
                RaycastHit2D raycast = Physics2D.Raycast(transform.position, Vector2.down, distance, layerMask);
                if (raycast.collider != null)
                {
                    if (raycast.collider.gameObject.layer == LayerMask.NameToLayer(Constant.layerMainPlayer))
                    {
                        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        isActive = true;
                    }
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!isActive) return;
            SpikeBreak();
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = dam;
            col.GetComponent<IDamage>()?.TakeDamage(damageInfo);
        }

        public void Active()
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            isActive = true;
        }

        void SpikeBreak()
        {
            isBreak = true;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            FXBreak.Play();
            SprSpike.SetActive(false);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            distance = transform.position.y - EndAnchor.position.y;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down*Mathf.Abs(distance));
        }
    }
}

