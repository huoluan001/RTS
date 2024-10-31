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
    List<MainBuildingSO> BuildFacilities { get; }

    void SetArmy(Vector3 moveSpeed, bool3 isReverseMove, bool isAmphibious,
                    CrushList crushingAndCrushedLevel, List<ArmyLabelSO> labels,
                        int buildingTime, List<MainBuildingSO> buildFacilities);
}