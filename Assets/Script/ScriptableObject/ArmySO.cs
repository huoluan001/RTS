using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.EditorTools;
using UnityEngine;


[CreateAssetMenu(fileName = "ArmySO", menuName = "ScriptableObjects/Data/ArmySO"), SerializeField]
public class ArmySO : ScriptableObject, IBaseInfo, IArmy, IWeapon, ISkill
{
    [Header("IBaseInfo")]
    [Tooltip("派系"), SerializeField] private FactionSO factionSO;
    [Tooltip("编号"), SerializeField] private int id;
    [Tooltip("中文名称"), SerializeField] private string nameChinese;
    [Tooltip("英文名称"), SerializeField] private string nameEnglish;
    [Tooltip("Icon"), SerializeField] private Sprite icon;
    [Tooltip("中文备注"), TextArea(), SerializeField] private string commentChinese;
    [Tooltip("英文备注"), TextArea(), SerializeField] private string commentEnglish;
    [Tooltip("兵种"), SerializeField] private Troop troop;
    [Tooltip("活动范围"), SerializeField] private List<ActionScope> actionScopes;
    [Tooltip("经验"), SerializeField] private int exp;
    [Tooltip("生命值"), SerializeField] private int hp;
    [Tooltip("科技前提"), SerializeField] private List<MainBuildingSO> requirement;
    [Tooltip("建造价格"), SerializeField] private int price;
    [Tooltip("警戒/清雾半径"), SerializeField] private Vector2Int warningAndClearFogRad;
    [Tooltip("护甲类型"), SerializeField] private ArmorSO armorType;
    [Tooltip("预制体"), SerializeField] private GameObject gameObjectPrefab;

    [Header("IArmy")]
    [Tooltip("移动速度"), SerializeField] private Vector3 moveSpeed;
    [Tooltip("能否倒退移动"), SerializeField] private bool3 isReverseMove;
    [Tooltip("是否两栖"), SerializeField] private bool isAmphibious;
    [Tooltip("碾压等级/被碾压等级"), SerializeField] private CrushList crushingAndCrushedLevel;
    [Tooltip("标签"), SerializeField] private List<ArmyLabelSO> labels;
    [Tooltip("建造时间"), SerializeField] private int buildingTime;
    [Tooltip("建造设施"), SerializeField] private MainBuildingSO buildFacilities;

    [Header("IWeapon")]
    [Tooltip("武器组"), SerializeField] private List<Weapon> weapons;

    [Header("ISkill")]
    [Tooltip("技能组"), SerializeField] private List<Skill> skills;







    public FactionSO FactionSO => factionSO;
    public int Id => id;
    public string NameChinese => nameChinese;
    public string NameEnglish => nameEnglish;
    public string CommentChinese => commentChinese;
    public string CommentEnglish => commentEnglish;
    public Sprite Icon => icon;
    public Troop Troop => troop;
    public List<ActionScope> ActionScopes => actionScopes;
    public Vector3 MoveSpeed => moveSpeed;
    public int Exp => exp;
    public bool IsAmphibious => isAmphibious;
    public bool3 IsReverseMove => isReverseMove;
    public CrushList CrushingAndCrushedLevel => crushingAndCrushedLevel;
    public Vector2Int WarningAndClearFogRad => warningAndClearFogRad;
    public List<ArmyLabelSO> Labels => labels;
    public List<MainBuildingSO> Requirement => requirement;
    public int BuildingTime => buildingTime;
    public int Price => price;
    public MainBuildingSO BuildFacilities => buildFacilities;
    public ArmorSO ArmorType => armorType;
    public int Hp => hp;
    public List<Weapon> Weapons => weapons;
    public List<Skill> Skills => skills;
    public GameObject GameObjectPrefab { get => gameObjectPrefab; }



    public void SetBaseInfo(FactionSO factionSO, int id, string nameChinese, string nameEnglish, Sprite icon, string commentChinese, string commentEnglish, Troop troop, List<ActionScope> actionScopes, int exp, int hp, int price, List<MainBuildingSO> requirement, Vector2Int warningAndClearFogRad, ArmorSO armorType, GameObject gameObjectPrefab)
    {
        this.factionSO = factionSO;
        this.id = id;
        this.nameChinese = nameChinese;
        this.nameEnglish = nameEnglish;
        this.icon = icon;
        this.commentChinese = commentChinese;
        this.commentEnglish = commentEnglish;
        this.troop = troop;
        this.actionScopes = actionScopes;
        this.exp = exp;
        this.hp = hp;
        this.requirement = requirement;
        this.price = price;
        this.warningAndClearFogRad = warningAndClearFogRad;
        this.armorType = armorType;
        this.gameObjectPrefab = gameObjectPrefab;

    }
    public void SetArmy(Vector3 moveSpeed, bool3 isReverseMove, bool isAmphibious,CrushList crushingAndCrushedLevel, List<ArmyLabelSO> labels,int buildingTime, MainBuildingSO buildFacilities)
    {
        this.moveSpeed = moveSpeed;
        this.isReverseMove = isReverseMove;
        this.isAmphibious = isAmphibious;
        this.crushingAndCrushedLevel = crushingAndCrushedLevel;
        this.labels = labels;
        this.buildingTime = buildingTime;
        this.buildFacilities = buildFacilities;
    }
}