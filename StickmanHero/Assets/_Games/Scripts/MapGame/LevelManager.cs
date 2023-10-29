using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperFight
{
    public class DataGround
    {
        public TYPE_GROUND Type { private set; get; }
        public Vector3 Position { private set; get; }

        public DataGround(TYPE_GROUND type, Vector3 pos)
        {
            Type = type;
            Position = pos;
        }
    }

    public class DataEnemy
    {
        public TYPE_ENEMY Type { private set; get; }
        public Vector3 Position { private set; get; }
        public DataEnemy(TYPE_ENEMY type, Vector3 pos)
        {
            Type = type;
            Position = pos;
        }
    }

    public class DataItem
    {
        public TYPE_ITEM Type { private set; get; }
        public Vector3 Position { private set; get; }
        public DataItem(TYPE_ITEM type, Vector3 pos)
        {
            Type = type;
            Position = pos;
        }
    }

    public class LevelManager
    {
        private static LevelManager mInstance = null;
        public GameObject currentMap { private set; get; }
        public static LevelManager getInstance()
        {
            if (mInstance == null)
            {
                mInstance = new LevelManager();
            }

            return mInstance;
        }
        public LevelData getLevelBoss(int currLvGame)
        {
            currentMap = GameObject.Instantiate(Resources.Load<GameObject>("Level/Boss" + currLvGame));
            LevelData lvData = currentMap.GetComponent<LevelData>();
            lvData.initData();
            return lvData;
        }
        public LevelData getLevelData(int currLvGame)
        {
            string levelPath = "Level/Level " + currLvGame;
#if UNITY_ANDROID
            if (currLvGame <= 4)
            {
                levelPath = "Level/Level " + currLvGame + "T";
            }
#endif
            currentMap = GameObject.Instantiate(Resources.Load<GameObject>(levelPath));
            LevelData lvData = currentMap.GetComponent<LevelData>();
            lvData.initData();
            return lvData;
        }

        public LevelData getLevelDataIOS(int currLvGame)
        {
            Debug.Log("ahihi: 33");
            currentMap = GameObject.Instantiate(Resources.Load<GameObject>("Level/Level " + currLvGame + "A"));
            LevelData lvData = currentMap.GetComponent<LevelData>();
            lvData.initData();
            return lvData;
        }

        public LevelData getLevelTest()
        {
            currentMap = GameObject.Instantiate(Resources.Load<GameObject>("Level/LevelTest"));
            LevelData lvData = currentMap.GetComponent<LevelData>();
            lvData.initData();
            return lvData;
        }

        public void DestroyMap()
        {
            if (currentMap != null)
            {
                GameObject.Destroy(currentMap);
            }
        }
    }
}