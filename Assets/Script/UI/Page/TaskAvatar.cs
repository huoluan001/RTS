using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskAvatar : MonoBehaviour
{
    // count label
    public TMP_Text TMPText;
    public Image Coating;
    public Image Icon;
    public TaskAvatarState currentState;

    private Material _grayscaleMaterial;
    private int _count;

    public void Start()
    {
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
        if (newState == TaskAvatarState.NoTask)
        {
            ResetTaskAvatar();
        }
        else
        {
            currentState = TaskAvatarState.HaveTask;
            Coating.enabled = true;
            TMPText.enabled = true;
        }
    }


    public void UpdateValue(float value)
    {
        Coating.fillAmount = value;
    }

    public void AddCount(int count)
    {
        if (TMPText.enabled == false)
            TMPText.enabled = true;
        _count += count;
        TMPText.text = _count.ToString();
    }
}