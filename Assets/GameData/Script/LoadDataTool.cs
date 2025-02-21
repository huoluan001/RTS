using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using GameData.Script.Enum;
using NPOI.SS.UserModel;
using Unity.Mathematics;
using UnityEditor;

public static class LoadDataTool
{
    #region table

    private static ArmorSoTable _armorSoTable;

    public static ArmorSoTable ArmorSoTable
    {
        get
        {
            if (!_armorSoTable)
                _armorSoTable = GetAsset<ArmorSoTable>().FirstOrDefault();
            return _armorSoTable;
        }
    }

    private static ArmySoTable _armySoTable;

    public static ArmySoTable ArmySoTable
    {
        get
        {
            if (!_armySoTable)
                _armySoTable = GetAsset<ArmySoTable>().FirstOrDefault();
            return _armySoTable;
        }
    }

    private static MainBuildingSoTable _mainBuildingSoTable;

    public static MainBuildingSoTable MainBuildingSoTable
    {
        get
        {
            if (!_mainBuildingSoTable)

                _mainBuildingSoTable = GetAsset<MainBuildingSoTable>().FirstOrDefault();

            return _mainBuildingSoTable;
        }
    }

    private static OtherBuildingSoTable _otherBuildingSoTable;

    public static OtherBuildingSoTable OtherBuildingSoTable
    {
        get
        {
            if (!_otherBuildingSoTable)

                _otherBuildingSoTable = GetAsset<OtherBuildingSoTable>().FirstOrDefault();

            return _otherBuildingSoTable;
        }
    }


    private static FactionSo _alliedForcesFactionSo;
    private static FactionSo _empireFactionSo;
    private static FactionSo _sovietUnionFactionSo;

    public static FactionSo AlliedForcesFactionSo
    {
        get
        {
            if (!_alliedForcesFactionSo)
                FactionSosInit();
            return _alliedForcesFactionSo;
        }
    }

    public static FactionSo EmpireFactionSo
    {
        get
        {
            if (!_alliedForcesFactionSo)
                FactionSosInit();
            return _empireFactionSo;
        }
    }

    public static FactionSo SovietUnionFactionSo
    {
        get
        {
            if (!_alliedForcesFactionSo)
                FactionSosInit();
            return _sovietUnionFactionSo;
        }
    }

    #endregion
    public static readonly string MainBuildingsFolderPath = "/GameData/MainBuildings";
    public static readonly string OtherBuildingsFolderPath = "/GameData/OtherBuildings";
    public static readonly string ArmyFolderPath = "/GameData/Element";
    public static readonly string ArmorTypeFolderPath = "/GameData/ArmorType";
    public static readonly string ExcelFilePath = "Assets/GameData/RTS.xlsx";
    public static readonly string FactionDBPath = "/GameData/Factions";

    private static void FactionSosInit()
    {
        var factionSos = GetAsset<FactionSo>();
        factionSos.ForEach(factionSo =>
        {
            if (factionSo.factionEnum == FactionEnum.AlliedForces)
                _alliedForcesFactionSo = factionSo;
            else if (factionSo.factionEnum == FactionEnum.Empire)
                _empireFactionSo = factionSo;
            else if (factionSo.factionEnum == FactionEnum.SovietUnion)
                _sovietUnionFactionSo = factionSo;
        });
    }


    /// <summary>
    /// 得到一个文件路径内所有文件的文件名列表
    /// </summary>
    /// <param name="pathName">路径不包含Assets及其之前的路径</param>
    /// <returns>List<string></returns>
    public static List<string> GetAllFileNamesInPath(string path)
    {
        var files = Directory.GetFiles(Application.dataPath + path);
        List<string> res = new List<string>();
        foreach (var file in files)
        {
            if (file.EndsWith(".meta")) continue;
            res.Add(Path.GetFileName(file));
        }

        return res;
    }

    public static string GetFactionUnitPath(FactionEnum faction, SequenceType sequenceType)
    {
        return FactionDBPath +
               faction switch
               {
                   FactionEnum.AlliedForces => "/AlliedForces",
                   FactionEnum.Empire => "/Empire",
                   _ => "/SovietUnion"
               } +
               sequenceType switch
               {
                   SequenceType.MainBuildingSequence => "/MainBuilding",
                   SequenceType.OtherBuildingSequence => "/OtherBuilding",
                   _ => "/Element",
               };
    }

