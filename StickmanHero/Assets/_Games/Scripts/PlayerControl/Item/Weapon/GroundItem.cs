using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    [RequireComponent(typeof(Collider2D))]
    public class GroundItem : MonoBehaviour
    {
        public Weapon itemObject;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>() != null)
            {
                other.GetComponent<PlayerManager>().character.LoadWeapon(itemObject);
                gameObject.SetActive(false);
            }
        }
    }
}

