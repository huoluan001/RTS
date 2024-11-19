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
    public readonly FactionSo FactionSo;

    public readonly int CurrentSequenceMaxTaskCount;
    public int CurrentSequenceAllTaskCount;

    public LinkedList<ProduceTask> TaskList;
    public Dictionary<TaskAvatar, LinkedList<ProduceTask>> TaskMap;

    private bool _stopAddTask;

    public Sequence(SequenceType sequenceType, FactionSo factionSo, Page page, int sequenceIndex)
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
        TaskList = new LinkedList<ProduceTask>();
        TaskMap = new Dictionary<TaskAvatar, LinkedList<ProduceTask>>();
    }

    public void AddTask(GameObject taskAvatarGameObject, int count)
    {
        Image icon = taskAvatarGameObject.GetComponent<Image>();
        var info = Page.GetIBaseInfoWithIcon(icon.sprite, FactionSo);
        TaskAvatar taskAvatar = taskAvatarGameObject.GetComponent<TaskAvatar>();
        if (CurrentSequenceAllTaskCount >= CurrentSequenceMaxTaskCount) return;
        count = Math.Min(CurrentSequenceMaxTaskCount - CurrentSequenceAllTaskCount, count);
        CurrentSequenceAllTaskCount += count;
        if (CurrentSequenceAllTaskCount == CurrentSequenceMaxTaskCount)
        {
            Page.taskAvatarlist.ForEach(ta => ta.Icon.material = GameManager.GameAsset.grayscale);
            _stopAddTask = true;
        }

        // 如果task队列为空，或者最后一个任务不是本次添加任务的同一个任务，则创建一个新的任务
        if (TaskList.Count == 0 || TaskList.Last.Value.Info != info)
        {
            var task = new ProduceTask(info, count, this, taskAvatar);
            taskAvatar.currentState = TaskAvatar.TaskAvatarState.HaveTask;
            TaskMap[taskAvatar].AddLast(task);
            TaskList.AddLast(task);
        }
        else
        {
            TaskList.Last.Value.Count += count;
        }
        taskAvatar.AddCount(count);
    }

    public void ProductionMovesForward()
    {
        if (TaskList.Count == 0)
            return;
        TaskList.First(t => t.CurrentTaskState != ProduceTask.TaskState.Paused).Forward();
        
    }
    

    public void EndTaskCallBack()
    {
        if (_stopAddTask)
        {
            _stopAddTask = false;
            if (this == Page.GetCurrentSequence())
            {
                Page.taskAvatarlist.ForEach(ta => ta.Icon.material = null);
            }
            
        }
    }
}