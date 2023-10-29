using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using mygame.sdk;

namespace SuperFight
{
    public class ItemDoor : BaseItem
    {
        private bool openDoor;
        [SerializeField] GameObject lockDoor;
        [SerializeField] ParticleSystem enderDust;
        [SerializeField] TextMeshPro txtEnemy;
        private void OnEnable()
        {
            lockDoor.SetActive(true);
            openDoor = false;
            GetComponent<Collider2D>().enabled = true;
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.CompareTag(Constant.layerMainPlayer) && openDoor)
            {
                PlayerMoveToDoor(col.transform, new Vector3(transform.position.x, col.transform.position.y), .2f);
                //PlayScreenCtl.Instance.ShowPopupWin();
                GetComponent<BoxCollider2D>().enabled = false;
                GameplayCtrl.Instance.onGameWin();
                //ShowAdsInDoor();
                if (SoundManager.Instance.IsEnableEffect)
                {
                    GetComponent<AudioSource>().Play();
                }
                
            }
        }
        void ShowAdsInDoor()
        {
            FIRhelper.logEvent("go_to_the_door_level" + GameRes.GetLevel());
            bool isshow = AdsHelper.Instance.showFull(false, GameRes.LevelCommon(), false, false, "go_to_the_door", false, true, (satead) =>
            {
                if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                {
                    GameplayCtrl.Instance.onGameWin();
                }
            });
            if (!isshow)
            {
                GameplayCtrl.Instance.onGameWin();
            }
        }

        void PlayerMoveToDoor(Transform _player, Vector3 _target, float _time)
        {
            _player.DOMove(_target, _time).SetEase(Ease.Linear);
        }
        public void OpenDoor()
        {
            // SoundManager.Instance.playSoundFx(SoundManager.Instance.effOpenDoor);
            enderDust.Play();
            lockDoor.SetActive(false);
            openDoor = true;
        }
        public void SetTextEneMyKill(int _enemyCount, int _enemyKill)
        {
            txtEnemy.text = _enemyKill + "/" + _enemyCount;
        }
    }
}