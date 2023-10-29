using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlugins;
using UnityEngine.UI;
using mygame.sdk;
using TMPro;

namespace SuperFight
{
    public class LevelPickPopupCtl : MonoBehaviour
    {
        public Button buttonClose;
        public Button buttonPrev;
        public Button buttonNext;
        public KeepPlayingPopup keepPlayingPopup;
        public TextMeshProUGUI textPage;
        public LevelPick[] Levels;
        int tmpLevel = 0;
        int page = 1;
        int maxPage
        {
            get { return PlayerPrefs.GetInt("maxPage", 0); }
            set { PlayerPrefs.SetInt("maxPage", value); }
        }

        //public Text textCurrentLevel;
        public int allLevel = 120;
        public void Awake()
        {
            buttonClose.onClick.AddListener(() => ClickButtonClose());
            buttonPrev.onClick.AddListener(() => ClickButtonPrev());
            buttonNext.onClick.AddListener(() => ClickButtonNext());
            for (int i = 0; i < Levels.Length; i++)
            {
                Levels[i].Initialize(this);
            }
        }
        public void ClickButtonClose()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
            Close();
        }

        public void ClickButtonPrev()
        {
            if (page == 1) return;
            page--;
            DisplayLevel();
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
        }
        public void ClickButtonNext()
        {
            if (GameRes.GetLevel() > allLevel)
            {
                allLevel = GameRes.GetLevel();
            }
            if (page >= (allLevel / Levels.Length) + (allLevel % Levels.Length == 0 ? 0 : 1)) return;
            page++;
            DisplayLevel();
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effClickUI);
        }

        public void DisplayLevel()
        {
            int _allpage = 0;
            if (GameRes.GetLevel() > GameManager.Instance.maxLevel)
            {
                _allpage = GameRes.GetLevel() % Levels.Length == 0 ? GameRes.GetLevel() / Levels.Length : (GameRes.GetLevel() / Levels.Length) + 1;
            }
            else
            {
                _allpage = GameManager.Instance.maxLevel % Levels.Length == 0 ? GameManager.Instance.maxLevel / Levels.Length : (GameManager.Instance.maxLevel / Levels.Length) + 1;
            }
            textPage.text = "Level " + GameRes.GetLevel() + "/" + (_allpage * Levels.Length);
            for (int i = 0; i < Levels.Length; i++)
            {
                if (i == Levels[i].id)
                {
                    Levels[i].thisLevel = (page - 1) * Levels.Length + i + 1;
                    Levels[i].RefreshTextLevel(isBoss(Levels[i].thisLevel));
                }
            }
        }
        public void OnSelectLevel(int level)
        {
            if (GameRes.GetLevel() < level)
            {
                keepPlayingPopup.gameObject.SetActive(true);
            }
            else if (GameRes.GetLevel() == level)
            {
                GameManager.Instance.PlayGame(GameRes.GetLevel());
                Close();
            }
            else
            {
                FIRhelper.logEvent("pick_level" + GameRes.GetLevel());
                bool isshow = AdsHelper.Instance.showFull(false, GameRes.LevelCommon() - 1, false, false, "pick_level", false, true, (satead) =>
                {
                    if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                    {
                        GameManager.Instance.PlayGame(level);
                        Close();
                    }
                });
                if (!isshow)
                {
                    GameManager.Instance.PlayGame(level);
                    Close();
                }
            }
        }
        bool isBoss(int _level)
        {
            if (_level % 4 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Open()
        {
            allLevel = GameManager.Instance.maxLevel;
            page = (GameRes.GetLevel() / Levels.Length) + (GameRes.GetLevel() % Levels.Length == 0 ? 0 : 1);
            Debug.Log("Page" + (GameRes.GetLevel() / Levels.Length) + (GameRes.GetLevel() % Levels.Length == 0 ? 0 : 1));
            DisplayLevel();
            gameObject.SetActive(true);
            keepPlayingPopup.gameObject.SetActive(false);
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }


}
