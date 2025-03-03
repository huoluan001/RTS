using System.Collections.Generic;
using GameData.Script.Enum;
using Unity.Mathematics;
using UnityEngine;

public interface IArmy
{
    Vector3 MoveSpeed { get; }
    bool3 IsReverseMove { get; }
    bool IsAmphibious { get; }
    CrushList CrushingAndCrushedLevel { get; }
    List<ArmyLabelEnum> Labels { get; }
    int BuildingTime { get; }
    List<MainBuildingSo> BuildFacilities { get; }

    void SetArmy(Vector3 moveSpeed, bool3 isReverseMove, bool isAmphibious,
                    CrushList crushingAndCrushedLevel, List<ArmyLabelEnum> labels,
                        int buildingTime, List<MainBuildingSo> buildFacilities);
}