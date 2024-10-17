using UnityEditor.Rendering;
using UnityEngine;
111

public class ProduceTask<TScriptableObject> where TScriptableObject : IBaseInfo
{
    public dynamic info;
    
    private uint _count;
    private uint Count => _count;
    public bool isRun;
    private float timeSingle;
    public ProduceTask(TScriptableObject info)
    {
        if(info is MainBuildingSO mainBuildingSO)
        {
            this.info = mainBuildingSO;
        }
        else if (info is OtherBuildingSO otherBuildingSO)
        {
            this.info = otherBuildingSO;
        }
        else if (info is ArmySO armySO)
        {
            this.info = armySO;
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