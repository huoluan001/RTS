using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    FactionSO Faction { get; }
    uint Id { get; }
    string NameChinese { get; }
    string NameEnglish { get; }
    Sprite Icon { get; }
    string CommentChinese { get; }
    string CommentEnglish { get; }
    Troop Troop { get; }
    List<ActionScope> ActionScopes { get; }
    List<BuildingLabelSO> Label { get; }
    uint Exp { get; }
    List<MainBuildingSO> Requirement { get; }
    Vector2Int Area { get; }
    Vector2Int BuildingAndPlacementTime { get; }
    Vector2Int ExpandScope { get; }
    uint BuildingPrice { get; }
    Vector2Int WarningAndClearFogRad { get; }
    int PowerConsume { get; }
    ArmorSO ArmorType { get; }
    uint Hp { get; }
}