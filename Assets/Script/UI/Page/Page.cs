using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

public class Page<TScriptableIbject> : MonoBehaviour where TScriptableIbject : IBaseInfo
{
    public GameAsset gameAsset;
    public MainBuilding mainBuilding;
    public Transform contentTransform;
    public SequenceType sequenceType;
    public List<TScriptableIbject> Infos;
    public List<Sequence<TScriptableIbject>> buildingSequences = new List<Sequence<TScriptableIbject>>();
    public bool isShow;
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
            var sequence = new Sequence<TScriptableIbject>();
            buildingSequences.Add(sequence);
            sequence.sequenceIndex = nextSequenceIndex;
            nextSequenceIndex++;
        }
    }

}