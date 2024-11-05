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

    public SequenceType sequenceType;
    public List<Sequence> sequences = new List<Sequence>();

    // 存储各派系的在本序列类型的IBaseInfo, Init会初始化本派系，当需求出现时，在会有其他的派系
    public Dictionary<FactionEnum, List<IBaseInfo>> ibaseInfos_Dic;

    public int currentSequenceIndex;
    public Sequence currentSequence;
    public FactionEnum currrntShowFaction;

    public bool isShow;
    public int maxSequenceIndex;
    public GameObject firstAvatar;
    public LinkedList<GameObject> SeqAvatars;
    public GameObject SeqAvatarPrefab;
    private LinkedListNode<GameObject> leftCursor;
    private LinkedListNode<GameObject> rightCursor;
    private int nextSequenceIndex = 1;
    private RectTransform rectTransform;
    void Start()
    {

        rectTransform = GetComponent<RectTransform>();
        this.ShowAndHideSwitch();
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
        SeqAvatars = new LinkedList<GameObject>();
        SeqAvatars.AddLast(firstAvatar);
        leftCursor = SeqAvatars.First;
        rightCursor = SeqAvatars.First;

        // 初始化ibaseInfo_Dic
        ibaseInfos_Dic = new Dictionary<FactionEnum, List<IBaseInfo>>();
        ibaseInfos_Dic[factionSO.factionEnum] = GetIBaseInfoListWithSequenceTypeAndFactionSO(sequenceType, factionSO);

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
        if (SeqAvatars.Count < maxSequenceIndex)
            rightCursor = SeqAvatars.Last;
        return nextSequenceIndex++;
    }

    private void Update()
    {
        if (GameManager.gameAsset.commander.isRunningProduce)
        {
            sequences.ForEach(sequence => sequence.ProductionMovesForward());
        }
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
    public void CurrentSequenceAddTask(Sprite icon, GameObject item, bool isPlus = false)
    {
        currentSequence.AddTask(icon,item, isPlus);
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

    public IBaseInfo GetIBaseInfo(int index, FactionSO factionSO)
    {
        if (!ibaseInfos_Dic.Keys.Contains(factionSO.factionEnum))
            ibaseInfos_Dic[factionSO.factionEnum] = GetIBaseInfoListWithSequenceTypeAndFactionSO(sequenceType, factionSO);
        return ibaseInfos_Dic[factionSO.factionEnum][index];
    }
    public IBaseInfo GetIBaseInfo(Sprite icon, FactionSO factionSO)
    {
        if (!ibaseInfos_Dic.Keys.Contains(factionSO.factionEnum))
            ibaseInfos_Dic[factionSO.factionEnum] = GetIBaseInfoListWithSequenceTypeAndFactionSO(sequenceType, factionSO);
        return ibaseInfos_Dic[factionSO.factionEnum].First(ibi => ibi.Icon == icon);
    }

    public List<IBaseInfo> GetIBaseInfoListWithSequenceTypeAndFactionSO(SequenceType sequenceType, FactionSO factionSO)
    {
        return sequenceType switch
        {
            SequenceType.MainBuildingSequence => factionSO.MainBuildings.Cast<IBaseInfo>().ToList(),
            SequenceType.OtherBuildingSequence => factionSO.OtherBuildings.Cast<IBaseInfo>().ToList(),
            SequenceType.InfantrySequence => factionSO.Infantry.Cast<IBaseInfo>().ToList(),
            SequenceType.VehicleSequence => factionSO.Vehicle.Cast<IBaseInfo>().ToList(),
            SequenceType.DockSequence => factionSO.Dock.Cast<IBaseInfo>().ToList(),
            _ => factionSO.Dock.Cast<IBaseInfo>().ToList()
        };
    }
}