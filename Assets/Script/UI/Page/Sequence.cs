using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using System;

public class Sequence
{
    public readonly int SequenceIndex;
    public readonly Page Page;
    public SequenceType SequenceType;
    public readonly FactionSO FactionSo;

    public readonly int CurrentSequenceMaxTaskCount;
    public int CurrentSequenceAllTaskCount;

    private LinkedList<ProduceTask> _taskList;

    public Sequence(SequenceType sequenceType, FactionSO factionSo, Page page, int sequenceIndex)
    {
        this.SequenceType = sequenceType;
        if (sequenceType == SequenceType.MainBuildingSequence ||
            page.sequenceType == SequenceType.OtherBuildingSequence)
        {
            CurrentSequenceMaxTaskCount = 1;
        }
        else
        {
            CurrentSequenceMaxTaskCount = 99;
        }

        CurrentSequenceAllTaskCount = 0;
        this.FactionSo = factionSo;
        this.Page = page;
        this.SequenceIndex = sequenceIndex;
        _taskList = new LinkedList<ProduceTask>();
    }

    public void AddTask(GameObject taskAvatarGameObject, int count)
    {
        Image icon = taskAvatarGameObject.GetComponent<Image>();
        var info = Page.GetIBaseInfoWithIcon(icon.sprite, FactionSo);
        TaskAvatar taskAvatar = taskAvatarGameObject.GetComponent<TaskAvatar>();
        if (CurrentSequenceAllTaskCount >= CurrentSequenceMaxTaskCount) return;
        count = Math.Min(CurrentSequenceMaxTaskCount - CurrentSequenceAllTaskCount, count);
        CurrentSequenceAllTaskCount += count;

        if (_taskList.Count == 0 || _taskList.Last.Value.Info != info)
        {
            var task = new ProduceTask(info, count, this, taskAvatar);
            _taskList.AddLast(task);
        }
        else
        {
            _taskList.Last.Value.Count += count;
        }
    }

    public void ProductionMovesForward()
    {
        if (_taskList.Count == 0)
            return;
        _taskList.First(t => t.CurrentTaskState != ProduceTask.TaskState.Paused).Forward();
    }


    // // 使用SeqTask同步UI
    // public void UpdateProduceTasksUI()
    // {
    //     foreach (var kv in produceTasks)
    //     {
    //         page.contentPageTransform.FindChild()
    //     }
    // }
}