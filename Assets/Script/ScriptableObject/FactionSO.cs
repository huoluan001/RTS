using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "FactionSo", menuName = "ScriptableObjects/Data/FactionSo")]
public class FactionSo : ScriptableObject
{
    public FactionEnum factionEnum;
    public List<MainBuildingSo> MainBuildings;
    public List<OtherBuildingSo> OtherBuildings;

    public List<ArmySo> Infantry;
    public List<ArmySo> Vehicle;
    public List<ArmySo> Aircraft;
    public List<ArmySo> Dock;

    public List<IBaseInfo> GetBaseInfos(SequenceType sequenceType)
    {
        if (sequenceType == SequenceType.MainBuildingSequence)
            return MainBuildings.Cast<IBaseInfo>().ToList();
        if (sequenceType == SequenceType.OtherBuildingSequence)
            return OtherBuildings.Cast<IBaseInfo>().ToList();
        if (sequenceType == SequenceType.InfantrySequence)
            return Infantry.Cast<IBaseInfo>().ToList();
        if (sequenceType == SequenceType.VehicleSequence)
            return Vehicle.Cast<IBaseInfo>().ToList();
        if (sequenceType == SequenceType.AircraftSequence)
            return Aircraft.Cast<IBaseInfo>().ToList();
        if (sequenceType == SequenceType.DockSequence)
            return Dock.Cast<IBaseInfo>().ToList();
        else
            return null;    
    }

}
