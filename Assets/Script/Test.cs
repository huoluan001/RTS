using UnityEngine;
using Animancer;
public class Test : MonoBehaviour
{
    private AnimancerComponent _Animancer;
    [SerializeField] public ClipTransition move;
    public ClipTransition load;
    public AvatarMask avatarMask;
    private void Awake()
    {
        _Animancer = GetComponent<AnimancerComponent>();
        _Animancer.Layers[0].Play(move);
        _Animancer.Layers[1].SetMask(avatarMask);
        _Animancer.Layers[1].Play(load);
    }
}