using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SuperFight
{
    public class HealBar : MonoBehaviour
    {
        [SerializeField] Image imgHeal;
        [SerializeField] Image imgHealEffect;
        [SerializeField] float timeDelay;
        [SerializeField] float timeDuration;
        [SerializeField] Ease typeEaseHeal;
        [SerializeField] Ease typeEaseHealEffect;

        [SerializeField] float fillAmountPerHeal;

        public float testDamageTake;
        // private void Start()
        // {
        //     fillAmountPerHeal = (float)1 / GetPlayerHeal();
        // }
        public void ResetHealBar()
        {
            fillAmountPerHeal = (float)1 / GetPlayerHeal();
            tweenerHeal.Kill();
            tweenerHealEffect.Kill();
            imgHeal.fillAmount = 1;
            imgHealEffect.fillAmount = 1;
        }
        public float GetPlayerHeal()
        {
            return 1;
            // return PlayerManager.Instance.playerStats.stats.maxHealth;
        }
        Tweener tweenerHeal;
        Tweener tweenerHealEffect;
        public void TakeDamge(float _heal)
        {
            var fill = _heal * fillAmountPerHeal;
            tweenerHeal = imgHeal.DOFillAmount(fill, timeDuration).SetEase(typeEaseHeal);
            tweenerHealEffect = imgHealEffect.DOFillAmount(fill, timeDuration).SetEase(typeEaseHealEffect).SetDelay(timeDelay);
        }

        public void OnHealing(float _heal)
        {
            var fill = _heal * fillAmountPerHeal;
            tweenerHeal = imgHeal.DOFillAmount(fill, timeDuration).SetEase(typeEaseHeal);
            tweenerHealEffect = imgHealEffect.DOFillAmount(fill, timeDuration/2).SetEase(typeEaseHealEffect);
        }
    }
}

