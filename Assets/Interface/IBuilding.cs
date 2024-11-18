using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    BuildingLabelSo Label { get; }
    Vector2Int Area { get; }
    Vector2 BuildingAndPlacementTime { get; }
    Vector2Int ExpandScope { get; }
    int PowerConsume { get; }
    
    void SetBuilding(BuildingLabelSo label, Vector2Int area, Vector2Int expandScope, Vector2 buildingAndPlacementTime, int powerConsume);
                        
   
}