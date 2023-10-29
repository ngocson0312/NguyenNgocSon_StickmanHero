using System.Collections.Generic;
using mygame.sdk;
using UnityEngine;
using UnityEngine.UI;

public class ListOtherGame : MonoBehaviour
{
    public GameObject obList;
    public Sprite spGameDef;
    public List<PromoGame> listGames = new List<PromoGame>();
    private void Awake()
    {
        for (int i = 0; i < listGames.Count; i++)
        {
            listGames[i].transform.GetChild(0).GetComponent<RectTransform>().anchorMin = new Vector2(0.06875f, 0.09375001f);
            listGames[i].transform.GetChild(0).GetComponent<RectTransform>().anchorMax = new Vector2(0.9312501f, 0.9312501f);
            listGames[i].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            listGames[i].transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            listGames[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = spGameDef;
            var Textad = listGames[i].transform.GetChild(1);
            Textad.transform.SetSiblingIndex(1);
            //Textad.GetComponent<RectTransform>().anchorMin = new Vector2(0.10625f, 0.7f);
            //Textad.GetComponent<RectTransform>().anchorMax = new Vector2(0.35625f, 0.9312501f);
            //Textad.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            //Textad.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        }
    }

    private void OnEnable()
    {
        showListOther();
    }

    void showListOther()
    {
        var list = FIRhelper.Instance.getListGamePromo(3);
        if (list != null && list.Count > 0)
        {
            obList.SetActive(true);
            for (int i = 0; i < listGames.Count; i++)
            {
                if (i < list.Count)
                {
                    listGames[i].intitGame(list[i]);
                    listGames[i].gameObject.SetActive(true);
                    listGames[i].show(null);
                }
                else
                {
                    listGames[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            obList.SetActive(false);
        }
    }
}
