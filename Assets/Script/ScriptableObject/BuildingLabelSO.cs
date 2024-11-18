using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "BuildingLabelSo", menuName = "ScriptableObjects/Data/BuildingLabelSo")]
public class BuildingLabelSo : ScriptableObject
{
    [SerializeField] private int elementIndex;
    public string LableNameZH;
    public string LabelNameEN;
}