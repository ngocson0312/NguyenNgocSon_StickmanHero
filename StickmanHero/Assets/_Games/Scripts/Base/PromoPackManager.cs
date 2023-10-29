using SuperFight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoPackManager : Singleton<PromoPackManager>
{
    [SerializeField] Transform canvas;
    [SerializeField] GameObject starterPack;
    [SerializeField] GameObject legendPack;
    [SerializeField] GameObject kaisaPack;
    [SerializeField] GameObject rivenPack;
    [SerializeField] GameObject superLevelUp1;
    [SerializeField] GameObject superLevelUp2;
    [SerializeField] GameObject superLevelUp3;
    [SerializeField] InappData[] weeklySale;
    [SerializeField] InappData subReward;
    [SerializeField] InappData subRewardaily;
    public void ShowStarterPack()
    {
        Instantiate(starterPack, canvas);
    }
    public void ShowLegendPack()
    {
        Instantiate(legendPack, canvas);
    }
    public void ShowKaisaPack()
    {
       // Instantiate(kaisaPack, canvas);
    }
    public void ShowRivenPack()
    {
       // Instantiate(rivenPack, canvas);
    }
    public void ShowSuperLevelUp1()
    {
        Instantiate(superLevelUp1, canvas);
    }
    public void ShowSuperLevelUp2()
    {
        Instantiate(superLevelUp2, canvas);
    }
    public void ShowSuperLevelUp3()
    {
        Instantiate(superLevelUp3, canvas);
    }
    public void ShowSuperWeekly()
    {
        //superWeekly.Show(weeklySale[0]);
    }
}
