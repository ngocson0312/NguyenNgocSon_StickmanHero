using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using mygame.sdk;
using Spine.Unity.AttachmentTools;
using Random = UnityEngine.Random;

namespace SuperFight
{
    public class GameplayCtrl : Singleton<GameplayCtrl>
    {
        //-----------------------------------------------------------------------------------------------------------------------------------

        public GAME_STATE gameState;
        public ObjGameManager objManager;
        public LevelData currMapData = null;
        private List<ItemSavePoint> listItemSavePoint;
        private ItemSavePoint currSavePoint = null;
        public int coinEarned { set; get; }
        private int enemyCount;
        public int enemyKill { set; get; }
        protected bool isRevive = false;
        public bool isTestLevel = false;
        PlayScreenCtl playScreen;

        void Start()
        {
            initFirst();
        }

        private void initFirst()
        {
            playScreen = ScreenUIManager.Instance.GetScreen<PlayScreenCtl>(ScreenName.PLAYSCREEN);
            currSavePoint = null;
        }

        private void initMap()
        {
            currSavePoint = null;
            coinEarned = 0;
            enemyCount = currMapData.listEnemies.Count;
            if (currMapData.isLevelBoss)
            {
                objManager.initBoss(currMapData.bossFightArena);
                enemyCount = currMapData.bossFightArena.bosses.Length;
            }
            enemyKill = 0;
            currMapData.InitAdmix();
            objManager.initEnemy(currMapData.listEnemies);
            objManager.initItemGame(currMapData.listItems);
            PlayerManager.Instance.ResetCharacter(currMapData.playerPoint.position, true);
            if (!isTestLevel)
            {
                GameManager.Instance.standingCtrl.SetIdTheme(currMapData.typeThemes);
            }
        }

        //Init Game-------------------------------------------------------------------------------------------------------------------------------------
        // public void handleStateInitGame()
        // {
        //     switchStateGame(GAME_STATE.GS_PLAYING);
        // }

        public void StartGame(int level)
        {
            ScreenUIManager.Instance.ShowScreen(ScreenName.PLAYSCREEN);
            isTestLevel = false;
            if (currMapData != null)
            {
                clearMapToNewGame();
            }
            GC.Collect();
            AtlasUtilities.ClearCache();
            Resources.UnloadUnusedAssets();
            if (GameManager.Instance.gameMode == GAMEMODE.BOSS)
            {
                currMapData = LevelManager.getInstance().getLevelBoss(level);
            }
            else
            {
                // if (mygame.sdk.SDKManager.quaKietri())
                // {
                currMapData = LevelManager.getInstance().getLevelData(level);
                // }
                // else
                // {
                //     currMapData = LevelManager.getInstance().getLevelDataIOS(level);
                // }
            }

            isRevive = false;
            initMap();
            if (currMapData.isLevelBoss)
            {
                SoundManager.Instance.playSoundBG(SoundManager.Instance.musicBgGameBoss);
            }
            else
            {
                SoundManager.Instance.playSoundBG(SoundManager.Instance.musicBgGameplay);
            }


            playScreen.DeActiveHealthBar();
            playScreen.ResetUI();
            playScreen.SetHeart(GameRes.getRes(RES_type.HEART));
            playScreen.SetEnemyCount(enemyCount, 0);
            objManager.winDoorInMap.SetTextEneMyKill(enemyCount, 0);
            CameraController.Instance.CamOnStart(currMapData.playerPoint.position);
            CameraController.Instance.SetOffset(new Vector3(0, 1.6f, -10));
            SwitchStateGame(GAME_STATE.GS_PLAYING);
        }

        public void StartGameTest()
        {
            isTestLevel = true;
            if (currMapData != null)
            {
                clearMapToNewGame();
            }
            currMapData = LevelManager.getInstance().getLevelTest();
            initMap();
            playScreen.SetTestUI();
            CameraController.Instance.CamOnStart(currMapData.playerPoint.position);
            CameraController.Instance.SetOffset(new Vector3(0, 1.6f, -10));
            // switchStateGame(GAME_STATE.GS_INITGAME);
        }


        //Play Game-------------------------------------------------------------------------------------------------------------------------------------
        public void handleStatePlayGame()
        {

        }

        public void mainPlayerBeKill(float delayHandle = 0f)
        {
            // playScreen.setItemVisible(false);
            if (delayHandle > 0)
            {
                StartCoroutine(delayHandleMainDie(delayHandle));
                return;
            }
            bool stillHeart = GameRes.AddRes(RES_type.HEART, -1, "player_die_level_" + GameManager.Instance.CurrLevel);
            playScreen.SetHeart(GameRes.getRes(RES_type.HEART));
            if (!stillHeart)
            {
                if (!isRevive)
                {
                    onRevive();
                }
                else
                {
                    onGameLose();
                }
            }
            else
            {
                mainPlayerRevive();
            }
        }

