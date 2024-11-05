using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NPOI.SS.UserModel;
using Unity.Mathematics;
public static class Tool
{
    public static FactionSO AlliedForcesFactionSO;
    public static FactionSO EmpireFactionSO;
    public static FactionSO SovietUnionFactionSO;
    public static ArmorSOTable ArmorSOTable;
    public static MainBuildingSOTable MainBuildingSOTable;
    public static void InitFactionSOConvertTo(FactionSO alliedForcesFactionSO, FactionSO empireFactionSO, FactionSO sovietUnionFactionSO, ArmorSOTable armorSOTable, MainBuildingSOTable mainBuildingSOTable)
    {
        AlliedForcesFactionSO = alliedForcesFactionSO;
        EmpireFactionSO = empireFactionSO;
        SovietUnionFactionSO = sovietUnionFactionSO;
        ArmorSOTable = armorSOTable;
        MainBuildingSOTable = mainBuildingSOTable;
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
    public static TroopType ConvertToTroop(this string value)
    {
        if (value == "建筑")
            return TroopType.Building;
        else if (value == "步兵")
            return TroopType.Soldier;
        else if (value == "载具")
            return TroopType.Vehicle;
        else if (value == "科技")
            return TroopType.Technology;
        else
            return TroopType.None;
    }
    public static Vector2Int ConvertToVector2Int(this string value)
    {
        if (value == "") return Vector2Int.zero;
        int[] res = value.Split(',').Select(i => int.Parse(i)).ToArray();
        return new Vector2Int(res[0], res[1]);
    }
    public static MainBuildingSO ConvertToMainBuildingWithName(this string name)
    {
        return MainBuildingSOTable.mainBuildingSOTableElement.Values.ToList().
            FirstOrDefault(mainBuilding => mainBuilding.NameChinese == name);
    }
    public static Vector3 ConvertToVector3(this string value)
    {
        if (value == "") return Vector3.zero;
        float[] res = value.Split(',').Select(i => float.Parse(i)).ToArray();
        return new Vector3(res[0], res[1], res[2]);
    }
    public static ArmorSO ConvertToArmorSO(this string value)
    {
        if (value == "") return null;
        foreach (var armor in ArmorSOTable.armorSOTableElement)
        {
            if (armor.Value.ArmorNameZH == value)
            {
                return armor.Value;
            }
        }
        return null;
    }
    public static string GetCellString(this IRow value, int index)
    {
        return value == null ? "" : value.Cells[index].ToString().Trim();

    }
    public static FactionSO ConvertToFaction(this string value)
    {
        return value switch { "盟军" => AlliedForcesFactionSO, "帝国" => EmpireFactionSO, _ => SovietUnionFactionSO };
    }
    public static Vector2 ConvertToVector2(this string value, char option = ',')
    {
        if (value == "") return Vector2.zero;
        var res = value.Split(option).Select(i => float.Parse(i)).ToArray();
        return res.Length == 2 ? new Vector2(res[0], res[1]) : new Vector2(res[0], res[0]);
    }
    public static void ReadAndWriteRowToIBaseInfoAsync(this IRow current, IBaseInfo baseInfo)
    {
        FactionSO factionSO = current.GetCellString(1).ConvertToFaction();
        if (baseInfo is MainBuildingSO && !factionSO.MainBuildings.Contains(baseInfo))
        {
            factionSO.MainBuildings.Add(baseInfo as MainBuildingSO);
        }
        else if (baseInfo is OtherBuildingSO && !factionSO.OtherBuildings.Contains(baseInfo))
        {
            factionSO.OtherBuildings.Add(baseInfo as OtherBuildingSO);
        }

        int id = int.Parse(current.GetCellString(2));
        string nameChinese = current.GetCellString(3);
        string nameEnglish = current.GetCellString(4);
        string commentChinese = current.GetCellString(6);
        string commentEnglish = "null";
        TroopType troopType = current.GetCellString(7).ConvertToTroop();
        if (baseInfo is ArmySO && troopType is TroopType.Soldier && !factionSO.Infantry.Contains(baseInfo))
            factionSO.Infantry.Add(baseInfo as ArmySO);
        List<ActionScope> actionScopes = current.GetCellString(8).ConvertToActionScopes();
        int exp = int.Parse(current.GetCellString(9));
        int hp = int.Parse(current.GetCellString(10));
        List<MainBuildingSO> requirements = baseInfo is MainBuildingSO || current.GetCellString(11) == "null" ? null : current.GetCellString(11).Split(',').Select(req => req.ConvertToMainBuildingWithName()).ToList();
        int price = int.Parse(current.GetCellString(12));
        Vector2Int warningAndClearFogRad = current.GetCellString(13).ConvertToVector2Int();
        ArmorSO armorType = baseInfo is ArmySO ? current.GetCellString(15).ConvertToArmorSO() : current.GetCellString(14).ConvertToArmorSO();
        baseInfo.SetBaseInfo(factionSO, id, nameChinese, nameEnglish, null, commentChinese, commentEnglish, troopType, actionScopes, exp, hp, price, requirements, warningAndClearFogRad, armorType, null);
    }
    public static void ReadAndWriteRowToIBuilding(this IRow current, int startIndex, IBuilding building, Dictionary<string, BuildingLabelSO> buildingLabelSOs)
    {
        BuildingLabelSO buildingLabelSO = current.GetCellString(startIndex) == "" ? null : buildingLabelSOs[current.GetCellString(startIndex)];
        Vector2Int area = current.GetCellString(startIndex + 1).ConvertToVector2Int();
        Vector2Int expandScope = current.GetCellString(startIndex + 2).ConvertToVector2Int();
        Vector2 buildingAndPlacementTime = current.GetCellString(startIndex + 3).ConvertToVector2();
        int powerConsume = int.Parse(current.GetCellString(startIndex + 4));
        building.SetBuilding(buildingLabelSO, area, expandScope, buildingAndPlacementTime, powerConsume);
    }
    public static void ReadAndWriteRowToISkill(this IRow current, int startIndex, ISkill skill)
    {
        string skillNameZH = current.GetCellString(startIndex);
        if (skillNameZH == "") return;
        string skillNameEN = current.GetCellString(startIndex + 1);
        string skillComment = current.GetCellString(startIndex + 3);
        float cd = float.Parse(current.GetCellString(startIndex + 4));
        float preTime = float.Parse(current.GetCellString(startIndex + 5));
        float postTime = float.Parse(current.GetCellString(startIndex + 6));
        skill.SetSkill(skillNameZH, skillNameEN, skillComment, cd, preTime, postTime);
    }
    public static void ReadAndWriteRowToIWeapon(this IRow current, int startIndex, IWeapon weapon, Dictionary<int, DamageTypeSO> damageTypeSOs)
    {
        string weaponNameZH = current.GetCellString(startIndex);
        if (weaponNameZH == "") return;
        string weaponNameEN = null;
        DamageTypeSO damageType = current.GetCellString(startIndex + 1) == "" ? null :
                        damageTypeSOs.Values.FirstOrDefault(damageType => damageType.damageTypeZH == current.GetCellString(startIndex + 1));
        Vector2 singleDamage = current.GetCellString(startIndex + 2).ConvertToVector2();
        Vector2 range = current.GetCellString(startIndex + 3).ConvertToVector2();
        int magazineSize = int.Parse(current.GetCellString(startIndex + 4));
        Vector2 magazineLoadingTime = magazineLoadingTime = current.GetCellString(startIndex + 5).ConvertToVector2();
        Vector2 aimingTime = current.GetCellString(startIndex + 6).ConvertToVector2();
        Vector2 firingDuration = current.GetCellString(startIndex + 7).ConvertToVector2();
        Vector2 sputteringRadius = current.GetCellString(startIndex + 8).ConvertToVector2();
        Vector2 sputteringDamage = sputteringDamage = current.GetCellString(startIndex + 9).ConvertToVector2();
        weapon.SetWeapon(weaponNameZH, weaponNameEN, damageType, singleDamage, range, magazineSize, magazineLoadingTime, aimingTime, firingDuration, sputteringRadius, sputteringDamage);
    }
    public static void ReadAndWriteRowToIArmy(this IRow current, int startIndex, IArmy army, Dictionary<string, ArmyLabelSO> armyLabelSOs)
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
        List<ArmyLabelSO> labels = current.GetCellString(startIndex + 7).Split(',').Select(l => armyLabelSOs[l]).ToList();
        int buildingTime = int.Parse(current.GetCellString(startIndex + 8));
        List<MainBuildingSO> buildFacilities = current.GetCellString(startIndex + 9).Split(',').Select(m => m.ConvertToMainBuildingWithName()).ToList();
        army.SetArmy(moveSpeed, isReverseMove, isAmphibious, crushingAndCrushedLevel, labels, buildingTime, buildFacilities);
    }
}