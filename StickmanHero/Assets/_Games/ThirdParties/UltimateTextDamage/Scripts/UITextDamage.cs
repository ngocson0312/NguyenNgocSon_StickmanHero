using UnityEngine;
using System;
using UnityEngine.Events;
using TMPro;

#if UTD_TEXT_MESH_PRO
using TMPro;
#else
using UnityEngine.UI;
#endif

namespace SuperFight
{
    public class UITextDamage : MonoBehaviour
    {
        public enum StackToStringFormat
        {
            UTDScientificNotation ,
            ToStringFormat
        }

        [Serializable]
        public class StackUseEvent : UnityEvent<string>
        {
        }
        
        public event Action< UITextDamage , Transform > eventOnEnd;

#if UTD_TEXT_MESH_PRO
        public TextMeshProUGUI UsedLabel
#else
        public TextMeshPro UsedLabel
#endif
        {
            get
            {
#if UTD_TEXT_MESH_PRO
                return labelTMP;
#else
                return label;
#endif
            }
        }

        [ Header( "The Text ui of the item" )]
#if UTD_TEXT_MESH_PRO
        public TextMeshProUGUI labelTMP;
#else
        public TextMeshPro label;
#endif

        [Header( "Damage Stack options" )]
        public StackToStringFormat stackToStringType;
        public string toStringFormat = "0";
        public float stackReusableTime = 0.2f;
        public StackUseEvent onItemStackUse;

        public bool autoFaceCameraWorldSpace { get; set; }
        public Canvas Canvas { get; set; }
        public float Offset { get; set; }
        public Camera Cam { get; set; }
        public bool followsTarget { get; set; }
        public Transform currentTransformFollow { get; set; }
        public bool IsStackReusable => m_IsStackReUsable;

        private RectTransform rectTransform;
        private Transform toFollow;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private float m_accumulate = 0;
        private bool m_IsStackReUsable = true;

        private bool firstTime;
        private float m_currentTime = 0;

        /// <summary>
        /// Start Monobehaviour
        /// </summary>
        void Start( )
        {
            rectTransform = transform as RectTransform;
        }

        /// <summary>
        /// Shows the ui text
        /// </summary>
        /// <param name="text">string that will be filled</param>
        /// <param name="transform">Transform in world space where the text will be positioned</param>
        public void Show( string text , Transform transform )
        {
            if( UsedLabel != null )
                UsedLabel.text = text;
            Offset = 0;
            toFollow = transform;
            firstTime = true;
            m_currentTime = 0;
            if( !gameObject.activeSelf )
                gameObject.SetActive( true );
        }

        /// <summary>
        /// Adds stack damage and shows the text.
        /// </summary>
        /// <param name="value">The value to stack and accumulate</param>
        /// <param name="transform">Transform in world space where the text will be positioned</param>
        /// <param name="stackKey">The stack key that can be used within the same item for different visual effects.</param>
        public void AddStackAndShow( float value , Transform transform , string stackKey )
        {
            m_accumulate += value;
            m_IsStackReUsable = true;

            if( UsedLabel != null )
            {
                if( stackToStringType == StackToStringFormat.UTDScientificNotation )
                    UsedLabel.text = m_accumulate.ToStringScientific( );
                else if( stackToStringType == StackToStringFormat.ToStringFormat )
                    UsedLabel.text = m_accumulate.ToString( toStringFormat );
            }

            if( !gameObject.activeSelf )
            {
                toFollow = transform;
                Offset = 0;
                firstTime = true;
                gameObject.SetActive( true );

                Color a = label.color;
                a.a = 1;
                label.color = a;

            }

            onItemStackUse.Invoke( stackKey );

            m_currentTime = 0;
        }

        private void OnValidate( )
        {
#if UTD_TEXT_MESH_PRO
            labelTMP = GetComponentInChildren<TextMeshProUGUI>( );
#else
            /*label =GetComponentInChildren<TextMeshPro>( );
            label.sortingOrder = 100;*/
#endif
        }

        /// <summary>
        /// LateUpdate from Monobehaviour
        /// </summary>
        void LateUpdate( )
        {
            if( !toFollow ) return;

            UpdatePosition( );
        }

        private void Update( )
        {
            if( m_IsStackReUsable )
            {
                m_currentTime += Time.deltaTime;

                if( m_currentTime >= stackReusableTime )
                {
                    m_IsStackReUsable = false;
                }
            }
        }

        /// <summary>
        /// Animation event, must call this at the end of the text animation.
        /// </summary>
        public void End( )
        {
            m_accumulate = 0;
            if( eventOnEnd != null ) eventOnEnd( this , currentTransformFollow );
            gameObject.SetActive( false );
        }

        /// <summary>
        /// Updates the position of the text
        /// </summary>
        private void UpdatePosition( )
        {
            Vector3 uiWorldPos = toFollow.position;

            if( firstTime )
            {
                initialPosition = uiWorldPos;
                initialRotation = toFollow.rotation;
            }

            if( Canvas.renderMode == RenderMode.WorldSpace )
            {
                if( !followsTarget )
                {
                    uiWorldPos = initialPosition;
                }

                transform.position = uiWorldPos + Vector3.up * Offset;

                if( autoFaceCameraWorldSpace && Cam != null )
                {
                    Vector3 dir = Cam.transform.position - transform.position;
                    transform.rotation = Quaternion.LookRotation( dir.normalized  , Cam.transform.up );
                }
                else
                {
                    transform.rotation = initialRotation;
                }

                transform.localRotation *= Quaternion.Euler( 0 , 180 , 0 );
            }
            else
            {
                if( !followsTarget )
                {
                    uiWorldPos = initialPosition;
                }

                if( Cam != null )
                {
                    Vector2 screenPoint;
                    screenPoint = Cam.WorldToScreenPoint( uiWorldPos );
                    Vector2 output;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle( transform.parent as RectTransform , screenPoint + Vector2.up * Offset , Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Canvas.worldCamera , out output );
                    rectTransform.anchoredPosition3D = output;
                }
                else
                {
                    rectTransform.anchoredPosition3D = ( Canvas.transform as RectTransform ).InverseTransformPoint( uiWorldPos ) + Vector3.up * Offset;
                }
            }

            transform.SetAsLastSibling( );

            firstTime = false;
        }
    }
}
