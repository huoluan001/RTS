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
    // 技能属性
    public enum SkillAttributes
    {
        MorphologicalTransformation,    // 形态转换
        ShortGain,   // 短暂增益
        PointToPoint,   // 点对点
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
    

    // 命令类型
    public enum CommandType
    {
        Stop,               // 停止，什么都不需要
        ForcedAttack,       // 强制攻击，需要position或gameobject
        AdvanceAttack,      // 行进攻击，需要position
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
    public enum SequenceType
    {
        None = 0,
        MainBuildingSequence = 1,
        OtherBuildingSequence = 2,
        InfantrySequence = 3,
        VehicleSequence = 4,
        DockSequence = 5,
        AircraftSequence = 6
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