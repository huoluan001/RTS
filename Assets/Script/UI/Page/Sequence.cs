using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
public class Sequence
{

    public int sequenceIndex;
    public Page page;
    public SequenceType sequenceType;
    public FactionSO factionSO;

    public int currentSequenceMaxTaskCount = 1;
    public int currentSequenceAllTaskCount = 0;
    private Dictionary<IBaseInfo, ProduceTask<IBaseInfo>> produceTasks;
    public Sequence(SequenceType sequenceType, FactionSO factionSO, Page page, int sequenceIndex)
    {
        this.sequenceType = sequenceType;
        if (sequenceType == SequenceType.MainBuildingSequence || page.sequenceType == SequenceType.OtherBuildingSequence)
        {
            currentSequenceMaxTaskCount = 1;
        }
        else
        {
            currentSequenceMaxTaskCount = 99;
        }
        this.factionSO = factionSO;
        this.page = page;
        this.sequenceIndex = sequenceIndex;
        produceTasks = new Dictionary<IBaseInfo, ProduceTask<IBaseInfo>>();

    }

    public void ProductionMovesForward()
    {
        // 序列的第一个任务向前生产
        if (produceTasks.Count == 0)
            return;
        produceTasks.First().Value.ProductionMovesForward();
    }

    public void EndCurrentTaskCallBack()
    {
        produceTasks.Remove(produceTasks.First().Key);
    }


    public void AddTask(GameObject taskAvatarGameObject, bool isPlus = false)
    {
        Image image = taskAvatarGameObject.GetComponent<Image>();
        var info = page.GetIBaseInfo(image.sprite, factionSO);
        TaskAvatar taskAvatar = taskAvatarGameObject.GetComponent<TaskAvatar>();
        Image Coating = taskAvatar.Coating;
        TMP_Text tMP_Text = taskAvatar.tMP_Text;

        // new task
        if (!produceTasks.ContainsKey(info))
        {
            produceTasks.Add(info, new ProduceTask<IBaseInfo>(info, this, taskAvatar));
            // 图标变为灰色, 涂层打开，数量标记打开
            taskAvatarGameObject.transform.GetComponent<Image>().material = GameManager.gameAsset.grayscale;
            Coating.gameObject.SetActive(true);
            Coating.fillAmount = 1;
            tMP_Text.gameObject.SetActive(true);
        }

        if (isPlus)
        {
            produceTasks[info].AddTaskPlus();
        }
        else
        {
            produceTasks[info].AddTask();
        }


        if (isPlus)
        {
            produceTasks[info].AddTaskPlus();
        }
        else
        {
            produceTasks[info].AddTask();
        }
        tMP_Text.text = produceTasks[info].Count.ToString();


    }

    public void CancalTask(IBaseInfo info)
    {
        if (produceTasks.ContainsKey(info))
        {
            produceTasks[info].ReductionOneTask();
        }
    }

    public void ClearTask(IBaseInfo info)
    {
        if (produceTasks.ContainsKey(info))
        {
            produceTasks.Remove(info);
        }
    }
    // 使用SeqTask同步UI
    public void UpdateProduceTasksUI()
    {
        foreach (var kv in produceTasks)
        {
            // page.contentPageTransform.FindChild()
        }
    }
}