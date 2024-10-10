using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[CreateAssetMenu(fileName = "OtherBuildingSO", menuName = "ScriptableObjects/Data/OtherBuildingSO")]
public class OtherBuildingSO : ScriptableObject, IBuilding,IWeapon,ISkill
{
    [Header("info")]
    [Tooltip("派系"), SerializeField] private FactionSO faction;
    [Tooltip("编号"), SerializeField] private uint id;
    [Tooltip("中文名称"), SerializeField] private string nameChinese;
    [Tooltip("英文名称"), SerializeField] private string nameEnglish;
    [Tooltip("Icon"), SerializeField] private Sprite icon;
    [Tooltip("中文备注"), TextArea(), SerializeField] private string commentChinese;
    [Tooltip("英文备注"), TextArea(), SerializeField] private string commentEnglish;
    [Tooltip("兵种"), SerializeField] private Troop troop;
    [Tooltip("活动范围"), SerializeField] private List<ActionScope> actionScopes;
    [Tooltip("用途"), SerializeField] private BuildingLabelSO label;
    [Tooltip("经验"), SerializeField] private uint exp;
    [Tooltip("科技前提"), SerializeField] private List<MainBuildingSO> requirement;
    [Tooltip("占地面积"), SerializeField] private Vector2Int area;
    [Tooltip("建造/展开时长"), SerializeField] private Vector2Int buildingAndPlacementTime;
    [Tooltip("拓展范围"), SerializeField] private Vector2Int expandScope;
    [Tooltip("建造价格"), SerializeField] private uint buildingPrice;
    [Tooltip("警戒/清雾半径"), SerializeField] private Vector2Int warningAndClearFogRad;
    [Tooltip("电力消耗"), SerializeField] private int powerConsume;
    [Tooltip("武器组"), SerializeField] public List<Weapon> weapons;
    [Tooltip("技能组"), SerializeField] private List<Skill> skills;
    [Tooltip("护甲类型"), SerializeField] private ArmorSO armorType;
    [Tooltip("生命值"), SerializeField] private uint hp;
    
    public FactionSO Faction => faction;
    public uint Id => id;
    public string NameChinese => nameChinese;
    public string NameEnglish => nameEnglish;
    public Sprite Icon => icon;
    public string CommentChinese => commentChinese;
    public string CommentEnglish => commentEnglish;
    public Troop Troop => troop;
    public List<ActionScope> ActionScopes => actionScopes;
    public BuildingLabelSO Label => label;
    public uint Exp => exp;
    public List<MainBuildingSO> Requirement => requirement;
    public Vector2Int Area => area;
    public Vector2Int BuildingAndPlacementTime => buildingAndPlacementTime;
    public Vector2Int ExpandScope => expandScope;
    public uint BuildingPrice => buildingPrice;
    public Vector2Int WarningAndClearFogRad => warningAndClearFogRad;
    public int PowerConsume => powerConsume;
    public ArmorSO ArmorType => armorType;
    public uint Hp => hp;
    public List<Weapon> Weapons => weapons;
    public List<Skill> Skills => skills;
}
