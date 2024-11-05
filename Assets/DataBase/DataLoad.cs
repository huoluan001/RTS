using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using UnityEngine.AddressableAssets;


public class DataLoad : MonoBehaviour
{
    #region field
    public MainBuildingSOTable mainBuildingSOTable;
    public OtherBuildingSOTable otherBuildingSOTable;
    public ArmySOTable armySOTable;
    public ArmorSOTable armorSOTable;

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
        string alliedForcesMainBuildingPath = GetFactionPath(FactionEnum.AlliedForces, SequenceType.MainBuildingSequence);
        string empireMainBuildingPath = GetFactionPath(FactionEnum.Empire, SequenceType.MainBuildingSequence);
        string sovietUnionMainBuildingPath = GetFactionPath(FactionEnum.SovietUnion, SequenceType.MainBuildingSequence);
        // 获得各阵营主建筑SO文件名
        List<string> alliedForcesMainBuildingSOFiles = GetFileNameInPath(alliedForcesMainBuildingPath);
        List<string> empireMainBuildingSOFiles = GetFileNameInPath(empireMainBuildingPath);
        List<string> sovietUnionMainBuildingSOFiles = GetFileNameInPath(sovietUnionMainBuildingPath);
        // 得到建筑标签
        var buildingLabelSOs = GetBuildingLabelSOs();

        EditorUtility.SetDirty(mainBuildingSOTable);
        
        // tool初始化
        Tool.InitFactionSOConvertTo(alliedForcesFactionSO, empireFactionSO, sovietUnionFactionSO, armorSOTable, mainBuildingSOTable);
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
                currentRow.ReadAndWriteRowToIBaseInfoAsync(mainBuildingSO);
                currentRow.ReadAndWriteRowToIBuilding(16, mainBuildingSO, buildingLabelSOs);
                // 盟军的科技，这是一个特殊的存在，不可能存在技能，直接跳过
                if (currentRow.GetCellString(7).ConvertToTroop() != TroopType.Technology)
                    currentRow.ReadAndWriteRowToISkill(21, mainBuildingSO);
                EditorUtility.SetDirty(mainBuildingSO);

