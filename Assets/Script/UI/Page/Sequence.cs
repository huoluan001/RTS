using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;

public class Sequence
{
    public readonly int SequenceIndex;
    private readonly Page _page;
    //public SequenceType SequenceType;
    public readonly FactionSo FactionSo;

    private readonly int _currentSequenceMaxTaskCount;
    private int _currentSequenceAllTaskCount;

    public LinkedList<ProduceTask> TaskList;
    public Dictionary<TaskAvatar, LinkedList<ProduceTask>> TaskMap;

    private bool _stopAddTask;

    public Sequence(SequenceType sequenceType, FactionSo factionSo, Page page, int sequenceIndex)
    {
        //this.SequenceType = sequenceType;
        if (sequenceType == SequenceType.MainBuildingSequence ||
            page.sequenceType == SequenceType.OtherBuildingSequence)
        {
            _currentSequenceMaxTaskCount = 1;
        }
        else
        {
            _currentSequenceMaxTaskCount = 99;
        }

        _currentSequenceAllTaskCount = 0;
        FactionSo = factionSo;
        _page = page;
        SequenceIndex = sequenceIndex;
        TaskList = new LinkedList<ProduceTask>();
        TaskMap = new Dictionary<TaskAvatar, LinkedList<ProduceTask>>();
    }

    public void OnClickMouseLeft(GameObject taskAvatarGameObject, int count = 1)
    {
        TaskAvatar taskAvatar = taskAvatarGameObject.GetComponent<TaskAvatar>();
        TaskState currentState = TaskMap[taskAvatar].First.Value.CurrentTaskState;
        if (currentState == TaskState.Paused)
        {
            StartTask(taskAvatar);
        }
        else if (currentState == TaskState.Ready)
        {
            BuildingTask(taskAvatar);
        }
        else if (currentState == TaskState.Waiting)
        {
            AddTask(taskAvatar, count);
        }
    }

    private void StartTask(TaskAvatar taskAvatar)
    {
        foreach (var task in TaskMap[taskAvatar])
            task.CurrentTaskState = TaskState.Waiting;
    }

    private void BuildingTask(TaskAvatar taskAvatar)
    {
        
    }

    private void AddTask(TaskAvatar taskAvatar, int count)
    {
        if (_currentSequenceAllTaskCount >= _currentSequenceMaxTaskCount)
            return;
        var info = _page.GetIBaseInfoWithIcon(taskAvatar.Icon.sprite, FactionSo);

        count = Math.Min(_currentSequenceMaxTaskCount - _currentSequenceAllTaskCount, count);
        _currentSequenceAllTaskCount += count;
        if (_currentSequenceAllTaskCount == _currentSequenceMaxTaskCount)
        {
            _page.taskAvatarlist.ForEach(ta => ta.Icon.material = GameManager.GameAsset.grayscale);
            _stopAddTask = true;
        }

        // 如果task队列为空，或者最后一个任务不是本次添加任务的同一个任务，则创建一个新的任务
        if (TaskList.Count == 0 || TaskList.Last.Value.Info != info)
        {
            var task = new ProduceTask(info, count, this, taskAvatar);
            taskAvatar.currentState = TaskAvatarState.HaveTask;
            TaskMap[taskAvatar].AddLast(task);
            TaskList.AddLast(task);
        }
        else
            TaskList.Last.Value.Count += count;

        taskAvatar.AddCount(count);
    }


    public void CancelTask(GameObject taskAvatarGameObject)
    {
        TaskAvatar taskAvatar = taskAvatarGameObject.GetComponent<TaskAvatar>();
        if (TaskMap[taskAvatar].First.Value.CurrentTaskState == TaskState.Paused)
        {
            foreach (var task in TaskMap[taskAvatar])
            {
                TaskList.Remove(task);
            }
        }

        TaskMap[taskAvatar].Clear();
    }

    public void PauseTask(GameObject taskAvatarGameObject)
    {
        TaskAvatar taskAvatar = taskAvatarGameObject.GetComponent<TaskAvatar>();
        if (TaskMap[taskAvatar].First.Value.CurrentTaskState != TaskState.Paused)
        {
            foreach (var task in TaskMap[taskAvatar])
            {
                task.CurrentTaskState = TaskState.Paused;
            }
        }
    }

    public void ProductionMovesForward()
    {
        if (TaskList.Count == 0)
            return;
        TaskList.First(t => t.CurrentTaskState != TaskState.Paused).Forward();
    }


    public void EndTaskCallBack()
    {
        if (_stopAddTask)
        {
            _stopAddTask = false;
            if (this == _page.GetCurrentSequence())
            {
                _page.taskAvatarlist.ForEach(ta => ta.Icon.material = null);
            }
        }
    }
}

public class ProduceTask
{
    public readonly IBaseInfo Info;
    public int Count;
    public float Value;
    private readonly Sequence _sequence;
    public readonly TaskAvatar TaskAvatar;
    public TaskState CurrentTaskState;
    private readonly float _produceSpeed4Price;
    private readonly float _produceSpeed4Value;

    public ProduceTask(IBaseInfo info, int count, Sequence sequence, TaskAvatar taskAvatar)
    {
        Info = info;
        Count = count;
        Value = 1f;
        _sequence = sequence;
        this.TaskAvatar = taskAvatar;
        CurrentTaskState = TaskState.Waiting;

        int price = info.Price;
        float time;
        if (info is MainBuildingSo || info is OtherBuildingSo)
        {
            time = ((MainBuildingSo)info).BuildingAndPlacementTime.x;
        }
        else
        {
            time = ((ArmySo)info).BuildingTime;
        }

        _produceSpeed4Price = price / time;
        _produceSpeed4Value = 1 / time;
    }

    public void Forward()
    {
        if (CurrentTaskState == TaskState.Ready)
            return;
        if (GameManager.GameAsset.commander.Fund < _produceSpeed4Price)
            return;
        GameManager.GameAsset.commander.Fund -= _produceSpeed4Price;
        Value -= _produceSpeed4Value;
        TaskAvatar.UpdateValue(Value);
        if (Value <= 0f)
        {
            _sequence.EndTaskCallBack();
            // success
            // Instantiate(info.GameObjectPrefab);
            if (Count == 1) // 最后一个单位，本任务结束
            {
                _sequence.TaskList.Remove(this);
                _sequence.TaskMap[TaskAvatar].Remove(this);
                if (_sequence.TaskMap[TaskAvatar].Count == 0)
                {
                    TaskAvatar.SwitchTaskAvatar(TaskAvatarState.NoTask);
                    return;
                }

                TaskAvatar.UpdateValue(1f);
            }
            else
            {
                Count--;
                Value = 1f;
                TaskAvatar.UpdateValue(Value);
                TaskAvatar.AddCount(-Count);
            }
        }
    }
}