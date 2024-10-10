using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemInit : MonoBehaviour
{
    public GameAsset gameAsset;
    private void Start()
    {
        gameAsset.eventSystem = GetComponent<EventSystem>();
    }
}