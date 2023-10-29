using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace SuperFight
{
    public class ItemChest : MonoBehaviour
    {
        [SerializeField] Transform LidAnchor;
        [SerializeField] Transform CoinSpawnAnchor;
        [SerializeField] int CoinNum = 15;
        [SerializeField] int hitToOpen = 2;
        [SerializeField] ParticleSystem FXHit;
        [SerializeField] ParticleSystem FXChest;
        bool isOpen = false;
        // Start is called before the first frame update
        float time = .2f;
        float currentTime;
        private void Start()
        {
            currentTime = time;
        }
        /*private void Update()
        {
            if (isOpen) return;
            var col = Physics2D.OverlapBox(CoinSpawnAnchor.position, new Vector3(rangeX, rangeY, rangeZ),0);
            if (col == null) return;
            if (col.gameObject.layer == LayerMask.NameToLayer(Constant.layerMainPlayer))
            {
                var _isAttack = col.gameObject.GetComponent<PlayerManager>().isInteracting;
                if (!_isAttack) return;
                if (currentTime > 0)
                {
                    currentTime -= Time.deltaTime;
                    if (currentTime <= 0)
                    {
                        FXHit.Play();
                        hitToOpen--;
                        SoundManager.Instance.playRandFx(TYPE_RAND_FX.FX_TAKE_DAMAGE);
                        currentTime = time;
                    }
                }
                if (hitToOpen <= 0)
                {
                    isOpen = true;
                    OpenChest(CoinSpawnAnchor.position);
                }
            }
        }*/
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer(Constant.layerMainPlayer))
            {
                if (!isOpen)
                {
                    isOpen = true;
                    OpenChest(CoinSpawnAnchor.position);
                }
            }
        }
        void OpenChest(Vector3 position)
        {
            SoundManager.Instance.playSoundFx(SoundManager.Instance.effChestBreak);
            LidAnchor.DOLocalRotate(new Vector3(90,0,-65),.5f).OnComplete(()=> {
                FXChest.Stop();
                CreateCoinChest(position);
            });
        }
        public void CreateCoinChest(Vector3 position)
        {
            StartCoroutine(IECreateCoinBoss(position));
        }

        IEnumerator IECreateCoinBoss(Vector3 position)
        {
            float delayTime = 0;
            for (int i = 0; i < CoinNum; i++)
            {
                yield return new WaitForSeconds(delayTime);
                GameplayCtrl.Instance.objManager.createCoin(position);
                delayTime += 0.005f;
            }
        }
/*
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(CoinSpawnAnchor.position, new Vector3(rangeX, rangeY, rangeZ) );
        }*/
    }
}

