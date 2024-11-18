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
    public MainBuildingSoTable mainBuildingSoTable;
    public OtherBuildingSoTable otherBuildingSoTable;
    public ArmySoTable armySoTable;
    public ArmorSoTable armorSoTable;

    public FactionSo alliedForcesFactionSo;
    public FactionSo empireFactionSo;
    public FactionSo sovietUnionFactionSo;

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
        // 获得各阵营主建筑So文件夹路径
        string alliedForcesMainBuildingPath = GetFactionPath(FactionEnum.AlliedForces, SequenceType.MainBuildingSequence);
        string empireMainBuildingPath = GetFactionPath(FactionEnum.Empire, SequenceType.MainBuildingSequence);
        string sovietUnionMainBuildingPath = GetFactionPath(FactionEnum.SovietUnion, SequenceType.MainBuildingSequence);
        // 获得各阵营主建筑So文件名
        List<string> alliedForcesMainBuildingSoFiles = GetFileNameInPath(alliedForcesMainBuildingPath);
        List<string> empireMainBuildingSoFiles = GetFileNameInPath(empireMainBuildingPath);
        List<string> sovietUnionMainBuildingSoFiles = GetFileNameInPath(sovietUnionMainBuildingPath);
        // 得到建筑标签
        var buildingLabelSos = GetBuildingLabelSos();

        EditorUtility.SetDirty(mainBuildingSoTable);
        
        // tool初始化
        Tool.InitFactionSoConvertTo(alliedForcesFactionSo, empireFactionSo, sovietUnionFactionSo, armorSoTable, mainBuildingSoTable);
        using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(0);
            MainBuildingSo previousMainBuildingSo = null;
            for (int i = 3; i <= 32; i++)
            {
                var currentRow = sheet.GetRow(i);
                // ID 的string
                string idstring = currentRow.GetCellString(2);
                // 如果ID为null，表示这一行没有信息，只是上一行的另外一个技能
                if (idstring == "")
                {
                    currentRow.ReadAndWriteRowToISkill(21, previousMainBuildingSo);
                    continue;
                }
                int id = int.Parse(idstring);
                string nameEN = currentRow.GetCellString(4);
                string factionName = currentRow.GetCellString(1);

                // 判断So文件是否已经存在, 并得到MainBuildingSo
                string fileName = $"{id}_{nameEN}.asset";
                MainBuildingSo mainBuildingSo;
                Action loadCallBack;
                (string folderPath, List<string> exitsFileNames) = factionName switch
                {
                    "盟军" => (alliedForcesMainBuildingPath, alliedForcesMainBuildingSoFiles),
                    "帝国" => (empireMainBuildingPath, empireMainBuildingSoFiles),
                    _ => (sovietUnionMainBuildingPath, sovietUnionMainBuildingSoFiles)
                };
                string filePath = $"Assets{folderPath}/{fileName}";
                if (exitsFileNames.Contains(fileName))  // get
                {
                    mainBuildingSo = AssetDatabase.LoadAssetAtPath<MainBuildingSo>(filePath);
                    loadCallBack = () => Debug.Log(fileName + "---同步完成");
                }
                else    //create
                {
                    mainBuildingSo = ScriptableObject.CreateInstance<MainBuildingSo>();
                    AssetDatabase.CreateAsset(mainBuildingSo, filePath);
                    loadCallBack = () => Debug.Log(fileName + "---创建并同步完成");
                }
                // 配置主建筑的IbaseInfo和Ibuilding
                currentRow.ReadAndWriteRowToIBaseInfoAsync(mainBuildingSo);
                currentRow.ReadAndWriteRowToIBuilding(16, mainBuildingSo, buildingLabelSos);
                // 盟军的科技，这是一个特殊的存在，不可能存在技能，直接跳过
                if (currentRow.GetCellString(7).ConvertToTroop() != TroopType.Technology)
                    currentRow.ReadAndWriteRowToISkill(21, mainBuildingSo);
                EditorUtility.SetDirty(mainBuildingSo);

                loadCallBack.Invoke();
                previousMainBuildingSo = mainBuildingSo;
                mainBuildingSoTable.mainBuildingSoTableElement[id] = mainBuildingSo;
                
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
                var mainBuildingSo = name.ConvertToMainBuildingWithName();
                mainBuildingSo.SetRequirement(requirements);
                EditorUtility.SetDirty(mainBuildingSo);
            }
            AssetDatabase.SaveAssets();
            Debug.Log("requriement完成");


        }
    }
    #endregion

    #region OB
    [ContextMenu("OtherBuildingSo其他建筑数据导入")]
    public void LoadOtherBuildingSo()
    {
        // 获得各阵营其他建筑So文件夹路径
        string alliedForcesOtherBuildingPath = GetFactionPath(FactionEnum.AlliedForces, SequenceType.OtherBuildingSequence);
        string empireOtherBuildingPath = GetFactionPath(FactionEnum.Empire, SequenceType.OtherBuildingSequence);
        string sovietUnionOtherBuildingPath = GetFactionPath(FactionEnum.SovietUnion, SequenceType.OtherBuildingSequence);
        // 获得各阵营其他建筑So文件名
        List<string> alliedForcesOtherBuildingSoFiles = GetFileNameInPath(alliedForcesOtherBuildingPath);
        List<string> empireOtherBuildingSoFiles = GetFileNameInPath(empireOtherBuildingPath);
        List<string> sovietUnionOtherBuildingSoFiles = GetFileNameInPath(sovietUnionOtherBuildingPath);
        // 得到建筑标签
        var buildingLabelSos = GetBuildingLabelSos();
        var damageTypeSos = GetDamageTypeSos();
        EditorUtility.SetDirty(otherBuildingSoTable);
        Tool.InitFactionSoConvertTo(alliedForcesFactionSo, empireFactionSo, sovietUnionFactionSo, armorSoTable, mainBuildingSoTable);
        using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(1);
            OtherBuildingSo previousOtherBuildingSo = null;
            for (int i = 3; i <= 28; i++)
            {
                var currentRow = sheet.GetRow(i);

                string idstring = currentRow.GetCellString(2);
                // 如果ID为null，表示这一行没有信息，只是上一行的另外一个技能
                if (idstring == "")
                {
                    currentRow.ReadAndWriteRowToIWeapon(22, previousOtherBuildingSo, damageTypeSos);
                    currentRow.ReadAndWriteRowToISkill(33, previousOtherBuildingSo);
                    continue;
                }
                int id = int.Parse(idstring);
                string nameEN = currentRow.GetCellString(4);
                string factionName = currentRow.GetCellString(1);

                // 判断So文件是否已经存在, 并得到OtherBuildingSo
                string fileName = $"{id}_{nameEN}.asset";
                OtherBuildingSo otherBuildingSo;
                Action loadCallBack;
                (string folderPath, List<string> exitsFileNames) = factionName switch
                {
                    "盟军" => (alliedForcesOtherBuildingPath, alliedForcesOtherBuildingSoFiles),
                    "帝国" => (empireOtherBuildingPath, empireOtherBuildingSoFiles),
                    _ => (sovietUnionOtherBuildingPath, sovietUnionOtherBuildingSoFiles)
                };
                string filePath = $"Assets{folderPath}/{fileName}";
                if (exitsFileNames.Contains(fileName))  // get
                {
                    otherBuildingSo = AssetDatabase.LoadAssetAtPath<OtherBuildingSo>(filePath);
                    loadCallBack = () => Debug.Log(fileName + "---同步完成");
                }
                else    //create
                {
                    otherBuildingSo = ScriptableObject.CreateInstance<OtherBuildingSo>();
                    AssetDatabase.CreateAsset(otherBuildingSo, filePath);
                    loadCallBack = () => Debug.Log(fileName + "---创建并同步完成");
                }
                currentRow.ReadAndWriteRowToIBaseInfoAsync(otherBuildingSo);
                currentRow.ReadAndWriteRowToIBuilding(16, otherBuildingSo, buildingLabelSos);
                currentRow.ReadAndWriteRowToIWeapon(22, otherBuildingSo, damageTypeSos);
                currentRow.ReadAndWriteRowToISkill(33, otherBuildingSo);
                EditorUtility.SetDirty(otherBuildingSo);
                loadCallBack.Invoke();
                previousOtherBuildingSo = otherBuildingSo;
                otherBuildingSoTable.otherBuildingSoTableElement[id] = otherBuildingSo;
                
            }
            AssetDatabase.SaveAssets();
        }
    }
    #endregion

    #region Army
    [ContextMenu("ArmySo军队数据导入")]
    public void LoadArmySo()
    {
        // 获得各阵营其他建筑So文件夹路径
        string alliedForcesArmyPath = GetFactionPath(FactionEnum.AlliedForces, SequenceType.None);
        string empireArmyPath = GetFactionPath(FactionEnum.Empire, SequenceType.None);
        string sovietUnionArmyPath = GetFactionPath(FactionEnum.SovietUnion, SequenceType.None);
        // 获得各阵营其他建筑So文件名
        List<string> alliedForcesArmySoFiles = GetFileNameInPath(alliedForcesArmyPath);
        List<string> empireArmySoFiles = GetFileNameInPath(empireArmyPath);
        List<string> sovietUnionArmySoFiles = GetFileNameInPath(sovietUnionArmyPath);
        var armyLabelSos = GetArmyLabelSos();
        var damageTypeSos = GetDamageTypeSos();
        Tool.InitFactionSoConvertTo(alliedForcesFactionSo, empireFactionSo, sovietUnionFactionSo, armorSoTable, mainBuildingSoTable);
        using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(2);
            ArmySo previousArmySo = null;
            EditorUtility.SetDirty(armySoTable);
            for (int i = 3; i <= 123; i++)
            {
                var currentRow = sheet.GetRow(i);
                string idstring = currentRow.GetCellString(2);
                // 如果ID为null，表示这一行没有信息，只是上一行的另外一个技能或武器
                if (idstring == "")
                {
                    currentRow.ReadAndWriteRowToIWeapon(27, previousArmySo, damageTypeSos);
                    currentRow.ReadAndWriteRowToISkill(38, previousArmySo);
                    continue;
                }
                int id = int.Parse(idstring);
                string nameEN = currentRow.GetCellString(4);
                string factionName = currentRow.GetCellString(1);

                // 判断So文件是否已经存在, 并得到MainBuildingSo
                string fileName = $"{id}_{nameEN}.asset";
                ArmySo armySo;
                Action loadCallBack;
                (string folderPath, List<string> exitsFileNames) = factionName switch
                {
                    "盟军" => (alliedForcesArmyPath, alliedForcesArmySoFiles),
                    "帝国" => (empireArmyPath, empireArmySoFiles),
                    _ => (sovietUnionArmyPath, sovietUnionArmySoFiles)
                };
                string filePath = $"Assets{folderPath}/{fileName}";
                if (exitsFileNames.Contains(fileName))  // get
                {
                    armySo = AssetDatabase.LoadAssetAtPath<ArmySo>(filePath);
                    loadCallBack = () => Debug.Log(fileName + "---同步完成");
                }
                else    //create
                {
                    armySo = ScriptableObject.CreateInstance<ArmySo>();
                    AssetDatabase.CreateAsset(armySo, filePath);
                    loadCallBack = () => Debug.Log(fileName + "---创建并同步完成");
                }
                currentRow.ReadAndWriteRowToIBaseInfoAsync(armySo);
                currentRow.ReadAndWriteRowToIArmy(17, armySo, armyLabelSos);
                currentRow.ReadAndWriteRowToIWeapon(27, armySo, damageTypeSos);
                currentRow.ReadAndWriteRowToISkill(38, armySo);
                // 科技前提写入
                var requriement = currentRow.GetCellString(11);
                if (requriement != "null")
                {
                    var requirements = requriement.Split(',').Select(req => req.ConvertToMainBuildingWithName()).ToList();
                    armySo.SetRequirement(requirements);
                }
                EditorUtility.SetDirty(armySo);
                loadCallBack.Invoke();
                previousArmySo = armySo;
                armySoTable.armySoTableElement[id] = armySo;
                
            }
            FactionArmyDataLoad();
            AssetDatabase.SaveAssets();
        }
    }
    #endregion

    [ContextMenu("Faction校准")]
    public void FactionCalibrate()
    {
        alliedForcesFactionSo.MainBuildings = alliedForcesFactionSo.MainBuildings.Where(m => m != null).ToList();
        alliedForcesFactionSo.OtherBuildings = alliedForcesFactionSo.OtherBuildings.Where(m => m != null).ToList();
        alliedForcesFactionSo.Infantry = alliedForcesFactionSo.Infantry.Where(m => m != null).ToList();
        alliedForcesFactionSo.Vehicle = alliedForcesFactionSo.Vehicle.Where(m => m != null).ToList();
        alliedForcesFactionSo.Dock = alliedForcesFactionSo.Dock.Where(m => m != null).ToList();
        alliedForcesFactionSo.Aircraft = alliedForcesFactionSo.Aircraft.Where(m => m != null).ToList();

        empireFactionSo.MainBuildings = empireFactionSo.MainBuildings.Where(m => m != null).ToList();
        empireFactionSo.OtherBuildings = empireFactionSo.OtherBuildings.Where(m => m != null).ToList();
        empireFactionSo.Infantry = empireFactionSo.Infantry.Where(m => m != null).ToList();
        empireFactionSo.Vehicle = empireFactionSo.Vehicle.Where(m => m != null).ToList();
        empireFactionSo.Dock = empireFactionSo.Dock.Where(m => m != null).ToList();
        empireFactionSo.Aircraft = empireFactionSo.Aircraft.Where(m => m != null).ToList();

        sovietUnionFactionSo.MainBuildings = empireFactionSo.MainBuildings.Where(m => m != null).ToList();
        sovietUnionFactionSo.OtherBuildings = empireFactionSo.OtherBuildings.Where(m => m != null).ToList();
        sovietUnionFactionSo.Infantry = empireFactionSo.Infantry.Where(m => m != null).ToList();
        sovietUnionFactionSo.Vehicle = empireFactionSo.Vehicle.Where(m => m != null).ToList();
        sovietUnionFactionSo.Dock = empireFactionSo.Dock.Where(m => m != null).ToList();
        sovietUnionFactionSo.Aircraft = empireFactionSo.Aircraft.Where(m => m != null).ToList();
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
            var damageTypeSos = GetDamageTypeSos();
            for (int i = 1; i <= 35; i++)
            {
                var currentRow = sheet.GetRow(i);
                int index = int.Parse(currentRow.GetCellString(0));
                string armorNameZH = currentRow.GetCellString(1).Trim();
                string armorNameEN = currentRow.GetCellString(2).Trim();
                string fileName = $"{index}_{armorNameEN}.asset";
                string filePath = "Assets" + folderPath + "/" + fileName;
                Action loadCallBack;
                ArmorSo armorSo;
                if (existedFileName.Contains(fileName))
                {
                    armorSo = AssetDatabase.LoadAssetAtPath<ArmorSo>(filePath);
                    loadCallBack = () => Debug.Log(armorNameZH + "---同步完成");
                }
                else
                {
                    armorSo = ScriptableObject.CreateInstance<ArmorSo>();
                    AssetDatabase.CreateAsset(armorSo, filePath);
                    loadCallBack = () => Debug.Log(armorNameZH + "---创建并同步完成");
                }
                armorSo.SetIndex(index);
                armorSo.SetArmorNameZH(armorNameZH);
                armorSo.SetArmorNameEN(armorNameEN);
                for (int j = 1; j <= 15; j++)
                {
                    int value = int.Parse(currentRow.GetCellString(j + 3));
                    armorSo.SetDamageModifiers(damageTypeSos[j], value);
                }
                armorSoTable.armorSoTableElement[index] = armorSo;
                EditorUtility.SetDirty(armorSo);
                loadCallBack.Invoke();
            }
            EditorUtility.SetDirty(armorSoTable);
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
    /// <returns>返回一个字典，key为伤害编号，value为DamageTypeSo</returns>
    private Dictionary<int, DamageTypeSo> GetDamageTypeSos()
    {
        var fileNames = GetFileNameInPath(damageTypePath);
        Dictionary<int, DamageTypeSo> res = new Dictionary<int, DamageTypeSo>();
        foreach (var fileName in fileNames)
        {
            var damageTypeSo = AssetDatabase.LoadAssetAtPath<DamageTypeSo>("Assets" + damageTypePath + "/" + fileName);
            res[damageTypeSo.index] = damageTypeSo;
        }
        return res;
    }

    private Dictionary<string, BuildingLabelSo> GetBuildingLabelSos()
    {
        var fileNames = GetFileNameInPath(buildingLabelPath);
        Dictionary<string, BuildingLabelSo> res = new Dictionary<string, BuildingLabelSo>();
        foreach (var fileName in fileNames)
        {
            var buildingLabelSo = AssetDatabase.LoadAssetAtPath<BuildingLabelSo>("Assets" + buildingLabelPath + "/" + fileName);
            res[buildingLabelSo.LableNameZH] = buildingLabelSo;
        }
        return res;
    }

    private Dictionary<string, ArmyLabelSo> GetArmyLabelSos()
    {
        var fileNames = GetFileNameInPath(armyLabelPath);
        Dictionary<string, ArmyLabelSo> res = new Dictionary<string, ArmyLabelSo>();
        foreach (var fileName in fileNames)
        {
            var armyLabelSo = AssetDatabase.LoadAssetAtPath<ArmyLabelSo>("Assets" + armyLabelPath + "/" + fileName);
            res[armyLabelSo.LableNameZH] = armyLabelSo;
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
        foreach (var m in mainBuildingSoTable.mainBuildingSoTableElement.Values)
        {
            Addressables.LoadAssetAsync<Sprite>(m.NameChinese).Completed += (handle) =>
            {
                m.SetIcon(handle.Result);
            };
            EditorUtility.SetDirty(m);
            
        }
        foreach (var m in otherBuildingSoTable.otherBuildingSoTableElement.Values)
        {
            Addressables.LoadAssetAsync<Sprite>(m.NameChinese).Completed += (handle) =>
            {
                m.SetIcon(handle.Result);
            };
            EditorUtility.SetDirty(m);
        }
        foreach (var m in armySoTable.armySoTableElement.Values)
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

    public FactionSo GetFactionSo(string factionName)
    {
        return factionName switch { "盟军" => alliedForcesFactionSo, "帝国" => empireFactionSo, _ => sovietUnionFactionSo };
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
        EditorUtility.SetDirty(alliedForcesFactionSo);
        EditorUtility.SetDirty(empireFactionSo);
        EditorUtility.SetDirty(sovietUnionFactionSo);
        foreach (var army in armySoTable.armySoTableElement.Values)
        {
            if (army.BuildFacilities.Contains(ArmoredFactory) && !alliedForcesFactionSo.Vehicle.Contains(army))
                alliedForcesFactionSo.Vehicle.Add(army);
            if (army.BuildFacilities.Contains(Harbour) && !alliedForcesFactionSo.Dock.Contains(army))
                alliedForcesFactionSo.Dock.Add(army);
            if (army.BuildFacilities.Contains(AirForceBase) && !alliedForcesFactionSo.Aircraft.Contains(army))
                alliedForcesFactionSo.Aircraft.Add(army);

            if (army.BuildFacilities.Contains(MechaFactory) && !empireFactionSo.Vehicle.Contains(army))
                empireFactionSo.Vehicle.Add(army);
            if (army.BuildFacilities.Contains(EmpirePier) && !empireFactionSo.Dock.Contains(army))
                empireFactionSo.Dock.Add(army);

            if (army.BuildFacilities.Contains(WarFactory) && !sovietUnionFactionSo.Vehicle.Contains(army))
                sovietUnionFactionSo.Vehicle.Add(army);
            if (army.BuildFacilities.Contains(NavalShipyard) && !sovietUnionFactionSo.Dock.Contains(army))
                sovietUnionFactionSo.Dock.Add(army);
            if (army.BuildFacilities.Contains(Airport) && !sovietUnionFactionSo.Aircraft.Contains(army))
                sovietUnionFactionSo.Aircraft.Add(army);
        }
    }
}
#endregion
