using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public interface IArmy
{
    Vector3 MoveSpeed { get; }
    bool3 IsReverseMove { get; }
    bool IsAmphibious { get; }
    CrushList CrushingAndCrushedLevel { get; }
    List<ArmyLabelSo> Labels { get; }
    int BuildingTime { get; }
    List<MainBuildingSo> BuildFacilities { get; }

    void SetArmy(Vector3 moveSpeed, bool3 isReverseMove, bool isAmphibious,
                    CrushList crushingAndCrushedLevel, List<ArmyLabelSo> labels,
                        int buildingTime, List<MainBuildingSo> buildFacilities);
}