using UnityEngine;
namespace SuperFight
{
    public abstract class Weapon : MonoBehaviour
    {
        public HAND curHand;
        [Range(-100, 100)]
        public int damage;
        public MoveSet[] moveSets;
        public bool keyWeapon;
        public bool hasFx = true;
        public float attackRange;
        public LayerMask layerContact;
        public int sortingLayer = 1;
        public abstract void TriggerSkill(Controller controller, int indexSkill);
    }
    [System.Serializable]
    public class MoveSet
    {
        public string animationName;
        public float stunTime;
        public AudioClip activeSound;
        public AudioClip impactSound;
    }

    public enum HAND
    {
        LEFT,
        RIGHT,
        BOTH
    }
}

