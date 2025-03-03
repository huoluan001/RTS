using System;
using Animancer;
using UnityEngine;

// Army声明Army共有的字段，组件，并在Awake中完成组件赋值
public partial class Army : Unit
{
    public ArmySo armySo;
    private Vector3? _moveTarget = null;
    [SerializeField] private Unit _attackTarget;
    public bool canMoveAttack;
    public AttackModel currentAttackModel;

    protected Action TargetChangedCallBack;
    public Weapon currentWeapon;

    private void Awake()
    {
        animancer = GetComponent<AnimancerComponent>();
    }

    


    

    
}