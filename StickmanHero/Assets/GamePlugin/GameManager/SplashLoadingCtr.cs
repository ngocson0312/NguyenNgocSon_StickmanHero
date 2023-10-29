using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mygame.sdk;

public class SplashLoadingCtr : MonoBehaviour
{
	private const float perStep = 0.75f;
    public Image imgFill;
    public Text txtFill;
    private int isWaitConfig = 0;
    private float toTimeWait = 2;
    private float minWait = 2;
    private int stateGetCf = 0;
    float timestep1 = 0;
    float timestep2 = 0;
    float timeRun = 0;
    float kTime = 1;
    // Start is called before the first frame update
    public void showLoading(float time = 2, int isWaitCondition = 0, float minwait = 2)
    {
        RectTransform rc = GetComponent<RectTransform>();
        rc.anchorMin = new Vector2(0, 0);
        rc.anchorMax = new Vector2(1, 1);
        rc.sizeDelta = new Vector2(0, 0);
        rc.anchoredPosition = Vector2.zero;
        rc.anchoredPosition3D = Vector3.zero;
        SDKManager.Instance.isAllowShowFirstOpen = false;

        toTimeWait = time;
        this.minWait = minwait;
        if (this.minWait < 2)
        {
            this.minWait = 2;
        }
        isWaitConfig = isWaitCondition;
        stateGetCf = 0;
        if (isWaitConfig == 1)
        {
            if (GameHelper.checkLvXaDu())
            {
                toTimeWait = this.minWait;
            }
        }
        if (toTimeWait < 2)
        {
            toTimeWait = 2;
        }

        if (isWaitConfig != 0)
        {
            FIRhelper.CBGetconfig += onGetConfig;
        }
        timestep1 = perStep * toTimeWait;
        timestep2 = toTimeWait - timestep1;
        timeRun = 0;
        imgFill.fillAmount = 0;
        txtFill.text = "0%";
        kTime = 1;
    }

    public void onGetConfig()
    {
        stateGetCf = 1;
    }

    private void Update()
    {
        if (stateGetCf == 1 && timeRun >= minWait)
        {
            stateGetCf = 2;
            kTime = 3;
        }
        timeRun += Time.deltaTime * kTime;
        if (timeRun <= timestep1)
        {
            imgFill.fillAmount = (timeRun * 0.9f / perStep) / toTimeWait;
        }
        else
        {
            imgFill.fillAmount = 0.9f + 0.1f * (timeRun - timestep1) / (toTimeWait * (1.0f - perStep));
        }
        int nf = (int)(imgFill.fillAmount * 100);
        if (nf > 100)
        {
            nf = 100;
        }
        txtFill.text = "" + nf + "%";

        if (timeRun >= toTimeWait)
        {
            txtFill.text = "100%";
            SDKManager.Instance.isAllowShowFirstOpen = true;
            SDKManager.Instance.onSplashFinishLoding();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
