using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace SuperFight
{
    public class ShowPromoPackController : Singleton<ShowPromoPackController>
    {
        public int CurrentDay(string _dataTime)
        {
            DateTime epochStart = new DateTime(long.Parse(_dataTime));
            int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalDays;
            return currentEpochTime;
        }
        #region Timer
        int timeRemain = 2 * 3600;
        float _timer = 1;
        #endregion
        private void Start()
        {
            SetTimeRemain();
        }
        private void Update()
        {
            Timer();
        }
        public void DisPlayTime(TextMeshProUGUI _textMeshTimer)
        {
            _textMeshTimer.text = DisPlayTime(timeRemain);
        }
        void Timer()
        {
            if (timeRemain > 0)
            {
                if (_timer > 0)
                {
                    _timer -= Time.deltaTime;
                    if (_timer <= 0)
                    {
                        timeRemain--;
                        _timer = 1;
                    }
                }
            }
            else
            {
                SetTimeRemain();
            }
        }
        string DisPlayTime(int _second)
        {
            int hour = TimeSpan.FromSeconds(_second).Hours;
            int min = TimeSpan.FromSeconds(_second).Minutes;
            int sec = TimeSpan.FromSeconds(_second).Seconds;
            return string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
        }
        void SetTimeRemain()
        {
            timeRemain = ((24 - DateTime.Now.Hour) * 60 - DateTime.Now.Minute) * 60;
        }
    }
}

