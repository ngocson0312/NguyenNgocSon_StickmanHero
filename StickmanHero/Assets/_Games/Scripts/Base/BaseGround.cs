using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperFight
{
    public class BaseGround : MonoBehaviour
    {
        [SerializeField] private TYPE_GROUND type;
        public TYPE_GROUND TypeGround => type;
    }
}