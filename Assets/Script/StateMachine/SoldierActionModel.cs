public interface ISoldierActionModel
{
    void Init();
    void Run();
    void Exit();
}

public enum SoldierActionModel
{
    Idle,
    Move,
    Attack
}

public class IdleSoldierAction : ISoldierActionModel
{
    public void Init()
    {
        
    }

    public void Run()
    {
       
    }

    public void Exit()
    {
        
    }
}

public class MoveSoldierAction : ISoldierActionModel
{
    public void Init()
    {
        
    }

    public void Run()
    {
       
    }

    public void Exit()
    {
        
    }
}

public class AttackSoldierAction : ISoldierActionModel
{
    public void Init()
    {
        
    }

    public void Run()
    {
       
    }

    public void Exit()
    {
        
    }
}