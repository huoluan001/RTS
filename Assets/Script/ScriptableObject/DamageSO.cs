using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DamageTypeSo", menuName = "ScriptableObjects/Data/DamageTypeSo")]
public class DamageTypeSo : ScriptableObject
{
    public int index;
    public string damageTypeZH;
    public string damageTypeEN;

}