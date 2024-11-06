using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using System;
public class Sequence
{

    public int sequenceIndex;
    public Page page;
    public SequenceType sequenceType;
    public FactionSO factionSO;

    public int currentSequenceMaxTaskCount;
    public int currentSequenceAllTaskCount;
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
        currentSequenceAllTaskCount = 0;
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

    public void EndCurrentTaskCallBack(TaskAvatar taskAvatar)
    {
        // 化身的图片从灰度恢复到彩色
        taskAvatar.GetComponent<Image>().material = null;
        // 黄色的涂层消失
        taskAvatar.Coating.gameObject.SetActive(false);
        // 数字标记消失
        taskAvatar.tMP_Text.gameObject.SetActive(false);
        produceTasks.Remove(produceTasks.First().Key);

    }

    public void AddTask(GameObject taskAvatarGameObject, bool isPlus = false)
    {
        if(currentSequenceAllTaskCount >= currentSequenceMaxTaskCount)
        {
            return;
        }
        int addCount = isPlus ? 5 : 1;
        addCount = Math.Min(currentSequenceMaxTaskCount - currentSequenceAllTaskCount, addCount);
        

        Image icon = taskAvatarGameObject.GetComponent<Image>();
        var info = page.GetIBaseInfo(icon.sprite, factionSO);
        TaskAvatar taskAvatar = taskAvatarGameObject.GetComponent<TaskAvatar>();
        Image Coating = taskAvatar.Coating;
        TMP_Text tMP_Text = taskAvatar.tMP_Text;

        // new task
        if (!produceTasks.ContainsKey(info))
        {
            
            produceTasks.Add(info, new ProduceTask<IBaseInfo>(info, this, taskAvatar));
            // 图标变为灰色, 涂层打开，数量标记打开
            icon.material = GameManager.gameAsset.grayscale;
            Coating.gameObject.SetActive(true);
            Coating.fillAmount = 1f;
            tMP_Text.gameObject.SetActive(true);
        }
        produceTasks[info].count += addCount;
        currentSequenceAllTaskCount += addCount;
        tMP_Text.text = produceTasks[info].count.ToString();
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