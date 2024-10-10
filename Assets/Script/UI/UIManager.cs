using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject selectionBoxUI;
    public GameAsset gameAsset;

    public Sequence currentSequence;
    public Dictionary<SequenceType, Page> pages;


    public GameObject sequencesParent;

    private void Start()
    {
        gameAsset.selectionBoxUI = selectionBoxUI;
        gameAsset.graphicRaycaster = GetComponent<GraphicRaycaster>();
        // CreateSequence(SequenceType.MainBuildingSequence);
        // UpdateSequence();
    }

    public void CreatePage(SequenceType sequenceType)
    {
        GameObject sequenceGameObject = Instantiate(gameAsset.sequencePagePrefab);
        var page = sequenceGameObject.GetComponent<Page>();
        page.transform.SetParent(sequencesParent.transform, false);
        if(sequenceType == SequenceType.MainBuildingSequence)
        {
            page.buildingInfos = gameAsset.factionSO.MainBuildings;
        }
        else if(sequenceType == SequenceType.InfantrySequence || 
                    sequenceType == SequenceType.VehicleSequence || 
                        sequenceType == SequenceType.VehicleSequence || 
                            sequenceType == SequenceType.DockSequence) 
        {
            page.armyInfos = gameAsset.factionSO.Infantry;
        }
        page.isShow = true;
    }

    public void CreateSequence(SequenceType sequenceType)
    {
        if(!pages.ContainsKey(sequenceType))
        {
            CreatePage(sequenceType);
        }
        pages[sequenceType].AddSequence();
    }
}
