using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class Page : MonoBehaviour
{
    // page 隶属的主建筑/生产建筑
    public MainBuilding mainBuilding;

    public SequenceType sequenceType;
    public GameAsset gameAsset;
    // 下一个生产序列的编号
    private uint nextSequenceIndex = 1;
    // 所有的生产序列
    public List<Sequence> sequences = new List<Sequence>();

    public List<Image> images;
    public bool isShow;
    private RectTransform rectTransform;
    public List<MainBuildingSO> buildingInfos;
    public List<ArmySO> armyInfos;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (isShow) Show();
        else Hide();
    }
    private void Show()
    {
        rectTransform.localScale = new Vector3(1, 1, 1);
    }
    private void Hide()
    {
        rectTransform.localScale = new Vector3(1, 0, 1);
    }

    public void AddSequence()
    {

    }

   
}