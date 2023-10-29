using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame.sdk;


namespace SuperFight
{
    public class ItemWeapon : BaseItem
    {
        // [SerializeField] private BoxCollider2D collider;
        private bool isBeEat = false;
        [SerializeField] GameObject ObjAds;
        [SerializeField] SpriteRenderer sprWeapon;
        [SerializeField] Weapon weapon;
        private void OnEnable()
        {
            isBeEat = false;
            SetWeaponEat();
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer(Constant.layerMainPlayer))
            {
                if (GameManager.Instance.FreeSwordItem > 0)
                {
                    GameManager.Instance.FreeSwordItem--;
                    beEat();
                }
                else
                {
                    int res = AdsHelper.Instance.showGift(GameRes.LevelCommon(), "eat_item_weapon", state =>
                    {
                        if (state == AD_State.AD_CLOSE || state == AD_State.AD_SHOW_FAIL)
                        {
                            SoundManager.Instance.enableSoundInAds(true);
                            GameplayCtrl.Instance.SwitchStateGame(GAME_STATE.GS_PLAYING);
                            GameplayCtrl.Instance.objManager.resumeAllEnemy();
                        }
                        else if (state == AD_State.AD_REWARD_OK)
                        {
                            FIRhelper.logEvent("show_gift_eat_weapon");
                            SoundManager.Instance.enableSoundInAds(true);
                            GameplayCtrl.Instance.SwitchStateGame(GAME_STATE.GS_PLAYING);
                            GameplayCtrl.Instance.objManager.resumeAllEnemy();
                            beEat();
                        }
                    });
                    // if (res == 0)
                    // {
                    //     //PlayerManager.Instance.input.ResetInput();
                    //     SoundManager.Instance.enableSoundInAds(false);
                    //     GameplayCtrl.Instance.SwitchStateGame(GAME_STATE.GS_PAUSEGAME);
                    //     GameplayCtrl.Instance.objManager.pauseAllEnemy();
                    // }
                }
            }
        }

        private void beEat()
        {
            if (isBeEat) return;
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effWeaponCollect);
            isBeEat = true;
            GameplayCtrl.Instance.PickUpWeapon(this, weapon);
        }
        void SetWeaponEat()
        {
            if (GameManager.Instance.FreeSwordItem > 0)
            {
                ObjAds.SetActive(false);
            }
            else
            {
                ObjAds.SetActive(true);
            }
        }
    }
}