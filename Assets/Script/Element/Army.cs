using System;
using Animancer;
using UnityEngine;

// Army声明Army共有的字段，组件，并在Awake中完成组件赋值
public class Army : Unit
{
    public ArmySo armySo;
    private Vector3? _moveTarget = null;
    [SerializeField] private Unit _attackTarget;

    protected Action TargetChangedCallBack;
    public Weapon currentWeapon;

    private void Awake()
    {
        animancer = GetComponent<AnimancerComponent>();
    }

    [ContextMenu("AddAttack")]
    public void AddAttackTarget()
    {
        // _attackTarget = target;
        _attackTarget = this;
        _moveTarget = null;
        TargetChangedCallBack?.Invoke();
    }

    [ContextMenu("AddMove")]
    public void AddMoveTarget()
    {
        // _moveTarget = target;
        _moveTarget = Vector3.back;
        _attackTarget = null;
        TargetChangedCallBack?.Invoke();
    }

    [ContextMenu("CanMove")]
    public void CancelMoveTarget()
    {
        _moveTarget = null;
        TargetChangedCallBack?.Invoke();
    }

    [ContextMenu("CanAttack")]
    public void CancelAttackTarget()
    {
        _attackTarget = null;
        TargetChangedCallBack?.Invoke();
    }


    public bool AttackTargetInRange()
    {
        var distance = Vector3.Distance(transform.position, _attackTarget.transform.position);
        return distance < currentWeapon.Range;
    }

    public bool AttackTargetIsNull() => _attackTarget == null;
    public bool MoveTargetIsNull() => _moveTarget == null;

    public Vector3? GetMovePos()
    {
        if (_moveTarget != null)
            return _moveTarget;
        if (_attackTarget != null)
        {
            return _attackTarget.transform.position;
        }
        return null;
    }
}