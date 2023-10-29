using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame.sdk;

public class WrapperBtOtherGame : MonoBehaviour
{
    private long timeShoPromogame = 0;
    [SerializeField] private PromoGame gameOther;
    private PromoGameOb gameProCurr = null;
    bool isfirst = true;

    private void OnEnable()
    {
        if (!isfirst)
        {
            showPromoGame();
        }
    }

    void Start()
    {
        isfirst = false;
        showPromoGame();
    }

    #region gamePromo
    public void showPromoGame()
    {
        Debug.Log("promo: showPromoGame 0");
        gameOther.gameObject.SetActive(false);
        PromoGameOb gamePro = null;
        if (timeShoPromogame <= 0)
        {
            Debug.Log("promo: showPromoGame 10");
            gamePro = FIRhelper.Instance.getGamePromo();
            if (gamePro != null)
            {
                Debug.Log("promo: showPromoGame 100");
                timeShoPromogame = SdkUtil.systemCurrentMiliseconds();
            }
        }
        else
        {
            Debug.Log("promo: showPromoGame 11");
            long t = SdkUtil.systemCurrentMiliseconds();
            if ((t - timeShoPromogame) >= 90 * 1000)
            {
                Debug.Log("promo: showPromoGame 110");
                gamePro = FIRhelper.Instance.nextGamePromo();
                if (gamePro != null)
                {
                    Debug.Log("promo: showPromoGame 1100");
                    timeShoPromogame = t;
                }
                else
                {
                    Debug.Log("promo: showPromoGame 1101");
                    gamePro = gameProCurr;
                }
            }
            else
            {
                Debug.Log("promo: showPromoGame 1103");
                gamePro = gameProCurr;
            }
        }

        if (gamePro != null)
        {
            Debug.Log("promo: showPromoGame 2");
            string nameimg = ImageLoader.url2nameData(gamePro.getImg(0), 1);
            Texture2D tt = null;
            if (System.IO.File.Exists(DownLoadUtil.pathCache() + "/" + nameimg))
            {
                Debug.Log("promo: showPromoGame 20");
                tt = ImageLoader.Instance.loadTexttureFromCache(nameimg, 100, 100);
            }
            if (tt != null)
            {
                Debug.Log("promo: showPromoGame 210");
                gameProCurr = gamePro;
                gameOther.intitGame(gameProCurr);
                gameOther.show(ImageLoader.CreateSprite(tt));
            }
            else
            {
                Debug.Log("promo: showPromoGame 211");
                PromoGameOb gamehasIcon = FIRhelper.Instance.getGameHasIcon();
                if (gamehasIcon != null)
                {
                    Debug.Log("promo: showPromoGame 2110");
                    nameimg = ImageLoader.url2nameData(gamehasIcon.getImg(0), 1);
                    tt = ImageLoader.Instance.loadTexttureFromCache(nameimg, 100, 100);
                    gameProCurr = gamehasIcon;
                    gameOther.intitGame(gameProCurr);
                    gameOther.show(ImageLoader.CreateSprite(tt));
                }
                else
                {
                    Debug.Log("promo: showPromoGame 2111");
                    gameProCurr = gamePro;
                    gameOther.intitGame(gameProCurr);
                    gameOther.show(null);
                }
            }
        }
    }
    #endregion
}
