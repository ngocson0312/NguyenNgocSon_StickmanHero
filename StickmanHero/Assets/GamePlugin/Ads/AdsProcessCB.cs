using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mygame.sdk
{
    public class AdsCBPro
    {
        public AdsCBPro(Action action, float tdl = 0)
        {
            cb = action;
            timeDelay = (long)(tdl * 1000);
            tAddCb = SdkUtil.systemCurrentMiliseconds();
        }
        public Action cb;
        public long timeDelay = 0;
        public long tAddCb = 0;
    }
    public class AdsProcessCB : MonoBehaviour
    {
        private static readonly object queueLock = new object();
        private static List<AdsCBPro> _executionQueue = new List<AdsCBPro>();

        private static AdsProcessCB _instance;

        long timecall = 0;
        long tcurr = 0;

        public void Update()
        {
            //lock (queueLock)
            {
                if (_executionQueue != null && _executionQueue.Count > 0)
                {
#if ENABLE_MYLOG
                timecall = SdkUtil.systemCurrentMiliseconds();
#endif
                    tcurr = SdkUtil.systemCurrentMiliseconds();
                    for (int i = 0; i < _executionQueue.Count; i++)
                    {
                        if (_executionQueue[i].timeDelay == 0 || (tcurr - _executionQueue[i].tAddCb) >= _executionQueue[i].timeDelay)
                        {
                            AdsCBPro ac = _executionQueue[i];
                            _executionQueue.RemoveAt(i);
                            i--;
                            ac.cb?.Invoke();
                        }
                    }
#if ENABLE_MYLOG
                SdkUtil.logd("ads AdsProcessCB Update1=" + (tcurr - timecall));
#endif
                }
            }
        }

        public void Enqueue(Action action)
        {
            //lock (queueLock)
            {
                AdsCBPro acb = new AdsCBPro(action);
                _executionQueue.Add(acb);
            }
        }

        public void Enqueue(Action action, float time)
        {
            //lock (queueLock)
            {
                AdsCBPro acb = new AdsCBPro(action, time);
                _executionQueue.Add(acb);
            }
        }

        public static bool Exists()
        {
            return _instance != null;
        }

        public static AdsProcessCB Instance()
        {
            if (!Exists())
                throw new Exception(
                    "AdsProcessCB could not find the AdsProcessCB object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
            return _instance;
        }


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {

            }
        }
    }
}