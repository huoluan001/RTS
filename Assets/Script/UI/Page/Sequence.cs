using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
public class Sequence<TScriptableObject> where TScriptableObject : IBaseInfo
{
    public uint sequenceIndex;
    public Page<TScriptableObject> page;
    public Dictionary<TScriptableObject, ProduceTask<TScriptableObject>> produceTasks;
    public uint currentSequenceMaxTaskCount = 1;

    public Sequence()
    {
        if(page.sequenceType == SequenceType.MainBuildingSequence 
            || page.sequenceType == SequenceType.OtherBuildingSequence)
        {
            currentSequenceMaxTaskCount = 1;
        }
        else
        {
            currentSequenceMaxTaskCount = 99;
        }
    }


    public void AddTask(TScriptableObject info, bool isPlus)
    {
        // new task
        if(!produceTasks.ContainsKey(info))
        {
            produceTasks.Add(info, new ProduceTask<TScriptableObject>(info));
        }
        if(isPlus)
        {
            produceTasks[info].AddTaskPlus();
        }
        else
        {
            produceTasks[info].AddTask();
        } 
    }

    public void CancalTask(TScriptableObject info)
    {
        if(produceTasks.ContainsKey(info))
        {
            produceTasks[info].CancalTask();
        }
    }

    public void ClearTask(TScriptableObject info)
    {
        if(produceTasks.ContainsKey(info))
        {
            produceTasks.Remove(info);
        }
    }
}