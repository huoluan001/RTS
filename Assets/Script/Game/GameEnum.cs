using System.Drawing;

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

    public enum Faction
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
    // 兵种
    public enum Troop
    {
        Building,   // 建筑
        Soldier,    // 步兵
        Vehicle,    // 载具
        Technology,
        None
    }

    // 军队行为模式
    public enum ArmyBehaviorModel
    {
        Aggression,     // 侵略
        Vigilance,      // 警戒
        Sticking,       // 固守
        Ceasefire,      // 停火
    }

    // 指挥官命令模式
    public enum CommandModel
    {
        None,           // 默认直接命令，立即执行
        PathPoint,      // 路径点模式，持续执行
        Plan,           // 计划模式，命令创建完成后执行
    }
    // 命令类型
    public enum CommandType
    {
        None,
        CrushMovement,      // 碾压移动，需要position
        ReverseMovement,    // 倒退移动，需要position
        Stop,               // 停止，什么都不需要
        ForcedAttack,       // 强制攻击，需要position或gameobject
        AdvanceAttack,      // 行进攻击，需要position
        
    }

    public enum Technology
    {
        T1, T2, T3, T4
    }
    // 职业定位
    public enum CareerOrientation
    {
        Anti_Infantry,      // 反步兵
        Anti_Armor,         // 反装甲
        Anti_InfantryAndArmor,  // 反步兵和装甲
        Osmosis,            // 渗透
        HeavyBombing,       // 重轰炸
        Auxiliary,          // 辅助
        Economy,            // 经济
        Maintenance         // 维修
    }

    public enum CrushLabel
    {
        Default,
        New
    }

    public enum SequenceType
    {
        None,
        MainBuildingSequence,
        OtherBuildingSequence,
        InfantrySequence,
        VehicleSequence,
        DockSequence,
        AircraftSequence
    }
    

}