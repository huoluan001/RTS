using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
public class MainBuildingSequence
{
    public uint sequenceIndex;
    public MainBuildingPage mainBuildingPage;
    public LinkedList<MainBuildingProduceTask> produceTasks;

    public uint currentSequenceMaxTaskCount = 1;

    public void AddTask(MainBuildingSO info)
    {
        var tasks = produceTasks.Where(task => task.info == info).ToArray();
        // nex task
        if(tasks.Count() == 0)
        {
            produceTasks.AddLast(new MainBuildingProduceTask(info));
            tasks[0].AddTask();
        }
        // old task add count
        else
        {
            tasks[0].AddTask();
        }
    }

    public void RemoveTask()
    {

    }

    public void ClearTask()
    {

    }


    

    
}