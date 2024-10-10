using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "BuildingLabelSO", menuName = "ScriptableObjects/Data/BuildingLabelSO")]
public class BuildingLabelSO : ScriptableObject
{
    [SerializeField] private uint elementIndex;
    public string LableNameZH;
    public string LabelNameEN;
}