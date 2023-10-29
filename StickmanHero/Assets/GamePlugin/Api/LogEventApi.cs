#define ENABLE_API_LOG
using mygame.sdk;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

namespace Myapi
{
    public enum MyEventLog
    {
        login = 0,
        logLevel,
        logInApp,
        logResource,
        logSessionMode,
        logAd,
        logDie,
        AdsMaxConversionData
    }

    public enum LevelPassStatus
    {
        Play = 0,
        PlayBonus,
        PlayAgain,
        Skip,
        Win,
        Lose,
        EndBonus,
        Die,
        Skin,
    }
    public enum AdsTypeLog
    {
        Interstitial = 0,
        Reward
    }

    public enum TypeDif
    {
        Easy,
        Normal,
        Hard
    }
    public class LogEventApi
    {
        private static LogEventApi _instance = null;
#if UNITY_ANDROID
        string platform = "ANDROID";
#elif UNITY_IOS || UNITY_IPHONE
        string platform = "IOS";
#else
        string platform = "";
#endif

        List<ObjectLog> listQueueLog = new List<ObjectLog>();

        public static LogEventApi Instance()
        {
            if (_instance == null)
            {
                _instance = new LogEventApi();
                _instance.readLog();
            }
            return _instance;
        }
        public string getbaseParam(MyEventLog eventlog)
        {
            string nameapilog = eventlog.ToString();
            string urllog = PlayerPrefs.GetString("mem_url_log", AppConfig.urlLogEvent);
            string re = $"{urllog}/{nameapilog}?";
            re += $"gameid={AppConfig.gameID}";
            string dvid = GameHelper.Instance.AdsIdentify;
            if (dvid == null || dvid.Length < 3)
            {
                dvid = GameHelper.Instance.deviceid;
            }
            re += $"&deviceid={dvid}";
            re += $"&version={AppConfig.verapp}";
            re += $"&platform={platform}";
            re += $"&country={GameHelper.Instance.countryCode}";
            long tcr = SdkUtil.systemCurrentMiliseconds() / 1000;
            re += $"&createTime={tcr}";

            return re;
        }
        public void pushMemLog()
        {
            try
            {
                if (listQueueLog != null && listQueueLog.Count > 0)
                {
                    SdkUtil.logd($"pushMemLog");
                    listQueueLog[0].tryCount++;
                    saveListLog();
                    LogEvent(listQueueLog[0]);
                }
            }
            catch (Exception ex)
            {
                Debug.Log("mysdk: ex=" + ex.ToString());
            }
        }

        public void logLogin()
        {
            string data = $"{{\"country\":\"{GameHelper.Instance.countryCode}\"}}";
            SdkUtil.logd($"logLogin:={data}");
            LogEvent(MyEventLog.login, data);
        }
        public void logLevel(int lv, string gameMode, TypeDif typeDif, LevelPassStatus passLevelStatus, string where)
        {
            int dua = (int)GameRes.duarationPlayLv;
            string data = $"{{\"level\":{lv},\"mode\":\"{gameMode}\",\"levelDifficulty\":\"{typeDif.ToString()}\",\"duration\":\"{dua}\",\"passLevelStatus\":\"{passLevelStatus.ToString()}\",\"where\":\"{where}\"}}";
            SdkUtil.logd($"loglevel:={data}");
            LogEvent(MyEventLog.logLevel, data);
            if (passLevelStatus == LevelPassStatus.Win || passLevelStatus == LevelPassStatus.Lose)
            {
                GameRes.timeStartGame = 0;
                GameRes.timeEndGame = 0;
                GameRes.duarationPlayLv = 0;
            }
        }
        public void logInApp(string productId, string transactionId, string purchaseToken, string currencyCode, float price, string where)
        {
            string data = $"{{\"level\":{GameRes.LevelCommon()},\"productId\":\"{productId}\",\"transactionId\":\"{transactionId}\",\"purchaseToken\":\"purchaseToken\",\"currencyCode\":\"{currencyCode}\",\"price\":{price},\"where\":\"{where}\"}}";
            SdkUtil.logd($"loginapp:={data}");
            LogEvent(MyEventLog.logInApp, data);
        }
        public void logResource(int lv, string flowtype, string itemtype, int itemid, string currency, int amount, int vaB, int vaA, string des)
        {
            string data = $"{{\"level\":{lv},\"flowType\":\"{flowtype}\",\"itemType\":\"{itemtype}\",\"itemId\":{itemid},\"currency\":\"{currency}\",\"amount\":{amount},\"valueBefore\":{vaB},\"valueAfter\":{vaA},\"dictionaryDetail\":\"{des}\"}}";
            SdkUtil.logd($"logresource:={data}");
            LogEvent(MyEventLog.logResource, data);
        }
        public void logSessionMode(int lv, string mode, long startTime, long endTime, int duration)
        {
            string data = $"{{\"level\":{lv},\"mode\":\"{mode}\",\"startTime\":{startTime},\"endTime\":{endTime},\"duration\":{duration}}}";
            SdkUtil.logd($"logsession:={data}");
            LogEvent(MyEventLog.logSessionMode, data);
        }
        public void LogAds(AdsTypeLog typeLog, string where)
        {
            int countLogAds = PlayerPrefs.GetInt("gameResCountAds", 1);
            string data = $"{{\"level\":{GameRes.LevelCommon()},\"typeAd\":\"{typeLog.ToString()}\",\"where\":\"{where}\",\"count\":{countLogAds}}}";
            SdkUtil.logd($"logads:={data}");
            LogEvent(MyEventLog.logAd, data);
            countLogAds++;
            PlayerPrefs.SetInt("gameResCountAds", countLogAds);
        }

