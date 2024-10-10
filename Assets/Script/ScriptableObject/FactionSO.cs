using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CampSO", menuName = "ScriptableObjects/Data/FactionSO")]
public class FactionSO : ScriptableObject
{
    public Faction Faction;
    public List<MainBuildingSO> MainBuildings;
    public List<OtherBuildingSO> OtherBuildings;

    public List<ArmySO> Infantry;
    public List<ArmySO> Vehicle;
    public List<ArmySO> Aircraft;
    public List<ArmySO> Dock;
    
}
