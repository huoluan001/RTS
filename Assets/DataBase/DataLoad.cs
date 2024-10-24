using UnityEngine;
using System.IO;
using UnityEditor;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System;

using System.Linq;

public class DataLoad : MonoBehaviour
{
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

    [ContextMenu("MainBuildingData")]
    public void LoadMainDuilding()
    {
        string alliedForcesMainBuildingPath = GetFactionPath(Faction.AlliedForces, SequenceType.MainBuildingSequence);
        string empireMainBuildingPath = GetFactionPath(Faction.Empire, SequenceType.MainBuildingSequence);
        string sovietUnionMainBuildingPath = GetFactionPath(Faction.SovietUnion, SequenceType.MainBuildingSequence);
        List<string> alliedForcesMainBuildingSOFiles = GetFileNameInPath(alliedForcesMainBuildingPath);
        List<string> empireMainBuildingSOFiles = GetFileNameInPath(empireMainBuildingPath);
        List<string> sovietUnionMainBuildingSOFiles = GetFileNameInPath(sovietUnionMainBuildingPath);
        var buildingLabelSOs = GetBuildingLabelSOs();
        using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
        {
            var workbook = new XSSFWorkbook(fileStream);
            ISheet sheet = workbook.GetSheetAt(0);

            var currentRow = sheet.GetRow(3);
            string factionName = currentRow.Cells[1].ToString();
            FactionSO factionSO;
            int id = int.Parse(currentRow.Cells[2].ToString());
            string nameZH = currentRow.Cells[3].ToString().Trim();
            string nameEN = currentRow.Cells[4].ToString().Trim();
            string comment = currentRow.Cells[6].ToString().Trim();
            Troop troop = currentRow.Cells[7].ToString().Trim().ConvertToTroop();
            List<ActionScope> actionScopes = currentRow.Cells[8].
                ToString().Trim().Split(',').ToList().Select(s => s.ConvertToActionScope()).ToList();
            int exp = int.Parse(currentRow.Cells[9].ToString().Trim());
            int hp = int.Parse(currentRow.Cells[10].ToString().Trim());
            int price = int.Parse(currentRow.Cells[12].ToString());
            Vector2Int warningAndClearFogRad = currentRow.Cells[13].ToString().ConvertToVector2Int();
            ArmorSO armorSO = currentRow.Cells[14].ToString().ConvertToArmorSO(armorSOPage);

            string fileName = $"{id}_{nameEN}.asset";
            string folderPath = null;
            List<string> exitsFileNames;
            if (factionName == "盟军")
            {
                folderPath = alliedForcesMainBuildingPath;
                exitsFileNames = alliedForcesMainBuildingSOFiles;
                factionSO = alliedForcesFactionSO;
            }
            else if (factionName == "帝国")
            {
                folderPath = empireMainBuildingPath;
                exitsFileNames = empireMainBuildingSOFiles;
                factionSO = empireFactionSO;
            }
            else
            {
                folderPath = sovietUnionMainBuildingPath;
                exitsFileNames = sovietUnionMainBuildingSOFiles;
                factionSO = sovietUnionFactionSO;
            }
            string filePath = $"Assets" + folderPath + "/" + fileName;
            MainBuildingSO mainBuildingSO;
            if (exitsFileNames.Contains(fileName))  // get
            {
                mainBuildingSO = AssetDatabase.LoadAssetAtPath<MainBuildingSO>(filePath);
            }
            else    //create
            {
                mainBuildingSO = ScriptableObject.CreateInstance<MainBuildingSO>();
                AssetDatabase.CreateAsset(mainBuildingSO, filePath);
            }
            mainBuildingSO.SetDataBaseInfoNoRequirementAndGameObjectPrefab(factionSO, id, nameZH, nameEN,
                comment, null, troop, actionScopes, exp, hp, price,warningAndClearFogRad, armorSO);
            BuildingLabelSO buildingLabelSO = buildingLabelSOs[currentRow.Cells[16].ToString().Trim()];
            Vector2Int area = currentRow.Cells[17].ToString().Trim().ConvertToVector2Int();
            Vector2Int expandScope = currentRow.Cells[18].ToString().Trim().ConvertToVector2Int();
            Vector2Int buildingAndPlacementTime = currentRow.Cells[19].ToString().Trim().ConvertToVector2Int();
            int powerConsume = int.Parse(currentRow.Cells[20].ToString().Trim());
            mainBuildingSO.SetDataBuildingInfo(buildingLabelSO, area, expandScope, 
                buildingAndPlacementTime, powerConsume);

        }

    }








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
                int index = int.Parse(currentRow.GetCell(0).ToString());
                string armorNameZH = currentRow.GetCell(1).ToString().Trim();
                string armorNameEN = currentRow.GetCell(2).ToString().Trim();
                string fileName = $"{index}_{armorNameEN}.asset";
                string filePath = "Assets" + folderPath + "/" + fileName;
                Action loadCallBack;
                ArmorSO armorSO;
                if (existedFileName.Contains(fileName))
                {
                    armorSO = AssetDatabase.LoadAssetAtPath<ArmorSO>(filePath);
                    loadCallBack = new Action(() => Debug.Log(armorNameZH + "---同步完成"));
                }
                else
                {
                    armorSO = ScriptableObject.CreateInstance<ArmorSO>();
                    AssetDatabase.CreateAsset(armorSO, filePath);
                    loadCallBack = new Action(() => Debug.Log(armorNameZH + "---创建并同步完成"));
                }
                
                armorSO.SetIndex(index);
                armorSO.SetArmorNameZH(armorNameZH);
                armorSO.SetArmorNameEN(armorNameEN);
                for (int j = 1; j <= 15; j++)
                {
                    int value = int.Parse(currentRow.GetCell(j + 3).ToString());
                    armorSO.SetDamageModifiers(damageTypeSOs[j], value);
                }
                armorSOPage.armorSOPageElement[index] = armorSO;
                loadCallBack.Invoke();
            }
        }
    }

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
        Debug.Log(GetFileNameInPath(GetFactionPath(Faction.AlliedForces, SequenceType.MainBuildingSequence))[0]);
        // using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
        // {
        //     var workbook = new XSSFWorkbook(fileStream);
        //     ISheet sheet = workbook.GetSheetAt(0);
        //     Debug.Log(sheet.GetRow(row).GetCell(col).ToString());
        // }
    }
}

public static class ConvertTo
{
    public static ActionScope ConvertToActionScope(this string value)
    {
        if (value.ToLower() == "land")
            return ActionScope.Land;
        else if (value.ToLower() == "ocean")
            return ActionScope.Ocean;
        else if (value.ToLower() == "air")
            return ActionScope.Air;
        else if (value.ToLower() == "submarine")
            return ActionScope.Submarine;
        else if (value.ToLower() == "aviation")
            return ActionScope.Aviation;
        else
            return ActionScope.None;
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
        int[] res = value.Split(',').Select(i => int.Parse(i)).ToArray();
        return new Vector2Int(res[0], res[1]);
    }
    public static ArmorSO ConvertToArmorSO(this string value, ArmorSOPage armorSOPage)
    {
        foreach(var armor in armorSOPage.armorSOPageElement)
        {
            if(armor.Value.ArmorNameZH == value)
            {
                return armor.Value;
            }
        }
        return null;
        
    }

}