        private IEnumerator delayHandleMainDie(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            bool stillHeart = GameRes.AddRes(RES_type.HEART, -1, "player_die_level_" + GameManager.Instance.CurrLevel);
            playScreen.SetHeart(GameRes.getRes(RES_type.HEART));
            if (!stillHeart)
            {
                if (!isRevive)
                {
                    isRevive = true;
                    onRevive();
                }
                else
                {
                    onGameLose();
                }
            }
            else
            {
                mainPlayerRevive();
            }
        }

        private void mainPlayerRevive()
        {
            ScreenUIManager.Instance.ShowScreen(ScreenName.PLAYSCREEN);
            playScreen.ResetUI();
            if (currSavePoint != null)
            {
                PlayerManager.Instance.ResetCharacter(currSavePoint.getPosReborn(), false);
            }
            else
            {
                PlayerManager.Instance.ResetCharacter(currMapData.playerPoint.position, false);
            }
        }

        public void setNewCheckPoint(ItemSavePoint savePoint)
        {
            currSavePoint = savePoint;
        }

        public void enemyBeKill(Enemy enemy, bool hasCoin = true)
        {
            enemyKill++;
            if (hasCoin && !isTestLevel)
            {
                createCoin(enemy.transform.position + Vector3.up);
            }
            if (objManager.winDoorInMap != null)
            {
                objManager.winDoorInMap.SetTextEneMyKill(enemyCount, enemyKill);
            }
            playScreen.SetEnemyCount(enemyCount, enemyKill);
            if (enemyKill == enemyCount)
            {
                objManager.winDoorInMap.OpenDoor();
            }
        }

        public void freeEnemyBeKill(Enemy enemy, float time)
        {
            StartCoroutine(IEFreeEnemy(enemy, time));
        }
        IEnumerator IEFreeEnemy(Enemy enemy, float time)
        {
            yield return new WaitForSeconds(time);
            objManager.removeEnemy(enemy);
        }
        public void createCoin(Vector3 position)
        {
            int numCoinRand = Random.Range(Constant.minCoinKillEnemy, Constant.maxCoinKillEnemy + 1);
            for (int i = 0; i < numCoinRand; i++)
            {
                objManager.createCoin(position);
            }
        }

        public void CreateCoinBoss(Vector3 position)
        {
            StartCoroutine(_CreateCoinBoss(position));
        }

        IEnumerator _CreateCoinBoss(Vector3 position)
        {
            float delayTime = 0;
            for (int i = 0; i < 30; i++)
            {
                yield return new WaitForSeconds(delayTime);
                objManager.createCoin(position);
                delayTime += 0.005f;
            }
        }

        public void CreateCoinOnKillBoss(Vector3 _position, int _minCoin = 10, int _maxCoint = 20)
        {
            int numCoinRand = Random.Range(_minCoin, _maxCoint);
            for (int i = 0; i < numCoinRand; i++)
            {
                objManager.createCoin(_position);
            }
        }

        public void itemHealthBeEat(ItemHealth item, float valPercent)
        {
            int res = AdsHelper.Instance.showGift(GameRes.LevelCommon(), "eat_item_healing", state =>
            {
                if (state == AD_State.AD_SHOW)
                {
                    SoundManager.Instance.enableSoundInAds(false);
                    PlayerManager.Instance.input.ResetInput();
                }
                else if (state == AD_State.AD_CLOSE || state == AD_State.AD_SHOW_FAIL)
                {
                    SoundManager.Instance.enableSoundInAds(true);
                    SwitchStateGame(GAME_STATE.GS_PLAYING);
                    objManager.resumeAllEnemy();
                }
                else if (state == AD_State.AD_REWARD_OK)
                {
                    FIRhelper.logEvent("show_gift_eat_health");
                    SoundManager.Instance.playSoundFx(SoundManager.Instance.effItemHealthBeEat);
                    objManager.removeItem(item);
                    PlayerManager.Instance.OnHealing(valPercent);
                }

                //else if(state == AD_State.AD_REWARD_FAIL || state == AD_State.AD_SHOW_FAIL)
                //{
                // item.noEat();
                //}
            });

            // if (res == 0)
            // {
            //     SoundManager.Instance.enableSoundInAds(false);
            //     switchStateGame(GAME_STATE.GS_PAUSEGAME);
            //     objManager.pauseAllEnemy();
            // }
            //else
            //{
            // item.noEat();
            //}
        }

        public void PickUpWeapon(ItemWeapon itemWeapon, Weapon weapon)
        {
            PlayerManager.Instance.character.LoadWeapon(weapon);
            objManager.removeItem(itemWeapon);
        }

        public void coinBeEat(ItemCoin coin, int value)
        {
            coinEarned += value;
            StartCoroutine(DelayCoin(coin));
        }


        IEnumerator DelayCoin(ItemCoin coin)
        {
            yield return new WaitForSeconds(1.5f);
            objManager.removeItem(coin);
        }

        public void setFxExplode(Transform transf)
        {
            objManager.createFxExplode(transf);
        }

        //Pause Game-------------------------------------------------------------------------------------------------------------------------------------
        public void handleStatePauseGame()
        {

        }

