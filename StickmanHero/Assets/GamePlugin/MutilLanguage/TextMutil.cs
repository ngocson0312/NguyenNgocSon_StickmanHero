//#define Use_TMPro
//#define Use_TexLB

using System;
using UnityEngine;
using UnityEngine.UI;
#if Use_TMPro
using TMPro;
#endif

namespace mygame.sdk
{

    public enum StateCapText
    {
        None = 0,
        FirstCap,
        FirstCapOnly,
        AllCap,
        AllLow
    }

    public enum TypeText
    {
        Text_UI = 0,
        Text_Mesh_pro_ui,
        Text_Mesh_pro,
        Text_UI_Label
    }
    public enum FormatText
    {
        None = 0,
        F_String,
        F_Int,
        F_Float

    }
    public class TextMutil : MonoBehaviour
    {
        public string key = "";
#if UNITY_EDITOR
        public string value = "";
        [HideInInspector] public string memKey = "------------------------------";
        [HideInInspector] public bool isCreateKey = false;
#endif
        public TypeText typeText = TypeText.Text_UI;
        public StateCapText stateCap = StateCapText.None;
        [HideInInspector] public bool isView;
        public bool setOnStart = true;
        public bool setOnAwake = false;
        public FormatText stateFormat = FormatText.None;
        public string objectFormat = "";

        private bool isChangeFont = true;
        
        private void Awake()
        {
            if (setOnAwake)
                setText();
        }

        void Start()
        {
            if (MutilLanguage.Instance().isChangeFont)
            {
                changeFont();
            }

            if (setOnStart)
            {
                setText();
            }
            MutilLanguage.Instance().listTxt.Add(this);
        }

        public void Initialized(bool isSetStart, bool isSetAwake)
        {
            setOnStart = isSetStart;
            setOnAwake = isSetAwake;
            if (MutilLanguage.Instance().isChangeFont)
            {
                changeFont();
            }
        }

        private void OnEnable()
        {
            isView = true;
        }

        private void OnDisable()
        {
            isView = false;
        }

        private void OnDestroy()
        {
            try
            {
                MutilLanguage.Instance().listTxt.Remove(this);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        public void changeFont()
        {
            if (!isChangeFont) return;
            isChangeFont = false;
            if (typeText == TypeText.Text_UI)
            {
                UnityEngine.UI.Text txt = GetComponent<UnityEngine.UI.Text>();
                if (txt != null) {
                    txt.font = SDKManager.Instance.fontReplace;
                    txt.fontSize = (int) (txt.fontSize * SDKManager.Instance.fontSize);
                }
            }
            else if (typeText == TypeText.Text_UI_Label)
            {
#if Use_TexLB
                UILabel lb = GetComponent<UILabel>();
                if (lb != null)
                {
                   lb.fontStyle = FontStyle.Normal;
                   lb.trueTypeFont = SDKManager.Instance.fontReplace;
                }
#endif
            }
#if Use_TMPro
            else if (typeText == TypeText.Text_Mesh_pro_ui)
            {
                
            }
            else if (typeText == TypeText.Text_Mesh_pro)
            {
                
            }
#endif
        }

        public bool setText()
        {
            if (key == null || key.Length <= 0)
            {
                return false;
            }

            try
            {
                if (MutilLanguage.Instance().isChangeFont)
                {
                    changeFont();
                }
                if (typeText == TypeText.Text_UI)
                {
                    GetComponent<Text>().text = MutilLanguage.getStringWithKey(key, stateCap, stateFormat, objectFormat);
                }
                else if (typeText == TypeText.Text_UI_Label)
                {
#if Use_TexLB
                    GetComponent<UILabel>().text = MutilLanguage.getStringWithKey(key, stateCap, stateFormat, objectFormat);
#endif
                }
#if Use_TMPro
                else if (typeText == TypeText.Text_Mesh_pro_ui)
                {
                    GetComponent<TextMeshProUGUI>().text = MutilLanguage.getStringWithKey(key, stateCap, stateFormat, objectFormat);
                }
                else if (typeText == TypeText.Text_Mesh_pro)
                {
                    GetComponent<TextMeshPro>().text = MutilLanguage.getStringWithKey(key, stateCap, stateFormat, objectFormat);
                }
#endif
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log("mysdk: ex=" + ex.ToString());
                return false;
            }
        }

        public bool setText(string key)
        {
            this.key = key;
            return setText();
        }
    }
}
