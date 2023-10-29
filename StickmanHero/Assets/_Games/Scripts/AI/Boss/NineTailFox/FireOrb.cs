using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PathologicalGames;
namespace SuperFight
{
    public class FireOrb : MonoBehaviour
    {
        DamageInfo damageInfo;
        [SerializeField] Transform prefabFireBall;
        // private void Start()
        // {
        //     Active(1, new DamageInfo());
        // }
        public void Active(int facingDirection, DamageInfo damageInfo)
        {
            gameObject.SetActive(true);
            this.damageInfo = damageInfo;
            transform.DOMoveX(transform.position.x + 3 * facingDirection, 1f).SetEase(Ease.Linear);
            transform.DOMoveY(transform.position.y + 5, 1f).SetEase(Ease.Linear);
            StartCoroutine(StartThrow());
        }
        IEnumerator StartThrow()
        {
            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < Random.Range(7, 10); i++)
            {
                Projectile p = PoolManager.Pools["Projectile"].Spawn(prefabFireBall).GetComponent<Projectile>();
                float angle = Random.Range(-150, -40);
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                p.transform.position = transform.position;
                p.Initialize(direction, damageInfo);
                yield return new WaitForSeconds(0.5f);
            }
            gameObject.SetActive(false);
        }
    }
}

