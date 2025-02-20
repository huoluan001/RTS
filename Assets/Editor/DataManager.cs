using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using System.IO;
using GameData.Script.Enum;
using NPOI.XSSF.UserModel;
using Object = UnityEngine.Object;

public class DataManager : EditorWindow
{
    private ListView _listView;
    private ScrollView _scrollView;
    private Action _clickCallback;

    private Button _factionButton;
    private Button _mBButton;
    private Button _oBButton;
    private Button _armyButton;
    private Button _armorButton;


    [MenuItem("UI Toolkit/DataManager")]
    public static void ShowExample()
    {
        DataManager wnd = GetWindow<DataManager>();
        wnd.titleContent = new GUIContent("LoadData");
    }

    private void OnEnable()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/DataManager.uxml");
        VisualElement labelFromUxml = visualTree.CloneTree();
        rootVisualElement.Add(labelFromUxml);

        _listView = rootVisualElement.Q<ListView>("SoList");
        _scrollView = rootVisualElement.Q<ScrollView>("SoInfo");

        var updateDropdownButton = rootVisualElement.Q<DropdownField>("UpdateDropDownField");
        updateDropdownButton.choices = new List<string>()
            { "Update", "MainBuilding", "OtherBuilding", "Army", "Armor", "All" };
        updateDropdownButton.RegisterValueChangedCallback(evt => UpGameData(evt.newValue));


        _listView.makeItem = () => new Label();
        _listView.fixedItemHeight = 16f;
        _listView.selectionType = SelectionType.Single;
        _listView.style.flexGrow = 1;

        _listView.selectionChanged += (enumerable) =>
        {
            _scrollView.Clear();
            ScriptableObject scriptableObject = (ScriptableObject)enumerable.First();
            SerializedObject serializedObject = new SerializedObject(scriptableObject);
            SerializedProperty serializedProperty = serializedObject.GetIterator();
            serializedProperty.Next(true);
            while (serializedProperty.NextVisible(true))
            {
                PropertyField propertyField = new PropertyField(serializedProperty);
                propertyField.Bind(serializedObject);
                _scrollView.Add(propertyField);
            }
        };
        _factionButton = rootVisualElement.Q<Button>("FactionBTN");
        _mBButton = rootVisualElement.Q<Button>("MainBuildingBTN");
        _oBButton = rootVisualElement.Q<Button>("OtherBuildingBTN");
        _armyButton = rootVisualElement.Q<Button>("ArmyBTN");
        _armorButton = rootVisualElement.Q<Button>("ArmorBTN");

