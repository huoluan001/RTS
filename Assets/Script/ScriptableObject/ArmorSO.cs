using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Linq;

[Serializable]
[CreateAssetMenu(fileName = "ArmorSO", menuName = "ScriptableObjects/Data/ArmorSO")]
public class ArmorSO : ScriptableObject
{
    [SerializeField] private uint elementIndex;
    [SerializeField] private string armorNameZH;
    [SerializeField] private string armorNameEN;
    [SerializeField] private DamageModifiers damageModifiers;
    public DamageModifiers DamageModifiers => damageModifiers;
    public string ArmorNameZH => armorNameZH;
    public string ArmorNameEN => armorNameEN;
    public uint ElementIndex => elementIndex;


    // 肉搏,Melee;
    // 狙击,Sniper;
    // 枪弹,Bomb;
    // 机炮,MachineGun;
    // 破片,Fragment;
    // 火箭,Rocket;
    // 穿甲,ArmorPiercing;
    // 光谱,Spectrum;
    // 电击,ElectricShock;
    // 高爆,HighExplosive;
    // 榴弹,Grenade;
    // 鱼雷,Torpedo;
    // 冲击,Shock;
    // 辐射,Radiation;									

}





