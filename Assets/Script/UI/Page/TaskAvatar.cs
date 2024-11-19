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
    public enum TaskAvatarState
    {
        HaveTask,
        NoTask,
    }

    // count label
    public TMP_Text TMPText;
    public Image Coating;
    public Image Icon;
    public TaskAvatarState currentState;
    [SerializeField] private GameObject coatingGameObject;

    private Material _grayscaleMaterial;
    private int _count;

    public void Start()
    {
        currentState = TaskAvatarState.NoTask;
        Icon = GetComponent<Image>();
        Coating = coatingGameObject.GetComponent<Image>();
        _grayscaleMaterial = GameManager.GameAsset.grayscale;
    }

    private void ResetTaskAvatar()
    {
        currentState = TaskAvatarState.NoTask;
        Icon.material = null;
        Coating.enabled = false;
        TMPText.enabled = false;
    }

    public void SwitchTaskAvatar(TaskAvatarState newState)
    {
        if(currentState == newState)
            return;
        if (newState == TaskAvatarState.NoTask)
        {
            ResetTaskAvatar();
        }
        else
        {
            newState = TaskAvatarState.HaveTask;
            Coating.enabled = true;
            TMPText.enabled = true;
            Coating.fillAmount = 1f;
            _count = 0;
        }
    }


    public void UpdateValue(float value)
    {
        if (currentState == TaskAvatarState.NoTask)
            return;
        Coating.fillAmount = value;
    }

    public void AddCount(int count)
    {
        if (currentState == TaskAvatarState.NoTask)
            return;
        if (TMPText.enabled == false)
            TMPText.enabled = true;
        _count += count;
        TMPText.text = _count.ToString();
    }

    public void UpdateCount(int count)
    {
        if (currentState == TaskAvatarState.NoTask || count <= 0)
            return;
        _count = count;
        if (_count <= 0)
        {
            TMPText.enabled = false;
            TMPText.text = "";
        }
        else
        {
            TMPText.enabled = true;
            TMPText.text = _count.ToString();
        }
    }
}