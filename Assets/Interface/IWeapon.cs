using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public interface IWeapon
{
    List<Weapon> Weapons { get; }
    void SetWeapon(string weaponNameZH, string weaponNameEN, DamageTypeSO damageType, Vector2 singleDamage, Vector2 range, int magazineSize, Vector2 magazineLoadingTime, Vector2 aimingTime, Vector2 firingDuration, Vector2 sputteringRadius, Vector2 sputteringDamage);


}