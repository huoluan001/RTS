using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskAvatar : MonoBehaviour
{
    public TMP_Text tMP_Text;
    [SerializeField]
    private GameObject CoatingGameObject;
    public Image Coating => CoatingGameObject.GetComponent<Image>();
}