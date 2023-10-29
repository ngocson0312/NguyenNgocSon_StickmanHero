using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mygame.sdk;
using DG.Tweening;
using TMPro;

namespace SuperFight
{
    public class LevelPick : MonoBehaviour
    {
        public int id;
        public int thisLevel;
        [Header("UI")]
        public TextMeshProUGUI textLevel;
        Button buttonLevel;
        public Image spr;
        public Image imgBoss;
        [Header("Sprite")]
        public Sprite Unlocked;
        public Sprite Selected;
        public Sprite Locked;
        private LevelPickPopupCtl levelPickPopupCtl;
        public void Initialize(LevelPickPopupCtl levelPickPopupCtl)
        {
            this.levelPickPopupCtl = levelPickPopupCtl;
            buttonLevel = GetComponent<Button>();
            buttonLevel.onClick.AddListener(() => OnClick());
        }
        public void OnClick()
        {
            if (!SDKManager.Instance.checkConnection()) return;
            levelPickPopupCtl.OnSelectLevel(this.thisLevel);
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
        }
        public void RefreshTextLevel(bool _isBoss = false)
        {
            if (_isBoss)
            {
                imgBoss.gameObject.SetActive(true);
            }
            else
            {
                imgBoss.gameObject.SetActive(false);
            }
            textLevel.text = thisLevel.ToString();
            tweener.Kill();
            if (thisLevel < GameRes.GetLevel())
            {
                transform.localScale = Vector3.one;
                spr.sprite = Unlocked;
            }
            else
            if (thisLevel == GameRes.GetLevel())
            {
                spr.sprite = Selected;
                ScaleThis();
            }
            else
            if (thisLevel > GameRes.GetLevel())
            {
                transform.localScale = Vector3.one;
                spr.sprite = Locked;
            }
        }


        Tweener tweener;
        public void ScaleThis()
        {
            transform.localScale = new Vector3(.95f, .95f, .95f);
            tweener = transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), .5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
    }

}
