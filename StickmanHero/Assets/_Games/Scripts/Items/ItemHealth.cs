using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SuperFight
{
    public class ItemHealth : BaseItem
    {
        [SerializeField] private float valuePercent = 0.5f;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer(Constant.layerMainPlayer))
            {
                PlayerManager.Instance.input.ResetInput();
                beEat();
            }
        }

        private void beEat()
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effItemHealthTouch);
            GameplayCtrl.Instance.itemHealthBeEat(this, valuePercent);
        }
    }
}