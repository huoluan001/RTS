using System;
using UnityEditor.EditorTools;
using UnityEngine;

[Serializable]
public class Weapon
{
    public string WeaponNameZH;
    public string WeaponNameEN;
    
    [Tooltip("伤害类型")] public DamageTypeSO damageType;
    [Tooltip("弹夹大小")] public float magazineSize;
    [Tooltip("射程"),SerializeField] public Vector2 _range;
    [Tooltip("溅射半径"),SerializeField] private Vector2 _sputteringRadius;
    [Tooltip("溅射伤害"), SerializeField] private Vector2 _sputteringDamage;
    [Tooltip("单发伤害"), SerializeField] private Vector2 _singleDamage;
    [Tooltip("瞄准时间"), SerializeField] private Vector2 _aimingTime;
    [Tooltip("开火持续时间"), SerializeField] private Vector2 _firingDuration;
    [Tooltip("弹夹装填时间"), SerializeField] private Vector2 _magazineLoadingTime;
    [Tooltip("一轮攻击时间"), SerializeField] private Vector2 _oneAttackTime;
    [Tooltip("一轮攻击伤害"), SerializeField] private Vector2 _oneAttackDamage;
    [Tooltip("DPS"), SerializeField] private Vector2 _DPS;

    public float Range => GetRandomValue(_range);
    public float SputteringRadius => GetRandomValue(_sputteringRadius);
    public float SputteringDamage => GetRandomValue(_sputteringDamage);
    public float SingleDamage => GetRandomValue(_singleDamage);
    public float AimingTime => GetRandomValue(_aimingTime);
    public float FiringDuration => GetRandomValue(_firingDuration);
    public float MagazineLoadingTime => GetRandomValue(_magazineLoadingTime);
    public float OneAttackTime => GetRandomValue(_oneAttackTime);
    public float OneAttackDamage => GetRandomValue(_oneAttackDamage);
    public float DPS => GetRandomValue(_DPS);
    private float GetRandomValue(Vector2 range)
    {
        return range.x == range.y ? range.x : UnityEngine.Random.Range(range.x, range.y);
    }
}