        public void LogEvent(MyEventLog eventLog, string jsonData)
        {
            string url = getbaseParam(eventLog);
#if !UNITY_EDITOR && ENABLE_API_LOG
            int islog = PlayerPrefs.GetInt("mem_flag_log", 1);
            if (islog == 1)
            {
                ApiManager.Instance.postRequest(url, jsonData, (b, s) =>
                {
                    if (!b || s == null || !s.Contains("\"status\":1"))
                    {
                        SdkUtil.logd($"LogEvent new err");
                        ObjectLog obmem = saveLog(url, jsonData);
                        listQueueLog.Add(obmem);
                    }
                    else
                    {
                        SdkUtil.logd($"LogEvent new ok");
                        pushMemLog();
                    }
                });
            }
#endif
        }

        private void LogEvent(ObjectLog oblog)
        {
            if (oblog != null)
            {
                string url = oblog.url;
#if !UNITY_EDITOR  && ENABLE_API_LOG
                ApiManager.Instance.postRequest(url, oblog.jsonData, (b, s) =>
                {
                    if (b && s != null && s.Contains("\"status\":1"))
                    {
                        SdkUtil.logd($"LogEvent try ok");
                        removeLogwhenLogOkOrOvertime(oblog);
                        pushMemLog();
                    }
                    else
                    {
                        SdkUtil.logd($"LogEvent try err");
                    }
                });
#endif
            }
        }

        private ObjectLog saveLog(string url, string jsonData)
        {
            try
            {
                ObjectLog re = new ObjectLog();
                re.logId = SdkUtil.systemCurrentMiliseconds();
                re.url = url;
                re.jsonData = jsonData;

                string path = Application.persistentDataPath + "/log.dat";

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(re.toString());
                    sw.Close();
                }
                return re;
            }
            catch (Exception ex)
            {
                Debug.Log("mysdk: ex=" + ex.ToString());
            }

            return null;
        }

        private void saveListLog()
        {
            try
            {
                string path = Application.persistentDataPath + "/log.dat";
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (var logitem in listQueueLog)
                    {
                        sw.WriteLine(logitem.toString());
                    }
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.Log("mysdk: ex=" + ex.ToString());
            }
        }

        private void readLog()
        {
            try
            {
                string path = Application.persistentDataPath + "/log.dat";
                if (File.Exists(path))
                {
                    listQueueLog.Clear();
                    long tcr = SdkUtil.systemCurrentMiliseconds();
                    long dtc = 3 * 24 * 3600000;
                    using (StreamReader sr = File.OpenText(path))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            SdkUtil.logd("readlog: memog=" + s);
                            ObjectLog ob = new ObjectLog();
                            if (ob.fromData(s) && (ob.tryCount <= 3 || (tcr - ob.logId) <= dtc))
                            {
                                SdkUtil.logd("readlog: add lis");
                                listQueueLog.Add(ob);
                            }
                        }
                        sr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("mysdk: ex=" + ex.ToString());
            }
        }

        private void removeLogwhenLogOkOrOvertime(ObjectLog log)
        {
            listQueueLog.Remove(log);
            saveListLog();
        }
    }

    public class ObjectLog
    {
        public long logId;
        public string url;
        public string jsonData;
        public int tryCount = 0;

        public string toString()
        {
            string re = $"{logId};s;{tryCount};s;{url};s;{jsonData}";
            return re;
        }

        public bool fromData(string sdata)
        {
            if (sdata != null)
            {
                string[] arrData = sdata.Split(new string[] { ";s;" }, StringSplitOptions.None);
                if (arrData != null && arrData.Length == 4)
                {
                    try
                    {
                        logId = long.Parse(arrData[0]);
                        tryCount = int.Parse(arrData[1]);
                        url = arrData[2];
                        jsonData = arrData[3];
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("mysdk: ex=" + ex.ToString());
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
