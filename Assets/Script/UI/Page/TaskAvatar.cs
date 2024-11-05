using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskAvatar : MonoBehaviour
{
    public TMP_Text tMP_Text;
    [SerializeField]
    private GameObject imageGameObject;
    public Image Coating => imageGameObject.GetComponent<Image>();
}