    public static List<T> GetAsset<T>() where T : Object
    {
        var type = typeof(T).Name;
        var guids = AssetDatabase.FindAssets($"t:{type}");
        var assets = new List<T>();
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            assets.Add(AssetDatabase.LoadAssetAtPath<T>(path));
        }

        return assets;
    }

    public static List<ActionScope> ConvertToActionScopes(this string value)
    {
        if (value == "") return null;
        return value.Split(',').ToList().Select(s => s.ToLower() switch
        {
            "land" => ActionScope.Land,
            "ocean" => ActionScope.Ocean,
            "air" => ActionScope.Air,
            "submarine" => ActionScope.Submarine,
            "aviation" => ActionScope.Aviation,
            _ => ActionScope.None
        }).ToList();
    }


    private static Vector2Int ConvertToVector2Int(this string value)
    {
        if (value == "") return Vector2Int.zero;
        var res = value.Split(',').Select(int.Parse).ToArray();
        return new Vector2Int(res[0], res[1]);
    }

    public static MainBuildingSo ConvertToMainBuildingWithName(this string name)
    {
        return MainBuildingSoTable.mainBuildingSoTableElement.Values.ToList()
            .FirstOrDefault(mainBuilding => mainBuilding.NameChinese == name);
    }

    private static Vector3 ConvertToVector3(this string value)
    {
        if (value == "") return Vector3.zero;
        var res = value.Split(',').Select(float.Parse).ToArray();
        return new Vector3(res[0], res[1], res[2]);
    }

    private static ArmorSo ConvertToArmorSo(this string value)
    {
        if (value == "") return null;
        foreach (var armor in ArmorSoTable.armorSoList)
        {
            if (armor.ArmorNameZh == value)
            {
                return armor;
            }
        }

        return null;
    }

    public static string GetCellString(this IRow value, int index)
    {
        return value == null ? "" : value.Cells[index].ToString().Trim();
    }

    private static FactionSo ConvertToFaction(this string value)
    {
        return value switch { "盟军" => AlliedForcesFactionSo, "帝国" => EmpireFactionSo, _ => SovietUnionFactionSo };
    }

    private static Vector2 ConvertToVector2(this string value, char option = ',')
    {
        if (value == "") return Vector2.zero;
        var res = value.Split(option).Select(float.Parse).ToArray();
        return res.Length == 2 ? new Vector2(res[0], res[1]) : new Vector2(res[0], res[0]);
    }

    public static void ReadAndWriteRowToIBaseInfo(this IRow current, IBaseInfo baseInfo)
    {
        FactionSo factionSo = current.GetCellString(1).ConvertToFaction();
        int id = int.Parse(current.GetCellString(2));
        string nameChinese = current.GetCellString(3);
        string nameEnglish = current.GetCellString(4);
        string commentChinese = current.GetCellString(6);
        string commentEnglish = "null";
        TroopType troopType = current.GetCellString(7).ConvertToTroop();
        if (baseInfo is ArmySo && troopType is TroopType.Soldier && !factionSo.infantry.Contains(baseInfo))
            factionSo.infantry.Add(baseInfo as ArmySo);
        List<ActionScope> actionScopes = current.GetCellString(8).ConvertToActionScopes();
        int exp = int.Parse(current.GetCellString(9));
        int hp = int.Parse(current.GetCellString(10));
        List<MainBuildingSo> requirements = baseInfo is MainBuildingSo || current.GetCellString(11) == "null"
            ? null
            : current.GetCellString(11).Split(',').Select(req => req.ConvertToMainBuildingWithName()).ToList();
        int price = int.Parse(current.GetCellString(12));
        Vector2Int warningAndClearFogRad = current.GetCellString(13).ConvertToVector2Int();
        ArmorSo armorType = baseInfo is ArmySo
            ? current.GetCellString(15).ConvertToArmorSo()
            : current.GetCellString(14).ConvertToArmorSo();
        baseInfo.SetBaseInfo(factionSo, id, nameChinese, nameEnglish, null, commentChinese, commentEnglish, troopType,
            actionScopes, exp, hp, price, requirements, warningAndClearFogRad, armorType, null);
    }

    public static void ReadAndWriteRowToIBuilding(this IRow current, int startIndex, IBuilding building)
    {
        BuildingLabelEnum buildingLabelEnum = current.GetCellString(startIndex).ToBuildingLabelEnum();
        Vector2Int area = current.GetCellString(startIndex + 1).ConvertToVector2Int();
        Vector2Int expandScope = current.GetCellString(startIndex + 2).ConvertToVector2Int();
        Vector2 buildingAndPlacementTime = current.GetCellString(startIndex + 3).ConvertToVector2();
        int powerConsume = int.Parse(current.GetCellString(startIndex + 4));
        building.SetBuilding(buildingLabelEnum, area, expandScope, buildingAndPlacementTime, powerConsume);
    }

    public static void ReadAndWriteRowToISkill(this IRow current, int startIndex, ISkill skill)
    {
        var skillNameZh = current.GetCellString(startIndex);
        if (skillNameZh == "") return;
        var skillNameEn = current.GetCellString(startIndex + 1);
        var skillComment = current.GetCellString(startIndex + 3);
        var cd = float.Parse(current.GetCellString(startIndex + 4));
        var preTime = float.Parse(current.GetCellString(startIndex + 5));
        var postTime = float.Parse(current.GetCellString(startIndex + 6));
        skill.SetSkill(skillNameZh, skillNameEn, skillComment, cd, preTime, postTime);
    }


    public static void ReadAndWriteRowToIWeapon(this IRow current, int startIndex, IWeapon weapon)
    {
        var weaponNameZh = current.GetCellString(startIndex);
        if (weaponNameZh == "") return;
        var damageTypeEnum = current.GetCellString(startIndex + 1).ToDamageTypeEnum();
        var singleDamage = current.GetCellString(startIndex + 2).ConvertToVector2();
        var range = current.GetCellString(startIndex + 3).ConvertToVector2();
        var magazineSize = int.Parse(current.GetCellString(startIndex + 4));
        var magazineLoadingTime = current.GetCellString(startIndex + 5).ConvertToVector2();
        var aimingTime = current.GetCellString(startIndex + 6).ConvertToVector2();
        var firingDuration = current.GetCellString(startIndex + 7).ConvertToVector2();
        var sputteringRadius = current.GetCellString(startIndex + 8).ConvertToVector2();
        var sputteringDamage = current.GetCellString(startIndex + 9).ConvertToVector2();
        // 武器的英文名称暂时为空
        weapon.SetWeapon(weaponNameZh, null, damageTypeEnum, singleDamage, range, magazineSize,
            magazineLoadingTime,
            aimingTime, firingDuration, sputteringRadius, sputteringDamage);
    }

    public static void ReadAndWriteRowToIArmy(this IRow current, int startIndex, IArmy army)
    {
        Vector3 moveSpeed = current.GetCellString(startIndex).ConvertToVector3();
        bool3 isReverseMove = new bool3(current.GetCellString(startIndex + 1) == "F" ? false : true,
            current.GetCellString(startIndex + 2) == "F" ? false : true,
            current.GetCellString(startIndex + 3) == "F" ? false : true);
        bool isAmphibious = current.GetCellString(startIndex + 4) == "F" ? false : true;
        CrushList crushingAndCrushedLevel = new CrushList();
        crushingAndCrushedLevel[CrushLabel.Default] = current.GetCellString(startIndex + 5).ConvertToVector2('/');
        Vector2 newvalue = current.GetCellString(startIndex + 6).ConvertToVector2('/');
        if (newvalue != Vector2.zero)
        {
            crushingAndCrushedLevel[CrushLabel.New] = newvalue;
        }

        List<ArmyLabelEnum> labels = current.GetCellString(startIndex + 7).Split(',').Select(l => l.ToArmyLabelEnum())
            .ToList();
        int buildingTime = int.Parse(current.GetCellString(startIndex + 8));
        List<MainBuildingSo> buildFacilities = current.GetCellString(startIndex + 9).Split(',')
            .Select(m => m.ConvertToMainBuildingWithName()).ToList();
        army.SetArmy(moveSpeed, isReverseMove, isAmphibious, crushingAndCrushedLevel, labels, buildingTime,
            buildFacilities);
    }


    #region DataConvert

    public static TroopType ConvertToTroop(this string value)
    {
        return value switch
        {
            "建筑" => TroopType.Building,
            "步兵" => TroopType.Soldier,
            "载具" => TroopType.Vehicle,
            "科技" => TroopType.Technology,
            _ => TroopType.None
        };
    }

    private static ArmyLabelEnum ToArmyLabelEnum(this string value)
    {
        return value switch
        {
            "反步兵" => ArmyLabelEnum.AntiInfantry,
            "反驻军" => ArmyLabelEnum.AntiGarrison,
            "反装甲" => ArmyLabelEnum.AntiArmor,
            "先进反装甲" => ArmyLabelEnum.AdvancedAntiArmor,
            "防空" => ArmyLabelEnum.AirDefense,
            "侦查" => ArmyLabelEnum.Reconnaissance,
            "渗透" => ArmyLabelEnum.Penetration,
            "冷冻" => ArmyLabelEnum.Frozen,
            "战场支援" => ArmyLabelEnum.BattlefieldSupport,
            "步兵运输" => ArmyLabelEnum.InfantryTransport,
            "步兵支援" => ArmyLabelEnum.InfantrySupport,
            "火炮支援" => ArmyLabelEnum.ArtillerySupport,
            "资源采集" => ArmyLabelEnum.ResourceCollection,
            "突击队员" => ArmyLabelEnum.Commandos,
            "轰炸机" => ArmyLabelEnum.Bombers,
            "制空战机" => ArmyLabelEnum.AirSupremacyFighter,
            "移动基地车" => ArmyLabelEnum.MobileCenterVehicle,
            _ => ArmyLabelEnum.None
        };
    }

    private static BuildingLabelEnum ToBuildingLabelEnum(this string value)
    {
        return value switch
        {
            "建造盟军建筑" => BuildingLabelEnum.BuildingAlliedBuilding,
            "建造苏联建筑" => BuildingLabelEnum.BuildingSovietBuilding,
            "建造纳米核心" => BuildingLabelEnum.ElectricPowerConstruction,
            "先进电力建筑" => BuildingLabelEnum.AdvancedPowerBuilding,
            "收集资源" => BuildingLabelEnum.CollectResource,
            "训练步兵" => BuildingLabelEnum.TrainingInfantry,
            "生产陆军载具" => BuildingLabelEnum.ProductionLandVehicle,
            "生产海军" => BuildingLabelEnum.ProductionNavy,
            "生产飞行器" => BuildingLabelEnum.ProductionAircraft,
            "生产机甲" => BuildingLabelEnum.ProductionMecha,
            "科技建筑" => BuildingLabelEnum.TechnologyBuilding,
            "阵地防御" => BuildingLabelEnum.PositionalDefense,
            "多功能基地防御" => BuildingLabelEnum.MultifunctionalBaseDefense,
            "反步兵基地防御" => BuildingLabelEnum.AntiInfantryBaseDefense,
            "防空基地防御" => BuildingLabelEnum.AirBaseDefense,
            "先进基地防御" => BuildingLabelEnum.AdvancedBaseDefense,
            "扩张建筑" => BuildingLabelEnum.ExpandBuilding,
            "入驻建筑" => BuildingLabelEnum.ResidentialBuilding,
            "超级武器" => BuildingLabelEnum.SuperWeapon,
            "终极武器" => BuildingLabelEnum.UltimateWeapon,
            _ => BuildingLabelEnum.None
        };
    }

    private static DamageTypeEnum ToDamageTypeEnum(this string value)
    {
        return value switch
        {
            "肉搏" => DamageTypeEnum.Melee,
            "狙击" => DamageTypeEnum.Sniper,
            "枪弹" => DamageTypeEnum.Bomb,
            "机炮" => DamageTypeEnum.MachineGun,
            "破片" => DamageTypeEnum.Fragment,
            "火箭" => DamageTypeEnum.Rocket,
            "穿甲" => DamageTypeEnum.ArmorPiercing,
            "光谱" => DamageTypeEnum.Spectrum,
            "电击" => DamageTypeEnum.ElectricShock,
            "高爆" => DamageTypeEnum.HighExplosive,
            "榴弹" => DamageTypeEnum.Grenade,
            "鱼雷" => DamageTypeEnum.Torpedo,
            "冲击" => DamageTypeEnum.Shock,
            "魔法" => DamageTypeEnum.Magic,
            "辐射" => DamageTypeEnum.Radiation,
            _ => DamageTypeEnum.None
        };
    }

    #endregion
}