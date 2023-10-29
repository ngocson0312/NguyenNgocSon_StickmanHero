using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public abstract class ItemObject : ScriptableObject
    {
        public string itemName;
        public ItemType itemType;
    }
    public enum ItemType
    {
        CONSUMABLE, WEAPON, EQUIPMENT
    }
}

