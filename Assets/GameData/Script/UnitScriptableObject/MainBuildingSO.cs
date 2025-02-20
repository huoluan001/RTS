using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameData.Script.Enum;
using UnityEngine;
using UnityEngine.UIElements;


[CreateAssetMenu(fileName = "MainBuildingSo", menuName = "ScriptableObjects/Data/MainBuildingSo")]
public partial class MainBuildingSo : ScriptableObject, IBaseInfo, IBuilding , ISkill
{
    #region So

    [Header("IBaseInfo")] [Tooltip("派系"), SerializeField]
    private FactionSo factionSo;

    [Tooltip("编号"), SerializeField] private int id;
    [Tooltip("优先级"), SerializeField] private int priority;
    [Tooltip("中文名称"), SerializeField] private string nameChinese;
    [Tooltip("英文名称"), SerializeField] private string nameEnglish;
    [Tooltip("Icon"), SerializeField] private Sprite icon;

    [Tooltip("中文备注"), TextArea(), SerializeField]
    private string commentChinese;

    [Tooltip("英文备注"), TextArea(), SerializeField]
    private string commentEnglish;

    [Tooltip("兵种"), SerializeField] private TroopType troopType;
    [Tooltip("活动范围"), SerializeField] private List<ActionScope> actionScopes;
    [Tooltip("经验"), SerializeField] private int exp;
    [Tooltip("生命值"), SerializeField] private int hp;
    [Tooltip("科技前提"), SerializeField] private List<MainBuildingSo> requirement;
    [Tooltip("建造价格"), SerializeField] private int price;
    [Tooltip("警戒/清雾半径"), SerializeField] private Vector2Int warningAndClearFogRad;
    [Tooltip("护甲类型"), SerializeField] private ArmorSo armorType;
    [Tooltip("预制体"), SerializeField] private GameObject gameObjectPrefab;

    [Header("IBuilding")] [Tooltip("用途"), SerializeField]
    private BuildingLabelEnum label;

    [Tooltip("占地面积"), SerializeField] private Vector2Int area;
    [Tooltip("拓展范围"), SerializeField] private Vector2Int expandScope;
    [Tooltip("建造/展开时长"), SerializeField] private Vector2 buildingAndPlacementTime;
    [Tooltip("电力消耗"), SerializeField] private int powerConsume;

    [Header("ISkill")] [Tooltip("技能组"), SerializeField]
    private List<Skill> skills;

    #endregion

    #region Interface

    public FactionSo FactionSo => factionSo;
    public int Id => id;
    public int Priority => priority;
    public string NameChinese => nameChinese;
    public string NameEnglish => nameEnglish;
    public Sprite Icon => icon;
    public string CommentChinese => commentChinese;
    public string CommentEnglish => commentEnglish;
    public TroopType TroopType => troopType;
    public List<ActionScope> ActionScopes => actionScopes;
    public BuildingLabelEnum Label => label;
    public int Exp => exp;
    public List<MainBuildingSo> Requirement => requirement;
    public Vector2Int Area => area;
    public Vector2 BuildingAndPlacementTime => buildingAndPlacementTime;
    public Vector2Int ExpandScope => expandScope;
    public int Price => price;
    public Vector2Int WarningAndClearFogRad => warningAndClearFogRad;
    public int PowerConsume => powerConsume;
    public ArmorSo ArmorType => armorType;
    public int Hp => hp;
    public List<Skill> Skills => skills;
    public GameObject GameObjectPrefab => gameObjectPrefab;

    #endregion

    public void SetRequirement(List<MainBuildingSo> mainBuildingSos)
    {
        requirement = mainBuildingSos;
    }

    public void SetBaseInfo(FactionSo factionSo, int id, string nameChinese, string nameEnglish, Sprite icon,
        string commentChinese, string commentEnglish, TroopType troopType, List<ActionScope> actionScopes, int exp,
        int hp, int price, List<MainBuildingSo> requirement, Vector2Int warningAndClearFogRad, ArmorSo armorType,
        GameObject gameObjectPrefab)
    {
        this.factionSo = factionSo;
        this.id = id;
        this.nameChinese = nameChinese;
        this.nameEnglish = nameEnglish;
        this.icon = icon;
        this.commentChinese = commentChinese;
        this.commentEnglish = commentEnglish;
        this.troopType = troopType;
        this.actionScopes = actionScopes;
        this.exp = exp;
        this.hp = hp;
        this.requirement = requirement;
        this.price = price;
        this.warningAndClearFogRad = warningAndClearFogRad;
        this.armorType = armorType;
        this.gameObjectPrefab = gameObjectPrefab;
    }

    public void SetBuilding(BuildingLabelEnum label, Vector2Int area, Vector2Int expandScope,
        Vector2 buildingAndPlacementTime, int powerConsume)
    {
        this.label = label;
        this.area = area;
        this.expandScope = expandScope;
        this.buildingAndPlacementTime = buildingAndPlacementTime;
        this.powerConsume = powerConsume;
    }

    public void SetSkill(string skillNameZH, string skillNameEN, string commentChinese, float skillCooling,
        float skillPre_Swing, float skillPost_Swing)
    {
        if (skills == null)
            skills = new List<Skill>();
        var res = skills.Where(s => s.skillNameEN == skillNameEN).ToList();
        Skill skill;
        if (res.Count() == 0)
            skills.Add(skill = new Skill());
        else
            skill = res[0];
        skill.skillNameZH = skillNameZH;
        skill.skillNameEN = skillNameEN;
        skill.commentChinese = commentChinese;
        skill.skillCooling = skillCooling;
        skill.skillPre_Swing = skillPre_Swing;
        skill.skillPost_Swing = skillPost_Swing;
    }

    public void SetIcon(Sprite sprite)
    {
        icon = sprite;
    }
}