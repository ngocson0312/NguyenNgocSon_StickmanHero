using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFight
{

    public class ItemElectricDoorMove : MonoBehaviour
    {
        [SerializeField] Transform StartAnchor;
        [SerializeField] Transform EndAnchor;
        [SerializeField] Transform DoorAnchor;
        [SerializeField] float speed = 2f;
        [SerializeField] float timeWait = 1f;
        [SerializeField] int damage = 50;
        [SerializeField] Electric electric;
        float currentTimeWait;
        bool moveUp;
        private void Start()
        {
            currentTimeWait = timeWait;
            electric.SetDamage(damage);
        }
        private void Update()
        {
            PlaneMoving();
        }
        void PlaneMoving()
        {
            if (moveUp)
            {
                DoorAnchor.position = Vector3.MoveTowards(DoorAnchor.position, EndAnchor.position, speed * Time.deltaTime);
                if (DoorAnchor.position == EndAnchor.position)
                {
                    if (currentTimeWait > 0)
                    {
                        currentTimeWait -= Time.fixedDeltaTime;
                        if (currentTimeWait <= 0)
                        {
                            moveUp = false;
                            currentTimeWait = timeWait;
                        }
                    }
                }
            }
            else
            {
                DoorAnchor.position = Vector3.MoveTowards(DoorAnchor.position, StartAnchor.position, speed * Time.deltaTime);
                if (DoorAnchor.position == StartAnchor.position)
                {
                    if (currentTimeWait > 0)
                    {
                        currentTimeWait -= Time.fixedDeltaTime;
                        if (currentTimeWait <= 0)
                        {
                            moveUp = true;
                            currentTimeWait = timeWait;
                        }
                    }
                }
            }
        }
        
    }
}