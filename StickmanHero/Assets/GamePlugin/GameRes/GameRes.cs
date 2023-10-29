using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mygame.sdk
{
    public enum Level_type
    {
        Common = 0,
        Normal
    }

    public enum RES_type
    {
        GOLD = 0,
        CRYSTAL,
        HEART
    }
    public class GameRes
    {
        private static GameRes _instance = null;
        public static long timeStartGame = 0;
        public static long timeEndGame = 0;
        public static float duarationPlayLv = 0;
        private static int lvCommon = -1;

        public static GameRes Instance()
        {
            if (_instance == null)
            {
                _instance = new GameRes();
            }
            return _instance;
        }
        public static void transLevelOld2New()
        {
            int lv = PlayerPrefsBase.Instance().getInt("game_res_level", 1);
            int lvnm = GetLevel(Level_type.Normal);
            if (lv > lvnm)
            {
                SetLevel(Level_type.Normal, lv);
            }
        }
        public static int LevelCommon()
        {
            if (lvCommon < 0)
            {
                lvCommon = PlayerPrefsBase.Instance().getInt("game_res_lv_cm", 1);
            }
            return lvCommon;
        }

        public static int GetLevel(Level_type lvType = Level_type.Normal)
        {
            if (lvType == Level_type.Common)
            {
                return LevelCommon();
            }
            else
            {
                string nameLevel = lvType.ToString().ToLower();
                return PlayerPrefsBase.Instance().getInt($"game_res_lv_{nameLevel}", 1);
            }
        }
        public static int GetLevel(string nameLevel)
        {
            if (nameLevel == null || nameLevel.Length == 0)
            {
                return LevelCommon();
            }
            else
            {
                return PlayerPrefsBase.Instance().getInt($"game_res_lv_{nameLevel}", 1);
            }
        }

        public static void SetLevel(Level_type lvType, int lv)
        {
            if (lvType == Level_type.Common)
            {
                lvCommon = lv;
                PlayerPrefsBase.Instance().setInt("game_res_lv_cm", lv);
            }
            else
            {
                string nameLevel = lvType.ToString().ToLower();
                PlayerPrefsBase.Instance().setInt($"game_res_lv_{nameLevel}", lv);
            }
        }
        public static void SetLevel(string nameLevel, int lv)
        {
            if (nameLevel == null || nameLevel.Length == 0)
            {
                lvCommon = lv;
                PlayerPrefsBase.Instance().setInt("game_res_lv_cm", lv);
            }
            else
            {
                PlayerPrefsBase.Instance().setInt($"game_res_lv_{nameLevel}", lv);
            }
        }

        public static void IncreaseLevel(Level_type lvType = Level_type.Normal)
        {
            lvCommon++;
            PlayerPrefsBase.Instance().setInt("game_res_lv_cm", lvCommon);
            if (lvType != Level_type.Common)
            {
                string nameLevel = lvType.ToString().ToLower();
                int lv = PlayerPrefsBase.Instance().getInt($"game_res_lv_{nameLevel}", 1);
                lv++;
                PlayerPrefsBase.Instance().setInt($"game_res_lv_{nameLevel}", lv);
            }
        }
        public static void IncreaseLevel(string nameLevel)
        {
            lvCommon++;
            PlayerPrefsBase.Instance().setInt("game_res_lv_cm", lvCommon);
            if (nameLevel != null || nameLevel.Length > 0)
            {
                int lv = PlayerPrefsBase.Instance().getInt($"game_res_lv_{nameLevel}", 1);
                lv++;
                PlayerPrefsBase.Instance().setInt($"game_res_lv_{nameLevel}", lv);
            }
        }

        public static void startLevel()
        {
            timeStartGame = SdkUtil.systemCurrentMiliseconds();
            timeEndGame = 0;
            duarationPlayLv = 0;
        }

        public static void endLevel()
        {
            if (timeStartGame > 0)
            {
                timeEndGame = SdkUtil.systemCurrentMiliseconds();
            }
            else
            {
                timeEndGame = 0;
            }
        }

        public static int getRes(RES_type type, int defaultva = 0)
        {
            string nameRes = type.ToString().ToLower();
            return getRes(nameRes, defaultva);
        }

        public static int getRes(string nameRes, int defaultva = 0)
        {
            int re = PlayerPrefsBase.Instance().getInt($"game_res_{nameRes}", -100000234);
            if (re == -100000234)
            {
                re = defaultva;
                if (defaultva != 0)
                {
                    PlayerPrefsBase.Instance().setInt($"game_res_{nameRes}", defaultva);
                }
            }
            return re;
        }

        public static bool isAddRes(RES_type type, int value)
        {
            string nameRes = type.ToString().ToLower();
            int _va = getRes(nameRes);
            if (_va >= -value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isAddRes(string nameRes, int value)
        {
            int _va = getRes(nameRes);
            if (_va >= -value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AddRes(RES_type type, int value, string desUpdate, bool isLog = true, float delayTime = 0, Action<int, int> cbAddRes = null)
        {
            string nameRes = type.ToString().ToLower();
            if (delayTime > 0)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(WaiAddRes(nameRes, value, desUpdate, isLog, delayTime, cbAddRes));
                return true;
            }
            else
            {
                return doAddRes(nameRes, value, desUpdate, isLog);
            }
        }

        public static bool AddRes(string nameRes, int value, string desUpdate, bool isLog = true, float delayTime = 0, Action<int, int> cbAddRes = null)
        {
            if (delayTime > 0)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(WaiAddRes(nameRes, value, desUpdate, isLog, delayTime, cbAddRes));
                return true;
            }
            else
            {
                return doAddRes(nameRes, value, desUpdate, isLog);
            }
        }

        private static IEnumerator WaiAddRes(string nameRes, int value, string desUpdate, bool isLog, float delayTime, Action<int, int> cbAddRes = null)
        {
            yield return new WaitForSeconds(delayTime);
            doAddRes(nameRes, value, desUpdate, isLog, cbAddRes);
        }

        private static bool doAddRes(string nameRes, int value, string desUpdate, bool isLog, Action<int, int> cbAddRes = null)
        {
            int _currVa = getRes(nameRes);
            if (cbAddRes != null)
            {
                cbAddRes(_currVa, _currVa + value);
            }
            if (isLog)
            {
                if (value > 0)
                {
                    Dictionary<string, string> dicParam = new Dictionary<string, string>();
                    dicParam.Add("gold", value.ToString());
                    dicParam.Add("des", desUpdate);
                    AdjustHelper.LogEvent(AdjustEventName.ResGet, dicParam);
                    Myapi.LogEventApi.Instance().logResource(LevelCommon(), "source", nameRes, 1, nameRes, value, _currVa, _currVa + value, desUpdate);
                }
                else
                {
                    Dictionary<string, string> dicParam = new Dictionary<string, string>();
                    dicParam.Add("gold", value.ToString());
                    dicParam.Add("des", desUpdate);
                    AdjustHelper.LogEvent(AdjustEventName.ResSpend, dicParam);
                    Myapi.LogEventApi.Instance().logResource(LevelCommon(), "sink", nameRes, 1, nameRes, value, _currVa, _currVa + value, desUpdate);
                }
            }
            if (_currVa >= -value)
            {
                PlayerPrefsBase.Instance().setInt($"game_res_{nameRes}", (_currVa + value));
                return true;
            }
            else
            {
                PlayerPrefsBase.Instance().setInt($"game_res_{nameRes}", 0);
                return false;
            }
        }
    }
}