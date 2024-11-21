using UnityEngine;
using System;
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

    public virtual void Forward()
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

public class BuildingProduceTask : ProduceTask
{
    public BuildingProduceTask(IBaseInfo info, int count, Sequence sequence, TaskAvatar taskAvatar) :
            base(info, count, sequence, taskAvatar)
    {

    }
    public override void Forward()
    {
        base.Forward();
    }
}

