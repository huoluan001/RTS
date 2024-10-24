using System.Collections.Generic;
using UnityEngine;

public interface IBaseInfo
{
    FactionSO FactionSO { get; }
    uint Id { get; }
    string NameChinese { get; }
    string NameEnglish { get; }
    Sprite Icon { get; }
    string CommentChinese { get; }
    string CommentEnglish { get; }
    Troop Troop { get; }
    List<ActionScope> ActionScopes { get; }
    uint Exp { get; }
    uint Hp { get; }
    uint Price { get; }
    List<MainBuildingSO> Requirement { get; }
    Vector2Int WarningAndClearFogRad { get; }
    ArmorSO ArmorType { get; }
    GameObject GameObjectPrefab { get; }
}