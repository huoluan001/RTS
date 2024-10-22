using UnityEditor.Rendering;
using UnityEngine;

public class ProduceTask<TScriptableObject> where TScriptableObject : IBaseInfo
{
    public GameAsset gameAsset;
    public dynamic info;
    private uint _count;
    private uint Count => _count;
    public float value = 0;
    private float produceSpeed4Price;
    private float produceSpeed4Value;
    private GameObject gameObjectPrefab;

    public ProduceTask(TScriptableObject info)
    {
        
        if(info is MainBuildingSO mainBuildingSO)
        {
            this.info = mainBuildingSO;
            produceSpeed4Price = info.BuildingPrice / mainBuildingSO.BuildingAndPlacementTime.x;
            produceSpeed4Value = 1 / mainBuildingSO.BuildingAndPlacementTime.x;
        }
        else if (info is OtherBuildingSO otherBuildingSO)
        {
            this.info = otherBuildingSO;
            produceSpeed4Price = info.BuildingPrice / otherBuildingSO.BuildingAndPlacementTime.x;
            produceSpeed4Value = 1 / otherBuildingSO.BuildingAndPlacementTime.x;
        }
        else if (info is ArmySO armySO)
        {
            this.info = armySO;
            produceSpeed4Price = info.BuildingPrice / armySO.BuildingTime;
            produceSpeed4Value = 1 / armySO.BuildingTime;
        }
    }

    public void ProductionMovesForward()
    {
        if(gameAsset.commander.Fund < produceSpeed4Price * Time.deltaTime)
        {
            gameAsset.commander.ProduceStop();
        }
        else
        {
            gameAsset.commander.Pay(produceSpeed4Price * Time.deltaTime);
            value += produceSpeed4Value * Time.deltaTime;
            if(value >= 1)
            {
                // end
            }
        }
    }

    public void AddTaskPlus()
    {
        _count += 5;
    }
    public void AddTask()
    {
        _count++;
    }
    public void CancalTask()
    {
        _count--;
    }
}