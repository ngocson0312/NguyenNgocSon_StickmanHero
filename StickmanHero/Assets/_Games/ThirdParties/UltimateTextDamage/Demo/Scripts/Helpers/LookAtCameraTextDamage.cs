using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guirao.UltimateTextDamage
{
    public class LookAtCameraTextDamage : MonoBehaviour
    {
        public Transform trCamera;
       
        private void LateUpdate( )
        {
            Vector3 dir = trCamera.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation( dir.normalized );
        }
    }
}
