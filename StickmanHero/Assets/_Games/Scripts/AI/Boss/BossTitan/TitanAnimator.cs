using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class TitanAnimator : AnimatorHandle
    {
        public override void ResetAnimator()
        {
            ResumeAnimator();
            animator.Rebind();
        }
    }
}

