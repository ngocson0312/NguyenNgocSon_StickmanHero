using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public abstract class CoreComponent : MonoBehaviour
    {
        protected Core core;
        public virtual void Initialize(Core core)
        {
            this.core = core;
        }
    }
}

