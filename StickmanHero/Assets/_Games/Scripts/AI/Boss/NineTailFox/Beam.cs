using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class Beam : MonoBehaviour
    {
        public ParticleSystem beamStart;
        public LineRenderer line;
        public ParticleSystem beamEnd;
        public float lifeTime = 2f;
        public float rayLength = 100f;
        public LayerMask layerContact;
        public LayerMask layerBlocked;
        private Vector3 direction;
        private DamageInfo damageInfo;
        private bool isActive;
        List<Collider2D> listColl;
        public void Prepare()
        {
            beamStart.gameObject.SetActive(true);
            gameObject.SetActive(true);
            line.gameObject.SetActive(false);
            beamStart.Play();
            beamEnd.Stop();
        }
        public void ActiveBeam(Vector3 direction, DamageInfo damageInfo)
        {
            this.damageInfo = damageInfo;
            this.direction = direction;
            damageInfo.stunTime = 0.6f;
            damageInfo.hitDirection = (int)direction.x;
            listColl = new List<Collider2D>();
            isActive = true;
            gameObject.SetActive(true);
            beamStart.Play();
            beamEnd.Play();
            line.gameObject.SetActive(true);
        }
        private void Update()
        {
            if (!isActive) return;
            // lifeTime -= Time.deltaTime;
            // if (lifeTime <= 0)
            // {
            //     isActive = false;
            //     gameObject.SetActive(false);
            // }
            line.SetPosition(0, transform.position);
            float maxLength = rayLength;
            RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, rayLength, layerBlocked);
            if (ray.transform != null)
            {
                line.SetPosition(1, ray.point);
                beamEnd.transform.position = ray.point;
                maxLength = Vector2.Distance(transform.position, ray.point);
            }
            line.sharedMaterial.mainTextureScale = new Vector2(maxLength / 8, 1);
            line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * 8, 0);
            RaycastHit2D[] rays = Physics2D.RaycastAll(transform.position, direction, maxLength, layerContact);
            for (int i = 0; i < rays.Length; i++)
            {
                if (!listColl.Contains(rays[i].collider))
                {
                    rays[i].collider.GetComponent<IDamage>()?.TakeDamage(damageInfo);
                    listColl.Add(rays[i].collider);
                }
            }
        }
        public void Deactive()
        {
            isActive = false;
            beamEnd.Stop();
            gameObject.SetActive(false);
        }
    }
}

