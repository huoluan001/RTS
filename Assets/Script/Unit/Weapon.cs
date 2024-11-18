using System;
using UnityEditor.EditorTools;
using UnityEngine;

[Serializable]
public class Weapon
{
    
    public string WeaponNameZH;
    public string WeaponNameEN;
    
    [Header("Damage")]
    [Tooltip("伤害类型")] public DamageTypeSo damageType;
    [Tooltip("单发伤害"), SerializeField] public Vector2 singleDamage;
    [Tooltip("射程"),SerializeField] public Vector2 range;
    [Tooltip("弹夹大小")] public int magazineSize;

    [Header("Time")]
    [Tooltip("弹夹装填时间"), SerializeField] public Vector2 magazineLoadingTime;
    [Tooltip("瞄准时间"), SerializeField] public Vector2 aimingTime;
    [Tooltip("开火持续时间"), SerializeField] public Vector2 firingDuration;

    [Header("溅射")]
    [Tooltip("溅射半径"),SerializeField] public Vector2 sputteringRadius;
    [Tooltip("溅射伤害"), SerializeField] public Vector2 sputteringDamage;

    public float Range => GetRandomValue(range);
    public float SputteringRadius => GetRandomValue(sputteringRadius);
    public float SputteringDamage => GetRandomValue(sputteringDamage);
    public float SingleDamage => GetRandomValue(singleDamage);
    public float AimingTime => GetRandomValue(aimingTime);
    public float FiringDuration => GetRandomValue(firingDuration);
    public float MagazineLoadingTime => GetRandomValue(magazineLoadingTime);
    private float GetRandomValue(Vector2 range)
    {
        return range.x == range.y ? range.x : UnityEngine.Random.Range(range.x, range.y);
    }

    public void SetWeaponData()
    {

    }
}
