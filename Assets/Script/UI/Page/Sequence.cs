using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
public class Sequence
{
    public uint sequenceIndex;
    public FactionSO factionSO;
    public Page page;
    public List<IBaseInfo> baseInfos;
    public Dictionary<IBaseInfo, ProduceTask<IBaseInfo>> produceTasks;
    public bool isRunning;
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

    public void ProductionMovesForward()
    {
        // 序列的第一个任务向前生产
        produceTasks.First().Value.ProductionMovesForward();
        

    }


    public void AddTask(IBaseInfo info, bool isPlus)
    {
        // new task
        if(!produceTasks.ContainsKey(info))
        {
            produceTasks.Add(info, new ProduceTask<IBaseInfo>(info));
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

    public void CancalTask(IBaseInfo info)
    {
        if(produceTasks.ContainsKey(info))
        {
            produceTasks[info].CancalTask();
        }
    }

    public void ClearTask(IBaseInfo info)
    {
        if(produceTasks.ContainsKey(info))
        {
            produceTasks.Remove(info);
        }
    }
}