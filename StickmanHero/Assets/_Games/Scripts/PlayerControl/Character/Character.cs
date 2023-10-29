using Spine.Unity;
using UnityEngine;
using System.Collections;

namespace SuperFight
{
    public class Character : AnimatorHandle
    {
        public SkeletonMecanim skeletonMecanim { get; private set; }
        // protected PlayerManager player;
        public Weapon defaultWeapon;
        public Weapon currentWeapon;
        public GameObject weaponHolderLeft;
        public GameObject weaponHolderRight;
        public MoveSet currentMoveSet;
        public int currentIndexMoveSet;
        public override void Initialize(Controller controller)
        {
            base.Initialize(controller);
            skeletonMecanim = GetComponent<SkeletonMecanim>();
            animator.speed = 1;
            currentIndexMoveSet = 0;
            LoadWeapon(defaultWeapon);
        }
        public void LoadSkin(string skinName)
        {
            skeletonMecanim.skeleton.SetSkin(skinName);
            skeletonMecanim.skeleton.SetBonesToSetupPose();
            skeletonMecanim.skeleton.SetToSetupPose();
        }

        public virtual void HandleCombo()
        {
            bool canDoCombo = animator.GetBool("CandoCombo");
            if (canDoCombo)
            {
                currentIndexMoveSet++;
                if (currentIndexMoveSet >= currentWeapon.moveSets.Length)
                {
                    currentIndexMoveSet = 0;
                }
                currentMoveSet = currentWeapon.moveSets[currentIndexMoveSet];
                AudioManager.Instance.PlayOneShot(currentMoveSet.activeSound, 1f);
                PlayAnimation(currentMoveSet.animationName, 0.1f, 1, true);
                DeactiveCombo();
            }
            else
            {
                if (!controller.isInteracting && !canDoCombo)
                {
                    currentIndexMoveSet = 0;
                    currentMoveSet = currentWeapon.moveSets[currentIndexMoveSet];
                    AudioManager.Instance.PlayOneShot(currentMoveSet.activeSound, 1f);
                    PlayAnimation(currentMoveSet.animationName, 0.1f, 1, true);
                }
            }
        }

        public void LoadWeapon(Weapon weapon, bool overrideKeyWeapon = false)
        {
            if (currentWeapon != null && currentWeapon.keyWeapon && !overrideKeyWeapon) return;
            if (weapon == null)
            {
                weapon = defaultWeapon;
            }

            int childCount = weaponHolderLeft.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Destroy(weaponHolderLeft.transform.GetChild(i).gameObject);
            }

            int childCount2 = weaponHolderRight.transform.childCount;
            for (int i = childCount2 - 1; i >= 0; i--)
            {
                Destroy(weaponHolderRight.transform.GetChild(i).gameObject);
            }

            if (weapon.curHand == HAND.LEFT)
            {
                if (weapon != null)
                {
                    currentWeapon = Instantiate(weapon, weaponHolderLeft.transform);
                    currentWeapon.transform.localPosition = Vector3.zero;
                    PlayerManager.Instance.PlayFXEquipWeapon();
                }
            }
            else if (weapon.curHand == HAND.RIGHT)
            {
                if (weapon != null)
                {
                    currentWeapon = Instantiate(weapon, weaponHolderRight.transform);
                    currentWeapon.transform.localPosition = Vector3.zero;
                    PlayerManager.Instance.PlayFXEquipWeapon();
                }
            }
            else
            {
                if (weapon != null)
                {
                    //left
                    currentWeapon = Instantiate(weapon, weaponHolderLeft.transform);
                    currentWeapon.transform.localPosition = Vector3.zero;
                    //right
                    currentWeapon = Instantiate(weapon, weaponHolderRight.transform);
                    currentWeapon.transform.localPosition = Vector3.zero;
                    PlayerManager.Instance.PlayFXEquipWeapon();
                }
            }
        }

        public void PlayAnimation(string stateName, int layer, bool isInteracting)
        {
            animator.CrossFade(stateName, 0.1f, layer);
            animator.SetBool("IsInteracting", isInteracting);
        }
        public virtual void TriggerSkill()
        {
            currentWeapon.TriggerSkill(controller, currentIndexMoveSet);
        }
        public void ActiveCombo()
        {
            animator.SetBool("CandoCombo", true);
        }
        public void DeactiveCombo()
        {
            animator.SetBool("CandoCombo", false);
        }

        public void ActionRun()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effRun);
        }
        public void Pause()
        {
            animator.speed = 0;
        }
        public void Resume()
        {
            animator.speed = 1;
        }


        public override void ResetAnimator()
        {
            gameObject.SetActive(true);
            animator.Rebind();
            Resume();
            skeletonMecanim.Initialize(true);
            skeletonMecanim.skeleton.SetToSetupPose();
            skeletonMecanim.skeleton.SetBonesToSetupPose();
        }
    }
    [System.Serializable]
    public class CharacterStats
    {
        public CharacterStats(int health, int damage)
        {
            this.health = health;
            this.damage = damage;
        }
        public int health;
        public int damage;
        public float currHealth { get; set; }
        public void ApplyDamage(int damage)
        {
            currHealth -= damage;
        }
        public void SetCurrentHealth(float newValue)
        {
            currHealth = newValue;
            if (currHealth > health)
            {
                currHealth = health;
            }
        }
        public float normalizedHealth
        {
            get { return currHealth / (float)health; }
        }
        public void ResetStats()
        {
            currHealth = health;
        }
    }
}

