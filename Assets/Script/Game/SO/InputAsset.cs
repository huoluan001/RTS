using UnityEngine;

[CreateAssetMenu(fileName = "InputAsset", menuName = "ScriptableObjects/Programming/InputAsset")]
public class InputAsset : ScriptableObject
{
    // inputSystem
    public RTSInputSystem inputSystem;
    public RTSInputSystem.PlayerActions player;

    // input Data
    public bool LeftShiftDown;
    public bool LeftControlDown;
}