using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

public class Page : MonoBehaviour
{
    public GameAsset gameAsset;
    public MainBuilding mainBuilding;
    public Transform contentTransform;
    public SequenceType sequenceType;
    public List<Sequence> sequences = new List<Sequence>();
    public bool isShow;
    public int currentSequenceIndex;
    private int nextSequenceIndex = 1;
    
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (isShow) Show();
        else Hide();
        UpdateContainImage();
        AddSequence();
    }

    private void Update()
    {
        if(gameAsset.commander.isRunningProduce)
        {
            sequences.ForEach(sequence => sequence.ProductionMovesForward());
        }
    }
    private void Show()
    {
        rectTransform.localScale = new Vector3(1, 1, 1);
    }
    private void Hide()
    {
        rectTransform.localScale = new Vector3(1, 0, 1);
    }

    private void UpdateContainImage()
    {
        var infos = sequences.Where(sequence => sequence.sequenceIndex == currentSequenceIndex).ToArray().First().baseInfos;
        List<Image> images = contentTransform.GetComponentsInChildren<Image>().ToList();
        for (int i = 0; i < infos.Count; i++)
            images[i].sprite = infos[i].Icon;
        for (int i = infos.Count; i < images.Count; i++)
            images[i].sprite = null;
    }

    public void AddSequence()
    { 
        if (sequenceType == SequenceType.MainBuildingSequence)
        {
            var sequence = new Sequence();
            sequence.page = this;
            sequences.Add(sequence);
            sequence.sequenceIndex = nextSequenceIndex;
            nextSequenceIndex++;
        }
    }

}