
using Animancer;
using UnityEngine;


public class Army : Unit
{
    public ArmySo armySo;
    public Vector3? MoveTarget = null;
    public Unit attackTarget;

    public Weapon currentWeapon;
}