using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;
using System;


public class Page : MonoBehaviour
{
    public Transform contentPageTransform;
    public Transform contentMenuTransform;
    public List<TMP_Text> sequenceTMPIndexs;
    public SequenceType sequenceType;
    public List<Sequence> sequences = new List<Sequence>();

    public int currentSequenceIndex;
    public Sequence currentSequence;
    public FactionEnum currrntShowFaction;

    public bool isShow;

    public int maxSequenceIndex;
    public GameObject sequenceIndexPrefab;
    private TMP_Text leftCursor;
    private TMP_Text rightCursor;
    private int nextSequenceIndex = 1;
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public int Init(SequenceType sequenceType, Transform transformParent, bool isShow, FactionSO factionSO)
    {
        // init Page
        this.sequenceType = sequenceType;
        transform.SetParent(transformParent, false);
        this.isShow = isShow;

        // Page默认有一个Sequence
        var sequence = new Sequence(sequenceType, factionSO, this, 1);
        sequences.Add(sequence);
        nextSequenceIndex = 2;
        currentSequenceIndex = 1;
        currentSequence = sequence;
        // 一次不考虑图片是否更新，直接Load
        LoadContainImage();
        currrntShowFaction = factionSO.factionEnum;
        // sequence的TMPindex init
        leftCursor = sequenceTMPIndexs[0];
        rightCursor = sequenceTMPIndexs[0];
        sequenceTMPIndexs[0].GetComponent<Button>().onClick.AddListener(() => SwitchCurrentSequence(1));

        // 返回序列编号
        return 1;
    }
    public int AddSequence(FactionSO factionSO)
    {
        var sequence = new Sequence(sequenceType, factionSO, this, nextSequenceIndex);
        sequences.Add(sequence);
        GameObject sequenceindexTMP = Instantiate(sequenceIndexPrefab);
        sequenceindexTMP.transform.SetParent(contentMenuTransform, false);
        sequenceindexTMP.GetComponent<Button>().onClick.AddListener(() => SwitchCurrentSequence(nextSequenceIndex));
        sequenceTMPIndexs.Add(sequenceindexTMP.GetComponent<TMP_Text>());
        if(sequenceTMPIndexs.Count < maxSequenceIndex) rightCursor = sequenceindexTMP.GetComponent<TMP_Text>();
        return nextSequenceIndex++;
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
        if (isShow)
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
        if (currrntShowFaction == currentSequence.factionSO.factionEnum) return;
        List<Sprite> Sprites = currentSequence.factionSO.GetBaseInfos(sequenceType).Select(m => m.Icon).ToList();

        List<Image> images = contentPageTransform.GetComponentsInChildren<Image>().ToList();
        for (int i = 0; i < Sprites.Count; i++)
            images[i].sprite = Sprites[i];
        for (int i = Sprites.Count; i < images.Count; i++)
            images[i].sprite = null;
    }
    private void LoadContainImage()
    {
        List<Sprite> Sprites = currentSequence.factionSO.GetBaseInfos(sequenceType).Select(m => m.Icon).ToList();

        List<Image> images = contentPageTransform.GetComponentsInChildren<Image>().ToList();
        for (int i = 0; i < Sprites.Count; i++)
            images[i].sprite = Sprites[i];
        for (int i = Sprites.Count; i < images.Count; i++)
            images[i].sprite = null;
    }



    public Sequence GetCurrentSequence()
    {
        return sequences.First(sequence => sequence.sequenceIndex == currentSequenceIndex);
    }

    public void SwitchCurrentSequence(int targetIndex)
    {
        currentSequenceIndex = targetIndex;
        currentSequence = GetCurrentSequence();
        UpdateContainImage();
    }

}