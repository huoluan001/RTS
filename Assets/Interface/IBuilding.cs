using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    BuildingLabelSO Label { get; }
    List<MainBuildingSO> Requirement { get; }
    Vector2Int Area { get; }
    Vector2Int BuildingAndPlacementTime { get; }
    Vector2Int ExpandScope { get; }
    uint BuildingPrice { get; }
    Vector2Int WarningAndClearFogRad { get; }
    int PowerConsume { get; }
   
}