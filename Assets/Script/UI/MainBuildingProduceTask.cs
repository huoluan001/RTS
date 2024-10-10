using UnityEngine;

public class MainBuildingProduceTask
{
    public uint unit_Id;
    public float timeSingle;
    public uint count;

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