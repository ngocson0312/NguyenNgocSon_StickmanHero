//#define ENABLE_SOCIAL
//#define ENABLE_HELPER

using System;
using System.Collections;
using UnityEngine;
#if ENABLE_SOCIAL
#if UNITY_IOS || UNITY_IPHONE

    using UnityEngine.SocialPlatforms.GameCenter;

#elif UNITY_ANDROID

using GooglePlayGames;
using GooglePlayGames.BasicApi;

#endif
#endif


namespace mygame.sdk
{
    public enum LEADER_BOARD_TYPE
    {
        TYPE_LEVEL
    }

    public class SocialHelper : MonoBehaviour
    {
        private string achiveMentIdSunmit = "";
        private int flagAuthent = -1;

        private string leaderboadIdSubmit = "";

        private int pointtoSubmit;
        private double progressAchivementSunmit = 1;
        private int stateAuthent = 0;
        private int stepAchiveSunmit = 0;
        public static SocialHelper Instance { get; private set; }

        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
#if ENABLE_SOCIAL
#if UNITY_ANDROID
    			leaderboadIdSubmit = GPGSIds.leaderboard_high_score;
#elif UNITY_IOS || UNITY_IPHONE
    			leaderboadIdSubmit = GameCenterIds.leaderboard_total_high_score;
#endif
#endif          

#if ENABLE_SOCIAL
#if UNITY_ANDROID
                PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                    //              .EnableSavedGames()//vvv
                    .Build();

