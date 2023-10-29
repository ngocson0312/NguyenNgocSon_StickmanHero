using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFight
{
    public class ItemElectricDoor : MonoBehaviour
    {
        [SerializeField] Electric electric;
        [SerializeField] int damage = 50;
        [SerializeField] float timeDuration = 2.5f;
        [SerializeField] Animation animation;
        [SerializeField] Transform TopAnchor;
        [SerializeField] Transform BotAnchor;
        [SerializeField] Transform ElectricAnchor;
        float currentTimeWait;
        bool isOpen;

        private void Start()
        {
            currentTimeWait = timeDuration;
            electric.SetDamage(damage);
            
        }
        
        private void Update()
        {
            OpenDoor();
        }
        void OpenDoor()
        {
            if (isOpen)
            {
                if (currentTimeWait > 0)
                {
                    currentTimeWait -= Time.fixedDeltaTime;
                    if (currentTimeWait <= 0)
                    {
                        isOpen = false;
                        animation.Play("ElectricDoor_Close");
                        currentTimeWait = timeDuration;
                    }
                }
            }
            else
            {
                if (currentTimeWait > 0)
                {
                    currentTimeWait -= Time.fixedDeltaTime;
                    if (currentTimeWait <= 0)
                    {
                        isOpen = true;
                        animation.Play("ElectricDoor_Open");
                        currentTimeWait = timeDuration;
                    }
                }
            }
        }

        void SetupDoor()
        {

        }
    }
}

