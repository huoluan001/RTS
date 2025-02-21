namespace Script.Enum
{
    public enum CommandModel
    {
        None,           // 默认直接命令，立即执行
        PathPoint,      // 路径点模式，持续执行
        Plan,           // 计划模式，命令创建完成后执行
    }
}