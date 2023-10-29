using System.Collections;
using mygame.sdk;
using UnityEngine;
namespace SuperFight
{
    public class ApplyConfigStats : MonoBehaviour
    {
        public BossFightArena bossFightArena;
        void Start()
        {
            if (GameHelper.checkLvXaDu())
            {
                GetComponent<Controller>().stats = new CharacterStats(PlayerPrefsUtil.cfBossHealth, PlayerPrefsUtil.cfBossDamage);
                bossFightArena.Initialize();
            }

        }
    }
}

