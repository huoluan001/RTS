using Script.Enum;
using UnityEngine;
using Animancer;

// Unit只声明共有字段，不实现方法
public class Unit : MonoBehaviour
{
    public Player commander;
    public AnimancerComponent animancer;
    public FindTargetModel currentFindTargetModel;
}