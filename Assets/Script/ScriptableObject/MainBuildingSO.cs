using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


[CreateAssetMenu(fileName = "MainBuildingSO", menuName = "ScriptableObjects/Data/MainBuildingSO")]
public partial class MainBuildingSO : ScriptableObject, IBaseInfo, IBuilding, ISkill
{
    #region SO
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

    [Header("IBuilding")]
    [Tooltip("用途"), SerializeField] private BuildingLabelSO label;
    [Tooltip("占地面积"), SerializeField] private Vector2Int area;
    [Tooltip("拓展范围"), SerializeField] private Vector2Int expandScope;
    [Tooltip("建造/展开时长"), SerializeField] private Vector2 buildingAndPlacementTime;
    [Tooltip("电力消耗"), SerializeField] private int powerConsume;

    [Header("ISkill")]
    [Tooltip("技能组"), SerializeField] private List<Skill> skills;
    #endregion

    #region Interface
    public FactionSO FactionSO => factionSO;
    public int Id => id;
    public string NameChinese => nameChinese;
    public string NameEnglish => nameEnglish;
    public Sprite Icon => icon;
    public string CommentChinese => commentChinese;
    public string CommentEnglish => commentEnglish;
    public Troop Troop => troop;
    public List<ActionScope> ActionScopes => actionScopes;
    public BuildingLabelSO Label => label;
    public int Exp => exp;
    public List<MainBuildingSO> Requirement => requirement;
    public Vector2Int Area => area;
    public Vector2 BuildingAndPlacementTime => buildingAndPlacementTime;
    public Vector2Int ExpandScope => expandScope;
    public int Price => price;
    public Vector2Int WarningAndClearFogRad => warningAndClearFogRad;
    public int PowerConsume => powerConsume;
    public ArmorSO ArmorType => armorType;
    public int Hp => hp;
    public List<Skill> Skills => skills;
    public GameObject GameObjectPrefab => gameObjectPrefab;
    #endregion
    
    public void SetRequirement(List<MainBuildingSO> mainBuildingSOs)
    {
        requirement = mainBuildingSOs;
    }
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
    public void SetBuilding(BuildingLabelSO label, Vector2Int area, Vector2Int expandScope, Vector2 buildingAndPlacementTime, int powerConsume)
    {
        this.label = label;
        this.area = area;
        this.expandScope = expandScope;
        this.buildingAndPlacementTime = buildingAndPlacementTime;
        this.powerConsume = powerConsume;
    }

    public void SetSkill(string skillNameZH, string skillNameEN, string commentChinese, int skillCooling, int skillPre_Swing, int skillPost_Swing)
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
}
