using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame.sdk;
using UnityEngine.Tilemaps;
using System.Linq;
namespace SuperFight
{
    public class LevelData : MonoBehaviour
    {
        public int levelIndex;
        public Transform playerPoint;
        public TypeThemes typeThemes;
        public bool isLevelBoss;
        // public List<DataGround> listGrounds { private set; get; }
        // public List<DataItem> listItems { private set; get; }
        // public List<DataEnemy> listEnemies { private set; get; }
        public BossFightArena bossFightArena;
        public List<BaseEnemy> listEnemies;
        public List<BaseItem> listItems;
        public Transform[] listPosAdmix;
        public void initData()
        {
            // initDataGround();
            // initDataItem();
            // initDataEnemy();
            if(bossFightArena != null)
            {
                bossFightArena.Initialize();
            }
        }

        public void InitAdmix()
        {
            // if (listPosAdmix != null && listPosAdmix.Length > 0)
            // {
            //     for (int i = 0; i < listPosAdmix.Length; i++)
            //     {
            //         AdMixObject adMixObject = AdmixHelper.genAd(AdmixType.Size6x5, listPosAdmix[i].position, Vector3.zero);
            //         if (adMixObject != null)
            //         {
            //             adMixObject.transform.localScale = Vector3.one * 0.5f;
            //             adMixObject.transform.localPosition = new Vector3(listPosAdmix[i].position.x, listPosAdmix[i].position.y, 14f);
            //             adMixObject.transform.eulerAngles = new Vector3(0, 180, 0);
            //         }
            //     }
            // }
        }

        private void initDataGround()
        {
            // Tilemap tilemap = layerGround.GetComponent<Tilemap>();
            // BoundsInt bounds = tilemap.cellBounds;
            // TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
            // Vector3 posTileBase = Vector3.zero;
            // Vector3Int coordinate = Vector3Int.zero;
            // listGrounds = new List<DataGround>();

            // for (int x = 0; x < bounds.size.x; x++)
            // {
            //     for (int y = 0; y < bounds.size.y; y++)
            //     {
            //         TileBase tile = allTiles[x + y * bounds.size.x];
            //         if (tile != null)
            //         {
            //             Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
            //             coordinate.x = x;
            //             coordinate.y = y;
            //             posTileBase = tilemap.CellToWorld(coordinate);
            //             listGrounds.Add(new DataGround(TYPE_GROUND.G_LAND_1, posTileBase));
            //         }
            //         else
            //         {
            //             Debug.Log("x:" + x + " y:" + y + " tile: (null)");
            //         }
            //     }
            // }
        }
        public void GetAllItem()
        {
            bossFightArena = GetComponentInChildren<BossFightArena>();
            listEnemies = GetComponentsInChildren<BaseEnemy>().ToList();
            listItems = GetComponentsInChildren<BaseItem>().ToList();

            for (int i = 0; i < listEnemies.Count; i++)
            {
                listEnemies[i].gameObject.name = listEnemies[i].TypeEnemy.ToString();
                listEnemies[i].ConfigEnemyStats(levelIndex);
            }

        }
        private void initDataItem()
        {
            // Tilemap tilemap = layerGround.GetComponent<Tilemap>();
            // BoundsInt bounds = tilemap.cellBounds;
            // TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
            // Vector3 posTileBase = Vector3.zero;
            // Vector3Int coordinate = Vector3Int.zero;
            // grounds = new List<DataGround>();

            // for (int x = 0; x < bounds.size.x; x++)
            // {
            //     for (int y = 0; y < bounds.size.y; y++)
            //     {
            //         TileBase tile = allTiles[x + y * bounds.size.x];
            //         if (tile != null)
            //         {
            //             Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
            //             coordinate.x = x;
            //             coordinate.y = y;
            //             posTileBase = tilemap.CellToWorld(coordinate);
            //             grounds.Add(new DataGround(TYPE_GROUND.G_LAND_1, posTileBase));
            //         }
            //         else
            //         {
            //             Debug.Log("x:" + x + " y:" + y + " tile: (null)");
            //         }
            //     }
            // }
        }

        private void initDataEnemy()
        {



            // Tilemap tilemap = layerGround.GetComponent<Tilemap>();
            // BoundsInt bounds = tilemap.cellBounds;
            // TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
            // Vector3 posTileBase = Vector3.zero;
            // Vector3Int coordinate = Vector3Int.zero;
            // grounds = new List<DataGround>();

            // for (int x = 0; x < bounds.size.x; x++)
            // {
            //     for (int y = 0; y < bounds.size.y; y++)
            //     {
            //         TileBase tile = allTiles[x + y * bounds.size.x];
            //         if (tile != null)
            //         {
            //             Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
            //             coordinate.x = x;
            //             coordinate.y = y;
            //             posTileBase = tilemap.CellToWorld(coordinate);
            //             grounds.Add(new DataGround(TYPE_GROUND.G_LAND_1, posTileBase));
            //         }
            //         else
            //         {
            //             Debug.Log("x:" + x + " y:" + y + " tile: (null)");
            //         }
            //     }
            // }
        }
    }
}