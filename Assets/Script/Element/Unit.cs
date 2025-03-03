using Script.Enum;
using UnityEngine;
using Animancer;

// Unit只声明共有字段，不实现方法
public class Unit : MonoBehaviour
{
    public const float MOTION_START_TIME = 0.0001f;
    public const float MOTION_END_TIME = 0.9999f;
    
    public Player commander;
    public FindTargetModel currentFindTargetModel = FindTargetModel.Aggression;
    
    
    [Header("Components")]
    public AnimancerComponent animancer;
    
    

    public virtual void F()
    {
        
    }
}