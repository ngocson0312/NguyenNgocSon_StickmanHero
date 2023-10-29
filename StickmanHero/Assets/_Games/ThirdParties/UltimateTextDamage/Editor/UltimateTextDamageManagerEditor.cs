using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace SuperFight
{
    [CustomEditor( typeof( UltimateTextDamageManager ) )]
    public class UltimateTextDamageManagerEditor : Editor
    {
        private ReorderableList list;

        private SerializedProperty pCam;
        private SerializedProperty pConvertToCamera;
        private SerializedProperty pAutoFaceCameraWorldSpace;
        private SerializedProperty pCanvas;

        private SerializedProperty pOverlap;
        private SerializedProperty pFollow;
        private SerializedProperty pOffset;
        private SerializedProperty pDamping;

        private Color colorOk;

        private void OnEnable( )
        {
            list = new ReorderableList( serializedObject , serializedObject.FindProperty( "textTypes" ) , true , true , true , true );
            pCam = serializedObject.FindProperty( "theCamera" );
            pCanvas = serializedObject.FindProperty( "canvas" );
            pConvertToCamera = serializedObject.FindProperty( "convertToCamera" );
            pOverlap = serializedObject.FindProperty( "overlaping" );
            pFollow = serializedObject.FindProperty( "followsTarget" );
            pOffset = serializedObject.FindProperty( "offsetUnits" );
            pDamping = serializedObject.FindProperty( "damping" );
            pAutoFaceCameraWorldSpace = serializedObject.FindProperty( "autoFaceToCamera" );

            colorOk = new Color( 0.75f , 2 , 0.75f );
        }

        public override void OnInspectorGUI( )
        {
            serializedObject.Update( );

            EditorGUILayout.HelpBox( "Welcome to UltimateTextDamage, to start, set up the references and add at least one text damage type." , MessageType.Info );

            EditorGUILayout.Space( );
            EditorGUILayout.LabelField( new GUIContent( "References" ) , EditorStyles.boldLabel );

            // Canvas
            EditorGUILayout.HelpBox( "The canvas in which TextDamage items will be rendered.\nSCREENSPACE canvas will enable the option to convert from a world camera." , MessageType.None );
            GUI.color = Color.white;
            if( pCanvas.objectReferenceValue == null )
            {
                EditorGUILayout.HelpBox( "No canvas reference, please assign the canvas" , MessageType.Error );
                GUI.color = Color.red;
            }
            else
            {
                GUI.color = colorOk;
            }
            EditorGUILayout.PropertyField( pCanvas );
           
            GUI.color = Color.white;
            if( pCanvas.objectReferenceValue != null && pCanvas.objectReferenceValue is Canvas )
            {
                Canvas c = pCanvas.objectReferenceValue as Canvas;
                if( c.renderMode != RenderMode.WorldSpace )
                {
                    EditorGUILayout.Space( );

                    // Convert
                    EditorGUILayout.HelpBox( "Enable this option if the position to spawn the item is in world space (i.e: showing the damage above an enemy).\n" +
                        "Disable this option if the position to spawn the item is in a Canvas (i.e: showing a message in a UI)." , MessageType.None );
                    EditorGUILayout.PropertyField( pConvertToCamera , new GUIContent( "Convert from camera" ) );

                    // Camera
                    if( pConvertToCamera.boolValue )
                    {
                        if( pCam.objectReferenceValue == null )
                        {
                            EditorGUILayout.HelpBox( "No world camera reference, will use Camera.main" , MessageType.Warning );
                            GUI.color = Color.yellow;
                        }
                        else
                        {
                            GUI.color = colorOk;
                        }
                        EditorGUILayout.PropertyField( pCam , new GUIContent( "World Camera" ) );
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox( "Enable this option if you want the text automatically to face the camera" , MessageType.None );
                    EditorGUILayout.PropertyField( pAutoFaceCameraWorldSpace , new GUIContent( "Auto face" ) );

                    if( pAutoFaceCameraWorldSpace.boolValue )
                    {
                        if( pCam.objectReferenceValue == null )
                        {
                            EditorGUILayout.HelpBox( "No camera assigned, will use Camera.main" , MessageType.Warning );
                            GUI.color = Color.yellow;
                        }
                        else
                        {
                            GUI.color = colorOk;
                        }
                        EditorGUILayout.PropertyField( pCam , new GUIContent( "Camera to face" ) );
                    }
                }
            }

            GUI.color = Color.white;
            EditorGUILayout.Space( );
            EditorGUILayout.Space( );
            EditorGUILayout.LabelField( new GUIContent( "Parameters" ) , EditorStyles.boldLabel );
            EditorGUILayout.HelpBox( "Activate follow for the text damage to always follow their target.\n(NOTE: Needs restart if parameter changed at runtime )" , MessageType.None );
            EditorGUILayout.PropertyField( pFollow );

            EditorGUILayout.Space( );
            EditorGUILayout.Space( );
            EditorGUILayout.PropertyField( pOverlap );
            EditorGUILayout.HelpBox( "Activate overlap to allow the text labels to overlap each other. Uncheck if you want the texts to auto move up.\n(NOTE: Needs restart if parameter changed at runtime )" , MessageType.None );
            if( !pOverlap.boolValue )
            {
                EditorGUILayout.Space( );
                EditorGUILayout.Space( );
                EditorGUILayout.PropertyField( pOffset );
                EditorGUILayout.HelpBox( "Gap between each text, in world units (if canvas is in world space) or screen units (if canvas is in screen space)." , MessageType.None );
                EditorGUILayout.Space( );
                EditorGUILayout.Space( );
                EditorGUILayout.PropertyField( pDamping );
                EditorGUILayout.HelpBox( "The amount of force of the text when repositioned. (Typical ranges 5-15)" , MessageType.None );
                EditorGUILayout.Space( );
            }

            EditorGUILayout.Space( );
            EditorGUILayout.Space( );
            EditorGUILayout.LabelField( new GUIContent( "Text types" ) , EditorStyles.boldLabel );
            EditorGUILayout.HelpBox( "Pool of different texts you can show. Each can have its own animation, typical use for example would be, one for normal damage, and another for criticals" , MessageType.Info );

            list.drawHeaderCallback = ( Rect rect ) =>
            {
                EditorGUI.LabelField( rect , "Text types" );
            };

            list.drawElementCallback = ( Rect rect , int index , bool isActive , bool isFocused ) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;

                EditorGUI.PrefixLabel( new Rect( rect.x , rect.y , 25 , EditorGUIUtility.singleLineHeight ) ,
                    new GUIContent( "key" )
                    );

                if( string.IsNullOrEmpty( element.FindPropertyRelative( "keyType" ).stringValue ) )
                    GUI.color = Color.red;
                else
                    GUI.color = colorOk;
                EditorGUI.PropertyField( new Rect( rect.x + 30 , rect.y , rect.width * 0.2f , EditorGUIUtility.singleLineHeight ) ,
                                            element.FindPropertyRelative( "keyType" ) , 
                                            GUIContent.none );
                GUI.color = Color.white;

                if( element.FindPropertyRelative( "prefab" ).objectReferenceValue == null )
                    GUI.color = Color.red;
                else
                    GUI.color = colorOk;

                EditorGUI.PropertyField( new Rect( rect.x + rect.width * 0.2f + 35 , rect.y , rect.width * 0.5f , EditorGUIUtility.singleLineHeight ) ,
                                            element.FindPropertyRelative( "prefab" ) , 
                                            GUIContent.none );
                GUI.color = Color.white;

                EditorGUI.PrefixLabel( new Rect( rect.x + rect.width * 0.7f + 40 , rect.y , 30  , EditorGUIUtility.singleLineHeight ) ,
                                        new GUIContent( "Count" )
                  );


                if( element.FindPropertyRelative( "poolCount" ).intValue < 0 )
                    GUI.color = Color.red;
                else if( element.FindPropertyRelative( "poolCount" ).intValue == 0 )
                    GUI.color = Color.yellow;
                else
                    GUI.color = colorOk;
                EditorGUI.PropertyField( new Rect( rect.x + rect.width * 0.7f + 80 , rect.y , rect.width * 0.1f , EditorGUIUtility.singleLineHeight ) ,
                                            element.FindPropertyRelative( "poolCount" ) , 
                                            GUIContent.none );
                GUI.color = Color.white;
            };

            list.onAddCallback = ( ReorderableList l ) => {
                var index = l.serializedProperty.arraySize;
                l.serializedProperty.arraySize++;
                l.index = index;
                var element = l.serializedProperty.GetArrayElementAtIndex(index);
                element.FindPropertyRelative( "keyType" ).stringValue = "key"+index.ToString( );
                element.FindPropertyRelative( "poolCount" ).intValue = 20;
                element.FindPropertyRelative( "prefab" ).objectReferenceValue = AssetDatabase.LoadAssetAtPath( "Assets/UltimateTextDamage/Prefabs/TextDamageItems/TextDamageItem_default.prefab" , typeof( GameObject ) ) as GameObject;
            };

            list.DoLayoutList( );
            serializedObject.ApplyModifiedProperties( );
        }
    }
}