using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace SuperFight
{
    public class ItemSavePoint : BaseItem
    {
        [SerializeField] Animation animShowText;
        [SerializeField] GameObject objFire;
        [SerializeField] TextMeshPro checkPointText;

        public bool IsEnable { private set; get; }

        public Vector3 getPosReborn()
        {
            return transform.position;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(Constant.layerMainPlayer))
            {
                enableCheckPoint();
            }
        }

        public void resetSavePoint()
        {
            IsEnable = false;
            checkPointText.gameObject.SetActive(false);
            objFire.SetActive(false);
        }

        private void enableCheckPoint()
        {
            if (IsEnable) return;
            IsEnable = true;
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effSavePoint);

            checkPointText.gameObject.SetActive(true);
            objFire.SetActive(true);
            animShowText.Play("CheckPointOn");
            GameplayCtrl.Instance.setNewCheckPoint(this);

        }

    }
}