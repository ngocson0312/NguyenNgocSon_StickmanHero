using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Guirao.UltimateTextDamage
{
    public class UIDemo : MonoBehaviour
    {
        public bool isSceneTextMeshPro = true;
        public Text labelText;
        
        // Use this for initialization
        void Start( )
        {

#if UNITY_EDITOR
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup( EditorUserBuildSettings.selectedBuildTargetGroup );
            List<string> allDefines = symbols.Split ( ';' ).ToList();

            bool textMeshProOK = false;
            foreach( string s in allDefines )
            {
                if( s == "UTD_TEXT_MESH_PRO" )
                {
                    textMeshProOK = true;
                    break;
                }
            }

            if( isSceneTextMeshPro )
            {
                labelText.enabled = !textMeshProOK;
                labelText.text = "ALERT: This demo won't work using the new 'TextMeshPro' with the current configuration.\nBe sure to install TextMeshpro through the package manager and enable 'use text mesh pro' in the configuration of UltimateTextDamage by going to the menu 'Window/UltimateTextDamage->Open preferences'";
            }
            else
            {
                labelText.enabled = textMeshProOK;
                labelText.text = "ALERT: This demo won't work using the old 'Text' with the current configuration.\nYou can change the configuration of UltimateTextDamage and switching to not use TextMeshPro by going to the menu 'Window/UltimateTextDamage->Open preferences'";
            }
#endif
        }
    }
}
