using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;
using System.Collections.Generic;

namespace SuperFight
{
    public class UltimateTextDamageEditor : EditorWindow
    {
        // Added to window menu
        [MenuItem( "Window/UltimateTextDamage/Open preferences" )]
        public static void OpenPreferences( )
        {
            UTDWindow window = (UTDWindow)EditorWindow.GetWindow<UTDWindow>( "UTD Preferences" );
            window.Show( );
        }
    }

    public class UTDWindow : EditorWindow
    {
        public static readonly string [] Symbols = new string[] {
            "UTD_TEXT_MESH_PRO"
        };

        void OnGUI( )
        {
            GUILayout.Label( "Ultimate Text Damage" , EditorStyles.helpBox );
            GUILayout.Label( "Preferences" , EditorStyles.largeLabel );

            bool useNewText = ContainsSymbol( );

            GUILayout.Label( "Text renderer" , EditorStyles.boldLabel );
            GUILayout.Label( "Toggle to use the new TextMeshPro renderer" , EditorStyles.helpBox );
            bool newValue  = GUILayout.Toggle( useNewText , "Use TextMeshPro" );

            if( newValue != useNewText )
            {
                if( newValue )
                {
                    AddDefineSymbol( );
                    EditorUtility.DisplayDialog( "UltimateTextDamage" , "You have switched to TextMeshPro. If you see errors in the console please be sure you have TextMeshPro installed through the package manager. Also be sure your UltimateTextDamage items use TextMeshPro, please check the demo scenes ending in _tmp." , "OK" );
                }
                else
                {
                    RemoveDefineSymbols( );
                }
            }
        }

        private void RemoveDefineSymbols( )
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
            List<string> allDefines = definesString.Split ( ';' ).ToList ();
            allDefines.RemoveAll( r => Symbols.Contains( r ) );
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
               EditorUserBuildSettings.selectedBuildTargetGroup ,
               string.Join( ";" , allDefines.ToArray( ) ) );
        }

        private void AddDefineSymbol( )
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
            List<string> allDefines = definesString.Split ( ';' ).ToList ();
            allDefines.AddRange( Symbols.Except( allDefines ) );
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup ,
                string.Join( ";" , allDefines.ToArray( ) ) );
        }

        private bool ContainsSymbol( )
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup( EditorUserBuildSettings.selectedBuildTargetGroup );
            return symbols.Contains( Symbols[ 0 ] );
        }
    }
}