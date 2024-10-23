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
    public string armyLabelPath;
    public string buildingLabelPath;
    public string damageTypePath;

    public string factionDBPath;

    public int row;
    public int col;

    public void Start()
    {
        LoadArmorData();
    }



    [ContextMenu("LoadArmorData")]
    public void LoadArmorData()
    {

        string folderPath = armorTypePath;
        var existedFileName = GetFileNameInPath(folderPath);
        FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read);
        var workbook =  new XSSFWorkbook(fileStream);
        ISheet sheet = workbook.GetSheetAt(3);

        var damageTypeSOs = GetDamageTypeSOs();

        for(int i = 1; i <= 35; i++)
        {

            var currentRow = sheet.GetRow(i);
            int index = int.Parse(currentRow.GetCell(0).ToString());
            string armorNameZH = currentRow.GetCell(1).ToString();
            string armorNameEN = currentRow.GetCell(2).ToString();
            string fileName = $"{index}_{armorNameEN}.asset";
            string filePath = "Assets" + folderPath + "/" + fileName;
            if(existedFileName.Contains(fileName))
            {
                ArmorSO armorSO = AssetDatabase.LoadAssetAtPath<ArmorSO>(filePath);
                armorSO.SetIndex(index);
                armorSO.SetArmorNameZH(armorNameZH);
                armorSO.SetArmorNameEN(armorNameEN);
                for(int j = 1; j <= 15; j++)
                {
                    int value = int.Parse(currentRow.GetCell(j + 3).ToString());
                    armorSO.SetDamageModifiers(damageTypeSOs[j], value);
                }
                Debug.Log(armorNameZH + "---同步完成");
            }
            else
            {
                ArmorSO armorSO = ScriptableObject.CreateInstance<ArmorSO>();
                AssetDatabase.CreateAsset(armorSO, filePath);
                armorSO.SetIndex(index);
                armorSO.SetArmorNameZH(armorNameZH);
                armorSO.SetArmorNameEN(armorNameEN);
                for(int j = 1; j <= 15; j++)
                {
                    int value = int.Parse(currentRow.GetCell(j + 3).ToString());
                    armorSO.SetDamageModifiers(damageTypeSOs[j], value);
                }
                Debug.Log(armorNameZH + "---创建并同步完成");
            }
            
        }
    }
    public void UpData()
    {

    }

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

    [ContextMenu("GetDamageTypeSOs")]
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

    [ContextMenu("DownloadExcel")]
    void LoadExcelAsync()
    {
        
    }



}

