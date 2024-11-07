using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

public class Page : MonoBehaviour
{
    public Transform contentPageTransform;
    public Transform contentMenuTransform;



    // 存储各派系的在本序列类型的IBaseInfo, Init会初始化本派系，当需求出现时，在会有其他的派系
    public Dictionary<FactionEnum, List<IBaseInfo>> ibaseInfos_Dic;

    // 控制信息，必须在初始化时赋值
    public SequenceType sequenceType;
    public List<Sequence> sequences;
    public int currentSequenceIndex;
    public Sequence currentSequence;
    public FactionEnum currrntShowFactionEnum;
    public bool isShow;

    // 最多显示的序列数量
    public int maxSequenceShowCount;
    public LinkedList<GameObject> SeqAvatars;
    public List<GameObject> taskAvatarlist;
    public GameObject SeqAvatarPrefab;
    public GameObject TaskAvatarPrefab;

    private LinkedListNode<GameObject> leftCursor;
    private LinkedListNode<GameObject> rightCursor;
    private int nextSequenceIndex = 1;
    private RectTransform rectTransform;

    public List<int> lll = new List<int>();

    public int head;
    public int tail;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        this.ShowAndHideSwitch();
    }
    public int Init(SequenceType sequenceType, bool isShow, FactionSO factionSO)
    {
        // init Page
        this.sequenceType = sequenceType;
        sequences = new List<Sequence>();
        // Page默认有一个Sequence
        var sequence = new Sequence(sequenceType, factionSO, this, nextSequenceIndex++);
        sequences.Add(sequence);
        currentSequenceIndex = 1;
        currentSequence = sequence;
        currrntShowFactionEnum = factionSO.factionEnum;
        this.isShow = isShow;

        // 初始化ibaseInfo_Dic
        ibaseInfos_Dic = new Dictionary<FactionEnum, List<IBaseInfo>>();
        ibaseInfos_Dic[factionSO.factionEnum] = GetIBaseInfoListWithSequenceTypeAndFactionSO(sequenceType, factionSO);

        // 一次不考虑图片是否更新，禁止检查，直接update
        taskAvatarlist = new List<GameObject>();
        UpdateContainImageWithCheck(false);

        // SeqAvatar init
        SeqAvatars = new LinkedList<GameObject>();
        GameObject firstSeqAvatarGameObject = Instantiate(SeqAvatarPrefab, contentMenuTransform, false);
        firstSeqAvatarGameObject.GetComponent<TMP_Text>().text = currentSequenceIndex.ToString();
        SeqAvatars.AddLast(firstSeqAvatarGameObject);
        leftCursor = SeqAvatars.First;
        rightCursor = SeqAvatars.First;
        maxSequenceShowCount = 9;


        // 返回序列编号
        return 1;
    }
    public int AddSequence(FactionSO factionSO)
    {
        var sequence = new Sequence(sequenceType, factionSO, this, nextSequenceIndex);
        sequences.Add(sequence);
        // 序列化身初始化
        GameObject seqAvatar = Instantiate(SeqAvatarPrefab);
        seqAvatar.transform.SetParent(contentMenuTransform, false);
        TMP_Text tMP_Text = seqAvatar.GetComponent<TMP_Text>();
        tMP_Text.text = nextSequenceIndex.ToString();

        SeqAvatars.AddLast(seqAvatar);
        if (SeqAvatars.Count <= maxSequenceShowCount)
            rightCursor = SeqAvatars.Last;
        else
            seqAvatar.SetActive(false);



        head = int.Parse(leftCursor.Value.transform.GetComponent<TMP_Text>().text);
        tail = int.Parse(rightCursor.Value.transform.GetComponent<TMP_Text>().text);

        lll.Clear();
        foreach (var i in SeqAvatars)
        {
            lll.Add(int.Parse(i.transform.GetComponent<TMP_Text>().text));
        }




        return nextSequenceIndex++;
    }

    private void Update()
    {
        // if (GameManager.gameAsset.commander.isRunningProduce)
        // {

        // }
        sequences.ForEach(sequence => sequence.ProductionMovesForward());
    }

    public void ShowAndHideSwitch()
    {
        if (isShow)
            rectTransform.localScale = new Vector3(1, 1, 1);
        else
            rectTransform.localScale = new Vector3(1, 0, 1);
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
        if (shouldCheckFaction && currrntShowFactionEnum == currentSequence.factionSO.factionEnum)
            return;
        List<Sprite> sprites = ibaseInfos_Dic[currentSequence.factionSO.factionEnum].Select(x => x.Icon).ToList();
        if(currentSequence.factionSO.factionEnum == FactionEnum.AlliedForces && sequenceType == SequenceType.MainBuildingSequence)
        {
            sprites = sprites.Skip(1).SkipLast(1).ToList();
        }

        if (taskAvatarlist.Count < sprites.Count)
        {
            int num = sprites.Count - taskAvatarlist.Count;
            for (int i = 0; i < num; i++)
                taskAvatarlist.Add(Instantiate(TaskAvatarPrefab, contentPageTransform, false));
        }
        else if (taskAvatarlist.Count > sprites.Count)
        {
            taskAvatarlist.RemoveRange(sprites.Count, taskAvatarlist.Count - sprites.Count);
        }
        taskAvatarlist.Zip(sprites, (avatar, sprite) => avatar.GetComponent<Image>().sprite = sprite).ToList();
    }
    public void CurrentSequenceAddTask(GameObject taskAvatarGameObject, bool isPlus = false)
    {
        currentSequence.AddTask(taskAvatarGameObject, isPlus);
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
        currentSequence.UpdateProduceTasksUI();
    }

    public void SeqAvatarLeftMove()
    {
        if (SeqAvatars.Count <= 9)
            return;
        if (leftCursor == SeqAvatars.First)
            return;
        leftCursor = leftCursor.Previous;
        leftCursor.Value.SetActive(true);
        rightCursor.Value.SetActive(false);
        rightCursor = rightCursor.Previous;
    }
    public void SeqAvatarRightMove()
    {
        if (SeqAvatars.Count <= 9)
            return;
        if (rightCursor == SeqAvatars.Last)
            return;
        leftCursor.Value.SetActive(false);
        leftCursor = leftCursor.Next;
        rightCursor = rightCursor.Next;
        rightCursor.Value.SetActive(true);
    }

    public IBaseInfo GetIBaseInfoWithIndex(int index, FactionSO factionSO)
    {
        if (!ibaseInfos_Dic.Keys.Contains(factionSO.factionEnum))
            ibaseInfos_Dic[factionSO.factionEnum] = GetIBaseInfoListWithSequenceTypeAndFactionSO(sequenceType, factionSO);
        return ibaseInfos_Dic[factionSO.factionEnum][index];
    }
    public IBaseInfo GetIBaseInfoWithIcon(Sprite icon, FactionSO factionSO)
    {
        if (!ibaseInfos_Dic.Keys.Contains(factionSO.factionEnum))
            ibaseInfos_Dic[factionSO.factionEnum] = GetIBaseInfoListWithSequenceTypeAndFactionSO(sequenceType, factionSO);
        return ibaseInfos_Dic[factionSO.factionEnum].First(ibi => ibi.Icon == icon);
    }

    public List<IBaseInfo> GetIBaseInfoListWithSequenceTypeAndFactionSO(SequenceType sequenceType, FactionSO factionSO)
    {
        return factionSO.GetBaseInfos(sequenceType);


    }
}