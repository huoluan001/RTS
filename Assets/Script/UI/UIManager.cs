using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject selectionBoxUI;
    public GameAsset gameAsset;

    public Sequence currentSequence;
    public List<Sequence> sequences;
    public GameObject sequencesParent;

    private void Start()
    {
        gameAsset.selectionBoxUI = selectionBoxUI;
        gameAsset.graphicRaycaster = GetComponent<GraphicRaycaster>();
        CreateSequence(SequenceType.MainBuildingSequence);
        // UpdateSequence();
    }

    // private void UpdateSequence()
    // {
    //     if(currentSequence == SequenceType.None)
    //     {
    //         sequence.ForEach(item => item.sprite = null);
    //     }
    //     else
    //     {

    //         for(int i = 0; i < factionSO.MainBuildings.Count; i++)
    //         {
    //             sequence[i].sprite = factionSO.MainBuildings[i].Icon;
    //         }
    //         for(int i = factionSO.MainBuildings.Count; i < sequence.Count; i++)
    //         {
    //             sequence[i].sprite = null;
    //         }
    //     }
    // }
    public void CreateSequence(SequenceType sequenceType)
    {
        GameObject sequenceGameObject = Instantiate(gameAsset.sequencePrefab);
        var sequence = sequenceGameObject.GetComponent<Sequence>();
        sequence.transform.SetParent(sequencesParent.transform, false);
        if(sequenceType == SequenceType.MainBuildingSequence)
        {
            sequence.BuildingInfos = gameAsset.factionSO.MainBuildings;
        }
        sequence.isShow = true;
    }

}
