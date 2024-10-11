using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
public class Sequence<TScriptableIbject>
{
    public uint sequenceIndex;
    public Page<TScriptableIbject> Page;
    public LinkedList<ProduceTask<TScriptableIbject>> produceTasks;

    public uint currentSequenceMaxTaskCount = 1;

    public void AddTask(MainBuildingSO info)
    {
        var tasks = produceTasks.Where(task => task.info == info).ToArray();
        // nex task
        if(tasks.Count() == 0)
        {
            produceTasks.AddLast(new ProduceTask<TScriptableIbject>(info));
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