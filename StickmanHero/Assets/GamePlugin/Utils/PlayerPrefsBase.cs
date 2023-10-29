using System;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace mygame.sdk
{
    public class PlayerPrefsBase
    {
        private static PlayerPrefsBase _instance;

        public static PlayerPrefsBase Instance()
        {
            if (_instance == null)
            {
                _instance = new PlayerPrefsBase();
            }

            return _instance;
        }

        private bool isEncode = false;

        private bool checkHas = true;

        private string getKey(string key)
        {
            if (isEncode)
            {
                char[] arrc = key.ToCharArray();
                for (int i = 0; i < arrc.Length; i++)
                {
                    if (arrc[i] >= 'A' && arrc[i] <= 'Z')
                    {
                        arrc[i] += (char)3;
                        if (arrc[i] > 'Z')
                        {
                            arrc[i] -= 'Z';
                        }
                    }
                    else if (arrc[i] >= 'a' && arrc[i] <= 'z')
                    {
                        arrc[i] += (char)3;
                        if (arrc[i] > 'z')
                        {
                            arrc[i] -= 'z';
                        }
                    }
                }
                return new string(arrc);
            }
            else
            {
                return key;
            }
        }

        public int getInt(string key, int valdef = 0)
        {
            bool isll = false;
            if (key.CompareTo("ladsdjsjhdj") == 0)
            {
                isll = true;
            }
            if (isEncode)
            {
                if (checkHas && PlayerPrefs.HasKey(key))
                {
                    var val = PlayerPrefs.GetInt(key, valdef);
                    PlayerPrefs.DeleteKey(key);
                    setInt(key, val);
                    return val;
                }
                string genkey = getKey(key);
                if (PlayerPrefs.HasKey(genkey))
                {
                    int re = PlayerPrefs.GetInt(genkey, valdef);
                    int k1 = ((re >> 16) & 0xFFFF);
                    int k2 = (re & 0xFFFF);
                    re = ((k2 << 16) | k1);
                    re -= 3;
                    if ((re & 1) == 0)
                    {
                        return (re >> 1);
                    }
                    else
                    {
                        return valdef;
                    }
                }
                else
                {
                    return valdef;
                }
            }
            else
            {
                return PlayerPrefs.GetInt(key, valdef);
            }
        }

        public void setInt(string key, int val)
        {
            if (isEncode)
            {
                string genkey = getKey(key);
                int nv = (val << 1);
                nv += 3;
                int k1 = (nv >> 16);
                k1 = (k1 & 0xFFFF);
                int k2 = (nv & 0xFFFF);
                nv = ((k2 << 16) | k1);
                PlayerPrefs.SetInt(genkey, nv);
            }
            else
            {
                PlayerPrefs.SetInt(key, val);
            }
        }

        public float getFloat(string key, float valdef)
        {
            if (isEncode)
            {
                if (checkHas && PlayerPrefs.HasKey(key))
                {
                    var val = PlayerPrefs.GetFloat(key, valdef);
                    PlayerPrefs.DeleteKey(key);
                    setFloat(key, val);
                    return val;
                }
                string genkey = getKey(key);
                if (PlayerPrefs.HasKey(genkey))
                {
                    float re = PlayerPrefs.GetFloat(genkey, valdef) - 3;
                    return (re / 2);
                }
                else
                {
                    return valdef;
                }
            }
            else
            {
                return PlayerPrefs.GetFloat(key, valdef);
            }
        }

        public void setFloat(string key, float val)
        {
            if (isEncode)
            {
                string genkey = getKey(key);
                float nv = val * 2;
                nv += 3;
                PlayerPrefs.SetFloat(genkey, nv);
            }
            else
            {
                PlayerPrefs.SetFloat(key, val);
            }
        }

        public string getString(string key, string valdef)
        {
            if (isEncode)
            {
                if (checkHas && PlayerPrefs.HasKey(key))
                {
                    var val = PlayerPrefs.GetString(key, valdef);
                    PlayerPrefs.DeleteKey(key);
                    setString(key, val);
                    return val;
                }
                string genkey = getKey(key);
                return PlayerPrefs.GetString(genkey, valdef);
            }
            else
            {
                return PlayerPrefs.GetString(key, valdef);
            }
        }

        public void setString(string key, string val)
        {
            if (isEncode)
            {
                string genkey = getKey(key);
                PlayerPrefs.SetString(genkey, val);
            }
            else
            {
                PlayerPrefs.SetString(key, val);
            }
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(getKey(key));
        }
    }
}