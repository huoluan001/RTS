using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GameAsset", menuName = "ScriptableObjects/Programming/GameAsset")]
public class GameAsset : ScriptableObject
{
    public Player commander;

    public BuildingManager buildingManager;

    

    [Header("UI")]
    [HideInInspector] public UIManager UIManager;
    [HideInInspector] public GameObject selectionBoxUI;
    [HideInInspector] public EventSystem eventSystem;
    [HideInInspector] public GraphicRaycaster graphicRaycaster;
    public GameObject mainBuildingSequencePagePrefab;

    // CameraMove
    public Vector2 cameraMoveVector2;
    public float cameraMoveSpeed;
    public float cameraRotationSpeed;


    // Mouse And Keyboard
    
    
    
    public Material grayscale;

}
