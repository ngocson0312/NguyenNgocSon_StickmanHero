using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperFight
{
    public class BaseEnemy : MonoBehaviour
    {
        [SerializeField] private TYPE_ENEMY type;
        public TYPE_ENEMY TypeEnemy => type;
        [Header("Stats")]
        public CharacterStats enemyStats;
        public bool overrideBaseStats;

        public float attackSpeed;
        [Header("Spider Haning")]
        public float moveRange;
        public float firstDelay;

        public void ConfigEnemyStats(int level)
        {

            float maxHeath = 0;
            float damage = 0;
            switch (TypeEnemy)
            {
                case TYPE_ENEMY.E_PIG:
                    maxHeath = 60;
                    damage = 5;
                    break;
                case TYPE_ENEMY.E_ENDERMAN:
                    maxHeath = 80;
                    damage = 15;
                    break;
                case TYPE_ENEMY.E_SPIDER:
                    maxHeath = 60;
                    damage = 20;
                    break;
                case TYPE_ENEMY.E_ZOMBIE:
                    maxHeath = 50;
                    damage = 10;
                    break;
                case TYPE_ENEMY.E_CREEPER:
                    maxHeath = 100;
                    damage = 20;
                    break;
                case TYPE_ENEMY.E_PIGMAN:
                    maxHeath = 80;
                    damage = 17;
                    break;
                case TYPE_ENEMY.E_SKELETON:
                    maxHeath = 60;
                    damage = 15;
                    break;
                case TYPE_ENEMY.E_GOLEM:
                    maxHeath = 120;
                    damage = 15;
                    break;
                case TYPE_ENEMY.E_SPIDERHANGING:
                    maxHeath = 35;
                    damage = 20;
                    break;
                case TYPE_ENEMY.E_PIGLETSLIDE:
                    maxHeath = 60;
                    damage = 5;
                    break;
                case TYPE_ENEMY.E_ENDERMANLASER:
                    maxHeath = 55;
                    damage = 20;
                    break;
                case TYPE_ENEMY.E_BEE:
                    maxHeath = 0;
                    damage = 0;
                    break;
                case TYPE_ENEMY.E_WITHERSKELETON:
                    maxHeath = 60;
                    damage = 22;
                    break;
                case TYPE_ENEMY.E_BLASTER:
                    maxHeath = 45;
                    damage = 22;
                    break;
                case TYPE_ENEMY.E_IOS_1:
                    maxHeath = 50;
                    damage = 10;
                    break;
                case TYPE_ENEMY.E_IOS_2:
                    maxHeath = 60;
                    damage = 15;
                    break;
                case TYPE_ENEMY.E_IOS_3:
                    maxHeath = 60;
                    damage = 22;
                    break;
                default:
                    break;
                    
            }
            maxHeath = maxHeath + ((float)level / 10f) * maxHeath;
            damage = damage + ((float)level / 10f) * damage;
            enemyStats = new CharacterStats((int)maxHeath, (int)damage);
        }

    }
}