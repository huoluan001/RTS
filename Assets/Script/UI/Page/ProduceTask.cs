using UnityEditor.Rendering;
using UnityEngine;

public class ProduceTask
{
    public IBaseInfo Info;
    public int Count;
    public float Value;
    public Sequence Sequence;
    public TaskAvatar TaskAvatar;
    public TaskState CurrentTaskState;
    

    private float _produceSpeed4Price;
    private float _produceSpeed4Value;
    


    public ProduceTask(IBaseInfo info, int count, Sequence sequence, TaskAvatar taskAvatar)
    {
        Info = info;
        Count = count;
        Value = 1f;
        Sequence = sequence;
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
            Sequence.EndTaskCallBack();
            // success
            // Instantiate(info.GameObjectPrefab);
            if (Count == 1) // 最后一个单位，本任务结束
            {
                Sequence.TaskList.Remove(this);
                Sequence.TaskMap[TaskAvatar].Remove(this);
                if (Sequence.TaskMap[TaskAvatar].Count == 0)
                {
                    TaskAvatar.SwitchTaskAvatar(TaskAvatar.TaskAvatarState.NoTask);
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

    public enum TaskState
    {
        Running,
        Waiting,
        Paused,
        Ready
    }
}