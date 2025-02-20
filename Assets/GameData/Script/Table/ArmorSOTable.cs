using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ArmorSoTable", menuName = "ScriptableObjects/Data/ArmorSoTable")]
public class ArmorSoTable : ScriptableObject
{
    public List<ArmorSo> armorSoList;
}