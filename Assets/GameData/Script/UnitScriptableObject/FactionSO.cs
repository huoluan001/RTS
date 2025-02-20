using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "FactionSo", menuName = "ScriptableObjects/Data/FactionSo")]
public class FactionSo : ScriptableObject
{
    public FactionEnum factionEnum;
    public List<MainBuildingSo> mainBuildings;
    public List<OtherBuildingSo> otherBuildings;

    public List<ArmySo> army;
    public List<ArmySo> infantry;
    public List<ArmySo> vehicle;
    public List<ArmySo> aircraft;
    public List<ArmySo> dock;

    public List<IBaseInfo> GetBaseInfos(SequenceType sequenceType)
    {
        if (sequenceType == SequenceType.MainBuildingSequence)
            return mainBuildings.Cast<IBaseInfo>().ToList();
        if (sequenceType == SequenceType.OtherBuildingSequence)
            return otherBuildings.Cast<IBaseInfo>().ToList();
        if (sequenceType == SequenceType.InfantrySequence)
            return infantry.Cast<IBaseInfo>().ToList();
        if (sequenceType == SequenceType.VehicleSequence)
            return vehicle.Cast<IBaseInfo>().ToList();
        if (sequenceType == SequenceType.AircraftSequence)
            return aircraft.Cast<IBaseInfo>().ToList();
        if (sequenceType == SequenceType.DockSequence)
            return dock.Cast<IBaseInfo>().ToList();
        else
            return null;
    }
}