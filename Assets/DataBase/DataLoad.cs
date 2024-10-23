using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEditor;
public class DataLoad : MonoBehaviour
{
    public MainBuildingSOPage mainBuildingSOPage;
    public OtherBuildingSOPage otherBuildingSOPage;
    public ArmySOPage armySOPage;
    
    public string armorTypePath = "Assets/ArmorType";
    public string armyLabelPath;
    public string buildingLabelPath;
    public string damageTypePath;

    public string factionDBPath;



    [ContextMenu("导入数据")]
    public void LoadData()
    {
        string folderPath = armorTypePath;
        string fileName = "1_MainBuildingArmor";
        string[] fileGuids = AssetDatabase.FindAssets("", new[] { folderPath });

        bool fileFound = false;

        foreach (string guid in fileGuids)
        {
            // 根据 GUID 获取文件路径
            string filePath = AssetDatabase.GUIDToAssetPath(guid);
            
            // 获取文件名（不包含路径的文件名）
            string currentFileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            
            // 检查文件名是否匹配
            if (currentFileName == fileName)
            {
                fileFound = true;
                Debug.Log($"File '{fileName}' found at: {filePath}");
                break;
            }
        }

        if (!fileFound)
        {
            Debug.Log($"File '{fileName}' not found in folder: {folderPath}");
        }
    }
    public void UpData()
    {

    }




    void DownloadExcel()
    {
        

        
        
    }

    
}
