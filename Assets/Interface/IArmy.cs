using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public interface IArmy
{
    Vector3 MoveSpeed { get; }
    bool3 IsReverseMove { get; }
    bool IsAmphibious { get; }
    CrushList CrushingAndCrushedLevel { get; }
    List<ArmyLabelSO> Labels { get; }
    int BuildingTime { get; }
    MainBuildingSO BuildFacilities { get; }
}