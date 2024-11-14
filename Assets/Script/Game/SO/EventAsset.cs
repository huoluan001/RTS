using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EventAsset", menuName = "ScriptableObjects/Programming/EventAsset")]
public class EventAsset : ScriptableObject
{
    public Action MouseLeftClickPerformed;
    
    public Action MouseLeftClickCanceled;
}