using System.Collections.Generic;
using UnityEngine;
public class Army : MonoBehaviour
{
    public UnitComponent unitComponent;
    public ArmySO ArmySO;
#region 公开控制信息
    public Player commander;
    private uint CurrentMaxHP;
    private uint CurrentHP;
    private uint CurrentUpgradeExp;
    private uint CurrentExp;
    private Level Level;
    private ArmyBehaviorModel ArmyBehaviorModel;
#endregion
    
#region Move
    private float MoveSpeed;
    private Vector3 MoveTarget;
    private float AngularSpeed;

    private List<ActionScope> CurrentActionScopes;
#endregion

#region Attack
    private List<AttackModel> CurrentAttackModels;
    private List<CareerOrientation> CareerOrientations;
    private GameObject AttackTarget;

#endregion

#region 逻辑控制信息
    private bool isAttacking;
    private bool isMoving;
    private bool isForcedAttack;
    private bool isAdvanceAttack;
#endregion

    private GameObject SkillTarget;

    private void Awake()
    {

    }
    private void Start()
    {
        
    }

    private void Update()
    {

    }

}