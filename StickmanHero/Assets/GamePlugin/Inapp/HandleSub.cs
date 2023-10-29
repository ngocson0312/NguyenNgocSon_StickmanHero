using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame.sdk;
#if ENABLE_INAPP
using UnityEngine.Purchasing;
#endif

public class HandleSub : MonoBehaviour
{
    public static HandleSub Instance = null;

    private Dictionary<string, SubResultOb> listSubs = new Dictionary<string, SubResultOb>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void checkStartSub()
    {
        CheckHasSubscription();
    }

    public void checkHandSub()
    {
        handleFirstSub();
        handleDailySub();
    }

    void handleFirstSub()
    {
        foreach (var item in listSubs)
        {
            Debug.Log($"mysdk: inapp HandleSub handleFirstSub:{item.Key} status={item.Value.statusSub}, rcv={item.Value.countRcv}");
            if (item.Value.statusSub == 1)
            {
                item.Value.statusSub = 2;
                if (item.Value.countRcv == 0)
                {
                    Debug.Log($"mysdk: inapp HandleSub handleFirstSub: RCV {item.Value.skuId}");
                    item.Value.countRcv = 1;
                    item.Value.dayDailyRcv = SdkUtil.systemCurrentMiliseconds() / 1000;
                    InappHelper.Instance.handleSub(item.Value.skuId, 1);
                }
                item.Value.saveData();
            }
        }
    }
    void handleDailySub()
    {
        foreach (var item in listSubs)
        {
            Debug.Log($"mysdk: inapp HandleSub handleDailySub:{item.Key} status={item.Value.statusSub}, rcv={item.Value.countRcv}");
            if (item.Value.statusSub > 0 && item.Value.countRcv >= 0)
            {
                if (isRcvSubOfDay(item.Value.dayDailyRcv) && item.Value.countRcv < item.Value.objInapp.periodSub)
                {
                    item.Value.countRcv++;
                    item.Value.dayDailyRcv = SdkUtil.systemCurrentMiliseconds() / 1000;
                    Debug.Log($"mysdk: inapp HandleSub handleDailySub: RCV {item.Value.skuId} dayDailyRcv={item.Value.dayDailyRcv}");
                    if (item.Value.countRcv >= item.Value.objInapp.periodSub)
                    {
                        item.Value.statusSub = 0;
                    }
                    item.Value.saveData();
                    int dayrcv = SdkUtil.SubDay(item.Value.dayDailyRcv, item.Value.purchaseDate) + 1;
                    InappHelper.Instance.handleSub(item.Value.skuId, dayrcv);
                }
            }
        }
    }
 
#if ENABLE_INAPP
    public void onBuySubSuccess(PkgObject obinapp, Product pro)
    {
        SubResultOb reSub;
        if (listSubs.ContainsKey(obinapp.id))
        {
            Debug.Log($"mysdk: inapp HandleSub onBuySubSuccess 1:{obinapp.id}");
            reSub = listSubs[obinapp.id];
            if (!isExpired(obinapp.id))
            {
                return;
            }
        }
        else
        {
            Debug.Log($"mysdk: inapp HandleSub onBuySubSuccess 2:{obinapp.id}");
            reSub = new SubResultOb(obinapp.id, obinapp);
            listSubs.Add(obinapp.id, reSub);
        }
        reSub.statusSub = 1;
        reSub.countRcv = 0;
        reSub.dayDailyRcv = 0;
        reSub.purchaseDate = SdkUtil.systemCurrentMiliseconds() / 1000;
        reSub.purchaseExpire = reSub.purchaseDate + obinapp.periodSub * 24 * 3600;
        Debug.Log($"mysdk: inapp HandleSub onBuySubSuccess dayDailyRcv={reSub.dayDailyRcv}, purchaseDate={reSub.purchaseDate}, reSub.purchaseExpire={reSub.purchaseExpire}, periodSub={obinapp.periodSub}");
        reSub.saveData();

        handleFirstSub();
        handleDailySub();
    }
    public void ReceiveSubscriptionProduct(SubscriptionInfo subinfo, PkgObject obinapp)
    {
        Debug.Log($"mysdk: inapp ReceiveSubscriptionProduct:{obinapp.id}");
        SubResultOb reSub;
        if (listSubs.ContainsKey(obinapp.id))
        {
            Debug.Log($"mysdk: inapp ReceiveSubscriptionProduct11");
            reSub = listSubs[obinapp.id];
        }
        else
        {
            Debug.Log($"mysdk: inapp ReceiveSubscriptionProduct12");
            reSub = new SubResultOb(obinapp.id, obinapp);
            reSub.statusSub = 1;
            reSub.countRcv = -1;
            reSub.dayDailyRcv = 0;
            reSub.purchaseDate = SdkUtil.systemCurrentMiliseconds() / 1000;
            listSubs.Add(obinapp.id, reSub);
        }
        reSub.purchaseDate = SdkUtil.TimeStampFromDateTime(subinfo.getPurchaseDate());
        reSub.purchaseExpire = SdkUtil.TimeStampFromDateTime(subinfo.getExpireDate());
        if (subinfo.isExpired() == Result.True)
        {
            Debug.Log($"mysdk: inapp ReceiveSubscriptionProduct:{obinapp.id} isExpired");
            reSub.statusSub = 0;
            reSub.countRcv = 300;
            reSub.saveData();
            listSubs.Remove(obinapp.id);
            return;
        }
        else
        {
            if (reSub.countRcv == -1)
            {
                long tnow = SdkUtil.systemCurrentMiliseconds() / 1000;
                int dd = SdkUtil.SubDay(tnow, reSub.purchaseDate);
                if (dd == 0)
                {
                    reSub.statusSub = 1;
                }
                else
                {
                    reSub.statusSub = 2;
                }
                reSub.countRcv = dd;
                reSub.dayDailyRcv = 0;
                reSub.saveData();
                Debug.Log($"mysdk: inapp ReceiveSubscriptionProduct:{obinapp.id} not handle reSub.ount={reSub.countRcv} dayDailyRcv={reSub.dayDailyRcv}-{SdkUtil.DateTimeFromTimeStamp(reSub.dayDailyRcv)}");
                handleFirstSub();
                handleDailySub();
            }
            else
            {
                int dd = SdkUtil.SubDay(reSub.purchaseExpire, reSub.dayDailyRcv);
                Debug.Log($"mysdk: inapp ReceiveSubscriptionProduct:{obinapp.id} exist del day ex vs drcv = {dd}");
                Debug.Log($"mysdk: inapp ReceiveSubscriptionProduct purchaseExpire={reSub.purchaseExpire}-{SdkUtil.DateTimeFromTimeStamp(reSub.purchaseExpire)}");
                Debug.Log($"mysdk: inapp ReceiveSubscriptionProduct dayDailyRcv={reSub.dayDailyRcv}-{SdkUtil.DateTimeFromTimeStamp(reSub.dayDailyRcv)}");
                if (dd > reSub.objInapp.periodSub)
                {
                    Debug.Log($"mysdk: inapp ReceiveSubscriptionProduct:{obinapp.id} renew");
                    reSub.statusSub = 2;
                    reSub.countRcv = 0;
                    reSub.saveData();
                    handleDailySub();
                }
                else
                {
                    Debug.Log($"mysdk: inapp ReceiveSubscriptionProduct:{obinapp.id} normal daily");
                    reSub.saveData();
                    handleDailySub();
                }
            }
        }
    }
#endif

