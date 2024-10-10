using UnityEngine;

[CreateAssetMenu(fileName = "GameSetting", menuName = "ScriptableObjects/Programming/GameSetting")]
public class GameSetting : ScriptableObject
{
    public Vector2 windowSize = new Vector2(1920, 1080);
    [Range(0,10)] public uint selectionBoxLineWidth;

    public ShowLanguage asas;
}
