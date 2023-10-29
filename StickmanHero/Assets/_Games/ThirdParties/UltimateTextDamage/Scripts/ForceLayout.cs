using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperFight
{
    public class ForceLayout : MonoBehaviour
    {
        void OnEnable( )
        {
            foreach( ContentSizeFitter contentSizeFitter in GetComponentsInChildren<ContentSizeFitter>( true ) )
            {
                contentSizeFitter.enabled = false;
                contentSizeFitter.enabled = true;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate( transform as RectTransform );
        }
    }
}