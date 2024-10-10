using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GameAsset", menuName = "ScriptableObjects/Programming/GameAsset")]
public class GameAsset : ScriptableObject
{
    // inputSystem
    public RTSInputSystem inputSystem;
    public RTSInputSystem.PlayerActions player;

    [Header("UI")]
    public FactionSO factionSO;
    public GameObject selectionBoxUI;
    [HideInInspector] public EventSystem eventSystem;
    [HideInInspector] public GraphicRaycaster graphicRaycaster;
    public GameObject sequencePrefab;

    // CameraMove
    public Vector2 cameraMoveVector2;
    public float cameraMoveSpeed;
    public float cameraRotationSpeed;

}
