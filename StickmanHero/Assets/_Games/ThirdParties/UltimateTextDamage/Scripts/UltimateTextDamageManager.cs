using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SuperFight
{
    [System.Serializable]
    public class TextDamageType
    {
        public string keyType;
        public int poolCount = 20;
        public UITextDamage prefab;
    }

    public class UltimateTextDamageManager : Singleton<UltimateTextDamageManager>
    {
        public Canvas canvas;
        public bool convertToCamera = true;
        public Camera theCamera;

        public bool autoFaceToCamera = false;
        public bool overlaping = true;
        public bool followsTarget = false;
        public float offsetUnits = 100; // This is if no overlaping
        public float damping = 20; // This is if no overlaping

        public List < TextDamageType > textTypes;

        private Dictionary< string , List< UITextDamage > > m_dTextTypes;
        private Dictionary< Transform , List< UITextDamage > > m_instancesInScreen;
        private List< GameObject > m_tempObjects = new List<GameObject>( );
        private const int kTempObjectsCount = 30;
        
        /// <summary>
        /// Start Monobehaviours, initializes the manager with the pools
        /// </summary>
        public void Start( )
        {
            if( ( convertToCamera || autoFaceToCamera ) && theCamera == null )
                theCamera = Camera.main;

            // Allocate memory for our dictionaries
            m_instancesInScreen = new Dictionary<Transform , List<UITextDamage>>( );
            m_dTextTypes = new Dictionary<string , List<UITextDamage>>( );

            // Initialize all text types with a pool
            foreach( TextDamageType text in textTypes )
            {
                Initialize( text );
            }

            for( int i = 0 ; i < kTempObjectsCount ; i++ )
            {
                GameObject gTemp = new GameObject( "TEMP OBJECT" + i );
                gTemp.hideFlags = HideFlags.HideInHierarchy;
                gTemp.transform.SetParent( transform );
                gTemp.SetActive( false );
                m_tempObjects.Add( gTemp );
            }
        }

        /// <summary>
        /// Adds stack damage and shows the text.
        /// </summary>
        /// <param name="value">The value to stack and accumulate.</param>
        /// <param name="transform">Transform in world space where the text will be positioned.</param>
        /// <param name="stackKey">The stack key that can be used within the same item for different visual effects.</param>
        /// <param name="key">Key type.</param> 
        public void AddStack( float value , Transform target , string stackKey = "normal" , string key = "default" )
        {
            UITextDamage uiToUse = null;

            if( !m_instancesInScreen.ContainsKey( target ) )
                m_instancesInScreen.Add( target , new List<UITextDamage>( ) );

            foreach( UITextDamage currentActive in m_instancesInScreen[ target ] )
            {
                if( currentActive != null && currentActive.IsStackReusable )
                {
                    bool isSameKey = false;
                    foreach( UITextDamage text in m_dTextTypes[ key ] )
                    {
                        if( text == currentActive )
                        {
                            isSameKey = true;
                            break;
                        }
                    }

                    if( isSameKey )
                    {
                        uiToUse = currentActive;
                        break;
                    }
                }
            }

            if( uiToUse == null )
            {
                uiToUse = GetAvailableText( key );
                m_instancesInScreen[ target ].Add( uiToUse );
            }

            // Subscribe to animation end event
            uiToUse.eventOnEnd -= Label_eventOnEnd;
            uiToUse.eventOnEnd += Label_eventOnEnd;

            // Inject the transform
            uiToUse.currentTransformFollow = target;

            uiToUse.Cam = theCamera;

            // Show and set the text
            uiToUse.AddStackAndShow( value , target , stackKey );
        }

        /// <summary>
        /// Shows a desired text.
        /// </summary>
        /// <param name="text">Text to show as string</param>
        /// <param name="target">Transform for the text position to show</param>
        /// <param name="key">Key type</param>
        public void Add( string text , Transform target , string key = "default" )
        {
            // Get available text instance to use
            UITextDamage uiToUse = GetAvailableText( key );

            if( !m_instancesInScreen.ContainsKey( target ) )
                m_instancesInScreen.Add( target , new List<UITextDamage>( ) );

            m_instancesInScreen[ target ].Add( uiToUse );

            // Subscribe to animation end event
            uiToUse.eventOnEnd += Label_eventOnEnd;

            // Inject the transform
            uiToUse.currentTransformFollow = target;

            uiToUse.Cam = theCamera;

            // Show and set the text
            uiToUse.Show( text , target );
        }


        /// <summary>
        /// Shows a desired text.
        /// </summary>
        /// <param name="text">Text to show as string</param>
        /// <param name="position">Position for the text to show. Note: No overlaping and follow target won't work using this method</param>
        /// <param name="key">Key type</param>
        public void Add( string text , Vector3 position , string key = "default" )
        {
            GameObject temp = GetTempObject( );
            temp.SetActive( true );
            temp.transform.position = position;

            Add( text , temp.transform , key );
        }

        /// <summary>
        /// Instantiates one text object of the desired type
        /// </summary>
        /// <param name="text">damage type</param>
        /// <returns></returns>
        private UITextDamage AllocateOneInstance( TextDamageType text )
        {
            if( text == null )
                return null;

            GameObject g = Instantiate( text.prefab.gameObject ) as GameObject;
            g.transform.SetParent( transform );
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            g.transform.localScale = Vector3.one;

            UITextDamage td = g.GetComponent< UITextDamage >( );
            td.Canvas = this.canvas;
            td.autoFaceCameraWorldSpace = autoFaceToCamera;
            td.Cam = theCamera;
            td.followsTarget = followsTarget;
            g.SetActive( false );

            return td;
        }

        /// <summary>
        /// Initializes a pool of objects
        /// </summary>
        /// <param name="text">damage type</param>
        private void Initialize( TextDamageType text )
        {
            m_dTextTypes.Add( text.keyType , new List<UITextDamage>( ) );
            List< UITextDamage > container = m_dTextTypes[ text.keyType ];

            for( int i = 0 ; i < text.poolCount ; i++ )
            {
                container.Add( AllocateOneInstance( text ) );
            }

            // If original prefab is in the scene, disable
            if( text.prefab.gameObject.scene == UnityEngine.SceneManagement.SceneManager.GetActiveScene( ) )
                text.prefab.gameObject.SetActive( false );
        }

        private void LateUpdate( )
        {
            if( overlaping ) return;

            if( m_instancesInScreen != null )
            {
                foreach( var keypair in m_instancesInScreen )
                {
                    int i = keypair.Value.Count;
                    foreach( UITextDamage text in keypair.Value )
                    {
                        text.Offset = Mathf.Lerp( text.Offset , ( i ) * offsetUnits , Time.deltaTime * damping );
                        i--;
                    }
                }
            }
        }

        private void Label_eventOnEnd( UITextDamage obj , Transform transformFollow )
        {
            obj.eventOnEnd -= Label_eventOnEnd;

            if( m_instancesInScreen.ContainsKey( transformFollow ) )
            {
                m_instancesInScreen[ transformFollow ].Remove( obj );

                if( transformFollow )
                {
                    if( m_tempObjects.Contains( transformFollow.gameObject ) )
                        transformFollow.gameObject.SetActive( false );
                }
            }
        }

        private UITextDamage GetAvailableText( string keyType )
        {
            bool ok = m_dTextTypes.ContainsKey( keyType );

            if( !ok )
            {
                Debug.LogError( "Text Damage -> Cannot find keyType " + keyType + " on  manager " + gameObject.name );
                return null;
            }

            List< UITextDamage > candidates = m_dTextTypes[ keyType ];
            for( int i = 0 ; i < candidates.Count ; i++ )
            {
                if( candidates[ i ].gameObject.activeSelf ) continue;
                if( candidates[ i ].UsedLabel != null )
                {
                    candidates[ i ].UsedLabel.transform.localPosition = Vector3.zero;
                }
                candidates[ i ].transform.localScale = Vector3.one;
                return candidates[ i ];
            }

            // Instantiate new
            List< UITextDamage > container = m_dTextTypes[ keyType ];
            UITextDamage newInstance = AllocateOneInstance( textTypes.Find( t => t.keyType == keyType ) );
            container.Add( newInstance );

            return newInstance;
        }

        private GameObject GetTempObject( )
        {
            GameObject temp = m_tempObjects.Find( o=> o.activeSelf == false );
            if( temp == null )
            {
                temp = new GameObject( );
                temp.transform.SetParent( transform );
                temp.hideFlags = HideFlags.HideInInspector;
                temp.SetActive( false );
                m_tempObjects.Add( temp );
            }

            return temp;
        }
    }
}