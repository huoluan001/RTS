using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;

[Serializable]
public class Sequence
{
    public readonly int SequenceIndex;
    private readonly Page _page;
    public SequenceType SequenceType;
    public readonly FactionSo FactionSo;
    private readonly int _currentSequenceMaxTaskCount;
    [SerializeField]
    private int _currentSequenceAllTaskCount;
    public List<ProduceTask> TaskList;
    public Dictionary<TaskAvatar, LinkedList<ProduceTask>> TaskMap;
    public bool StopAddTask;
    private HashSet<SequenceType> buildingSequenceTypeHashMap = new HashSet<SequenceType>() { SequenceType.MainBuildingSequence, SequenceType.OtherBuildingSequence };

    public Sequence(SequenceType sequenceType, FactionSo factionSo, Page page, int sequenceIndex)
    {
        (this.SequenceType, this.FactionSo, this._page, this.SequenceIndex) = (sequenceType, factionSo, page, sequenceIndex);
        _currentSequenceMaxTaskCount = buildingSequenceTypeHashMap.Contains(sequenceType) ? 10 : 99;
        _currentSequenceAllTaskCount = 0;
        TaskList = new List<ProduceTask>();
        TaskMap = new Dictionary<TaskAvatar, LinkedList<ProduceTask>>();
        StopAddTask = false;
    }

    public void OnClickMouseLeft(GameObject taskAvatarGameObject)
    {
        TaskAvatar taskAvatar = taskAvatarGameObject.GetComponent<TaskAvatar>();
        if (!TaskMap.ContainsKey(taskAvatar) || TaskMap[taskAvatar].Count == 0)
        {
            AddTask(taskAvatar);
            return;
        }
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
            AddTask(taskAvatar);
        }
    }

    public void OnClickMouseRight(GameObject taskAvatarGameObject)
    {
        TaskAvatar taskAvatar = taskAvatarGameObject.GetComponent<TaskAvatar>();
        if (!TaskMap.ContainsKey(taskAvatar) || TaskMap[taskAvatar].Count == 0)
        {
            return;
        }
        TaskState currentState = TaskMap[taskAvatar].First.Value.CurrentTaskState;
        if (currentState == TaskState.Waiting)
        {
            if (GameManager.InputAsset.LeftShiftDown)
            {
                CancelTask(taskAvatar);
            }
            else
            {
                PauseTask(taskAvatar);
            }
        }
        else if (currentState == TaskState.Paused)
        {
            CancelTask(taskAvatar);
        }
        else if (currentState == TaskState.Ready)
        {
            CancelTask(taskAvatar);
        }

    }
    private void BuildingTask(TaskAvatar taskAvatar)
    {

    }


    private void StartTask(TaskAvatar taskAvatar)
    {
        foreach (var task in TaskMap[taskAvatar])
            task.CurrentTaskState = TaskState.Waiting;
    }

    private void AddTask(TaskAvatar taskAvatar)
    {
        // 已经达到上限
        if (_currentSequenceAllTaskCount >= _currentSequenceMaxTaskCount)
            return;
        int count = GameManager.InputAsset.LeftShiftDown ? 5 : 1;
        count = Math.Min(_currentSequenceMaxTaskCount - _currentSequenceAllTaskCount, count);
        _currentSequenceAllTaskCount += count;
        if (_currentSequenceAllTaskCount == _currentSequenceMaxTaskCount)
        {
            _page.taskAvatarlist.ForEach(ta => ta.Icon.material = GameManager.GameAsset.grayscale);
            StopAddTask = true;
        }
        var info = _page.GetIBaseInfoWithIcon(taskAvatar.Icon.sprite, FactionSo);
        // 如果task队列为空，或者最后一个任务不是本次添加任务的同一个任务，则创建一个新的任务
        if (TaskList.Count == 0 || TaskList.Last().Info != info)
        {
            var task = new ProduceTask(info, count, this, taskAvatar);
            taskAvatar.SwitchTaskAvatar(TaskAvatarState.HaveTask);
            if (!TaskMap.ContainsKey(taskAvatar))
            {
                TaskMap[taskAvatar] = new LinkedList<ProduceTask>();
            }
            TaskMap[taskAvatar].AddLast(task);
            TaskList.Add(task);
        }
        else
            TaskList.Last().Count += count;

        taskAvatar.AddCount(count);
    }


    public void CancelTask(TaskAvatar taskAvatar)
    {
        foreach (var task in TaskMap[taskAvatar])
            TaskList.Remove(task);
        TaskMap[taskAvatar].Clear();
    }

    public void PauseTask(TaskAvatar taskAvatar)
    {
        foreach (var task in TaskMap[taskAvatar])
            task.CurrentTaskState = TaskState.Paused;
    }

    public void ProductionMovesForward()
    {
        if (TaskList.Count == 0)
            return;
        TaskList.FirstOrDefault(t => t.CurrentTaskState != TaskState.Paused)?.Forward();
    }


    public void EndTaskCallBack()
    {
        _currentSequenceAllTaskCount--;
        if (StopAddTask)
        {
            StopAddTask = false;
            if (this == _page.GetCurrentSequence())
            {
                _page.taskAvatarlist.ForEach(ta => ta.Icon.material = null);
            }
        }
    }
}

[Serializable]
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
        (this.Info, this.Count, this._sequence, this.TaskAvatar, Value, CurrentTaskState) =
                (info, count, sequence, taskAvatar, 1f, TaskState.Waiting);

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
        // if (GameManager.GameAsset.commander.Fund < _produceSpeed4Price)
        //return;
        //GameManager.GameAsset.commander.Fund -= _produceSpeed4Price;
        Value -= _produceSpeed4Value * Time.deltaTime;
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
                TaskAvatar.AddCount(-Count);
            }
            else
            {
                Count--;
                Value = 1f;
                TaskAvatar.AddCount(-1);
            }
        }
    }

}

