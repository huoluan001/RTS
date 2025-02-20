using System;
using System.Collections.Generic;
using GameData.Script.Enum;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public interface IWeapon
{
    List<Weapon> Weapons { get; }

    void SetWeapon(string weaponNameZH, string weaponNameEN, DamageTypeEnum damageType, Vector2 singleDamage,
        Vector2 range, int magazineSize, Vector2 magazineLoadingTime, Vector2 aimingTime, Vector2 firingDuration,
        Vector2 sputteringRadius, Vector2 sputteringDamage);
}