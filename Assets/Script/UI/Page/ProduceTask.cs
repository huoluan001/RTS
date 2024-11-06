using UnityEditor.Rendering;
using UnityEngine;

public class ProduceTask<TScriptableObject> where TScriptableObject : IBaseInfo
{
    public dynamic info;
    public Sequence sequence;
    public int count;
    private float produceSpeed4Price;
    private float produceSpeed4Value;
    private GameObject gameObjectPrefab;
    private TaskAvatar taskAvatar;

    public ProduceTask(TScriptableObject info, Sequence sequence, TaskAvatar taskAvatar)
    {
        this.sequence = sequence;
        this.taskAvatar = taskAvatar;
        this.count = 0;
        if(info is MainBuildingSO mainBuildingSO)
        {
            this.info = mainBuildingSO;
            produceSpeed4Price = info.Price / mainBuildingSO.BuildingAndPlacementTime.x;
            produceSpeed4Value = 1 / mainBuildingSO.BuildingAndPlacementTime.x;
        }
        else if (info is OtherBuildingSO otherBuildingSO)
        {
            this.info = otherBuildingSO;
            produceSpeed4Price = info.Price / otherBuildingSO.BuildingAndPlacementTime.x;
            produceSpeed4Value = 1 / otherBuildingSO.BuildingAndPlacementTime.x;
        }
        else if (info is ArmySO armySO)
        {
            this.info = armySO;
            produceSpeed4Price = info.Price / armySO.BuildingTime;
            produceSpeed4Value = 1 / armySO.BuildingTime;
        }
    }

    public void ProductionMovesForward()
    {
        // var commander = GameManager.gameAsset.commander;
        // if(commander.Fund < produceSpeed4Price * Time.deltaTime)
        // {
        //     commander.ProduceStop();
        // }
        // else
        // {
            // commander.Pay(produceSpeed4Price * Time.deltaTime);
            taskAvatar.Coating.fillAmount -= produceSpeed4Value * Time.deltaTime;
            if(taskAvatar.Coating.fillAmount <= 0)
            {
                // end 完成一次生产
                // var gameObject = GameObject.Instantiate(gameObjectPrefab);
                Debug.Log("success");
                count--;
                sequence.currentSequenceAllTaskCount--;
                if(count <= 0)
                {
                    sequence.EndCurrentTaskCallBack(taskAvatar);
                }
                else
                {
                    taskAvatar.Coating.fillAmount = 1;
                }
            }
        // }
    }

    
}