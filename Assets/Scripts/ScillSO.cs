using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "DuoGrid/LevelData", order = 3)]
public class ScillSO : ScriptableObject
{
    [Header("база")]
    public string Name;
    public string id;
    public Sprite icon;
    public bool Finish;
    public bool PlayerUpHealth;
    public int HealthUp;
    public string description;
    [Header("настройки")]
    public int AddedChanceToSkipCell;
    public bool ZoneBuffOfAbility;
    public int BuffTo;
    public bool AddNewSlot;
    public bool AddRegeneration;
    public int TurnsForRegeneration;
    public int regenerationCount;
    public EffectType EffectTypeInNewSlot;
    public int LevelEffectTypeInNewSlot;
    [Header("скилы до")]
    public List<ScillSO> scillsPrerequisites = new();
    [Header("следующие скилы")]
    public List<ScillSO> scillsNext = new();
}
