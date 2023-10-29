using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace SuperFight
{
    public class Spin : MonoBehaviour
    {
        public LuckWheelPopup spinPopup;
        public float spinDuration;
        public bool _isSpinning;
        public GiftBox[] box;
        float randomRotateAngle;
        float pieceAngle;
        float pieceHalfAngle;
        public bool cheat;
        public int getSkin1;
        public int getSkin2;
        public Transform RotateAnchor;
        private void Start()
        {
            pieceAngle = 360 / 8;
            pieceHalfAngle = pieceAngle / 2f;
            randomRotateAngle = 600f * spinDuration;
            SetIdGiftBox();
        }
        void SetIdGiftBox()
        {
            for (int i = 0; i < box.Length; i++)
            {
                box[i].id = i;
            }
        }

        public void OnSpin()
        {
            if (!_isSpinning)
            {
                SoundManager.Instance.playSoundFx(SoundManager.Instance.effSpin);
                _isSpinning = true;
                Vector3 AngleRotate = Vector3.back * Random.Range(randomRotateAngle, randomRotateAngle + 360f);
                if (cheat)//
                {
                    var _angle = AngleRotate.z % 360;
                    Debug.Log("angle:" + _angle);
                    if (spinPopup.spinCount == getSkin1 - 1)
                    {
                        //Cant get skin
                        if (spinPopup.spinCount == getSkin2 - 1)
                        {
                            //YellowBox
                            AngleRotate = new Vector3(0, 0, 10 + ((int)AngleRotate.z / 360) * 360);
                        }
                        else if (spinPopup.spinCount == getSkin1 - 1)
                        {
                            //VioletBox
                            AngleRotate = new Vector3(0, 0, 200 + ((int)AngleRotate.z / 360) * 360);
                        }
                    }
                    else
                    {
                        //Can get skin
                        if ((_angle > 0 && _angle < 22.5 || _angle < 360 && _angle > 337.5) || (_angle < 0 && _angle > -22.5 || _angle > -360 && _angle < -337.5))
                        {
                            //YellowBox
                            AngleRotate += new Vector3(0, 0, -45);
                        }
                        else if (_angle > 157.5 && _angle < 202.5 || _angle < -157.5 && _angle > -202.5)
                        {
                            //VioletBox
                            AngleRotate += new Vector3(0, 0, -45);
                        }
                    }
                }
                for (int i = 0; i < box.Length; i++)
                {
                    box[i].transform.DOLocalRotate(-AngleRotate, spinDuration, RotateMode.Fast).SetEase(Ease.OutFlash);
                }
                RotateAnchor.DORotate(AngleRotate, spinDuration, RotateMode.Fast).SetEase(Ease.OutQuart)
                 .OnComplete(() => {
                     float angle = RotateAnchor.eulerAngles.z + pieceHalfAngle;
                     int index = ((int)((angle * 8f) / 360f)) % 8;
                     _isSpinning = false;
                     spinPopup.idBox = index;
                     SoundManager.Instance.stopSoundEff();
                     box[index].ClaimGift();
                     spinPopup.spinCount++;
                 });
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                OnSpin();
            }
        }
    }
    
}
