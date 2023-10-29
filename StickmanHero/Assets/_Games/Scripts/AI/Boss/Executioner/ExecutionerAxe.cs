using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class ExecutionerAxe : MonoBehaviour
    {
        private DamageInfo damageInfo;
        public void CatchAxe()
        {
            gameObject.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
        }
        public void ThrowAxe(DamageInfo damageInfo)
        {
            gameObject.SetActive(true);
            GetComponent<Collider2D>().enabled = true;
            this.damageInfo = damageInfo;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetInstanceID() != damageInfo.idSender)
            {
                other.GetComponent<IDamage>()?.TakeDamage(damageInfo);
            }
        }
    }

}
