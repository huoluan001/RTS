using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmyLabelSo", menuName = "ScriptableObjects/Data/ArmyLabelSo")]
public class ArmyLabelSo : ScriptableObject
{
    [SerializeField] private int elementIndex;
    public string LableNameZH;
    public string LabelNameEN;
}
