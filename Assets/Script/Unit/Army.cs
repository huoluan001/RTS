using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class Army : MonoBehaviour
{
    public UnitComponent unitComponent;
    public ArmySO ArmySO;
#region 公开控制信息
    public Player commander;
    private int CurrentMaxHP;
    private int CurrentHP;
    private int CurrentUpgradeExp;
    private int CurrentExp;
    private Level Level;
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

    public IActionModel currentActionModel;
    

    private void Awake()
    {
        currentActionModel.SwitchAcionModel(currentActionModel, ActionModelEnum.Alert);
    }
    private void Start()
    {
        
    }

    private void Update()
    {

    }

}