                PlayGamesPlatform.InitializeInstance(config);
                //PlayGamesPlatform.DebugLogEnabled = true;
                PlayGamesPlatform.Activate();
#elif UNITY_IOS || UNITY_IPHONE
                    GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
#endif
            }
        }

        // Use this for initialization
        private void Start()
        {
#if UNITY_IOS || UNITY_IPHONE
            StartCoroutine(wait4Authen());
#elif UNITY_ANDROID
            if (checkGamePlayInstalled()) {
                StartCoroutine(wait4Authen());
            }
#endif
        }

        private IEnumerator wait4Authen()
        {
            Debug.Log("mysdk: social wait4Authen 1");
            yield return new WaitForSeconds(1);
            Debug.Log("mysdk: social wait4Authen 2");
            if (stateAuthent == 0)
            {
                Debug.Log("mysdk: social wait4Authen 3");
                authentSocial();
            }
        }

        public bool checkGamePlayInstalled() {
            Debug.Log("mysdk: checkGamePlayInstalled");
            return GameHelper.Instance.checkPackageAppIsPresent("com.google.android.play.games");
        }

        public void authentSocial(Action<int, string, string, string> onLoginService = null)
        {
#if ENABLE_SOCIAL
            stateAuthent = 2;
            Debug.Log("mysdk: authentSocial 1");
            Social.localUser.Authenticate((bool success) =>
            {

                Debug.Log("mysdk: authentSocial = " + success.ToString());
                if (success)
                {
                    PlayerPrefs.SetInt("mem_login_game_user", 1);
#if UNITY_ANDROID
                    PlayerPrefs.SetString("GameUserId", PlayGamesPlatform.Instance.localUser.id);
                    PlayerPrefs.SetString("GameUserName", PlayGamesPlatform.Instance.localUser.userName);
                    onLoginService?.Invoke(1, PlayGamesPlatform.Instance.localUser.id, PlayGamesPlatform.Instance.localUser.userName, PlayGamesPlatform.Instance.GetUserImageUrl());
#elif UNITY_IOS || UNITY_IPHONE
                    PlayerPrefs.SetString("GameUserId", Social.localUser.id);
                    PlayerPrefs.SetString("GameUserName", Social.localUser.userName);
                    onLoginService?.Invoke(1, Social.localUser.id, Social.localUser.userName, ""/*Social.localUser.image*/);
#endif
                    stateAuthent = 1;
                    if (flagAuthent == 1)
                    {
                        flagAuthent = -1;
                        _submitScore(leaderboadIdSubmit, pointtoSubmit);
                    }
                    else if (flagAuthent == 2)
                    {
                        flagAuthent = -1;
                        _showLeaderBoard();
                    }
                    else if (flagAuthent == 3)
                    {
                        flagAuthent = -1;
                        _submitArchivement(achiveMentIdSunmit, progressAchivementSunmit);
                    }
                    else if (flagAuthent == 4)
                    {
                        flagAuthent = -1;
                        _showArchivement();
                    }
                    else if (flagAuthent == 5)
                    {
                        flagAuthent = -1;
                        _incrementAchievement(achiveMentIdSunmit, stepAchiveSunmit);
                    }
                }
                else
                {
                    onLoginService?.Invoke(0, "", "", "");
                    stateAuthent = -1;
                    flagAuthent = -1;
                }
            });
#endif
#if ENABLE_HELPER
            javaAuthen();
#endif
        }

        public void showLeaderBoard()
        {
#if ENABLE_SOCIAL
            Debug.Log("mysdk: social showLeaderBoard1");
            if (stateAuthent == 0 || stateAuthent == 2)
            {
                Debug.Log("mysdk: social showLeaderBoard2");
                flagAuthent = 2;
                if (stateAuthent == 0)
                {
                    Debug.Log("mysdk: social showLeaderBoard3");
                    authentSocial();
                }
            }
            else if (Social.localUser.authenticated)
            {
                Debug.Log("mysdk: social showLeaderBoard4");
                _showLeaderBoard();
            }
#endif
#if ENABLE_HELPER
            javaShowLeaderboard();
#endif
        }

        public string getLeaderBoardId(LEADER_BOARD_TYPE type)
        {
            var lid = "";
#if ENABLE_SOCIAL
            if (type == LEADER_BOARD_TYPE.TYPE_LEVEL)
            {
#if UNITY_ANDROID
                lid = "";//vvvGPGSIds.LeaderBoardId;//vvv
#elif UNITY_IOS || UNITY_IPHONE
                lid = GameCenterIds.leaderboard_total_high_score;
#endif
            }
#endif

            return lid;
        }

        public void submitScore(LEADER_BOARD_TYPE type, int score)
        {
            bool isss = Social.localUser.authenticated;
            var lid = getLeaderBoardId(type);
#if ENABLE_SOCIAL
            Debug.Log("mysdk: social submitScore1");
            if (stateAuthent == 0 || stateAuthent == 2)
            {
                Debug.Log("mysdk: social submitScore2");
                flagAuthent = 1;
                if (stateAuthent == 0)
                {
                    Debug.Log("mysdk: social submitScore3");
                    int memlogin = PlayerPrefs.GetInt("mem_login_gg_ok", 0);
                    if (memlogin == 1)
                    {
                        Debug.Log("mysdk: social submitScore4");
                        authentSocial();
                    }
                }

                pointtoSubmit = score;
                leaderboadIdSubmit = lid;
            }
            else if (Social.localUser.authenticated)
            {
                Debug.Log("mysdk: social submitScore4");
                _submitScore(lid, score);
            }
#endif
#if ENABLE_HELPER
            javaSubmitscore(score, lid);
#endif
        }

        public void submitArchivement(string archivement, double progress)
        {
#if ENABLE_SOCIAL
            Debug.Log("mysdk: social submitArchivement 1");
            if (stateAuthent == 0 || stateAuthent == 2)
            {
                Debug.Log("mysdk: social submitArchivement 2");
                flagAuthent = 3;
                if (stateAuthent == 0)
                {
                    Debug.Log("mysdk: social submitArchivement 3");
                    authentSocial();
                }

                achiveMentIdSunmit = archivement;
                progressAchivementSunmit = progress;
            }
            else if (Social.localUser.authenticated)
            {
                Debug.Log("mysdk: social submitArchivement 4");
                _submitArchivement(archivement, progress);
            }
#endif
        }

        public void showArchivement()
        {
#if ENABLE_SOCIAL
            Debug.Log("mysdk: social showArchivement 1");
            if (stateAuthent == 0 || stateAuthent == 2)
            {
                Debug.Log("mysdk: social showArchivement 2");
                flagAuthent = 4;
                if (stateAuthent == 0)
                {
                    Debug.Log("mysdk: social showArchivement 3");
                    authentSocial();
                }
            }
            else if (Social.localUser.authenticated)
            {
                Debug.Log("mysdk: social showArchivement 4");
                _showArchivement();
            }
#endif
        }

        public void incrementAchievement(string archivement, int stepAchive)
        {
#if ENABLE_SOCIAL
            Debug.Log("mysdk: social incrementAchievement 1");
            if (stateAuthent == 0 || stateAuthent == 2)
            {
                Debug.Log("mysdk: social incrementAchievement 2");
                flagAuthent = 5;
                if (stateAuthent == 0)
                {
                    Debug.Log("mysdk: social incrementAchievement 3");
                    authentSocial();
                }

                achiveMentIdSunmit = archivement;
                stepAchiveSunmit = stepAchive;
            }
            else if (Social.localUser.authenticated)
            {
                Debug.Log("mysdk: social incrementAchievement 4");
                _incrementAchievement(archivement, stepAchive);
            }
#endif
        }

        private void _submitArchivement(string archivementid, double progress)
        {
#if ENABLE_SOCIAL
            Social.ReportProgress(archivementid, progress, (bool success) =>
            {
                Debug.Log("mysdk: social _submitArchivement " + success);
                // handle success or failure
            });
#endif
        }

        private void _submitScore(string lid, int score)
        {
#if ENABLE_SOCIAL
            Debug.Log("mysdk: social _submitScore");
            Social.ReportScore(score, lid, (bool success) =>
            {
                Debug.Log("mysdk: social _submitScore " + success);
                // handle success or failure
            });
#endif
        }

        private void _showArchivement()
        {
#if ENABLE_SOCIAL
            Debug.Log("mysdk: social _showLeaderBoard");
            Social.ShowAchievementsUI();
#endif
        }

        private void _incrementAchievement(string aid, int step)
        {
#if ENABLE_SOCIAL
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.IncrementAchievement(aid, step, (bool success) =>
            {
                Debug.Log("mysdk: social _incrementAchievement " + success);
                // handle success or failure
            });
#endif
#endif
        }

        private void _showLeaderBoard()
        {
#if ENABLE_SOCIAL
            Debug.Log("mysdk: social _showLeaderBoard");
            Social.ShowLeaderboardUI();
#endif
        }

        private void javaAuthen()
        {
#if UNITY_ANDROID
            using (var gGhelper = new AndroidJavaObject("com.helper.game.GGHelper"))
            {
                gGhelper.CallStatic("login");
            }
#endif
        }

        private void javaSubmitscore(int score, string lid)
        {
#if UNITY_ANDROID
            using (var gGhelper = new AndroidJavaObject("com.helper.game.GGHelper"))
            {
                gGhelper.CallStatic("submitScore", score, lid);
            }
#endif
        }

        private void javaShowLeaderboard()
        {
#if UNITY_ANDROID
            using (var gGhelper = new AndroidJavaObject("com.helper.game.GGHelper"))
            {
                gGhelper.CallStatic("showLeaderBoard");
            }
#endif
        }
    }
}