using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionConditions
{
    EndMission,CollectPlasma,UseShield,UseWeaponPack,FlyDistanceTotal,FlyDistanceOnce,KillBoss,DontShootForDistance
}

public enum EnemyTypes
{
    All, Gunship, Mine, Missile, Blimp, Slicer, Dragon
}

public enum BossTypes
{
    All, Robodactyl, Behemoth, Battlecruiser, CyberKraken
}

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObject/New Mission")]
public class MissionScriptableObject : ScriptableObject
{
    public MissionConditions missionCondition;
    public EnemyTypes enemy;
    public BossTypes boss;
    public int missionAmount;
}
