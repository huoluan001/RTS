using UnityEngine;
using System.IO;
using UnityEditor;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;
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



    [ContextMenu("LoadArmorData")]
    public void LoadArmorData()
    {
        string folderPath = armorTypePath;
        var s = GetFileNameInPath(folderPath);
        ArmorSO armorSO = AssetDatabase.LoadAssetAtPath<ArmorSO>("Assets" + folderPath + "/" + s[0]);
        if (armorSO == null) Debug.Log("null");
        armorSO.SetArmorNameZH("nihao");

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
    private Dictionary<uint, DamageTypeSO> GetDamageTypeSOs()
    {
        var fileNames = GetFileNameInPath(damageTypePath);
        Dictionary<uint, DamageTypeSO> res = new Dictionary<uint, DamageTypeSO>();
        foreach (var fileName in fileNames)
        {
            var damageTypeSO = AssetDatabase.LoadAssetAtPath<DamageTypeSO>("Assets" + damageTypePath + "/" + fileName);
            res[damageTypeSO.index] = damageTypeSO;
        }
        return res;
    }

    [ContextMenu("DownloadExcel")]
    void DownloadExcel()
    {
        if (!File.Exists(excelFilePath))
        {
            Debug.LogError("Excel file not found: " + excelFilePath);
            return;
        }
        FileStream fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read);

        // 创建 Workbook 对象
        IWorkbook workbook = new XSSFWorkbook(fileStream); // 适用于 .xlsx 文件

        // 获取第一个工作表
        ISheet sheet = workbook.GetSheetAt(5);
        for (int row = 0; row <= sheet.LastRowNum; row++)
        {
            IRow currentRow = sheet.GetRow(row);

            for (int col = 0; col < currentRow.LastCellNum; col++)
            {
                ICell cell = currentRow.GetCell(col);

                string cellValue = cell.ToString();
                Debug.Log($"Row {row}, Column {col}: {cellValue}");
            }
        }

        // 关闭文件流
        fileStream.Close();



    }


}