                loadCallBack.Invoke();
                previousMainBuildingSO = mainBuildingSO;
                mainBuildingSOTable.mainBuildingSOTableElement[id] = mainBuildingSO;
                
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
                var mainBuildingSO = name.ConvertToMainBuildingWithName();
                mainBuildingSO.SetRequirement(requirements);
                EditorUtility.SetDirty(mainBuildingSO);
            }
            AssetDatabase.SaveAssets();
            Debug.Log("requriement完成");


        }
    }
    #endregion

    #region OB
    [ContextMenu("OtherBuildingSO其他建筑数据导入")]
    public void LoadOtherBuildingSO()
    {
        // 获得各阵营其他建筑SO文件夹路径
        string alliedForcesOtherBuildingPath = GetFactionPath(FactionEnum.AlliedForces, SequenceType.OtherBuildingSequence);
        string empireOtherBuildingPath = GetFactionPath(FactionEnum.Empire, SequenceType.OtherBuildingSequence);
        string sovietUnionOtherBuildingPath = GetFactionPath(FactionEnum.SovietUnion, SequenceType.OtherBuildingSequence);
        // 获得各阵营其他建筑SO文件名
        List<string> alliedForcesOtherBuildingSOFiles = GetFileNameInPath(alliedForcesOtherBuildingPath);
        List<string> empireOtherBuildingSOFiles = GetFileNameInPath(empireOtherBuildingPath);
        List<string> sovietUnionOtherBuildingSOFiles = GetFileNameInPath(sovietUnionOtherBuildingPath);
        // 得到建筑标签
        var buildingLabelSOs = GetBuildingLabelSOs();
        var damageTypeSOs = GetDamageTypeSOs();
        EditorUtility.SetDirty(otherBuildingSOTable);
        Tool.InitFactionSOConvertTo(alliedForcesFactionSO, empireFactionSO, sovietUnionFactionSO, armorSOTable, mainBuildingSOTable);
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

                // 判断SO文件是否已经存在, 并得到OtherBuildingSO
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
                currentRow.ReadAndWriteRowToIBaseInfoAsync(otherBuildingSO);
                currentRow.ReadAndWriteRowToIBuilding(16, otherBuildingSO, buildingLabelSOs);
                currentRow.ReadAndWriteRowToIWeapon(22, otherBuildingSO, damageTypeSOs);
                currentRow.ReadAndWriteRowToISkill(33, otherBuildingSO);
                EditorUtility.SetDirty(otherBuildingSO);
                loadCallBack.Invoke();
                previousOtherBuildingSO = otherBuildingSO;
                otherBuildingSOTable.otherBuildingSOTableElement[id] = otherBuildingSO;
                
            }
            AssetDatabase.SaveAssets();
        }
    }
    #endregion

    #region Army
    [ContextMenu("ArmySO军队数据导入")]
    public void LoadArmySO()
    {
        // 获得各阵营其他建筑SO文件夹路径
        string alliedForcesArmyPath = GetFactionPath(FactionEnum.AlliedForces, SequenceType.None);
        string empireArmyPath = GetFactionPath(FactionEnum.Empire, SequenceType.None);
        string sovietUnionArmyPath = GetFactionPath(FactionEnum.SovietUnion, SequenceType.None);
        // 获得各阵营其他建筑SO文件名
        List<string> alliedForcesArmySOFiles = GetFileNameInPath(alliedForcesArmyPath);
        List<string> empireArmySOFiles = GetFileNameInPath(empireArmyPath);
        List<string> sovietUnionArmySOFiles = GetFileNameInPath(sovietUnionArmyPath);
        var armyLabelSOs = GetArmyLabelSOs();
        var damageTypeSOs = GetDamageTypeSOs();
        Tool.InitFactionSOConvertTo(alliedForcesFactionSO, empireFactionSO, sovietUnionFactionSO, armorSOTable, mainBuildingSOTable);
        using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(2);
            ArmySO previousArmySO = null;
            EditorUtility.SetDirty(armySOTable);
            for (int i = 3; i <= 123; i++)
            {
                var currentRow = sheet.GetRow(i);
                string idstring = currentRow.GetCellString(2);
                // 如果ID为null，表示这一行没有信息，只是上一行的另外一个技能或武器
                if (idstring == "")
                {
                    currentRow.ReadAndWriteRowToIWeapon(27, previousArmySO, damageTypeSOs);
                    currentRow.ReadAndWriteRowToISkill(38, previousArmySO);
                    continue;
                }
                int id = int.Parse(idstring);
                string nameEN = currentRow.GetCellString(4);
                string factionName = currentRow.GetCellString(1);

                // 判断SO文件是否已经存在, 并得到MainBuildingSO
                string fileName = $"{id}_{nameEN}.asset";
                ArmySO armySO;
                Action loadCallBack;
                (string folderPath, List<string> exitsFileNames) = factionName switch
                {
                    "盟军" => (alliedForcesArmyPath, alliedForcesArmySOFiles),
                    "帝国" => (empireArmyPath, empireArmySOFiles),
                    _ => (sovietUnionArmyPath, sovietUnionArmySOFiles)
                };
                string filePath = $"Assets{folderPath}/{fileName}";
                if (exitsFileNames.Contains(fileName))  // get
                {
                    armySO = AssetDatabase.LoadAssetAtPath<ArmySO>(filePath);
                    loadCallBack = () => Debug.Log(fileName + "---同步完成");
                }
                else    //create
                {
                    armySO = ScriptableObject.CreateInstance<ArmySO>();
                    AssetDatabase.CreateAsset(armySO, filePath);
                    loadCallBack = () => Debug.Log(fileName + "---创建并同步完成");
                }
                currentRow.ReadAndWriteRowToIBaseInfoAsync(armySO);
                currentRow.ReadAndWriteRowToIArmy(17, armySO, armyLabelSOs);
                currentRow.ReadAndWriteRowToIWeapon(27, armySO, damageTypeSOs);
                currentRow.ReadAndWriteRowToISkill(38, armySO);
                // 科技前提写入
                var requriement = currentRow.GetCellString(11);
                if (requriement != "null")
                {
                    var requirements = requriement.Split(',').Select(req => req.ConvertToMainBuildingWithName()).ToList();
                    armySO.SetRequirement(requirements);
                }
                EditorUtility.SetDirty(armySO);
                loadCallBack.Invoke();
                previousArmySO = armySO;
                armySOTable.armySOTableElement[id] = armySO;
                
            }
            FactionArmyDataLoad();
            AssetDatabase.SaveAssets();
        }
    }
    #endregion

    [ContextMenu("Faction校准")]
    public void FactionCalibrate()
    {
        alliedForcesFactionSO.MainBuildings = alliedForcesFactionSO.MainBuildings.Where(m => m != null).ToList();
        alliedForcesFactionSO.OtherBuildings = alliedForcesFactionSO.OtherBuildings.Where(m => m != null).ToList();
        alliedForcesFactionSO.Infantry = alliedForcesFactionSO.Infantry.Where(m => m != null).ToList();
        alliedForcesFactionSO.Vehicle = alliedForcesFactionSO.Vehicle.Where(m => m != null).ToList();
        alliedForcesFactionSO.Dock = alliedForcesFactionSO.Dock.Where(m => m != null).ToList();
        alliedForcesFactionSO.Aircraft = alliedForcesFactionSO.Aircraft.Where(m => m != null).ToList();

        empireFactionSO.MainBuildings = empireFactionSO.MainBuildings.Where(m => m != null).ToList();
        empireFactionSO.OtherBuildings = empireFactionSO.OtherBuildings.Where(m => m != null).ToList();
        empireFactionSO.Infantry = empireFactionSO.Infantry.Where(m => m != null).ToList();
        empireFactionSO.Vehicle = empireFactionSO.Vehicle.Where(m => m != null).ToList();
        empireFactionSO.Dock = empireFactionSO.Dock.Where(m => m != null).ToList();
        empireFactionSO.Aircraft = empireFactionSO.Aircraft.Where(m => m != null).ToList();

        sovietUnionFactionSO.MainBuildings = empireFactionSO.MainBuildings.Where(m => m != null).ToList();
        sovietUnionFactionSO.OtherBuildings = empireFactionSO.OtherBuildings.Where(m => m != null).ToList();
        sovietUnionFactionSO.Infantry = empireFactionSO.Infantry.Where(m => m != null).ToList();
        sovietUnionFactionSO.Vehicle = empireFactionSO.Vehicle.Where(m => m != null).ToList();
        sovietUnionFactionSO.Dock = empireFactionSO.Dock.Where(m => m != null).ToList();
        sovietUnionFactionSO.Aircraft = empireFactionSO.Aircraft.Where(m => m != null).ToList();
    }

    #region Armor
    [ContextMenu("LoadArmorData护甲数据导入")]
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
                armorSOTable.armorSOTableElement[index] = armorSO;
                EditorUtility.SetDirty(armorSO);
                loadCallBack.Invoke();
            }
            EditorUtility.SetDirty(armorSOTable);
            AssetDatabase.SaveAssets();
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
    private string GetFactionPath(FactionEnum faction, SequenceType sequenceType)
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
                _ => "/Army",
            };
    }

    [ContextMenu("LoadUnitIcon")]
    public void LoadUnitIcon()
    {
        foreach (var m in mainBuildingSOTable.mainBuildingSOTableElement.Values)
        {
            Addressables.LoadAssetAsync<Sprite>(m.NameChinese).Completed += (handle) =>
            {
                m.SetIcon(handle.Result);
            };
            EditorUtility.SetDirty(m);
            
        }
        foreach (var m in otherBuildingSOTable.otherBuildingSOTableElement.Values)
        {
            Addressables.LoadAssetAsync<Sprite>(m.NameChinese).Completed += (handle) =>
            {
                m.SetIcon(handle.Result);
            };
            EditorUtility.SetDirty(m);
        }
        foreach (var m in armySOTable.armySOTableElement.Values)
        {
            Addressables.LoadAssetAsync<Sprite>(m.NameChinese).Completed += (handle) =>
            {
                m.SetIcon(handle.Result);
            };
            EditorUtility.SetDirty(m);
        }
        AssetDatabase.SaveAssets();
    }

    [ContextMenu("DownloadExcel")]
    void LoadExcelAsync()
    {


    }

    public FactionSO GetFactionSO(string factionName)
    {
        return factionName switch { "盟军" => alliedForcesFactionSO, "帝国" => empireFactionSO, _ => sovietUnionFactionSO };
    }

    public void FactionArmyDataLoad()
    {
        var ArmoredFactory = "装甲工厂".ConvertToMainBuildingWithName();
        var Harbour = "海港".ConvertToMainBuildingWithName();
        var AirForceBase = "空军基地".ConvertToMainBuildingWithName();

        var MechaFactory = "机甲工厂".ConvertToMainBuildingWithName();
        var EmpirePier = "帝国码头".ConvertToMainBuildingWithName();

        var WarFactory = "战争工厂".ConvertToMainBuildingWithName();
        var NavalShipyard = "海军造船厂".ConvertToMainBuildingWithName();
        var Airport = "机场".ConvertToMainBuildingWithName();
        EditorUtility.SetDirty(alliedForcesFactionSO);
        EditorUtility.SetDirty(empireFactionSO);
        EditorUtility.SetDirty(sovietUnionFactionSO);
        foreach (var army in armySOTable.armySOTableElement.Values)
        {
            if (army.BuildFacilities.Contains(ArmoredFactory) && !alliedForcesFactionSO.Vehicle.Contains(army))
                alliedForcesFactionSO.Vehicle.Add(army);
            if (army.BuildFacilities.Contains(Harbour) && !alliedForcesFactionSO.Dock.Contains(army))
                alliedForcesFactionSO.Dock.Add(army);
            if (army.BuildFacilities.Contains(AirForceBase) && !alliedForcesFactionSO.Aircraft.Contains(army))
                alliedForcesFactionSO.Aircraft.Add(army);

            if (army.BuildFacilities.Contains(MechaFactory) && !empireFactionSO.Vehicle.Contains(army))
                empireFactionSO.Vehicle.Add(army);
            if (army.BuildFacilities.Contains(EmpirePier) && !empireFactionSO.Dock.Contains(army))
                empireFactionSO.Dock.Add(army);

            if (army.BuildFacilities.Contains(WarFactory) && !sovietUnionFactionSO.Vehicle.Contains(army))
                sovietUnionFactionSO.Vehicle.Add(army);
            if (army.BuildFacilities.Contains(NavalShipyard) && !sovietUnionFactionSO.Dock.Contains(army))
                sovietUnionFactionSO.Dock.Add(army);
            if (army.BuildFacilities.Contains(Airport) && !sovietUnionFactionSO.Aircraft.Contains(army))
                sovietUnionFactionSO.Aircraft.Add(army);
        }
    }
}
#endregion
