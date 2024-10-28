using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    BuildingLabelSO Label { get; }
    Vector2Int Area { get; }
    Vector2Int BuildingAndPlacementTime { get; }
    Vector2Int ExpandScope { get; }
    int PowerConsume { get; }
    
    void SetBuilding(BuildingLabelSO label, Vector2Int area, Vector2Int buildingAndPlacementTime,
                        Vector2Int expandScope, int powerConsume);
   
}