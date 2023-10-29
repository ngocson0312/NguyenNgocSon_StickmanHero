using SuperFight;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinData", menuName = "Skin")]
public class SkinData : ScriptableObject
{
    public Skin[] listSkin;
}

[System.Serializable]
public class Skin
{
    public string skinName;
    public Character character;
    public Sprite avatar;
    public int levelUnlock;
    public SkinType skinType;
}

public enum SkinType
{
    NORMAL,
    CARD,
    DAILY_GIFT,
    EVENT,
    PESTIGE
}