        _factionButton.clickable.clicked += () => OnClickFactionSo(_factionButton);
        _mBButton.clickable.clicked += () => OnClick<MainBuildingSo>(_mBButton);
        _oBButton.clickable.clicked += () => OnClick<OtherBuildingSo>(_oBButton);
        _armyButton.clickable.clicked += () => OnClick<ArmySo>(_armyButton);
        _armorButton.clickable.clicked += () =>
        {
            _clickCallback?.Invoke();
            _armorButton.SetEnabled(false);
            var assets = LoadDataTool.GetAsset<ArmorSo>();
            _listView.Clear();
            _listView.bindItem = null;
            _listView.itemsSource = assets;
            _listView.bindItem = (element, index) => { ((Label)element).text = assets[index].ArmorNameEn; };
            _clickCallback = () => _armorButton.SetEnabled(true);
        };
    }

    private void OnClick<T>(Button button) where T : Object, IBaseInfo
    {
        _clickCallback?.Invoke();
        button.SetEnabled(false);
        List<T> assets = LoadDataTool.GetAsset<T>();
        _listView.Clear();
        _listView.bindItem = null;
        _listView.itemsSource = assets;
        _listView.bindItem = (element, index) => { ((Label)element).text = assets[index].NameEnglish; };
        _clickCallback = () => button.SetEnabled(true);
    }

    private void OnClickFactionSo(Button button)
    {
        _clickCallback?.Invoke();
        button.SetEnabled(false);
        List<FactionSo> assets = LoadDataTool.GetAsset<FactionSo>();
        _listView.Clear();
        _listView.bindItem = null;
        _listView.itemsSource = assets;
        _listView.bindItem = (element, index) => { ((Label)element).text = assets[index].factionEnum.ToString(); };
        _clickCallback = () => button.SetEnabled(true);
    }

    private void UpGameData(string updateType)
    {
        if (updateType == "Armor")
        {
            UpArmorData();
            return;
        }

        if (updateType == "Army")
        {
            UpArmyData();
            return;
        }

        if (updateType == "MainBuilding")
        {
            UpMainBuildingData();
            return;
        }

        if (updateType == "OtherBuilding")
        {
            UpOtherBuildingData();
            return;
        }

        if (updateType == "All")
        {
            UpArmorData();
            UpMainBuildingData();
            UpOtherBuildingData();
            UpArmyData();
        }
    }


    private void UpArmorData()
    {
        var folderPath = LoadDataTool.ArmorTypeFolderPath;
        var armorSoTable = LoadDataTool.GetAsset<ArmorSoTable>().FirstOrDefault();
        armorSoTable!.armorSoList.Clear();
        EditorUtility.SetDirty(armorSoTable);

        var existedFileName = LoadDataTool.GetAllFileNamesInPath(folderPath);
        using var fileStream = new FileStream(LoadDataTool.ExcelFilePath, FileMode.Open, FileAccess.Read);
        var workbook = new XSSFWorkbook(fileStream);
        var sheet = workbook.GetSheetAt(3);
        for (int i = 1; i <= 35; i++)
        {
            var currentRow = sheet.GetRow(i);
            var index = int.Parse(currentRow.GetCellString(0));
            var armorNameZh = currentRow.GetCellString(1).Trim();
            var armorNameEn = currentRow.GetCellString(2).Trim();
            var fileName = $"{index}_{armorNameEn}.asset";
            var filePath = "Assets" + folderPath + "/" + fileName;
            Action loadCallBack;
            ArmorSo armorSo;
            if (existedFileName.Contains(fileName))
            {
                armorSo = AssetDatabase.LoadAssetAtPath<ArmorSo>(filePath);
                loadCallBack = () => Debug.Log(armorNameZh + "---同步完成");
            }
            else
            {
                armorSo = ScriptableObject.CreateInstance<ArmorSo>();
                AssetDatabase.CreateAsset(armorSo, filePath);
                loadCallBack = () => Debug.Log(armorNameZh + "---创建并同步完成");
            }

            armorSo.SetValue(index, armorNameZh, armorNameEn);
            for (int j = 1; j <= 15; j++)
            {
                int value = int.Parse(currentRow.GetCellString(j + 3));
                armorSo.SetDamageModifiers((DamageTypeEnum)j, value);
            }

            armorSoTable!.armorSoList.Add(armorSo);
            EditorUtility.SetDirty(armorSo);
            loadCallBack.Invoke();
        }


        AssetDatabase.SaveAssets();
    }

    private void UpMainBuildingData()
    {
        var folderPath = LoadDataTool.MainBuildingsFolderPath;
        var existedFileName = LoadDataTool.GetAllFileNamesInPath(folderPath);

        var mainBuildingSoTable = LoadDataTool.MainBuildingSoTable;
        mainBuildingSoTable.mainBuildingSoTableElement.Clear();

        var alliedForcesFactionSo = LoadDataTool.AlliedForcesFactionSo;
        alliedForcesFactionSo.mainBuildings.Clear();
        var empireFactionSo = LoadDataTool.EmpireFactionSo;
        empireFactionSo.mainBuildings.Clear();
        var sovietUnionFactionSo = LoadDataTool.SovietUnionFactionSo;
        sovietUnionFactionSo.mainBuildings.Clear();

        EditorUtility.SetDirty(mainBuildingSoTable);
        EditorUtility.SetDirty(alliedForcesFactionSo);
        EditorUtility.SetDirty(empireFactionSo);
        EditorUtility.SetDirty(sovietUnionFactionSo);

        using var fileStream = new FileStream(LoadDataTool.ExcelFilePath, FileMode.Open, FileAccess.Read);
        var workbook = new XSSFWorkbook(fileStream);
        var sheet = workbook.GetSheetAt(0);
        MainBuildingSo previousMainBuildingSo = null;

        for (int i = 3; i <= 32; i++)
        {
            var currentRow = sheet.GetRow(i);
            var idString = currentRow.GetCellString(2);
            if (idString == "")
                continue;
            var id = int.Parse(idString);
            var nameEn = currentRow.GetCellString(4);
            var fileName = $"{id}_{nameEn}.asset";
            var filePath = $"Assets{folderPath}/{fileName}";
            if (!existedFileName.Contains(fileName)) // get
            {
                var mainBuildingSo = ScriptableObject.CreateInstance<MainBuildingSo>();
                EditorUtility.SetDirty(mainBuildingSo);
                AssetDatabase.CreateAsset(mainBuildingSo, filePath);
                mainBuildingSoTable.mainBuildingSoTableElement[id] = mainBuildingSo;
            }
            
        }

        for (var i = 3; i <= 32; i++)
        {
            var currentRow = sheet.GetRow(i);
            var idString = currentRow.GetCellString(2);
            // 如果ID为null，表示这一行没有信息，只是上一行的另外一个技能
            if (idString == "")
            {
                currentRow.ReadAndWriteRowToISkill(21, previousMainBuildingSo);
                continue;
            }

            var id = int.Parse(idString);
            var nameEn = currentRow.GetCellString(4);
            var fileName = $"{id}_{nameEn}.asset";
            var filePath = $"Assets{folderPath}/{fileName}";
            var mainBuildingSo = AssetDatabase.LoadAssetAtPath<MainBuildingSo>(filePath);

            var factionName = currentRow.GetCellString(1);
            if (factionName == "盟军")
                alliedForcesFactionSo.mainBuildings.Add(mainBuildingSo);
            else if (factionName == "苏联")
                sovietUnionFactionSo.mainBuildings.Add(mainBuildingSo);
            else if (factionName == "帝国")
                empireFactionSo.mainBuildings.Add(mainBuildingSo);

            // 配置主建筑的IbaseInfo和Ibuilding
            currentRow.ReadAndWriteRowToIBaseInfo(mainBuildingSo);
            currentRow.ReadAndWriteRowToIBuilding(16, mainBuildingSo);
            // 盟军的科技，这是一个特殊的存在，不可能存在技能，直接跳过
            if (currentRow.GetCellString(7).ConvertToTroop() != TroopType.Technology)
                currentRow.ReadAndWriteRowToISkill(21, mainBuildingSo);

            var requirement = currentRow.GetCellString(11);
            if (requirement != "null")
            {
                var requirements = requirement.Split(',').Select(req => req.ConvertToMainBuildingWithName()).ToList();
                mainBuildingSo.SetRequirement(requirements);
            }

            previousMainBuildingSo = mainBuildingSo;
        }

        AssetDatabase.SaveAssets();
    }

    private void UpOtherBuildingData()
    {
        var folderPath = LoadDataTool.OtherBuildingsFolderPath;
        var existedFileName = LoadDataTool.GetAllFileNamesInPath(folderPath);

        var otherBuildingSoTable = LoadDataTool.OtherBuildingSoTable;
        otherBuildingSoTable.otherBuildingSoTableElement.Clear();

        var alliedForcesFactionSo = LoadDataTool.AlliedForcesFactionSo;
        alliedForcesFactionSo.otherBuildings.Clear();
        var empireFactionSo = LoadDataTool.EmpireFactionSo;
        empireFactionSo.otherBuildings.Clear();
        var sovietUnionFactionSo = LoadDataTool.SovietUnionFactionSo;
        sovietUnionFactionSo.otherBuildings.Clear();

        EditorUtility.SetDirty(otherBuildingSoTable);
        EditorUtility.SetDirty(alliedForcesFactionSo);
        EditorUtility.SetDirty(empireFactionSo);
        EditorUtility.SetDirty(sovietUnionFactionSo);

        using FileStream fileStream = new FileStream(LoadDataTool.ExcelFilePath, FileMode.Open, FileAccess.Read);
        var workbook = new XSSFWorkbook(fileStream);
        var sheet = workbook.GetSheetAt(1);
        OtherBuildingSo previousOtherBuildingSo = null;

        for (int i = 3; i <= 28; i++)
        {
            var currentRow = sheet.GetRow(i);

            string idstring = currentRow.GetCellString(2);
            // 如果ID为null，表示这一行没有信息，只是上一行的另外一个技能
            if (idstring == "")
            {
                currentRow.ReadAndWriteRowToIWeapon(22, previousOtherBuildingSo);
                currentRow.ReadAndWriteRowToISkill(33, previousOtherBuildingSo);
                continue;
            }

            int id = int.Parse(idstring);
            string nameEn = currentRow.GetCellString(4);

            // 判断So文件是否已经存在, 并得到OtherBuildingSo
            string fileName = $"{id}_{nameEn}.asset";
            OtherBuildingSo otherBuildingSo;
            string filePath = $"Assets{folderPath}/{fileName}";
            if (existedFileName.Contains(fileName)) // get
            {
                otherBuildingSo = AssetDatabase.LoadAssetAtPath<OtherBuildingSo>(filePath);
            }
            else //create
            {
                otherBuildingSo = ScriptableObject.CreateInstance<OtherBuildingSo>();
                AssetDatabase.CreateAsset(otherBuildingSo, filePath);
            }

            otherBuildingSoTable.otherBuildingSoTableElement[id] = otherBuildingSo;

            string factionName = currentRow.GetCellString(1);
            if (factionName == "盟军")
                alliedForcesFactionSo.otherBuildings.Add(otherBuildingSo);
            else if (factionName == "苏联")
                sovietUnionFactionSo.otherBuildings.Add(otherBuildingSo);
            else if (factionName == "帝国")
                empireFactionSo.otherBuildings.Add(otherBuildingSo);

            currentRow.ReadAndWriteRowToIBaseInfo(otherBuildingSo);
            currentRow.ReadAndWriteRowToIBuilding(16, otherBuildingSo);
            currentRow.ReadAndWriteRowToIWeapon(22, otherBuildingSo);
            currentRow.ReadAndWriteRowToISkill(33, otherBuildingSo);

            EditorUtility.SetDirty(otherBuildingSo);
            previousOtherBuildingSo = otherBuildingSo;
        }

        AssetDatabase.SaveAssets();
    }

    private void UpArmyData()
    {
        var folderPath = LoadDataTool.ArmyFolderPath;
        var existedFileName = LoadDataTool.GetAllFileNamesInPath(folderPath);

        var armySoTable = LoadDataTool.ArmySoTable;
        armySoTable.armySoTableElement.Clear();

        var alliedForcesFactionSo = LoadDataTool.AlliedForcesFactionSo;
        alliedForcesFactionSo.infantry.Clear();
        alliedForcesFactionSo.vehicle.Clear();
        alliedForcesFactionSo.aircraft.Clear();
        alliedForcesFactionSo.dock.Clear();
        var empireFactionSo = LoadDataTool.EmpireFactionSo;
        empireFactionSo.infantry.Clear();
        empireFactionSo.vehicle.Clear();
        empireFactionSo.aircraft.Clear();
        empireFactionSo.dock.Clear();
        var sovietUnionFactionSo = LoadDataTool.SovietUnionFactionSo;
        sovietUnionFactionSo.infantry.Clear();
        sovietUnionFactionSo.vehicle.Clear();
        sovietUnionFactionSo.aircraft.Clear();
        sovietUnionFactionSo.dock.Clear();

        EditorUtility.SetDirty(armySoTable);
        EditorUtility.SetDirty(alliedForcesFactionSo);
        EditorUtility.SetDirty(empireFactionSo);
        EditorUtility.SetDirty(sovietUnionFactionSo);

        using FileStream fileStream = new FileStream(LoadDataTool.ExcelFilePath, FileMode.Open, FileAccess.Read);
        var workbook = new XSSFWorkbook(fileStream);
        var sheet = workbook.GetSheetAt(2);
        ArmySo previousArmySo = null;

        for (int i = 3; i <= 123; i++)
        {
            var currentRow = sheet.GetRow(i);
            string idstring = currentRow.GetCellString(2);
            // 如果ID为null，表示这一行没有信息，只是上一行的另外一个技能或武器
            if (idstring == "")
            {
                currentRow.ReadAndWriteRowToIWeapon(27, previousArmySo);
                currentRow.ReadAndWriteRowToISkill(38, previousArmySo);
                continue;
            }

            int id = int.Parse(idstring);
            string nameEn = currentRow.GetCellString(4);

            // 判断So文件是否已经存在, 并得到MainBuildingSo
            string fileName = $"{id}_{nameEn}.asset";
            ArmySo armySo;
            string filePath = $"Assets{folderPath}/{fileName}";
            if (existedFileName.Contains(fileName)) // get
            {
                armySo = AssetDatabase.LoadAssetAtPath<ArmySo>(filePath);
            }
            else //create
            {
                armySo = ScriptableObject.CreateInstance<ArmySo>();
                AssetDatabase.CreateAsset(armySo, filePath);
            }

            armySoTable.armySoTableElement[id] = armySo;

            string factionName = currentRow.GetCellString(1);
            if (factionName == "盟军")
                alliedForcesFactionSo.army.Add(armySo);
            else if (factionName == "苏联")
                sovietUnionFactionSo.army.Add(armySo);
            else if (factionName == "帝国")
                empireFactionSo.army.Add(armySo);

            currentRow.ReadAndWriteRowToIBaseInfo(armySo);
            currentRow.ReadAndWriteRowToIArmy(17, armySo);
            currentRow.ReadAndWriteRowToIWeapon(27, armySo);
            currentRow.ReadAndWriteRowToISkill(38, armySo);
            // 科技前提写入
            var requirement = currentRow.GetCellString(11);
            if (requirement != "null")
            {
                var requirements = requirement.Split(',').Select(req => req.ConvertToMainBuildingWithName())
                    .ToList();
                armySo.SetRequirement(requirements);
            }

            EditorUtility.SetDirty(armySo);
            previousArmySo = armySo;
        }

        AssetDatabase.SaveAssets();
    }
}