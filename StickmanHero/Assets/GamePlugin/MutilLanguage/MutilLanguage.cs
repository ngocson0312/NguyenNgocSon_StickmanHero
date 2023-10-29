//#define use_mutil_lang

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mygame.sdk;

public class MutilLanguage
{
    public const string langDefault = "default";//"default";
    public const bool isConvertCountryCode2Lang = false;

    private static MutilLanguage _instance = null;

    private IDictionary<string, string> dicStringCurr = new Dictionary<string, string>();
    private List<string> listCountryChangeFont = new List<string>();

    public string languageCode4mutil { get; set; }

    public List<TextMutil> listTxt { get; private set; }
    public bool isChangeFont { get; private set; }

    public static MutilLanguage Instance()
    {
        if (_instance == null)
        {
            _instance = new MutilLanguage();
            _instance.listTxt = new List<TextMutil>();
            string[] arrLangChangeFont = { "vi", "th", "ja", "ko" };
            for (int i = 0; i < arrLangChangeFont.Length; i++)
            {
                _instance.listCountryChangeFont.Add(arrLangChangeFont[i]);
            }
        }
        return _instance;
    }
    public void initRes()
    {
        Debug.Log("mysdk: mutil lang initRes 0");
#if use_mutil_lang
        languageCode4mutil = PlayerPrefs.GetString("mem_set_lang", langDefault);
        if (languageCode4mutil.Equals(langDefault))
        {
            languageCode4mutil = GameHelper.Instance.languageCode;
            // if (isConvertCountryCode2Lang && languageCode4mutil != null && (languageCode4mutil.Equals("en") || languageCode4mutil.Equals(langDefault)))
            // {
            //     languageCode4mutil = CountryCodeUtil.getLanguageCodeFromCountryCode(GameHelper.Instance.countryCode);
            // }
        }
        if (languageCode4mutil == null || languageCode4mutil.Length == 0)
        {
            languageCode4mutil = langDefault;
        }
        #if UNITY_EDITOR
        // languageCode4mutil = "ja";//vvv
        #endif

        bool isdefault = false;
        TextAsset tac = Resources.Load<TextAsset>("Langs/" + languageCode4mutil);
        if (tac == null || tac.text == null || tac.text.Length < 10)
        {
            isdefault = true;
            tac = Resources.Load<TextAsset>("Langs/" + langDefault);
            isChangeFont = false;
        }
        else
        {
            if (languageCode4mutil.CompareTo(langDefault) == 0)
            {
                isdefault = true;
                isChangeFont = false;
            }
            else
            {
                if (listCountryChangeFont.Contains(languageCode4mutil))
                {
                    isChangeFont = true;
                }
                else
                {
                    isChangeFont = false;
                }
            }
        }

        if (dicStringCurr == null)
        {
            dicStringCurr = new Dictionary<string, string>();
        }
        string data = tac.text;
        var dictmp = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(data);
        foreach (KeyValuePair<string, object> item in dictmp)
        {
            dicStringCurr.Add(item.Key, (string)item.Value);
        }
        //ListStringCountry list = JsonUtility.FromJson<ListStringCountry>(data);

        if (!isdefault)
        {
            data = Resources.Load<TextAsset>("Langs/" + langDefault).text;
            var adsplcf = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(data);
            foreach (KeyValuePair<string, object> item in adsplcf)
            {
                if (!dicStringCurr.ContainsKey(item.Key))
                {
                    dicStringCurr.Add(item.Key, (string)item.Value);
                }
            }
        }

        Debug.Log("mysdk: mutil lang initRes 1");
#endif
    }

    public void reInitRes()
    {
        Debug.Log("mysdk: mutil lang reInitRes 0");
#if use_mutil_lang
        string lset = PlayerPrefs.GetString("mem_set_lang", "");
        if ((languageCode4mutil.CompareTo(langDefault) == 0 || languageCode4mutil.CompareTo("en") == 0) && lset.Length <= 0)
        {
            languageCode4mutil = GameHelper.Instance.languageCode;
            // if (isConvertCountryCode2Lang && languageCode4mutil != null && (languageCode4mutil.CompareTo(langDefault) == 0 || languageCode4mutil.CompareTo("en") == 0))
            // {
            //     languageCode4mutil = CountryCodeUtil.getLanguageCodeFromCountryCode(GameHelper.Instance.countryCode);
            // }
            Debug.Log("mysdk: mutil lang reInitRes languageCode4mutil=" + languageCode4mutil);
            if (languageCode4mutil != null && languageCode4mutil.CompareTo(langDefault) != 0)
            {
                Debug.Log("mysdk: mutil lang reInitRes 1");
                TextAsset tac = Resources.Load<TextAsset>("Langs/" + languageCode4mutil);
                Debug.Log("mysdk: mutil lang reInitRes 2");
                if (tac != null && tac.text != null && tac.text.Length > 10)
                {
                    Debug.Log("mysdk: mutil lang reInitRes lang=" + languageCode4mutil);
                    string data = tac.text;
                    var dictmp = (IDictionary<string, object>)MyJson.JsonDecoder.DecodeText(data);
                    foreach (KeyValuePair<string, object> item in dictmp)
                    {
                        dicStringCurr[item.Key] = (string)item.Value;
                    }
                    for (int i = 0; i < listTxt.Count; i++)
                    {
                        listTxt[i].setText();
                    }
                }
            }
        }
#endif
    }

