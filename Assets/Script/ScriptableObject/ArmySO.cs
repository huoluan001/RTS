using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmyOS", menuName = "ScriptableObjects/Data/ArmyOS"), SerializeField]
public class ArmySO : ScriptableObject, IArmy, IWeapon, ISkill, IBaseInfo
{
    [Header("Info")]
    [Tooltip("派系"), SerializeField] private FactionSO faction;
    [Tooltip("编号"), SerializeField] private uint id;
    [Tooltip("中文名称"), SerializeField] private string nameChinese;
    [Tooltip("英文名称"), SerializeField] private string nameEnglish;
    [Tooltip("中文备注"), TextArea(), SerializeField] private string commentChinese;
    [Tooltip("英文备注"), TextArea(), SerializeField] private string commentEnglish;
    [Tooltip("Icon"), SerializeField] private Sprite icon;
    [Tooltip("兵种"), SerializeField] private Troop troop;
    [Tooltip("行动范围"), SerializeField] private List<ActionScope> actionScopeList;
    [Tooltip("移动速度"), SerializeField] private Vector3 moveSpeed;
    [Tooltip("经验"), SerializeField] private uint exp;
    [Tooltip("是否两栖"), SerializeField] private bool isAmphibious;
    [Tooltip("能否倒退移动"), SerializeField] private bool3 isReverseMove;
    [Tooltip("碾压等级/被碾压等级"), SerializeField] private CrushList crushingAndCrushedLevel;
    [Tooltip("警戒/清雾半径"), SerializeField] private Vector2Int warningAndClearFogRad;
    [Tooltip("标签"), SerializeField] private List<ArmyLabelSO> labels;
    [Tooltip("科技前提"), SerializeField] private List<GameObject> requirement;
    [Tooltip("建造时间"), SerializeField] private uint buildingTime;
    [Tooltip("建造价格"), SerializeField] private uint buildingPrice;
    [Tooltip("建造设施"), SerializeField] private MainBuildingSO buildFacilities;
    [Tooltip("武器"), SerializeField] private List<Weapon> weapons;
    [Tooltip("技能"), SerializeField] private List<Skill> skills;
    [Tooltip("护甲类型"), SerializeField] private ArmorSO armor;
    [Tooltip("生命值"), SerializeField] public uint hp;

    public FactionSO Faction => faction;
    public uint Id => id;
    public string NameChinese => nameChinese;
    public string NameEnglish => nameEnglish;
    public string CommentChinese => commentChinese;
    public string CommentEnglish => commentEnglish;
    public Sprite Icon => icon;
    public Troop Troop => troop;
    public List<ActionScope> ActionScopes => actionScopeList;
    public Vector3 MoveSpeed => moveSpeed;
    public uint Exp => exp;
    public bool IsAmphibious => isAmphibious;
    public bool3 IsReverseMove => isReverseMove;
    public CrushList CrushingAndCrushedLevel => crushingAndCrushedLevel;
    public Vector2Int WarningAndClearFogRad => warningAndClearFogRad;
    public List<ArmyLabelSO> Labels => labels;
    public List<GameObject> Requirement => requirement;
    public uint BuildingTime => buildingTime;
    public uint BuildingPrice => buildingPrice;
    public MainBuildingSO BuildFacilities => buildFacilities;
    public ArmorSO ArmorType => armor;
    public uint Hp => hp;
    public List<Weapon> Weapons => weapons;
    public List<Skill> Skills => skills;

}