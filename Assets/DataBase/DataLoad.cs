using UnityEngine;
using System.IO;
using UnityEditor;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Threading.Tasks;
public class DataLoad : MonoBehaviour
{
    public MainBuildingSOPage mainBuildingSOPage;
    public OtherBuildingSOPage otherBuildingSOPage;
    public ArmySOPage armySOPage;
    public string excelFilePath;
    public string armorTypePath;
    public string damageTypePath;
    public string armyLabelPath;
    public string buildingLabelPath;
    public string FactionDBPath;
    public partial class MainBuildingSO
    {
        [ContextMenu("MainBuildingData")]
        public void LoadMainDuilding()
        {
            // string alliedForcesMainBuildingPath = DataLoad.GetFactionPath(Faction.AlliedForces, SequenceType.MainBuildingSequence);
            // string empireMainBuildingPath = GetFactionPath(Faction.Empire, SequenceType.MainBuildingSequence);
            // string sovietUnionMainBuildingPath = GetFactionPath(Faction.SovietUnion, SequenceType.MainBuildingSequence);
            // List<string> alliedForcesMainBuildingSOFiles = GetFileNameInPath(alliedForcesMainBuildingPath);
            // List<string> empireMainBuildingSOFiles = GetFileNameInPath(empireMainBuildingPath);
            // List<string> sovietUnionMainBuildingSOFiles = GetFileNameInPath(sovietUnionMainBuildingPath);
            // using (FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
            {
                // var workbook = new XSSFWorkbook(fileStream);
                // ISheet sheet = workbook.GetSheetAt(0);

                // var currentRow = sheet.GetRow(3);
                // string factionName = currentRow.Cells[1].ToString();
                // uint id = uint.Parse(currentRow.Cells[2].ToString());
                // string nameZH = currentRow.Cells[3].ToString();
                // string nameEN = currentRow.Cells[4].ToString();
                // string comment = currentRow.Cells[5].ToString();

                // string fileName = $"{id}_{nameEN}";
                // string folderPath = null;
                // List<string> exitsFileNames;
                // if (factionName == "盟军")
                // {
                //     folderPath = alliedForcesMainBuildingPath;
                //     exitsFileNames = alliedForcesMainBuildingSOFiles;
                // }
                // else if (factionName == "帝国")
                // {
                //     folderPath = empireMainBuildingPath;
                //     exitsFileNames = empireMainBuildingSOFiles;
                // }
                // else
                // {
                //     folderPath = sovietUnionMainBuildingPath;
                //     exitsFileNames = sovietUnionMainBuildingSOFiles;
                // }
                // string filePath = $"Assets" + folderPath + "/" + fileName;
                // MainBuildingSO mainBuildingSO;
                // if (exitsFileNames.Contains(fileName))
                // {
                //     // get
                //     mainBuildingSO = AssetDatabase.LoadAssetAtPath<MainBuildingSO>(filePath);
                // }
                // else
                // {
                //     //create
                //     mainBuildingSO = ScriptableObject.CreateInstance<MainBuildingSO>();
                //     AssetDatabase.CreateAsset(mainBuildingSO, filePath);
                // }








            }


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
                string armorNameZH = currentRow.GetCell(1).ToString();
                string armorNameEN = currentRow.GetCell(2).ToString();
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

