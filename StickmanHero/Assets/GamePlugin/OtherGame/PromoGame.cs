using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace mygame.sdk
{
    public class PromoGame : MonoBehaviour
    {
        PromoGameOb gameOb;
        public Button uiBt;
        public Image uiImg;
        int idxImg = 0;
        bool isani = false;
        bool isLoadOk = false;

        private void OnEnable()
        {
            if (isLoadOk)
            {
#if ENABLE_AppsFlyer
                Dictionary<string, string> ParamPromo = new Dictionary<string, string>();
                ParamPromo.Add("af_promo_appid", AppConfig.appid);
                AppsFlyerSDK.AppsFlyer.recordCrossPromoteImpression(gameOb.appid, "cross_promo_campaign", ParamPromo);
#endif
            }
        }
        public void intitGame(Button bt, Image img, PromoGameOb g)
        {
            uiBt = bt;
            uiImg = img;
            intitGame(g);
        }

        public void intitGame(PromoGameOb g)
        {
            gameOb = g;
            idxImg = 0;
            isLoadOk = false;
            uiBt.onClick.RemoveAllListeners();
            uiBt.onClick.AddListener(() =>
            {
                if (gameOb != null)
                {
                    onclickOtherGame();
                }
            });
        }

        public void show(Sprite sp)
        {
            if (uiImg != null && gameOb != null && gameOb.listImgs.Count > 0)
            {
                if (sp == null)
                {
                    ImageLoader.LoadImageSprite(gameOb.listImgs[0], 100, 100, (sprite) =>
                    {
                        isLoadOk = true;
                        gameObject.SetActive(true);
                        uiImg.sprite = sprite;
                        gameOb.ListTT.Clear();
                        gameOb.ListTT.Add(sprite);
                        loadNextImgs();
                    });
                }
                else
                {
                    isLoadOk = true;
                    uiImg.sprite = sp;
                    gameOb.ListTT.Clear();
                    gameOb.ListTT.Add(uiImg.sprite);
                    gameObject.SetActive(true);
                    loadNextImgs();
                }
            }
        }

        void loadNextImgs()
        {
            isani = false;
            if (gameOb != null && gameOb.listImgs.Count > 1)
            {
                for (int i = 1; i < gameOb.listImgs.Count; i++)
                {
                    int mi = i;
                    gameOb.ListTT.Add(null);
                    ImageLoader.LoadImageSprite(gameOb.listImgs[mi], 100, 100, (sprite) =>
                    {
                        if (mi < gameOb.ListTT.Count)
                        {
                            gameOb.ListTT[mi] = sprite;
                        }
                        else
                        {
                            gameOb.ListTT.Add(sprite);
                        }
                        bool isall = true;
                        for (int j = 0; j < gameOb.ListTT.Count; j++)
                        {
                            if (gameOb.ListTT[j] == null)
                            {
                                isall = false;
                                break;
                            }
                        }
                        if (isall && !isani)
                        {
                            isani = true;
                            idxImg = 0;
                            StartCoroutine(aniGame());
                        }
                    });
                }
            }
        }

        IEnumerator aniGame()
        {
            uiImg.sprite = gameOb.ListTT[idxImg];
            yield return new WaitForSeconds(0.3f);
            idxImg++;
            if (idxImg >= gameOb.ListTT.Count)
            {
                idxImg = 0;
                yield return new WaitForSeconds(3.5f);
            }
            StartCoroutine(aniGame());
        }

        void onclickOtherGame()
        {
            if (gameOb != null)
            {
                FIRhelper.Instance.onClickPromoGame(gameOb);
            }
        }
    }
    public class PromoGameOb
    {
        public string name = "";
        public string pkg = "";
        public string appid = "";
        string icons = "";
        public string link = "";
        public string des = "";
        public List<string> listImgs = new List<string>();
        public List<Sprite> ListTT = new List<Sprite>();

        public void setImg(string dli)
        {
            icons = dli;
            listImgs.Clear();
            string[] arrtype = icons.Split(',');
            for (int i = 0; i < arrtype.Length; i++)
            {
                listImgs.Add(arrtype[i]);
            }
        }

        public string getImg(int idx)
        {
            if (listImgs.Count > idx)
            {
                return listImgs[idx];
            }
            return "";
        }

        public string getImgs()
        {
            return icons;
        }
    }
}