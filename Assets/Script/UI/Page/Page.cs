using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class Page : MonoBehaviour
{
    public Transform contentTransform;
    public SequenceType sequenceType;
    
    public List<Sequence> sequences = new List<Sequence>();
    public bool isShow;
    public int currentSequenceIndex = 1;
    private int nextSequenceIndex = 1;
    
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // if(gameAsset.commander.isRunningProduce)
        // {
        //     sequences.ForEach(sequence => sequence.ProductionMovesForward());
        // }
    }


    public void ShowAndHideSwitch()
    {
        if(isShow)
        {   
            rectTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            rectTransform.localScale = new Vector3(1, 0, 1);
        }
    }
    public void Show()
    {
        rectTransform.localScale = new Vector3(1, 1, 1);
        isShow = true;
    }
    public void Hide()
    {
        rectTransform.localScale = new Vector3(1, 0, 1);
        isShow = false;
    }

    private void UpdateContainImage()
    {
        var sequence = sequences.First(sequence => sequence.sequenceIndex == currentSequenceIndex);
        var faction = sequence.factionSO;
        List<Sprite> Sprites = faction.GetBaseInfos(sequenceType).Select(m => m.Icon).ToList();

        List<Image> images = contentTransform.GetComponentsInChildren<Image>().ToList();
        for (int i = 0; i < Sprites.Count; i++)
            images[i].sprite = Sprites[i];
        for (int i = Sprites.Count; i < images.Count; i++)
            images[i].sprite = null;
    }

    public int AddSequence(FactionSO factionSO)
    { 
        var sequence = new Sequence(sequenceType, factionSO, this, nextSequenceIndex);
        sequences.Add(sequence);
        return nextSequenceIndex++;
    }

}