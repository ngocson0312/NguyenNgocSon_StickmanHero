using System;
using SuperFight;
using UnityEngine;
public interface IDamage
{
    void TakeDamage(DamageInfo damageInfo);
}
[System.Serializable]
public struct DamageInfo
{
    public int damage;
    public int hitDirection;
    public bool isKnockBack;
    public float stunTime;
    public int idSender;
    public CharacterType characterType;
    public AudioClip impactSound;
    public Controller owner;
}
public enum CharacterType
{
    Mob = 0, Character, Boss
}