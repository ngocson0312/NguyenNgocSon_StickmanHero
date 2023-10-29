using UnityEngine;

public class CountryCodeUtil
{
    public static string convertToCountryCode(string code)
    {
        string re = code.ToLower();
        switch (re)
        {
            case "vi":
                re = "vn";
                break;
            case "ca":
                re = "ad";
                break;
            case "ar":
                re = "ae";
                break;
            case "fa":
            case "ps":
                re = "af";
                break;
            case "sq":
                re = "al";
                break;
            case "hy":
                re = "am";
                break;
            case "zh-hans":
                re = "cn";
                break;
        }
        return re;
    }

    public static string getLanguageCodeFromCountryCode(string countryCode)
    {
        string re = MutilLanguage.langDefault;
        if (countryCode != null && countryCode.Length > 0)
        {
            string[] dic = { "af-ZA", "ar-AE", "ar-BH", "ar-DZ", "ar-EG", "ar-IQ", "ar-JO", "ar-KW", "ar-LB", "ar-LY", "ar-MA", "ar-OM", "ar-QA", "ar-SA", "ar-SY", "ar-TN", "ar-YE", "az-AZ", "az-AZ", "be-BY", "bg-BG", "bs-BA", "ca-ES", "cs-CZ", "cy-GB", "da-DK", "de-AT", "de-CH", "de-DE", "de-LI", "de-LU", "dv-MV", "el-GR", "en-AU", "en-BZ", "en-CA", "en-CB", "en-GB", "en-IE", "en-JM", "en-NZ", "en-PH", "en-TT", "en-US", "en-ZA", "en-ZW", "es-AR", "es-BO", "es-CL", "es-CO", "es-CR", "es-DO", "es-EC", "es-ES", "es-ES", "es-GT", "es-HN", "es-MX", "es-NI", "es-PA", "es-PE", "es-PR", "es-PY", "es-SV", "es-UY", "es-VE", "et-EE", "eu-ES", "fa-IR", "fi-FI", "fo-FO", "fr-BE", "fr-CA", "fr-CH", "fr-FR", "fr-LU", "fr-MC", "gl-ES", "gu-IN", "he-IL", "hi-IN", "hr-BA", "hr-HR", "hu-HU", "hy-AM", "id-ID", "is-IS", "it-CH", "it-IT", "ja-JP", "ka-GE", "kk-KZ", "kn-IN", "ko-KR", "kok-IN", "ky-KG", "lt-LT", "lv-LV", "mi-NZ", "mk-MK", "mn-MN", "mr-IN", "ms-BN", "ms-MY", "mt-MT", "nb-NO", "nl-BE", "nl-NL", "nn-NO", "ns-ZA", "pa-IN", "pl-PL", "ps-AR", "pt-BR", "pt-PT", "qu-BO", "qu-EC", "qu-PE", "ro-RO", "ru-RU", "sa-IN", "se-FI", "se-FI", "se-FI", "se-NO", "se-NO", "se-NO", "se-SE", "se-SE", "se-SE", "sk-SK", "sl-SI", "sq-AL", "sr-BA", "sr-BA", "sr-SP", "sr-SP", "sv-FI", "sv-SE", "sw-KE", "syr-SY", "ta-IN", "te-IN", "th-TH", "tl-PH", "tn-ZA", "tr-TR", "tt-RU", "uk-UA", "ur-PK", "uz-UZ", "uz-UZ", "vi-VN", "xh-ZA", "zh-CN", "zh-HK", "zh-MO", "zh-SG", "zh-TW", "zu-ZA" };
            string upcode = countryCode.ToUpper();
            for (int i = 0; i < dic.Length; i++)
            {
                if (dic[i].Contains(upcode))
                {
                    int idx = dic[i].IndexOf('-');
                    re = dic[i].Substring(0, idx);
                    Debug.Log("mysdk: getLanguageCodeFromCountryCode countryCode=" + countryCode + ", re=" + re);
                    break;
                }
            }
        }

        return re;
    }
}