        public void OnStartPause()
        {
            SwitchStateGame(GAME_STATE.GS_PAUSEGAME);
            // Time.timeScale = 0;
            objManager.pauseAllEnemy();
            PlayerManager.Instance.OnPause();
        }

        public void OnEndPause()
        {
            SwitchStateGame(GAME_STATE.GS_PLAYING);
            objManager.resumeAllEnemy();
            PlayerManager.Instance.OnResume();
        }

        private void OnApplicationPause(bool pauseStatus)
        {

        }

        //Second Change-------------------------------------------------------------------------------------------------------------------------------------

        private void onRevive()
        {
            OnStartPause();
            objManager.pauseAllEnemy();
            GameManager.Instance.SecondChange();
        }

        public void handleSecondChance()
        {
            OnEndPause();
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effRevive);
            objManager.resumeAllEnemy();
            mainPlayerRevive();
        }

        public void handleTimeOutRevive()
        {
            onGameLose();
        }

        //Gameover--------------------------------------------------------------------------------------------------------------------------------------
        public void handleStateGameover()
        {

        }


        public void onGameLose()
        {
            if (gameState == GAME_STATE.GS_STOP) return;
            SwitchStateGame(GAME_STATE.GS_STOP);
            objManager.pauseAllEnemy();
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effLose);
            SoundManager.Instance.musicWait(SoundManager.Instance.effLose.length);
            StartCoroutine(delayShowPopupGameover(2f, false));
        }

        public void onGameWin()
        {
            if (gameState == GAME_STATE.GS_STOP) return;
            SwitchStateGame(GAME_STATE.GS_STOP);
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effWin);
            SoundManager.Instance.musicWait(SoundManager.Instance.effWin.length);
            StartCoroutine(delayShowPopupGameover(2f, true));
        }

        private IEnumerator delayShowPopupGameover(float timeDelay, bool isWin)
        {
            if (isWin)
            {
                CameraController.Instance.CamOnWin();
                yield return new WaitForSeconds(.2f);
                ActionWin();
                yield return new WaitForSeconds(timeDelay);
                GameManager.Instance.GameWin(coinEarned);
            }
            else
            {
                CameraController.Instance.CamOnLose();
                yield return new WaitForSeconds(timeDelay);
                GameManager.Instance.GameLose();
            }

        }
        private void ActionWin()
        {
            PlayerManager.Instance.OnWin();
        }
        void SetCoinEarn()
        {

        }

        public void clearMapToNewGame()
        {
            PlayerManager.Instance.transform.parent = null;
            objManager.clearAllEnemy();
            objManager.clearAllItem();
            objManager.clearAllFx();
            // AdmixHelper.freeAll();
            LevelManager.getInstance().DestroyMap();
        }

        public void restartNewGame()
        {
            clearMapToNewGame();
            if (GameManager.Instance.CurrLevel > GameManager.Instance.maxLevel)
            {
                StartGame(GameManager.Instance.Getlevelrandom(GameManager.Instance.CurrLevel));
            }
            else
            {
                StartGame(GameManager.Instance.CurrLevel);
            }

        }
        //================TEST===============
        public void TestNextLevel()
        {
            GameRes.IncreaseLevel();
            clearMapToNewGame();
            GameManager.Instance.PlayGame(GameRes.GetLevel());
        }
        public void TestBackLevel()
        {
            GameplayCtrl.Instance.onGameWin();
            // if (currLvGame > 1)
            // {
            //     currLvGame--;
            //     clearMapToNewGame();
            //     StartGame(currLvGame);
            // }
        }

        //=================================

        //----------------------------------------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                if (GameRes.GetLevel() < GameManager.Instance.maxLevel)
                {
                    GameRes.IncreaseLevel();
                    clearMapToNewGame();
                    GameManager.Instance.PlayGame(GameRes.GetLevel());
                }

            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (GameRes.GetLevel() > 1)
                {
                    GameRes.SetLevel(Level_type.Normal, GameRes.GetLevel() - 1);
                    clearMapToNewGame();
                    GameManager.Instance.PlayGame(GameRes.GetLevel());
                }

            }
        }
#endif


        //STATE GAME--------------------------------------------------------------------------------------------------------------

        public void SwitchStateGame(GAME_STATE state)
        {
            gameState = state;
        }

        // void LateUpdate()
        // {
        //     if (Instance == null)
        //     {
        //         return;
        //     }
        //     switch (mStateGame)
        //     {
        //         case GAME_STATE.GS_INITGAME:
        //             handleStateInitGame();
        //             break;

        //         case GAME_STATE.GS_PLAYING:
        //             handleStatePlayGame();
        //             break;

        //         case GAME_STATE.GS_PAUSEGAME:
        //             handleStatePauseGame();
        //             break;

        //         case GAME_STATE.GS_GAMEOVER:
        //             handleStateGameover();
        //             break;
        //         default:
        //             break;

        //     }
        // }
        //-------------------------------------------------------------------------------------------------------------------------------

    }
}