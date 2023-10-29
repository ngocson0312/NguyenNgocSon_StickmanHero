using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperFight
{
    public class BaseItem : MonoBehaviour
    {
        [SerializeField] private TYPE_ITEM type;
        public TYPE_ITEM TypeItem => type;
    }
}