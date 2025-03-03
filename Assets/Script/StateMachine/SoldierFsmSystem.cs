using UnityEngine;
using System.Collections.Generic;
using Script.Element;

public class SoldierFsmSystem
{
    private readonly Dictionary<SoldierState, ISoldierFsmState> state_Map;
    public ISoldierFsmState CurrentSoldierState = null;
    public ArmySo ArmySo;
    public readonly Soldier Soldier;

    public SoldierFsmSystem(ArmySo armySo, Soldier soldier)
    {
        this.ArmySo = armySo;
        this.Soldier = soldier;
        state_Map = new Dictionary<SoldierState, ISoldierFsmState>()
        {
            { SoldierState.Idle, new IdleSoldierFsmState(this) },
            { SoldierState.Move, new MoveSoldierFsmState(this) },
            { SoldierState.Attack, new AttackSoldierFsmState(this) }
        };
        CurrentSoldierState = state_Map[SoldierState.Idle];
        soldier.PlayIdleMotion();
    }

    private void SwitchState(SoldierState state)
    {
        CurrentSoldierState.Exit();
        CurrentSoldierState = state_Map[state];
        CurrentSoldierState.Enter();
    }
    
    public void CheckTransition()
    {
        // 如果没有攻击目标和移动目标，则保持Idle
        if (Soldier.AttackTargetIsNull() && Soldier.MoveTargetIsNull())
        {
            SwitchState(SoldierState.Idle);
            return;
        }
        // 如果有移动目标，切换到Move状态
        if (!Soldier.MoveTargetIsNull())
        {
            SwitchState(SoldierState.Move);
            return;
        }
        // 如果有攻击目标且在攻击范围内，切换到Attack状态
        SwitchState(Soldier.AttackTargetInRange() ? SoldierState.Attack : SoldierState.Move);
    }
}

public interface ISoldierFsmState
{
    void Enter();
    void Run();
    void Exit();
}

public enum SoldierState
{
    Idle,
    Move,
    Attack
}

public class IdleSoldierFsmState : ISoldierFsmState
{
    private SoldierFsmSystem _soldierFsmSystem;
    private Soldier _soldier;

    public IdleSoldierFsmState(SoldierFsmSystem soldierFsmSystem)
    {
        _soldierFsmSystem = soldierFsmSystem;
        _soldier = _soldierFsmSystem.Soldier;
    }

    public void Enter()
    {
        _soldier.PlayIdleMotion();
    }

    public void Run()
    {
    }

    public void Exit()
    {
    }
}
public class MoveSoldierFsmState : ISoldierFsmState
{
    private float _moveSpeed;
    private SoldierFsmSystem _soldierFsmSystem;
    private readonly Soldier _soldier;

    public MoveSoldierFsmState(SoldierFsmSystem soldierFsmSystem)
    {
        _soldierFsmSystem = soldierFsmSystem;
        _soldier = _soldierFsmSystem.Soldier;
    }

    public void Enter()
    {
        _soldier.PlayMoveMotion();
    }

    public void Run()
    {
        var targetPos = _soldier.GetMovePos();
        if(targetPos == null)
            return;
        var moveDir = targetPos - _soldier.transform.position;
        _soldier.transform.Translate(moveDir.Value * (_moveSpeed * Time.deltaTime));
    }

    public void Exit()
    {
    }
}
public class AttackSoldierFsmState : ISoldierFsmState
{
    private readonly SoldierFsmSystem _soldierFsmSystem;
    private readonly Soldier _soldier;
    private Weapon _weapon;
    private float _fireTime;
    
    private float _bulletLoadTime;
    private float _aimTime;
    private int _maxBulletCount;
    private int _currentBulletCount;


    private void WeaponChanged(Weapon weapon)
    {
        _weapon = weapon;
        _fireTime = weapon.FiringDuration;
        _bulletLoadTime = weapon.MagazineLoadingTime;
        _aimTime = weapon.AimingTime;
        _maxBulletCount = weapon.magazineSize;
        _currentBulletCount = _maxBulletCount;
    }
    
    public AttackSoldierFsmState(SoldierFsmSystem soldierFsmSystem)
    {
        _soldierFsmSystem = soldierFsmSystem;
        _soldier = _soldierFsmSystem.Soldier;
        WeaponChanged(_soldier.currentWeapon);

        _soldier.FireCallback += FireCallBack;
        _soldier.BulletLoadCallback += BulletLoadCallBack;
    }

    private void FireCallBack()
    {
        _currentBulletCount--;
        if (_currentBulletCount == 0)
        {
            _soldier.PlayBulletLoadMotion();
        }
    }

    private void BulletLoadCallBack()
    {
        _currentBulletCount = _maxBulletCount;
        
        if (_soldierFsmSystem.CurrentSoldierState == this)
        {
            _soldier.PlayFireMotion();
        }
        
    }

    public void Enter()
    {
        if (_currentBulletCount == 0)
        {
            _soldier.PlayBulletLoadMotion();
        }
        else
        {
            _soldier.PlayFireMotion();
        }
    }

    public void Run()
    {

    }

    public void Exit()
    {
        if (_currentBulletCount == _maxBulletCount)
        {
            _soldier.PlayBulletLoadAnimationInUpBody();
        }
    }
}