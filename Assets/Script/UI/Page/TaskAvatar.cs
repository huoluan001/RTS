using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TaskAvatar : MonoBehaviour
{
    // count label
    public TMP_Text TMPText;
    public Image Coating;
    public Image Icon;

    [SerializeField] private GameObject coatingGameObject;

    public List<ProduceTask> Tasks;
    private Material _grayscaleMaterial;

    public void Start()
    {
        Tasks = new List<ProduceTask>();
        Icon = GetComponent<Image>();
        Coating = coatingGameObject.GetComponent<Image>();
        _grayscaleMaterial = GameManager.GameAsset.grayscale;
    }


    public void UpdateValue(float value)
    {
        Coating.fillAmount = value;
    }

    public bool Completed()
    {
        return Coating.fillAmount <= 0f;
    }

    public void UpdateCount(int count)
    {
        TMPText.text = count.ToString();
    }

    public void IconMaterialChanged()
    {
        Icon.material = Icon.material == null ? _grayscaleMaterial : null;
    }
}