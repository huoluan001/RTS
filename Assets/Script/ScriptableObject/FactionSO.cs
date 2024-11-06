using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "FactionSO", menuName = "ScriptableObjects/Data/FactionSO")]
public class FactionSO : ScriptableObject
{
    public FactionEnum factionEnum;
    public List<MainBuildingSO> MainBuildings;
    public List<OtherBuildingSO> OtherBuildings;

    public List<ArmySO> Infantry;
    public List<ArmySO> Vehicle;
    public List<ArmySO> Aircraft;
    public List<ArmySO> Dock;

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
