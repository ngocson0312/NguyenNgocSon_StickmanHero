using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SuperFight
{
    public class ItemCoin : BaseItem
    {
        [SerializeField] private int minValue = 1;
        [SerializeField] private int maxValue = 10;
        [SerializeField] private TextMeshPro coinText;
        [SerializeField] private BoxCollider2D collider;

        private bool isBeEat = false;
        public float force;
        public void InitCoin(Vector3 posInit)
        {
            isBeEat = false;
            transform.position = posInit;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-force / 2, force / 2), force * 1.5f));
            coinText.gameObject.SetActive(false);
            StartCoroutine(DelayCollider());
            collider.enabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        IEnumerator DelayCollider()
        {
            yield return new WaitForSeconds(0.7f);
            collider.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer(Constant.layerMainPlayer))
            {
                beEat();
            }
        }

        private void beEat()
        {
            if (isBeEat) return;

            SoundManager.Instance.playSoundFx(SoundManager.Instance.effCollectCoin);

            isBeEat = true;
            int val = 0;
            if (GameManager.Instance.CurrLevel <= 10)
            {
                val = Random.Range(minValue, maxValue + GameManager.Instance.CurrLevel);
            }
            else if (GameManager.Instance.CurrLevel <= 20)
            {
                val = Random.Range(minValue + 10, maxValue + GameManager.Instance.CurrLevel);
            }
            else if (GameManager.Instance.CurrLevel <= 30)
            {
                val = Random.Range(minValue + 15, maxValue + GameManager.Instance.CurrLevel);
            }
            else if (GameManager.Instance.CurrLevel <= 40)
            {
                val = Random.Range(minValue + 20, maxValue + GameManager.Instance.CurrLevel);
            }
            else
            {
                val = Random.Range(minValue + 25, maxValue + GameManager.Instance.CurrLevel);
            }
            coinText.gameObject.SetActive(true);
            coinText.text = "+" + val.ToString();
            collider.enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<Animation>().Play("CoinOn");
            GameplayCtrl.Instance.coinBeEat(this, val);

        }
    }
}