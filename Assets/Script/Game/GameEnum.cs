namespace UnityEngine
{
    public enum TeamId
    {
        None, One, Two, Three, Four, Five,
    }

    public enum Identity
    {
        Dependent, Suzerain
    }

    public enum FactionEnum
    {
        None, AlliedForces, SovietUnion, Empire
    }

    // 界面语言
    public enum ShowLanguage
    {
        English, Chinese
    }
    
    // 军队等级
    public enum Level
    {
        None, One, Two, Star
    }
    
    // 攻击模式
    public enum AttackModel
    {
        // 陆地, 海洋， 空中，潜艇，航空
        ToLand, ToOcean, ToAir, ToSubmarine, ToAviation
    }
    // 行动范围
    public enum ActionScope
    {
        Land,   // 陆地
        Ocean,  // 海洋
        Air,    // 空中
        Submarine,  // 下潜
        Aviation,   // 航空
        None,
    }
    

    public enum Technology
    {
        T1, T2, T3, T4
    }
    // 职业定位


    public enum CrushLabel
    {
        Default,
        New
    }

    public enum TaskState
    {
        Waiting,
        Paused,
        Ready
    }
    public enum TaskAvatarState
    {
        HaveTask,
        NoTask,
    }
    

}