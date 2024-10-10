using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmyLabelSO", menuName = "ScriptableObjects/Data/ArmyLabelSO")]
public class ArmyLabelSO : ScriptableObject
{
    [SerializeField] private uint elementIndex;
    public string LableNameZH;
    public string LabelNameEN;
}
