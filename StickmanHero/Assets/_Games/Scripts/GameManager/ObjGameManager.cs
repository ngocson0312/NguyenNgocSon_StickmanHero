using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperFight
{
    public class ObjGameManager : MonoBehaviour
    {
        //PrefabEnemy-----------------------------------------------------------------------------------
        [Header("Enemy")]
        [SerializeField] private Zombie prefabZombie;
        [SerializeField] private Piglet prefabPiglet;
        [SerializeField] private Enderman prefabEnderman;
        [SerializeField] private Creeper prefabCreeper;
        [SerializeField] private Spider prefabSpider;
        [SerializeField] private SpiderHanging prefabSpiderHanging;
        [SerializeField] private Pigman prefabPigman;
        [SerializeField] private Golem prefabGolem;
        [SerializeField] private Skeleton prefabSkeleton;
        [SerializeField] private Piglet prefabPigletSlide;
        [SerializeField] private Enderman prefabEndermanLaser;
        [SerializeField] private WitherSkelton prefabWitherSeleton;
        [SerializeField] private Blaze prefabBlaze;

        [SerializeField] private Zombie prefabIOS1;
        [SerializeField] private Skeleton prefabIOS2;
        [SerializeField] private Zombie prefabIOS3;
        [SerializeField] private Enemy prefabIOS4;
        //PrefabItem-----------------------------------------------------------------------------------
        [Header("Item")]
        [SerializeField] private ItemSavePoint prefItemSavePoint;
        [SerializeField] private ItemCoin prefItemCoin;
        [SerializeField] private ItemDoor prefItemDoor;
        [SerializeField] private ItemHealth prefItemHealth;
        [SerializeField] private ItemWeapon prefItemSword;
        [SerializeField] private ItemWeapon prefItemBow;

        [SerializeField] private ItemDoor prefItemDoorIos;
        [SerializeField] private ItemSavePoint prefItemSavePointIos;
        //PrefabFx------------------------------------------------------------------------------------
        [Header("FX")]
        [SerializeField] BaseFxGame prefabFxExplosion;

        //---------------------------------------------------------------------------------------------
        [Header("=====")]
        // private List<Boss> listBossInGame;
        private BossFightArena arena;
        private List<Enemy> listEnemyInGame;
        private List<BaseItem> listItemInGame;
        private List<BaseFxGame> listFxInGame;

        public ItemDoor winDoorInMap { private set; get; }

        public void initBoss(BossFightArena arena)
        {
            this.arena = arena;
        }

        public void initEnemy(List<BaseEnemy> listEnemies)
        {
            if (listEnemyInGame == null)
            {
                listEnemyInGame = new List<Enemy>();
            }
            else
            {
                listEnemyInGame.Clear();
            }
            if (listEnemies != null)
            {
                Enemy enemy = null;
                for (int i = 0; i < listEnemies.Count; ++i)
                {
                    switch (listEnemies[i].TypeEnemy)
                    {
                        case TYPE_ENEMY.E_ZOMBIE:
                            enemy = PoolingObject.GetObjectFree<Zombie>(prefabZombie);
                            break;

                        case TYPE_ENEMY.E_PIG:
                            enemy = PoolingObject.GetObjectFree<Piglet>(prefabPiglet);
                            break;

                        case TYPE_ENEMY.E_ENDERMAN:
                            enemy = PoolingObject.GetObjectFree<Enderman>(prefabEnderman);
                            break;

                        case TYPE_ENEMY.E_CREEPER:
                            enemy = PoolingObject.GetObjectFree<Creeper>(prefabCreeper);
                            break;

                        case TYPE_ENEMY.E_SPIDER:
                            enemy = PoolingObject.GetObjectFree<Spider>(prefabSpider);
                            break;

                        case TYPE_ENEMY.E_BEE:
                            // enemy = PoolingObject.GetObjectFree<Bee>(prefabBee);
                            break;

                        case TYPE_ENEMY.E_PIGMAN:
                            enemy = PoolingObject.GetObjectFree<Pigman>(prefabPigman);
                            break;
                        case TYPE_ENEMY.E_GOLEM:
                            enemy = PoolingObject.GetObjectFree<Golem>(prefabGolem);
                            break;
                        case TYPE_ENEMY.E_SKELETON:
                            enemy = PoolingObject.GetObjectFree<Skeleton>(prefabSkeleton);
                            Skeleton skeleton = (Skeleton)enemy;
                            skeleton.attackSpeed = listEnemies[i].attackSpeed;
                            break;
                        case TYPE_ENEMY.E_SPIDERHANGING:
                            enemy = PoolingObject.GetObjectFree<SpiderHanging>(prefabSpiderHanging);
                            enemy.GetComponent<SpiderHanging>().moveRange = listEnemies[i].moveRange;
                            enemy.GetComponent<SpiderHanging>().firstDelay = listEnemies[i].firstDelay;
                            break;

                        case TYPE_ENEMY.E_PIGLETSLIDE:
                            enemy = PoolingObject.GetObjectFree<Piglet>(prefabPigletSlide);
                            break;

                        case TYPE_ENEMY.E_ENDERMANLASER:
                            enemy = PoolingObject.GetObjectFree<Enderman>(prefabEndermanLaser);
                            break;

                        case TYPE_ENEMY.E_WITHERSKELETON:
                            enemy = PoolingObject.GetObjectFree<WitherSkelton>(prefabWitherSeleton);
                            break;
                        case TYPE_ENEMY.E_BLASTER:
                            enemy = PoolingObject.GetObjectFree<Blaze>(prefabBlaze);
                            break;
                        case TYPE_ENEMY.E_IOS_1:
                            enemy = PoolingObject.GetObjectFree<Zombie>(prefabIOS1);
                            break;

                        case TYPE_ENEMY.E_IOS_2:
                            enemy = PoolingObject.GetObjectFree<Skeleton>(prefabIOS2);
                            break;

                        case TYPE_ENEMY.E_IOS_3:
                            enemy = PoolingObject.GetObjectFree<Zombie>(prefabIOS3);
                            break;
                        case TYPE_ENEMY.E_IOS_4:
                            enemy = PoolingObject.GetObjectFree<Enemy>(prefabIOS4);
                            break;
                        default:
                            break;
                    }
                    enemy.transform.position = listEnemies[i].transform.position;
                    enemy.transform.parent = LevelManager.getInstance().currentMap.transform;
                    enemy.Initialize();
                    if (listEnemies[i].overrideBaseStats)
                    {
                        enemy.ResetStatEnemy(listEnemies[i].enemyStats);
                    }
                    else
                    {
                        enemy.ResetStatEnemy(null);
                    }
                    listEnemyInGame.Add(enemy);
                }
            }

        }

        public void addEnemy(Enemy enemy)
        {
            if (enemy == null || listEnemyInGame == null) return;
            listEnemyInGame.Add(enemy);
        }
        
        public void removeEnemy(Enemy enemy)
        {
            if (enemy == null || listEnemyInGame == null) return;

            if (listEnemyInGame.Contains(enemy))
            {
                PoolingObject.FreeObject(enemy.gameObject);
                listEnemyInGame.Remove(enemy);
                return;
            }
        }

        public void clearAllEnemy()
        {
            if (listEnemyInGame != null)
            {
                for (int i = 0; i < listEnemyInGame.Count; ++i)
                {
                    PoolingObject.FreeObject(listEnemyInGame[i].gameObject);
                }
                listEnemyInGame.Clear();
            }
        }

        public void pauseAllEnemy()
        {
            if (listEnemyInGame != null)
            {
                for (int i = 0; i < listEnemyInGame.Count; ++i)
                {
                    listEnemyInGame[i].Pause();
                }
            }
            if (arena != null)
            {
                arena.OnPauseGame();
            }
        }

        public void resumeAllEnemy()
        {
            if (listEnemyInGame != null)
            {
                for (int i = 0; i < listEnemyInGame.Count; ++i)
                {
                    listEnemyInGame[i].Resume();
                }
            }
            if (arena != null)
            {
                arena.OnResumeGame();
            }
        }

        ///---------------------------------------------------------------------------------------------------------------------------------------
        public void initItemGame(List<BaseItem> listItem)
        {
            if (listItemInGame == null)
            {
                listItemInGame = new List<BaseItem>();
            }
            else
            {
                listItemInGame.Clear();
            }

            if (listItem != null)
            {
                BaseItem item = null;
                for (int i = 0; i < listItem.Count; ++i)
                {
                    switch (listItem[i].TypeItem)
                    {
                        case TYPE_ITEM.IT_SAVEPOINT:
                            item = PoolingObject.GetObjectFree<ItemSavePoint>(prefItemSavePoint);
                            item.GetComponent<ItemSavePoint>().resetSavePoint();
                            break;

                        case TYPE_ITEM.IT_COIN:
                            item = PoolingObject.GetObjectFree<ItemCoin>(prefItemCoin);
                            break;

                        case TYPE_ITEM.IT_WEAPON:
                            item = PoolingObject.GetObjectFree<ItemWeapon>(prefItemSword);
                            break;

                        case TYPE_ITEM.IT_BOW:
                            item = PoolingObject.GetObjectFree<ItemWeapon>(prefItemBow);
                            break;

                        case TYPE_ITEM.IT_HEALTH:
                            item = PoolingObject.GetObjectFree<ItemHealth>(prefItemHealth);
                            break;

                        case TYPE_ITEM.IT_WIN_DOOR:
                            winDoorInMap = PoolingObject.GetObjectFree<ItemDoor>(prefItemDoor);
                            item = winDoorInMap;
                            break;
                        case TYPE_ITEM.IT_WIN_DOOR_IOS:
                            winDoorInMap = PoolingObject.GetObjectFree<ItemDoor>(prefItemDoorIos);
                            item = winDoorInMap;
                            break;
                        case TYPE_ITEM.IT_SAVEPOINT_IOS:
                            item = PoolingObject.GetObjectFree<ItemSavePoint>(prefItemSavePointIos);
                            item.GetComponent<ItemSavePoint>().resetSavePoint();
                            break;
                        default:

                            break;
                    }

                    item.transform.position = listItem[i].transform.position;
                    item.transform.parent = LevelManager.getInstance().currentMap.transform;
                    listItemInGame.Add(item);
                }
            }
        }

        public void createCoin(Vector3 posInit)
        {
            ItemCoin coin = PoolingObject.GetObjectFree<ItemCoin>(prefItemCoin);
            coin.InitCoin(posInit);
            coin.transform.parent = LevelManager.getInstance().currentMap.transform;
            if (!listItemInGame.Contains(coin))
            {
                listItemInGame.Add(coin);
            }
        }

        public void removeItem(BaseItem item)
        {
            if (item == null || listItemInGame == null) return;

            if (listItemInGame.Contains(item))
            {
                PoolingObject.FreeObject(item.gameObject);
                listItemInGame.Remove(item);
            }
        }

        public void clearAllItem()
        {
            if (listItemInGame == null) return;

            for (int i = 0; i < listItemInGame.Count; ++i)
            {
                PoolingObject.FreeObject(listItemInGame[i].gameObject);
            }
            listItemInGame.Clear();
        }

        //--------------------------------------------------------------------------------------------------------

        public void createFxExplode(Transform transformInit)
        {
            if (listFxInGame == null)
            {
                listFxInGame = new List<BaseFxGame>();
            }

            BaseFxGame fxExplode = PoolingObject.GetObjectFree<BaseFxGame>(prefabFxExplosion);

            fxExplode.transform.position = transformInit.position;
            fxExplode.transform.eulerAngles = transformInit.eulerAngles;
            fxExplode.transform.localScale = transformInit.localScale;

            fxExplode.transform.parent = LevelManager.getInstance().currentMap.transform;
            // fxExplode.transform.parent = transformInit.parent;

            if (!listFxInGame.Contains(fxExplode)) listFxInGame.Add(fxExplode);
        }

        public void clearAllFx()
        {
            if (listFxInGame == null) return;
            for (int i = 0; i < listFxInGame.Count; ++i)
            {
                listFxInGame[i].Free();
            }
            listFxInGame.Clear();
        }
    }
}