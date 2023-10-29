using System.Collections;
using System.Collections.Generic;
using mygame.sdk;
using UnityEngine;
namespace SuperFight
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [Header("Components")]
        public PlayerController playerController;
        public Character character;
        public InputHandle input;
        private Collider2D coll;
        public Transform characterHolder;
        [Header("Flags Status")]
        public bool isDead;
        [SerializeField] private ParticleSystem fxMainDie;
        [SerializeField] private ParticleSystem fxEquipWeapon;
        private void Start()
        {
            Initialize();
        }
        public void ResetCharacter(Vector3 newPos, bool newLevel)
        {
            transform.position = newPos;
            isDead = false;
            input.ResetInput();
            input.ActiveInput();
            playerController.Resume();
            playerController.ResetScript();
            character.ResetAnimator();
            Weapon weapon = character.currentWeapon;
            SetSkin(DataManager.Instance.currentSkin);
            Debug.Log("kskskkk: " + newLevel);
            if (newLevel)
            {
                character.LoadWeapon(null, true);
            }
            else
            {
                if(weapon.keyWeapon)
                {
                    character.LoadWeapon(weapon, true);
                }
            }
            Invincible();
        }
        public void Invincible()
        {
            playerController.isInvincible = true;
            Invoke(nameof(DeactiveInvincible), 2f);
        }
        void DeactiveInvincible()
        {
            playerController.isInvincible = false;
        }
        public void Initialize()
        {
            coll = GetComponent<Collider2D>();
            playerController.Initialize(this);
            SetToDefaultSkin();
        }

        public void SetToDefaultSkin()
        {
            SetSkin(DataManager.Instance.currentSkin);
        }

        public void SetSkin(string skinName)
        {
            Debug.Log(skinName);
            if (character != null)
            {
                Destroy(character.gameObject);
            }
            SkinObject skin = DataManager.Instance.GetSkin(skinName);
            Character c = skin.character;
            if (c == null)
            {
                c = Resources.Load<Character>("DefaultCharacter");
            }
            character = Instantiate(c, characterHolder);
            playerController.animatorHandle = character;
            character.Initialize(playerController);
            character.ResetAnimator();
            character.LoadSkin(skinName);
        }

        void Update()
        {
            if (GameplayCtrl.Instance.gameState == GAME_STATE.GS_PAUSEGAME)
            {
                // playerController.SetVelocity(Vector2.zero);
                return;
            }
            if (character == null) return;
            playerController.UpdateScript();
        }
        void FixedUpdate()
        {
            if (GameplayCtrl.Instance.gameState == GAME_STATE.GS_PAUSEGAME) return;
            if (character == null) return;
            playerController.FixedUpdateScript();
        }
        public void OnHealing(float valPercent)
        {
            playerController.Heal(valPercent);
        }
        public void PlayerDie(bool _explode = false)
        {
            if (isDead) return;
            character.LoadWeapon(null);
            input.Deactive();
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effDeath);
            isDead = true;
            input.ResetInput();
            fxMainDie.Play();
            if (_explode)
            {
                character.gameObject.SetActive(false);
            }
            else
            {
                character.gameObject.SetActive(true);
            }
            GameplayCtrl.Instance.mainPlayerBeKill(2);
            FIRhelper.logEvent($"Level_{GameManager.Instance.CurrLevel:000}_die");
        }
        public Bounds GetBoundPlayer()
        {
            Vector2 center = coll.bounds.center;
            Bounds bounds = new Bounds(center, coll.bounds.size);
            return bounds;
        }
        public void OnWin()
        {
            character.PlayAnimation("Victory", 1, false);
        }
        public void PlayFXEquipWeapon()
        {
            fxEquipWeapon.Play();
        }
        public void OnPause()
        {
            playerController.Pause();
            character.Pause();
        }
        public void OnResume()
        {
            playerController.Resume();
            character.Resume();
        }
    }
}