using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent[] listEvent;

    public void PlayEvent(int id)
    {
        listEvent[id].Invoke();
    }
}
