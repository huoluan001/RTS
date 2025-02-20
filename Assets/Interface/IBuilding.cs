using GameData.Script.Enum;
using UnityEngine;

public interface IBuilding
{
    BuildingLabelEnum Label { get; }
    Vector2Int Area { get; }
    Vector2 BuildingAndPlacementTime { get; }
    Vector2Int ExpandScope { get; }
    int PowerConsume { get; }
    
    void SetBuilding(BuildingLabelEnum label, Vector2Int area, Vector2Int expandScope, Vector2 buildingAndPlacementTime, int powerConsume);
                        
   
}