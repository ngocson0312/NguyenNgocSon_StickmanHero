using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFight
{
    public class ItemPlane : MonoBehaviour
    {
        [SerializeField] Transform StartAnchor;
        [SerializeField] Transform EndAnchor;
        [SerializeField] Transform PlaneAnchor;
        [SerializeField] LineRenderer linePath;
        [SerializeField] float speed = 2f;
        [SerializeField] float timeWait = 1f;
        float currentTimeWait;
        bool moveUp;
        private void Start()
        {
            currentTimeWait = timeWait;
        }

        private void OnEnable()
        {
            linePath.positionCount = 2;
            linePath.SetPosition(0, StartAnchor.position + new Vector3(0,0.25f,0));
            linePath.SetPosition(1, EndAnchor.position + new Vector3(0, 0.25f, 0));
        }
        private void Update()
        {
            PlaneMoving();
        }
        void PlaneMoving()
        {
            if (moveUp)
            {
                PlaneAnchor.position = Vector3.MoveTowards(PlaneAnchor.position, EndAnchor.position, speed * Time.deltaTime);
                if (PlaneAnchor.position == EndAnchor.position)
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
                PlaneAnchor.position = Vector3.MoveTowards(PlaneAnchor.position, StartAnchor.position, speed * Time.deltaTime);
                if (PlaneAnchor.position == StartAnchor.position)
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

