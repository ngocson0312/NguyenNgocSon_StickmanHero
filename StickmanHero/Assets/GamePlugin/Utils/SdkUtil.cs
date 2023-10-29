//#define ENABLE_MYLOG

using System;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace mygame.sdk
{
    public class SdkUtil
    {
        public static int levelLog = 0;//0-debug; 1-warring; 2-error

        public static int screenWith = -1;
        public static int screenHight = -1;

        private static int verSdk = 0;
        public static long systemCurrentMiliseconds()
        {
            var re = (DateTime.UtcNow.Ticks - 621355968000000000) / 10000;
            return re;
        }

        public static bool isDevicesStrong()
        {
#if UNITY_EDITOR
            return true;
#elif (UNITY_IOS || UNITY_IPHONE)
            int something = (int)UnityEngine.iOS.Device.generation;
            if (something >= 50)
            {
                return true;
            }
            else
            {
                return false;
            }
#else
            int mem = SystemInfo.systemMemorySize;
            if (mem >= 4000)
            {
                return true;
            }
            else
            {
                return false;
            }
#endif      
        }

        public static string converMoney(long money)
        {
            //Debug.Log($"aaa b={money}");
            var re = money.ToString();

            if (re.Length > 3)
            {
                var n = (re.Length - 1) / 3;
                var len = re.Length - 3;
                for (var i = 0; i < n; i++)
                {
                    re = re.Insert(len - 3 * i, ".");
                }
            }

            return re;
        }

        public static string converMoneyK(int money)
        {
            int m;
            if (money >= 1000)
                m = money / 1000;
            else
                m = money;
            var re = m.ToString();

            if (re.Length > 3)
            {
                var n = (re.Length - 1) / 3;
                var len = re.Length - 3;
                for (var i = 0; i < n; i++)
                {
                    re = re.Insert(len, ".");
                    len -= 3;
                }
            }

            return re;
        }

        public static string convertMoneyToString(long money)
        {
            if (money >= 1000000000)
            {
                return (money / 1000000000).ToString() + "B";
            }
            if (money >= 1000000)
            {
                return (money / 1000000).ToString() + "M";
            }
            if (money >= 10000)
            {
                return (money / 1000).ToString() + "K";
            }
            return money.ToString();
        }

        public static string getMoney(int va)
        {
            string re = "";

            int tmp = va;
            while (tmp > 0)
            {
                int n = tmp / 1000;
                tmp = tmp % 1000;
                if (n > 0)
                {
                    if (re.Length == 0)
                    {
                        re = "" + n;
                    }
                    else
                    {
                        re += ("," + n.ToString("000"));
                    }
                }
                else
                {
                    if (re.Length == 0)
                    {
                        re = "" + tmp;
                    }
                    else
                    {
                        re += ("," + tmp.ToString("000"));
                    }
                    break;
                }
            }

            return re;
        }

        public static int getAndroidBuildVersion()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (verSdk <= 0)
            {
                using (var buildVersion = new AndroidJavaClass("android.os.Build$VERSION"))
                {
                    int verSdk = buildVersion.GetStatic<int>("SDK_INT");
                    return verSdk;
                }
            }
            else
            {
                return verSdk;
            }
#else
            return 1000;
#endif
        }

        public static string formatNumber(int number)
        {
            var re = number.ToString();
            if (re.Length > 3)
            {
                var n = (re.Length - 1) / 3;
                var len = re.Length - 3;
                for (var i = 0; i < n; i++)
                {
                    re = re.Insert(len, ".");
                    len -= 3;
                }
            }

            return re;
        }

        public static string genPlayerName()
        {
            var name = PlayerPrefs.GetString("mem_name_player", "");
            if (name.Length == 0)
            {
                var t = systemCurrentMiliseconds();
                t = t / 1000;
                t = t % 10000;
                var idxch1 = Random.Range(0, 26);
                var idxch2 = Random.Range(0, 26);
                var ch1 = (char)('a' + (char)idxch1);
                var ch2 = (char)('a' + (char)idxch2);
                name = "Player_" + ch1 + ch2 + t;
            }

            return name;
        }

        public static string int2TimeString(int seconds)
        {
            string re = "";
            int h = seconds / 3600;
            int m = seconds % 3600;
            int s = m % 60;
            m = m / 60;
            re = string.Format("{0}:{1:d2}:{2:d2}", h, m, s);

            return re;
        }

        public static DateTime DateTimeFromTimeStamp(long tsp)
        {
            if (tsp < 0)
            {
                tsp = 0;
            }
            DateTime re = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            re = re.AddSeconds(tsp).ToLocalTime();
            return re;
        }

        public static long TimeStampFromDateTime(DateTime tsp)
        {
            DateTimeOffset dof = new DateTimeOffset(tsp);
            return dof.ToUnixTimeSeconds();
        }

        public static int SubDay(long from, long to)
        {
            DateTime dbase = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            DateTime dFrom = dbase.AddSeconds(from);
            DateTime dto = dbase.AddSeconds(to);
            int re = dFrom.DayOfYear - dto.DayOfYear;
            int dy = dFrom.Year - dto.Year;
            re = dy * 365 + re;
            return re;
        }

        public static int SubDay(DateTime from, DateTime to)
        {
            int re = (int)((from.ToBinary() - to.ToBinary()) / (24 * 3600));
            return re;
        }
		
#if UNITY_EDITOR
        public static void myMaHoa(string bkey, out byte[] makey, out int[] paskey, out byte[] pasva)
        {
            if (bkey != null && bkey.Length > 5)
            {
                makey = new byte[bkey.Length];
                int nl = UnityEngine.Random.Range(bkey.Length / 2 - 5, bkey.Length / 2 + 5);
                if (nl <= 0)
                {
                    nl = bkey.Length / 2;
                }
                paskey = new int[nl];
                pasva = new byte[nl];
                int nd = bkey.Length / nl;
                if (nd <= 0)
                {
                    nd = 2;
                }
                for (int i = 0; i < nl; i++)
                {
                    int na = (i + 4) / 2;
                    paskey[i] = nd * i + UnityEngine.Random.Range(-na, na);
                    pasva[i] = (byte)(UnityEngine.Random.Range(0, 20));
                }
                int idxp = 0;
                //String sslog = "";
                for (int i = 0; i < bkey.Length; i++)
                {
                    byte ch = (byte)bkey[i];
                    if (idxp < paskey.Length && i >= paskey[idxp])
                    {
                        idxp++;
                    }
                    if (idxp < paskey.Length)
                    {
                        ch -= pasva[idxp];
                        //sslog += $"{i},{idxp};";
                    }
                    makey[i] = ch;
                }
                //Debug.Log($"aaa={sslog}");
            }
            else
            {
                makey = new byte[5];
                paskey = new int[5];
                pasva = new byte[5];
            }
        }
#endif

        public static string myGiaima(byte[] makey, int[] paskey, byte[] pasva)
        {
            if (makey != null && makey.Length > 5 && paskey != null && paskey.Length > 0 && pasva != null && pasva.Length == paskey.Length)
            {
                string pkey = "";
                var sb = new System.Text.StringBuilder();
                int idxp = 0;
                //String sslog = "";
                for (int i = 0; i < makey.Length; i++)
                {
                    char ch = (char)makey[i];
                    if (idxp < paskey.Length)
                    {
                        if (i >= paskey[idxp])
                        {
                            idxp++;
                        }
                    }
                    if (idxp < paskey.Length)
                    {
                        ch = (char)(makey[i] + pasva[idxp]);
                        //sslog += $"{i},{idxp};";
                    }
                    sb.Append(ch);
                }
                pkey = sb.ToString();
                //Debug.Log($"aaa={sslog}");
                //Debug.Log($"aaabbb={pkey}");
                return pkey;
            }
            return "";
        }

        public static bool isiPad()
        {
#if (UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
            if (UnityEngine.iOS.Device.generation.ToString().Contains("iPad"))
            {
                return true;
            }
            else
            {
                float w = Screen.width;
                float h = Screen.height;
                if (h < w)
                {
                    float th = h;
                    h = w;
                    w = th;
                }
                if (w > 0)
                {
                    float per = h / w;
                    if (per < 1.65f)
                    {
                        return true;
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
#else
            float w = Screen.width;
            float h = Screen.height;
            if (h < w)
            {
                float th = h;
                h = w;
                w = th;
            }
            if (w > 0)
            {
                float per = h / w;
                if (per < 1.65f)
                {
                    return true;
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
#endif
        }

        public static float ScreenDpi()
        {
            float re = 1.5f;
#if UNITY_IOS || UNITY_IPHONE
            if (UnityEngine.iOS.Device.generation.ToString().Contains("iPad"))
            {
                Debug.Log("mysdk: iPad dpi=" + Screen.dpi);
                re = Screen.dpi;
            }
            else
            {
                Debug.Log("mysdk: iPhone dpi=" + Screen.dpi);
                re = Screen.dpi;
            }
#else
            Debug.Log("mysdk: Android dpi=" + Screen.dpi);
            re = Screen.dpi;
#endif
            if (re <= 0)
            {
                re = 1.65f * 160;
            }
            else if (re <= 1)
            {
                re = 1.5f * Screen.dpi;
            }
            Debug.Log("mysdk: screen dpi=" + re);
            return re;
        }

        public static float getHeighBanner()
        {
            float dpi = ScreenDpi();
            if (isiPad())
            {
                return 90 * dpi / 160.0f;
            }
            else
            {
                return 50 * dpi / 160.0f;
            }
        }

        public static void logd(string log)
        {
#if ENABLE_MYLOG
            if (levelLog <= 0)
            {
                Debug.Log("mysdk: " + log);
            }
#endif
        }

        public static void logW(string log)
        {
#if ENABLE_MYLOG
            if (levelLog <= 1)
            {
                Debug.LogWarning("mysdk: " + log);
            }
#endif
        }

        public static void logE(string log)
        {
#if ENABLE_MYLOG
            if (levelLog <= 2)
            {
                Debug.LogError("mysdk: " + log);
            }
#endif
        }
    }
}