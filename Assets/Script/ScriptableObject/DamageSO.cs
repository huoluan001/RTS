using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DamageTypeSO", menuName = "ScriptableObjects/Data/DamageTypeSO")]
public class DamageTypeSO : ScriptableObject
{
    public int index;
    public string damageTypeZH;
    public string damageTypeEN;

}