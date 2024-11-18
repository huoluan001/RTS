using UnityEditor.Rendering;
using UnityEngine;

public class ProduceTask
{
    public IBaseInfo Info;
    public int Count;
    public float Value;
    public Sequence Sequence;
    public TaskState CurrentTaskState;

    private float _produceSpeed4Price;
    private float _produceSpeed4Value;
    private TaskAvatar _taskAvatar;


    public ProduceTask(IBaseInfo info, int count, Sequence sequence, TaskAvatar taskAvatar)
    {
        Info = info;
        Count = count;
        Value = 1f;
        Sequence = sequence;
        this._taskAvatar = taskAvatar;
        CurrentTaskState = TaskState.Waiting;
        taskAvatar.Tasks.Add(this);

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
        _taskAvatar.UpdateValue(Value);
        if (_taskAvatar.Completed())
        {
            Sequence.EndTaskCallBack();
            // success
            // Instantiate(info.GameObjectPrefab);
            if (Count == 1)
            {
                _taskAvatar.Coating.fillAmount = 1;
                _taskAvatar.Tasks.Remove(this);
            }
            else
            {
                Count--;
                Value = 1f;
                _taskAvatar.UpdateValue(Value);
                _taskAvatar.UpdateCount(Count);
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