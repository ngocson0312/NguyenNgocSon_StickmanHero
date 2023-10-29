using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame.sdk;

namespace SuperFight
{
    public class ItemChangeSkin : MonoBehaviour
    {
        [SerializeField] GameObject ObjAds;
        [SerializeField] string SkinName;
        [SerializeField] ParticleSystem FXChangeSkin;
        [SerializeField] List<Skin> ListSkinLocked;
        [SerializeField] Spine.Unity.SkeletonAnimation skeletonAnimation;

        void Start()
        {
            SetSkinChange();
        }
        void SetSkinChange()
        {
            SkinData SkinDataCard = DataManager.Instance.SkinDataCard;
            for (int i = 0; i < SkinDataCard.listSkin.Length; i++)
            {
                if (!DataManager.Instance.IsUnlockSkin(SkinDataCard.listSkin[i].skinName))
                {
                    ListSkinLocked.Add(SkinDataCard.listSkin[i]);
                }
            }
            
            if (ListSkinLocked.Count <= 0)
            {
                gameObject.SetActive(false);
                return;
            }
            int rd = Random.Range(0, ListSkinLocked.Count);
            skeletonAnimation.skeleton.SetSkin(ListSkinLocked[rd].skinName);
            SkinName = ListSkinLocked[rd].skinName;
            if (GameManager.Instance.FreeSkinItem > 0)
            {
                ObjAds.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer(Constant.layerMainPlayer))
            {
                if (GameManager.Instance.FreeSkinItem > 0)
                {
                    GameManager.Instance.FreeSkinItem--;
                    ChangeSkin(col.transform);
                }
                else
                {
                    int res = AdsHelper.Instance.showGift(GameRes.LevelCommon(), "eat_item_skin", state =>
                    {
                        if (state == AD_State.AD_CLOSE || state == AD_State.AD_SHOW_FAIL)
                        {
                            SoundManager.Instance.enableSoundInAds(true);
                            GameplayCtrl.Instance.SwitchStateGame(GAME_STATE.GS_PLAYING);
                            GameplayCtrl.Instance.objManager.resumeAllEnemy();
                        }
                        else if (state == AD_State.AD_REWARD_OK)
                        {
                            FIRhelper.logEvent("show_gift_eat_skin");
                            SoundManager.Instance.enableSoundInAds(true);
                            GameplayCtrl.Instance.SwitchStateGame(GAME_STATE.GS_PLAYING);
                            GameplayCtrl.Instance.objManager.resumeAllEnemy();
                            ChangeSkin(col.transform);
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
        void ChangeSkin(Transform _ParticaleAnchor)
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effItemChangeSkin);
            Instantiate(FXChangeSkin, _ParticaleAnchor).transform.position += Vector3.up * 1;
            PlayerManager.Instance.SetSkin(SkinName);
            DataManager.Instance.currentTrySkin = SkinName;
            gameObject.SetActive(false);
        }
    }
}

