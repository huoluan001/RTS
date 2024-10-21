using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    BuildingLabelSO Label { get; }
    Vector2Int Area { get; }
    Vector2Int BuildingAndPlacementTime { get; }
    Vector2Int ExpandScope { get; }
    int PowerConsume { get; }
   
}