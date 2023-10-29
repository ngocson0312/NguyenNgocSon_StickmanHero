using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FillBar : MonoBehaviour
{
    [SerializeField] Image fillbar;
    [SerializeField] Image shrinkBar;
    [SerializeField] float shrinkTime;
    float shrinkTimer;
    public void InitializeBar(bool doAnimation)
    {
        shrinkTimer = 0;
        if (doAnimation)
        {
            gameObject.SetActive(true);
            fillbar.fillAmount = 0;
            fillbar.DOFillAmount(1, 1f);
            if (shrinkBar != null)
            {
                shrinkBar.DOFillAmount(1, 1f);
                shrinkBar.fillAmount = 0;
            }
        }
        else
        {
            fillbar.fillAmount = 1;
            shrinkBar.fillAmount = 1;
        }
    }

    public void ResetFillBar()
    {
        fillbar.fillAmount = 1;
        shrinkBar.fillAmount = 1;
    }
    public void UpdateFillBar(float normalizeAmount)
    {
        fillbar.fillAmount = normalizeAmount;
        shrinkTimer = shrinkTime;
    }
    private void Update()
    {
        if (shrinkBar == null) return;
        if (shrinkTimer > 0)
        {
            shrinkTimer -= Time.deltaTime;
        }
        else
        {
            shrinkBar.fillAmount = Mathf.Lerp(shrinkBar.fillAmount, fillbar.fillAmount, 0.1f);
        }
    }
    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}
