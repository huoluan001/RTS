using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TaskAvatar : MonoBehaviour
{
    // count label
    public TMP_Text TMPText;
    public Image Coating;
    public Image Icon;

    [SerializeField] private GameObject coatingGameObject;

    public List<ProduceTask> Tasks;

    public void Start()
    {
        Icon = GetComponent<Image>();
        Coating = coatingGameObject.GetComponent<Image>();
    }


    public void UpdateValue(float value)
    {
        Coating.fillAmount = value;
    }

    public bool Completed()
    {
        return Coating.fillAmount <= 0f;
    }

    public void UpdateCount(int count)
    {
        TMPText.text = count.ToString();
    }

    // 只负责更新底图和涂层
    public void UpdateState()
    {
        ProduceTask task = Tasks.First();
        if (task == null)
        {
            Icon.material = null;
            Coating.gameObject.SetActive(false);
            TMPText.gameObject.SetActive(false);
        }
        else if (task.CurrentTaskState == ProduceTask.TaskState.Running)
        {
            Icon.material = GameManager.GameAsset.grayscale;
            Coating.gameObject.SetActive(true);
            TMPText.gameObject.SetActive(true);
        }
        else if (task.CurrentTaskState == ProduceTask.TaskState.Waiting)
        {
            Icon.material = null;
            Coating.gameObject.SetActive(false);
            TMPText.gameObject.SetActive(true);
        }
        else if (task.CurrentTaskState == ProduceTask.TaskState.Paused)
        {
            Icon.material = null;
            Coating.gameObject.SetActive(true);
            TMPText.gameObject.SetActive(true);
        }
    }
}