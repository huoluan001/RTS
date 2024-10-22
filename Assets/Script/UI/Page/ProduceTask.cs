using UnityEditor.Rendering;
using UnityEngine;

public class ProduceTask<TScriptableObject> where TScriptableObject : IBaseInfo
{
    public dynamic info;
    public Sequence sequence;
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
        var commander = GameManager.gameAsset.commander;
        if(commander.Fund < produceSpeed4Price * Time.deltaTime)
        {
            commander.ProduceStop();
        }
        else
        {
            commander.Pay(produceSpeed4Price * Time.deltaTime);
            value += produceSpeed4Value * Time.deltaTime;
            if(value >= 1)
            {
                // end 完成一次生产
                var gameObject = GameObject.Instantiate(gameObjectPrefab);
                _count--;
                if(_count == 0)
                {
                    sequence.EndCurrentTaskCallBack();
                }
                else
                {
                    value = 0;
                }
                

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
    public void ReductionOneTask()
    {
        _count--;
    }
}