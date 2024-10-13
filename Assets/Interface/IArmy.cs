using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public interface IArmy
{
    bool IsAmphibious { get; }
    bool3 IsReverseMove { get; }
    CrushList CrushingAndCrushedLevel { get; }
    Vector2Int WarningAndClearFogRad { get; }
    List<ArmyLabelSO> Labels { get; }
    List<GameObject> Requirement { get; }
    uint BuildingTime { get; }
    uint BuildingPrice { get; }
    MainBuildingSO BuildFacilities { get; }
}