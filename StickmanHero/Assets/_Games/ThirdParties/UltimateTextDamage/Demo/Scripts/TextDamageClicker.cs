using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperFight;
namespace Guirao.UltimateTextDamage
{
    public class TextDamageClicker : MonoBehaviour
    {
        public UltimateTextDamageManager textManager;
        public Transform overrideTransform;
        public bool onlyDefault = false;
        public float minValue = 450;
        public float maxValue = 2000;
        public string[] optionsToShow = null;

        private void OnMouseUpAsButton( )
        {
            if( !onlyDefault && Random.value < 0.1f )
            {
                string randomStatus = "BLEEDING";
                int r = Random.Range( 0 , 2 );
                if( r == 0 )
                    randomStatus = "STUN";
                else
                    randomStatus = "CURSED";
                textManager.Add( randomStatus , overrideTransform != null ? overrideTransform : transform , "status" );
            }
            else
            {
                if( optionsToShow != null && optionsToShow.Length > 0 )
                {
                    textManager.Add( optionsToShow[ Random.Range( 0 , optionsToShow.Length ) ] , overrideTransform != null ? overrideTransform : transform );
                }
                else
                {
                    textManager.Add(  Random.Range( minValue , maxValue ).ToStringScientific( ) , overrideTransform != null ? overrideTransform : transform );
                }
            }
        }

        public bool autoclicker = true;
        public float clickRate = 1;

        float lastTimeClick;
        private void Update( )
        {
            if( !autoclicker )
                return;

            if( Time.time - lastTimeClick >= 1f / clickRate )
            {
                lastTimeClick = Time.time; //+ Random.value * 0.5f;
                OnMouseUpAsButton( );
            }
        }

        public void ForceAdd( )
        {
            OnMouseUpAsButton( );
        }
    }
}
