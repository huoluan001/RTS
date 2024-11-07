using UnityEngine;
using UnityEngine.PlayerLoop;

public interface IActionModel
{
    public ActionModelEnum currentActionModelEnum { get; set; }

    public void Init(ActionModelEnum target = ActionModelEnum.Alert)
    {
        this.SwitchAcionModel(this, target);
    }
    public void OnEnter();
    public void OnRun();
    public void OnExit();
    public void SwitchAcionModel(IActionModel currentActionModel, ActionModelEnum target)
    {
        currentActionModel = target switch
        {
            ActionModelEnum.Invade => new Invade(),
            ActionModelEnum.Alert => new Alert(),
            ActionModelEnum.Defend => new Defend(),
            _ => new Ceasefire()
        };
        currentActionModel.currentActionModelEnum = target;
    }
}

public class Invade : IActionModel
{
    public ActionModelEnum currentActionModelEnum { get; set; }

    public virtual void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnRun()
    {
        throw new System.NotImplementedException();
    }
}
public class Alert : IActionModel
{
    public ActionModelEnum currentActionModelEnum { get; set; }

    public virtual void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnRun()
    {
        throw new System.NotImplementedException();
    }
}
public class Defend : IActionModel
{
    public ActionModelEnum currentActionModelEnum { get; set; }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnRun()
    {
        throw new System.NotImplementedException();
    }
}
public class Ceasefire : IActionModel
{
    public ActionModelEnum currentActionModelEnum { get; set; }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnRun()
    {
        throw new System.NotImplementedException();
    }
}