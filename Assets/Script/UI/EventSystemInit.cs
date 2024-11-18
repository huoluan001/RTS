using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemInit : MonoBehaviour
{
    private void Awake()
    {
        GameManager.GameAsset.eventSystem = GetComponent<EventSystem>();
    }
}