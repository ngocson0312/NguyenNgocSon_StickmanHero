using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SuperFight
{
    public class ItemSawTrap : MonoBehaviour
    {
        [Header("Custome")]
        public float timeRotate = 0.25f;
        public float timeMoveDuration = 2f;
        public float delayTime = 1f;
        [SerializeField] Transform SawAnchor;
        [SerializeField] Transform StartAnchor;
        [SerializeField] Transform EndAnchor;
        private void Start()
        {
            SawAnchor.DORotate(Vector3.forward * 360, timeRotate, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            TrapMove();
        }
        public void TrapMove()
        {
            SawAnchor.DOLocalMove(EndAnchor.localPosition, timeMoveDuration).SetEase(Ease.Linear).SetDelay(delayTime).OnComplete(() =>
            {
                SawAnchor.DOLocalMove(StartAnchor.localPosition, timeMoveDuration).SetEase(Ease.Linear).SetDelay(delayTime).OnComplete(() =>
                {
                    TrapMove();
                });
            });
        }
    }
}

