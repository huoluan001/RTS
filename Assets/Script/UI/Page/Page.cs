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

    public SequenceType sequenceType;
    public List<Sequence> sequences = new List<Sequence>();

    public int currentSequenceIndex;
    public Sequence currentSequence;
    public FactionEnum currrntShowFaction;

    public bool isShow;

    public int maxSequenceIndex;
    public LinkedList<GameObject> SeqAvatars;
    public GameObject SeqAvatarPrefab;
    private LinkedListNode<GameObject> leftCursor;
    private LinkedListNode<GameObject> rightCursor;
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
        // 一次不考虑图片是否更新，禁止检查，直接update
        UpdateContainImageWithCheck(false);
        currrntShowFaction = factionSO.factionEnum;
        // SeqAvatar init
        leftCursor = SeqAvatars.First;
        rightCursor = SeqAvatars.First;
        SeqAvatars.First.Value.GetComponent<Button>().onClick.AddListener(() => SwitchCurrentSequence(1));

        // 返回序列编号
        return 1;
    }
    public int AddSequence(FactionSO factionSO)
    {
        var sequence = new Sequence(sequenceType, factionSO, this, nextSequenceIndex);
        sequences.Add(sequence);
        GameObject seqAvatar = Instantiate(SeqAvatarPrefab);
        seqAvatar.transform.SetParent(contentMenuTransform, false);
        seqAvatar.GetComponent<Button>().onClick.AddListener(() => SwitchCurrentSequence(nextSequenceIndex));
        TMP_Text tMP_Text = seqAvatar.GetComponent<TMP_Text>();
        tMP_Text.text = nextSequenceIndex.ToString();
        SeqAvatars.AddLast(seqAvatar);
        if (SeqAvatars.Count < maxSequenceIndex)
            rightCursor = SeqAvatars.Last;
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

    private void UpdateContainImageWithCheck(bool shouldCheckFaction = true)
    {
        if (shouldCheckFaction && currrntShowFaction == currentSequence.factionSO.factionEnum) return;
        List<Sprite> sprites = currentSequence.factionSO.GetBaseInfos(sequenceType).Select(m => m.Icon).ToList();

        List<Image> images = contentPageTransform.GetComponentsInChildren<Image>().ToList();
        for (int i = 0; i < images.Count; i++)
        {
            images[i].sprite = (i < sprites.Count) ? sprites[i] : null;
        }
    }



    public Sequence GetCurrentSequence()
    {
        return sequences.First(sequence => sequence.sequenceIndex == currentSequenceIndex);
    }

    public void SwitchCurrentSequence(int targetIndex)
    {
        currentSequenceIndex = targetIndex;
        currentSequence = GetCurrentSequence();
        UpdateContainImageWithCheck();
    }

    public void SeqAvatarLeftMove()
    {
        if (SeqAvatars.Count <= 9) return;
        leftCursor = leftCursor.Previous;
        leftCursor.Value.SetActive(true);
        rightCursor.Value.SetActive(false);
        rightCursor = rightCursor.Previous;

    }
    public void SeqAvatarRightMove()
    {
        if (SeqAvatars.Count <= 9) return;
        leftCursor.Value.SetActive(false);
        leftCursor = leftCursor.Next;
        rightCursor = rightCursor.Next;
        rightCursor.Value.SetActive(true);
    }

}