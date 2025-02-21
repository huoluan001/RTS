using System.Collections.Generic;
using System.Linq;
using GameData.Script.Enum;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmySo", menuName = "ScriptableObjects/Data/ArmySo"), SerializeField]
public class ArmySo : ScriptableObject, IBaseInfo, IArmy, IWeapon, ISkill
{
    [Header("IBaseInfo")]
    [Tooltip("派系"), SerializeField] private FactionSo factionSo;
    [Tooltip("编号"), SerializeField] private int id;
    [Tooltip("优先级"), SerializeField] private int priority;
    [Tooltip("中文名称"), SerializeField] private string nameChinese;
    [Tooltip("英文名称"), SerializeField] private string nameEnglish;
    [Tooltip("Icon"), SerializeField] private Sprite icon;
    [Tooltip("中文备注"), TextArea(), SerializeField] private string commentChinese;
    [Tooltip("英文备注"), TextArea(), SerializeField] private string commentEnglish;
    [Tooltip("兵种"), SerializeField] private TroopType troopType;
    [Tooltip("活动范围"), SerializeField] private List<ActionScope> actionScopes;
    [Tooltip("经验"), SerializeField] private int exp;
    [Tooltip("生命值"), SerializeField] private int hp;
    [Tooltip("科技前提"), SerializeField] private List<MainBuildingSo> requirement;
    [Tooltip("建造价格"), SerializeField] private int price;
    [Tooltip("警戒/清雾半径"), SerializeField] private Vector2Int warningAndClearFogRad;
    [Tooltip("护甲类型"), SerializeField] private ArmorSo armorType;
    [Tooltip("预制体"), SerializeField] private GameObject gameObjectPrefab;

    [Header("IArmy")]
    [Tooltip("移动速度"), SerializeField] private Vector3 moveSpeed;
    [Tooltip("能否倒退移动"), SerializeField] private bool3 isReverseMove;
    [Tooltip("是否两栖"), SerializeField] private bool isAmphibious;
    [Tooltip("碾压等级/被碾压等级"), SerializeField] private CrushList crushingAndCrushedLevel;
    [Tooltip("标签"), SerializeField] private List<ArmyLabelEnum> labels;
    [Tooltip("建造时间"), SerializeField] private int buildingTime;
    [Tooltip("建造设施"), SerializeField] private List<MainBuildingSo> buildFacilities;

    [Header("IWeapon")]
    [Tooltip("武器组"), SerializeField] private List<Weapon> weapons;

    [Header("ISkill")]
    [Tooltip("技能组"), SerializeField] private List<Skill> skills;
    
    public FactionSo FactionSo => factionSo;
    public int Id => id;
    public int Priority => priority;
    public string NameChinese => nameChinese;
    public string NameEnglish => nameEnglish;
    public string CommentChinese => commentChinese;
    public string CommentEnglish => commentEnglish;
    public Sprite Icon => icon;
    public TroopType TroopType => troopType;
    public List<ActionScope> ActionScopes => actionScopes;
    public Vector3 MoveSpeed => moveSpeed;
    public int Exp => exp;
    public bool IsAmphibious => isAmphibious;
    public bool3 IsReverseMove => isReverseMove;
    public CrushList CrushingAndCrushedLevel => crushingAndCrushedLevel;
    public Vector2Int WarningAndClearFogRad => warningAndClearFogRad;
    public List<ArmyLabelEnum> Labels => labels;
    public List<MainBuildingSo> Requirement => requirement;
    public int BuildingTime => buildingTime;
    public int Price => price;
    public List<MainBuildingSo> BuildFacilities => buildFacilities;
    public ArmorSo ArmorType => armorType;
    public int Hp => hp;
    public List<Weapon> Weapons => weapons;
    public List<Skill> Skills => skills;
    public GameObject GameObjectPrefab => gameObjectPrefab;

    public void SetBaseInfo(FactionSo factionSo, int id, string nameChinese, string nameEnglish, Sprite icon, string commentChinese, string commentEnglish, TroopType troopType, List<ActionScope> actionScopes, int exp, int hp, int price, List<MainBuildingSo> requirement, Vector2Int warningAndClearFogRad, ArmorSo armorType, GameObject gameObjectPrefab)
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


    public void SetArmy(Vector3 moveSpeed, bool3 isReverseMove, bool isAmphibious, CrushList crushingAndCrushedLevel, List<ArmyLabelEnum> labels, int buildingTime, List<MainBuildingSo> buildFacilities)
    {
        this.moveSpeed = moveSpeed;
        this.isReverseMove = isReverseMove;
        this.isAmphibious = isAmphibious;
        this.crushingAndCrushedLevel = crushingAndCrushedLevel;
        this.labels = labels;
        this.buildingTime = buildingTime;
        this.buildFacilities = buildFacilities;
    }

    public void SetWeapon(string weaponNameZH, string weaponNameEN, DamageTypeEnum damageType, Vector2 singleDamage, Vector2 range, int magazineSize, Vector2 magazineLoadingTime, Vector2 aimingTime, Vector2 firingDuration, Vector2 sputteringRadius, Vector2 sputteringDamage)
    {
        if (weapons == null)
            weapons = new List<Weapon>();
        var res = weapons.Where(w => w.weaponNameZh == weaponNameZH).ToList();
        Weapon weapon;
        if (res.Count() == 0)
            weapons.Add(weapon = new Weapon());
        else
            weapon = res[0];
        weapon.weaponNameZh = weaponNameZH;
        weapon.weaponNameEn = weaponNameEN;
        weapon.damageType = damageType;
        weapon.singleDamage = singleDamage;
        weapon.range = range;
        weapon.magazineSize = magazineSize;
        weapon.magazineLoadingTime = magazineLoadingTime;
        weapon.aimingTime = aimingTime;
        weapon.firingDuration = firingDuration;
        weapon.sputteringRadius = sputteringRadius;
        weapon.sputteringDamage = sputteringDamage;
    }


    public void SetRequirement(List<MainBuildingSo> mainBuildingSos)
    {
        requirement = mainBuildingSos;
    }
    public void SetSkill(string skillNameZH, string skillNameEN, string commentChinese, float skillCooling, float skillPre_Swing, float skillPost_Swing)
    {
        if (skills == null)
            skills = new List<Skill>();
        var res = skills.Where(s => s.skillNameEn == skillNameEN).ToList();
        Skill skill;
        if (res.Count() == 0)
            skills.Add(skill = new Skill());
        else
            skill = res[0];
        skill.skillNameZh = skillNameZH;
        skill.skillNameEn = skillNameEN;
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