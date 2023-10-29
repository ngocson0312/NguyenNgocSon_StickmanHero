using UnityEngine;
using mygame.sdk;
using UnityEngine.UI;

public class PopupRateCtl : MonoBehaviour
{
    [SerializeField] GameObject[] stars;
    int countStar = 0;

    public Button star1;
    public Button star2;
    public Button star3;
    public Button star4;
    public Button star5;
    public Button Rate;
    public Button Close;

    private void Awake()
    {
        star1.onClick.AddListener(() => onclickStar(1));
        star2.onClick.AddListener(() => onclickStar(2));
        star3.onClick.AddListener(() => onclickStar(3));
        star4.onClick.AddListener(() => onclickStar(4));
        star5.onClick.AddListener(() => onclickStar(5));
        Rate.onClick.AddListener(onClickRate);
        Close.onClick.AddListener(onClose);
    }

    private void OnEnable()
    {
        countStar = 0;
        for (int i = 0; i < 5; i++)
        {
            stars[i].SetActive(i < countStar);
        }
    }
    public void onclickStar(int count)
    {
        countStar = count;
        for (int i = 0; i < 5; i++)
        {
            stars[i].SetActive(i < count);
        }
    }
    public void onClose()
    {
        // AudioManager.Instance.PlayOneShot("click");
        gameObject.SetActive(false);
    }
    public void onClickRate()
    {
        // AudioManager.Instance.PlayOneShot("click");
        if (countStar >= 4)
        {
            GameHelper.Instance.rate();
        }
        PlayerPrefs.SetInt("is_show_rate", 1);
        gameObject.SetActive(false);
    }

    // void CallPopupWin()
    // {
    //     GameManager.Instance.StartCoroutine(GameManager.Instance.DelayShowPopup(1, 1));
    // }
}
