
namespace SuperFight
{
    public enum GAME_STATE
    {
        GS_STOP,
        GS_PAUSEGAME,
        GS_PLAYING
    }

    public enum TYPE_GROUND
    {
        G_LAND_1 = 0,

    }

    public enum TYPE_ENEMY
    {
        E_PIG = 0,
        E_ENDERMAN,
        E_SPIDER,
        E_ZOMBIE,
        E_CREEPER,
        E_PIGMAN,
        E_SKELETON,
        E_GOLEM,
        E_SPIDERHANGING,
        E_BEE,
        B_WITCH,
        B_COW,
        B_GHOST,
        B_DRAGON,
        E_PIGLETSLIDE,
        E_ENDERMANLASER,
        E_WITHERSKELETON,
        E_BLASTER,
        E_IOS_1,
        E_IOS_2,
        E_IOS_3,
        E_IOS_4
    }

    public enum TYPE_ITEM
    {
        IT_SAVEPOINT = 0,
        IT_WEAPON,
        IT_HEALTH,
        IT_COIN,
        IT_WIN_DOOR,
        IT_WIN_DOOR_IOS,
        IT_SAVEPOINT_IOS,
        IT_MOVING,
        IT_BOW,
    }

    public class Constant
    {
        public static readonly string layerMainPlayer = "Player";
        public static readonly string layerItemGame = "Item";
        public static readonly string layerEnemy = "Enemy";
        public static readonly string layerGround = "Ground";
        public static readonly int minCoinKillEnemy = 0;
        public static readonly int maxCoinKillEnemy = 6;
        public static readonly float timeCountdownRevive = 6f;
    }
}