    private bool isRcvSubOfDay(long tmpRcv)
    {
        bool re = false;
        DateTime dnow = DateTime.Now;
        DateTime drcv = SdkUtil.DateTimeFromTimeStamp(tmpRcv);
        if (dnow.Year > drcv.Year)
        {
            re = true;
        }
        else if (dnow.Year == drcv.Year)
        {
            if (dnow.Month > drcv.Month)
            {
                re = true;
            }
            else if (dnow.Month == drcv.Month)
            {
                if (dnow.Day > drcv.Day)
                {
                    re = true;
                }
            }
        }

        return re;
    }
    private void CheckHasSubscription()
    {
        Debug.Log($"mysdk: inapp HandleSub CheckHasSubscription");
        listSubs.Clear();
        foreach (var item in InappHelper.Instance.listCurr.listWithId)
        {
            if (item.Value.typeInapp == 2)
            {
                PkgObject obiap = item.Value;
                SubResultOb subre = new SubResultOb(obiap.id, obiap);
                subre.loadData();
                if (subre.statusSub > 0)
                {
                    Debug.Log($"mysdk: inapp HandleSub CheckHasSubscription add sub={subre.skuId}");
                    listSubs.Add(subre.skuId, subre);
                }
            }
        }
    }
    public bool hasSub(string skuid)
    {
        return listSubs.ContainsKey(skuid);
    }

