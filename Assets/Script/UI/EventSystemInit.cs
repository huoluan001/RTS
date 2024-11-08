using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemInit : MonoBehaviour
{
    private void Awake()
    {
        GameManager.gameAsset.eventSystem = GetComponent<EventSystem>();
    }
}