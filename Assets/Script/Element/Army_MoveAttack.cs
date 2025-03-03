using System;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public partial class Army
{

    public HashSet<GameObject> CanSeeTargets = new HashSet<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            Debug.LogWarning($"{other.name} entered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if (other != null && CanSeeTargets.Contains(other.gameObject))
        // {
        //     CanSeeTargets.Remove(other.gameObject);
        //     Debug.Log($"{other.name} Exited");
        // }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            Debug.Log($"{other.name} Stayed");
        }
    }

    public void AddAttackTarget(Unit target)
    {
        _attackTarget = target;
        if (!canMoveAttack)
            _moveTarget = null;
        TargetChangedCallBack?.Invoke();
    }
    public void AddMoveTarget(Vector3 target)
    {
        _moveTarget = target;
        if (!canMoveAttack)
            _attackTarget = null;
        TargetChangedCallBack?.Invoke();
    }
    public void CancelMoveTarget()
    {
        _moveTarget = null;
        TargetChangedCallBack?.Invoke();
    }
    public void CancelAttackTarget()
    {
        _attackTarget = null;
        TargetChangedCallBack?.Invoke();
    }
    public void CancelMoveAndAttackTarget()
    {
        _moveTarget = null;
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