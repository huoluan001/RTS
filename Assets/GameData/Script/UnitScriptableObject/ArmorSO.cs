using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Linq;
using GameData.Script.Enum;
using UnityEngine.Serialization;

[Serializable]
[CreateAssetMenu(fileName = "ArmorSo", menuName = "ScriptableObjects/Data/ArmorSo")]
public class ArmorSo : ScriptableObject
{
    [SerializeField] private int elementIndex;
    [SerializeField] private string armorNameZh;
    [SerializeField] private string armorNameEn;
    [SerializeField] private DamageModifiers damageModifiers;

    public int ElementIndex => elementIndex;
    public string ArmorNameZh => armorNameZh;
    public string ArmorNameEn => armorNameEn;
    public DamageModifiers DamageModifiers => damageModifiers;

    public void SetValue(int index,string armorNameZhValue, string armorNameEnValue)
    {
        elementIndex = index;
        this.armorNameZh = armorNameZhValue;
        armorNameEn = armorNameEnValue;
    }
    public void SetIndex(int index) => elementIndex = index;
    public void SetArmorNameZh(string newName) => armorNameZh = newName;
    public void SetArmorNameEn(string newName) => armorNameEn = newName;
    public void SetDamageModifiers(DamageTypeEnum damage, int value) => damageModifiers[damage] = value;
    
								

}