    public static void setText(Text textui, string defaulttxt = "0d0o")
    {
#if use_mutil_lang
        if (_instance != null && textui != null)
        {
            TextMutil txtscript = textui.GetComponent<TextMutil>();
            if (txtscript != null)
            {
                if (txtscript.setText() == false)
                {
                    if (string.Compare(defaulttxt, "0d0o") != 0)
                    {
                        textui.text = defaulttxt;
                    }
                }
            }
            else
            {
                if (string.Compare(defaulttxt, "0d0o") != 0)
                {
                    textui.text = defaulttxt;
                }
            }
        }
        else if (textui != null && string.Compare(defaulttxt, "0d0o") != 0)
        {
            textui.text = defaulttxt;
        }
#else
        if (textui != null && string.Compare(defaulttxt, "0d0o") != 0)
        {
            textui.text = defaulttxt;
        }
#endif
    }

    public static string getStringWithKeyNormal(string key, string defaulttxt = "")
    {
        return getStringWithKey(key, StateCapText.None, FormatText.None, null, defaulttxt);
    }

    public static string getStringWithKey(string key, StateCapText stateCap = StateCapText.None, FormatText stateFormat = FormatText.None, object obFormat = null, string defaulttxt = "")
    {
        string re = defaulttxt;
#if use_mutil_lang
        if (_instance != null)
        {
            string va;
            if (_instance.dicStringCurr.TryGetValue(key, out va))
            {
                switch (stateFormat)
                {
                    case FormatText.F_Int:
                        re = string.Format(va, obFormat);
                        break;
                    case FormatText.F_Float:
                        re = string.Format(va, obFormat);
                        break;
                    case FormatText.F_String:
                        re = string.Format(va, obFormat);
                        break;
                    default:
                        re = va;
                        break;
                }
            }
        }
#endif
        switch (stateCap)
        {
            case StateCapText.AllCap:
                re = re.ToUpper();
                break;
            case StateCapText.FirstCap:
                if (re.Length > 0)
                {
                    if (re.Length == 1)
                    {
                        re = re.ToUpper();
                    }
                    else
                    {
                        re = char.ToUpper(re[0]) + re.Substring(1);
                    }
                }
                break;
            case StateCapText.FirstCapOnly:
                if (re.Length > 0)
                {
                    if (re.Length == 1)
                    {
                        re = re.ToUpper();
                    }
                    else
                    {
                        string t1 = re.ToLower();
                        re = char.ToUpper(t1[0]) + t1.Substring(1);
                    }
                }
                break;
            case StateCapText.AllLow:
                re = re.ToLower();
                break;
        }
        return re;
    }

    public static void setTextWithKey(Text textui, string key, string defaulttxt = "0d0o")
    {
#if use_mutil_lang
        if (_instance != null && textui != null)
        {
            TextMutil txtscript = textui.GetComponent<TextMutil>();
            if (txtscript != null)
            {
                if (txtscript.setText(key) == false)
                {
                    if (string.Compare(defaulttxt, "0d0o") != 0)
                    {
                        textui.text = defaulttxt;
                    }
                }
            }
            else
            {
                if (string.Compare(defaulttxt, "0d0o") != 0)
                {
                    textui.text = defaulttxt;
                }
            }
        }
        else if (textui != null && string.Compare(defaulttxt, "0d0o") != 0)
        {
            textui.text = defaulttxt;
        }
#else
        if (textui != null && string.Compare(defaulttxt, "0d0o") != 0)
        {
            textui.text = defaulttxt;
        }
#endif
    }

    public void setLang(string lang)
    {
        PlayerPrefs.SetString("mem_set_lang", lang);
        initRes();
        for (int i = 0; i < listTxt.Count; i++)
        {
            listTxt[i].setText();
        }
    }
}

[System.Serializable]
public class ItemStringCountry
{
    public string key;
    public string value;
}

[System.Serializable]
public class ListStringCountry
{
    public List<ItemStringCountry> list;
}