    public bool isExpired(string skuid)
    {
        if (listSubs.ContainsKey(skuid))
        {
            if (listSubs[skuid].statusSub == 0)
            {
                return true;
            }
            else
            {
                long dnow = SdkUtil.systemCurrentMiliseconds() / 1000;
                long sub = dnow - listSubs[skuid].purchaseExpire;
                if (sub >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }
}

public class SubResultOb
{
    public string skuId;
    public int statusSub = 0;
    public int countRcv;
    public long dayDailyRcv;
    public long purchaseDate;
    public long purchaseExpire;
    public PkgObject objInapp;

    public SubResultOb(string sid, PkgObject obj)
    {
        skuId = sid;
        objInapp = obj;
        countRcv = 0;
        purchaseDate = SdkUtil.systemCurrentMiliseconds() / 1000;
        purchaseExpire = purchaseDate;
        dayDailyRcv = purchaseDate;
    }

    public void checkExpire()
    {
        long dnow = SdkUtil.systemCurrentMiliseconds() / 1000;
        long dd = dnow - purchaseExpire;
        if (dd >= 0)
        {
            statusSub = 0;
            saveData();
        }
    }

    public void saveData()
    {
        string dataDate = $"{purchaseDate},{purchaseExpire},{dayDailyRcv}";
        PlayerPrefsBase.Instance().setString($"{skuId}_date", dataDate);
        PlayerPrefsBase.Instance().setInt($"{skuId}_countRcv", countRcv);
        PlayerPrefsBase.Instance().setInt($"{skuId}_status", statusSub);
        Debug.Log($"mysdk: inapp HandleSub saveData {skuId} s={statusSub}, c={countRcv}, d={dataDate}");
    }

    public void loadData()
    {
        statusSub = PlayerPrefsBase.Instance().getInt($"{skuId}_status", 0);
        countRcv = PlayerPrefsBase.Instance().getInt($"{skuId}_countRcv", 0);
        string dataDate = PlayerPrefsBase.Instance().getString($"{skuId}_date", "");
        Debug.Log($"mysdk: inapp HandleSub loadData {skuId} s={statusSub}, c={countRcv}, d={dataDate}");
        if (dataDate.Length >= 5)
        {
            string[] arrday = dataDate.Split(',');
            if (arrday.Length == 3)
            {
                purchaseDate = long.Parse(arrday[0]);
                Debug.Log($"mysdk: inapp HandleSub loadData {skuId} purchaseDate={purchaseDate}-{SdkUtil.DateTimeFromTimeStamp(purchaseDate)}");
                purchaseExpire = long.Parse(arrday[1]);
                Debug.Log($"mysdk: inapp HandleSub loadData {skuId} purchaseExpire={purchaseExpire}-{SdkUtil.DateTimeFromTimeStamp(purchaseExpire)}");
                dayDailyRcv = long.Parse(arrday[2]);
                Debug.Log($"mysdk: inapp HandleSub loadData {skuId} dayDailyRcv={dayDailyRcv}-{SdkUtil.DateTimeFromTimeStamp(dayDailyRcv)}");
                checkExpire();
            }
            else
            {
                countRcv = 0;
                statusSub = 0;
                purchaseDate = SdkUtil.systemCurrentMiliseconds() / 1000;
                purchaseExpire = purchaseDate;
                dayDailyRcv = purchaseDate;
                Debug.Log($"mysdk: inapp HandleSub loadData 1 {skuId} purchaseDate={purchaseDate} purchaseExpire={purchaseExpire} dayDailyRcv={dayDailyRcv}");
            }
        }
        else
        {
            countRcv = 0;
            statusSub = 0;
            purchaseDate = SdkUtil.systemCurrentMiliseconds() / 1000;
            purchaseExpire = purchaseDate;
            dayDailyRcv = purchaseDate;
            Debug.Log($"mysdk: inapp HandleSub loadData 2 {skuId} purchaseDate={purchaseDate} purchaseExpire={purchaseExpire} dayDailyRcv={dayDailyRcv}");
        }
    }
}