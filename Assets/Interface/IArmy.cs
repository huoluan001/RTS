using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

interface IArmy
{
    FactionSO Faction { get; }
    uint Id { get; }
    string NameChinese { get; }
    string NameEnglish { get; }
    string CommentChinese { get; }
    string CommentEnglish { get; }
    Sprite Icon { get; }
    Troop Troop { get; }
    List<ActionScope> ActionScopeList { get; }
    Vector3 MoveSpeed { get; }
    uint Exp { get; }
    bool IsAmphibious { get; }
    bool3 IsReverseMove { get; }
    CrushList CrushingAndCrushedLevel { get; }
    Vector2Int WarningAndClearFogRad { get; }
    List<ArmyLabelSO> Labels { get; }
    List<GameObject> Requirement { get; }
    uint BuildingTime { get; }
    uint BuildingPrice { get; }
    MainBuildingSO BuildFacilities { get; }
    ArmorSO Armor { get; }
    uint Hp { get; }
}