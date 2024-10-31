using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.ArrayExtensions;
using JetBrains.Annotations;
using NPOI.HSSF.Record;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using Unity.Mathematics;

public class DataLoad : MonoBehaviour
{
    #region field
    public MainBuildingSOPage mainBuildingSOPage;
    public OtherBuildingSOPage otherBuildingSOPage;
    public ArmySOPage armySOPage;
    public ArmorSOPage armorSOPage;

    public FactionSO alliedForcesFactionSO;
    public FactionSO empireFactionSO;
    public FactionSO sovietUnionFactionSO;

    public string excelFilePath;
    public string armorTypePath;
    public string damageTypePath;
    public string armyLabelPath;
    public string buildingLabelPath;
    public string FactionDBPath;
    public int row;
    public int col;
    #endregion

    #region MB
    [ContextMenu("MainBuildingData主建筑数据导入")]
    public void LoadMainBuilding()
    {
        // 获得各阵营主建筑SO文件夹路径
        string alliedForcesMainBuildingPath = GetFactionPath(Faction.AlliedForces, SequenceType.MainBuildingSequence);
        string empireMainBuildingPath = GetFactionPath(Faction.Empire, SequenceType.MainBuildingSequence);
        string sovietUnionMainBuildingPath = GetFactionPath(Faction.SovietUnion, SequenceType.MainBuildingSequence);
        // 获得各阵营主建筑SO文件名
        List<string> alliedForcesMainBuildingSOFiles = GetFileNameInPath(alliedForcesMainBuildingPath);
        List<string> empireMainBuildingSOFiles = GetFileNameInPath(empireMainBuildingPath);
        List<string> sovietUnionMainBuildingSOFiles = GetFileNameInPath(sovietUnionMainBuildingPath);
        // 得到建筑标签
        var buildingLabelSOs = GetBuildingLabelSOs();
        // tool初始化
        Tool.InitFactionSOConvertTo(alliedForcesFactionSO, empireFactionSO, sovietUnionFactionSO, armorSOPage, mainBuildingSOPage);
        using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(0);
            MainBuildingSO previousMainBuildingSO = null;
            for (int i = 3; i <= 32; i++)
            {
                var currentRow = sheet.GetRow(i);
                // ID 的string
                string idstring = currentRow.GetCellString(2);
                // 如果ID为null，表示这一行没有信息，只是上一行的另外一个技能
                if (idstring == "")
                {
                    currentRow.ReadAndWriteRowToISkill(21, previousMainBuildingSO);
                    continue;
                }
                int id = int.Parse(idstring);
                string nameEN = currentRow.GetCellString(4);
                string factionName = currentRow.GetCellString(1);

                // 判断SO文件是否已经存在, 并得到MainBuildingSO
                string fileName = $"{id}_{nameEN}.asset";
                MainBuildingSO mainBuildingSO;
                Action loadCallBack;
                (string folderPath, List<string> exitsFileNames) = factionName switch
                {
                    "盟军" => (alliedForcesMainBuildingPath, alliedForcesMainBuildingSOFiles),
                    "帝国" => (empireMainBuildingPath, empireMainBuildingSOFiles),
                    _ => (sovietUnionMainBuildingPath, sovietUnionMainBuildingSOFiles)
                };
                string filePath = $"Assets{folderPath}/{fileName}";
                if (exitsFileNames.Contains(fileName))  // get
                {
                    mainBuildingSO = AssetDatabase.LoadAssetAtPath<MainBuildingSO>(filePath);
                    loadCallBack = () => Debug.Log(fileName + "---同步完成");
                }
                else    //create
                {
                    mainBuildingSO = ScriptableObject.CreateInstance<MainBuildingSO>();
                    AssetDatabase.CreateAsset(mainBuildingSO, filePath);
                    loadCallBack = () => Debug.Log(fileName + "---创建并同步完成");
                }
                // 配置主建筑的IbaseInfo和Ibuilding
                currentRow.ReadAndWriteRowToIBaseInfo(mainBuildingSO);
                currentRow.ReadAndWriteRowToIBuilding(16, mainBuildingSO, buildingLabelSOs);
                // 盟军的科技，这是一个特殊的存在，不可能存在技能，直接跳过
                if (currentRow.GetCellString(7).ConvertToTroop() != Troop.Technology)
                    currentRow.ReadAndWriteRowToISkill(21, mainBuildingSO);
                loadCallBack.Invoke();
                previousMainBuildingSO = mainBuildingSO;
                mainBuildingSOPage.mainBuildingSOPageElement[id] = mainBuildingSO;
            }

            // 科技前提写入
            for (int i = 3; i <= 32; i++)
            {
                var currentRow = sheet.GetRow(i);
                var requriement = currentRow.GetCellString(11);
                if (requriement == "null") continue;
                var requirements = requriement.Split(',').Select(req => req.ConvertToMainBuildingWithName()).ToList();
                var name = currentRow.GetCellString(3);
                if (name == "") continue;
                name.ConvertToMainBuildingWithName().SetRequirement(requirements);
            }
            Debug.Log("requriement完成");
        }
    }
    #endregion

    #region OB
    [ContextMenu("OtherBuildingSO")]
    public void LoadOtherBuildingSO()
    {
        // 获得各阵营其他建筑SO文件夹路径
        string alliedForcesOtherBuildingPath = GetFactionPath(Faction.AlliedForces, SequenceType.OtherBuildingSequence);
        string empireOtherBuildingPath = GetFactionPath(Faction.Empire, SequenceType.OtherBuildingSequence);
        string sovietUnionOtherBuildingPath = GetFactionPath(Faction.SovietUnion, SequenceType.OtherBuildingSequence);
        // 获得各阵营其他建筑SO文件名
        List<string> alliedForcesOtherBuildingSOFiles = GetFileNameInPath(alliedForcesOtherBuildingPath);
        List<string> empireOtherBuildingSOFiles = GetFileNameInPath(empireOtherBuildingPath);
        List<string> sovietUnionOtherBuildingSOFiles = GetFileNameInPath(sovietUnionOtherBuildingPath);
        // 得到建筑标签
        var buildingLabelSOs = GetBuildingLabelSOs();
        var damageTypeSOs = GetDamageTypeSOs();
        Tool.InitFactionSOConvertTo(alliedForcesFactionSO, empireFactionSO, sovietUnionFactionSO, armorSOPage, mainBuildingSOPage);
        using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(1);
            OtherBuildingSO previousOtherBuildingSO = null;
            for (int i = 3; i <= 28; i++)
            {
                var currentRow = sheet.GetRow(i);

                string idstring = currentRow.GetCellString(2);
                // 如果ID为null，表示这一行没有信息，只是上一行的另外一个技能
                if (idstring == "")
                {
                    currentRow.ReadAndWriteRowToIWeapon(22, previousOtherBuildingSO, damageTypeSOs);
                    currentRow.ReadAndWriteRowToISkill(33, previousOtherBuildingSO);
                    continue;
                }
                int id = int.Parse(idstring);
                string nameEN = currentRow.GetCellString(4);
                string factionName = currentRow.GetCellString(1);

                // 判断SO文件是否已经存在, 并得到MainBuildingSO
                string fileName = $"{id}_{nameEN}.asset";
                OtherBuildingSO otherBuildingSO;
                Action loadCallBack;
                (string folderPath, List<string> exitsFileNames) = factionName switch
                {
                    "盟军" => (alliedForcesOtherBuildingPath, alliedForcesOtherBuildingSOFiles),
                    "帝国" => (empireOtherBuildingPath, empireOtherBuildingSOFiles),
                    _ => (sovietUnionOtherBuildingPath, sovietUnionOtherBuildingSOFiles)
                };
                string filePath = $"Assets{folderPath}/{fileName}";
                if (exitsFileNames.Contains(fileName))  // get
                {
                    otherBuildingSO = AssetDatabase.LoadAssetAtPath<OtherBuildingSO>(filePath);
                    loadCallBack = () => Debug.Log(fileName + "---同步完成");
                }
                else    //create
                {
                    otherBuildingSO = ScriptableObject.CreateInstance<OtherBuildingSO>();
                    AssetDatabase.CreateAsset(otherBuildingSO, filePath);
                    loadCallBack = () => Debug.Log(fileName + "---创建并同步完成");
                }
                currentRow.ReadAndWriteRowToIBaseInfo(otherBuildingSO);
                currentRow.ReadAndWriteRowToIBuilding(16, otherBuildingSO, buildingLabelSOs);
                currentRow.ReadAndWriteRowToIWeapon(22, otherBuildingSO, damageTypeSOs);
                currentRow.ReadAndWriteRowToISkill(33, otherBuildingSO);
                // 科技前提写入
                var requriement = currentRow.GetCellString(11);
                if (requriement != "null")
                {
                    var requirements = requriement.Split(',').Select(req => req.ConvertToMainBuildingWithName()).ToList();
                    otherBuildingSO.SetRequirement(requirements);
                }
                loadCallBack.Invoke();
                previousOtherBuildingSO = otherBuildingSO;
                otherBuildingSOPage.otherBuildingSOPageElement[id] = otherBuildingSO;
            }
        }
    }
    #endregion
    #region Army
    [ContextMenu("ArmySO")]
    public void LoadArmySO()
    {
        var damageTypeSOs = GetDamageTypeSOs();
        Tool.InitFactionSOConvertTo(alliedForcesFactionSO, empireFactionSO, sovietUnionFactionSO, armorSOPage, mainBuildingSOPage);
        using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(2);
            OtherBuildingSO previousOtherBuildingSO = null;
        }

    }
    #endregion

    #region Armor
    [ContextMenu("LoadArmorData")]
    public void LoadArmorData()
    {
        string folderPath = armorTypePath;
        var existedFileName = GetFileNameInPath(folderPath);
        using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(3);
            var damageTypeSOs = GetDamageTypeSOs();
            for (int i = 1; i <= 35; i++)
            {
                var currentRow = sheet.GetRow(i);
                int index = int.Parse(currentRow.GetCellString(0));
                string armorNameZH = currentRow.GetCellString(1).Trim();
                string armorNameEN = currentRow.GetCellString(2).Trim();
                string fileName = $"{index}_{armorNameEN}.asset";
                string filePath = "Assets" + folderPath + "/" + fileName;
                Action loadCallBack;
                ArmorSO armorSO;
                if (existedFileName.Contains(fileName))
                {
                    armorSO = AssetDatabase.LoadAssetAtPath<ArmorSO>(filePath);
                    loadCallBack = () => Debug.Log(armorNameZH + "---同步完成");
                }
                else
                {
                    armorSO = ScriptableObject.CreateInstance<ArmorSO>();
                    AssetDatabase.CreateAsset(armorSO, filePath);
                    loadCallBack = () => Debug.Log(armorNameZH + "---创建并同步完成");
                }
                armorSO.SetIndex(index);
                armorSO.SetArmorNameZH(armorNameZH);
                armorSO.SetArmorNameEN(armorNameEN);
                for (int j = 1; j <= 15; j++)
                {
                    int value = int.Parse(currentRow.GetCellString(j + 3));
                    armorSO.SetDamageModifiers(damageTypeSOs[j], value);
                }
                armorSOPage.armorSOPageElement[index] = armorSO;
                loadCallBack.Invoke();
            }
        }
    }
    #endregion
    #region Method
    /// <summary>
    /// 得到一个文件路径内所有文件的文件名列表
    /// </summary>
    /// <param name="pathName">路径不包含Assets及其之前的路径</param>
    /// <returns>List<string></returns>
    private List<string> GetFileNameInPath(string pathName)
    {
        var files = Directory.GetFiles(Application.dataPath + pathName);
        List<string> res = new List<string>();
        foreach (var file in files)
        {
            if (file.EndsWith(".meta")) continue;
            res.Add(Path.GetFileName(file));
        }
        return res;
    }

    /// <summary>
    /// 得到游戏伤害类型
    /// </summary>
    /// <returns>返回一个字典，key为伤害编号，value为DamageTypeSO</returns>
    private Dictionary<int, DamageTypeSO> GetDamageTypeSOs()
    {
        var fileNames = GetFileNameInPath(damageTypePath);
        Dictionary<int, DamageTypeSO> res = new Dictionary<int, DamageTypeSO>();
        foreach (var fileName in fileNames)
        {
            var damageTypeSO = AssetDatabase.LoadAssetAtPath<DamageTypeSO>("Assets" + damageTypePath + "/" + fileName);
            res[damageTypeSO.index] = damageTypeSO;
        }
        return res;
    }

    private Dictionary<string, BuildingLabelSO> GetBuildingLabelSOs()
    {
        var fileNames = GetFileNameInPath(buildingLabelPath);
        Dictionary<string, BuildingLabelSO> res = new Dictionary<string, BuildingLabelSO>();
        foreach (var fileName in fileNames)
        {
            var buildingLabelSO = AssetDatabase.LoadAssetAtPath<BuildingLabelSO>("Assets" + buildingLabelPath + "/" + fileName);
            res[buildingLabelSO.LableNameZH] = buildingLabelSO;
        }
        return res;
    }

    private Dictionary<string, ArmyLabelSO> GetArmyLabelSOs()
    {
        var fileNames = GetFileNameInPath(armyLabelPath);
        Dictionary<string, ArmyLabelSO> res = new Dictionary<string, ArmyLabelSO>();
        foreach (var fileName in fileNames)
        {
            var armyLabelSO = AssetDatabase.LoadAssetAtPath<ArmyLabelSO>("Assets" + armyLabelPath + "/" + fileName);
            res[armyLabelSO.LableNameZH] = armyLabelSO;
        }
        return res;
    }
    private string GetFactionPath(Faction faction, SequenceType sequenceType)
    {
        return FactionDBPath +
            faction switch
            {
                Faction.AlliedForces => "/AlliedForces",
                Faction.Empire => "/Empire",
                _ => "/SovietUnion"
            } +
            sequenceType switch
            {
                SequenceType.MainBuildingSequence => "/MainBuilding",
                SequenceType.OtherBuildingSequence => "/OtherBuilding",
                SequenceType.InfantrySequence => "/Infantry",
                SequenceType.VehicleSequence => "/Vehicle",
                SequenceType.DockSequence => "/Dock",
                _ => "/Aircraft"
            };
    }

    [ContextMenu("DownloadExcel")]
    void LoadExcelAsync()
    {
        using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(1);
            var currentrow = sheet.GetRow(row);
            string aa = currentrow.GetCell(13).ToString();
            Debug.Log("ff");
        }
    }

    public FactionSO GetFactionSO(string factionName)
    {
        return factionName switch { "盟军" => alliedForcesFactionSO, "帝国" => empireFactionSO, _ => sovietUnionFactionSO };
    }
}
#endregion
#region Tool
public static class Tool
{
    public static FactionSO AlliedForcesFactionSO;
    public static FactionSO EmpireFactionSO;
    public static FactionSO SovietUnionFactionSO;
    public static ArmorSOPage ArmorSOPage;
    public static MainBuildingSOPage MainBuildingSOPage;
    public static void InitFactionSOConvertTo(FactionSO alliedForcesFactionSO, FactionSO empireFactionSO, FactionSO sovietUnionFactionSO, ArmorSOPage armorSOPage, MainBuildingSOPage mainBuildingSOPage)
    {
        AlliedForcesFactionSO = alliedForcesFactionSO;
        EmpireFactionSO = empireFactionSO;
        SovietUnionFactionSO = sovietUnionFactionSO;
        ArmorSOPage = armorSOPage;
        MainBuildingSOPage = mainBuildingSOPage;
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
    public static Troop ConvertToTroop(this string value)
    {
        if (value == "建筑")
            return Troop.Building;
        else if (value == "步兵")
            return Troop.Soldier;
        else if (value == "载具")
            return Troop.Vehicle;
        else if (value == "科技")
            return Troop.Technology;
        else
            return Troop.None;
    }
    public static Vector2Int ConvertToVector2Int(this string value)
    {
        if (value == "") return Vector2Int.zero;
        int[] res = value.Split(',').Select(i => int.Parse(i)).ToArray();
        return new Vector2Int(res[0], res[1]);
    }
    public static MainBuildingSO ConvertToMainBuildingWithName(this string name)
    {
        return MainBuildingSOPage.mainBuildingSOPageElement.Values.ToList().
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
        foreach (var armor in ArmorSOPage.armorSOPageElement)
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
        return value switch { "盟军" => AlliedForcesFactionSO, "帝国" => EmpireFactionSO, _ => EmpireFactionSO };
    }
    public static Vector2 ConvertToVector2(this string value, char option = ',')
    {
        if (value == "") return Vector2.zero;
        var res = value.Split(option).Select(i => float.Parse(i)).ToArray();
        return res.Length == 2 ? new Vector2(res[0], res[1]) : new Vector2(res[0], res[0]);
    }
    public static void ReadAndWriteRowToIBaseInfo(this IRow current, IBaseInfo baseInfo)
    {
        FactionSO factionSO = current.GetCellString(1).ConvertToFaction();
        int id = int.Parse(current.GetCellString(2));
        string nameChinese = current.GetCellString(3);
        string nameEnglish = current.GetCellString(4);
        string commentChinese = current.GetCellString(6);
        string commentEnglish = "null";
        Troop troop = current.GetCellString(7).ConvertToTroop();
        List<ActionScope> actionScopes = current.GetCellString(8).ConvertToActionScopes();
        int exp = int.Parse(current.GetCellString(9));
        int hp = int.Parse(current.GetCellString(10));
        int price = int.Parse(current.GetCellString(12));
        Vector2Int warningAndClearFogRad = current.GetCellString(13).ConvertToVector2Int();
        ArmorSO armorType = current.GetCellString(14).ConvertToArmorSO();
        baseInfo.SetBaseInfo(factionSO, id, nameChinese, nameEnglish, null, commentChinese, commentEnglish, troop, actionScopes, exp, hp, price, null, warningAndClearFogRad, armorType, null);
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
        float magazineSize = float.Parse(current.GetCellString(startIndex + 4));
        Vector2 magazineLoadingTime = current.GetCellString(startIndex + 5).ConvertToVector2();
        Vector2 aimingTime = current.GetCellString(startIndex + 6).ConvertToVector2();
        Vector2 firingDuration = current.GetCellString(startIndex + 7).ConvertToVector2();
        Vector2 sputteringRadius = current.GetCellString(startIndex + 8).ConvertToVector2();
        Vector2 sputteringDamage = current.GetCellString(startIndex + 9).ConvertToVector2();
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
            crushingAndCrushedLevel[CrushLabel.New] = current.GetCellString(startIndex + 7).ConvertToVector2('/');
        }
        List<ArmyLabelSO> labels = current.GetCellString(startIndex + 8).Split(',').Select(l => armyLabelSOs[l]).ToList();
        int buildingTime = int.Parse(current.GetCellString(startIndex + 9));
        List<MainBuildingSO> buildFacilities = current.GetCellString(startIndex + 10).Split(',').Select(m => m.ConvertToMainBuildingWithName()).ToList();
        army.SetArmy(moveSpeed, isReverseMove, isAmphibious, crushingAndCrushedLevel, labels, buildingTime, buildFacilities);
    }
}
#endregion