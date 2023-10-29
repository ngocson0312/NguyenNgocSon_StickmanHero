using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SuperFight
{
    public class ItemSpikeTrap : MonoBehaviour
    {
        [SerializeField] Spike[] spikes;
        [SerializeField] int dam = 50;
        [SerializeField] Transform SpikeAnchor;
        [SerializeField] Transform StartAnchor;
        [SerializeField] Transform EndAnchor;
        [SerializeField] float timeDelayUP = 1f;
        [SerializeField] float timeDelayDown = 1f;
        [SerializeField] float timeUp = .35f;
        [SerializeField] float timeDown = 1.5f;
        [SerializeField] Ease ease = Ease.InQuart;
        Tweener twMoveUP;
        Tweener twMoveDown;
        private void OnEnable()
        {
            SpikeMove();
            for (int i = 0; i < spikes.Length; i++)
            {
                if (spikes[i] != null)
                {
                    spikes[i].damage = dam;
                }
            }
        }

        void SpikeMove()
        {
            twMoveDown = SpikeAnchor.DOLocalMoveY(EndAnchor.localPosition.y, timeDown).SetEase(ease).SetDelay(timeDelayDown).OnComplete(() => {
                twMoveUP = SpikeAnchor.DOLocalMoveY(StartAnchor.localPosition.y, timeUp).SetEase(ease).SetDelay(timeDelayUP).OnComplete(() => {
                    KillTween();
                    SpikeMove();
                });
            });
        }
        void KillTween()
        {
            twMoveDown.Kill();
            twMoveUP.Kill();
        }
        private void OnDisable()
        {
            KillTween();
        }
    }
}

