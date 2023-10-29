using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame.sdk;
using System;

namespace SuperFight
{
    public class GameManager : Singleton<GameManager>
    {
        public GAMEMODE gameMode;
        public StandingCtrl standingCtrl;
        [Header("Component")]
        public DataManager dataManager;
        [SerializeField] RevivePopupCtl revivePopup;
        public LuckWheelPopup luckWheelPopup;
        public NotEnoughPopup notEnoughPopup;
        private ScreenUIManager screenManager;
        public static event Action resetDay;
        [Header("Level")]
        public int maxLevel = 32;
        public int CurrLevel;
        public int levelTempId
        {
            get { return PlayerPrefs.GetInt("level_temp_id", 1); }
            set { PlayerPrefs.SetInt("level_temp_id", value); }
        }
        public int Getlevelrandom(int _level)
        {
            int lvLoad = PlayerPrefs.GetInt("mem_lv_load" + _level, 0);
            if (lvLoad <= 0)
            {
                lvLoad = ranlevelOver(_level);
            }
            Debug.Log($"aaa: Getlevelrandom level={_level}-lvLoad={lvLoad}");
            return lvLoad;
        }

        public int FreeSkinItem
        {
            get { return PlayerPrefs.GetInt("FreeSkinItem", 2); }
            set { PlayerPrefs.SetInt("FreeSkinItem", value); }
        }
        public int FreeSwordItem
        {
            get { return PlayerPrefs.GetInt("FreeSwordItem", 2); }
            set { PlayerPrefs.SetInt("FreeSwordItem", value); }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------
        private List<int> listLvRanNormal = new List<int>();
        private List<int> listLvRanBoss = new List<int>();

        public int ranlevelOver(int level)
        {
            int lvRan;
            if (level % 4 == 0)
            {
                if (listLvRanBoss.Count == 0)
                {
                    getListLvRan("_boss");
                    if (listLvRanBoss.Count == 0)
                    {
                        for (int i = 20; i <= maxLevel; i++)
                        {
                            if (i % 4 == 0)
                            {
                                int va = i;
                                listLvRanBoss.Add(va);
                            }
                        }
                    }
                }
                int n = UnityEngine.Random.Range(0, listLvRanBoss.Count);
                lvRan = listLvRanBoss[n];
                listLvRanBoss.RemoveAt(n);
                saveListLvRan(listLvRanBoss, "_boss", level, lvRan);
            }
            else
            {
                if (listLvRanNormal.Count == 0)
                {
                    getListLvRan("_normal");
                    if (listLvRanNormal.Count == 0)
                    {
                        for (int i = 20; i <= maxLevel; i++)
                        {
                            if (i % 4 != 0)
                            {
                                int va = i;
                                listLvRanNormal.Add(va);
                            }
                        }
                    }
                }
                int n = UnityEngine.Random.Range(0, listLvRanNormal.Count);
                lvRan = listLvRanNormal[n];
                listLvRanNormal.RemoveAt(n);
                saveListLvRan(listLvRanNormal, "_normal", level, lvRan);
            }

            return lvRan;
        }
        void getListLvRan(string namelist)
        {
            string keyran;
            List<int> ltmp;
            if (name.StartsWith("_normal"))
            {
                keyran = "mem_slv_ran_normal";
                ltmp = listLvRanNormal;
            }
            else
            {
                keyran = "mem_slv_ran_boss";
                ltmp = listLvRanBoss;
            }

            string sli = PlayerPrefs.GetString(keyran, "");
            if (sli.Length > 0)
            {
                string[] arr = sli.Split(',');
                if (arr != null)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        int va = int.Parse(arr[i]);
                        ltmp.Add(va);
                    }
                }
            }
        }
        void saveListLvRan(List<int> list, string namelist, int level, int lvran)
        {
            string slv = "";
            if (list.Count > 0)
            {
                slv = "" + list[0];
                if (list.Count > 1)
                {
                    for (int i = 1; i < list.Count; i++)
                    {
                        slv += ("," + list[i]);
                    }
                }
            }
            PlayerPrefs.SetString("mem_slv_ran" + namelist, slv);
            PlayerPrefs.SetInt("mem_lv_load" + level, lvran);
        }
        //========================================================
        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
            dataManager.LoadData();
        }
        void Start()
        {
            PopupManager.Instance.Initialize();
            if (PlayerPrefsUtil.Yesterday.Length <= 0)
            {
                resetDay?.Invoke();
                PlayerPrefsUtil.Yesterday = DateTime.Today.ToString();
            }
            else
            {
                DateTime yesterday = DateTime.Parse(PlayerPrefsUtil.Yesterday);
                if (yesterday < DateTime.Today)
                {
                    resetDay?.Invoke();
                    PlayerPrefsUtil.Yesterday = DateTime.Today.ToString();
                }
            }
            screenManager = ScreenUIManager.Instance;
            SoundManager.Instance.playSoundBG(SoundManager.Instance.musicBgMenu);
            Invoke("LoadAds", 2f);
            screenManager.ShowScreen(ScreenName.MAINSCREEN);
            mygame.sdk.SDKManager.CBFinishloadDing += () =>
            {
                AdsHelper.Instance.showBanner(AD_BANNER_POS.TOP, App_Open_ad_Orien.Orien_Landscape, 320);
            };
        }

      

        public void ShowNotEnough(UpgradePopupCtrl _upgradePopupCtrl)
        {
            notEnoughPopup.gameObject.SetActive(true);
            notEnoughPopup.upgradePopupCtrl = _upgradePopupCtrl;
        }

        public void SwitchGameMode(GAMEMODE mode)
        {
            gameMode = mode;
        }
        public void PlayLevelBoss(int level)
        {
            SwitchGameMode(GAMEMODE.BOSS);
            standingCtrl.HideStanding();
            GameplayCtrl.Instance.StartGame(level);
            if (mygame.sdk.GameHelper.checkLvXaDu())
            {
                AdsHelper.Instance.showFull(false, GameRes.LevelCommon(), false, false, "play_game", false, true);
            }
        }
        public void PlayGame(int level = 0)
        {
            SwitchGameMode(GAMEMODE.CAMPAIGN);
            CurrLevel = level;
            if (mygame.sdk.GameHelper.checkLvXaDu())
            {
               
                if (level <= 0)
                {
                    level = GameRes.GetLevel();
                    CurrLevel = level;
                }

                if (level > maxLevel)
                {
                    level = Getlevelrandom(CurrLevel);
                }
            }
            else
            {
               
                if (level > 6)
                {
                    int a = level / 6;
                    int b = level;
                    level = b - 6 * a;
                }
            }
           
            standingCtrl.HideStanding();
            GameplayCtrl.Instance.StartGame(level);
           
        }

        public void PlayLevelTest()
        {
            GameplayCtrl.Instance.SwitchStateGame(GAME_STATE.GS_PLAYING);
            standingCtrl.HideStanding();
            GameplayCtrl.Instance.StartGameTest();
        }

        public void BackMenu()
        {
            PopupManager.Instance.ClearQueue();
            GameplayCtrl.Instance.SwitchStateGame(GAME_STATE.GS_STOP);
            SoundManager.Instance.playSoundBG(SoundManager.Instance.musicBgMenu);
            standingCtrl.ShowStanding(PlayerPrefsUtil.Theme);
            PlayerManager.Instance.ResetCharacter(Vector3.zero, true);
            GameplayCtrl.Instance.clearMapToNewGame();
            CameraController.Instance.CameraOnMain(Vector3.zero);
            screenManager.ShowScreen(ScreenName.MAINSCREEN);
           
        }
        public void Restart()
        {
            if (!SDKManager.Instance.checkConnection()) return;
            if (gameMode == GAMEMODE.CAMPAIGN)
            {
                FIRhelper.logEvent($"Level_{CurrLevel:000}_restart");
                bool isshow = AdsHelper.Instance.showFull(false, GameRes.LevelCommon() - 1, false, false, "restart_game", false, true, (satead) =>
                {
                    if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                    {
                        PlayGame(CurrLevel);
                    }
                });
                if (!isshow)
                {
                    PlayGame(CurrLevel);
                }

            }
            else
            {
                bool isshow = AdsHelper.Instance.showFull(false, GameRes.LevelCommon() - 1, false, false, "restart_game", false, true, (satead) =>
               {
                   if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                   {
                       PlayLevelBoss(DataManager.Instance.currentBossLevelSelect);
                   }
               });
                if (!isshow)
                {
                    PlayLevelBoss(DataManager.Instance.currentBossLevelSelect);
                }
            }
        }
        public void PauseGame()
        {
            GameplayCtrl.Instance.OnStartPause();
        }
        public void ResumeGame()
        {
            GameplayCtrl.Instance.OnEndPause();
        }

        public void GameLose()
        {
            FIRhelper.logEvent($"Level_{CurrLevel:000}_lose");
            bool isshow = AdsHelper.Instance.showFull(false, GameRes.LevelCommon() - 1, false, false, "lose_level", false, true, (satead) =>
            {
                if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                {
                    // losePopup.Show();
                    screenManager.ShowScreen(ScreenName.LOSESCREEN);
                }
            });
            if (!isshow)
            {
                screenManager.ShowScreen(ScreenName.LOSESCREEN);
            }

        }

        public void GameWin(int coinEarned)
        {
            if (CurrLevel == GameRes.GetLevel())
            {
                if (gameMode != GAMEMODE.BOSS)
                {
                    GameRes.IncreaseLevel();
                }
                CurrLevel = GameRes.GetLevel();
                if (GameRes.GetLevel() == 11)
                {
                    PromoPackManager.Instance.ShowRivenPack();
                }
                if (GameRes.GetLevel() == 16)
                {
                    PromoPackManager.Instance.ShowKaisaPack();
                }
            }

            FIRhelper.logEvent("win_level" + GameRes.GetLevel());
            bool isshow = AdsHelper.Instance.showFull(false, GameRes.LevelCommon() - 1, false, false, "win_level", false, true, (satead) =>
            {
                if (satead == AD_State.AD_CLOSE || satead == AD_State.AD_SHOW_FAIL)
                {
                    DataManager.Instance.AddCoin(coinEarned, 0, "finish_level");
                    WinScreen winScreen = screenManager.ShowScreen<WinScreen>(ScreenName.WINSCREEN);
                    winScreen.SetCoinEarn(coinEarned);
                }
            });
            if (!isshow)
            {
                DataManager.Instance.AddCoin(coinEarned, 0, "finish_level");
                WinScreen winScreen = screenManager.ShowScreen<WinScreen>(ScreenName.WINSCREEN);
                winScreen.SetCoinEarn(coinEarned);
            }
        }

        public void SecondChange()
        {
            revivePopup.gameObject.SetActive(true);
        }

        public void TimeoutRevive()
        {
            GameplayCtrl.Instance.handleTimeOutRevive();
        }

        public void OnRevival()
        {
            GameplayCtrl.Instance.handleSecondChance();
        }

        public void NextLevel(int _currentLevel)
        {
            if (_currentLevel != GameRes.GetLevel())
            {
                _currentLevel++;
            }
            PlayGame(_currentLevel);
        }
    }
    public enum GAMEMODE
    {
        CAMPAIGN, BOSS
    }
}

