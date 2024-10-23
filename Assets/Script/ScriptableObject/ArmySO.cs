using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.EditorTools;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmyOS", menuName = "ScriptableObjects/Data/ArmyOS"), SerializeField]
public class ArmySO : ScriptableObject,IBaseInfo, IArmy, IWeapon, ISkill
{
    [Header("IBaseInfo")]
    [Tooltip("派系"), SerializeField] private FactionSO faction;
    [Tooltip("编号"), SerializeField] private uint id;
    [Tooltip("中文名称"), SerializeField] private string nameChinese;
    [Tooltip("英文名称"), SerializeField] private string nameEnglish;
    [Tooltip("Icon"), SerializeField] private Sprite icon;
    [Tooltip("中文备注"), TextArea(), SerializeField] private string commentChinese;
    [Tooltip("英文备注"), TextArea(), SerializeField] private string commentEnglish;
    [Tooltip("兵种"), SerializeField] private Troop troop;
    [Tooltip("活动范围"), SerializeField] private List<ActionScope> actionScopes;
    [Tooltip("经验"), SerializeField] private uint exp;
    [Tooltip("生命值"), SerializeField] private uint hp;
    [Tooltip("科技前提"), SerializeField] private List<MainBuildingSO> requirement;
    [Tooltip("建造价格"), SerializeField] private uint price;
    [Tooltip("警戒/清雾半径"), SerializeField] private Vector2Int warningAndClearFogRad;
    [Tooltip("护甲类型"), SerializeField] private ArmorSO armorType;
    [Tooltip("预制体"), SerializeField] private GameObject gameObjectPrefab;

    [Header("IArmy")]
    [Tooltip("移动速度"), SerializeField] private Vector3 moveSpeed;
    [Tooltip("能否倒退移动"), SerializeField] private bool3 isReverseMove;
    [Tooltip("是否两栖"), SerializeField] private bool isAmphibious;
    [Tooltip("碾压等级/被碾压等级"), SerializeField] private CrushList crushingAndCrushedLevel;
    [Tooltip("标签"), SerializeField] private List<ArmyLabelSO> labels;
    [Tooltip("建造时间"), SerializeField] private uint buildingTime;
    [Tooltip("建造设施"), SerializeField] private MainBuildingSO buildFacilities;


    [Header("IWeapon")]
    [Tooltip("武器组"), SerializeField] private List<Weapon> weapons;

    [Header("ISkill")]
    [Tooltip("技能组"), SerializeField] private List<Skill> skills;







    public FactionSO Faction => faction;
    public uint Id => id;
    public string NameChinese => nameChinese;
    public string NameEnglish => nameEnglish;
    public string CommentChinese => commentChinese;
    public string CommentEnglish => commentEnglish;
    public Sprite Icon => icon;
    public Troop Troop => troop;
    public List<ActionScope> ActionScopes => actionScopes;
    public Vector3 MoveSpeed => moveSpeed;
    public uint Exp => exp;
    public bool IsAmphibious => isAmphibious;
    public bool3 IsReverseMove => isReverseMove;
    public CrushList CrushingAndCrushedLevel => crushingAndCrushedLevel;
    public Vector2Int WarningAndClearFogRad => warningAndClearFogRad;
    public List<ArmyLabelSO> Labels => labels;
    public List<MainBuildingSO> Requirement => requirement;
    public uint BuildingTime => buildingTime;
    public uint Price => price;
    public MainBuildingSO BuildFacilities => buildFacilities;
    public ArmorSO ArmorType => armorType;
    public uint Hp => hp;
    public List<Weapon> Weapons => weapons;
    public List<Skill> Skills => skills;
    public GameObject GameObjectPrefab => gameObjectPrefab;
}