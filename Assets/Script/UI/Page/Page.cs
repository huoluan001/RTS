using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

public class Page : MonoBehaviour
{
    public Transform contentPageTransform;
    public Transform contentMenuTransform;

    // 存储各派系的在本序列类型的IBaseInfo, Init会初始化本派系，当需求出现时，在添加其他的派系
    private Dictionary<FactionEnum, List<IBaseInfo>> _iBaseInfosDic;

    // 控制信息，必须在初始化时赋值
    public SequenceType sequenceType;
    [SerializeField]
    public List<Sequence> _sequences;
    public int currentSequenceIndex;
    private Sequence _currentSequence;
    public FactionEnum currentShowFactionEnum;
    public bool isShow;

    // 最多显示的序列数量
    private int _maxSequenceShowCount;
    private LinkedList<GameObject> _seqAvatars;
    public List<TaskAvatar> taskAvatarlist;
    public GameObject SeqAvatarPrefab;
    public GameObject TaskAvatarPrefab;

    private LinkedListNode<GameObject> _leftCursor;
    private LinkedListNode<GameObject> _rightCursor;
    private int _nextSequenceIndex = 1;
    private RectTransform _rectTransform;
    public List<int> lll = new List<int>();
    public int head;
    public int tail;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        this.ShowAndHideSwitch();
    }
    public int Init(SequenceType sequenceType, bool isShow, FactionSo factionSo)
    {
        // init Page
        this.sequenceType = sequenceType;
        _sequences = new List<Sequence>();

        // 初始化ibaseInfo_Dic
        _iBaseInfosDic = new Dictionary<FactionEnum, List<IBaseInfo>>();
        _iBaseInfosDic[factionSo.factionEnum] = GetIBaseInfoListWithSequenceTypeAndFactionSo(sequenceType, factionSo);

        // Page默认有一个Sequence
        var sequence = new Sequence(sequenceType, factionSo, this, _nextSequenceIndex);
        _sequences.Add(sequence);
        currentSequenceIndex = _nextSequenceIndex;
        _currentSequence = sequence;
        currentShowFactionEnum = factionSo.factionEnum;
        this.isShow = isShow;
        // 一次不考虑图片是否更新，禁止检查，直接update
        taskAvatarlist = new List<TaskAvatar>();
        UpdateContainImageWithCheck(false);

        // SeqAvatar init
        _seqAvatars = new LinkedList<GameObject>();
        GameObject firstSeqAvatarGameObject = Instantiate(SeqAvatarPrefab, contentMenuTransform, false);
        firstSeqAvatarGameObject.GetComponent<TMP_Text>().text = currentSequenceIndex.ToString();
        _seqAvatars.AddLast(firstSeqAvatarGameObject);
        _leftCursor = _seqAvatars.First;
        _rightCursor = _seqAvatars.First;
        _maxSequenceShowCount = 9;


        // 返回序列编号
        return _nextSequenceIndex++;
    }
    public int AddSequence(FactionSo factionSo)
    {
        var sequence = new Sequence(sequenceType, factionSo, this, _nextSequenceIndex);
        _sequences.Add(sequence);
        // 序列化身初始化
        GameObject seqAvatar = Instantiate(SeqAvatarPrefab, contentMenuTransform, false);
        TMP_Text tMP_Text = seqAvatar.GetComponent<TMP_Text>();
        tMP_Text.text = _nextSequenceIndex.ToString();

        _seqAvatars.AddLast(seqAvatar);
        if (_seqAvatars.Count <= _maxSequenceShowCount)
            _rightCursor = _seqAvatars.Last;
        else
            seqAvatar.SetActive(false);

        head = int.Parse(_leftCursor.Value.transform.GetComponent<TMP_Text>().text);
        tail = int.Parse(_rightCursor.Value.transform.GetComponent<TMP_Text>().text);

        lll.Clear();
        foreach (var i in _seqAvatars)
        {
            lll.Add(int.Parse(i.transform.GetComponent<TMP_Text>().text));
        }
        return _nextSequenceIndex++;
    }

    private void Update()
    {
        _sequences.ForEach(sequence => sequence.ProductionMovesForward());
    }



    private void UpdateContainImageWithCheck(bool shouldCheckFaction = true)
    {
        if (shouldCheckFaction && currentShowFactionEnum == _currentSequence.FactionSo.factionEnum)
            return;
        List<Sprite> sprites = _iBaseInfosDic[_currentSequence.FactionSo.factionEnum].Select(x => x.Icon).ToList();
        if (_currentSequence.FactionSo.factionEnum == FactionEnum.AlliedForces && sequenceType == SequenceType.MainBuildingSequence)
        {
            sprites = sprites.Skip(1).SkipLast(1).ToList();
        }
        int spritesCount = sprites.Count;
        int taskAvatarsCount = taskAvatarlist.Count;


        for (int i = taskAvatarsCount; i < spritesCount; i++)
            taskAvatarlist.Add(Instantiate(TaskAvatarPrefab, contentPageTransform, false).GetComponent<TaskAvatar>());

        for (int i = 0; i < taskAvatarsCount; i++)
        {
            taskAvatarlist[i].gameObject.SetActive(i < spritesCount);
        }



        sprites.Zip(taskAvatarlist, (sprite, taskAvatar) => taskAvatar.Icon.sprite = sprite).ToArray();
    }
    public void CurrentSequenceAddTask(GameObject taskAvatarGameObject, bool isPlus = false)
    {
        _currentSequence.OnClickMouseLeft(taskAvatarGameObject);
    }

    public void CurrentSequencePauseOrCancelTask(GameObject taskAvatarGameObject)
    {
        _currentSequence.OnClickMouseRight(taskAvatarGameObject);
    }

    public Sequence GetCurrentSequence() => _sequences.First(sequence => sequence.SequenceIndex == currentSequenceIndex);


    public void SwitchCurrentSequence(int targetIndex)
    {
        currentSequenceIndex = targetIndex;
        _currentSequence = GetCurrentSequence();
        UpdateContainImageWithCheck();
        taskAvatarlist.ForEach(ta => ta.SwitchTaskAvatar(TaskAvatarState.NoTask));
        foreach (var currtask in _currentSequence.TaskList)
        {
            var taskAvatar = currtask.TaskAvatar;

            if (_currentSequence.TaskMap[taskAvatar].First() == currtask)
            {
                taskAvatar.SwitchTaskAvatar(TaskAvatarState.HaveTask);
                taskAvatar.UpdateValue(currtask.Value);
            }

            taskAvatar.AddCount(currtask.Count);
        }
        if (_currentSequence.StopAddTask)
        {
            taskAvatarlist.ForEach(ta => ta.Icon.material = GameManager.GameAsset.grayscale);
        }

    }

    public void SeqAvatarLeftMove()
    {
        if (_seqAvatars.Count <= 9)
            return;
        if (_leftCursor == _seqAvatars.First)
            return;
        _leftCursor = _leftCursor.Previous;
        _leftCursor.Value.SetActive(true);
        _rightCursor.Value.SetActive(false);
        _rightCursor = _rightCursor.Previous;
    }
    public void SeqAvatarRightMove()
    {
        if (_seqAvatars.Count <= 9)
            return;
        if (_rightCursor == _seqAvatars.Last)
            return;
        _leftCursor.Value.SetActive(false);
        _leftCursor = _leftCursor.Next;
        _rightCursor = _rightCursor.Next;
        _rightCursor.Value.SetActive(true);
    }

    public IBaseInfo GetIBaseInfoWithIcon(Sprite icon, FactionSo factionSo)
    {
        if (!_iBaseInfosDic.Keys.Contains(factionSo.factionEnum))
            _iBaseInfosDic[factionSo.factionEnum] = GetIBaseInfoListWithSequenceTypeAndFactionSo(sequenceType, factionSo);
        return _iBaseInfosDic[factionSo.factionEnum].First(ibi => ibi.Icon == icon);
    }

    public List<IBaseInfo> GetIBaseInfoListWithSequenceTypeAndFactionSo(SequenceType sequenceType, FactionSo factionSo)
    {
        return factionSo.GetBaseInfos(sequenceType);
    }
    public void ShowAndHideSwitch()
    {
        if (isShow)
            _rectTransform.localScale = new Vector3(1, 1, 1);
        else
            _rectTransform.localScale = new Vector3(1, 0, 1);
    }
    public void Show()
    {
        _rectTransform.localScale = new Vector3(1, 1, 1);
        isShow = true;
    }
    public void Hide()
    {
        _rectTransform.localScale = new Vector3(1, 0, 1);
        isShow = false;
    }
}