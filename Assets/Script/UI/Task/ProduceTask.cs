using UnityEngine;

public class ProduceTask<TScriptableIbject>
{
    public MainBuildingSO info;
    public float timeSingle;
    public uint count;
    public bool isRun;

    public ProduceTask(MainBuildingSO info)
    {
        this.info = info;
        timeSingle = info.BuildingAndPlacementTime.x;

    }

    public void AddTaskPlus()
    {
        count+=5;
    }
    public void AddTask()
    {
        count++;
    }
    public void CancalTask()
    {
        count--;
        if(count < 0)
        {
            count = 0;
        }
    }

    public void ClearTask()
    {
        count = 0;
    }
}