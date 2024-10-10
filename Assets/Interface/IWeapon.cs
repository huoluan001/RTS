using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

interface IWeapon
{
    List<Weapon> Weapons { get; }
}