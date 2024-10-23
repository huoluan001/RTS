using UnityEngine;

[CreateAssetMenu(fileName = "MainBuildingSOPage", menuName = "ScriptableObjects/Data/MainBuildingSOPage")]
public class MainBuildingSOPage : ScriptableObject
{
    public MainBuildingSOPageElement mainBuildingSOPageElement;
}
[CreateAssetMenu(fileName = "OtherBuildingSOPage", menuName = "ScriptableObjects/Data/OtherBuildingSOPage")]
public class OtherBuildingSOPage : ScriptableObject
{
    public OtherBuildingSOPageElement otherBuildingSOPageElement;
}
[CreateAssetMenu(fileName = "ArmySOPage", menuName = "ScriptableObjects/Data/ArmySOPage")]
public class ArmySOPage : ScriptableObject
{
    public ArmySOPageElement armySOPageElement;
}