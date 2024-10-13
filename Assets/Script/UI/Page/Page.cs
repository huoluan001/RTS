using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

public class Page<TScriptableObject> : MonoBehaviour where TScriptableObject : IBaseInfo
{
    public GameAsset gameAsset;
    public MainBuilding mainBuilding;
    public Transform contentTransform;
    public SequenceType sequenceType;
    public List<TScriptableObject> Infos;
    public List<Sequence<TScriptableObject>> buildingSequences = new List<Sequence<TScriptableObject>>();
    public bool isShow;
    public uint currentSequenceIndex;
    private uint nextSequenceIndex = 1;
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
        List<Image> images = contentTransform.GetComponentsInChildren<Image>().ToList();
        for (int i = 0; i < Infos.Count; i++)
            images[i].sprite = Infos[i].Icon;
        for (int i = Infos.Count; i < images.Count; i++)
            images[i].sprite = null;
    }

    public void AddSequence()
    {
        if (sequenceType == SequenceType.MainBuildingSequence)
        {
            var sequence = new Sequence<TScriptableObject>();
            sequence.page = this;
            buildingSequences.Add(sequence);
            sequence.sequenceIndex = nextSequenceIndex;
            nextSequenceIndex++;
        }
    }

}