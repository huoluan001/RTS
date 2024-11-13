using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using Object = UnityEngine.Object;

public class DataManager : EditorWindow
{
    private ListView _listView;
    private ScrollView _scrollView;

    private Button _factionButton;
    private Button _mBButton;
    private Button _oBButton;
    private Button _armyButton;
    private Button _armorButton;

    private Action _clickCallback;

    // [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

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

        _listView = rootVisualElement.Q<ListView>("SOList");
        _scrollView = rootVisualElement.Q<ScrollView>("SOInfo");
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

        _factionButton.clickable.clicked += () => OnClickFactionSO(_factionButton);
        _mBButton.clickable.clicked += () => OnClick<MainBuildingSO>(_mBButton);
        _oBButton.clickable.clicked += () => OnClick<OtherBuildingSO>(_oBButton);
        _armyButton.clickable.clicked += () => OnClick<ArmySO>(_armyButton);
        _armorButton.clickable.clicked += () =>
        {
            _listView.style.scale = new Vector2(0f, 0f);
        };
       
    }


    private List<T> GetAsset<T>() where T : Object
    {
        var type = typeof(T).Name;
        var guids = AssetDatabase.FindAssets($"t:{type}");
        var assets = new List<T>();
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            assets.Add(AssetDatabase.LoadAssetAtPath<T>(path));
        }

        return assets;
    }

    private void OnClick<T>(Button button) where T : Object, IBaseInfo
    {
        _clickCallback?.Invoke();
        button.SetEnabled(false);
        List<T> assets = GetAsset<T>();
        _listView.Clear();
        _listView.bindItem = null;
        _listView.itemsSource = assets;
        _listView.bindItem = (element, index) => { ((Label)element).text = assets[index].NameEnglish; };
        _clickCallback = () => button.SetEnabled(true);
    }

    private void OnClickFactionSO(Button button)
    {
        _clickCallback?.Invoke();
        button.SetEnabled(false);
        List<FactionSO> assets = GetAsset<FactionSO>();
        _listView.Clear();
        _listView.bindItem = null;
        _listView.itemsSource = assets;
        _listView.bindItem = (element, index) => { ((Label)element).text = assets[index].factionEnum.ToString(); };
        _clickCallback = () => button.SetEnabled(true);
    }
    
}