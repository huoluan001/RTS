using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
public class Sequence : MonoBehaviour
{
    public bool isShow;

    public List<Image> images;
    private RectTransform rectTransform;

    public List<MainBuildingSO> BuildingInfos;
    public List<ArmorSO> ArmorInfos;

    public float currentProduce;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (isShow) Show();
        else Hide();
        UpdateInfo();
    }

    private void Update()
    {

    }

    private void Show()
    {
        rectTransform.localScale = new Vector3(1, 1, 1);
    }
    private void Hide()
    {
        rectTransform.localScale = new Vector3(1, 0, 1);
    }

    private void UpdateInfo()
    {
        if(BuildingInfos.Count != 0)
        {
            for (int i = 0; i < BuildingInfos.Count; i++)
            {
                images[i].sprite = BuildingInfos[i].Icon;
            }
            for (int i = BuildingInfos.Count; i < images.Count; i++)
            {
                images[i].sprite = null;
            }
        }
    }
}