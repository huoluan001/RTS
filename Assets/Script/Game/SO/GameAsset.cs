using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GameAsset", menuName = "ScriptableObjects/Programming/GameAsset")]
public class GameAsset : ScriptableObject
{
    public Player commander;

    public MainBuildingSOPage mainBuildingSOPage;
    public OtherBuildingSOPage otherBuildingSOPage;
    public ArmySOPage armySOPage;
    public ArmorSOPage armorSOPage;
    // inputSystem
    public RTSInputSystem inputSystem;
    public RTSInputSystem.PlayerActions player;

    [Header("UI")]
    public FactionSO factionSO;
    public GameObject selectionBoxUI;
    [HideInInspector] public EventSystem eventSystem;
    [HideInInspector] public GraphicRaycaster graphicRaycaster;
    public GameObject mainBuildingSequencePagePrefab;

    // CameraMove
    public Vector2 cameraMoveVector2;
    public float cameraMoveSpeed;
    public float cameraRotationSpeed;

    public List<BuildingLabelSO> buildLabel;
    public List<BuildingLabelSO> ProductionLabel;

}
